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

        public List<ReferenceCustomObjectPM> GetByObjectTableId(string id, int tenant, string objectTableId)
        {
            List<ReferenceCustomObject> referenceCustomObjects = repository.GetByObjectTableId(id, tenant, objectTableId);
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
                result = from a in iQueryable
                         select new ReferenceCustomObjectList()
                         {
                             Tenant = a.Tenant,
                             CreatedBy = a.CreatedBy,
                             CreateDate = a.CreateDate,
                             UpdatedBy = a.UpdatedBy,
                             UpdateDate = a.UpdateDate,
                             InActive = a.InActive
                         };

            return result;
        }

        private ReferenceCustomObjectPM MapReferenceCustomObjectToReferenceCustomObjectPM(ReferenceCustomObject referenceCustomObject, ReferenceCustomObjectPM referenceCustomObjectPM)
        {
            referenceCustomObjectPM.Id = referenceCustomObject.Id;
            referenceCustomObjectPM.Tenant = referenceCustomObject.Tenant;
            referenceCustomObjectPM.CreateDate = referenceCustomObject.CreateDate;
            referenceCustomObjectPM.CreatedBy = referenceCustomObject.CreatedBy;
            referenceCustomObjectPM.UpdatedBy = referenceCustomObject.UpdatedBy;
            referenceCustomObjectPM.UpdateDate = referenceCustomObject.UpdateDate;
            referenceCustomObjectPM.ObjectTableId = referenceCustomObject.ObjectTableId;
            referenceCustomObjectPM.InActive = referenceCustomObject.InActive;
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
            var objectTable = objectTables.Where(d => d.Id == objectTableId).FirstOrDefault();
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
