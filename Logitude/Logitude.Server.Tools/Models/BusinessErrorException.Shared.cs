using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Models
{
    public class BusinessErrorException : Exception
    {
        public object CurrentContextTag { get; set; } //itzik
        public int Code { get; private set; } //itzik

        public BusinessErrorException(string message, BusinessErrorExceptionEnum businessErrorExceptionEnum = BusinessErrorExceptionEnum.none)
            : base(message)
        { this.Code = (int)businessErrorExceptionEnum; }

    }
    public enum BusinessErrorExceptionEnum
    {
        none = 0
    }
}
