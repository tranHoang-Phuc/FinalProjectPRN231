using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Dtos.Response
{
    public class QuestionResponseList
    {
        public int Total { get; set; }
        public List<QuestionResponse> Questions { get; set; }
    }
}
