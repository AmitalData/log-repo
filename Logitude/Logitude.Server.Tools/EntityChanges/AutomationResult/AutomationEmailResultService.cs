using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.AutomationResult
{
    public class AutomationEmailResultService : GeneralAutomationResultService, IAutomationResultService
    {


        public AutomationEmailResultService()
        {
        }

        public void Run(AutomationResultArgs automationResultArgs)
        {
            var automationsEmail = automationResultArgs.AutomationLists.Where(d => d.ResultCode == "EMAIL").ToList();
            if (automationsEmail.Count > 0)
            {
                foreach (Automation automation in automationsEmail)
                {
                    AddAutomationQueue(new AutomationQueueArgs() { EntityChangeId = automationResultArgs.EntityChange.Id, AutomationId = automation.Id, AutomationType = automationResultArgs.EntityChangeArgs.ProcessType, EntityId = automationResultArgs.EntityChange.Id, Tenant = automation.Tenant, ExecutedImmediately = true });
                }
            }
        }

  
    }

}
