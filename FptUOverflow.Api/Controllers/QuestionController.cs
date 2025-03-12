using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using FptUOverflow.Api.Services.IServices;
using FptUOverflow.Core.CoreObjects;
using FptUOverflow.Infra.EfCore.Dtos.Request;
using FptUOverflow.Infra.EfCore.Dtos.Response;
using FptUOverflow.Infra.EfCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FptUOverflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions(
            [FromQuery] int? pageIndex,
            [FromQuery] string? filter,
            [FromQuery] string? tags,
            [FromQuery] string? order,
            [FromQuery] string? search,
            [FromQuery] int pageSize
           )
        {
            var filterList = !string.IsNullOrEmpty(filter) ? filter.Split(',').ToArray() : null;
            var tagList = !string.IsNullOrEmpty(tags) ? tags.Split(',').ToArray() : null;
            var baseResponse = await _questionService.GetQuestionsAsync(pageIndex, filterList, tagList, order, search, pageSize);
            var response = new BaseResponse<QuestionResponseList>
            {
                Data = baseResponse
            };
            return Ok(response);
        }
        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionById([FromRoute] Guid id)
        {
            var baseResponse = await _questionService.GetQuestionByIdAsync(id);
            var response = new BaseResponse<QuestionResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromBody] CreateQuestionRequest request)
        {
            var baseResponse = await _questionService.CreateQuestionAsync(request);
            return CreatedAtAction(nameof(GetQuestionById), new { baseResponse.Id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion([FromRoute] Guid id, [FromBody] UpdateQuestionRequest request)
        {
            var baseResponse = await _questionService.UpdateQuestionAsync(id, request);
            var response = new BaseResponse<QuestionResponse>
            {
                Data = baseResponse
            };
            return Ok(baseResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] Guid id) {
            await _questionService.DeleteQuestionAsync(id);
            return NoContent();
        }
        [HttpPut("{id}/{mode}")]
        public async Task<IActionResult> VoteForQuestion([FromRoute] Guid id, [FromRoute] string mode)
        {
            var baseResponse = await _questionService.VoteForQuestionAsync(id, mode);
            var response = new BaseResponse<QuestionResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }   

        [HttpPost("{id}/answers")]
        public async Task<IActionResult> CreateAnswer([FromRoute] Guid id, [FromBody]CreationAnswer answer )
        {
            var baseResponse = await _questionService.CreateAnswerAsync(id, answer);
            var response = new BaseResponse<AnswerResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }

        [HttpPut("{id}/answers/{answerId}")]
        public async Task<IActionResult> UpdateAnswer([FromRoute] Guid id, [FromRoute] Guid answerId, [FromBody] UpdateAnswerRequest request)
        {
            var baseResponse = await _questionService.UpdateAnswerAsync(id, answerId, request);
            var response = new BaseResponse<AnswerResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }

        [HttpDelete("{id}/answers/{answerId}")]
        public async Task<IActionResult> DeleteAnswer([FromRoute] Guid id, [FromRoute] Guid answerId)
        {
            await _questionService.DeleteAnswerAsync(id, answerId);
            return NoContent();
        }

        
    }
}
