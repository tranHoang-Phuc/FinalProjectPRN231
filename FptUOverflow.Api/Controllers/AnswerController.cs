using FptUOverflow.Api.Services;
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
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpPut("{id}/answers/{answerId}/{accept}")]
        public async Task<IActionResult> ApproveAnswer([FromRoute] Guid id, [FromRoute] Guid answerId, [FromRoute] string accept)
        {
            var baseResponse = await _answerService.ApproveAnswerAsync(id, answerId, accept);
            var response = new BaseResponse<AnswerResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }

        [HttpPost("{id}/{mode}")]
        public async Task<IActionResult> VoteAnswer([FromRoute] Guid id, [FromRoute] string mode)
        {
            var baseResponse = await _answerService.VoteAnswerAsync(id, mode);
            var response = new BaseResponse<AnswerResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var baseResponse = await _answerService.GetAnswerByIdAsync(id);
            var response = new BaseResponse<AnswerResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer([FromRoute] Guid id)
        {
            await _answerService.DeleteAnswerAsync(id);
            return NoContent();
        }

        
    }
}
