using FptUOverflow.Api.Services.IServices;
using FptUOverflow.Core.CoreObjects;
using FptUOverflow.Infra.EfCore.Dtos.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FptUOverflow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags(string? keyword)
        {
            var baseResponse = await _tagService.GetAllTagsAsync(keyword);
            var response = new BaseResponse<List<TagItemResponse>>
            {
                Data = baseResponse,
            };
            return Ok(response);
        }
    }
}
