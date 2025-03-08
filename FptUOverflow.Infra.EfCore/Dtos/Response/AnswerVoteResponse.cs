using FptUOverflow.Infra.EfCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Dtos.Response
{
    public class AnswerVoteResponse
    {
        public Guid AnswerId { get; set; }
        public string CreatedBy { get; set; }
        public int Score { get; set; } = 10;
    }
}
