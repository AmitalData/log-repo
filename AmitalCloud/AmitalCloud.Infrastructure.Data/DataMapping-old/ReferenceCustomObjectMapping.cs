using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class ReferenceCustomObjectMapping
    {
        private static readonly int numberOfCustomFields = 50;
        public static void MapEntity(ReferenceCustomObjectPM entityPM, ReferenceCustomObject entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.CreatedBy = entityPM.CreatedBy;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedBy = entityPM.UpdatedBy;
            entityPOCO.ObjectTableId = entityPM.ObjectTableId;
            entityPOCO.InActive = entityPM.InActive;
            MapCustomFields(entityPM, entityPOCO);
            BuildSearchFields(entityPM, entityPOCO);
        }
        private static void BuildSearchFields(ReferenceCustomObjectPM entityPM, ReferenceCustomObject entityPOCO)
        {
            string mySearchFields = "";

            IAmitalCloudContext MyContext = AmitalCloudContext.GetContext(entityPM.Tenant);
            ObjectTable objectTable = MyContext.ObjectTables.Where(d => d.Id == entityPM.ObjectTableId).FirstOrDefault();

            #region Custom Fields
            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName(objectTable.Name, entityPM.Tenant).Where(o => o.DataTypeCode != "Decimal").ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(entityPM.Tenant);
            foreach (ObjectField field in customFields)
            {
                object value = customFieldResolver.GetFieldValue(entityPM, field, entityPM.Tenant);
                if (value != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, value.ToString());
                }
            }
            #endregion

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }

        private static void MapCustomFields(ReferenceCustomObjectPM entityPM, ReferenceCustomObject entityPOCO)
        {
            for (int i = 1; i <= numberOfCustomFields; i++)
            {
                MapCustomFieldValue(("Field" + i.ToString()), entityPM, entityPOCO);
            }
        }

        private static void MapCustomFieldValue(string fieldName, ReferenceCustomObjectPM entityPM, ReferenceCustomObject entityPOCO)
        {
            var customFieldValue = GetFieldValue(entityPM, fieldName);
            var fieldProperty = entityPOCO.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (fieldProperty == null) return;
            fieldProperty.SetValue(entityPOCO, customFieldValue, null);
        }

        private static object GetFieldValue(ReferenceCustomObjectPM entityPM, string fieldName)
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
