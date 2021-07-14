using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.Security
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