using FptUOverflow.Api.Services.IServices;
using FptUOverflow.Core.CoreObjects;
using FptUOverflow.Core.Exceptions;
using FptUOverflow.Infra.EfCore.DataAccess;
using FptUOverflow.Infra.EfCore.Dtos.Response;
using FptUOverflow.Infra.EfCore.Models;
using FptUOverflow.Infra.EfCore.Repositories;
using FptUOverflow.Infra.EfCore.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FptUOverflow.Api.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        private readonly OAuth2 _oAuth2;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(IOptions<OAuth2> oAuth2, IHttpClientFactory httpClientFactory,
            IConfiguration configuration, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _oAuth2 = oAuth2.Value;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IntrospectResponse> IntrospectAsync()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                throw new AppException(ErrorCode.InvalidToken);
            }
            return new IntrospectResponse
            {
                IsValid = await _jwtTokenGenerator.IsTokenValidAsync(token)
            };
        }

        public async Task<LoginResponse> OutboundAuthenticatedAsync(string code)
        {
            using ( var client = _httpClientFactory.CreateClient())
            {
                var tokenUrl = _configuration["GoogleAuth:TokenUrl"];
                var formContent = new Dictionary<string, string>
                {                    
                    {"code", code},
                    {"client_id", _oAuth2.ClientId},
                    {"client_secret", _oAuth2.ClientSecret},
                    {"redirect_uri", _oAuth2.RedirectUri},
                    {"grant_type", _oAuth2.GrantType}
                };
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, tokenUrl)
                {
                    Content = new FormUrlEncodedContent(formContent)
                };
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));



                var response = await client.SendAsync(httpRequest);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new AppException(ErrorCode.Unauthorized);
                }
                var content = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<ExchangeTokenResponse>(content);

                var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, _configuration["GoogleAuth:InfoUrl"]);
                userInfoRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", responseData.AccessToken);
                var userInfoResponse = await client.SendAsync(userInfoRequest);
                if (!userInfoResponse.IsSuccessStatusCode)
                {
                    throw new AppException(ErrorCode.Unauthorized);
                }
                var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync();
                var userInfo = JsonConvert.DeserializeObject<GoogleUserInfo>(userInfoContent);
                var users = await _unitOfWork.ApplicationUserRepository.FindAsync(x => x.Email == userInfo!.Email);
                if (users.Count() == 0)
                {
                    // Onboard User:
                    var newUser = new ApplicationUser
                    {
                        DisplayName = userInfo.Name,
                        Email = userInfo.Email,
                        UserName = userInfo.Email,
                        ProfileImage = userInfo.Picture,
                        NormalizedEmail = userInfo.Email.ToUpper(),
                        PhoneNumber = "0000000000"
                    };
                    var result = await _userManager.CreateAsync(newUser);
                    await _userManager.AddToRoleAsync(newUser, "USER");
                    await _unitOfWork.SaveChangesAsync();
                }
                
                var roles = await _userManager.GetRolesAsync(users!.First());
                var token = await _jwtTokenGenerator.GenerateToken(users!.First()!, roles);
                return new LoginResponse
                {
                    AccessToken = token,
                    ExpiresIn = 3600,
                };
            }
        }
    }
}

