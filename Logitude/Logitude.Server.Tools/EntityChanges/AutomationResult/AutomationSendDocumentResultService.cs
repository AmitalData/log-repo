using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.AutomationResult
{
    public class AutomationSendDocumentResultService : GeneralAutomationResultService, IAutomationResultService
    {
        public List<AutomationQueueArgs> AutomationQueues { get; set; }

        private AutomationResultArgs automationResultArgs { get; set; }
        private int tenant;
        private EntityChange entityChange;
        private List<Automation> sendDocumentAutomations = new List<Automation>();
        public string ResultCode { get { return "SENDDOCUMENT"; } }



        public bool DependencyOnLastEntityUpdate { get { return (processType == "OnCreate") ? true : false; } }

        private string processType = string.Empty;
        public AutomationSendDocumentResultService(string processType)
        {
            this.processType = processType;
        }



        public void Run(AutomationResultArgs automationResultArgs)
        {
            this.automationResultArgs = automationResultArgs;
            AutomationQueues = new List<AutomationQueueArgs>();

            this.entityChange = automationResultArgs.EntityChange;
            this.tenant = this.entityChange.Tenant;

            this.sendDocumentAutomations = automationResultArgs.AutomationLists.Where(d => d.ResultCode == ResultCode).ToList();
            if (sendDocumentAutomations.Count > 0)
            {
                ExecuteSendDocumentAutomations();
            }
        }

        private void ExecuteSendDocumentAutomations()
        {
            foreach (Automation automation in sendDocumentAutomations)
            {
                AutomationQueues.Add(new AutomationQueueArgs() { EntityChangeId = entityChange.Id, AutomationId = automation.Id, AutomationType = automationResultArgs.EntityChangeArgs.ProcessType, EntityId = entityChange.EntityId, Tenant = automation.Tenant,   ExecutedImmediately = true, EntityReference = automationResultArgs.EntityReference });
            }
        }
    }
}
