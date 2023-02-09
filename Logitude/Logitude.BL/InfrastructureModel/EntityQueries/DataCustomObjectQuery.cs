using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
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

        public List<DataCustomObjectPM> GetByObjectTableId(int tenant, string objectTableId)
        {
            List<DataCustomObject> dataCustomObjects = repository.GetByObjectTableId(tenant, objectTableId);
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
                result = from a in iQueryable.Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                         select new DataCustomObjectList()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             IsCancelled = a.IsCancelled,
                             StatusId = a.StatusId,
                             CreateDate = a.CreateDate,
                             UpdateDate = a.UpdateDate,
                             SearchFields = a.SearchFields,
                             ObjectTableId = a.ObjectTableId,
                             Field1 = a.Field1,
                             Field2 = a.Field2,
                             Field3 = a.Field3,
                             Field4 = a.Field4,
                             Field5 = a.Field5,
                             Field6 = a.Field6,
                             Field7 = a.Field7,
                             Field8 = a.Field8,
                             Field9 = a.Field9,
                             Field10 = a.Field10,
                             Field11 = a.Field11,
                             Field12 = a.Field12,
                             Field13 = a.Field13,
                             Field14 = a.Field14,
                             Field15 = a.Field15,
                             Field16 = a.Field16,
                             Field17 = a.Field17,
                             Field18 = a.Field18,
                             Field19 = a.Field19,
                             Field20 = a.Field20,
                             Field21 = a.Field21,
                             Field22 = a.Field22,
                             Field23 = a.Field23,
                             Field24 = a.Field24,
                             Field25 = a.Field25,
                             Field26 = a.Field26,
                             Field27 = a.Field27,
                             Field28 = a.Field28,
                             Field29 = a.Field29,
                             Field30 = a.Field30,
                             Field31 = a.Field31,
                             Field32 = a.Field32,
                             Field33 = a.Field33,
                             Field34 = a.Field34,
                             Field35 = a.Field35,
                             Field36 = a.Field36,
                             Field37 = a.Field37,
                             Field38 = a.Field38,
                             Field39 = a.Field39,
                             Field40 = a.Field40,
                             Field41 = a.Field41,
                             Field42 = a.Field42,
                             Field43 = a.Field43,
                             Field44 = a.Field44,
                             Field45 = a.Field45,
                             Field46 = a.Field46,
                             Field47 = a.Field47,
                             Field48 = a.Field48,
                             Field49 = a.Field49,
                             Field50 = a.Field50,
                             CreatedBy = (a.CreatedByUser != null && a.CreatedByUser.Contact != null) ? a.CreatedByUser.Contact.EnglishName : null,
                             UpdatedBy = (a.UpdatedByUser != null && a.UpdatedByUser.Contact != null) ? a.UpdatedByUser.Contact.EnglishName : null,
                         };

            return result;
        }

        private DataCustomObjectPM MapDataCustomObjectToDataCustomObjectPM(DataCustomObject dataCustomObject, DataCustomObjectPM dataCustomObjectPM)
        {
            dataCustomObjectPM.Id = dataCustomObject.Id;
            dataCustomObjectPM.Tenant = dataCustomObject.Tenant;
            dataCustomObjectPM.CreateDate = dataCustomObject.CreateDate;
            dataCustomObjectPM.UpdateDate = dataCustomObject.UpdateDate;
            dataCustomObjectPM.ObjectTableId = dataCustomObject.ObjectTableId;
            dataCustomObjectPM.IsCancelled = dataCustomObject.IsCancelled;
            dataCustomObjectPM.StatusId = dataCustomObject.StatusId;
            dataCustomObjectPM.SearchFields = dataCustomObject.SearchFields;
            dataCustomObjectPM.CreatedBy = dataCustomObject.CreatedBy;
            dataCustomObjectPM.UpdatedBy = dataCustomObject.UpdatedBy;
            for (int i = 1; i <= numberOfCustomFields; i++)
            {
                MapCustomFieldValue(("Field" + i.ToString()), dataCustomObject, dataCustomObjectPM);
            }
            new CustomChildEntityService(new CustomChildEntityArgs() { ParentEntity = dataCustomObjectPM, ParentEntityId = dataCustomObjectPM.Id, ParentObjectTableId = dataCustomObjectPM.ObjectTableId, Tenant = tenant }).Set();
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
            var objectTable = objectTables.Where(d => d.Id == objectTableId)?.FirstOrDefault();
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
