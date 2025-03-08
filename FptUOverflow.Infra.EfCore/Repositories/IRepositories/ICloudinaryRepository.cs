using FptUOverflow.Infra.EfCore.Dtos.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Repositories.IRepositories
{
    public interface ICloudinaryRepository
    {
        Task<CloudinaryUploadResponse> UploadImage(IFormFile file, string fileName, string folderName);
        Task<bool> DeleteImage(string publicId);
    }
}
