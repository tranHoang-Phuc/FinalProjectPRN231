using FptUOverflow.Api.Services;
using FptUOverflow.Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace FptUOverflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SSEController : ControllerBase
    {
        private readonly ISSEService _sseService;

        public SSEController(ISSEService sseService)
        {
            _sseService = sseService;
        }

        [HttpGet("events/{userId}")]
        public async Task GetEvents(Guid userId)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");
            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            var writer = new StreamWriter(Response.Body, Encoding.UTF8, leaveOpen: true);
            var channelReader = _sseService.RegisterUser(userId);

            try
            {
                await foreach (var message in channelReader.ReadAllAsync(HttpContext.RequestAborted))
                {
                    await writer.WriteAsync($"data: {message}\n\n");
                    await writer.FlushAsync();
                }
            }
            finally
            {
                _sseService.RemoveUser(userId);
            }
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromQuery] Guid userId, [FromBody] string message)
        {
            _sseService.SendEventToUser(userId, message);
            return Ok();
        }
    }
}
