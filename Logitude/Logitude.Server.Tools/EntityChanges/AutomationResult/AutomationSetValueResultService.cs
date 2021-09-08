using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
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
                        AutomationSetValueLists = GetAutomationSetValueLists(automation);
                        InsertAutomationListIntoCash(AutomationSetValueLists, automationSetValueListsName);
                    }
                }
                else
                {
                    AutomationSetValueLists = (List<AutomationSetValue>)CacheManager.CacheWrapper.Get(automationSetValueListsName);
                    if (hasCustomFieldSetValue(AutomationSetValueLists))
                    {
                        AutomationSetValueLists = GetAutomationSetValueLists(automation); 
                    }
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
                    AutomationSetValueChangedFieldsArgs AutomationChangedFields = new AutomationSetValueChangedFieldsArgs
                    {
                        EntityPM = entityPM,
                        AutomationFieldLists = automationFieldLists,
                        ChangedFields = fields,
                        SetValueItem = item,
                        PropInfo = propInfo,
                    };
                    fields = FillChangedFieldsList(AutomationChangedFields);
                }
            }
            #endregion

            entityChange.HasExecutedRecord = true;
            entityChangesAutomation.IsConditionTrue = true;
            entityChangesAutomation.DoneDate = TenantServerConfigration.GetCurrentDateTime(entityChange.Tenant);
            EntityChangesAutomationsSsucceedList.Add(entityChangesAutomation);
            entityChangesAutomation.ExecutionTime = (int)((DateTime.Now.Ticks - dateBefore.Ticks) / TimeSpan.TicksPerMillisecond);
        }

        private static bool hasCustomFieldSetValue(List<AutomationSetValue> AutomationSetValueLists)
        {
            return AutomationSetValueLists.Find(item => item.IsCustomField == true) != null;
        }

        private static void InsertAutomationListIntoCash(List<AutomationSetValue> AutomationSetValueLists, string automationSetValueListsName)
        {
            CacheManager.CacheWrapper.Insert(automationSetValueListsName, AutomationSetValueLists, null, System.DateTime.UtcNow.AddHours(12), TimeSpan.Zero);
        }

        private static List<AutomationSetValue> GetAutomationSetValueLists(Automation automation)
        {
            List<AutomationSetValue> AutomationSetValueLists;
            AutomatedBackup AutomatedBackup = LogitudeXmlSerializer.DeserializeObject<AutomatedBackup>(automation.AutomationXML);
            AutomationSetValueLists = AutomatedBackup.AutomationSetValueLists;
            return AutomationSetValueLists;
        }

        private List<c> FillChangedFieldsList(AutomationSetValueChangedFieldsArgs AutomationChangedFields)
        {
            object oldValue = AutomationChangedFields.PropInfo.GetValue(AutomationChangedFields.EntityPM) ?? "";
            object newValue = ResolveSetFieldValue(AutomationChangedFields.AutomationFieldLists, AutomationChangedFields.SetValueItem) ?? "";

            if (!IsValueChanged(oldValue, newValue) && !AutomationChangedFields.SetValueItem.IsCustomField)
            {
                return AutomationChangedFields.ChangedFields;
            }
             
            newValue = IsNewValueShouldBeNull(AutomationChangedFields, newValue) ? null : newValue;
            newValue = GetNewValueForCustomField(AutomationChangedFields, newValue);
            AutomationChangedFields.PropInfo.SetValue(AutomationChangedFields.EntityPM, newValue, null);
            return AddFieldToChangedFieldsList(AutomationChangedFields, oldValue, newValue);
        }

        private static object GetNewValueForCustomField(AutomationSetValueChangedFieldsArgs AutomationChangedFields, object newValue)
        {

            if (HasStringType(newValue)  || HasIntergerType(newValue) || HasBooleanType(newValue))
            {
                if (AutomationChangedFields.SetValueItem.IsCustomField)
                {
                    AutomationChangedFields.SetValueItem.Value = newValue.ToString();
                    newValue = GetNewCustomFieldClass(AutomationChangedFields.SetValueItem);
                }
            }
            return newValue;
        }
  
        private static bool HasStringType(object objectValue)
        {
            return objectValue?.GetType().Name == "String"; 
        }
        private static bool HasBooleanType(object objectValue)
        {
            return objectValue?.GetType().Name == "Boolean"; 
        } 
        private static bool HasIntergerType(object objectValue)
        {
            return objectValue?.GetType().Name == "Int32"; 
        }


        private object ResolveSetFieldValue(List<Field> automationFieldLists, AutomationSetValue item)
        {
            object result = null;

            if (item.OperatorCode.Contains("F") && item.DataTypeCode.Trim() != "Boolean")
            {
                Field field = automationFieldLists.Where(d => d.FieldCode == item.Value).FirstOrDefault();
                if (field != null) result = field.Value;
                if ((hasDateTypeField(item)) && item.OperatorCode == "SF" && !string.IsNullOrEmpty(field.Value) && !item.IsCustomField)
                {
                    result = ConvertToDate(field.Value);
                }
            }


            else if (item.IsCustomField && hasDateTypeField(item))
            {
                item.Value = GetDateValue(item);
                return GetNewCustomFieldClass(item);
            }

            else if (hasDateTypeField(item))
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
            else if (item.IsCustomField)
            {
                result = GetNewCustomFieldClass(item);
            }
            else result = item.Value;


            return result;
        }

        private static string GetDateValue(AutomationSetValue item)
        {
            var dateParts = item.Value.Split('*');
            if (item.Value.IndexOf("Today") > -1)
            {
                if(item.DataTypeCode == "Date")
                {
                    return dateParts[2].Substring(0, 8);
                }
                return dateParts[2];
            }
            else if (item.Value.IndexOf("Date") > -1)
            {
                return dateParts[1];
            }
            return null;
        }

        private static bool hasDateTypeField(AutomationSetValue item)
        {
            return item.DataTypeCode.Trim() == "DateTime" || item.DataTypeCode.Trim() == "Date";
        }

        private static object GetNewCustomFieldClass(AutomationSetValue item)
        {
            string[] objectFieldSeparator = item.ObjectFieldCode.Split('.');
            if (objectFieldSeparator.Length > 2)
            {
                return new CustomFieldClass(objectFieldSeparator[2], objectFieldSeparator[0], item.Value);
            }
            return null;
        }

        private static bool IsValueChanged(object oldValue, object newValue)
        {
            return oldValue.ToString().ToLower() != newValue.ToString().ToLower();
        }

        private static bool IsNewValueShouldBeNull(AutomationSetValueChangedFieldsArgs AutomationChangedFields, object newValue)
        {
            return (AutomationChangedFields.SetValueItem.DataTypeCode.Trim() == "DateTime" || AutomationChangedFields.SetValueItem.DataTypeCode.Trim() == "Date") && AutomationChangedFields.SetValueItem.OperatorCode == "SF" && string.IsNullOrEmpty(newValue.ToString());
        }

        private static List<c> AddFieldToChangedFieldsList(AutomationSetValueChangedFieldsArgs AutomationChangedFields, object oldValue, object newValue)
        {
            List<c> newChangedFields = AutomationChangedFields.ChangedFields;
            string fieldOldValue = ResolveFieldValue(oldValue);
            string fieldNewValue = ResolveFieldValue(newValue);

            c changedField = new c()
            {
                f = AutomationChangedFields.SetValueItem.ObjectFieldCode,
                o = fieldOldValue,
                n = fieldNewValue,
            };
            newChangedFields.Add(changedField);

            return newChangedFields;
        }

        private static string ResolveFieldValue(object value)
        {
            if (value != null && value.GetType() == typeof(CustomFieldClass))
            {
                CustomFieldClass customField = value as CustomFieldClass;
                return customField.Value;
            }
            return value?.ToString();
        }
    }
}

public class AutomationSetValueChangedFieldsArgs
{
    public object EntityPM { get; set; }
    public List<Field> AutomationFieldLists { get; set; }
    public List<c> ChangedFields { get; set; }
    public AutomationSetValue SetValueItem { get; set; }
    public PropertyInfo PropInfo { get; set; }
}