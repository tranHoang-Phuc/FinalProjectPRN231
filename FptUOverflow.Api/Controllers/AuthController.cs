using FptUOverflow.Api.Services.IServices;
using FptUOverflow.Core.CoreObjects;
using FptUOverflow.Infra.EfCore.Dtos.Request;
using FptUOverflow.Infra.EfCore.Dtos.Response;
using Microsoft.AspNetCore.Mvc;

namespace FptUOverflow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] ExchangeAccessToken request)
        {
            var baseResponse = await _authenticationService.OutboundAuthenticatedAsync(request.Token);
            var response = new BaseResponse<LoginResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }

        [HttpPost("Introspect")]
        public async Task<IActionResult> Introspect()
        {
            var baseResponse = await _authenticationService.IntrospectAsync();
            var response = new BaseResponse<IntrospectResponse>
            {
                Data = baseResponse
            };
            return Ok(response);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello");
        }
    }
}
