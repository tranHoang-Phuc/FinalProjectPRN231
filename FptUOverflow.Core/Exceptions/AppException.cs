using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FptUOverflow.Core.Exceptions
{
    public class AppException : Exception
    {
        public ErrorCode ErrorCode { get; set; }


        public AppException(ErrorCode errorCode)
            : base(errorCode.Message)
        {
            ErrorCode = errorCode;
        }

    }
}
