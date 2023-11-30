using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Services
{
    public class QueryTreeFilterFieldValueResolver
    {

        public static object Get(object parentEntity, string fieldName, string fieldDataType)
        {
            if (parentEntity.GetType().Name == "JObject")
            {
                return GetFromDynamicEntity(parentEntity, fieldName, fieldDataType);
            }
            else
            {
                return GetFromEntity(parentEntity, fieldName, fieldDataType);
            }
        }

        public static object GetFromDynamicEntity(object parentEntity, string fieldName, string fieldDataType)
        {
            dynamic dynamicParentEntity = parentEntity;
            object value = dynamicParentEntity[fieldName];
            if (value == null) return null;

            if (value.GetType().Name == "JObject")
            {
                return GetCustomFieldValue(value, fieldDataType);
            }

            if (string.IsNullOrEmpty(value.ToString())) return null;

            return FieldValueResolver.GetFieldDataValue(new ObjectField { DataTypeCode = fieldDataType }, value.ToString());
        }

        private static object GetCustomFieldValue(object value, string fieldDataType)
        {
            dynamic customFieldValue = value;

            if (customFieldValue["value"] == null || customFieldValue["value"] == "") return "";

            CustomFieldClass customFilterClass = new CustomFieldClass();
            return customFilterClass.SetFieldDataType(fieldDataType, customFieldValue["value"]);
        }
        public static object GetFromEntity(object parentEntity, string fieldName, string fieldDataType)
        {
            PropertyInfo propertyInfo = GetProperty(parentEntity, fieldName);
            if (propertyInfo == null) return "";
            object value = propertyInfo.GetValue(parentEntity, null);

            if (value == null) return "";

            if (value.GetType() != typeof(CustomFieldClass))
            {
                return value;
            }

            CustomFieldClass customFilterClass = new CustomFieldClass();
            return customFilterClass.SetFieldDataType(fieldDataType, (value as CustomFieldClass).Value);
        }

        private static PropertyInfo GetProperty(object entity, string FieldName)
        {
            Type type = entity.GetType();
            return type.GetProperty(FieldName);
        }
    }
}
