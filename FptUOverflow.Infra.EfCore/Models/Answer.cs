using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Models
{
    public class Answer : BaseModel
    {
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public string Content { get; set; }
        public string? CreatedBy { get; set; }
        public virtual ApplicationUser CreatedUser { get; set; }
        public bool IsApproved { get; set; } = false;
        public virtual ICollection<AnswerVote> AnswerVotes { get; set; }
    }
}
