using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges
{
   public class ResolverAutomationObjectFieldService
    {
        EntityChangeArgs entityChangeArgs { get; set; }

        public ResolverAutomationObjectFieldService(EntityChangeArgs entityChangeArgs)
        {
            this.entityChangeArgs = entityChangeArgs;

        }


        public List<Field> GetExternalEntityAutomationConditionFieldLists(List<ObjectField> automationsObjectFieldLists, List<Field> automationConditionFieldLists, int tenant)
        {
            List<Field> externalEntityAutomationConditionFieldLists = new List<Field>();
            foreach (ObjectField externalEnitityObjectField in automationsObjectFieldLists.Where(d => d.DisplayInAutomationAsEnitity).ToList())
            {
                Field externalEntityAutomationConditionField = automationConditionFieldLists.Where(d => d.FieldCode == externalEnitityObjectField.FieldCode).FirstOrDefault();
                if (externalEntityAutomationConditionField != null)
                {
                    List<ObjectField> externalEntityAutomationObjectFieldLists = automationsObjectFieldLists.Where(d => d.ObjectTableId == externalEnitityObjectField.LookUpTableId).ToList();
                    if (externalEntityAutomationObjectFieldLists.Count() > 0)
                    {
                        object externalEntity = GetEntityByIdAndObjectField(externalEntityAutomationConditionField.Value, externalEnitityObjectField, tenant);
                        List<Field> externalEntityAutomationConditionFieldsValueLists = GetExternalEntityAutomationConditionFieldsValueFromEntity(externalEnitityObjectField, externalEntityAutomationObjectFieldLists, externalEntity);
                        externalEntityAutomationConditionFieldLists = externalEntityAutomationConditionFieldLists.Concat(externalEntityAutomationConditionFieldsValueLists).ToList();
                    }
                }
            }
            return externalEntityAutomationConditionFieldLists;
        }

        private object GetEntityByIdAndObjectField(string entityId, ObjectField objectField, int tenant)
        {
            object automationEntity = null;
            if (!string.IsNullOrEmpty(entityId) && objectField.ObjectTable_LookUpTable != null)
            {
                if (objectField.ObjectTable_LookUpTable.Name == "Shipment" && entityChangeArgs.ExternalEntity != null) automationEntity = entityChangeArgs.ExternalEntity;
                else automationEntity = InjectionUtil.Instance.GetEntityByObjectTableNameAndEntityId(objectField.ObjectTable_LookUpTable.Name, entityId, tenant);
            }

            return automationEntity;
        }

        private List<Field> GetExternalEntityAutomationConditionFieldsValueFromEntity(ObjectField externalEnitityObjectField, List<ObjectField> externalEntityAutomationObjectFieldLists, object externalEntity)
        {
            List<Field> automationFieldLists = new List<Field>();
            foreach (ObjectField externalEntityAutomationObjectField in externalEntityAutomationObjectFieldLists)
            {
                string currentvalue = externalEntity != null ? GetValue(externalEntity, externalEntityAutomationObjectField) : "";
                Field automationConditionField = new Field() { FieldCode = externalEntityAutomationObjectField.FieldCode, Value = currentvalue != null ? currentvalue : "", OldValue = "", PropertyName = externalEntityAutomationObjectField.FieldName, PartnerObjectFieldCode = externalEnitityObjectField.FieldCode };
                automationFieldLists.Add(automationConditionField);

            }

            return automationFieldLists;
        }



        public List<Field> GetAutomationObjectFieldValueLists(List<ObjectField> automationsObjectFieldLists, AutomationObjectTableClass automationObjectTableClass)
        {
            List<Field> automationConditionFieldLists = new List<Field>();

            foreach (ObjectField objectField in automationsObjectFieldLists.Where(d => d.ObjectTableId == automationObjectTableClass.OriginalId).ToList())
            {
                string currentvalue = GetValue(entityChangeArgs.EntityPM, objectField);
                string oldvalue = "";
                bool ischange = false;

                if (entityChangeArgs.OldEntityPM != null)
                {
                    oldvalue = GetValue(entityChangeArgs.OldEntityPM, objectField);
                    if (currentvalue != oldvalue) ischange = true;
                }

                if (automationObjectTableClass.OriginalName == "Shipment")
                {
                    if (objectField.FieldName == "AgentContactId" && string.IsNullOrEmpty(currentvalue)) currentvalue = GetAgentContactIdValue(automationsObjectFieldLists);
                }

                Field automationConditionField = new Field() { FieldCode = objectField.FieldCode, Value = currentvalue != null ? currentvalue : "", OldValue = oldvalue != null ? oldvalue : "", IsChange = ischange, PropertyName = objectField.FieldName, };
                automationConditionFieldLists.Add(automationConditionField);
            }

            return automationConditionFieldLists;
        }

        private string GetAgentContactIdValue(List<ObjectField> automationsObjectFieldLists)
        {
            string result = string.Empty;
                var shipmentLevelObjectField = automationsObjectFieldLists.Where(d => d.FieldName == "ShipmentLevelCode").FirstOrDefault();
                string shipmentLevelCode = GetValue(entityChangeArgs.EntityPM, shipmentLevelObjectField);
            if (shipmentLevelCode == "H")
            {
                PropertyInfo propInfo = entityChangeArgs.EntityPM.GetType().GetProperty("MasterShipmentDataId");
                if (propInfo != null)
                {
                    var masterShipmentDataId = propInfo.GetValue(entityChangeArgs.EntityPM);
                    if (masterShipmentDataId != null)
                    {
                        if (!string.IsNullOrEmpty(masterShipmentDataId.ToString()))
                        {
                            ShipmentRepository shipmentRepository = new ShipmentRepository(entityChangeArgs.Tenant);
                            result = shipmentRepository.GetAgentContactByShipemntId(masterShipmentDataId.ToString(), entityChangeArgs.Tenant);
                        }
                    }
                }
            }
          
            

            return result;
        }

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

      
    }
}
