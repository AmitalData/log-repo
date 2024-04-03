using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.AutomationResult
{
   public interface IAutomationResultService
    {
        bool DependencyOnLastEntityUpdate { get; }
        List<AutomationQueueArgs> AutomationQueues { get; set; }

        string ResultCode { get; }
        void Run(AutomationResultArgs automationResultArgs);
    }
}
