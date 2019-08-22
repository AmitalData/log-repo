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

namespace CommunicationWorkerRole
{
    class DelayAutomationWorkerRole : WorkerEntryPoint
    {
        IQueueService queueservice;
        int Tenant = 0;

        string entityChangeId = string.Empty;
        string automationId = string.Empty;
        string entityId = string.Empty;
        int AutomationCount = 0;
        string type = string.Empty;
        public DelayAutomationWorkerRole(string tenant)
        {
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DelayAutomation";
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
                        queueservice.InitializeQueue("DelayAutomationQueue", 0);
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

                                EntityChangeHelper entityChangeHelper = new EntityChangeHelper();
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

                                if (automation.ResultCode == "EMAIL")
                                {
                                    entityChangesAutomation.ResultCode = "E-mail";
                                }

                                else if (automation.ResultCode == "FIELDSET")
                                {
                                    entityChangesAutomation.ResultCode = "Set Fields Value";
                                }

                                else if (automation.ResultCode == "FOLLOWUP")
                                {
                                    entityChangesAutomation.ResultCode = "F/U Creation";
                                }

                                else if (automation.ResultCode == "DOCOUTFOLLOWUP")
                                {
                                    entityChangesAutomation.ResultCode = "Docs Out F/U Creation";
                                }

                                else if (automation.ResultCode == "DOCINFOLLOWUP")
                                {
                                    entityChangesAutomation.ResultCode = "Docs In F/U Creation";
                                }

                                else if (automation.ResultCode == "QUEUE")
                                {
                                    entityChangesAutomation.ResultCode = "Queued Task";
                                }

                                string dateString = "";
                                if (automationConditionFields.LastUpdateDate != null)
                                {
                                    dateString = automationConditionFields.LastUpdateDate.ToString();
                                }

                                if (!string.IsNullOrEmpty(otherObjectTableId) && automation.ObjectTableId == otherObjectTableId)
                                {
                                    dateString = otherObjectTableLastUpdateDate;
                                }

                                ValidateAutomationResultClass validateResult = entityChangeHelper.ValidateAutomation(automation, entityChange, AutomationConditionFieldLists, dateString, "Delayed");
                                List<EntityChangeAutomation> ChangesAutomationsLists = FullEntityChangeAutomationList(entityChange, null);

                                List<EntityChangeAutomation> entityChangesAutomationsLists = ChangesAutomationsLists.Where(d => d.ResultCode == entityChangesAutomation.ResultCode).ToList();
                                entityChangesAutomation.IsConditionTrue = validateResult.IsAutomationValid;
                                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(Tenant);
                                ObjectTable objectTable = objectTabelRepository.GetSingleObjectTable(entityChange.ObjectTableId, Tenant, true);

                                #region E-mail
                                if (automation.ResultCode == "EMAIL")
                                {
                                    entityChangesAutomation.type = validateResult.IsAutomationValid ? "EmailSsucceed" : "EmailFailed";

                                    if (validateResult.IsAutomationValid)
                                    {
                                        string objectTableName = objectTable != null ? objectTable.Name : "";
                                        AutomationHelper automationHelper = new AutomationHelper();
                                        automationHelper.ExecuteEmailAutomation(entityChange, AutomationConditionFieldLists, automation, entityChangesAutomation, entityChangesAutomationsLists, objectTableName);
                                        entityChange.EmailAutomationSsucceedXml = entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
                                    }
                                    else
                                    {
                                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                                        entityChangesAutomationsLists.Add(entityChangesAutomation);
                                        entityChange.EmailAutomationFailedXml = entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList()) : "";

                                    }
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

                                                entityChangeHelper.SetValue(entityPM, entityChange, AutomationConditionFieldLists, dateString, entityChangesAutomationsLists, changesFields, automation, entityChangesAutomation, dateBefore);
                                                UpdateEntitiy(entityPM, objectTable.Name, entityChange.CreateByUserId, Tenant);
                                                rFields.cs = changesFields;
                                                entityChange.ChangesAutomationFieldsXml = rFields.cs != null && rFields.cs.Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(rFields) : "";
                                            }
                                        }

                                        entityChange.SetAutomationSsucceedXml = entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
                                    }
                                    else
                                    {
                                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                                        entityChangesAutomationsLists.Add(entityChangesAutomation);
                                        entityChange.SetAutomationFailedXml = entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList()) : "";
                                    }
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
                                                entityChangeHelper.AddAutomationFollowUp(entityPM, entityChange, AutomationConditionFieldLists, dateString, entityChangesAutomationsLists, automation, entityChangesAutomation, dateBefore);
                                               // UpdateEntitiy(entityPM, objectTable.Name, entityChange.CreateByUserId, Tenant);
                                            }
                                        }

                                        entityChange.FollowUpAutomationSsucceedXml = entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
                                    }
                                    else
                                    {
                                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                                        entityChangesAutomationsLists.Add(entityChangesAutomation);
                                        entityChange.FollowUpAutomationFailedXml = entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
                                    }
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
                                                entityChangeHelper.AddAutomationQueuedTask(entityPM, entityChange, AutomationConditionFieldLists, dateString, entityChangesAutomationsLists, automation, entityChangesAutomation, dateBefore);
                                                UpdateEntitiy(entityPM, objectTable.Name, entityChange.CreateByUserId, Tenant);
                                            }
                                        }

                                        entityChange.QueuedTaskAutomationSsucceedXml = entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
                                    }
                                    else
                                    {
                                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                                        entityChangesAutomationsLists.Add(entityChangesAutomation);
                                        entityChange.QueuedTaskAutomationFailedXml = entityChangesAutomationsLists.Where(d => !d.IsConditionTrue).ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(entityChangesAutomationsLists.Where(d => d.IsConditionTrue).ToList()) : "";
                                    }
                                }
                                #endregion

                                entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                                ChangesAutomationsLists.Add(entityChangesAutomation);

                                if (automationList.Count() == ChangesAutomationsLists.Count())
                                {
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
                                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Delay Automation Queue worker role start", null, null);
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
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Delay Automation Queue worker role start", null, null);
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
                queueservice.InitializeQueue("DelayAutomationQueue", Tenant);
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

            if (!string.IsNullOrEmpty(entityChange.EmailAutomationSsucceedXml))
            {
                var lists = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChange.EmailAutomationSsucceedXml);
                foreach (EntityChangeAutomation item in lists)
                {
                    entityChangeAutomationList.Add(item);
                }
            }

            if (!string.IsNullOrEmpty(entityChange.EmailAutomationFailedXml))
            {
                var lists = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChange.EmailAutomationFailedXml);
                foreach (EntityChangeAutomation item in lists)
                {
                    entityChangeAutomationList.Add(item);
                }
            }

            if (!string.IsNullOrEmpty(entityChange.SetAutomationSsucceedXml))
            {
                var lists = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChange.SetAutomationSsucceedXml);
                foreach (EntityChangeAutomation item in lists)
                {
                    entityChangeAutomationList.Add(item);
                }
            }

            if (!string.IsNullOrEmpty(entityChange.SetAutomationFailedXml))
            {
                var lists = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChange.SetAutomationFailedXml);
                foreach (EntityChangeAutomation item in lists)
                {
                    entityChangeAutomationList.Add(item);
                }
            }

            if (!string.IsNullOrEmpty(entityChange.FollowUpAutomationSsucceedXml))
            {
                var lists = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChange.FollowUpAutomationSsucceedXml);
                foreach (EntityChangeAutomation item in lists)
                {
                    entityChangeAutomationList.Add(item);
                }
            }

            if (!string.IsNullOrEmpty(entityChange.FollowUpAutomationFailedXml))
            {
                var lists = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChange.FollowUpAutomationFailedXml);
                foreach (EntityChangeAutomation item in lists)
                {
                    entityChangeAutomationList.Add(item);
                }
            }

            if (!string.IsNullOrEmpty(entityChange.SetSLAAutomationSsucceedXml))
            {
                var lists = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChange.SetSLAAutomationSsucceedXml);
                foreach (EntityChangeAutomation item in lists)
                {
                    entityChangeAutomationList.Add(item);
                }
            }

            if (!string.IsNullOrEmpty(entityChange.SetSLAAutomationFailedXml))
            {
                var lists = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChange.SetSLAAutomationFailedXml);
                foreach (EntityChangeAutomation item in lists)
                {
                    entityChangeAutomationList.Add(item);
                }
            }

            if (!string.IsNullOrEmpty(entityChange.QueuedTaskAutomationSsucceedXml))
            {
                var lists = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChange.QueuedTaskAutomationSsucceedXml);
                foreach (EntityChangeAutomation item in lists)
                {
                    entityChangeAutomationList.Add(item);
                }
            }

            if (!string.IsNullOrEmpty(entityChange.QueuedTaskAutomationFailedXml))
            {
                var lists = LogitudeXmlSerializer.DeserializeObject<List<EntityChangeAutomation>>(entityChange.QueuedTaskAutomationFailedXml);
                foreach (EntityChangeAutomation item in lists)
                {
                    entityChangeAutomationList.Add(item);
                }
            }

            return entityChangeAutomationList;
        }
    }
}