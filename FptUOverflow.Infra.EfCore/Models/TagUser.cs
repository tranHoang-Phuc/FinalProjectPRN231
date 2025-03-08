using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Models
{
    public class TagUser
    {
        public Guid TagId { get; set; }
        public virtual Tag Tag { get; set; }
        public string CreatedBy { get; set; }
        public virtual ApplicationUser CreatedUser { get; set; }
    }
}
