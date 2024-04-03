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
    public class AutomationSendInterfaceResultService : GeneralAutomationResultService, IAutomationResultService
    {

        public List<AutomationQueueArgs> AutomationQueues { get; set; }

        private AutomationResultArgs automationResultArgs { get; set; }

        public bool DependencyOnLastEntityUpdate { get { return true; } }
        public string ResultCode { get { return "SENDINTERFACE"; } }


        private int tenant;
        private EntityChange entityChange;

        private List<Automation> sendInterfaceAutomations = new List<Automation>();

        public void Run(AutomationResultArgs automationResultArgs)
        {
            this.automationResultArgs = automationResultArgs;
            AutomationQueues = new List<AutomationQueueArgs>();
            this.entityChange = automationResultArgs.EntityChange;
            this.tenant = this.entityChange.Tenant;

            this.sendInterfaceAutomations = automationResultArgs.AutomationLists.Where(d => d.ResultCode == ResultCode).ToList();
            if (sendInterfaceAutomations.Count > 0)
            {
                WriteEntityPMOnStorage(automationResultArgs);
                ExecuteSendInterfaceAutomations();
            }
        }


        private void WriteEntityPMOnStorage(AutomationResultArgs automationResultArgs)
        {
            string xmlString = LogitudeXmlSerializer.SerializeObjectToXmlString(automationResultArgs.EntityPM, true);
            byte[] xmlFile = Encoding.UTF8.GetBytes(xmlString);
            StorageDataService.WriteFileOnStorage(new StorageDataArgs() { FileName = (entityChange.Id + entityChange.EntityId + "Entity"), FolderName = "Others", Tenant =tenant, FileData = xmlFile });
        }

        private void ExecuteSendInterfaceAutomations()
        {
            foreach (Automation automation in sendInterfaceAutomations)
            {
                AutomationQueues.Add(new AutomationQueueArgs() { EntityChangeId = entityChange.Id, AutomationId = automation.Id, AutomationType = automationResultArgs.EntityChangeArgs.ProcessType, EntityId = entityChange.EntityId, Tenant = automation.Tenant,   ExecutedImmediately = true, EntityReference = automationResultArgs.EntityReference });
            }
        }


       
    }

}
