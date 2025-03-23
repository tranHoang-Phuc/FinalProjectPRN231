using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FptUOverflow.Core.CoreObjects
{
    public class ConcurrentUser
    {
        public Guid Id { get; set; }
        public Channel<string> Channel { get; set; }
        public Guid UserId { get; set; }
    }
}
