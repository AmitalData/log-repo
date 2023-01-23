using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.CustomFields
{
    public class EntityCustomFieldService
    {
        private string objectTableId = string.Empty;
        private int tenant;
        private List<CustomFieldsMainObject> customFieldsMainObjects = null;
        private List<ObjectField> customObjectFields = null;
        private CustomFieldsMainObjectRepository customFieldsMainObjectRepository;
        private string objectTableName = string.Empty;
        private string type = string.Empty;
        private List<object> entities = null;


        public EntityCustomFieldService(EntityCustomFieldServiceArgs entityCustomFieldServiceArgs)
        {
            objectTableId = new ObjectTableRepository(entityCustomFieldServiceArgs.Tenant).GetObjectTableIdByName(entityCustomFieldServiceArgs.ObjectTableName, entityCustomFieldServiceArgs.Tenant);
            tenant = entityCustomFieldServiceArgs.Tenant;
            objectTableName = entityCustomFieldServiceArgs.ObjectTableName;
            entities = entityCustomFieldServiceArgs.Entities;
            type = entityCustomFieldServiceArgs.Type;
            customFieldsMainObjectRepository = new CustomFieldsMainObjectRepository(tenant);
            customObjectFields = GetCustomObjectFields();
            customFieldsMainObjects = GetCustomFieldsMainObjects(entityCustomFieldServiceArgs.EntityId);
        }


        public void Update()
        {
            if (customObjectFields.Count() == 0 || entities == null || entities.Count() == 0) return;
            object entity = entities.FirstOrDefault();
            string entityId = GetPropertyValue(entity, "Id").ToString();
            CustomFieldsMainObject customFieldsMainObject = GetCustomFieldsMainObject(entityId);
            if (customFieldsMainObject == null) return;
            foreach (ObjectField customObjectField in customObjectFields)
            {
                SetPropertyValue(customFieldsMainObject, customObjectField.FieldName, (GetPropertyValue(entity, customObjectField.FieldName) as CustomFieldClass)?.Value);
            }
            if (IsEntityExist(entityId))
            {
                customFieldsMainObjectRepository.Update(customFieldsMainObject);
            }
            customFieldsMainObjectRepository.SubmitChanges();
        }

        public void Set()
        {
            if (customObjectFields.Count() == 0 || entities == null || entities.Count() == 0) return;
            foreach (object entity in entities)
            {
                SetCustomFieldValue(entity);
            }
        }

        private void SetCustomFieldValue(object entity)
        {
            CustomFieldsMainObject customFieldsMainObject = GetCustomFieldsMainObject((GetPropertyValue(entity, "Id").ToString()));
            if (entity == null || customFieldsMainObject == null) return;
            foreach (ObjectField customObjectField in customObjectFields)
            {
                SetPropertyValue(entity, customObjectField.FieldName, (type == "List" ? GetPropertyValue(customFieldsMainObject, customObjectField.FieldName) : GetCustomFieldValue(customFieldsMainObject, customObjectField)));
            }
        }


        private object GetCustomFieldValue(object entity, ObjectField objectField)
        {
            var propertyValue = GetPropertyValue(entity, objectField.FieldName);
            return new CustomFieldClass(objectField.FieldName, objectTableName, propertyValue != null ? propertyValue.ToString() : "");

        }


        private bool IsEntityExist(string entityId)
        {
            return customFieldsMainObjects.Where(d => d.EntityId == entityId).Any();
        }

        private CustomFieldsMainObject GetCustomFieldsMainObject(string entityId)
        {
            if (IsEntityExist(entityId)) return customFieldsMainObjects.Where(d => d.EntityId == entityId).FirstOrDefault();
          
            CustomFieldsMainObject customFieldsMainObject =  new CustomFieldsMainObject()
            {
                Id = IdCounter.GetNumber("ChildEntitiesCustomField", tenant).ToString(),
                Tenant = tenant,
                ObjectTableId = objectTableId,
                EntityId = entityId,
            };
            customFieldsMainObjectRepository.Add(customFieldsMainObject);
            return customFieldsMainObject;
        }


        private List<ObjectField> GetCustomObjectFields()
        {
            return ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName(objectTableName, tenant).ToList();
        }


        private List<CustomFieldsMainObject> GetCustomFieldsMainObjects(string entityId)
        {
            if (!string.IsNullOrEmpty(entityId))
            {
                return customFieldsMainObjectRepository.GetCustomFieldsMainObjects(tenant).Where(d => d.EntityId == entityId && d.ObjectTableId == objectTableId).ToList();
            }
            return customFieldsMainObjectRepository.GetCustomFieldsMainObjects(tenant).Where(d => d.ObjectTableId == objectTableId).ToList();
        }

        private void SetPropertyValue(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                prop.SetValue(obj, value, null);
            }
        }

        private object GetPropertyValue(object obj, string property)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                return prop.GetValue(obj);
            }
            return "";
        }

    }


    public class EntityCustomFieldServiceArgs
    {
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableName { get; set; }
        public string Type { get; set; }
        public List<object> Entities { get; set; }


    }

}

