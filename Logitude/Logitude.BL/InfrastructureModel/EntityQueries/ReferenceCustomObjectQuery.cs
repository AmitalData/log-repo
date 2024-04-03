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
    public class ReferenceCustomObjectQuery
    {
        private ReferenceCustomObjectRepository repository;
        private List<ObjectTable> objectTables;
        private int tenant;
        private int numberOfCustomFields = 50;
        public ReferenceCustomObjectQuery(int tenant)
        {
            repository = new ReferenceCustomObjectRepository(tenant);
            objectTables = new List<ObjectTable>();
            this.tenant = tenant;
        }
        public ReferenceCustomObjectQuery(ReferenceCustomObjectRepository customObjectrepository)
        {
            repository = customObjectrepository;
            objectTables = new List<ObjectTable>();
        }
        public ReferenceCustomObjectPM GetSinglePM(string id, int tenant)
        {
            ReferenceCustomObject referenceCustomObject = repository.GetSingleReferenceCustomObject(id, tenant);
            ReferenceCustomObjectPM result = MapReferenceCustomObjectToReferenceCustomObjectPM(referenceCustomObject, new ReferenceCustomObjectPM());
            return result;
        }

        public List<ReferenceCustomObjectPM> GetByObjectTableId(int tenant, string objectTableId)
        {
            List<ReferenceCustomObject> referenceCustomObjects = repository.GetByObjectTableId(tenant, objectTableId);
            List<ReferenceCustomObjectPM> results = new List<ReferenceCustomObjectPM>();
            foreach (ReferenceCustomObject referenceCustomObject in referenceCustomObjects)
            {
                results.Add(MapReferenceCustomObjectToReferenceCustomObjectPM(referenceCustomObject, new ReferenceCustomObjectPM()));
            }
            return results;
        }

        public IQueryable<ReferenceCustomObjectList> GetIQueryableEntityList(IQueryable<ReferenceCustomObject> iQueryable)
        {
            IQueryable<ReferenceCustomObjectList>
                result = from a in iQueryable.Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                         select new ReferenceCustomObjectList()
                         {
                             Id = a.Id,
                             Tenant = a.Tenant,
                             CreateDate = a.CreateDate,
                             UpdateDate = a.UpdateDate,
                             InActive = a.InActive,
                             SearchFields = a.SearchFields,
                             ObjectTableId= a.ObjectTableId,
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

        private ReferenceCustomObjectPM MapReferenceCustomObjectToReferenceCustomObjectPM(ReferenceCustomObject referenceCustomObject, ReferenceCustomObjectPM referenceCustomObjectPM)
        {
            referenceCustomObjectPM.Id = referenceCustomObject.Id;
            referenceCustomObjectPM.Tenant = referenceCustomObject.Tenant;
            referenceCustomObjectPM.CreateDate = referenceCustomObject.CreateDate;
            referenceCustomObjectPM.UpdateDate = referenceCustomObject.UpdateDate;
            referenceCustomObjectPM.ObjectTableId = referenceCustomObject.ObjectTableId;
            referenceCustomObjectPM.InActive = referenceCustomObject.InActive;
            referenceCustomObjectPM.SearchFields = referenceCustomObject.SearchFields;
            referenceCustomObjectPM.CreatedBy = referenceCustomObject.CreatedBy;
            referenceCustomObjectPM.UpdatedBy = referenceCustomObject.UpdatedBy;

            for (int i = 1; i <= numberOfCustomFields; i++)
            {
                MapCustomFieldValue(("Field" + i.ToString()), referenceCustomObject, referenceCustomObjectPM);
            }
            return referenceCustomObjectPM;
        }

        private void MapCustomFieldValue(string fieldName, ReferenceCustomObject referenceCustomObject, ReferenceCustomObjectPM referenceCustomObjectPM)
        {
            object customFieldValue = GetFieldValueByName(referenceCustomObject, fieldName);
            if (customFieldValue == null || string.IsNullOrEmpty(customFieldValue.ToString())) return;

            var objectTable = GetObjectTableById(referenceCustomObject.ObjectTableId, referenceCustomObject.Tenant, objectTables);
            if (objectTable == null) return;
            CustomFieldClass customFieldClass = new CustomFieldClass(fieldName, objectTable.Name, customFieldValue.ToString());
            SetFieldValue(referenceCustomObjectPM, fieldName, customFieldClass);
        }

        private ObjectTable GetObjectTableById(string objectTableId, int tenant, List<ObjectTable> objectTables)
        {
            var objectTable = objectTables.Where(d => d.Id == objectTableId)?.FirstOrDefault();
            if (objectTable != null) return objectTable;
            objectTable = ObjectTableRepository.GetSingleObjectTableById(objectTableId, tenant);
            objectTables.Add(objectTable);
            return objectTable;
        }

        private object GetFieldValueByName(ReferenceCustomObject referenceCustomObject, string fieldName)
        {
            PropertyInfo property = referenceCustomObject.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null) return null;
            return property.GetValue(referenceCustomObject, null);
        }

        private void SetFieldValue(ReferenceCustomObjectPM referenceCustomObjectPM, string fieldName, object fieldValue)
        {
            PropertyInfo propertyInfo = referenceCustomObjectPM.GetType().GetProperty(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null) return;
            propertyInfo.SetValue(referenceCustomObjectPM, fieldValue, null);
        }
    }
}
