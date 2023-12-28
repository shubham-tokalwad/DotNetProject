
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace refactor_me.Middleware
{
    public class ExceptionHandler
    {
    }
    public class CustomException : Exception
    {
        public int ErrorCode { get; }

        public CustomException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}