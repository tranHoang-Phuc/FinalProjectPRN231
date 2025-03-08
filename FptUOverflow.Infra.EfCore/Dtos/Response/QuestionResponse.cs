using FptUOverflow.Infra.EfCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Dtos.Response
{
    public class QuestionResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string DetailProblem { get; set; }
        public string? Expecting { get; set; }
        public int Views { get; set; } = 0;
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ApplicationUserResponse CreatedUser { get; set; }
        public  ICollection<AnswerResponse> Answers { get; set; }
        public ICollection<QuestionVoteResponse> QuestionVotes { get; set; }
        public ICollection<QuestionTagResponse> QuestionTags { get; set; }
    }
}
