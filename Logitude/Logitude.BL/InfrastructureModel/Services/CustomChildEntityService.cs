using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class CustomChildEntityService
    {
        private CustomChildEntityArgs customChildEntityArgs;
        private string parentObjectTableId = string.Empty;
        public CustomChildEntityService(CustomChildEntityArgs customChildEntityArgs)
        {
            this.customChildEntityArgs = customChildEntityArgs;
            parentObjectTableId = new ObjectTableQuery(customChildEntityArgs.Tenant).GetObjectTableIdByName(customChildEntityArgs.ParentObjectTableName);

        }
        public void Update()
        {
            if (string.IsNullOrEmpty(parentObjectTableId)) return;
            List<CustomChildEntity> customChildEntities = GetCustomChildEntities(this.customChildEntityArgs.ParentEntity);
            if (customChildEntities.Count() == 0) return;
            Parallel.ForEach(customChildEntities, (customChildEntity) =>
            {
                UpdateCustomChildEntity(customChildEntity);
            });

        }


        public void Set()
        {
            if (string.IsNullOrEmpty(parentObjectTableId)) return;
            var property = customChildEntityArgs.ParentEntity.GetType().GetProperty("CustomChildEntities", BindingFlags.Public | BindingFlags.Instance);
            if (property == null) return;
            property.SetValue(customChildEntityArgs.ParentEntity, BuildCustomChildEntities(), null);
        }



        private List<CustomChildEntity> BuildCustomChildEntities()
        {
            List<CustomChildEntity> customChildEntities = new List<CustomChildEntity>();
            var childObjectTables = new ObjectTableQuery(customChildEntityArgs.Tenant).GetObjectPMsByTenant(customChildEntityArgs.Tenant).Where(d=>d.IsCustom && d.ParentObjectTableId == parentObjectTableId).ToList();
            List<CustomChildObjectPM> customChildObjects = new CustomChildObjectQuery(customChildEntityArgs.Tenant).GetByParentEntityId(customChildEntityArgs.ParentEntityId, customChildEntityArgs.Tenant);

            foreach (ObjectTablePM objectTablePM in childObjectTables)
            {
                customChildEntities.Add(new CustomChildEntity()
                {
                    Name = objectTablePM.Name,
                    Values = customChildObjects.Where(d=>d.ObjectTableId == objectTablePM.Id).ToList(),
                });
            }


            return customChildEntities;
        }

        private List<CustomChildObjectPM> GetCustomChildObjects(string objectTableId)
        {
            return new List<CustomChildObjectPM>();
        }

       private void UpdateCustomChildEntity(CustomChildEntity customChildEntity)
        {
            if (customChildEntity.Values == null || customChildEntity.Values.Count() == 0) return;

            CustomChildObjectService customChildObjectService = new CustomChildObjectService(WebFreightContext.GetContext(customChildEntityArgs.Tenant), customChildEntityArgs.Tenant);
            customChildObjectService.Updates(customChildEntity.Values);

        }

        private List<CustomChildEntity> GetCustomChildEntities(object entity)
        {
            List<CustomChildEntity> customChildEntities = new List<CustomChildEntity>();
            PropertyInfo propertyInfo = entity.GetType().GetProperty("CustomChildEntities");
            if (propertyInfo == null) return customChildEntities;
            object propertyValue = propertyInfo.GetValue(entity, null);
           
            if (propertyValue != null && propertyValue.GetType() == typeof(List<CustomChildEntity>))
            {
                customChildEntities = (propertyValue as List<CustomChildEntity>);
            }
            return customChildEntities;
        }

    }


    public class CustomChildEntityArgs
    {
        public int Tenant { get; set; }

        public string ParentEntityId { get; set; }

        public string ParentObjectTableName { get; set; }

        public object ParentEntity { get; set; }

    }

}
