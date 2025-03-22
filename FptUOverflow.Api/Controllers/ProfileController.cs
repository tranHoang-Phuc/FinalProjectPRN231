using FptUOverflow.Api.Services.IServices;
using FptUOverflow.Core.CoreObjects;
using FptUOverflow.Infra.EfCore.Dtos.Request;
using FptUOverflow.Infra.EfCore.Dtos.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FptUOverflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile() {
            var baseResponse = await _profileService.GetProfileAsync();
            var response = new BaseResponse<ProfileResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }

        [HttpPut("Image")]
        public async Task<IActionResult> UpdateProfileImage([FromForm] UpdateProfileImageRequest request)
        {
            var baseResponse = await _profileService.UpdateProfileImageAsync(request.Image);
            var response = new BaseResponse<ProfileResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var baseResponse = await _profileService.UpdateProfileAsync(request);
            var response = new BaseResponse<ProfileResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }

        [HttpGet("Authors")]
        public async Task<IActionResult> GetAuthors(int? pageIndex, string? aliasName)
        {
            var baseResponse = await _profileService.GetAuthorsAsync(pageIndex);
            var response = new BaseResponse<PagedResponse<ProfileResponse>>
            {
                Data = baseResponse
            };
            return Ok(response);
        }
        [HttpGet("author/{alias}")]
        public async Task<IActionResult> GetAuthorByAlias(string alias)
        {
            var baseResponse = await _profileService.GetAuthorByAliasAsync(alias);
            var response = new BaseResponse<ProfileResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }
    }
}
