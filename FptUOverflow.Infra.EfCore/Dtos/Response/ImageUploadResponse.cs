using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Dtos.Response
{
    public class ImageUploadResponse
    {
        public string Url { get; set; }
        public string PublicId { get; set; }
    }
}
