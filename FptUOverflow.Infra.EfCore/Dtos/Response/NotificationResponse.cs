using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Dtos.Response
{
    public class NotificationResponse
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public QuestionResponse Question { get; set; }
        public string CreatedUserId { get; set; }
        public ApplicationUserResponse CreatedUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Type { get; set; }
    }
}
