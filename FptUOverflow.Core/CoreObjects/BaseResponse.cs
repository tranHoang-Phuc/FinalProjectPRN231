using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FptUOverflow.Core.CoreObjects
{
    public class BaseResponse<T>
    {
        public int Code { get; set; } = 1000;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T? Data { get; set; }
    }
}
