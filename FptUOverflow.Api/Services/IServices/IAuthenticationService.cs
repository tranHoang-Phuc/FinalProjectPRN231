using FptUOverflow.Infra.EfCore.Dtos.Response;

namespace FptUOverflow.Api.Services.IServices
{
    public interface IAuthenticationService
    {
        Task<LoginResponse> OutboundAuthenticatedAsync(string code);
        Task<IntrospectResponse> IntrospectAsync();
    }
}
