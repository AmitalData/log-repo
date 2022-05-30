using Logitude.BL.Helpers;
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

namespace Logitude.Server.Tools.CustomFields
{
    public static class ChildEntitiesCustomFieldService
    {

        private static string objectTableId = string.Empty;
        private static string entityId = string.Empty;
        private static string childObjectTableId = string.Empty;
        private static int tenant; 
        private static List<ChildEntitiesCustomField> childEntitiesCustomFields = null;
        private static CustomFieldResolver customFieldResolver = null;
        private static List<ObjectField> customObjectFields = null;
        private static List<object> childEntities = null;
        private static ChildEntitiesCustomFieldRepository childEntitiesCustomFieldRepository;
        private static string childObjectTableName = string.Empty;

        private static void Initialize(ChildEntitiesCustomFieldArgs childEntitiesCustomFieldArgs)
        {
            childEntities = childEntitiesCustomFieldArgs.ChildEntities;
            objectTableId = ObjectTableRepository.GetObjectTableByName(childEntitiesCustomFieldArgs.ObjectTableName);
            childObjectTableId = ObjectTableRepository.GetObjectTableByName(childEntitiesCustomFieldArgs.ChildObjectTableName);
            entityId = childEntitiesCustomFieldArgs.EntityId;
            tenant = childEntitiesCustomFieldArgs.Tenant;
            childObjectTableName = childEntitiesCustomFieldArgs.ChildObjectTableName;

            customFieldResolver = new CustomFieldResolver();
            childEntitiesCustomFieldRepository = new ChildEntitiesCustomFieldRepository(tenant);
            customObjectFields = GetCustomObjectFields();
            childEntitiesCustomFields = GetChildEntitiesCustomFields();
        }

        public static void Set(ChildEntitiesCustomFieldArgs childEntitiesCustomFieldArgs)
        {
           Initialize(childEntitiesCustomFieldArgs);

            if (childEntities == null || childEntities.Count() == 0 || customObjectFields.Count() == 0) return;
           
            foreach (ObjectField customObjectField in customObjectFields)
            {
                SetCustomFieldValues(customObjectField);
            }
        }


        private static void SetCustomFieldValues(ObjectField customObjectField)
        {
            foreach (object childEntity in childEntities)
            {
                SetCustomFieldEntityValues(customObjectField, childEntity);
            }
        }


        private static void SetCustomFieldEntityValues(ObjectField customObjectField, object childEntity)
        {
            if (childEntity == null) return;
            ChildEntitiesCustomField childEntitiesCustomField = GetChildEntitiesCustomField(childEntity);
            if (childEntitiesCustomField == null) return;
            SetPropertyValue(childEntity, customObjectField.FieldName, GetCustomFieldValue(childEntitiesCustomField, customObjectField));
        }


        public static void Update(ChildEntitiesCustomFieldArgs childEntitiesCustomFieldArgs)
        {
            Initialize(childEntitiesCustomFieldArgs);

            if (childEntities == null || childEntities.Count() == 0 || customObjectFields.Count() == 0) return;
            foreach (object childEntity in childEntities)
            {
                UpdateCustomFieldsValue(childEntity);
            }

            childEntitiesCustomFieldRepository.SubmitChanges();
        }


    



        private static void UpdateCustomFieldsValue(object childEntity)
        {
            ChildEntitiesCustomField childEntitiesCustomField = GetChildEntitiesCustomField(childEntity);
            foreach (ObjectField customObjectField in customObjectFields)
            {
                SetPropertyValue(childEntitiesCustomField, customObjectField.FieldName, (GetPropertyValue(childEntity, customObjectField.FieldName) as CustomFieldClass).Value);
            }

            if (IsNewEntity(childEntitiesCustomField))
            {
                childEntitiesCustomFieldRepository.Add(childEntitiesCustomField);
            }
            else childEntitiesCustomFieldRepository.Update(childEntitiesCustomField);
        }

        private static bool IsNewEntity(ChildEntitiesCustomField childEntitiesCustomField)
        {
            return !(childEntitiesCustomFields.Where(d => d.ChildEntityId == childEntitiesCustomField.ChildEntityId).Any());
        }

        private static ChildEntitiesCustomField GetChildEntitiesCustomField(object childEntity)
        {
            string childEntityId = GetPropertyValue(childEntity, "Id").ToString();
            var childEntitiesCustomField = childEntitiesCustomFields.Where(d => d.ChildEntityId == childEntityId).FirstOrDefault();
            if (childEntitiesCustomField != null) return childEntitiesCustomField;
            return new ChildEntitiesCustomField()
            {
                Id = IdCounter.GetNumber("ChildEntitiesCustomField", tenant).ToString(),
                Tenant = tenant,
                ObjectTableId = objectTableId,
                ChildEntityId = childEntityId,
                EntityId = entityId,
                ChildObjectTableId = childObjectTableId,
            };



        }

        private static object GetCustomFieldValue(object entity, ObjectField objectField)
        {
            var propertyValue = GetPropertyValue(entity, objectField.FieldName);
            return new CustomFieldClass(objectField.FieldName, childObjectTableName, propertyValue != null ? propertyValue.ToString():"") ;

        }
      
        private static List<ObjectField> GetCustomObjectFields()
        {
            return ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName(childObjectTableName, tenant).ToList();
        }

        private static List<ChildEntitiesCustomField> GetChildEntitiesCustomFields()
        {

          return  childEntitiesCustomFieldRepository.GetChildEntitiesCustomFields(tenant).Where(d=>d.EntityId == entityId && d.ObjectTableId == d.ObjectTableId && d.ChildObjectTableId == childObjectTableId).ToList();

        
        }
        private static void SetPropertyValue(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                prop.SetValue(obj, value, null);
            }
        }

        private static object  GetPropertyValue(object obj, string property)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
               return prop.GetValue(obj);
            }
            return "";
        }

    }




    public class CustomChildEntity
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }

        public string ChildEntityId { get; set; }

        public string ObjectTableId { get; set; }

        public string ChildObjectTableId { get; set; }


        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }


    }





    public class ChildEntitiesCustomFieldArgs
    {

        public int Tenant { get; set; }
        public string EntityId { get; set; }

        public string ChildEntityId { get; set; }

        public string ObjectTableName { get; set; }

        public string ChildObjectTableName { get; set; }

        public  List<object> ChildEntities { get; set; }

    }

}

