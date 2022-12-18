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
    public class DataCustomObjectQuery
    {
        private DataCustomObjectRepository repository;
        private List<ObjectTable> objectTables;
        private int tenant;
        private int numberOfCustomFields = 50;
        public DataCustomObjectQuery(int tenant)
        {
            repository = new DataCustomObjectRepository(tenant);
            objectTables = new List<ObjectTable>();
            this.tenant = tenant;
        }
        public DataCustomObjectQuery(DataCustomObjectRepository customObjectrepository)
        {
            repository = customObjectrepository;
            objectTables = new List<ObjectTable>();
        }
        public DataCustomObjectPM GetSinglePM(string id, int tenant)
        {
            DataCustomObject dataCustomObject = repository.GetSingleDataCustomObject(id, tenant);
            DataCustomObjectPM result = MapDataCustomObjectToDataCustomObjectPM(dataCustomObject, new DataCustomObjectPM());
            return result;
        }

        public List<DataCustomObjectPM> GetByObjectTableId(string id, int tenant, string objectTableId)
        {
            List<DataCustomObject> dataCustomObjects = repository.GetByObjectTableId(id, tenant, objectTableId);
            List<DataCustomObjectPM> results = new List<DataCustomObjectPM>();
            foreach (DataCustomObject dataCustomObject in dataCustomObjects)
            {
                results.Add(MapDataCustomObjectToDataCustomObjectPM(dataCustomObject, new DataCustomObjectPM()));
            }
            return results;
        }

        public IQueryable<DataCustomObjectList> GetIQueryableEntityList(IQueryable<DataCustomObject> iQueryable)
        {
            IQueryable<DataCustomObjectList>
                result = from a in iQueryable
                         select new DataCustomObjectList()
                         {
                             Tenant = a.Tenant,
                             IsCancelled = a.IsCancelled,
                             StatusId = a.StatusId,
                             CreatedBy = a.CreatedBy,
                             CreateDate = a.CreateDate,
                             UpdatedBy = a.UpdatedBy,
                             UpdateDate = a.UpdateDate
                         };

            return result;
        }

        private DataCustomObjectPM MapDataCustomObjectToDataCustomObjectPM(DataCustomObject dataCustomObject, DataCustomObjectPM dataCustomObjectPM)
        {
            dataCustomObjectPM.Id = dataCustomObject.Id;
            dataCustomObjectPM.Tenant = dataCustomObject.Tenant;
            dataCustomObjectPM.CreateDate = dataCustomObject.CreateDate;
            dataCustomObjectPM.CreatedBy = dataCustomObject.CreatedBy;
            dataCustomObjectPM.UpdatedBy = dataCustomObject.UpdatedBy;
            dataCustomObjectPM.UpdateDate = dataCustomObject.UpdateDate;
            dataCustomObjectPM.ObjectTableId = dataCustomObject.ObjectTableId;
            dataCustomObjectPM.IsCancelled = dataCustomObject.IsCancelled;
            dataCustomObjectPM.StatusId = dataCustomObject.StatusId;
            for (int i = 1; i <= numberOfCustomFields; i++)
            {
                MapCustomFieldValue(("Field" + i.ToString()), dataCustomObject, dataCustomObjectPM);
            }
            return dataCustomObjectPM;
        }

        private void MapCustomFieldValue(string fieldName, DataCustomObject dataCustomObject, DataCustomObjectPM dataCustomObjectPM)
        {
            object customFieldValue = GetFieldValueByName(dataCustomObject, fieldName);
            if (customFieldValue == null || string.IsNullOrEmpty(customFieldValue.ToString())) return;

            var objectTable = GetObjectTableById(dataCustomObject.ObjectTableId, dataCustomObject.Tenant, objectTables);
            if (objectTable == null) return;
            CustomFieldClass customFieldClass = new CustomFieldClass(fieldName, objectTable.Name, customFieldValue.ToString());
            SetFieldValue(dataCustomObjectPM, fieldName, customFieldClass);
        }

        private ObjectTable GetObjectTableById(string objectTableId, int tenant, List<ObjectTable> objectTables)
        {
            var objectTable = objectTables.Where(d => d.Id == objectTableId).FirstOrDefault();
            if (objectTable != null) return objectTable;
            objectTable = ObjectTableRepository.GetSingleObjectTableById(objectTableId, tenant);
            objectTables.Add(objectTable);
            return objectTable;
        }

        private object GetFieldValueByName(DataCustomObject dataCustomObject, string fieldName)
        {
            PropertyInfo property = dataCustomObject.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null) return null;
            return property.GetValue(dataCustomObject, null);
        }

        private void SetFieldValue(DataCustomObjectPM dataCustomObjectPM, string fieldName, object fieldValue)
        {
            PropertyInfo propertyInfo = dataCustomObjectPM.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null) return;
            propertyInfo.SetValue(dataCustomObjectPM, fieldValue, null);
        }
    }
}
