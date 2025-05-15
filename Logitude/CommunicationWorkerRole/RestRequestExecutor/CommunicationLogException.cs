using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.RestRequestExecutor
{
    public class CommunicationLogException : Exception
    {
        public CommunicationLogException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
