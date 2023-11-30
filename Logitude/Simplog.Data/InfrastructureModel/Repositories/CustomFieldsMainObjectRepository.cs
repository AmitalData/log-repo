using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class CustomFieldsMainObjectRepository : IRepository<CustomFieldsMainObject>
    {
        IWebFreightContext webFreightContext;
        public static int MaxNumberOfCustomFields = 50;

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public CustomFieldsMainObjectRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public CustomFieldsMainObjectRepository()
        { }

        public CustomFieldsMainObject GetSingleCustomFieldsMainObject(string id, int tenant)
        {
            return (from a in context.CustomFieldsMainObjects
                    where a.Tenant == tenant && a.Id == id
                    select a).FirstOrDefault();
        }

        public List<CustomFieldsMainObject> GetByObjectTableId(int tenant, string objectTableId)
        {
            return (from a in context.CustomFieldsMainObjects
                    where a.Tenant == tenant && a.ObjectTableId == objectTableId
                    select a).ToList();
        }

        public CustomFieldsMainObjectRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(CustomFieldsMainObject entity)
        {
            context.CustomFieldsMainObjects.Add(entity);
        }

        public void Remove(CustomFieldsMainObject entity)
        {
            context.CustomFieldsMainObjects.Attach(entity);
            context.CustomFieldsMainObjects.Remove(entity);
        }

        public void Update(CustomFieldsMainObject entity)
        {
            context.CustomFieldsMainObjects.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomFieldsMainObject> All()
        {
            return context.CustomFieldsMainObjects.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomFieldsMainObject> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CustomFieldsMainObject GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CustomFieldsMainObject> GetCustomFieldsMainObjects(int tenant)
        {
            return context.CustomFieldsMainObjects.Where(d => d.Tenant == tenant);
        }
    }
}
