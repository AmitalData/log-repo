using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationWorkerRole.RestRequestExecutor
{

    public static class CommunicationConstants
    {
        public const char TypeQueue = 'Q';
         public const char InOut = 'O';
        public const string SubjectCustomerActivation = "Customer ready for activation";
        public const string DefaultFolder = "ExternalTasksQueue";
    }
}
