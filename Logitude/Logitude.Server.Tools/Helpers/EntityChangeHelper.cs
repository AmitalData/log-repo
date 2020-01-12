using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Logitude.Server.Tools.Helpers
{
    public class EntityChangeHelper
    {
        #region Add Entity Change To Queue
        private void AddEntityChangeQueue(string entityChangeId, string type, int tenant)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("entitychangequeue", tenant);
            queueservice.Send(new Dictionary<string, string>() { { "EntityChangeId", entityChangeId }, { "Tenant", tenant.ToString() }, { "Type", type }, { "IsDelayAutomation", IsDelayAutomation.ToString().ToLower() } }, null, null, null, null);
        }
        #endregion

        #region Add Delayed Automation To Queue
        public void AddDelayedAutomationQueue(string entityChangeId, string type, int tenant, string automationId, int delay, string delaytimeIndicator, string entityId)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("DelayAutomationQueue", tenant);

            if (delaytimeIndicator == "OO" && delay != 0) delay = delay * 60;

            DateTime nextRunDate = DateTime.UtcNow.AddMinutes((double)delay);
            TimeSpan delayTime = nextRunDate - DateTime.UtcNow;
            queueservice.Send(new Dictionary<string, string>() { { "EntityChangeId", entityChangeId }, { "Tenant", tenant.ToString() }, { "Type", type }, { "EntityId", entityId }, { "AutomationId", automationId } }, delayTime, null, null, null);
        }
        #endregion

        #region Add Entity Change

        List<EntityChangeAutomation> EntityChangesAutomationsFailedList = new List<EntityChangeAutomation>();
        List<EntityChangeAutomation> EntityChangesAutomationsSsucceedList = new List<EntityChangeAutomation>();

        List<c> Changefields = new List<c>();
        bool IsDelayAutomation = false;
        public bool IsChangeSLA = false;
        public Object ExternalEntity = null;
        public void AddEntityChange(Object entityPM, Object oldentityPM, string processtype, string entityChangeFieldXml, string tableName, DateTime? startDate = null)
        {
            DateTime dateBefore = DateTime.Now;
            if (startDate != null) dateBefore = (DateTime)startDate;
            EntityChangesAutomationsFailedList = new List<EntityChangeAutomation>();
            EntityChangesAutomationsSsucceedList = new List<EntityChangeAutomation>();

            ObjectTable shipmentobjectTable = null;
            string OtherObjectTableIdWithLastUpdate = string.Empty;
            bool IsRunMasterHouseAutomation = false;

            Type type = entityPM.GetType();
            Object value = null;
            int tenant = 0;
            string entityId = string.Empty;

            //Tenant
            value = GetPropertyValue(entityPM, type, "Tenant");
            if (value != null) tenant = (int)value;

            //Entity
            value = GetPropertyValue(entityPM, type, "Id");
            if (value != null) entityId = value.ToString();

            if (tableName == "MasterAndHouse")
            {
                tableName = "Master";
                IsRunMasterHouseAutomation = true;
            }

            EntityChangeRepository entityChangeRepository = new EntityChangeRepository(tenant);
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(tableName, tenant, true);

            if (tableName == "Master" || IsRunMasterHouseAutomation)
            {
                shipmentobjectTable = objectTabelRepository.GetObjectTableByName("Shipment", tenant, true);
            }

            AutomationLastUpdateRepository automationLastUpdateRepository = new AutomationLastUpdateRepository(tenant);
            AutomationLastUpdate automationLastUpdate = automationLastUpdateRepository.GetSingleAutomationLastUpdate(objectTable.Id, tenant);

            AutomationLastUpdate otherAutomationLastUpdate = null;
            if (IsRunMasterHouseAutomation)
            {
                otherAutomationLastUpdate = automationLastUpdateRepository.GetSingleAutomationLastUpdate(shipmentobjectTable.Id, tenant);
            }

            EntityChange entityChange = CreateEntityChange(entityChangeFieldXml, shipmentobjectTable, tenant, entityId, objectTable);
            entityChangeRepository.Add(entityChange);

            if (automationLastUpdate != null || otherAutomationLastUpdate != null)
            {
                AutomationRepository automationRepository = new AutomationRepository(tenant);
                List<Automation> otherAutomations = null;
                List<Automation> automations = new List<Automation>();

                if (automationLastUpdate != null)
                {
                    automations = automationRepository.GetAutomationsByObjectTableId(objectTable.Id, tenant, automationLastUpdate.LastUpdateDate.ToString()).Where(d => d.Type == processtype).OrderBy(d => d.Order).ToList();
                }

                if (IsRunMasterHouseAutomation && otherAutomationLastUpdate != null)
                {
                    otherAutomations = automationRepository.GetAutomationsByObjectTableId(shipmentobjectTable.Id, tenant, otherAutomationLastUpdate.LastUpdateDate.ToString()).Where(d => d.Type == processtype).OrderBy(d => d.Order).ToList();
                }

                List<ObjectField> automationsObjectFieldLists = new List<ObjectField>();
                List<ObjectField> otherAutomationsObjectFieldLists = null;
                string lastUpdateDate = automationLastUpdate != null ? automationLastUpdate.LastUpdateDate.ToString() : "";

                if ((automations != null && automations.Count > 0) || (otherAutomations != null && otherAutomations.Count > 0))
                {
                    #region Build Automation Object Field Lists

                    string tableId = objectTable.Name == "Master" ? shipmentobjectTable.Id : objectTable.Id;

                    if (automationLastUpdate != null && automations.Count > 0)
                    {
                        automationsObjectFieldLists = BuildAutomationObjectFieldLists(automationsObjectFieldLists, automations, objectTable.Id, automationLastUpdate.LastUpdateDate.ToString(), tenant, processtype, tableId);
                    }

                    if (IsRunMasterHouseAutomation && otherAutomationLastUpdate != null && otherAutomations != null && otherAutomations.Count > 0)
                    {
                        otherAutomationsObjectFieldLists = BuildAutomationObjectFieldLists(otherAutomationsObjectFieldLists, otherAutomations, shipmentobjectTable.Id, otherAutomationLastUpdate.LastUpdateDate.ToString(), tenant, processtype);
                    }

                    #endregion

                    #region OtherAutomations
                    if (IsRunMasterHouseAutomation)
                    {
                        if (otherAutomations != null) automations = automations.Concat(otherAutomations).ToList();
                        if (otherAutomationsObjectFieldLists != null)
                        {
                            foreach (ObjectField item in otherAutomationsObjectFieldLists)
                            {
                                if (!automationsObjectFieldLists.Contains(item)) automationsObjectFieldLists.Add(item);
                            }
                        }

                        if (otherAutomationLastUpdate != null && IsRunMasterHouseAutomation)
                        {
                            OtherObjectTableIdWithLastUpdate = shipmentobjectTable.Id + "@" + otherAutomationLastUpdate.LastUpdateDate.ToString();
                        }
                    }
                    #endregion

                    #region Bluid EntityChanges
                    List<Field> automationConditionFieldLists = new List<Field>();
                    AutomationConditionFields automationConditionFields = new AutomationConditionFields();

                    foreach (ObjectField objectField in automationsObjectFieldLists.Where(d => d.ObjectTableId == tableId))
                    {
                        string objectTableName = shipmentobjectTable != null ? shipmentobjectTable.Name : objectTable != null ? objectTable.Name : "";
                        string currentvalue = GetValue(entityPM, objectField);
                        string oldvalue = "";
                        bool ischange = false;

                        if (oldentityPM != null)
                        {
                            oldvalue = GetValue(oldentityPM, objectField);
                            if (currentvalue != oldvalue) ischange = true;
                        }

                        if (objectTable.Name == "Shipment" || objectTable.Name == "Master")
                        {
                            if (objectField.FieldName == "AgentContactId")
                            {
                                if (string.IsNullOrEmpty(currentvalue))
                                {
                                    var shipmentLevelObjectField = automationsObjectFieldLists.Where(d => d.FieldName == "ShipmentLevelCode").FirstOrDefault();
                                    string shipmentLevelCode = GetValue(entityPM, shipmentLevelObjectField);
                                    if (shipmentLevelCode == "H")
                                    {
                                        PropertyInfo propInfo = entityPM.GetType().GetProperty("MasterShipmentDataId");
                                        if (propInfo != null)
                                        {
                                            var masterShipmentDataId = propInfo.GetValue(entityPM);
                                            if (masterShipmentDataId != null)
                                            {
                                                if (!string.IsNullOrEmpty(masterShipmentDataId.ToString()))
                                                {
                                                    ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                                                    currentvalue = shipmentRepository.GetAgentContactByShipemntId(masterShipmentDataId.ToString(), tenant);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        Field automationConditionField = new Field()
                        {
                            Id = objectField.Id,
                            Value = currentvalue != null ? currentvalue : "",
                            OldValue = oldvalue != null ? oldvalue : "",
                            IsChange = ischange,
                            PropertyName = objectField.FieldName,
                        };

                        automationConditionFieldLists.Add(automationConditionField);
                    }
                    List<Field> externalEntityAutomationConditionFieldLists = GetExternalEntityAutomationConditionFieldLists(automationsObjectFieldLists, automationConditionFieldLists, tenant);
                    if (externalEntityAutomationConditionFieldLists.Count() > 0)
                    {
                        automationConditionFieldLists = automationConditionFieldLists.Concat(externalEntityAutomationConditionFieldLists).ToList();
                    }

                    automationConditionFields.Fields = automationConditionFieldLists;
                    automationConditionFields.LastUpdateDate = automationLastUpdate != null ? automationLastUpdate.LastUpdateDate : null;
                    automationConditionFields.ObjectTableId = objectTable.Id;
                    automationConditionFields.OtherObjectTableIdWithLastUpdate = OtherObjectTableIdWithLastUpdate;
                    automationConditionFields.IsRunMasterHouseAutomation = IsRunMasterHouseAutomation;
                    entityChange.AutomationConditionFieldsXml = LogitudeXmlSerializer.SerializeObjectToXmlString(automationConditionFields);
                    #endregion

                    #region Email

                    List<Automation> emailAutomations = automations.Where(d => d.ResultCode == "EMAIL").ToList();
                    if (emailAutomations.Count > 0)
                    {
                        AddEntityChangeQueue(entityChange.Id, processtype, entityChange.Tenant);
                    }

                    #endregion

                    #region FollowUp
                    List<Automation> followUpautomationsList = automations.Where(d => d.ResultCode == "FOLLOWUP" || d.ResultCode == "DOCOUTFOLLOWUP" || d.ResultCode == "DOCINFOLLOWUP").ToList();
                    if (followUpautomationsList.Count > 0)
                    {
                        ApplyFollowUpAutomation(entityPM, followUpautomationsList, entityChange, automationConditionFieldLists, lastUpdateDate, oldentityPM, processtype, entityId, OtherObjectTableIdWithLastUpdate);
                    }
                    #endregion

                    #region SetValue
                    List<Automation> fieldSetAutomationsList = automations.Where(d => d.ResultCode == "FIELDSET").ToList();
                    if (fieldSetAutomationsList.Count > 0)
                    {
                        ApplySetValueAutomation(entityPM, fieldSetAutomationsList, entityChange, automationConditionFieldLists, lastUpdateDate, oldentityPM, processtype, entityId, OtherObjectTableIdWithLastUpdate);
                    }
                    #endregion

                    #region Set SLA
                    List<Automation> setSLAAutomationsList = automations.Where(d => d.ResultCode == "SETSLA").ToList();
                    if (setSLAAutomationsList.Count > 0)
                    {
                        ApplySetSLAValueAutomation(entityPM, setSLAAutomationsList, entityChange, automationConditionFieldLists, lastUpdateDate, oldentityPM, processtype, entityId, OtherObjectTableIdWithLastUpdate);
                    }
                    #endregion

                    #region Queued Task
                    List<Automation> QueueautomationsList = automations.Where(d => d.ResultCode == "QUEUE").ToList();
                    if (QueueautomationsList.Count > 0)
                    {
                        this.ApplyQueuedTaskAutomation(entityPM, QueueautomationsList, entityChange, automationConditionFieldLists, lastUpdateDate, oldentityPM, processtype, entityId, OtherObjectTableIdWithLastUpdate);
                    }
                    #endregion

                    entityChange.SetAutomationFailedXml = EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "Set Fields Value").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "Set Fields Value").ToList()) : "";
                    entityChange.SetSLAAutomationFailedXml = EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "Set SLA Value").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "Set SLA Value").ToList()) : "";
                    entityChange.FollowUpAutomationFailedXml = EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "F/U Creation" || d.ResultCode == "Docs Out F/U Creation" || d.ResultCode == "Docs In F/U Creation").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "F/U Creation" || d.ResultCode == "Docs Out F/U Creation" || d.ResultCode == "Docs In F/U Creation").ToList()) : "";
                    entityChange.QueuedTaskAutomationFailedXml = EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "QUEUE").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsFailedList.Where(d => d.ResultCode == "QUEUE").ToList()) : "";

                    entityChange.SetAutomationSsucceedXml = EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "Set Fields Value").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "Set Fields Value").ToList()) : "";
                    entityChange.SetSLAAutomationSsucceedXml = EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "Set SLA Value").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "Set SLA Value").ToList()) : "";
                    entityChange.FollowUpAutomationSsucceedXml = EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "F/U Creation" || d.ResultCode == "Docs Out F/U Creation" || d.ResultCode == "Docs In F/U Creation").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "F/U Creation" || d.ResultCode == "Docs Out F/U Creation" || d.ResultCode == "Docs In F/U Creation").ToList()) : "";
                    entityChange.QueuedTaskAutomationSsucceedXml = EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "QUEUE").ToList().Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(EntityChangesAutomationsSsucceedList.Where(d => d.ResultCode == "QUEUE").ToList()) : "";

                    if (Changefields.Count() > 0)
                    {
                        r rFields = new r();
                        rFields.cs = Changefields;
                        entityChange.ChangesAutomationFieldsXml = rFields.cs.Count > 0 ? LogitudeXmlSerializer.SerializeObjectToXmlString(rFields) : "";
                    }

                    if (!IsDelayAutomation && emailAutomations.Count == 0)
                    {
                        entityChange.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
                    }
                }
                else
                {
                    if (tableName == "Shipment" || tableName == "Master") return;
                    entityChange.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                }
            }
            else
            {
                if (tableName == "Shipment" || tableName == "Master") return;
                entityChange.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            }

            entityChange.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
            entityChangeRepository.SubmitChanges();
        }

        #region EntityAutomationFields

        private List<Field> GetExternalEntityAutomationConditionFieldLists( List<ObjectField> automationsObjectFieldLists, List<Field> automationConditionFieldLists, int tenant)
        {
            List<Field> externalEntityAutomationConditionFieldLists = new List<Field>();
            foreach (ObjectField externalEnitityObjectField in automationsObjectFieldLists.Where(d => d.DisplayInAutomationAsEnitity).ToList())
            {
                Field externalEntityAutomationConditionField = automationConditionFieldLists.Where(d => d.Id == externalEnitityObjectField.Id).FirstOrDefault();
                if (externalEntityAutomationConditionField != null)
                {
                    List<ObjectField> externalEntityAutomationObjectFieldLists = automationsObjectFieldLists.Where(d => d.ObjectTableId == externalEnitityObjectField.LookUpTableId).ToList();
                    if (externalEntityAutomationObjectFieldLists.Count() > 0)
                    {
                        object externalEntity = GetEntityByIdAndObjectField(externalEntityAutomationConditionField.Value , externalEnitityObjectField, tenant);
                        List<Field> externalEntityAutomationConditionFieldsValueLists = GetExternalEntityAutomationConditionFieldsValueFromEntity(externalEnitityObjectField, externalEntityAutomationObjectFieldLists, externalEntity);
                        externalEntityAutomationConditionFieldLists = externalEntityAutomationConditionFieldLists.Concat(externalEntityAutomationConditionFieldsValueLists).ToList();
                    }
                }
            }
            return externalEntityAutomationConditionFieldLists;
        }

        private object GetEntityByIdAndObjectField( string entityId , ObjectField objectField, int tenant)
        {
            object automationEntity = null;
            if (!string.IsNullOrEmpty(entityId) && objectField.ObjectTable_LookUpTable != null)
            {
                if (objectField.ObjectTable_LookUpTable.Name == "Shipment" && ExternalEntity != null) automationEntity = ExternalEntity;
                else automationEntity = InjectionUtil.Instance.GetEntityByObjectTableNameAndEntityId(objectField.ObjectTable_LookUpTable.Name, entityId, tenant);
            }

            return automationEntity;
        }

        private List<Field> GetExternalEntityAutomationConditionFieldsValueFromEntity( ObjectField externalEnitityObjectField, List<ObjectField> externalEntityAutomationObjectFieldLists, object externalEntity)
        {
            List<Field> automationFieldLists = new List<Field>();
            foreach (ObjectField externalEntityAutomationObjectField in externalEntityAutomationObjectFieldLists)
            {
                string currentvalue = externalEntity != null ? GetValue(externalEntity, externalEntityAutomationObjectField) : "";
                Field automationConditionField = new Field() { Id = externalEntityAutomationObjectField.Id, Value = currentvalue != null ? currentvalue : "", OldValue = "", PropertyName = externalEntityAutomationObjectField.FieldName, PartnerObjectFieldId = externalEnitityObjectField.Id };
                automationFieldLists.Add(automationConditionField);

            }

            return automationFieldLists;
        }

        #endregion


        private EntityChange CreateEntityChange(string entityChangeFieldXml, ObjectTable shipmentobjectTable, int tenant, string entityId, ObjectTable objectTable)
        {
            return new EntityChange()
            {
                Id = IdCounter.GetNumber("EntityChange", tenant),
                Tenant = tenant,
                EntityId = entityId,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CreateByUserId = GetLoggedContactId(tenant),
                ObjectTableId = shipmentobjectTable != null ? shipmentobjectTable.Id : objectTable != null ? objectTable.Id : "",
                ChangesFieldsXml = entityChangeFieldXml,
                CheckStartDate = TenantServerConfigration.GetCurrentDateTime(tenant),
            };
        }
        private string GetLoggedContactId(int tenant)
        {
            string loggedContactId = string.Empty;
            ContactRepository contactRepository = new ContactRepository(tenant);
            if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                string email = HttpContext.Current.User.Identity.Name;
                var contact = contactRepository.GetSingleContactByEmail(email, tenant, true);
                if (contact != null) loggedContactId = contact.Id;
            }
            else
            {
                string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
                var contact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);
                if (contact != null) loggedContactId = contact.Id;

            }

            return loggedContactId;
        }



        private string GetStatuShipmentName(int tenant, string currentvalue)
        {
            Simplog.Data.InfrastructureModel.EntityPOCOs.EntityStatus status = EntityStatusRepository.GetSingleEntityStatus(currentvalue, tenant, true);
            if (status != null)
            {
                currentvalue = status.Name;
            }

            return currentvalue;
        }

        #endregion

        #region Build Cusom Object Field Lists

        private List<ObjectField> BuildCusomObjectFieldLists(List<Automation> Automations, string lastupdateautomation, string objectTableId)
        {
            AutomationResultEmailRecipientRepository automationResultEmailRecipientRepository = new AutomationResultEmailRecipientRepository(Automations[0].Tenant);
            ObjectFieldRepository objectFieldsRepository = new ObjectFieldRepository(Automations[0].Tenant);
            int LargeDelayTime = 0;
            AutomatedBackup automatedBackup = null;
            List<ObjectField> customObjectFieldLists = new List<ObjectField>();
            List<AutomationCondition> automationConditionList = new List<AutomationCondition>();
            List<AutomationSetValue> automationSetValueLists = new List<AutomationSetValue>();
            AutomationFollowUp automationFollowUp = new AutomationFollowUp();
            AutomationQueuedTask automationQueuedTask = new AutomationQueuedTask();

            List<AutomationCondition> delayedAutomationConditionList = new List<AutomationCondition>();
            List<ObjectField> objectFieldLists = objectFieldsRepository.GetAutomationObjectFieldsByObjectTableId(objectTableId, Automations[0].Tenant);

            foreach (Automation automation in Automations)
            {
                if (!string.IsNullOrEmpty(automation.AutomationXML))
                {
                    string automatedBackupName = "AutomatedBackupName" + lastupdateautomation + automation.Id + automation.Tenant;
                    if (CacheManager.CacheWrapper != null)
                    {
                        if (CacheManager.CacheWrapper.Get(automatedBackupName) == null || CacheManager.CacheWrapper.Get(automatedBackupName) == null)
                        {
                            automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                            automationConditionList = automatedBackup.AautomationConditionLists;
                            automationSetValueLists = automatedBackup.AutomationSetValueLists;
                            automationFollowUp = automatedBackup.AutomationFollowUp;
                            automationQueuedTask = automatedBackup.AutomationQueuedTask;
                            delayedAutomationConditionList = automatedBackup.DelayAautomationConditionLists;

                            CacheManager.CacheWrapper.Insert(automatedBackupName, automatedBackup, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                        }
                        else
                        {
                            automatedBackup = (AutomatedBackup)CacheManager.CacheWrapper.Get(automatedBackupName);
                            automationConditionList = automatedBackup.AautomationConditionLists;
                            automationSetValueLists = automatedBackup.AutomationSetValueLists;
                            automationFollowUp = automatedBackup.AutomationFollowUp;
                            automationQueuedTask = automatedBackup.AutomationQueuedTask;
                            delayedAutomationConditionList = automatedBackup.DelayAautomationConditionLists;
                        }
                    }
                    else
                    {
                        automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                        automationConditionList = automatedBackup.AautomationConditionLists;
                        automationSetValueLists = automatedBackup.AutomationSetValueLists;
                        automationFollowUp = automatedBackup.AutomationFollowUp;
                        automationQueuedTask = automatedBackup.AutomationQueuedTask;
                        delayedAutomationConditionList = automatedBackup.DelayAautomationConditionLists;
                    }

                    if (automatedBackup != null && automatedBackup.Delaytime > LargeDelayTime)
                    {
                        LargeDelayTime = automatedBackup.Delaytime;
                    }
                }

                #region Add Automation Condition Object Field

                if (automationConditionList != null && automationConditionList.Count > 0)
                {
                    foreach (AutomationCondition automationCondition in automationConditionList)
                    {
                        if (automationCondition.OperatorCode.Contains("F"))
                        {
                            ObjectField customobjectField = objectFieldLists.Where(d => d.Id == automationCondition.Value).FirstOrDefault();
                            if (customobjectField != null && !customObjectFieldLists.Contains(customobjectField))
                            {
                                customObjectFieldLists.Add(customobjectField);
                            }
                        }

                        ObjectField objectField = objectFieldLists.Where(d => d.Id == automationCondition.ObjectFieldId).FirstOrDefault();
                        if (objectField != null && !customObjectFieldLists.Contains(objectField))
                        {
                            customObjectFieldLists.Add(objectField);
                        }

                        ObjectField partnerObjectField = objectFieldLists.Where(d => d.Id == automationCondition.PartnerObjectFieldId).FirstOrDefault();
                        if (partnerObjectField != null && !customObjectFieldLists.Contains(partnerObjectField))
                        {
                            customObjectFieldLists.Add(partnerObjectField);
                        }

                    }
                }
                #endregion

                #region Add Automation Set Value Object Field

                if (automationSetValueLists != null && automationSetValueLists.Count > 0)
                {
                    foreach (AutomationSetValue automationSetValue in automationSetValueLists)
                    {
                        if (automationSetValue.OperatorCode.Contains("F"))
                        {
                            ObjectField objectFieldValue = objectFieldLists.Where(d => d.Id == automationSetValue.Value).FirstOrDefault();
                            if (objectFieldValue != null && !customObjectFieldLists.Contains(objectFieldValue))
                            {
                                customObjectFieldLists.Add(objectFieldValue);
                            }
                        }
                    }
                }
                #endregion

                #region F/U Creation ObjectField

                if (automationFollowUp != null)
                {
                    if (automationFollowUp.OwnerFieldType == "Field")
                    {
                        ObjectField objectFieldValue = objectFieldLists.Where(d => d.Id == automationFollowUp.OwnerValue).FirstOrDefault();
                        if (objectFieldValue != null && !customObjectFieldLists.Contains(objectFieldValue))
                        {
                            customObjectFieldLists.Add(objectFieldValue);
                        }
                    }


                    if (automationFollowUp.DateValue != null)
                    {
                        ObjectField objectFieldValue = objectFieldLists.Where(d => d.Id == automationFollowUp.DateValue.ToString()).FirstOrDefault();
                        if (objectFieldValue != null && !customObjectFieldLists.Contains(objectFieldValue))
                        {
                            customObjectFieldLists.Add(objectFieldValue);
                        }
                    }
                }
                #endregion

                #region Add Delayed Automation Condition Object Field

                if (delayedAutomationConditionList != null && delayedAutomationConditionList.Count > 0)
                {
                    foreach (AutomationCondition delayedautomationCondition in delayedAutomationConditionList)
                    {
                        if (delayedautomationCondition.OperatorCode.Contains("F"))
                        {
                            ObjectField customobjectField = objectFieldLists.Where(d => d.Id == delayedautomationCondition.Value).FirstOrDefault();
                            if (customobjectField != null && !customObjectFieldLists.Contains(customobjectField))
                            {
                                customObjectFieldLists.Add(customobjectField);
                            }
                        }

                        ObjectField objectField = objectFieldLists.Where(d => d.Id == delayedautomationCondition.ObjectFieldId).FirstOrDefault();
                        if (objectField != null && !customObjectFieldLists.Contains(objectField))
                        {
                            customObjectFieldLists.Add(objectField);
                        }
                    }
                }
                #endregion

                #region Add Email Recipient Object Field

                List<AutomationResultEmailRecipient> resultEmailRecipient = automationResultEmailRecipientRepository.GetAutomationResultEmailRecipientByAutomationId(automation.Id, automation.Tenant);

                foreach (AutomationResultEmailRecipient automationResultEmailRecipient in resultEmailRecipient)
                {
                    ObjectField objectField = objectFieldLists.Where(d => d.Id == automationResultEmailRecipient.RecipientValue).FirstOrDefault();

                    if (objectField != null && !customObjectFieldLists.Contains(objectField))
                    {
                        customObjectFieldLists.Add(objectField);
                    }
                }

                #endregion
            }

            return customObjectFieldLists;
        }

        private List<ObjectField> BuildAutomationObjectFieldLists(List<ObjectField> customObjectFieldLists, List<Automation> automations, string objectTableId, string lastUpdateDate, int tenant, string processtype, string otherObjectTableId = null)
        {
            string tableId = !string.IsNullOrEmpty(otherObjectTableId) ? otherObjectTableId : objectTableId;

            string customObjectFieldListsName = "CustomObjectFieldLists" + objectTableId + tenant + lastUpdateDate + processtype;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(customObjectFieldListsName) == null)
                {
                    customObjectFieldLists = BuildCusomObjectFieldLists(automations, lastUpdateDate, tableId);

                    if (customObjectFieldLists != null)
                    {
                        CacheManager.CacheWrapper.Insert(customObjectFieldListsName, customObjectFieldLists, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                    }
                }
                else
                {
                    customObjectFieldLists = (List<ObjectField>)CacheManager.CacheWrapper.Get(customObjectFieldListsName);
                }
            }

            else
            {
                customObjectFieldLists = BuildCusomObjectFieldLists(automations, lastUpdateDate, tableId);
            }
            return customObjectFieldLists;
        }

        #endregion

        #region Apply Set  SLA Value Automation
        public void ApplySetSLAValueAutomation(Object entityPM, List<Automation> automationsList, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, Object oldentityPM, string processtype, string entityId, string otherLastupdateautomation)
        {
            int tenant = entityChange.Tenant;

            foreach (Automation automation in automationsList)
            {
                DateTime dateBefore = DateTime.Now;
                EntityChangeAutomation entityChangesAutomation = CreateEntityChangeAutomation(automation);

                entityChangesAutomation.ResultCode = "Set SLA Value";
                string lastUpdate = GetLastUpdateDate(lastupdateautomation, otherLastupdateautomation, automation);
                ValidateAutomationResultClass validateResult = ValidateAutomation(automation, entityChange, automationFieldLists, lastUpdate, "");
                entityChangesAutomation.type = validateResult.IsAutomationValid ? "SetSsucceed" : "SetFailed";

                if (validateResult.IsAutomationValid)
                {
                    SetSLAValue(entityPM, entityChange, automationFieldLists, lastUpdate, EntityChangesAutomationsSsucceedList, Changefields, automation, entityChangesAutomation, dateBefore);
                }

                else
                {
                    entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
                    EntityChangesAutomationsFailedList.Add(entityChangesAutomation);
                    entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                }
            }
        }

        public void SetSLAValue(Object entityPM, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, List<EntityChangeAutomation> EntityChangesAutomationsSsucceedList, List<c> fields, Automation automation, EntityChangeAutomation entityChangesAutomation, DateTime dateBefore)
        {
            AutomationSetSLAValue AutomationSetSLAValue = null;
            string automationSetSLAValueName = "AutomationSetSLAValue" + lastupdateautomation + automation.Id + automation.Tenant;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(automationSetSLAValueName) == null)
                {
                    if (!string.IsNullOrEmpty(automation.AutomationXML))
                    {
                        AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                        AutomationSetSLAValue = AutomatedBackup.AutomationSetSLAValue;
                        CacheManager.CacheWrapper.Insert(automationSetSLAValueName, AutomationSetSLAValue, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                    }
                }
                else
                {
                    AutomationSetSLAValue = (AutomationSetSLAValue)CacheManager.CacheWrapper.Get(automationSetSLAValueName);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(automation.AutomationXML))
                {
                    AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                    AutomationSetSLAValue = AutomatedBackup.AutomationSetSLAValue;
                }
            }

            #region Set SLA Value

            object oldValue;
            object newValue = "";

            PropertyInfo propInfo = entityPM.GetType().GetProperty("SLAId");

            if (propInfo != null)
            {
                oldValue = propInfo.GetValue(entityPM);
                newValue = AutomationSetSLAValue.SLAId;

                if (oldValue == null) oldValue = "";
                if (newValue == null) newValue = "";

                if (oldValue.ToString().ToLower() != newValue.ToString().ToLower())
                {
                    IsChangeSLA = true;
                    propInfo.SetValue(entityPM, newValue, null);
                    c fieldc = new c()
                    {
                        f = AutomationSetSLAValue.ObjectFieldId,
                        o = oldValue.ToString(),
                        n = newValue.ToString(),
                    };

                    fields.Add(fieldc);
                }
            }

            #endregion

            entityChange.HasExecutedRecord = true;
            entityChangesAutomation.IsConditionTrue = true;
            entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
            EntityChangesAutomationsSsucceedList.Add(entityChangesAutomation);
            entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
        }
        #endregion

        #region Apply Set Value Automation

        public void ApplySetValueAutomation(Object entityPM, List<Automation> automationsList, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, Object oldentityPM, string processtype, string entityId, string otherLastupdateautomation)
        {
            int tenant = entityChange.Tenant;

            foreach (Automation automation in automationsList)
            {
                DateTime dateBefore = DateTime.Now;
                EntityChangeAutomation entityChangesAutomation = CreateEntityChangeAutomation(automation);

                if (automation.ResultCode == "FIELDSET") entityChangesAutomation.ResultCode = "Set Fields Value";

                string lastUpdate = GetLastUpdateDate(lastupdateautomation, otherLastupdateautomation, automation);

                ValidateAutomationResultClass validateResult = ValidateAutomation(automation, entityChange, automationFieldLists, lastUpdate, "");

                if (validateResult.Type == "Delayed")
                {
                    IsDelayAutomation = true;
                }

                entityChangesAutomation.type = validateResult.IsAutomationValid ? "SetSsucceed" : "SetFailed";

                if (validateResult.IsAutomationValid)
                {
                    if (validateResult.Type == "Delayed")
                    {
                        AddDelayedAutomationQueue(entityChange.Id, processtype, automation.Tenant, automation.Id, validateResult.Delaytime, validateResult.DelaytimeIndicator, entityId);
                    }
                    else SetValue(entityPM, entityChange, automationFieldLists, lastUpdate, EntityChangesAutomationsSsucceedList, Changefields, automation, entityChangesAutomation, dateBefore);
                }
                else
                {
                    entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
                    EntityChangesAutomationsFailedList.Add(entityChangesAutomation);
                    entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                }
            }
        }

        public void SetValue(Object entityPM, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, List<EntityChangeAutomation> EntityChangesAutomationsSsucceedList, List<c> fields, Automation automation, EntityChangeAutomation entityChangesAutomation, DateTime dateBefore)
        {
            List<AutomationSetValue> AutomationSetValueLists = null;
            string automationSetValueListsName = "AutomationSetValueLists" + lastupdateautomation + automation.Id + automation.Tenant;

            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(automationSetValueListsName) == null)
                {
                    if (!string.IsNullOrEmpty(automation.AutomationXML))
                    {
                        AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                        AutomationSetValueLists = AutomatedBackup.AutomationSetValueLists;
                        CacheManager.CacheWrapper.Insert(automationSetValueListsName, AutomationSetValueLists, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                    }
                }
                else
                {
                    AutomationSetValueLists = (List<AutomationSetValue>)CacheManager.CacheWrapper.Get(automationSetValueListsName);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(automation.AutomationXML))
                {
                    AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                    AutomationSetValueLists = AutomatedBackup.AutomationSetValueLists;
                }
            }

            #region Set Field Value

            object oldValue;
            object newValue = "";

            foreach (AutomationSetValue item in AutomationSetValueLists)
            {
                PropertyInfo propInfo = entityPM.GetType().GetProperty(item.FieldName);

                if (propInfo != null)
                {
                    oldValue = propInfo.GetValue(entityPM);

                    #region Get New Value

                    if (item.OperatorCode.Contains("F"))
                    {
                        Field field = automationFieldLists.Where(d => d.Id == item.Value).FirstOrDefault();
                        if (field != null) newValue = field.Value;
                    }

                    else if (item.DataTypeCode.Trim() == "DateTime" || item.DataTypeCode.Trim() == "Date")
                    {
                        newValue = ConvertToDate(item.Value);
                    }

                    else if (item.DataTypeCode.Trim() == "Boolean")
                    {
                        if (!string.IsNullOrEmpty(item.Value))
                        {
                            newValue = item.Value.ToLower() == "true" ? true : false;
                        }

                        else newValue = false;
                    }

                    else newValue = item.Value;

                    #endregion

                    if (oldValue == null) oldValue = "";
                    if (newValue == null) newValue = "";

                    if (oldValue.ToString().ToLower() != newValue.ToString().ToLower())
                    {
                        propInfo.SetValue(entityPM, newValue, null);
                        c fieldc = new c()
                        {
                            f = item.ObjectFieldId,
                            o = oldValue.ToString(),
                            n = newValue.ToString(),
                        };

                        fields.Add(fieldc);
                    }
                }
            }
            #endregion

            entityChange.HasExecutedRecord = true;
            entityChangesAutomation.IsConditionTrue = true;
            entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
            EntityChangesAutomationsSsucceedList.Add(entityChangesAutomation);
            entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
        }

        #endregion

        #region Validate Automation And Condition
        private bool ValidateCondition(List<Field> automationConditionFieldLists, AutomationCondition automationCondition, EntityChange entityChange)
        {
            bool isValid = true;
            string automationConditionPartnerObjectFieldId = !string.IsNullOrEmpty(automationCondition.PartnerObjectFieldId) ? automationCondition.PartnerObjectFieldId : null;
            Field automationConditionField = automationConditionFieldLists.Where(d => d.Id == automationCondition.ObjectFieldId && automationConditionPartnerObjectFieldId == d.PartnerObjectFieldId).FirstOrDefault();

            if (automationConditionField != null)
            {
                string automationConditionvalue = "";
                string automationConditionFieldValue = "";

                if (!string.IsNullOrEmpty(automationConditionField.Value)) automationConditionFieldValue = automationConditionField.Value.ToLower();
                if (!string.IsNullOrEmpty(automationCondition.Value)) automationConditionvalue = automationCondition.Value.ToLower();

                #region  CustomObjectFieldValue

                if (automationCondition.OperatorCode.Contains("F"))
                {
                    Field item = automationConditionFieldLists.Where(d => d.Id == automationConditionvalue).FirstOrDefault();

                    if (item != null)
                    {
                        if (!string.IsNullOrEmpty(item.Value)) automationConditionvalue = item.Value.ToLower();
                    }
                }

                #endregion

                #region CustomDateValue

                else if (automationCondition.ObjectFieldType == "DateTime" || automationCondition.ObjectFieldType == "Date")
                {
                    if (!string.IsNullOrEmpty(automationConditionvalue))
                    {
                        string[] datearray = automationConditionvalue.Split('*');

                        if (automationConditionvalue.ToLower().Contains("date"))
                        {
                            if (datearray.Length > 1) automationConditionvalue = datearray[1];
                        }
                        else
                        {
                            if (datearray.Length > 2)
                            {
                                int days = 0;
                                if (!string.IsNullOrEmpty(datearray[1])) days = Int32.Parse(datearray[1]);
                                int dateEscalationTime = 0;
                                DateTime? date = datearray[0].Contains("old") ? FieldValueResolver.ConvertToDate(automationConditionField.OldValue)  :TenantServerConfigration.GetCurrentDateTime(automationCondition.Tenant);
                                CustomFieldClass customFieldClass = new CustomFieldClass();

                                dateEscalationTime = (datearray[0] == "@today+" || datearray[0] == "@old value+") ? days : days * -1;
                                if (date != null)
                                {
                                    date = date.Value.AddDays(dateEscalationTime);
                                    automationConditionvalue = customFieldClass.ConvertToString(Convert.ToDateTime(date));
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(automationConditionvalue) && automationConditionvalue.Length >= 9) automationConditionvalue = automationConditionvalue.Remove(8);
                        if (!string.IsNullOrEmpty(automationConditionFieldValue) && automationConditionFieldValue.Length >= 9) automationConditionFieldValue = automationConditionFieldValue.Remove(8);
                    }
                }

                else if (automationCondition.ObjectFieldType == "Decimal" || automationCondition.ObjectFieldType == "Integer" || automationCondition.ObjectFieldType == "Double")
                {
                    automationConditionvalue = FieldValueResolver.GetFieldStringValue(new ObjectField() { DataTypeCode = automationCondition.ObjectFieldType }, automationCondition.Value);
                }

            
                else if (!string.IsNullOrEmpty(automationCondition.Value) && automationCondition.Value.Contains("@StatusName:"))
                {
                    automationConditionvalue = automationCondition.Value.Split('@')[0];
                    if (!string.IsNullOrEmpty(automationConditionvalue)) automationConditionvalue = automationConditionvalue.ToLower();
                }


                if (automationCondition.ObjectFieldType == "Boolean")
                {
                    if (string.IsNullOrEmpty(automationConditionvalue)) automationConditionvalue = "false";
                    if (string.IsNullOrEmpty(automationConditionFieldValue)) automationConditionvalue = "false";
                }

                #endregion


                if (automationCondition.OperatorCode == "<=" || automationCondition.OperatorCode == "<=F" || automationCondition.OperatorCode == ">" || automationCondition.OperatorCode == ">F" || automationCondition.OperatorCode == ">=" || automationCondition.OperatorCode == ">=F" || automationCondition.OperatorCode == "<" || automationCondition.OperatorCode == "<F")
                {
                    if (string.IsNullOrEmpty(automationConditionvalue) || string.IsNullOrEmpty(automationConditionFieldValue)) return false;
                }

                if (automationCondition.OperatorCode == "=" || automationCondition.OperatorCode == "=F")
                {
                    if (automationConditionFieldValue != automationConditionvalue) isValid = false;
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "<>" || automationCondition.OperatorCode == "<>F")
                {
                    if (automationConditionFieldValue == automationConditionvalue) isValid = false;
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "CONTAINS" || automationCondition.OperatorCode == "CONTAINSF")
                {
                    if (!automationConditionFieldValue.Contains(automationConditionvalue)) isValid = false;
                    return isValid;
                }
                else if (automationCondition.OperatorCode == "!CONTAINS" || automationCondition.OperatorCode == "!CONTAINSF")
                {
                    if (!string.IsNullOrEmpty(automationConditionFieldValue) && automationConditionFieldValue.Contains(automationConditionvalue)) isValid = false;
                    return isValid;
                }

                else if (automationCondition.OperatorCode == ">" || automationCondition.OperatorCode == ">F")
                {
                    int reslutCompare = automationConditionFieldValue.CompareTo(automationConditionvalue);
                    if (reslutCompare <= 0) isValid = false;
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "<" || automationCondition.OperatorCode == "<F")
                {
                    int reslutCompare = automationConditionFieldValue.CompareTo(automationConditionvalue);
                    if (reslutCompare >= 0) isValid = false;
                    return isValid;
                }

                else if (automationCondition.OperatorCode == ">=" || automationCondition.OperatorCode == ">=F")
                {
                    int reslutCompare = automationConditionFieldValue.CompareTo(automationConditionvalue);
                    if (reslutCompare == -1) isValid = false;
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "<=" || automationCondition.OperatorCode == "<=F")
                {
                    int reslutCompare = automationConditionFieldValue.CompareTo(automationConditionvalue);
                    if (reslutCompare == 1) isValid = false;
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "CHANGEDTO")
                {
                    if (automationConditionFieldValue != automationConditionvalue || !automationConditionField.IsChange) isValid = false;
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "CHANGED")
                {
                    if (!automationConditionField.IsChange) isValid = false;
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "ISEMPTY")
                {
                    if (!string.IsNullOrEmpty(automationConditionFieldValue)) isValid = false;
                    return isValid;
                }
                else if (automationCondition.OperatorCode == "ISNOTEMPTY")
                {
                    if (string.IsNullOrEmpty(automationConditionFieldValue)) isValid = false;
                    return isValid;
                }

                else if (automationCondition.OperatorCode == "EqualSystemVariable" && automationCondition.Value == "SystemUser")
                {
                    string userId = GetSystemContactIdByTenant(entityChange.Tenant);
                    if (userId != automationConditionFieldValue) isValid = false;

                    return isValid;
                }

            }
            else
            {
                isValid = false;
            }

            return isValid;
        }


        private string GetSystemContactIdByTenant(int tenant)
        {
            string loggedContactId = string.Empty;
            ContactRepository contactRepository = new ContactRepository(tenant);
            string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
            var contact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);
            if (contact != null) loggedContactId = contact.Id;

            return loggedContactId;
        }

        public ValidateAutomationResultClass ValidateAutomation(Automation automation, EntityChange entityChange, List<Field> automationConditionFields, string lastupdateautomation, string typeConditionValidate)
        {
            bool validconditionAnd = true;
            bool validconditionOr = false;
            AutomatedBackup automatedBackup = null;
            bool IsConditionValid = false;

            ValidateAutomationResultClass validateResult = new ValidateAutomationResultClass();
            List<AutomationCondition> automationConditionList = null;

            if (entityChange.CreateDate >= automation.UpdateDate)
            {
                #region Load AutomatedBackup From Cache
                string automatedBackupName = "AutomatedBackupName" + lastupdateautomation + automation.Id + automation.Tenant;

                if (CacheManager.CacheWrapper != null)
                {
                    if (CacheManager.CacheWrapper.Get(automatedBackupName) == null)
                    {
                        if (!string.IsNullOrEmpty(automation.AutomationXML))
                        {
                            automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);

                            if (typeConditionValidate == "Delayed") automationConditionList = automatedBackup.DelayAautomationConditionLists;
                            else automationConditionList = automatedBackup.AautomationConditionLists;

                            CacheManager.CacheWrapper.Insert(automatedBackupName, automatedBackup, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        automatedBackup = (AutomatedBackup)CacheManager.CacheWrapper.Get(automatedBackupName);
                        if (typeConditionValidate == "Delayed") automationConditionList = automatedBackup.DelayAautomationConditionLists;
                        else automationConditionList = automatedBackup.AautomationConditionLists;
                    }
                }

                else
                {
                    if (!string.IsNullOrEmpty(automation.AutomationXML))
                    {
                        automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                        if (typeConditionValidate == "Delayed") automationConditionList = automatedBackup.DelayAautomationConditionLists;
                        else automationConditionList = automatedBackup.AautomationConditionLists;
                    }
                }

                #endregion
            }
            else
            {
                AutomationHistoryRepository automationHistoryRepository = new AutomationHistoryRepository(entityChange.Tenant);
                string automationXML = automationHistoryRepository.GetAutomationXMLFromAutomationHistoryByDate(entityChange.CreateDate, automation.Id, entityChange.Tenant);
                if (!string.IsNullOrEmpty(automationXML))
                {
                    automatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automationXML);
                    if (typeConditionValidate == "Delayed") automationConditionList = automatedBackup.DelayAautomationConditionLists;
                    else automationConditionList = automatedBackup.AautomationConditionLists;
                }
            }

            if (automationConditionList != null && automationConditionList.Count() > 0)
            {
                int conditionAndCount = automationConditionList.Where(d => d.ConditionType == "And").Count();
                int conditionOrCount = automationConditionList.Where(d => d.ConditionType == "Or").Count();

                if (conditionOrCount == 0) validconditionOr = true;
                foreach (AutomationCondition automationCondition in automationConditionList.Where(d => d.ConditionType == "And"))
                {
                    validconditionAnd = ValidateCondition(automationConditionFields, automationCondition, entityChange);
                    if (!validconditionAnd) break;
                }

                if (validconditionAnd)
                {
                    foreach (AutomationCondition automationCondition in automationConditionList.Where(d => d.ConditionType == "Or"))
                    {
                        validconditionOr = ValidateCondition(automationConditionFields, automationCondition, entityChange);
                        if (validconditionOr) break;
                    }
                }

                if (validconditionAnd && validconditionOr) IsConditionValid = true;
            }

            else IsConditionValid = true;
            validateResult.IsAutomationValid = IsConditionValid;

            if (automatedBackup != null)
            {
                validateResult.Type = automatedBackup.Type;
                validateResult.DelaytimeIndicator = automatedBackup.DelaytimeIndicator;
                validateResult.Delaytime = automatedBackup.Delaytime;
            }

            return validateResult;
        }
        #endregion

        #region  Apply FollowUp Automation
        public void ApplyFollowUpAutomation(Object entityPM, List<Automation> automations, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, Object oldentityPM, string processtype, string entityId, string otherLastupdateautomation)
        {
            if (automations.Count > 0)
            {
                foreach (Automation automation in automations)
                {
                    DateTime dateBefore = DateTime.Now;
                    EntityChangeAutomation entityChangesAutomation = CreateEntityChangeAutomation(automation);
                    if (automation.ResultCode == "FOLLOWUP") entityChangesAutomation.ResultCode = "F/U Creation";
                    else if (automation.ResultCode == "DOCOUTFOLLOWUP") entityChangesAutomation.ResultCode = "Docs Out F/U Creation";
                    else if (automation.ResultCode == "DOCINFOLLOWUP") entityChangesAutomation.ResultCode = "Docs In F/U Creation";

                    string lastUpdate = GetLastUpdateDate(lastupdateautomation, otherLastupdateautomation, automation);

                    ValidateAutomationResultClass validateResult = ValidateAutomation(automation, entityChange, automationFieldLists, lastUpdate, "");

                    if (validateResult.Type == "Delayed") IsDelayAutomation = true;

                    entityChangesAutomation.type = validateResult.IsAutomationValid ? "FollowUpCreatedSsucceed" : "FollowUpCreatedFailed";

                    if (validateResult.IsAutomationValid)
                    {
                        if (validateResult.Type == "Delayed")
                        {
                            AddDelayedAutomationQueue(entityChange.Id, processtype, automation.Tenant, automation.Id, validateResult.Delaytime, validateResult.DelaytimeIndicator, entityId);
                        }
                        else AddAutomationFollowUp(entityPM, entityChange, automationFieldLists, lastUpdate, EntityChangesAutomationsSsucceedList, automation, entityChangesAutomation, dateBefore, true);

                    }
                    else
                    {
                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
                        EntityChangesAutomationsFailedList.Add(entityChangesAutomation);
                        entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
                    }
                }
            }
        }

        private EntityChangeAutomation CreateEntityChangeAutomation(Automation automation)
        {
            return new EntityChangeAutomation()
            {
                Id = IdCounter.GetNumber("EntityChangeAutomation", automation.Tenant),
                AutomationId = automation.Id,
                AutomationType = automation.Type,
                ResultCode = automation.ResultCode,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(automation.Tenant),
                AutomationDescription = automation.Description,
                AutomationName = automation.Name,
            };
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
                    Field field = automationFieldLists.Where(d => d.Id == automationFollowUp.OwnerValue).FirstOrDefault();
                    if (field != null) ownerId = field.Value;
                }

                //Date
                if (!string.IsNullOrEmpty(automationFollowUp.DateValue))
                {
                    Field field = automationFieldLists.Where(d => d.Id == automationFollowUp.DateValue).FirstOrDefault();
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
                ObjectTableName = "Shipment",
                Notes = automationFollowUp.FollowUpEnglishName + "\n" + "Resulted from Automation",

            });


        }

        #endregion

        #region Apply Queued Task Automation
        public void ApplyQueuedTaskAutomation(Object entityPM, List<Automation> automations, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, Object oldentityPM, string processtype, string entityId, string otherLastupdateautomation)
        {
            if (automations.Count > 0)
            {
                foreach (Automation automation in automations)
                {
                    DateTime dateNow = DateTime.Now;
                    EntityChangeAutomation entityChangesAutomation = CreateEntityChangeAutomation(automation);
                    entityChangesAutomation.ResultCode = "Queued Task";

                    string lastUpdate = this.GetLastUpdateDate(lastupdateautomation, otherLastupdateautomation, automation);

                    ValidateAutomationResultClass validateResult = this.ValidateAutomation(automation, entityChange, automationFieldLists, lastUpdate, "");

                    if (validateResult.Type == "Delayed")
                    {
                        IsDelayAutomation = true;
                    }

                    entityChangesAutomation.type = validateResult.IsAutomationValid ? "QueuedTaskCreatedSsucceed" : "QueuedTaskCreatedFailed";

                    if (validateResult.IsAutomationValid)
                    {
                        if (validateResult.Type == "Delayed")
                        {
                            this.AddDelayedAutomationQueue(entityChange.Id, processtype, automation.Tenant, automation.Id, validateResult.Delaytime, validateResult.DelaytimeIndicator, entityId);
                        }

                        else
                        {
                            this.AddAutomationQueuedTask(entityPM, entityChange, automationFieldLists, lastUpdate, EntityChangesAutomationsSsucceedList, automation, entityChangesAutomation, dateNow);
                        }

                    }
                    else
                    {
                        entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
                        EntityChangesAutomationsFailedList.Add(entityChangesAutomation);
                        entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateNow.Ticks) / TimeSpan.TicksPerMillisecond);
                    }
                }
            }
        }

        public void AddAutomationQueuedTask(Object entityPM, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, List<EntityChangeAutomation> EntityChangesAutomationsSsucceedList, Automation automation, EntityChangeAutomation entityChangesAutomation, DateTime dateNow)
        {
            int tenant = automation.Tenant;
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);

            ContactRepository contactRepository = new ContactRepository(commonContext);
            UserRepository userRepository = new UserRepository(commonContext);

            AutomationQueuedTask automationQueuedTask = null;

            #region  Get AutomationQueuedTask From Cache

            string automationQueuedTaskName = "AutomationQueuedTask" + lastupdateautomation + automation.Id + tenant;

            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(automationQueuedTaskName) == null)
                {
                    if (!string.IsNullOrEmpty(automation.AutomationXML))
                    {
                        AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                        automationQueuedTask = AutomatedBackup.AutomationQueuedTask;
                        CacheManager.CacheWrapper.Insert(automationQueuedTaskName, automationQueuedTask, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
                    }
                }
                else
                {
                    automationQueuedTask = (AutomationQueuedTask)CacheManager.CacheWrapper.Get(automationQueuedTaskName);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(automation.AutomationXML))
                {
                    AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
                    automationQueuedTask = AutomatedBackup.AutomationQueuedTask;
                }
            }

            #endregion

            if (automationQueuedTask != null)
            {
                string dueDateFieldName = automationQueuedTask.DateFieldValue;
                string offsetType = automationQueuedTask.OffsetTypeValue;
                string timeUnit = automationQueuedTask.TaskTimeUnitValue;
                string queueId = automationQueuedTask.QueueId;
                string teamId = automationQueuedTask.TeamId;
                int? offset = automationQueuedTask.TaskOffset;
                string ownerId = automationQueuedTask.TaskOwnerId;
                string customerId = automationQueuedTask.TaskCustomerId;
                string subject = automationQueuedTask.TaskSubject;
                string description = automationQueuedTask.TaskDescription;
                string priorityId = automationQueuedTask.TaskPriorityId;
                DateTime? dueDate = null;

                if (!string.IsNullOrEmpty(queueId))
                {
                    string businessRoleId = "";

                    IInfrastructureContext infraContext = InfrastructureContext.GetContext(tenant);
                    BusinessProcessQueueRepository businessProcessQueueRepository = new BusinessProcessQueueRepository(infraContext);

                    BusinessProcessQueue queue = businessProcessQueueRepository.GetSingle(queueId, tenant);
                    if (queue != null)
                    {
                        businessRoleId = queue.BusinessRoleId;
                    }

                    if (!string.IsNullOrEmpty(businessRoleId))
                    {
                        TeamMemberBusinessRoleRepository teamMemberBusinessRoleRepository = new TeamMemberBusinessRoleRepository(infraContext);
                        IQueryable<TeamMemberBusinessRole> teamMemberBusinessRoles = teamMemberBusinessRoleRepository.GetAllByBusinessRoleId(businessRoleId, tenant);

                        string loggedUserBranchId = "";
                        string loggedUserBusinessUnitId = "";

                        User loggedUser = userRepository.GetSingleUser(entityChange.CreateByUserId, tenant);
                        if (loggedUser != null)
                        {
                            loggedUserBranchId = loggedUser.BranchId;
                            loggedUserBusinessUnitId = loggedUser.BusinessUnitId;
                        }

                        if (string.IsNullOrEmpty(ownerId))
                        {
                            TeamMemberBusinessRole myMember = teamMemberBusinessRoles.FirstOrDefault();

                            if (myMember != null && myMember.LBPTeamMember != null)
                            {
                                ownerId = myMember.LBPTeamMember.MemberUserId;
                            }
                        }

                        else
                        {
                            if (string.IsNullOrEmpty(teamId))
                            {
                                TeamMemberBusinessRole myMember = teamMemberBusinessRoles.FirstOrDefault();

                                if (myMember != null && myMember.LBPTeamMember != null)
                                {
                                    teamId = myMember.LBPTeamMember.TeamId;
                                }
                            }
                        }

                        Type type = entityPM.GetType();
                        string entityId = string.Empty;
                        Object value = GetPropertyValue(entityPM, type, "Id");
                        if (value != null)
                        {
                            entityId = value.ToString();
                        }

                        ActivityRepository activityRepository = new ActivityRepository(tenant);
                        Activity activity = new Activity();
                        activity.Id = IdCounter.GetNumber("Activity", tenant);
                        activity.Tenant = tenant;
                        activity.ConcurrencyGUID = Guid.NewGuid().ToString();
                        activity.ShipmentId = entityId;
                        activity.IsOpen = true;
                        activity.ActivityStatusCode = "N";
                        activity.ActivityTypeCode = "TX";
                        activity.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        activity.CreatedByUserId = entityChange.CreateByUserId;
                        activity.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                        activity.UpdatedByUserId = entityChange.CreateByUserId;
                        activity.BranchId = loggedUserBranchId;
                        activity.BusinessUnitId = loggedUserBusinessUnitId;
                        activity.PriorityCode = !string.IsNullOrEmpty(priorityId) ? priorityId : "02";
                        activity.OwnerId = string.IsNullOrEmpty(ownerId) ? null : ownerId;
                        activity.CustomerId = string.IsNullOrEmpty(customerId) ? null : customerId;
                        activity.BusinessProcessQueueId = queueId;
                        activity.TeamId = string.IsNullOrEmpty(teamId) ? null : teamId;
                        activity.Description = description;
                        activity.Subject = subject;

                        if (offset != null)
                        {
                            int? myOffset = null;
                            switch (offsetType)
                            {
                                case "A":
                                    {
                                        myOffset = offset;
                                        break;
                                    }

                                case "B":
                                    {
                                        myOffset = offset * -1;
                                        break;
                                    }
                            }

                            if (timeUnit == "D")
                            {
                                activity.DueDateOffset = myOffset * 24;
                            }
                            else
                            {
                                activity.DueDateOffset = myOffset;
                            }
                        }

                        if (!string.IsNullOrEmpty(dueDateFieldName))
                        {
                            activity.DueDateDateField = dueDateFieldName;

                            Type dateType = entityPM.GetType();
                            if (dateType != null)
                            {
                                PropertyInfo propInfo = dateType.GetProperty(dueDateFieldName);

                                if (propInfo != null)
                                {
                                    object dateObject = propInfo.GetValue(entityPM);
                                    if (dateObject != null)
                                    {
                                        DateTime? dateValue = (DateTime)dateObject;
                                        if (dateValue != null)
                                        {
                                            if (activity.DueDateOffset == null)
                                            {
                                                dueDate = dateValue;
                                            }

                                            else
                                            {
                                                dueDate = dateValue.Value.AddHours((double)activity.DueDateOffset);
                                            }

                                            activity.DueDate = dueDate;
                                        }
                                    }
                                }
                            }
                        }

                        activityRepository.Add(activity);
                        activityRepository.SubmitChanges();

                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = tenant,
                            EventTypeCode = "CRAV",
                            UserId = entityChange.CreateByUserId,
                            EntityId = activity.Id,
                            ObjectTableName = "Activity",
                        });
                    }
                }

                entityChange.HasExecutedRecord = true;
                entityChangesAutomation.IsConditionTrue = true;
                entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
                entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateNow.Ticks) / TimeSpan.TicksPerMillisecond);
                EntityChangesAutomationsSsucceedList.Add(entityChangesAutomation);
            }
        }
        #endregion

        #region GetPropertyValue
        private object GetPropertyValue(Object entityPM, Type type, string fieldName)
        {
            PropertyInfo propertyInf = type.GetProperty(fieldName);
            object value = null;
            if (propertyInf != null)
            {
                value = propertyInf.GetValue(entityPM, null);
            }
            return value;
        }

        private string GetValue(object currentEntity, ObjectField objectField)
        {
            Object value = null;

            if (objectField != null)
            {
                PropertyInfo propertyInf = GetProperty(currentEntity, objectField.FieldName);

                if (propertyInf != null)
                {
                    value = propertyInf.GetValue(currentEntity, null);
                    if (value != null)
                    {
                        if (value.GetType() == typeof(CustomFieldClass))
                        {
                            CustomFieldClass classvalue = value as CustomFieldClass;
                            value = classvalue.Value;
                        }
                        else
                        {
                            value = FieldValueResolver.GetFieldStringValue(objectField, value);
                        }
                    }
                }
            }

            if (value == null) return null;
            return value.ToString();
        }

        private PropertyInfo GetProperty(object entity, string FieldName)
        {
            Type type = entity.GetType();
            return type.GetProperty(FieldName);
        }

        private DateTime? ConvertToDate(string value)
        {
            DateTime? date = null;

            if (!string.IsNullOrEmpty(value))
            {
                if (value.Length < 12)
                {
                    int count = 12 - value.Length;
                    while (count != 0)
                    {
                        value += "0";
                        count -= 1;
                    }
                }

                if (value.Length >= 12)
                {
                    date = new DateTime(System.Convert.ToInt32(value.Substring(0, 4)), System.Convert.ToInt32(value.Substring(4, 2)), System.Convert.ToInt32(value.Substring(6, 2)), System.Convert.ToInt32(value.Substring(8, 2)), System.Convert.ToInt32(value.Substring(10, 2)), System.Convert.ToInt32(value.Substring(12, 2)));
                }
            }

            return date;
        }

        #endregion

        public bool CheckIfEntityHaveAutomation(string objectTableName, string type, int tenant)
        {
            bool result = false;

            string tableName = objectTableName;
            if (objectTableName == "MasterAndHouse") tableName = "Shipment";

            ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
            ObjectTable objecttable = objecttableRepository.GetObjectTableByName(tableName, 0, true);

            if (objecttable != null)
            {
                AutomationRepository automationRepository = new AutomationRepository(tenant);
                result = automationRepository.CheckIfEntityHaveAutomation(objecttable.Id, type, tenant);
                if (!result && objectTableName == "MasterAndHouse")
                {
                    objecttable = objecttableRepository.GetObjectTableByName("Master", 0, true);
                    result = automationRepository.CheckIfEntityHaveAutomation(objecttable.Id, type, tenant);
                }
            }

            return result;
        }

        private string GetLastUpdateDate(string lastupdateautomation, string otherLastupdateautomation, Automation automation)
        {
            string lastUpdate = lastupdateautomation;
            if (!string.IsNullOrEmpty(otherLastupdateautomation))
            {
                string otherObjectTableId = otherLastupdateautomation.Split('@')[0];

                if (automation.ObjectTableId == otherObjectTableId)
                {
                    lastUpdate = otherLastupdateautomation.Split('@')[1];
                }
            }

            return lastUpdate;
        }

        public static bool IsShowLogBoxAutomationFields()
        {
            bool result = false;
            if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && (LogitudeSettings.DeploymentStage.ToLower() == "logboxpre" || LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1" || LogitudeSettings.DeploymentStage.ToLower() == "test2" || LogitudeSettings.LogitudeURL == "http://localhost:9996"))
            {
                result = true;
            }
            return result;
        }



    }
}