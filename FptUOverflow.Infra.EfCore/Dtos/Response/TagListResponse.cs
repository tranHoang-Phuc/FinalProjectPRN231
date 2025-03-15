using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Infra.EfCore.Dtos.Response
{
    public class TagListResponse
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public List<TagItemResponse> Tags { get; set; }
    }
}
