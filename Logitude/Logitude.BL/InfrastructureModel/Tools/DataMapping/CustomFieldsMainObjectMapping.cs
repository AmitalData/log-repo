using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class CustomFieldsMainObjectMapping
    {
        private static readonly int numberOfCustomFields = 50;
        public static void MapEntity(CustomFieldsMainObjectPM entityPM, CustomFieldsMainObject entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
            entityPOCO.ObjectTableId = entityPM.ObjectTableId;
            entityPOCO.EntityId = entityPM.EntityId;
            MapCustomFields(entityPM , entityPOCO);
        }

        private static void MapCustomFields(CustomFieldsMainObjectPM entityPM, CustomFieldsMainObject entityPOCO)
        {
            for (int i = 1; i <= numberOfCustomFields; i++)
            {
                MapCustomFieldValue(("Field" + i.ToString()) , entityPM , entityPOCO);
            }
        }

        private static void MapCustomFieldValue( string fieldName, CustomFieldsMainObjectPM entityPM, CustomFieldsMainObject entityPOCO)
        {
            var customFieldValue = GetFieldValue(entityPM, fieldName);
            var fieldProperty = entityPOCO.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (fieldProperty == null) return;
            fieldProperty.SetValue(entityPOCO, customFieldValue, null);
        }

        private static object GetFieldValue(CustomFieldsMainObjectPM entityPM, string fieldName)
        {
            var fieldProperty = entityPM.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (fieldProperty == null) return null;

            var customFieldValue = fieldProperty.GetValue(entityPM, null);
            if (customFieldValue != null && customFieldValue.GetType() == typeof(CustomFieldClass))
            {
                CustomFieldClass c = customFieldValue as CustomFieldClass;
                customFieldValue = c.Value;
            }
            return customFieldValue;
        }
    }
}
