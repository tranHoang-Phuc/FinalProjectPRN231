using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Models
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public string CreatedUserId { get; set; }
        public virtual ApplicationUser CreatedUser { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Type { get; set; } = 1;

    }
}
