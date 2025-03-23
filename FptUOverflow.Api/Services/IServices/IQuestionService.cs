
using FptUOverflow.Infra.EfCore.Dtos.Request;
using FptUOverflow.Infra.EfCore.Dtos.Response;

namespace FptUOverflow.Api.Services.IServices
{
    public interface IQuestionService
    {
        Task<AnswerResponse> CreateAnswerAsync(Guid id, CreationAnswer answer);
        Task<QuestionResponse> CreateQuestionAsync(CreateQuestionRequest request);
        Task DeleteAnswerAsync(Guid id, Guid answerId);
        Task DeleteImageAsync(string url);
        Task DeleteQuestionAsync(Guid id);
        Task<QuestionResponseList> GetAskedQuestion(string? aliasName);
        Task<QuestionResponse> GetQuestionByIdAsync(Guid id);
        Task<QuestionResponseList> GetQuestionsAsync(int? pageIndex, string[]? filter, string[]? tags, string? order, string? search, int pageSize);
        Task<QuestionCount> GetQuestionsCountAsync(int? pageIndex, string[]? filter, string[]? tags, string? order, string? search);
        Task<AnswerResponse> UpdateAnswerAsync(Guid id, Guid answerId, UpdateAnswerRequest request);
        Task<QuestionResponse> UpdateQuestionAsync(Guid id, UpdateQuestionRequest request);
        Task<ImageUploadResponse> UploadImageAsync(IFormFile file);
        Task<QuestionResponse> VoteForQuestionAsync(Guid id, string mode);
    }
}
