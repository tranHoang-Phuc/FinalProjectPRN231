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
    public class ImageController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public ImageController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImageAsync([FromForm] IFormFile file)
        {
            
            var baseResponse = await _questionService.UploadImageAsync(file);
            var response = new BaseResponse<ImageUploadResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImageAsync([FromBody] DeleteImageRequest request)
        {
            await _questionService.DeleteImageAsync(request.Url);
            return NoContent();
        }
    }
}
