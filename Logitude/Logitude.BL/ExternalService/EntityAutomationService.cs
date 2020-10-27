
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.EntityChanges;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ExternalService
{
    public  class EntityAutomationService
    {
        private object entityPM = null;
        private object poco = null;
        private string objectTableName = string.Empty;
        private List<ObjectFieldPM> automationObjectFields = null;
        private int tenant;
        private string automationType = string.Empty;
        private object oldEntityPM = null;
        private string entityId = string.Empty;
        public EntityAutomationService(EntityAutomationArgs args)
        {
            this.entityPM = args.EntityPM;
            this.poco = args.Poco;
            this.tenant = args.Tenant;
            this.objectTableName = args.ObjectTableName;
            this.automationType = args.AutomationType;
            this.entityId = args.EntityId;
            if (automationType != "OnCreate")
            {
                this.automationObjectFields = GetObjectFieldsUsedInAutomation();
                this.oldEntityPM = args.OldEntityPM;
                MapAutomationFieldsFormPocoToEntityPM(poco, this.oldEntityPM);
            }

            if (args.EntityAutomationMappingPMFields != null)
            {
                args.EntityAutomationMappingPMFields.Map(this.entityPM, this.oldEntityPM);
            }
        }

        public void RunAutomation()
        {
            string entityChangeFieldXml = automationType == "OnCreate" ? "" : GetEntityChangeFieldXml();

            var mainEntityChangeService = new MainEntityChangeService(new EntityChangeArgs() {EntityPM = entityPM, OldEntityPM= oldEntityPM , ProcessType = automationType, EntityChangeFieldXml = entityChangeFieldXml  , ObjectTableName = objectTableName , EntityId = this.entityId, Tenant = tenant, StartDate = DateTime.Now });
            mainEntityChangeService.AddEntityChange();

        }

        private List<ObjectFieldPM> GetObjectFieldsUsedInAutomation()
        {
            List<ObjectFieldPM> objectFieldLists = new List<ObjectFieldPM>();
            ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
            ObjectTable objecttable = objecttableRepository.GetObjectTableByName(objectTableName, 0, true);
            if (objecttable != null)
            {
                ObjectFieldQuery objectFieldQuery = new ObjectFieldQuery(tenant);
                objectFieldLists = objectFieldQuery.GetObjectFieldsAllowedinAutomationConditionsPMsByObjectTableId(objecttable.Id, tenant);
            }
            return objectFieldLists;
        }

        private string GetEntityChangeFieldXml()
        {
            var notifyPropertyChangeValuesLists = BuildChangedProperties(entityPM, oldEntityPM);
            return EntityPMChangeTrackingHelper.GetChangesDetectedXml(notifyPropertyChangeValuesLists);
        }

        private void MapAutomationFieldsFormPocoToEntityPM( object poco , object entityPM)
        {
            if (automationObjectFields != null)
            {
                foreach (ObjectFieldPM objectFieldPM in automationObjectFields)
                {
                    object value = GetPropertyValue(poco, objectFieldPM.FieldName);
                    if (objectFieldPM.IsCustom)
                    {
                        value = new CustomFieldClass(objectFieldPM.FieldName, objectTableName, value!=null ? value.ToString():"");
                    }
                    PropertyInfo propInfo = entityPM.GetType().GetProperty(objectFieldPM.FieldName);
                    propInfo.SetValue(entityPM, value, null);
                }
            }

        }

        private object GetPropertyValue(object entity, string fieldName)
        {
            Type type = entity.GetType();
            PropertyInfo propertyInf = type.GetProperty(fieldName);
            object value = null;
            if (propertyInf != null)
            {
                value = propertyInf.GetValue(entity, null);
            }
            return value;
        }

        public List<NotifyPropertyChangeValues> BuildChangedProperties(object entityPM, object oldEntityPM)
        {
            List<NotifyPropertyChangeValues> notifyPropertyChangeValuesLists = new List<NotifyPropertyChangeValues>();
            if (automationObjectFields != null)
            {
                foreach (ObjectFieldPM objectFieldPM in automationObjectFields)
                {
                    object oldValue = GetPropertyValue(oldEntityPM, objectFieldPM.FieldName);
                    object newValue = GetPropertyValue(entityPM, objectFieldPM.FieldName);
                    NotifyPropertyChangeValues notifyPropertyChangeValues = GetNotifyPropertyChangeValues(new NotifyPropertyChangeArgs() { PropertyName = objectFieldPM.FieldName, PropertyType = objectFieldPM.DataTypeCode, OldValue = oldValue, NewValue = newValue, IsCustom = objectFieldPM.IsCustom });
                    if (notifyPropertyChangeValues != null) notifyPropertyChangeValuesLists.Add(notifyPropertyChangeValues);
                }
            }
            return notifyPropertyChangeValuesLists;

        }

        private NotifyPropertyChangeValues GetNotifyPropertyChangeValues(NotifyPropertyChangeArgs args)
        {
            NotifyPropertyChangeValues notifyPropertyChangeValues = null;
            string oldValue = GetValueFromObject(args.OldValue, args.IsCustom);
            string newValue = GetValueFromObject(args.NewValue, args.IsCustom);
            if (oldValue != newValue)
            {
                notifyPropertyChangeValues = new NotifyPropertyChangeValues() { PropertyName = args.PropertyName, OldValue = oldValue, NewValue = newValue, PropertyType = args.IsCustom ? "CustomFieldClass" : args.PropertyType };
            }
            return notifyPropertyChangeValues;
        }

        private string GetValueFromObject(object value, bool isCustom)
        {
            string result = "";
            if (value != null)
            {
                object objectValue = isCustom ? GetPropertyValue(value, "Value") : value;
                if (objectValue != null)
                {
                    result = objectValue.ToString();
                }
            }
            return result;
        }

    }

    public class NotifyPropertyChangeArgs
    {
        public string PropertyName { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public bool IsCustom { get; set; }
        public string PropertyType { get; set; }
    }

    public class EntityAutomationArgs
    {
        public object EntityPM { get; set; }
        public object OldEntityPM { get; set; }

        public object Poco { get; set; }

        public string AutomationType { get; set; }
        public string ObjectTableName { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public IEntityAutomationMappingPMFields EntityAutomationMappingPMFields { get; set; }
    }

}
