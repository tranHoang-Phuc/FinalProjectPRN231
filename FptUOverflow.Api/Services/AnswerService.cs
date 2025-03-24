using AutoMapper;
using FptUOverflow.Api.Services.IServices;
using FptUOverflow.Core.Exceptions;
using FptUOverflow.Infra.EfCore.DataAccess;
using FptUOverflow.Infra.EfCore.Dtos.Response;
using FptUOverflow.Infra.EfCore.Models;
using System.Security.Claims;

namespace FptUOverflow.Api.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AnswerService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AnswerResponse> ApproveAnswerAsync(Guid id, Guid answerId, string action)
        {
            var userId = GetUserId();
            var questions = await _unitOfWork.QuestionRepository.GetAllAsync(q => q.Id == id, "CreatedUser,Answers,QuestionVotes,QuestionTags.Tag");
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
            if (question!.CreatedBy != userId)
            {
                throw new AppException(ErrorCode.NotOwner);
            }
            if (action == "approve")
            {
                answer.IsApproved = true;
            }
            else
            {
                answer.IsApproved = false;
            }
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AnswerResponse>(answer);
        }

        public async Task DeleteAnswerAsync(Guid id)
        {
            var answer = await _unitOfWork.AnswerRepository.GetAllAsync(a => a.Id == id);
            if (answer.FirstOrDefault() == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            var answerVotes = await _unitOfWork.AnswerVoteRepository.GetAllAsync(v => v.AnswerId == id);
            await _unitOfWork.AnswerVoteRepository.RemoveRange(answerVotes);
            
            await _unitOfWork.AnswerRepository.DeleteAsync(answer.FirstOrDefault()!);
            await _unitOfWork.SaveChangesAsync();

        }

        public async Task<AnswerResponse> GetAnswerByIdAsync(Guid id)
        {
            var answers = await _unitOfWork.AnswerRepository.GetAllAsync(a => a.Id == id, "Question,CreatedUser,AnswerVotes");
            if (answers.FirstOrDefault() == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            return _mapper.Map<AnswerResponse>(answers.FirstOrDefault());
        }

        public async Task<AnswerResponse> VoteAnswerAsync(Guid id, string mode)
        {
            var answer = await _unitOfWork.AnswerRepository.GetAllAsync(a => a.Id == id, "AnswerVotes");
            if (answer.FirstOrDefault() == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            var userId = GetUserId();
            var answerVote = answer.FirstOrDefault()!.AnswerVotes.FirstOrDefault(v => v.CreatedBy == userId && v.AnswerId == id);
            if (answerVote == null)
            {
                var vote = new AnswerVote
                {
                    AnswerId = id,
                    CreatedBy = userId,
                    Score = "up" == mode ? 10 : -2
                };
                await _unitOfWork.AnswerVoteRepository.AddAsync(vote);
                answerVote = vote;
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                if (mode == "up")
                {
                    answerVote.Score += 10;
                }
                else
                {
                    answerVote.Score -= 2;
                }
                await _unitOfWork.SaveChangesAsync();
            }
            return _mapper.Map<AnswerResponse>(answer.FirstOrDefault());
        }

        private string GetUserId()
        {
            return _httpContextAccessor!.HttpContext!.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}
