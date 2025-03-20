using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Core.CoreObjects
{
    public class PagedResponse<T>
    {
        public int TotalPage { get; set; }
        public int PageIndex { get; set; }
        public List<T> Data { get; set; }
    }
}
