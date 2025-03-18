using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Dtos.Request
{
    public class UpdateProfileImageRequest
    {
        public IFormFile Image { get; set; }
    }
}
