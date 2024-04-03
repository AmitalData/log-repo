using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class CustomFieldsMainObjectQuery
    {
        private CustomFieldsMainObjectRepository repository;
        private List<ObjectTable> objectTables;
        private int tenant;
        private int numberOfCustomFields = 50;
        public CustomFieldsMainObjectQuery(int tenant)
        {
            repository = new CustomFieldsMainObjectRepository(tenant);
            objectTables = new List<ObjectTable>();
            this.tenant = tenant;
        }
        public CustomFieldsMainObjectQuery(CustomFieldsMainObjectRepository customFieldsMainObjectsRepository)
        {
            repository = customFieldsMainObjectsRepository;
            objectTables = new List<ObjectTable>();
        }
        public CustomFieldsMainObjectPM GetSinglePM(string id, int tenant)
        {
            CustomFieldsMainObject customFieldsMainObject = repository.GetSingleCustomFieldsMainObject(id, tenant);
            CustomFieldsMainObjectPM result = MapDataCustomFieldsMainObjectToCustomFieldsMainObjectPM(customFieldsMainObject, new CustomFieldsMainObjectPM());
            return result;
        }

        public List<CustomFieldsMainObjectPM> GetByObjectTableId(int tenant, string objectTableId)
        {
            List<CustomFieldsMainObject> customFieldsMainObjects = repository.GetByObjectTableId(tenant, objectTableId);
            List<CustomFieldsMainObjectPM> results = new List<CustomFieldsMainObjectPM>();
            foreach (CustomFieldsMainObject customFieldsMainObject in customFieldsMainObjects)
            {
                results.Add(MapDataCustomFieldsMainObjectToCustomFieldsMainObjectPM(customFieldsMainObject, new CustomFieldsMainObjectPM()));
            }
            return results;
        }

        public IQueryable<CustomFieldsMainObjectList> GetIQueryableEntityList(IQueryable<CustomFieldsMainObject> iQueryable)
        {
            IQueryable<CustomFieldsMainObjectList>
                result = from a in iQueryable
                         select new CustomFieldsMainObjectList()
                         {
                             Tenant = a.Tenant,
                             ObjectTableId = a.ObjectTableId,
                             EntityId = a.EntityId,
                         };

            return result;
        }

        private CustomFieldsMainObjectPM MapDataCustomFieldsMainObjectToCustomFieldsMainObjectPM(CustomFieldsMainObject customFieldsMainObject, CustomFieldsMainObjectPM customFieldsMainObjectPM)
        {
            customFieldsMainObjectPM.Id = customFieldsMainObject.Id;
            customFieldsMainObjectPM.Tenant = customFieldsMainObject.Tenant;
            customFieldsMainObjectPM.ObjectTableId = customFieldsMainObject.ObjectTableId;
            customFieldsMainObjectPM.EntityId = customFieldsMainObject.EntityId;
            for (int i = 1; i <= numberOfCustomFields; i++)
            {
                MapCustomFieldValue(("Field" + i.ToString()), customFieldsMainObject, customFieldsMainObjectPM);
            }
            return customFieldsMainObjectPM;
        }

        private void MapCustomFieldValue(string fieldName, CustomFieldsMainObject customFieldsMainObject, CustomFieldsMainObjectPM customFieldsMainObjectPM)
        {
            object customFieldValue = GetFieldValueByName(customFieldsMainObject, fieldName);
            if (customFieldValue == null || string.IsNullOrEmpty(customFieldValue.ToString())) return;

            var objectTable = GetObjectTableById(customFieldsMainObject.ObjectTableId, customFieldsMainObject.Tenant, objectTables);
            if (objectTable == null) return;
            CustomFieldClass customFieldClass = new CustomFieldClass(fieldName, objectTable.Name, customFieldValue.ToString());
            SetFieldValue(customFieldsMainObjectPM, fieldName, customFieldClass);
        }

        private ObjectTable GetObjectTableById(string objectTableId, int tenant, List<ObjectTable> objectTables)
        {
            var objectTable = objectTables.Where(d => d.Id == objectTableId).FirstOrDefault();
            if (objectTable != null) return objectTable;
            objectTable = ObjectTableRepository.GetSingleObjectTableById(objectTableId, tenant);
            objectTables.Add(objectTable);
            return objectTable;
        }

        private object GetFieldValueByName(CustomFieldsMainObject customFieldsMainObject, string fieldName)
        {
            PropertyInfo property = customFieldsMainObject.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null) return null;
            return property.GetValue(customFieldsMainObject, null);
        }

        private void SetFieldValue(CustomFieldsMainObjectPM customFieldsMainObjectPM, string fieldName, object fieldValue)
        {
            PropertyInfo propertyInfo = customFieldsMainObjectPM.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null) return;
            propertyInfo.SetValue(customFieldsMainObjectPM, fieldValue, null);
        }
    }
}
