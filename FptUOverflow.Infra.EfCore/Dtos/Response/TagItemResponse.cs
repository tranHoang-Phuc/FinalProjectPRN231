using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Dtos.Response
{
    public class TagItemResponse
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
        public int NumberOfQuestions { get; set; }
        public int NumberOfQuestionsToday { get; set; }
        public int NumberOfQuestionThisWeek { get; set; }
    }
}
