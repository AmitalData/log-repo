using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.Security
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
