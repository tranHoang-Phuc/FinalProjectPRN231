using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Dtos.Request
{
    public class UpdateQuestionRequest
    {
        public string Title { get; set; }
        public string DetailProblem { get; set; }
        public string? Expecting { get; set; }
        public List<string> Tags { get; set; }
    }
}
