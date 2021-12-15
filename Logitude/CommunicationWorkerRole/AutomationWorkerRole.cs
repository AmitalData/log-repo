using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.EntityChanges.AutomationResult;
using Logitude.Server.Tools.EntityChanges.Service;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.AutomationModel;

namespace CommunicationWorkerRole
{
    class AutomationWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
        int Tenant = 0;
        string entityChangeId = string.Empty;
        string automationId = string.Empty;
        string entityId = string.Empty;
        int AutomationCount = 0;
        string type = string.Empty;
        bool executedImmediately = false;

        public AutomationWorkerRole(string tenant)
        {
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "AutomationWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            ConnectClient();
            return base.OnStart();
        }

        public override async void AsyncRun()
        {
            APICredentialsParameters APICredentialsParam = new APICredentialsParameters()
            {
                PrimaryKey = "8eb9c6e4-c1ca-43e5-8061-87a7adcdc5f8",
                SecondaryKey = "c2dd0ebf-20bf-4d44-916c-7f9000dce4ec"
            };

            using (var client = new HttpClient())
            {

            }

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        queueservice = new DbQueueService();
                        queueservice.InitializeQueue("AutomationQueue", 0);
                        var response = queueservice.Receive();
                        LastActivity = DateTime.UtcNow;

                        if (response != null && response.MessageId != null)
                        {
                            try
                            {
                                entityChangeId = response.MessageValues["EntityChangeId"].ToString();
                                type = response.MessageValues["Type"].ToString();
                                automationId = response.MessageValues["AutomationId"].ToString();
                                entityId = response.MessageValues["EntityId"];
                                executedImmediately = response.MessageValues["ExecutedImmediately"] != null ? bool.Parse(response.MessageValues["ExecutedImmediately"].ToString()) : false;

                                string tenant = response.MessageValues["Tenant"].ToString();

                                Tenant = int.Parse(tenant);

                                if (string.IsNullOrEmpty(entityChangeId) || string.IsNullOrEmpty(automationId))
                                {
                                    queueservice.Complete();
                                    continue;
                                }

                                EntityChangeRepository entityChangeRepository = new EntityChangeRepository(Tenant);
                                EntityChange entityChange = entityChangeRepository.GetSingleEntityChange(entityChangeId, Tenant);

                                if (entityChange == null)
                                {
                                    queueservice.Complete();
                                    continue;
                                }

                                if (string.IsNullOrEmpty(entityId))
                                {
                                    entityId = entityChange.EntityId;
                                }

                                List<Field> AutomationConditionFieldLists = null;
                                AutomationConditionFields automationConditionFields = new AutomationConditionFields();
                                if (!string.IsNullOrEmpty(entityChange.AutomationConditionFieldsXml))
                                {
                                    automationConditionFields = LogitudeXmlSerializer.DeserializeObject<AutomationConditionFields>(entityChange.AutomationConditionFieldsXml);
                                    AutomationConditionFieldLists = automationConditionFields.Fields;
                                }

                                AutomationRepository automationRepository = new AutomationRepository(Tenant);
                                string objectTableId = automationConditionFields.ObjectTableId;
                                if (string.IsNullOrEmpty(objectTableId)) objectTableId = entityChange.ObjectTableId;

                                string otherObjectTableLastUpdateDate = string.Empty;
                                string otherObjectTableId = string.Empty;
                                List<Automation> automationList = new List<Automation>();

                                if (automationConditionFields.LastUpdateDate != null)
                                {
                                    automationList = automationRepository.GetAutomationsByObjectTableId(objectTableId, Tenant, automationConditionFields.LastUpdateDate.ToString()).Where(d => d.Type == type).ToList();
                                }

                                if (automationConditionFields.IsRunMasterHouseAutomation && !string.IsNullOrEmpty(automationConditionFields.OtherObjectTableIdWithLastUpdate))
                                {
                                    var otherObjectTableDetails = automationConditionFields.OtherObjectTableIdWithLastUpdate.Split('@');
                                    otherObjectTableId = otherObjectTableDetails[0];
                                    if (otherObjectTableDetails.Length > 1) otherObjectTableLastUpdateDate = otherObjectTableDetails[1];

                                    if (!string.IsNullOrEmpty(otherObjectTableId) && !string.IsNullOrEmpty(otherObjectTableLastUpdateDate))
                                    {
                                        List<Automation> otherAutomations = automationRepository.GetAutomationsByObjectTableId(otherObjectTableId, Tenant, otherObjectTableLastUpdateDate).Where(d => d.Type == type).ToList();
                                        foreach (Automation item in otherAutomations)
                                        {
                                            automationList.Add(item);
                                        }
                                    }
                                }

                                Automation automation = automationList.Where(d => d.Id == automationId).FirstOrDefault();
                                AutomationCount = automationList.Count();

                                if (automation == null)
                                {
                                    queueservice.Complete();
                                    continue;
                                }

                                GeneralAutomationResultService generalAutomationResultService = new GeneralAutomationResultService();
                                DateTime dateBefore = DateTime.Now;
                                EntityChangeAutomation entityChangesAutomation = new EntityChangeAutomation()
                                {
                                    Id = IdCounter.GetNumber("EntityChangeAutomation", Tenant),
                                    AutomationId = automation.Id,
                                    AutomationType = automation.Type,
                                    CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                                    AutomationName = automation.Name,
                                    AutomationDescription = automation.Description,
                                };

                                entityChangesAutomation.ResultCode = GetentityChangesResultCode(automation.ResultCode);
                                string automationLastUpdateDate = "";
                                if (automationConditionFields.LastUpdateDate != null)
                                {
                                    automationLastUpdateDate = automationConditionFields.LastUpdateDate.ToString();
                                }

                                if (!string.IsNullOrEmpty(otherObjectTableId) && automation.ObjectTableId == otherObjectTableId)
                                {
                                    automationLastUpdateDate = otherObjectTableLastUpdateDate;
                                }

                                ValidateAutomationResultClass validateResult = generalAutomationResultService.ValidateAutomation(automation, entityChange, AutomationConditionFieldLists, automationLastUpdateDate, executedImmediately ? "" : "Delayed");
                                entityChangesAutomation.ConditionsList = validateResult.ConditionsList;

                                if (validateResult.IsAutomationValid && executedImmediately && validateResult.Type == "Delayed")
                                {
                                    DelaytimeDetails delaytimeDetails = new DelaytimeDetails() { Type = validateResult.Type, Delaytime = validateResult.Delaytime, DelaytimeIndicator = validateResult.DelaytimeIndicator, DelaytimeOp = validateResult.DelaytimeOp, SelectedDelaytimeFieldCode = validateResult.SelectedDelaytimeFieldCode };
                                    generalAutomationResultService.AddAutomationQueue(new AutomationQueueArgs() { EntityChangeId = entityChange.Id, AutomationId = automation.Id, AutomationType = type, EntityId = entityId, Tenant = automation.Tenant, AutomationDelayTime = generalAutomationResultService.GetAutomationDelayTime(delaytimeDetails, AutomationConditionFieldLists, entityChange.Tenant) });
                                    queueservice.Complete();
                                    LogDoneItemInMemory();

                                    continue;
                                }



                                List<EntityChangeAutomation> ChangesAutomationsLists = FullEntityChangeAutomationList(entityChange, null);
                                List<EntityChangeAutomation> entityChangesAutomationsLists = ChangesAutomationsLists.Where(d => d.ResultCode == entityChangesAutomation.ResultCode).ToList();
                                entityChangesAutomation.IsConditionTrue = validateResult.IsAutomationValid;
                                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(Tenant);
                                ObjectTable objectTable = objectTabelRepository.GetSingleObjectTable(entityChange.ObjectTableId, Tenant, true);
                                AutomatedBackup automatedBackup = generalAutomationResultService.GetAutomatedBackupClass(automation, entityChange, automationLastUpdateDate);
                                #region Send Interface


                                if (automation.ResultCode == "SENDINTERFACE")
                                {
                                    entityChangesAutomation.type = validateResult.IsAutomationValid ? "SendInterfaceSsucceed" : "SendInterfaceFailed";
                                    if (validateResult.IsAutomationValid)
                                    {

                                        AutomationSendInterface automationSendInterface = automatedBackup.AutomationSendInterface;
                                        string documentId = GetSendInterfaceDataContractDocumentId(entityChange, automationSendInterface);
                                        if (automationSendInterface.SendVia == "EMAIL")
                                        {
                                            entityChangesAutomation.ComunicationLogId = new AutomationHelper().ExecuteEmailAutomation(new AutomationSendEmailArgs() { EntityId = entityChange.EntityId, CreateByUserId = entityChange.CreateByUserId, ObjectTableId = entityChange.ObjectTableId, Tenant = entityChange.Tenant, AutomationConditionFieldLists = AutomationConditionFieldLists, Automation = automation, ObjectTableName = objectTable != null ? objectTable.Name : "", ExternalAttachmentDocumentId = documentId });
                                        }
                                        else if (automationSendInterface.SendVia == "FTP")
                                        {
                                            ApplyAuomationSendInterfaceFTP(entityChange, automationSendInterface, documentId);
                                        }
                                        else if (automationSendInterface.SendVia == "WEBHOOK")
                                        {
                                            ApplyAutomationSendInterfaceWebHook(entityChange, automationSendInterface, documentId);
                                        }
                                        MarkEntityChangeExecutedRecord(entityChange, entityChangesAutomation, entityChangesAutomationsLists);
                                    }
                                    else
                                    {
                                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                                        entityChangesAutomationsLists.Add(entityChangesAutomation);
                                    }

                                    entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                                    entityChange.SendInterfaceAutomationFailedXml = entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList()) : "";
                                    entityChange.SendInterfaceAutomationSsucceedXml = entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
                                }
                                #endregion

                                #region Send Document
                                if (automation.ResultCode == "SENDDOCUMENT")
                                {
                                    entityChangesAutomation.type = validateResult.IsAutomationValid ? "SendDocumentSsucceed" : "SendDocumentFailed";
                                    if (validateResult.IsAutomationValid)
                                    {
                                        AutomationSendDocument automationSendDocument = automatedBackup.AutomationSendDocument;
                                        string objectTableName = objectTable != null ? objectTable.Name : "";
                                        AutomationSendEmailArgs automationSendEmailArgs = new AutomationSendEmailArgs() { EntityId = entityChange.EntityId, CreateByUserId = entityChange.CreateByUserId, ObjectTableId = entityChange.ObjectTableId, Tenant = entityChange.Tenant, AutomationConditionFieldLists = AutomationConditionFieldLists, Automation = automation, ObjectTableName = objectTableName, ReportTemplateId = automatedBackup.ReportTemplateId, DocumentCopyId = automatedBackup.DocumentCopyId };
                                        if (automationSendDocument.SendVia == "EMAIL")
                                        {
                                            AutomationHelper automationHelper = new AutomationHelper();
                                            string comunicationLogId = automationHelper.ExecuteEmailAutomation(automationSendEmailArgs);
                                        }
                                        else if (automationSendDocument.SendVia == "FTP")
                                        {
                                            AutomationDocumentHelper automationDocumentHelper = new AutomationDocumentHelper(automationSendEmailArgs);
                                            AutomationDocumentResult automationDocumentResult = automationDocumentHelper.GetAutomationDocumentResult();
                                            automationDocumentResult.ObjectTableName = objectTableName;
                                            string documentFileName = GetDocumentFileName(entityChange, automation, automationDocumentResult);
                                            ApplyAutomationSendDocumentFTP(automationSendDocument, automationDocumentResult, documentFileName);
                                        }
                                        MarkEntityChangeExecutedRecord(entityChange, entityChangesAutomation, entityChangesAutomationsLists);
                                    }
                                    else
                                    {
                                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                                        entityChangesAutomationsLists.Add(entityChangesAutomation);
                                    }
                                    entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                                    entityChange.SendDocumentAutomationFailedXml = entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList()) : "";
                                    entityChange.SendDocumentAutomationSsucceedXml = entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
                                }
                                #endregion

                                #region E-mail
                                if (automation.ResultCode == "EMAIL")
                                {
                                    entityChangesAutomation.type = validateResult.IsAutomationValid ? "EmailSsucceed" : "EmailFailed";

                                    if (validateResult.IsAutomationValid)
                                    {
                                        string objectTableName = objectTable != null ? objectTable.Name : "";
                                        AutomationHelper automationHelper = new AutomationHelper();
                                        string comunicationLogId = automationHelper.ExecuteEmailAutomation(new AutomationSendEmailArgs() { EntityId = entityChange.EntityId, CreateByUserId = entityChange.CreateByUserId, ObjectTableId = entityChange.ObjectTableId, Tenant = entityChange.Tenant, AutomationConditionFieldLists = AutomationConditionFieldLists, Automation = automation, ObjectTableName = objectTableName, ReportTemplateId = automatedBackup.ReportTemplateId, DocumentCopyId = automatedBackup.DocumentCopyId });
                                        MarkEntityChangeExecutedRecord(entityChange, entityChangesAutomation, entityChangesAutomationsLists);
                                    }
                                    else
                                    {
                                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                                        entityChangesAutomationsLists.Add(entityChangesAutomation);
                                    }

                                    entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                                    entityChange.EmailAutomationSsucceedXml = entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
                                    entityChange.EmailAutomationFailedXml = entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList()) : "";


                                }
                                #endregion

                                #region Set Fields Value
                                else if (automation.ResultCode == "FIELDSET")
                                {
                                    entityChangesAutomation.type = validateResult.IsAutomationValid ? "SetSsucceed" : "SetFailed";

                                    if (validateResult.IsAutomationValid)
                                    {
                                        if (objectTable != null)
                                        {
                                            var entityPM = GetEntity(objectTable.Name, entityId, Tenant);
                                            if (entityPM != null)
                                            {
                                                r rFields = new r();
                                                List<c> changesFields = new List<c>();

                                                if (!string.IsNullOrEmpty(entityChange.ChangesAutomationFieldsXml))
                                                {
                                                    rFields = LogitudeXmlSerializer.DeserializeObject<r>(entityChange.ChangesAutomationFieldsXml);
                                                    if (rFields != null)
                                                    {
                                                        changesFields = rFields.cs;
                                                    }
                                                }


                                                new AutomationSetValueResultService().SetValue(entityPM, entityChange, AutomationConditionFieldLists, automationLastUpdateDate, entityChangesAutomationsLists, changesFields, automation, entityChangesAutomation, dateBefore);
                                                UpdateEntitiy(entityPM, objectTable.Name, entityChange.CreateByUserId, Tenant);
                                                rFields.cs = changesFields;
                                                entityChange.ChangesAutomationFieldsXml = rFields.cs != null && rFields.cs.Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(rFields) : "";
                                            }
                                        }

                                    }
                                    else
                                    {
                                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                                        entityChangesAutomationsLists.Add(entityChangesAutomation);
                                    }

                                    entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                                    entityChange.SetAutomationSsucceedXml = entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
                                    entityChange.SetAutomationFailedXml = entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList()) : "";






                                }
                                #endregion

                                #region F/U Creation
                                else if (automation.ResultCode == "FOLLOWUP" || automation.ResultCode == "DOCOUTFOLLOWUP" || automation.ResultCode == "DOCINFOLLOWUP")
                                {
                                    entityChangesAutomation.type = validateResult.IsAutomationValid ? "FollowUpCreatedSsucceed" : "FollowUpCreatedFailed";

                                    if (validateResult.IsAutomationValid)
                                    {
                                        if (objectTable != null)
                                        {
                                            var entityPM = GetEntity(objectTable.Name, entityId, Tenant);
                                            if (entityPM != null)
                                            {
                                                new AutomationFollowUpResultService().AddAutomationFollowUp(entityPM, entityChange, AutomationConditionFieldLists, automationLastUpdateDate, entityChangesAutomationsLists, automation, entityChangesAutomation, dateBefore);
                                                // UpdateEntitiy(entityPM, objectTable.Name, entityChange.CreateByUserId, Tenant);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                                        entityChangesAutomationsLists.Add(entityChangesAutomation);
                                    }
                                    entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                                    entityChange.FollowUpAutomationFailedXml = entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList()) : "";
                                    entityChange.FollowUpAutomationSsucceedXml = entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";



                                }
                                #endregion

                                #region Queued Task
                                else if (automation.ResultCode == "QUEUE")
                                {
                                    entityChangesAutomation.type = validateResult.IsAutomationValid ? "QueuedTaskCreatedSsucceed" : "QueuedTaskCreatedFailed";

                                    if (validateResult.IsAutomationValid)
                                    {
                                        if (objectTable != null)
                                        {
                                            var entityPM = GetEntity(objectTable.Name, entityId, Tenant);
                                            if (entityPM != null)
                                            {
                                                new AutomationQueuedTaskResultService().AddAutomationQueuedTask(entityPM, entityChange, AutomationConditionFieldLists, automationLastUpdateDate, entityChangesAutomationsLists, automation, entityChangesAutomation, dateBefore);
                                                UpdateEntitiy(entityPM, objectTable.Name, entityChange.CreateByUserId, Tenant);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                                        entityChangesAutomationsLists.Add(entityChangesAutomation);
                                    }
                                    entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                                    entityChange.QueuedTaskAutomationSsucceedXml = entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
                                    entityChange.QueuedTaskAutomationFailedXml = entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList()) : "";



                                }
                                #endregion

                                #region Event
                                else if (automation.ResultCode == "EVENTCREATION")
                                {
                                    EventCreationAutomation(objectTable, new AutomationEventCreationArguments { EntityChange = entityChange, DateBefore = dateBefore, EntityChangeAutomation = entityChangesAutomation, ValidateResult = validateResult, EntityChangesAutomationsLists = entityChangesAutomationsLists, Automation = automation, EntityChangesAutomationsSsucceedList = entityChangesAutomationsLists});                                    
                                }
                                #endregion


                                entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                                ChangesAutomationsLists.Add(entityChangesAutomation);

                                if (automationList.Count() == ChangesAutomationsLists.Count())
                                {

                                    if (automationList.Where(d => d.ResultCode == "SENDINTERFACE").Any())
                                    {
                                        StorageDataArgs storageDataArgs = new StorageDataArgs() { FileName = (entityChange.Id + entityChange.EntityId + "Entity"), FolderName = "Others", Tenant = entityChange.Tenant };
                                        StorageDataService.DeleteFileFromStorage(storageDataArgs);
                                    }

                                    entityChange.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                                }

                                entityChangeRepository.Update(entityChange);
                                entityChangeRepository.SubmitChanges();

                                queueservice.Complete();
                                LogDoneItemInMemory();
                            }

                            catch (Exception ex)
                            {
                                #region HandleException
                                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Automation Queue worker role start", null, null);
                                if (response.MessageValues.Keys.Contains("EntityChangeId"))
                                {
                                    if (!string.IsNullOrEmpty(entityChangeId))
                                    {
                                        if (response.RetryNumber <= 1)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                        }

                                        if (response.RetryNumber > 1 && response.RetryNumber <= 2)
                                        {
                                            queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                        }
                                        if (response.RetryNumber >= 3)
                                        {
                                            queueservice.CompleteAsFailed();
                                        }
                                    }

                                    else
                                    {
                                        queueservice.CompleteAsFailed();
                                    }
                                }

                                else
                                {
                                    queueservice.CompleteAsFailed();
                                }
                                #endregion
                            }
                        }

                        else
                        {
                            Thread.Sleep(10000);
                        }
                    }

                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Automation Queue worker role start", null, null);
                        Thread.Sleep(10000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private void EventCreationAutomation(ObjectTable objectTable, AutomationEventCreationArguments automationEventCreationArguments)
        {
            var entityPM = GetEntity(objectTable.Name, entityId, Tenant);
            automationEventCreationArguments.EntityPM = entityPM;
            CreateEventCreationAutomation(automationEventCreationArguments);
            UpdateEntitiy(entityPM, objectTable.Name, automationEventCreationArguments.EntityChange.CreateByUserId, Tenant);
        }

        private void CreateEventCreationAutomation(AutomationEventCreationArguments automationEventCreationArguments)
        {
            automationEventCreationArguments.EntityChangeAutomation.type = automationEventCreationArguments.ValidateResult.IsAutomationValid ? "EventCreatedSucceed" : "EventCreatedFailed";
            automationEventCreationArguments.EntityChangeAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
            if (automationEventCreationArguments.ValidateResult.IsAutomationValid)
            {
                ApplyEventCreationAutomation(automationEventCreationArguments);
            }
            else
            {
                automationEventCreationArguments.EntityChangesAutomationsLists.Add(automationEventCreationArguments.EntityChangeAutomation);
            }

            automationEventCreationArguments.EntityChangeAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - automationEventCreationArguments.DateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
            automationEventCreationArguments.EntityChange.EventAutomationSsucceedXml = automationEventCreationArguments.EntityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(automationEventCreationArguments.EntityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
            automationEventCreationArguments.EntityChange.EventAutomationFailedXml = automationEventCreationArguments.EntityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(automationEventCreationArguments.EntityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList()) : "";
        }

        private static void ApplyEventCreationAutomation(AutomationEventCreationArguments automationEventCreationArguments)
        {
            new AutomationEventCreationService().CreateEvent(automationEventCreationArguments);
            MarkEntityChangeExecutedRecord(automationEventCreationArguments.EntityChange, automationEventCreationArguments.EntityChangeAutomation, automationEventCreationArguments.EntityChangesAutomationsLists);
        }

        private void ApplyAuomationSendInterfaceFTP(EntityChange entityChange, AutomationSendInterface automationSendInterface, string documentId)
        {
            FTPAutomationServiceArgs fTPAutomationServiceArgs = new FTPAutomationServiceArgs()
            {
                FTPDetails = automationSendInterface.FTPDetails,
                DocumentId = documentId,
                Tenant = Tenant,
                EntityId = entityId,
                ObjectTableId = entityChange.ObjectTableId,
                ComputingPartnerId = automationSendInterface.ComputingPartnerId,
                AdditionalFolderDetails = "/fromlogitude",
            };
            var ftpAutomationService = new FTPAutomationService(fTPAutomationServiceArgs);
            ftpAutomationService.Run();
        }

        private void ApplyAutomationSendInterfaceWebHook(EntityChange entityChange, AutomationSendInterface automationSendInterface, string documentId)
        {
            WebHookAutomationServiceArgs webHookAutomationServiceArgs = new WebHookAutomationServiceArgs()
            {
                WebHookDetails = automationSendInterface.WebHookDetails,
                DocumentId = documentId,
                Tenant = Tenant,
                EntityId = entityId,
                ObjectTableId = entityChange.ObjectTableId,
                ComputingPartnerId = automationSendInterface.ComputingPartnerId,
            };
            WebHookAutomationService webHookAutomationService = new WebHookAutomationService(webHookAutomationServiceArgs);
            webHookAutomationService.Run();
        }

        private void ApplyAutomationSendDocumentFTP(AutomationSendDocument automationSendDocument, AutomationDocumentResult automationDocumentResult, string documentFileName)
        {
            FTPAutomationServiceArgs fTPAutomationServiceArgs = new FTPAutomationServiceArgs()
            {
                FTPDetails = automationSendDocument.FTPDetails,
                DocumentId = automationDocumentResult.DocumentId,
                Tenant = Tenant,
                EntityId = entityId,
                ObjectTableId = automationDocumentResult.ObjectTableId,
                DocumentFileName = documentFileName,
            };
            var ftpAutomationService = new FTPAutomationService(fTPAutomationServiceArgs);
            ftpAutomationService.Run();
        }


        private string GetentityChangesResultCode(string resultCode)
        {
            string result = string.Empty;
            if (resultCode == "EMAIL") result = "E-mail";
            else if (resultCode == "FIELDSET") result = "Set Fields Value";
            else if (resultCode == "FOLLOWUP") result = "F/U Creation";
            else if (resultCode == "DOCOUTFOLLOWUP") result = "Docs Out F/U Creation";
            else if (resultCode == "DOCINFOLLOWUP") result = "Docs In F/U Creation";
            else if (resultCode == "QUEUE") result = "Queued Task";
            else if (resultCode == "SENDINTERFACE") result = "Send Interface";
            else if (resultCode == "SENDDOCUMENT") result = "Documents Send";
            else if (resultCode == "CREATETASK") result = "Create Task";

            return result;

        }

        private static string GetSendInterfaceDataContractDocumentId(EntityChange entityChange, AutomationSendInterface automationSendInterface)
        {
            StorageDataArgs storageDataArgs = new StorageDataArgs() { FileName = (entityChange.Id + entityChange.EntityId + "Entity"), FolderName = "Others", Tenant = entityChange.Tenant };
            byte[] objectData = StorageDataService.ReadFileFromStorage(storageDataArgs);
            ShipmentPM shipmentPM = LogitudeXmlSerializer.DeserializeObject<ShipmentPM>(objectData);
            SendInterfaceDataContractService sendInterfaceDataContractService = new SendInterfaceDataContractService(shipmentPM, automationSendInterface, entityChange.Tenant);
            string documentId = sendInterfaceDataContractService.GetDataContractDocumentId(automationSendInterface.Format);
            return documentId;
        }

        private static string GetDocumentFileName(EntityChange entityChange, Automation automation, AutomationDocumentResult automationDocumentResult)
        {
            SendDocumentDataExernalService sendDocumentDataExernalService = new SendDocumentDataExernalService(entityChange, automation, automationDocumentResult);
            string documentName = sendDocumentDataExernalService.GetDocumentFileName();
            return documentName;
        }

        private static void MarkEntityChangeExecutedRecord(EntityChange entityChange, EntityChangeAutomation entityChangesAutomation, List<EntityChangeAutomation> entityChangesAutomationsLists)
        {
            entityChange.HasExecutedRecord = true;
            entityChangesAutomation.IsConditionTrue = true;
            entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
            entityChangesAutomationsLists.Add(entityChangesAutomation);
        }


        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("AutomationQueue", Tenant);
            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }
        }

        object EntityRepsitory = null;
        public object GetEntity(string entityName, string entityId, int tenant)
        {
            if (entityName == "Master")
            {
                entityName = "Shipment";
            }

            Assembly blAssembly = Assembly.Load("Logitude.BL");
            object entityQuery = null;
            object theEntity = null;
            string typePath = "Logitude.BL.ShipmentsModel.EntityQueries." + entityName + "Query";
            string pmtypePath = "Logitude.BL.ShipmentsModel.EntityPMs." + entityName + "PM";

            Type pmtype = blAssembly.GetType(pmtypePath);
            Type type = blAssembly.GetType(typePath);

            if (type == null)
            {
                typePath = "Logitude.BL.CommonDataModel.EntityQueries." + entityName + "Query";
                type = blAssembly.GetType(typePath);

                pmtypePath = "Logitude.BL.CommonDataModel.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }

            if (type == null)
            {
                typePath = "Logitude.BL." + entityName + "Query";
                type = blAssembly.GetType(typePath);

                pmtypePath = "Logitude.BL." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }

            if (type == null)
            {
                typePath = "Logitude.BL.InfrastructureModel.EntityQueries." + entityName + "Query";
                type = blAssembly.GetType(typePath);

                pmtypePath = "Logitude.BL.InfrastructureModel.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }

            if (type == null)
            {
                typePath = "Logitude.BL.QuoteModel.EntityQueries." + entityName + "Query";
                type = blAssembly.GetType(typePath);

                pmtypePath = "Logitude.BL.QuoteModel.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }

            if (type == null)
            {
                typePath = "Logitude.BL.InvoiceModel.EntityQueries." + entityName + "Query";
                type = blAssembly.GetType(typePath);

                pmtypePath = "Logitude.BL.InvoiceModel.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }

            if (type == null)
            {
                Assembly assembly = Assembly.Load("Logitude.CRM.BL");
                typePath = "Logitude.CRM.BL.EntityQueryServices." + entityName + "QueryService";
                type = assembly.GetType(typePath);

                pmtypePath = "Logitude.CRM.BL.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }

            if (type == null)
            {
                Assembly assembly = Assembly.Load("Logitude.Customs.BL");
                typePath = "Logitude.Customs.BL.EntityQueryServices." + entityName + "QueryService";
                type = assembly.GetType(typePath);

                pmtypePath = "Logitude.Customs.Def.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }

            if (type == null)
            {
                Assembly assembly = Assembly.Load("Logitude.BookingLib.BL");
                typePath = "Logitude.BookingLib.BL.EntityQueryServices." + entityName + "QueryService";
                type = assembly.GetType(typePath);

                pmtypePath = "Logitude.BookingLib.BL.EntityPMs." + entityName + "PM";
                pmtype = blAssembly.GetType(typePath);
            }

            if (type != null)
            {
                entityQuery = Activator.CreateInstance(type, tenant);
                MethodInfo methodInfo = null;

                if (entityName == "Shipment")
                {
                    methodInfo = entityQuery.GetType().GetMethod("GetSinglePMWithLists");
                }
                else
                {
                    methodInfo = entityQuery.GetType().GetMethod("GetSinglePM");

                    if (methodInfo == null)
                    {
                        methodInfo = entityQuery.GetType().GetMethod("GetSingle");
                    }
                    if (methodInfo == null)
                    {
                        methodInfo = entityQuery.GetType().GetMethod("GetSingle" + entityName + "PM");
                    }
                }

                ParameterInfo[] methodParameters = methodInfo.GetParameters();
                object[] parameters = new object[] { };
                switch (methodParameters.Count())
                {
                    case 1:
                        parameters = new object[] { entityId };
                        break;
                    case 2:
                        parameters = new object[] { entityId, tenant };
                        break;
                    case 3:
                        parameters = new object[] { entityId, true, false };
                        break;
                }

                if (methodInfo != null)
                {
                    theEntity = methodInfo.Invoke(entityQuery, parameters);
                }
                else
                {
                    throw new Exception("GetSingle" + entityName + "PM" + "not found!");
                }
            }

            if (theEntity == null && pmtype != null)
            {
                theEntity = Activator.CreateInstance(pmtype);
            }

            return theEntity;
        }

        public void UpdateEntitiy(object theEntity, string tableName, string userId, int tenant)
        {
            if (tableName == "Master") tableName = "Shipment";


            if (tableName == "Ticket")
            {
                TicketPM ticketPM = (TicketPM)theEntity;
                ICRMContext MyContext = CRMContext.GetContext(ticketPM.Tenant);
                TicketUpdateService service = new TicketUpdateService(MyContext, new Dictionary<string, IContext>(), ticketPM.Tenant);
                ticketPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                ticketPM.IsUpdateByAutomation = true;
                service.Update(ticketPM, true);
            }

            else if (tableName == "Shipment")
            {
                ShipmentPM shipmentPM = (ShipmentPM)theEntity;
                IShipmentsContext objectContext = ShipmentsContext.GetContext(0);

                ContactQuery contactQuery = new ContactQuery(tenant);
                string email = contactQuery.GetContactEmailById(userId, tenant);
                if (string.IsNullOrEmpty(email))
                {
                    email = "system@tenant" + shipmentPM.Tenant + ".com";
                }
                shipmentPM.IsUpdateByAutomation = true;
                ShipmentService service = new ShipmentService(objectContext, shipmentPM, email);
                service.Update(true);
            }

            else if (tableName == "Container")
            {
                UpdateContainer(theEntity);
            }
        }

        private static void UpdateContainer(object theEntity)
        {
            ContainerPM containerPM = (ContainerPM)theEntity;
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(0);
            ContainerService service = new ContainerService(shipmentsContext, containerPM.Tenant);
            containerPM.IsUpdateByAutomation = true;
            service.Update(containerPM);
        }

        public void SubmitChanges()
        {
            if (EntityRepsitory != null)
            {
                MethodInfo methodInfo = EntityRepsitory.GetType().GetMethod("SubmitChanges");
                if (methodInfo != null)
                {
                    ParameterInfo[] parametersInfo = methodInfo.GetParameters();
                    object[] parameters = new object[] { };
                    parameters = new object[] { };

                    if (methodInfo != null)
                    {
                        methodInfo.Invoke(EntityRepsitory, parameters);
                    }

                    else
                    {
                        throw new Exception("SubmitChanges" + "not found!");
                    }
                }
            }

            EntityRepsitory = null;
        }

        public List<EntityChangeAutomation> FullEntityChangeAutomationList(EntityChange entityChange, List<EntityChangeAutomation> changeAutomationList)
        {
            List<EntityChangeAutomation> entityChangeAutomationList = new List<EntityChangeAutomation>();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.EmailAutomationSsucceedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.EmailAutomationFailedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.SetAutomationSsucceedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.SetAutomationFailedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.FollowUpAutomationSsucceedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.FollowUpAutomationFailedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.SetSLAAutomationSsucceedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.SetSLAAutomationFailedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.QueuedTaskAutomationSsucceedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.QueuedTaskAutomationFailedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.SendInterfaceAutomationSsucceedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.SendInterfaceAutomationFailedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.SendDocumentAutomationSsucceedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.SendDocumentAutomationFailedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.CreateTaskAutomationSsucceedXml)).ToList();
            entityChangeAutomationList = entityChangeAutomationList.Concat(GetEntityChangeAutomationList(entityChange.CreateTaskAutomationFailedXml)).ToList();



            return entityChangeAutomationList;
        }

        private List<EntityChangeAutomation> GetEntityChangeAutomationList(string entityChangeAutomationXml)
        {
            List<EntityChangeAutomation> entityChangeAutomationList = new List<EntityChangeAutomation>();
            if (!string.IsNullOrEmpty(entityChangeAutomationXml))
            {
                var lists = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChangeAutomationXml);
                foreach (EntityChangeAutomation item in lists)
                {
                    entityChangeAutomationList.Add(item);
                }
            }
            return entityChangeAutomationList;
        }


    }
}