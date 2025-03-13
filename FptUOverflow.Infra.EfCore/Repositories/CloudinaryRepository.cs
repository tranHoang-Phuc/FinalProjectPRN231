using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FptUOverflow.Core.Exceptions;
using FptUOverflow.Infra.EfCore.Dtos.Response;
using FptUOverflow.Infra.EfCore.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Repositories
{
    public class CloudinaryRepository : ICloudinaryRepository
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryRepository(IServiceProvider serviceProvider)
        {
            _cloudinary = serviceProvider.GetRequiredService<Cloudinary>();
        }
        public async Task<bool> DeleteImage(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result.Result == "ok";
        }

        public async Task<CloudinaryUploadResponse> UploadImage(IFormFile file, string fileName, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new AppException(ErrorCode.FileNotFound);
            }         
            // Check image file size
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                Transformation = new Transformation().Quality("auto").FetchFormat("auto"),
                Folder = folderName
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new AppException(ErrorCode.UploadFailed);
            }
            return new CloudinaryUploadResponse
            {
                Url = uploadResult.SecureUrl.AbsoluteUri,
                PublicId = uploadResult.PublicId
            };
        }

        
    }
}
