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
        public string ResultCode { get { return "EMAIL"; } }

        public bool DependencyOnLastEntityUpdate { get { return (processType == "OnCreate") ? true : false; } }

        private string processType = string.Empty;
        public AutomationEmailResultService(string processType)
        {
            this.processType = processType;
        }

        public void Run(AutomationResultArgs automationResultArgs)
        {
            var automationsEmail = automationResultArgs.AutomationLists.Where(d => d.ResultCode == ResultCode).ToList();
            if (automationsEmail.Count > 0)
            {
                foreach (Automation automation in automationsEmail)
                {
                    AddAutomationQueue(new AutomationQueueArgs() { EntityChangeId = automationResultArgs.EntityChange.Id, AutomationId = automation.Id, AutomationType = automationResultArgs.EntityChangeArgs.ProcessType, EntityId = automationResultArgs.EntityChange.Id, Tenant = automation.Tenant, ExecutedImmediately = true , EntityReference  = automationResultArgs.EntityReference});
                }
            }
        }

  
    }

}
