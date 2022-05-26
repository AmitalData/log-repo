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
    public class CustomFieldsEntityService<T>
    {

        string objectTableId = string.Empty;
        string entityId = string.Empty;
        string childObjectTableId = string.Empty;
        string childEntityId = string.Empty;

        private List<CustomFieldsEntity> customFieldsEntities = null;
        private CustomFieldsEntityServiceArgs customFieldsEntityServiceArgs;
        public CustomFieldsEntityService(CustomFieldsEntityServiceArgs customFieldsEntityServiceArgs)
        {
            this.customFieldsEntityServiceArgs = customFieldsEntityServiceArgs;
            this.objectTableId = ObjectTableRepository.GetObjectTableByName(customFieldsEntityServiceArgs.ObjectTableName);
            this.childObjectTableId = ObjectTableRepository.GetObjectTableByName(customFieldsEntityServiceArgs.ChildObjectTableName);
            this.entityId = customFieldsEntityServiceArgs.EntityId;
            this.childEntityId = customFieldsEntityServiceArgs.ChildEntityId;
            this.GetCustomFieldsEntitis();
        }

        private List<CustomFieldsEntity> GetCustomFieldsEntitis()
        {
            /* Poco--*/
            var customFieldsEntities = new List<CustomFieldsEntity>();
            customFieldsEntities.Add(new CustomFieldsEntity() { Tenant = 1, Id = entityId, ChildEntityId = "1-1", ObjectTableId = objectTableId, ChildObjectTableId = "1-2", Field1 = "Abed", Field2 = "Test", Field3 = "ssss" });
            customFieldsEntities.Add(new CustomFieldsEntity() { Tenant = 1, Id = entityId, ChildEntityId = "1-2", ObjectTableId = objectTableId, ChildObjectTableId = "1-3", Field1 = "Abed2", Field2 = "Test2", Field3 = "ssss2" });
            return customFieldsEntities;
        }




        public void MapCustomFields(List<T> shipmentPackages)
        {
            foreach (T shipmentPackage in shipmentPackages)
            {
                string entityId = GetPropertyValue(shipmentPackage, "Id").ToString();
                var customFieldsEntity = customFieldsEntities.Where(d => d.ChildEntityId == entityId).FirstOrDefault();
                if (customFieldsEntity != null)
                {
                    int count = 0;
                    while (count <= 40)
                    {
                        SetPropertyValue(shipmentPackage, "Field1", new CustomFieldClass("Field1", customFieldsEntityServiceArgs.ChildObjectTableName, GetPropertyValue(customFieldsEntity, "Field1").ToString()));
                        count += 1;
                    }
                }
            }
        }




        //entityPoco.Field1 = entityPM.Field1 != null ? entityPM.Field1.Value : null;






        private object GetCustomFieldValue(CustomFieldsEntity customFieldsEntity)
        {
            return new CustomFieldClass("Field1", "Shipment", "");
        }

        public void UpdateCustomFields(List<T> entities)
        {
            foreach (T t in entities)
            {
                string entityId = GetPropertyValue(t, "Id").ToString();
                var customFieldsEntity = customFieldsEntities.Where(d => d.ChildEntityId == entityId).FirstOrDefault();
                if (customFieldsEntity != null)
                {
                    SetPropertyValue(t, "Field1", null);
                    SetPropertyValue(t, "Field2", null);
                    SetPropertyValue(t, "Field3", null);
                    SetPropertyValue(t, "Field4", null);
                    SetPropertyValue(t, "Field5", null);
                }
            }
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




    public class CustomFieldsEntity
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





    public class CustomFieldsEntityServiceArgs
    {

        public int Tenant { get; set; }
        public string EntityId { get; set; }

        public string ChildEntityId { get; set; }

        public string ObjectTableName { get; set; }

        public string ChildObjectTableName { get; set; }

    }

}

