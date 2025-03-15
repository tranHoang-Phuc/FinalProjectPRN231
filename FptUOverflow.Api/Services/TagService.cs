using AutoMapper;
using FptUOverflow.Api.Services.IServices;
using FptUOverflow.Core.Exceptions;
using FptUOverflow.Infra.EfCore.DataAccess;
using FptUOverflow.Infra.EfCore.Dtos.Response;

namespace FptUOverflow.Api.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TagListResponse> GetAllTagsAsync(string? keyword, int? pageIndex)
        {
            if (keyword == null)
            {
                keyword = "";
            }

            if(pageIndex == null)
            {
                pageIndex = 1;
            }

            var tags = await _unitOfWork.TagRepository.GetAllAsync(tag => tag.TagName.Contains(keyword), "CreatedUser,QuestionTags.Question");

            if (tags == null || !tags.Any())
            {
                throw new AppException(ErrorCode.NotFound);
            }

            var mappedTags = _mapper.Map<List<TagItemResponse>>(tags);

            var response = new TagListResponse
            {
                Tags = mappedTags.OrderByDescending(tag => tag.NumberOfQuestions).Take(16).ToList(),
                TotalPage = mappedTags.Count % 16 == 0 ? mappedTags.Count / 16 : mappedTags.Count /16 + 1,
                CurrentPage = pageIndex.Value
            };

            return response;
        }

    }
}
