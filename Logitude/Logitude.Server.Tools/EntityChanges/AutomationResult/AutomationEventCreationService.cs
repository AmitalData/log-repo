using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools.Models;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Logitude.Server.Tools.EntityChanges.AutomationResult
{
    public class AutomationEventCreationService : GeneralAutomationResultService, IAutomationResultService
    {
        public List<AutomationQueueArgs> AutomationQueues { get; set; }

        public string ResultCode { get { return "EVENTCREATION"; } }

        private AutomationResultArgs automationResultArgs;
    
        public  bool DependencyOnLastEntityUpdate { get { return (processType == "OnCreate") ? true : false; } }

        private string processType = string.Empty;
        public AutomationEventCreationService(string processType)
        {
            this.processType = processType;
        }


        public void Run(AutomationResultArgs automationResultArgs)
        {
            AutomationQueues = new List<AutomationQueueArgs>();
            this.automationResultArgs = automationResultArgs;
            List<Automation> eventAtomationsList = automationResultArgs.AutomationLists.Where(d => d.ResultCode == ResultCode).ToList();
            if (eventAtomationsList.Count == 0)
            {
                return;
            }

            foreach (Automation automation in eventAtomationsList)
            {
                CreateAutomation(automation);
            }
        }

        private void CreateAutomation(Automation automation)
        {
            DateTime dateBefore = DateTime.Now;
            EntityChangeAutomation entityChangesAutomation = CreateEntityChangeAutomation(automation);
            ValidateAutomationResultClass validateResult = CreateValidateAutomationResultClass(automation, entityChangesAutomation);
            if (!validateResult.IsAutomationValid)
            {
                HandleAutomationFailure(dateBefore, entityChangesAutomation);
                return;
            }

            if (validateResult.Type != "Delayed")
            {
                CreateEvent(new AutomationEventCreationArguments(automationResultArgs.EntityChange, entityChangesAutomation, dateBefore, automationResultArgs.MainEntityChangeService.EntityChangesAutomationsSsucceedList, automation));
                return;
            }
            DelaytimeDetails delaytimeDetails = new DelaytimeDetails() { Type = validateResult.Type, Delaytime = validateResult.Delaytime, DelaytimeIndicator = validateResult.DelaytimeIndicator, DelaytimeOp = validateResult.DelaytimeOp, SelectedDelaytimeFieldCode = validateResult.SelectedDelaytimeFieldCode };
            AutomationQueues.Add(new AutomationQueueArgs() { EntityChangeId = automationResultArgs.EntityChange.Id, AutomationId = automation.Id, AutomationType = automationResultArgs.EntityChangeArgs.ProcessType, EntityId = automationResultArgs.EntityChange.EntityId, Tenant = automation.Tenant, AutomationDelayTime = GetAutomationDelayTime(delaytimeDetails, automationResultArgs.AutomationFieldLists, automationResultArgs.EntityChange.Tenant), EntityReference = automationResultArgs.EntityReference });
        }

        private void HandleAutomationFailure(DateTime dateBefore, EntityChangeAutomation entityChangesAutomation)
        {
            entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(automationResultArgs.EntityChangeArgs.Tenant);
            automationResultArgs.MainEntityChangeService.EntityChangesAutomationsFailedList.Add(entityChangesAutomation);
            entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
        }

        public void CreateEvent(AutomationEventCreationArguments automationEventCreationArguments)
        {
            AutomatedBackup automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automationEventCreationArguments.Automation.AutomationXML);

            IAddManualTraceEventsHelper helper = ContainerAccessor.Container.Resolve(typeof(IAddManualTraceEventsHelper), "AddManualTraceEventsHelper") as IAddManualTraceEventsHelper;
            helper.Initialize(automationEventCreationArguments.EntityChange.Tenant);
            helper.Trace(BuildEvent(automationEventCreationArguments, automatedBackup), "system@tenant" + automationEventCreationArguments.EntityChange.Tenant + ".com");

            automationEventCreationArguments.EntityChange.HasExecutedRecord = true;
            automationEventCreationArguments.EntityChangeAutomation.IsConditionTrue = true;
            automationEventCreationArguments.EntityChangeAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(automationEventCreationArguments.EntityChange.Tenant);
            automationEventCreationArguments.EntityChangeAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - automationEventCreationArguments.DateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
            automationEventCreationArguments.EntityChangesAutomationsSsucceedList.Add(automationEventCreationArguments.EntityChangeAutomation);
        }

        private ValidateAutomationResultClass CreateValidateAutomationResultClass(Automation automation, EntityChangeAutomation entityChangesAutomation)
        {
            entityChangesAutomation.ResultCode = "Event Creation";

            string lastUpdate = GetLastAuomationUpdateDate(automationResultArgs.AutomationObjectTable, automationResultArgs.OtherAutomationObjectTable, automation);
            ValidateAutomationResultClass validateResult = ValidateAutomation(automation, automationResultArgs.EntityChange, automationResultArgs.AutomationFieldLists, lastUpdate, "");
            entityChangesAutomation.ConditionsList = validateResult.ConditionsList;
            if (validateResult.Type == "Delayed") automationResultArgs.MainEntityChangeService.IsDelayAutomation = true;

            entityChangesAutomation.type = validateResult.IsAutomationValid ? "EventCreatedSucceed" : "EventCreatedFailed";
            return validateResult;
        }


        private TraceEventsServiceArgs BuildEvent(AutomationEventCreationArguments automationEventCreationArgumentse, AutomatedBackup automatedBackup)
        {
            return new TraceEventsServiceArgs()
            {
                EntityId = automationEventCreationArgumentse.EntityChange.EntityId,
                ObjectTableId = automationEventCreationArgumentse.EntityChange.ObjectTableId,
                EventTypeId = automatedBackup.AutomationEvent.EventTypeId,
                EventDate = TenantServerConfigration.GetCurrentDateTime(automationEventCreationArgumentse.EntityChange.Tenant),
                Notes = automatedBackup.AutomationEvent.NoteValue,
                IsAutomation = true,
                EntityPM = automationEventCreationArgumentse.EntityPM ?? automationResultArgs?.EntityPM
            };
        }

    }

    public class AutomationEventCreationArguments
    {
        public AutomationEventCreationArguments()
        {
        }

        public AutomationEventCreationArguments(EntityChange entityChange, EntityChangeAutomation entityChangeAutomation, DateTime dateBefore, List<EntityChangeAutomation> entityChangesAutomationsSsucceedList, Automation automation)
        {
            EntityChange = entityChange;
            EntityChangeAutomation = entityChangeAutomation;
            DateBefore = dateBefore;
            EntityChangesAutomationsSsucceedList = entityChangesAutomationsSsucceedList;
            Automation = automation;
        }

        public EntityChange EntityChange { get; set; }
        public EntityChangeAutomation EntityChangeAutomation { get; set; }
        public DateTime DateBefore { get; set; }
        public List<EntityChangeAutomation> EntityChangesAutomationsSsucceedList { get; set; }
        public List<EntityChangeAutomation> EntityChangesAutomationsLists { get; set; }
        public Automation Automation { get; set; }
        public ValidateAutomationResultClass ValidateResult { get; set; }
        public object EntityPM { get; set; }
    }
}
