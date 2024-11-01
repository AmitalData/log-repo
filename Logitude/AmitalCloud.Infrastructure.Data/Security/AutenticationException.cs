using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AmitalCloud.Infrastructure.Data.Security
{
    public class AutenticationException : Exception
    {
        //string Message;
        public AutenticationException()
        {

        }
        public AutenticationException(string errorMessage) : base(errorMessage)
        {

        }
    }
}