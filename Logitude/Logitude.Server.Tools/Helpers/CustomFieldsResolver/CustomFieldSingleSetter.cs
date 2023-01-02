using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.Helpers.CustomFieldsResolver
{
    public class CustomFieldSingleSetter
    {
        public static void SetFieldValue(CustomFieldResolverArgs customFieldResolverArgs)
        {
            List<ObjectField> customObjectFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName(customFieldResolverArgs.ObjectTableName, customFieldResolverArgs.Tenant).ToList();
            ObjectField customObjectField = customObjectFields.FirstOrDefault(f => f.Code.Replace(" ", "") == customFieldResolverArgs.FieldCode);
            if (customObjectField == null) return;

            PropertyInfo propInfo = customFieldResolverArgs.EntityPM.GetType().GetProperty(customObjectField.FieldName);
            if (propInfo == null) return;

            CustomFieldClass customFilterClass = new CustomFieldClass();
            string customFieldValue = customFilterClass.SetFieldDataType(customObjectField.DataTypeCode, customFieldResolverArgs.FieldValue);
            customFieldValue = ResolveCustomFieldValue(customObjectField, customFieldValue, customFieldResolverArgs.Tenant);
            propInfo.SetValue(customFieldResolverArgs.EntityPM, new CustomFieldClass(customObjectField.FieldName, customFieldResolverArgs.ObjectTableName, customFieldValue));
        }

        private static string ResolveCustomFieldValue(ObjectField objectField, string fieldValue, int tenant)
        {
            //For now we handle picklist type
            if (objectField.DataTypeCode == "PickList")
            {
                return HandleCustomPickListField(objectField, fieldValue, tenant);
            }
            return fieldValue;
        }

        private static string HandleCustomPickListField(ObjectField objectField, string fieldValue, int tenant)
        {
            if (fieldValue == null) return "";

            CustomPickListRepository customPickListRepository = new CustomPickListRepository(tenant);
            CustomPickList picklist = customPickListRepository.GetSingleCustomPickListByValue(objectField.CustomPickListCode, fieldValue, tenant);
            if (picklist != null) return picklist.Id;

            return fieldValue;
        }
    }
}
