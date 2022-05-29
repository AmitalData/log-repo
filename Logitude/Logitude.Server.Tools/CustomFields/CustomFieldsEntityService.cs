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
    public class CustomFieldsEntityService
    {

        private string objectTableId = string.Empty;
        private string entityId = string.Empty;
        private string childObjectTableId = string.Empty;
        private int tenant; 
        private List<ChildEntitiesCustomField> childEntitiesCustomFields = null;
        private CustomChildEntityServiceArgs customChildEntityServiceArgs;
        private CustomFieldResolver customFieldResolver = null;
        private List<ObjectField> customObjectFields = null;
        private List<object> childEntities = null;

        
        private ChildEntitiesCustomFieldRepository childEntitiesCustomFieldRepository;
        public CustomFieldsEntityService()
        {
        }



        private void Initialize(CustomChildEntityServiceArgs customChildEntityServiceArgs)
        {
            this.customChildEntityServiceArgs = customChildEntityServiceArgs;
            this.childEntities = customChildEntityServiceArgs.ChildEntities;
            this.objectTableId = ObjectTableRepository.GetObjectTableByName(customChildEntityServiceArgs.ObjectTableName);
            this.childObjectTableId = ObjectTableRepository.GetObjectTableByName(customChildEntityServiceArgs.ChildObjectTableName);
            this.entityId = customChildEntityServiceArgs.EntityId;
            this.tenant = customChildEntityServiceArgs.Tenant;
            this.customFieldResolver = new CustomFieldResolver();
            this.childEntitiesCustomFieldRepository = new ChildEntitiesCustomFieldRepository(customChildEntityServiceArgs.Tenant);
            this.customObjectFields = this.GetCustomObjectFields();
            this.childEntitiesCustomFields = this.GetChildEntitiesCustomFields();
        }



        public void SetCustomFieldsValues(CustomChildEntityServiceArgs customChildEntityServiceArgs)
        {
            this.Initialize(customChildEntityServiceArgs);

            if (childEntities == null || childEntities.Count() == 0 || customObjectFields.Count() == 0) return;
           
            foreach (ObjectField customObjectField in customObjectFields)
            {
                foreach (object childEntity in childEntities)
                {
                    if (childEntity != null)
                    {
                        ChildEntitiesCustomField childEntitiesCustomField = GetChildEntitiesCustomField(childEntity);
                        if (childEntitiesCustomField != null)
                        {
                            SetPropertyValue(childEntity, customObjectField.FieldName, GetCustomFieldValue(childEntitiesCustomField, customObjectField));
                        }
                    }
                }
            }
        }

        public void UpdateCustomFields(CustomChildEntityServiceArgs customChildEntityServiceArgs)
        {
            this.Initialize(customChildEntityServiceArgs);

            if (childEntities == null || childEntities.Count() == 0 || customObjectFields.Count() == 0) return;
            foreach (object childEntity in childEntities)
            {
                ChildEntitiesCustomField childEntitiesCustomField = GetChildEntitiesCustomField(childEntity);
                foreach (ObjectField customObjectField in customObjectFields)
                {
                    CustomFieldClass customFieldClass = GetPropertyValue(childEntity, customObjectField.FieldName) as CustomFieldClass;
                    SetPropertyValue(childEntitiesCustomField, customObjectField.FieldName, customFieldClass.Value);
                }

                if (IsChildEntitiesCustomFieldIsNew(childEntitiesCustomField))
                {
                    childEntitiesCustomFieldRepository.Add(childEntitiesCustomField);
                }
                else childEntitiesCustomFieldRepository.Update(childEntitiesCustomField);
            }

            childEntitiesCustomFieldRepository.SubmitChanges();
        }


        private bool IsChildEntitiesCustomFieldIsNew(ChildEntitiesCustomField childEntitiesCustomField)
        {
            return !(this.childEntitiesCustomFields.Where(d => d.ChildEntityId == childEntitiesCustomField.ChildEntityId).Any());
        }

        private ChildEntitiesCustomField GetChildEntitiesCustomField(object childEntity)
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

        private object GetCustomFieldValue(object entity, ObjectField objectField)
        {
            var propertyValue = GetPropertyValue(entity, objectField.FieldName);
            return new CustomFieldClass(objectField.FieldName, customChildEntityServiceArgs.ChildObjectTableName, propertyValue != null ? propertyValue.ToString():"") ;

        }

        private List<ObjectField> GetCustomObjectFields()
        {
            return ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName(customChildEntityServiceArgs.ChildObjectTableName, tenant).ToList();
        }

        private List<ChildEntitiesCustomField> GetChildEntitiesCustomFields()
        {

          return  childEntitiesCustomFieldRepository.GetChildEntitiesCustomFields(tenant).Where(d=>d.EntityId == entityId && d.ObjectTableId == d.ObjectTableId && d.ChildObjectTableId == childObjectTableId).ToList();

        
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





    public class CustomChildEntityServiceArgs
    {

        public int Tenant { get; set; }
        public string EntityId { get; set; }

        public string ChildEntityId { get; set; }

        public string ObjectTableName { get; set; }

        public string ChildObjectTableName { get; set; }

        public  List<object> ChildEntities { get; set; }

    }

}

