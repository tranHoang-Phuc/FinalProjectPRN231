using FptUOverflow.Infra.EfCore.Dtos.Request;
using FptUOverflow.Infra.EfCore.Dtos.Response;

namespace FptUOverflow.Api.Services.IServices
{
    public interface IProfileService
    {
        Task<ProfileResponse> GetProfileAsync();
        Task<ProfileResponse> UpdateProfileAsync(UpdateProfileRequest request);
        Task<ProfileImageResponse> UpdateProfileImageAsync(IFormFile file);
    }
}
