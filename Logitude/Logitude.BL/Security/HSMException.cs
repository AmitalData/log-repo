using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Security
{
    public class HSMException : Exception
    {
        public HSMException()
        {
        }
        public HSMException(string errorMessage,string errorCode) : base(errorMessage)
        {
        }
    }
}
