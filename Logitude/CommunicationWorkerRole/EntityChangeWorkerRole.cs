using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole
{
    class EntityChangeWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
        string entityChangeId = string.Empty;
        string type = string.Empty;
        bool IsDelayAutomation = false;
        int tenant = 0;

        public EntityChangeWorkerRole()
        {
        
        }
        
        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "EntityChange";
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
                        queueservice.InitializeQueue("entitychangequeue", 0);
                        var response = queueservice.Receive();
                        string tenantString = null;
                        if (response != null && response.MessageId != null)
                        {
                            try
                            {
                                entityChangeId = response.MessageValues.Keys.Contains("EntityChangeId") ? response.MessageValues["EntityChangeId"].ToString() : "";
                                type = response.MessageValues.Keys.Contains("Type") ? response.MessageValues["Type"].ToString() : "";
                                string delayAutomation = response.MessageValues.Keys.Contains("IsDelayAutomation") ? response.MessageValues["IsDelayAutomation"].ToString() : "";
                                if (!string.IsNullOrEmpty(delayAutomation) && delayAutomation == "true")
                                {
                                    this.IsDelayAutomation = true;
                                }

                                if (response.MessageValues.Keys.Contains("Tenant"))
                                {
                                    tenantString = response.MessageValues["Tenant"].ToString();
                                    if (!string.IsNullOrEmpty(tenantString)) tenant = int.Parse(tenantString);
                                }

                                if (string.IsNullOrEmpty(entityChangeId) || string.IsNullOrEmpty(tenantString))
                                {
                                    queueservice.Complete();
                                    continue;
                                }

                                DateTime startDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                EntityChangeRepository entityChangeRepository = new EntityChangeRepository(tenant);
                                EntityChange entityChange = entityChangeRepository.GetSingleEntityChange(entityChangeId, tenant);

                                if (entityChange == null)
                                {
                                    queueservice.Complete();
                                    continue;
                                }

                                List<Field> AutomationConditionFieldLists = null;
                                AutomationConditionFields automationConditionFields = new AutomationConditionFields();

                                if (!string.IsNullOrEmpty(entityChange.AutomationConditionFieldsXml))
                                {
                                    automationConditionFields = LogitudeXmlSerializer.DeserializeObject<AutomationConditionFields>(entityChange.AutomationConditionFieldsXml);
                                    AutomationConditionFieldLists = automationConditionFields.Fields;
                                }

                                AutomationRepository automationRepository = new AutomationRepository(tenant);

                                string objectTableId = automationConditionFields.ObjectTableId;
                                if (string.IsNullOrEmpty(objectTableId)) objectTableId = entityChange.ObjectTableId;
                                string otherObjectTableLastUpdateDate = string.Empty;
                                string otherObjectTableId = string.Empty;
                                List<Automation> AutomationsList = new List<Automation>();

                                if (automationConditionFields.LastUpdateDate != null)
                                {
                                    AutomationsList = automationRepository.GetAutomationsByObjectTableId(objectTableId, tenant, automationConditionFields.LastUpdateDate.ToString()).Where(d => d.Type == type && d.ResultCode == "EMAIL").OrderBy(d => d.Order).ToList();
                                }

                                if (automationConditionFields.IsRunMasterHouseAutomation)
                                {
                                    otherObjectTableId = automationConditionFields.OtherObjectTableIdWithLastUpdate.Split('@')[0];
                                    otherObjectTableLastUpdateDate = automationConditionFields.OtherObjectTableIdWithLastUpdate.Split('@')[1];

                                    List<Automation> otherAutomations = automationRepository.GetAutomationsByObjectTableId(otherObjectTableId, tenant, otherObjectTableLastUpdateDate).Where(d => d.Type == type && d.ResultCode == "EMAIL").OrderBy(d => d.Order).ToList();

                                    foreach (Automation item in otherAutomations)
                                    {
                                        AutomationsList.Add(item);
                                    }
                                }

                                if (AutomationsList.Count == 0)
                                {
                                    queueservice.Complete();
                                    continue;
                                }

                                List<EntityChangeAutomation> EntityChangesAutomationsFailedList = new List<EntityChangeAutomation>();
                                List<EntityChangeAutomation> EntityChangesAutomationsSsucceedList = new List<EntityChangeAutomation>();
                                EntityChangeHelper entityChangeHelper = new EntityChangeHelper();
                                AutomationHistoryQuery automationHistoryQuery = new AutomationHistoryQuery(tenant);

                                bool IsEntityChageContainAnyDelay = false;
                                foreach (Automation automation in AutomationsList)
                                {
                                    DateTime dateBefore = DateTime.Now;
                                    EntityChangeAutomation entityChangesAutomation = new EntityChangeAutomation()
                                    {
                                        Id = IdCounter.GetNumber("EntityChangeAutomation", tenant),
                                        AutomationId = automation.Id,
                                        AutomationType = automation.Type,
                                        CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                                        AutomationName = automation.Name,
                                        AutomationDescription = automation.Description,
                                    };

                                    entityChangesAutomation.ResultCode = "E-mail";

                                    string dateString = "";
                                    if (automationConditionFields.LastUpdateDate != null)
                                    {
                                        dateString = automationConditionFields.LastUpdateDate.ToString();
                                    }

                                    if (!string.IsNullOrEmpty(otherObjectTableId) && automation.ObjectTableId == otherObjectTableId) dateString = otherObjectTableLastUpdateDate;

                                    ValidateAutomationResultClass validateResult = entityChangeHelper.ValidateAutomation(automation, entityChange, AutomationConditionFieldLists, dateString, "");
                                    entityChangesAutomation.type = validateResult.IsAutomationValid ? "EmailSsucceed" : "EmailFailed";

                                    if (validateResult.IsAutomationValid)
                                    {
                                        if (validateResult.Type == "Delayed")
                                        {
                                            IsEntityChageContainAnyDelay = true;
                                            entityChangeHelper.AddDelayedAutomationQueue(entityChange.Id, type, automation.Tenant, automation.Id, validateResult.Delaytime, validateResult.DelaytimeIndicator, entityChange.EntityId);
                                        }
                                        else
                                        {
                                            AutomationHelper automationHelper = new AutomationHelper();
                                            automationHelper.ExecuteEmailAutomation(entityChange, AutomationConditionFieldLists, automation, entityChangesAutomation, EntityChangesAutomationsSsucceedList);
                                            entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                                        }
                                    }

                                    else
                                    {
                                        entityChangesAutomation.IsConditionTrue = false;
                                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                        entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                                        EntityChangesAutomationsFailedList.Add(entityChangesAutomation);
                                    }
                                }

                                entityChange.EmailAutomationFailedXml = EntityChangesAutomationsFailedList.Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsFailedList) : "";
                                entityChange.EmailAutomationSsucceedXml = EntityChangesAutomationsSsucceedList.Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsSsucceedList) : "";
                                if (!IsEntityChageContainAnyDelay && !IsDelayAutomation) entityChange.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                                entityChangeRepository.Update(entityChange);
                                entityChangeRepository.SubmitChanges();

                                queueservice.Complete();
                                LogDoneItemInMemory();
                            }

                            catch (Exception ex)
                            {
                                #region HandleException
                                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Entity Change Queue worker role start", null, null);
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
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Entity Change Queue worker role start", null, null);
                        Thread.Sleep(10000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }
        
        private void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService();
                queueservice.InitializeQueue("entitychangequeue", tenant);
            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Entity Change worker role start", null, null);
            }
        }
    }
}
