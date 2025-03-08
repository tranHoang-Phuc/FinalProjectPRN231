using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Models
{
    public class QuestionVote
    {
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public string CreatedBy { get; set; }
        public virtual ApplicationUser CreatedUser { get; set; }
        public int Score { get; set; } = 10;
    }
}
