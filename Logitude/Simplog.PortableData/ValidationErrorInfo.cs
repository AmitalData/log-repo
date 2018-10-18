using System;
using System.Collections.Generic;
using System.Text;

namespace Simplog.PortableData
{
    public class ValidationErrorInfo
    {
        public string MessageType { get; set; }
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}
