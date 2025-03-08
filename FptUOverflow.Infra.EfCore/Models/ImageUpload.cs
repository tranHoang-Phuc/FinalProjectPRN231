using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Models
{
    public class ImageUpload : BaseModel
    {
        public string Url { get; set; }
        public string PublicId { get; set; }

    }
}
