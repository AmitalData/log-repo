using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.AutomationResult
{
    public class AutomationEmailResultService : IAutomationResultService
    {
       private AutomationResultArgs automationResultArgs { get; set; }
        public AutomationEmailResultService()
        {
        }

        public void Run(AutomationResultArgs automationResultArgs)
        {
            this.automationResultArgs = automationResultArgs;
            List<Automation> emailAutomations = automationResultArgs.AutomationLists.Where(d => d.ResultCode == "EMAIL").ToList();
            if (emailAutomations.Count > 0)
            {
                AddEntityChangeQueue(automationResultArgs.EntityChange.Id, automationResultArgs.EntityChangeArgs.ProcessType, automationResultArgs.EntityChangeArgs.Tenant);
            }
        }


        private void AddEntityChangeQueue(string entityChangeId, string type, int tenant)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("entitychangequeue", tenant);
            queueservice.Send(new Dictionary<string, string>() { { "EntityChangeId", entityChangeId }, { "Tenant", tenant.ToString() }, { "Type", type }, { "IsDelayAutomation", automationResultArgs.MainEntityChangeService.IsDelayAutomation.ToString().ToLower() } }, tenant, null, null, null, null);
        }
   
    }

}
