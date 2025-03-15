using FptUOverflow.Infra.EfCore.Dtos.Response;

namespace FptUOverflow.Api.Services.IServices
{
    public interface ITagService
    {
        public Task<TagListResponse> GetAllTagsAsync(string? keyword, int? pageIndex);

    }
}
