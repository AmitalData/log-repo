using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.Exceptions
{
    public class BusinessErrorException : Exception
    {
        public object CurrentContextTag { get; set; } 
        public int Code { get; private set; } 
        public BusinessErrorException(string message, BusinessErrorExceptionEnum businessErrorExceptionEnum = BusinessErrorExceptionEnum.none)
            : base(message)
        { this.Code = (int)businessErrorExceptionEnum; }
    }
    public enum BusinessErrorExceptionEnum
    {
        none = 0
    }
}
