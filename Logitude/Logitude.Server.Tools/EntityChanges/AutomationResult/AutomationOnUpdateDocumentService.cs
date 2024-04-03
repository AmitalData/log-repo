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
    public class AutomationOnUpdateDocumentService : GeneralAutomationResultService, IAutomationResultService
    {
        public List<AutomationQueueArgs> AutomationQueues { get; set; }

        private AutomationResultArgs automationResultArgs { get; set; }
        private List<Automation> onUpdateDocumentAutomations = new List<Automation>();
        private int tenant;
        private EntityChange entityChange;
        public string ResultCode { get { return "ONUPDATEDOCUMENT"; } }


        public bool DependencyOnLastEntityUpdate { get { return (processType == "OnCreate") ? true : false; } }

        private string processType = string.Empty;
        public AutomationOnUpdateDocumentService(string processType)
        {
            this.processType = processType;
        }


        public void Run(AutomationResultArgs automationResultArgs)
        {
            AutomationQueues = new List<AutomationQueueArgs>();

            this.automationResultArgs = automationResultArgs;
            this.entityChange = automationResultArgs.EntityChange;
            this.tenant = this.entityChange.Tenant;
            this.onUpdateDocumentAutomations = automationResultArgs.AutomationLists.Where(d => d.ResultCode == ResultCode).ToList();
            if (onUpdateDocumentAutomations.Count > 0)
            {
                this.ExecuteOnUpdateDocumentAutomations();
            }
        }

        private void ExecuteOnUpdateDocumentAutomations()
        {
            foreach (Automation automation in onUpdateDocumentAutomations)
            {
                AutomationQueues.Add(new AutomationQueueArgs() { EntityChangeId = entityChange.Id, AutomationId = automation.Id, AutomationType = automationResultArgs.EntityChangeArgs.ProcessType, EntityId = entityChange.EntityId, Tenant = automation.Tenant, ExtraDetails = automationResultArgs.ExtraDetails, ExecutedImmediately = true, EntityReference = automationResultArgs.EntityReference });
            }
        }

    }
}
