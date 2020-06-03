using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
public  class UpdateEventCustomFieldService
    {

        #region UpdateEventCustomFieldValue
        public static void UpdateEventCustomFieldValue(UpdateEventCustomFieldArgs args)
        {
            ObjectField objectField = GetCustomObjectFieldConnectedToEvent(args.CustomField, args.EventTypeId, args.Tenant);
            if (objectField != null)
            {
                object entity = args.Entity;
                CustomFieldClass customFieldValue = new CustomFieldClass(objectField.FieldName, args.ObjectTableName, new CustomFieldClass().SetFieldDataType(objectField.DataTypeCode, args.EventDateTime));
                if (entity == null)
                {
                    entity = InjectionUtil.Instance.GetEntityByObjectTableNameAndEntityId(args.ObjectTableName, args.EntityId, args.Tenant);
                    if (entity != null)
                    {
                        SetPropertyValueToEntity(objectField, entity, customFieldValue);
                        InjectionUtil.Instance.UpdateEntity(entity, args.ObjectTableName, args.Tenant);
                    }
                }
                else SetPropertyValueToEntity(objectField, entity, customFieldValue);
            }
        }
        private static ObjectField GetCustomObjectFieldConnectedToEvent(string customFieldName, string eventTypeId, int tenant)
        {
            ObjectField objectField = null;
            string customField = !string.IsNullOrEmpty(customFieldName) ? customFieldName : new EventTypeRepository(tenant).GetCustomFieldByEventTypeId(eventTypeId, tenant);
            if (!string.IsNullOrEmpty(customField))
            {
                ObjectFieldRepository objectFieldRepository = new ObjectFieldRepository(tenant);
                objectField = objectFieldRepository.GetSingleObjectFieldByFieldCode(customField, tenant);
            }
            return objectField;
        }
        private static void SetPropertyValueToEntity(ObjectField objectField, object entity, object fieldValue)
        {
            PropertyInfo propInfo = entity.GetType().GetProperty(objectField.FieldName);
            if (propInfo != null) propInfo.SetValue(entity, fieldValue, null);
        }
        #endregion
    }
}
