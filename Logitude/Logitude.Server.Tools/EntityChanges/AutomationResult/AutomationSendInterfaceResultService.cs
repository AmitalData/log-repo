using Logitude.Server.Tools.EntityChanges.Service;
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
        private AutomationResultArgs automationResultArgs { get; set; }
        private int tenant;
        private List<Automation> sendInterfaceAutomations = new List<Automation>();
        public AutomationSendInterfaceResultService()
        {
        }
        public void Run(AutomationResultArgs automationResultArgs)
        {
            this.automationResultArgs = automationResultArgs;
            this.tenant = automationResultArgs.EntityChange.Tenant;
            this.sendInterfaceAutomations = automationResultArgs.AutomationLists.Where(d => d.ResultCode == "SENDINTERFACE").ToList();
           
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
            StorageDataService.WriteFileOnStorage(new StorageDataArgs() { FileName = (automationResultArgs.EntityChange.Id + automationResultArgs.EntityChange.EntityId + "Entity"), FolderName = "Others", Tenant = automationResultArgs.EntityChange.Tenant, FileData = xmlFile });
        }

        private void ExecuteSendInterfaceAutomations()
        {
          
            foreach (Automation automation in sendInterfaceAutomations)
            {
                string lastAuomationUpdateDate = GetLastAuomationUpdateDate(automationResultArgs.AutomationObjectTable, automationResultArgs.OtherAutomationObjectTable, automation);
                AutomatedBackup automatedBackup = GetAutomatedBackupClass(automation, automationResultArgs.EntityChange, lastAuomationUpdateDate);
                TimeSpan? automationDelayTime = null;
                if (automatedBackup.Type == "Delayed")
                {
                    DelaytimeDetails delaytimeDetails = new DelaytimeDetails() { Type = automatedBackup.Type, Delaytime = automatedBackup.Delaytime, DelaytimeIndicator = automatedBackup.DelaytimeIndicator, DelaytimeOp = automatedBackup.DelaytimeOp, SelectedDelaytimeFieldCode = automatedBackup.SelectedDelaytimeFieldCode };
                    automationDelayTime = GetAutomationDelayTime(delaytimeDetails, automationResultArgs.AutomationFieldLists);
                }

                AddAutomationQueue(new AutomationQueueArgs() { EntityChangeId = automationResultArgs.EntityChange.Id, AutomationId = automation.Id, AutomationType = automationResultArgs.EntityChangeArgs.ProcessType, EntityId = automationResultArgs.EntityChange.EntityId, Tenant = automation.Tenant,  AutomationDelayTime = automationDelayTime  , ExecutedImmediately = (automatedBackup.Type == "Delayed" ? false:true)});
            }
        }


       
    }

}
