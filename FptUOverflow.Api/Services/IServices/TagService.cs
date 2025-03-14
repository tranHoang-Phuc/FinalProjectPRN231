using AutoMapper;
using FptUOverflow.Core.Exceptions;
using FptUOverflow.Infra.EfCore.DataAccess;
using FptUOverflow.Infra.EfCore.Dtos.Response;

namespace FptUOverflow.Api.Services.IServices
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

        public async Task<List<TagItemResponse>> GetAllTagsAsync(string? keyword)
        {
            if (keyword == null)
            {
                keyword = "";
            }

            var tags = await _unitOfWork.TagRepository.GetAllAsync(tag => tag.TagName.Contains(keyword), "CreatedUser,QuestionTags.Question");

            if (tags == null || !tags.Any())
            {
                throw new AppException(ErrorCode.NotFound);
            }

            var mappedTags = _mapper.Map<List<TagItemResponse>>(tags);

            return mappedTags.OrderByDescending(tag => tag.NumberOfQuestions).Take(16).ToList();
        }

    }
}
