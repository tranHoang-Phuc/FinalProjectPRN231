using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Dtos.Response
{
    public class ParamGetQuestion
    {
        public string[]? Tags { get; set; }
        public string[]? Filter { get; set; }
    }
}
