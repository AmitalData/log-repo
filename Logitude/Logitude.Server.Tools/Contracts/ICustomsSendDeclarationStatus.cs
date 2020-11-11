using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Contracts
{
    public interface ICustomsSendDeclarationStatus
    {
        void StartRun(string taskId, int seedDefaultTenant);
    }
}
