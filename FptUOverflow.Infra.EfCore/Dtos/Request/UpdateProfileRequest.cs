using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Dtos.Request
{
    public class UpdateProfileRequest
    {
        public string DisplayName { get; set; }
        public string? Location { get; set; }
        public string? Title { get; set; }
        public string? AboutMe { get; set; }
        public string? ProfileImage { get; set; }
    }
}
