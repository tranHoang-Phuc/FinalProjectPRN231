using FptUOverflow.Infra.EfCore.Dtos.Response;

namespace FptUOverflow.Api.Services.IServices
{
    public interface IAnswerService
    {
        Task<AnswerResponse> ApproveAnswerAsync(Guid id, Guid answerId, string action);
        Task<AnswerResponse> GetAnswerByIdAsync(Guid id);
        Task<AnswerResponse> VoteAnswerAsync(Guid id, string mode);
    }
}
