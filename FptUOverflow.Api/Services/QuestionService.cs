using AutoMapper;
using FptUOverflow.Api.Services.IServices;
using FptUOverflow.Core.Exceptions;
using FptUOverflow.Infra.EfCore.DataAccess;
using FptUOverflow.Infra.EfCore.Dtos.Request;
using FptUOverflow.Infra.EfCore.Dtos.Response;
using FptUOverflow.Infra.EfCore.Models;
using FptUOverflow.Infra.EfCore.Repositories.IRepositories;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace FptUOverflow.Api.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        
        public QuestionService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        

        public async Task<AnswerResponse> CreateAnswerAsync(Guid id, CreationAnswer answer)
        {
            var userId = GetUserId();
            var questions = await _unitOfWork.QuestionRepository.GetAllAsync(q => q.Id == id, "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            if (questions.FirstOrDefault() == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            var question = questions.FirstOrDefault();
            //if(question!.CreatedBy == userId)
            //{
            //    throw new AppException(ErrorCode.SelfAnswer);
            //}
            var newAnswer = new Answer
            {
                Id = Guid.NewGuid(),
                QuestionId = question!.Id,
                Content = answer.Content,
                CreatedBy = userId
            };
            await _unitOfWork.AnswerRepository.AddAsync(newAnswer);
            await _unitOfWork.SaveChangesAsync();
            var response = await _unitOfWork.QuestionRepository
                .GetAllAsync(q => q.Id == question.Id,
                "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            var answerResponse = response.FirstOrDefault()!.Answers.FirstOrDefault(a => a.Id == newAnswer.Id);
            return _mapper.Map<AnswerResponse>(answerResponse);
        }

        public async Task<QuestionResponse> CreateQuestionAsync(CreateQuestionRequest request)
        {
            var userId = GetUserId();
            var question = _mapper.Map<Question>(request);
            question.Id = Guid.NewGuid();
            question.CreatedBy = userId;
            var tags = request.Tags.Select(t => t.ToLower()).ToList();
            if (tags.Count() > 5)
            {
                throw new AppException(ErrorCode.OutOfSlot);
            }
            var existingTags = await _unitOfWork.TagRepository.GetAllAsync(
                t => tags.Contains(t.TagName));
            var newTags = tags.Except(existingTags.Select(t => t.TagName.ToLower()))
                .Select(tagName => new Tag
                {
                    Id = Guid.NewGuid(),
                    TagName = tagName,
                    CreatedBy = userId
                }).ToList();

            await _unitOfWork.TagRepository.AddRangeAsync(newTags);
            await _unitOfWork.QuestionRepository.AddAsync(question);
            await _unitOfWork.SaveChangesAsync();
            List<Tag> allTags = new List<Tag>();
            if (existingTags.Count() == newTags.Count())
            {
                allTags = newTags;
            } else
            {
                allTags = (await existingTags.ToListAsync()).Concat(newTags).ToList();
            }


            var questionTags = allTags.Select(t => new QuestionTag
            {
                QuestionId = question.Id,
                TagId = t.Id
            }).ToList();
            await _unitOfWork.QuestionTagRepository.AddRangeAsync(questionTags);
            await _unitOfWork.SaveChangesAsync();
            var response = await _unitOfWork.QuestionRepository
                .GetAllAsync(q => q.Id == question.Id, 
                "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            return _mapper.Map<QuestionResponse>(response.FirstOrDefault());
        }

        public async Task DeleteAnswerAsync(Guid id, Guid answerId)
        {
            var userId = GetUserId();
            var questions = await _unitOfWork.QuestionRepository.GetAllAsync(q => q.Id == id, "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            var answers = await _unitOfWork.AnswerRepository.GetAllAsync(a => a.Id == answerId, "CreatedUser");
            if (questions.FirstOrDefault() == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            if (answers.FirstOrDefault() == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            var question = questions.FirstOrDefault();
            var answer = answers.FirstOrDefault();
            if (userId != question!.CreatedBy)
            {
                throw new AppException(ErrorCode.NotOwner);
            }
            if(userId != answer!.CreatedBy)
            {
                throw new AppException(ErrorCode.NotOwner);
            }
            await _unitOfWork.AnswerRepository.DeleteAsync(answer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteImageAsync(string url)
        {
            var image = await _unitOfWork.ImageUploadRepository.GetAllAsync(i => i.Url == url);
            if (image.FirstOrDefault() == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            await _unitOfWork.CloudinaryRepository.DeleteImage(image.FirstOrDefault().PublicId);
            await _unitOfWork.ImageUploadRepository.DeleteAsync(image.FirstOrDefault());
            await _unitOfWork.SaveChangesAsync();
        }

        private List<string> GetImageUrl(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var urls = doc.DocumentNode.Descendants("img")
                .Select(e => e.GetAttributeValue("src", null))
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();
            return urls;
        }
        public async Task DeleteQuestionAsync(Guid id)
        {
            var questions = await _unitOfWork.QuestionRepository.GetAllAsync(q => q.Id == id, "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            if (questions.FirstOrDefault() == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            var question = questions.FirstOrDefault();
            var problemImageUrl = GetImageUrl(question!.DetailProblem);
            foreach (var url in problemImageUrl)
            {
                await DeleteImageAsync(url);
            }
            var expectingImageUrl = GetImageUrl(question!.Expecting!);
            foreach (var url in expectingImageUrl)
            {
                await DeleteImageAsync(url);
            }
            var answers = await _unitOfWork.AnswerRepository.GetAllAsync(a => a.QuestionId == question!.Id, "AnswerVotes");
            if (answers.FirstOrDefault() != null)
            {
                foreach(var answer in answers)
                {
                    foreach (var vote in answer.AnswerVotes)
                    {
                 
                        await _unitOfWork.AnswerVoteRepository.DeleteAsync(vote);
                    }
                    var answerImageUrl = GetImageUrl(answer.Content);
                    foreach (var url in answerImageUrl)
                    {
                        await DeleteImageAsync(url);
                    }
                    await _unitOfWork.AnswerRepository.DeleteAsync(answer);
                }
            }
            var questionTags = await _unitOfWork.QuestionTagRepository.GetAllAsync(qt => qt.QuestionId == question!.Id);
            if (questionTags.FirstOrDefault() != null)
            {
                foreach (var qt in questionTags)
                {
                    await _unitOfWork.QuestionTagRepository.DeleteAsync(qt);
                }
            }

            var questionVotes = await _unitOfWork.QuestionVoteRepository.GetAllAsync(qv => qv.QuestionId == question!.Id);
            if (questionVotes.FirstOrDefault() != null)
            {
                foreach (var qv in questionVotes)
                {
                    await _unitOfWork.QuestionVoteRepository.DeleteAsync(qv);
                }
            }

            await _unitOfWork.QuestionRepository.DeleteAsync(question);
            await _unitOfWork.SaveChangesAsync();
        }
        private string Test()
        {
            return "a";
        }
        public async Task<QuestionResponse> GetQuestionByIdAsync(Guid id)
        {
            var questions = await _unitOfWork.QuestionRepository
                .GetAllAsync(q => q.Id == id,
                "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            if (questions.FirstOrDefault() == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            questions!.FirstOrDefault()!.Views++;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<QuestionResponse>(questions.FirstOrDefault());
        }

        public async Task<QuestionResponseList> GetQuestionsAsync(int? pageIndex, string[]? filters, string[]? tags, string? order, string? search, int pageSize)
        {
            if(pageIndex == null)
            {
                pageIndex = 1;
            }

            var questions = await _unitOfWork.QuestionRepository.GetAllAsync(null, "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if(filter == "no-answers")
                    {
                        questions = questions.Where(q => q.Answers.Count() == 0);
                    } 
                    else if (filter == "no-accepted-answer")
                    {
                        questions = questions.Where(q => !q.Answers.Any(a => a.IsApproved));
                    } 
                    else
                    {
                        int daysOld = Convert.ToInt32(filter);
                        questions = questions.Where(q => q.CreatedAt >= DateTime.Now.AddDays(-daysOld));
                    }

                }
            }
            if (tags != null )
            {
                questions = questions.Where(q => q.QuestionTags.Any(qt => tags.Contains(qt.Tag.TagName)));
            }

            if(order != null)
            {
                if (order == "newest")
                {
                    questions = questions.OrderByDescending(q => q.CreatedAt);
                   
                } 
                else if (order == "oldest")
                {
                    questions = questions.OrderBy(q => q.CreatedAt);
                }
                else if(order == "views")
                {
                    questions = questions.OrderByDescending(q => q.Views);
                }
                else
                {
                    questions = questions.OrderByDescending(q => q.QuestionVotes.Sum(v => v.Score));
                }
            }
            if (search != null)
            {
                if (search.Contains("isaccepted:yes"))
                {
                    questions = questions.Where(q => q.Answers.Any(a => a.IsApproved));
                }
                else if (search.Contains("isaccepted:no"))
                {
                    questions = questions.Where(q => !q.Answers.Any(a => a.IsApproved));
                }
                if (search.Contains("answers:0"))
                {
                    questions = questions.Where(q => q.Answers.Count() == 0);
                }
                
                MatchCollection matches = Regex.Matches(search, @"""([^""]+)""");
                if (matches.Count() > 0)
                {
                    questions = questions.Where(q =>
                        matches.Cast<Match>().Any(m => q.Title.Contains(m.Groups[1].Value)));
                }

                MatchCollection matchesAnswer = Regex.Matches(search, @"score:\s*(\d+)");
                if (matchesAnswer.Count > 0)
                {
                    var score = Convert.ToInt32(matchesAnswer[0].Groups[1].Value);
                    questions = questions.Where(q => q.QuestionVotes.Count >= score);
                }

                MatchCollection matchesUser = Regex.Matches(search, @"user:[a-zA-Z0-9_-]+$");
                if (matchesUser.Count > 0)
                {
                    var user = matchesUser[0].Value.Split(":")[1];
                    questions = questions.Where(q => q.CreatedUser.Email.Contains(user));
                }
                Match match = Regex.Match(search, @"^\[tag\]\s+(.+)$");
                if (match.Success)
                {
                    string tagsSearch = match.Groups[1].Value;
                    string[] tagList = tagsSearch.Split(' ');
                    foreach (var tag in tagList)
                    {
                        questions = questions.Where(q => q.QuestionTags.Any(qt => qt.Tag.TagName.Contains(tag)));
                    }
                }
            }
            int count = questions.Count();
            questions = questions.Skip((pageIndex.Value - 1) * pageSize).Take(pageSize);
            return new QuestionResponseList {
                Questions = _mapper.Map<List<QuestionResponse>>(questions.ToList()),
                Total = count
            };
        }

        public async Task<QuestionCount> GetQuestionsCountAsync(int? pageIndex, string[]? filters, string[]? tags, string? order, string? search)
        {
            if (pageIndex == null)
            {
                pageIndex = 1;
            }

            var questions = await _unitOfWork.QuestionRepository.GetAllAsync(null, "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            if (filters!.Count() > 0)
            {
                foreach (var filter in filters)
                {
                    if (filter == "no-answers")
                    {
                        questions = questions.Where(q => q.Answers.Count() == 0);
                    }
                    else if (filter == "no-accepted-answer")
                    {
                        questions = questions.Where(q => !q.Answers.Any(a => a.IsApproved));
                    }
                    else
                    {
                        int daysOld = Convert.ToInt32(filter);
                        questions = questions.Where(q => q.CreatedAt >= DateTime.Now.AddDays(-daysOld));
                    }

                }
            }
            if (tags!.Count() > 0)
            {
                questions = questions.Where(q => q.QuestionTags.Any(qt => tags.Contains(qt.Tag.TagName)));
            }

            if (order != null)
            {
                if (order == "newest")
                {
                    questions = questions.OrderByDescending(q => q.CreatedAt);

                }
                else if (order == "oldest")
                {
                    questions = questions.OrderBy(q => q.CreatedAt);
                }
                else if (order == "views")
                {
                    questions = questions.OrderByDescending(q => q.Views);
                }
                else
                {
                    questions = questions.OrderByDescending(q => q.QuestionVotes.Sum(v => v.Score));
                }
            }
            if (search != null)
            {
                if (search.Contains("isaccepted:yes"))
                {
                    questions = questions.Where(q => q.Answers.Any(a => a.IsApproved));
                }
                else if (search.Contains("isaccepted:no"))
                {
                    questions = questions.Where(q => !q.Answers.Any(a => a.IsApproved));
                }
                if (search.Contains("answers:0"))
                {
                    questions = questions.Where(q => q.Answers.Count() == 0);
                }

                MatchCollection matches = Regex.Matches(search, @"""([^""]+)""");
                if (matches.Count() > 0)
                {
                    questions = questions.Where(q =>
                        matches.Cast<Match>().Any(m => q.Title.Contains(m.Groups[1].Value)));
                }

                MatchCollection matchesAnswer = Regex.Matches(search, @"score:\s*(\d+)");
                if (matchesAnswer.Count > 0)
                {
                    var score = Convert.ToInt32(matchesAnswer[0].Groups[1].Value);
                    questions = questions.Where(q => q.QuestionVotes.Count >= score);
                }

                MatchCollection matchesUser = Regex.Matches(search, @"user:[a-zA-Z0-9_-]+$");
                if (matchesUser.Count > 0)
                {
                    var user = matchesUser[0].Value.Split(":")[1];
                    questions = questions.Where(q => q.CreatedUser.Email.Contains(user));
                }
                Match match = Regex.Match(search, @"^\[tag\]\s+(.+)$");
                if (match.Success)
                {
                    string tagsSearch = match.Groups[1].Value;
                    string[] tagList = tagsSearch.Split(' ');
                    foreach (var tag in tagList)
                    {
                        questions = questions.Where(q => q.QuestionTags.Any(qt => qt.Tag.TagName.Contains(tag)));
                    }
                }
            }
            return new QuestionCount
            {
                Count = questions.Count()
            };
        }

        public async Task<AnswerResponse> UpdateAnswerAsync(Guid id, Guid answerId, UpdateAnswerRequest request)
        {
            var questions = await _unitOfWork.QuestionRepository
                .GetAllAsync(q => q.Id == id, "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            if (questions.FirstOrDefault() == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            var question = questions.FirstOrDefault();
            var answer = question!.Answers.FirstOrDefault(a => a.Id == answerId);
            if (answer == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            answer.Content = request.Content;
            answer.UpdatedAt = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AnswerResponse>(answer);
        }

        public async Task<QuestionResponse> UpdateQuestionAsync(Guid id, UpdateQuestionRequest request)
        {
            var userId = GetUserId();         
            var questions = await _unitOfWork.QuestionRepository
                .GetAllAsync(q => q.Id == id, "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            if (questions.FirstOrDefault() == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            var question = questions.FirstOrDefault();
            if(userId != question!.CreatedBy)
            {
                throw new AppException(ErrorCode.NotOwner);
            }
            var questionTags  = await _unitOfWork.QuestionTagRepository.GetAllAsync(qt => qt.QuestionId == question.Id);

            foreach (var questionTag in questionTags)
            {
                await _unitOfWork.QuestionTagRepository.DeleteAsync(questionTag);
            }
            await _unitOfWork.SaveChangesAsync();
            question.Title = request.Title;
            question.DetailProblem = request.DetailProblem;
            question.Expecting = request.Expecting;
            question.UpdatedAt = DateTime.Now;

            var existingTags = await _unitOfWork.TagRepository.GetAllAsync(
                t => request.Tags.Select(tag => tag.ToLower()).Contains(t.TagName.ToLower()));

            var existingTagNames = new HashSet<string>(existingTags.Select(t => t.TagName.ToLower()));

            var newTags = request.Tags
                .Select(tag => tag.ToLower()) 
                .Except(existingTagNames)
                .Select(tagName => new Tag
                {
                    Id = Guid.NewGuid(),
                    TagName = tagName,
                    CreatedBy = userId
                }).ToList();

            var addedNewTags = await _unitOfWork.TagRepository.GetAllAsync(t =>
                !newTags.Any(nt => t.TagName.ToLower().Contains(nt.TagName.ToLower())));


            await _unitOfWork.TagRepository.AddRangeAsync(newTags);
            await _unitOfWork.SaveChangesAsync();
            List<Tag> allTags = new List<Tag>();
            if (existingTags.Count() == newTags.Count())
            {
                allTags = newTags;
            }
            else
            {
                allTags = (await existingTags.ToListAsync()).Concat(newTags).ToList();
            }


            var questionTagsAdd = allTags.Select(t => new QuestionTag
            {
                QuestionId = question.Id,
                TagId = t.Id
            }).ToList();

           
            //}).ToList();
            await _unitOfWork.QuestionTagRepository.AddRangeAsync(questionTagsAdd);
            await _unitOfWork.SaveChangesAsync();
            var response = await _unitOfWork.QuestionRepository
                .GetAllAsync(q => q.Id == question.Id,
                "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            return _mapper.Map<QuestionResponse>(response.FirstOrDefault());
        }

        public async Task<ImageUploadResponse> UploadImageAsync(IFormFile file)
        {
            var uploadResult = await _unitOfWork.CloudinaryRepository.UploadImage(file, Guid.NewGuid().ToString(), "FinalPRN");
            var imageUpload = new ImageUpload
            {
                Id = Guid.NewGuid(),
                PublicId = uploadResult.PublicId,
                Url = uploadResult.Url.ToString(),
            };
            await _unitOfWork.ImageUploadRepository.AddAsync(imageUpload);
            await _unitOfWork.SaveChangesAsync();

            return new ImageUploadResponse
            {
                Url = uploadResult.Url.ToString(),
                PublicId = uploadResult.PublicId
            };
        }

        public async Task<QuestionResponse> VoteForQuestionAsync(Guid id, string mode)
        {
            var questions = await _unitOfWork.QuestionRepository.GetAllAsync(q => q.Id == id, "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            if (questions.FirstOrDefault() == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            var question = questions.FirstOrDefault();
            var userId = GetUserId();
            var existingVote = await _unitOfWork.QuestionVoteRepository.GetAllAsync(qv => qv.QuestionId == question!.Id && qv.CreatedBy == userId);
            if (existingVote.Count() != 0)
            {
                var voted = existingVote.FirstOrDefault();
                if (mode == "up" && voted.Score == 10)
                {
                    throw new AppException(ErrorCode.AlreadyVoted);
                }
                if (mode == "down" && voted.Score == -2)
                {
                    throw new AppException(ErrorCode.AlreadyVoted);
                }
                voted!.Score = mode == "up" ? 10 : -2;
                await _unitOfWork.SaveChangesAsync();
                var responseData = await _unitOfWork.QuestionRepository
                    .GetAllAsync(q => q.Id == question.Id,
                    "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
                return _mapper.Map<QuestionResponse>(responseData.FirstOrDefault());

            }
            var vote = new QuestionVote
            {
                QuestionId = question!.Id,
                CreatedBy = userId,
                Score = mode == "up" ? 10 : -2
            };
            await _unitOfWork.QuestionVoteRepository.AddAsync(vote);
            await _unitOfWork.SaveChangesAsync();
            var response = await _unitOfWork.QuestionRepository
                .GetAllAsync(q => q.Id == question.Id,
                "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
            return _mapper.Map<QuestionResponse>(response.FirstOrDefault());
           
        }

        private string GetUserId()
        {
            return _httpContextAccessor!.HttpContext!.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }

        public async Task<QuestionResponseList> GetAskedQuestion(string? aliasName)
        {
            var userId = GetUserId();
            if (aliasName == null)
            {
                var questions = await _unitOfWork.QuestionRepository
                    .GetAllAsync(q => q.CreatedBy == userId, "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
                return new QuestionResponseList
                {
                    Questions = _mapper.Map<List<QuestionResponse>>(questions.ToList()),
                    Total = questions.Count()
                };
            } 
            else
            {
                var questions = await _unitOfWork.QuestionRepository
                    .GetAllAsync(q => q.CreatedUser.Email.Contains(aliasName), "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
                return new QuestionResponseList
                {
                    Questions = _mapper.Map<List<QuestionResponse>>(questions.ToList()),
                    Total = questions.Count()
                };
            }
        }
    }
}
