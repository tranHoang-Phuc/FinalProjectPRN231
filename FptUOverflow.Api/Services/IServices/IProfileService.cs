using FptUOverflow.Core.CoreObjects;
using FptUOverflow.Infra.EfCore.Dtos.Request;
using FptUOverflow.Infra.EfCore.Dtos.Response;

namespace FptUOverflow.Api.Services.IServices
{
    public interface IProfileService
    {
        Task<ProfileResponse> GetAuthorByAliasAsync(string alias);
        Task<PagedResponse<ProfileResponse>> GetAuthorsAsync(int? pageIndex);
        Task<ProfileResponse> GetProfileAsync();
        Task<ProfileResponse> UpdateProfileAsync(UpdateProfileRequest request);
        Task<ProfileResponse> UpdateProfileImageAsync(IFormFile file);
    }
}
