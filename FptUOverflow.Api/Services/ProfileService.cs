using AutoMapper;
using FptUOverflow.Api.Services.IServices;
using FptUOverflow.Core.CoreObjects;
using FptUOverflow.Core.Exceptions;
using FptUOverflow.Infra.EfCore.DataAccess;
using FptUOverflow.Infra.EfCore.Dtos.Request;
using FptUOverflow.Infra.EfCore.Dtos.Response;
using FptUOverflow.Infra.EfCore.Models;
using System.Security.Claims;

namespace FptUOverflow.Api.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public ProfileService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<ProfileResponse> GetAuthorByAliasAsync(string alias)
        {
            var user = (await _unitOfWork.ApplicationUserRepository.GetAllAsync(u => u.Email.StartsWith(alias + "@"))).FirstOrDefault();
            if (user == null)
            {
                throw new AppException(ErrorCode.NotFound);
            }
            return _mapper.Map<ProfileResponse>(user);
        }

        public async Task<PagedResponse<ProfileResponse>> GetAuthorsAsync(int? pageIndex)
        {
            if(pageIndex == null)
            {
                pageIndex = 1;
            }
            var users = await _unitOfWork.ApplicationUserRepository.GetAllAsync(null, "QuestionVotes");
            int totalPage = users.Count() % 16 ==0 ? users.Count() / 16 : users.Count() / 16 + 1;
            users = users.Skip((pageIndex.Value - 1) * 16).Take(16);
            return new PagedResponse<ProfileResponse>
            {
                Data = _mapper.Map<List<ProfileResponse>>(users),
                PageIndex = pageIndex.Value,              
                TotalPage = totalPage
            };
        }

        public async Task<ProfileResponse> GetProfileAsync()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                throw new AppException(ErrorCode.Unauthorized);
            }
            var user = await _unitOfWork.ApplicationUserRepository.GetAllAsync(u => u.Id == userId);
            return _mapper.Map<ProfileResponse>(user.FirstOrDefault());
        }

        public async Task<ProfileResponse> UpdateProfileAsync(UpdateProfileRequest request)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                throw new AppException(ErrorCode.Unauthorized);
            }
            var user = (await _unitOfWork.ApplicationUserRepository.GetAllAsync(u => u.Id == userId)).FirstOrDefault();
            user!.DisplayName = request.DisplayName;
            user!.Location = request.Location;
            user!.Title = request.Title;
            user!.AboutMe = request.AboutMe;
            await _unitOfWork.ApplicationUserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ProfileResponse>(user);
        }

        public async Task<ProfileResponse> UpdateProfileImageAsync(IFormFile file)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                throw new AppException(ErrorCode.Unauthorized);
            }
            var uploadResult = await _unitOfWork.CloudinaryRepository.UploadImage(file, Guid.NewGuid().ToString(), "FinalPRN");
           
            var user = (await _unitOfWork.ApplicationUserRepository.GetAllAsync(u => u.Id == userId)).FirstOrDefault();

            var currentImage = (await _unitOfWork.ImageUploadRepository.GetAllAsync(i => i.Url == user.ProfileImage)).FirstOrDefault();

            await _unitOfWork.CloudinaryRepository.DeleteImage(currentImage!.PublicId);

            user!.ProfileImage = uploadResult.Url;
            var imageUpload = new ImageUpload
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.Url,

            };
            return _mapper.Map<ProfileResponse>(user);

        }

        private string GetUserId()
        {
            return _httpContextAccessor!.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);  
        }
    }
}
