using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Models
{
    public class Tag : BaseModel
    {
        public string TagName { get; set; }
        public string CreatedBy { get; set; }
        public virtual ApplicationUser CreatedUser { get; set; }
        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
        public virtual ICollection<TagUser> TagUsers { get; set; }

    }
}
