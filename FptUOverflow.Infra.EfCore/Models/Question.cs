using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Models
{
    public class Question : BaseModel
    {
        public string Title { get; set; }
        public string DetailProblem { get; set; }
        public string? Expecting { get; set; }
        public int Views { get; set; } = 0;
        public string CreatedBy { get; set; }
        public virtual ApplicationUser CreatedUser { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<QuestionVote> QuestionVotes { get; set; }
        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
    }
}
