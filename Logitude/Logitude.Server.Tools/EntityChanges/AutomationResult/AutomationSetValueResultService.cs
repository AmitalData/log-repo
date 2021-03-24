using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
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
   public class AutomationSetValueResultService : GeneralAutomationResultService, IAutomationResultService
    {


        AutomationResultArgs automationResultArgs { get; set; }
        public void Run(AutomationResultArgs automationResultArgs)
        {
            this.automationResultArgs = automationResultArgs;
            List<Automation> fieldSetAutomationsList = automationResultArgs.AutomationLists.Where(d => d.ResultCode == "FIELDSET").ToList();
            if (fieldSetAutomationsList.Count > 0)
            {

              var otherObjectTableIdWithLastUpdate = (automationResultArgs.OtherAutomationObjectTable != null && !string.IsNullOrEmpty(automationResultArgs.OtherAutomationObjectTable.AutomationLastUpdate)) ? (automationResultArgs.OtherAutomationObjectTable.Id + "@" + automationResultArgs.OtherAutomationObjectTable.AutomationLastUpdate) : "";


                ApplySetValueAutomation(automationResultArgs.EntityChangeArgs.EntityPM, fieldSetAutomationsList, automationResultArgs.EntityChange, automationResultArgs.AutomationFieldLists, automationResultArgs.AutomationObjectTable.AutomationLastUpdate, automationResultArgs.EntityChangeArgs.OldEntityPM, automationResultArgs.EntityChangeArgs.ProcessType, automationResultArgs.EntityChangeArgs.EntityId, otherObjectTableIdWithLastUpdate);
            }
        }

        public void ApplySetValueAutomation(Object entityPM, List<Automation> automationsList, EntityChange entityChange, List<Field> automationFieldLists, string lastupdateautomation, Object oldentityPM, string processtype, string entityId, string otherLastupdateautomation)
        {
            int tenant = entityChange.Tenant;
            ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);

            foreach (Automation automation in automationsList)
            {
                DateTime dateBefore = DateTime.Now;
                EntityChangeAutomation entityChangesAutomation = CreateEntityChangeAutomation(automation);

                if (automation.ResultCode == "FIELDSET") entityChangesAutomation.ResultCode = "Set Fields Value";

                string lastUpdate = GetLastAuomationUpdateDate(automationResultArgs.AutomationObjectTable, automationResultArgs.OtherAutomationObjectTable, automation);

                ValidateAutomationResultClass validateResult = ValidateAutomation(automation, entityChange, automationFieldLists, lastUpdate, "");
                entityChangesAutomation.ConditionsList = validateResult.ConditionsList;

                var isShipmentSetFieldDelayed = (validateResult.IsAutomationValid && automation.ResultCode == "FIELDSET" && validateResult.Type == "Delayed") ? objecttableRepository.IsObjectTableShipment(automation.ObjectTableId) : false;
                if (validateResult.Type == "Delayed" && !isShipmentSetFieldDelayed)
                {
                    this.automationResultArgs.MainEntityChangeService.IsDelayAutomation = true;
                }

                entityChangesAutomation.type = validateResult.IsAutomationValid ? "SetSsucceed" : "SetFailed";

                if (validateResult.IsAutomationValid)
                {
                    if (validateResult.Type == "Delayed" && !isShipmentSetFieldDelayed)
                    {
                        DelaytimeDetails delaytimeDetails = new DelaytimeDetails() { Type = validateResult.Type, Delaytime = validateResult.Delaytime, DelaytimeIndicator = validateResult.DelaytimeIndicator, DelaytimeOp = validateResult.DelaytimeOp, SelectedDelaytimeFieldCode = validateResult.SelectedDelaytimeFieldCode };
                        AddAutomationQueue(new AutomationQueueArgs() { EntityChangeId = entityChange.Id, AutomationId = automation.Id, AutomationType = processtype, EntityId = entityId, Tenant = automation.Tenant, AutomationDelayTime = GetAutomationDelayTime(delaytimeDetails, automationFieldLists, entityChange.Tenant) });
                    }
                    else SetValue(entityPM, entityChange, automationFieldLists, lastUpdate, this.automationResultArgs.MainEntityChangeService.EntityChangesAutomationsSsucceedList, this.automationResultArgs.MainEntityChangeService.Changefields, automation, entityChangesAutomation, dateBefore);
                }
                else
                { 
                    entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
                    this.automationResultArgs.MainEntityChangeService.EntityChangesAutomationsFailedList.Add(entityChangesAutomation);
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


            foreach (AutomationSetValue item in AutomationSetValueLists)
            {
                PropertyInfo propInfo = entityPM.GetType().GetProperty(item.FieldName);
                if (propInfo != null)
                {
                    object oldValue = propInfo.GetValue(entityPM);
                    object newValue = ResolveSetFieldValue(automationFieldLists, item);

                    if (oldValue == null) oldValue = "";
                    if (newValue == null) newValue = "";

                    if (oldValue.ToString().ToLower() != newValue.ToString().ToLower())
                    {
                        if ((item.DataTypeCode.Trim() == "DateTime" || item.DataTypeCode.Trim() == "Date") && item.OperatorCode == "SF" && string.IsNullOrEmpty(newValue.ToString()))
                        {
                            newValue = null;
                        }

                        propInfo.SetValue(entityPM, newValue, null);
                        c fieldc = new c()
                        {
                            f = item.ObjectFieldCode,
                            o = oldValue.ToString(),
                            n = newValue == null ? null : newValue.ToString(),
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

        private object ResolveSetFieldValue(List<Field> automationFieldLists, AutomationSetValue item)
        {
            object result = null;

            if (item.OperatorCode.Contains("F"))
            {
                Field field = automationFieldLists.Where(d => d.FieldCode == item.Value).FirstOrDefault();
                if (field != null) result = field.Value;
                if ((item.DataTypeCode.Trim() == "DateTime" || item.DataTypeCode.Trim() == "Date") && item.OperatorCode == "SF" && !string.IsNullOrEmpty(field.Value))
                {
                    result = ConvertToDate(field.Value);
                }
            }

            else if (item.DataTypeCode.Trim() == "DateTime" || item.DataTypeCode.Trim() == "Date")
            {
                var dateSplitParts = item.Value.Split('*');
                result = ConvertToDate(dateSplitParts[dateSplitParts.Length - 1]);
            }

            else if (item.DataTypeCode.Trim() == "Boolean")
            {
                if (!string.IsNullOrEmpty(item.Value)) result = !string.IsNullOrEmpty(item.Value) && item.Value.ToLower() == "true" ? true : false;
            }

            else if (item.DataTypeCode.Trim() == "Integer")
            {
                result = !string.IsNullOrEmpty(item.Value) ? Int32.Parse(item.Value) : 0;
            }
            else result = item.Value;


            return result;
        }

    }
}
