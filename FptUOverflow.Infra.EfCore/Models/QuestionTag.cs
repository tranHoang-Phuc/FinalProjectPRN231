using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Models
{
    public class QuestionTag
    {
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public Guid TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
