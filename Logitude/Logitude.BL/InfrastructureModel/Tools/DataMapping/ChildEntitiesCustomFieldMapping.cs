using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class ChildEntitiesCustomFieldMapping
    {
        public static void MapPocoToPM<T>(T entityPM, string childObjectTableName)
        {
            ChildEntitiesCustomField childEntitiesCustomField = GetChildEntitiesCustomField(entityPM, childObjectTableName);
            int maxNumberOfCustomFields = ChildEntitiesCustomFieldRepository.MaxNumberOfCustomFields;
            
            for (int i = 1; i <= maxNumberOfCustomFields; i++)
            {
                MapPocoFieldValueToPM(childEntitiesCustomField, entityPM, "Field" + i, childObjectTableName);
            }
        }

        private static void MapPocoFieldValueToPM<T>(ChildEntitiesCustomField ChildEntitiesCustomField, T entityPM, string targetField, string objectTableName)
        {
            string fieldValue = GetValue(ChildEntitiesCustomField, targetField)?.ToString();
            CustomFieldClass customFieldClassValue = new CustomFieldClass(targetField, objectTableName, fieldValue);
            SetValue(entityPM, targetField, customFieldClassValue);
        }

        public static ChildEntitiesCustomField MapPMTOPoco<T>(T childEntityPM, string childObjectTableName)
        {
            ChildEntitiesCustomField childEntitiesCustomField = GetChildEntitiesCustomField(childEntityPM, childObjectTableName);
            int maxNumberOfCustomFields = ChildEntitiesCustomFieldRepository.MaxNumberOfCustomFields;
            
            for (int i = 1; i <= maxNumberOfCustomFields; i++)
            {
                MapPMFieldValueToPoco(childEntitiesCustomField, childEntityPM, "Field" + i);
            }

            return childEntitiesCustomField;
        }

        private static ChildEntitiesCustomField GetChildEntitiesCustomField<T>(T entityPM, string childObjectTableName)
        {
            int tenant = (int)GetValue(entityPM, "Tenant");
            string childEntityId = GetValue(entityPM, "Id")?.ToString();
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(tenant);
            ObjectTablePM objectTablePM = objectTableQuery.GetObjectTableByName(childObjectTableName, tenant);
            if (objectTablePM == null) throw new ApplicationException("Child Object Table Does not Exist!");
            ChildEntitiesCustomFieldQuery childEntitiesCustomFieldQuery = new ChildEntitiesCustomFieldQuery(tenant);
            ChildEntitiesCustomField childEntitiesCustomField = childEntitiesCustomFieldQuery.GetSingle(childEntityId, objectTablePM.Id, tenant);
            //if childEntitiesCustomField null return new one 
            if(childEntitiesCustomField != null)
            {
                return childEntitiesCustomField;
            }
            childEntitiesCustomField = new ChildEntitiesCustomField
            {
                Id = IdCounter.GetNumber("ChildEntitiesCustomField", tenant).ToString(),
                ChildEntityId = childEntityId,
                ChildObjectTableId = objectTablePM.Id
            };
            return childEntitiesCustomField;
        }

        private static void MapPMFieldValueToPoco<T>(ChildEntitiesCustomField ChildEntitiesCustomField, T entityPM, string targetField)
        {
            object fieldValue = GetValue(entityPM, targetField);
            if (fieldValue == null) return;
            SetValue(ChildEntitiesCustomField, targetField, ((CustomFieldClass)fieldValue).Value);
        }

        private static void SetValue(object model, string field, object value)
        {
            PropertyInfo propertyInfo = model.GetType().GetProperty(field);
            propertyInfo.SetValue(model, value, null);
        }

        private static object GetValue(object model, string field)
        {
            PropertyInfo propertyInfo = model.GetType().GetProperty(field);
            return propertyInfo.GetValue(model, null);
        }
    }
}
