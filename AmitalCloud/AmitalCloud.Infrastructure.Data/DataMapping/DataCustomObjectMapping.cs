using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Helpers;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Data.Helpers;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class DataCustomObjectMapping
    {
        private static readonly int numberOfCustomFields = 50;
        public static void MapEntity(DataCustomObjectPM entityPM, DataCustomObject entityPOCO, bool isNewState, List<FieldChange> fieldChanges)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            FieldChange.Add(entityPOCO.CreateDate, entityPM.CreateDate, nameof(entityPM.CreateDate), fieldChanges);
            entityPOCO.CreateDate = entityPM.CreateDate;

            FieldChange.Add(entityPOCO.CreatedBy, entityPM.CreatedBy, nameof(entityPM.CreatedBy), fieldChanges);
            entityPOCO.CreatedBy = entityPM.CreatedBy;

            FieldChange.Add(entityPOCO.UpdateDate, entityPM.UpdateDate, nameof(entityPM.UpdateDate), fieldChanges);
            entityPOCO.UpdateDate = entityPM.UpdateDate;

            FieldChange.Add(entityPOCO.UpdatedBy, entityPM.UpdatedBy, nameof(entityPM.UpdatedBy), fieldChanges);
            entityPOCO.UpdatedBy = entityPM.UpdatedBy;

            FieldChange.Add(entityPOCO.ObjectTableId, entityPM.ObjectTableId, nameof(entityPM.ObjectTableId), fieldChanges);
            entityPOCO.ObjectTableId = entityPM.ObjectTableId;

            FieldChange.Add(entityPOCO.IsCancelled, entityPM.IsCancelled, nameof(entityPM.IsCancelled), fieldChanges);
            entityPOCO.IsCancelled = entityPM.IsCancelled;

            FieldChange.Add(entityPOCO.StatusId, entityPM.StatusId, nameof(entityPM.StatusId), fieldChanges);
            entityPOCO.StatusId = entityPM.StatusId;

            MapCustomFields(entityPM, entityPOCO, fieldChanges);
            BuildSearchFields(entityPM, entityPOCO);
        }
        private static void BuildSearchFields(DataCustomObjectPM entityPM, DataCustomObject entityPOCO)
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

        private static void MapCustomFields(DataCustomObjectPM entityPM, DataCustomObject entityPOCO, List<FieldChange> fieldChanges)
        {
            for (int i = 1; i <= numberOfCustomFields; i++)
            {
                MapCustomFieldValue(("Field" + i.ToString()), entityPM, entityPOCO, fieldChanges);
            }
        }

        private static void MapCustomFieldValue(string fieldName, DataCustomObjectPM entityPM, DataCustomObject entityPOCO, List<FieldChange> fieldChanges)
        {
            var customFieldValue = GetFieldValue(entityPM, fieldName);
            var oldCustomFieldValue = GetFieldValue(entityPOCO, fieldName);

            var fieldProperty = entityPOCO.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (fieldProperty == null) return;

            FieldChange.Add(oldCustomFieldValue, customFieldValue, fieldName, fieldChanges);
            fieldProperty.SetValue(entityPOCO, customFieldValue, null);
        }

        private static object GetFieldValue(DataCustomObjectPM entityPM, string fieldName)
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
        private static object GetFieldValue(DataCustomObject entityPOCO, string fieldName)
        {
            var fieldProperty = entityPOCO.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (fieldProperty == null) return null;

            var customFieldValue = fieldProperty.GetValue(entityPOCO, null);
            if (customFieldValue != null && customFieldValue.GetType() == typeof(CustomFieldClass))
            {
                CustomFieldClass c = customFieldValue as CustomFieldClass;
                customFieldValue = c.Value;
            }
            return customFieldValue;
        }
    }
}
