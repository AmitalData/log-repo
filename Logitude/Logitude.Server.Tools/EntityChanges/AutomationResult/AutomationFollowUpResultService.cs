using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.AutomationResult
{
   public class AutomationFollowUpResultService: GeneralAutomationResultService , IAutomationResultService
    {

        public List<AutomationQueueArgs> AutomationQueues { get; set; }

        public string ResultCode { get { return "FOLLOWUP"; } }
        public bool DependencyOnLastEntityUpdate { get { return (processType == "OnCreate") ? true : false; } }

        private string processType = string.Empty;
        public AutomationFollowUpResultService(string processType)
        {
            this.processType = processType;
        }
        public void Run(AutomationResultArgs automationResultArgs)
        {

            AutomationQueues = new List<AutomationQueueArgs>();

            List<Automation> followUpautomationsList = automationResultArgs.AutomationLists.Where(d => d.ResultCode == ResultCode || d.ResultCode == "DOCOUTFOLLOWUP" || d.ResultCode == "DOCINFOLLOWUP").ToList();

            if (followUpautomationsList.Count > 0)
            {
                foreach (Automation automation in followUpautomationsList)
                {
                    DateTime dateBefore = DateTime.Now;
                    EntityChangeAutomation entityChangesAutomation = CreateEntityChangeAutomation(automation);
                    if (automation.ResultCode == "FOLLOWUP") entityChangesAutomation.ResultCode = "F/U Creation";
                    else if (automation.ResultCode == "DOCOUTFOLLOWUP") entityChangesAutomation.ResultCode = "Docs Out F/U Creation";
                    else if (automation.ResultCode == "DOCINFOLLOWUP") entityChangesAutomation.ResultCode = "Docs In F/U Creation";

                    var otherLastupdateautomation = (automationResultArgs.OtherAutomationObjectTable != null && !string.IsNullOrEmpty(automationResultArgs.OtherAutomationObjectTable.AutomationLastUpdate)) ? (automationResultArgs.OtherAutomationObjectTable.Id + "@" + automationResultArgs.OtherAutomationObjectTable.AutomationLastUpdate) : "";
                    string lastUpdate = GetLastAuomationUpdateDate(automationResultArgs.AutomationObjectTable, automationResultArgs.OtherAutomationObjectTable, automation);

                    ValidateAutomationResultClass validateResult = ValidateAutomation(automation, automationResultArgs.EntityChange, automationResultArgs.AutomationFieldLists, lastUpdate, "");
                    entityChangesAutomation.ConditionsList = validateResult.ConditionsList;

                    if (validateResult.Type == "Delayed") automationResultArgs.MainEntityChangeService.IsDelayAutomation = true;

                    entityChangesAutomation.type = validateResult.IsAutomationValid ? "FollowUpCreatedSsucceed" : "FollowUpCreatedFailed";

                    if (validateResult.IsAutomationValid)
                    {
                        if (validateResult.Type == "Delayed")
                        {
                            DelaytimeDetails delaytimeDetails = new DelaytimeDetails() { Type = validateResult.Type, Delaytime = validateResult.Delaytime, DelaytimeIndicator = validateResult.DelaytimeIndicator, DelaytimeOp = validateResult.DelaytimeOp, SelectedDelaytimeFieldCode = validateResult.SelectedDelaytimeFieldCode };
                            AutomationQueues.Add(new AutomationQueueArgs() { EntityChangeId = automationResultArgs.EntityChange.Id, AutomationId = automation.Id, AutomationType = automationResultArgs.EntityChangeArgs.ProcessType, EntityId = automationResultArgs.EntityChangeArgs.EntityId, Tenant = automation.Tenant, AutomationDelayTime = GetAutomationDelayTime(delaytimeDetails, automationResultArgs.AutomationFieldLists, automationResultArgs.EntityChange.Tenant), EntityReference = automationResultArgs.EntityReference });

                        }
                        else AddAutomationFollowUp(automationResultArgs.EntityChangeArgs.EntityPM, automationResultArgs.EntityChange, automationResultArgs.AutomationFieldLists, lastUpdate, automationResultArgs.MainEntityChangeService.EntityChangesAutomationsSsucceedList, automation, entityChangesAutomation, dateBefore, true);

                    }
                    else
                    { 
                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(automationResultArgs.EntityChangeArgs.Tenant);
                        automationResultArgs.MainEntityChangeService.EntityChangesAutomationsFailedList.Add(entityChangesAutomation);
                        entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                    }
                }
            }
        }

        public void AddAutomationFollowUp(Object entityPM, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, List<EntityChangeAutomation> EntityChangesAutomationsSsucceedList, Automation automation, EntityChangeAutomation entityChangesAutomation, DateTime dateBefore, bool isRefreshShipmentFollowUps = false)
        {
            AutomationFollowUp automationFollowUp = null;

            #region  Get AutomationFollowUp From Cache

            string automationFollowUpName = "AutomationFollowUp" + lastupdateautomation + automation.Id + automation.Tenant;

            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(automationFollowUpName) == null)
                {
                    if (!string.IsNullOrEmpty(automation.AutomationXML))
                    {
                        AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                        automationFollowUp = AutomatedBackup.AutomationFollowUp;
                        CacheManager.CacheWrapper.Insert(automationFollowUpName, automationFollowUp, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                    }
                }
                else
                {
                    automationFollowUp = (AutomationFollowUp)CacheManager.CacheWrapper.Get(automationFollowUpName);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(automation.AutomationXML))
                {
                    AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                    automationFollowUp = AutomatedBackup.AutomationFollowUp;
                }
            }

            #endregion

            if (automationFollowUp != null)
            {
                #region Fill Data
                string userId = GetSystemContactIdByTenant(entityChange.Tenant);

                string ownerId = automationFollowUp.OwnerValue;
                string note = automationFollowUp.NoteValue;
                string eventTypeId = automationFollowUp.EventTypeId;
                string followUpDateFieldName = "";
                DateTime? date = null;

                //ownerId
                if (automationFollowUp.OwnerFieldType == "Field")
                {
                    ownerId = "";
                    Field field = automationFieldLists.Where(d => d.FieldCode == automationFollowUp.OwnerValue).FirstOrDefault();
                    if (field != null) ownerId = field.Value;
                }

                //Date
                if (!string.IsNullOrEmpty(automationFollowUp.DateValue))
                {
                    Field field = automationFieldLists.Where(d => d.FieldCode == automationFollowUp.DateValue).FirstOrDefault();
                    if (field != null)
                    {
                        followUpDateFieldName = field.PropertyName;
                        if (!string.IsNullOrEmpty(field.Value))
                        {

                            date = ConvertToDate(field.Value);
                        }
                    }
                }

                if (string.IsNullOrEmpty(ownerId)) ownerId = entityChange.CreateByUserId;

                #endregion

                if (!string.IsNullOrEmpty(ownerId))
                {

                    EventTypeRepository eventTypeRepository = new EventTypeRepository(entityChange.Tenant);
                    automationFollowUp.FollowUpEnglishName = eventTypeRepository.GetEventTypeNameById(automationFollowUp.EventTypeId, entityChange.Tenant);


                    FollowUpRepository followUpRepository = new FollowUpRepository(entityChange.Tenant);
                    bool isAddFollowUp = false;
                    if (automation.ResultCode == "DOCOUTFOLLOWUP" || automation.ResultCode == "DOCINFOLLOWUP")
                    {
                        if (automationFollowUp.DocumentTypeLists != null)
                        {
                            List<string> documentTypeIds = new List<string>();
                            foreach (FollowUpDocumentTypeList documentTypeList in automationFollowUp.DocumentTypeLists)
                            {
                                documentTypeIds.Add(documentTypeList.Id);
                            }

                            documentTypeIds = followUpRepository.GetDocumentTypeIdListsFromFollowUp(entityChange.EntityId, automationFollowUp.ObjectTableName, automationFollowUp.EventTypeId, documentTypeIds, entityChange.Tenant).ToList();
                            foreach (FollowUpDocumentTypeList documentTypeList in automationFollowUp.DocumentTypeLists)
                            {
                                string documentTypeId = documentTypeIds.Where(d => d == documentTypeList.Id).FirstOrDefault();
                                if (string.IsNullOrEmpty(documentTypeId))
                                {
                                    AddFollowUp(automation.Id, entityChange, automationFollowUp, ownerId, documentTypeList.Name, eventTypeId, date, followUpRepository, followUpDateFieldName, userId, documentTypeList.Id, documentTypeList.Area);
                                    isAddFollowUp = true;
                                    documentTypeIds.Add(documentTypeList.Id);
                                }
                            }
                        }
                    }
                    else
                    {
                        bool isFollowUpExist = followUpRepository.CheckIfFollowUpExist(entityChange.EntityId, automationFollowUp.ObjectTableName, automationFollowUp.EventTypeId, entityChange.Tenant);
                        if (!isFollowUpExist)
                        {
                            AddFollowUp(automation.Id, entityChange, automationFollowUp, ownerId, note, eventTypeId, date, followUpRepository, followUpDateFieldName, userId);
                            isAddFollowUp = true;
                        }
                    }

                    if (isAddFollowUp)
                    {
                        followUpRepository.SubmitChanges();
                        if (isRefreshShipmentFollowUps)
                        {
                            PropertyInfo propInfo = entityPM.GetType().GetProperty("IsRefreshShipmentFollowUps");
                            if (propInfo != null)
                            {
                                propInfo.SetValue(entityPM, true, null);
                            }
                            else
                            {
                                PropertyInfo QuotePropInfo = entityPM.GetType().GetProperty("IsRefreshQuoteFollowUps");
                                if (QuotePropInfo != null)
                                {
                                    QuotePropInfo.SetValue(entityPM, true, null);
                                }
                            }
                        }
                       
                    }
                }

                entityChange.HasExecutedRecord = true;
                entityChangesAutomation.IsConditionTrue = true;
                entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
                entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                EntityChangesAutomationsSsucceedList.Add(entityChangesAutomation);
            }
        }

        private void AddFollowUp(string automationId, EntityChange entityChange, AutomationFollowUp automationFollowUp, string ownerId, string note, string eventTypeId, DateTime? date, FollowUpRepository followUpRepository, string followUpDateFieldName, string userId, string documentTypeId = null, string area = null)
        {
            int tenant = entityChange.Tenant;
            FollowUp followUp = new FollowUp();
            followUp.Id = IdCounter.GetNumber("FollowUp", tenant).ToString();
            followUp.EventTypeId = eventTypeId;
            if (automationFollowUp.ObjectTableName == "Shipment" || automationFollowUp.ObjectTableName == "Master") followUp.ShipmentId = entityChange.EntityId;
            if (automationFollowUp.ObjectTableName == "Quote") followUp.QuoteId = entityChange.EntityId;
            followUp.OwnerUserId = ownerId;
            followUp.Notes = note;
            followUp.Date = date;
            followUp.IsNew = false;
            followUp.LegType = automationFollowUp.LegType;
            followUp.AutomationId = automationId;
            followUp.DateEscalationActionTimeIndicatorCode = automationFollowUp.DateEscalationActionTimeIndicatorCode;
            followUp.DateEscalationTime = automationFollowUp.DateEscalationTime;
            followUp.DateFieldName = followUpDateFieldName;

            if (!string.IsNullOrEmpty(documentTypeId)) followUp.DocumentTypeId = documentTypeId;
            if (!string.IsNullOrEmpty(area)) followUp.Area = area;

            followUp.Tenant = tenant;
            followUpRepository.Add(followUp);

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = tenant,
                EventTypeCode = "SFCR",
                UserId = userId,
                EntityId = entityChange.EntityId,
                ObjectTableName = automationFollowUp.ObjectTableName == "Master" ? "Shipment" : automationFollowUp.ObjectTableName,
                Notes = automationFollowUp.FollowUpEnglishName + "\n" + "Resulted from Automation",

            });


        }


     
    }
}
