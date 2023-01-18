using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ReferenceCustomObjectRepository : IRepository<ReferenceCustomObject>
    {
        IWebFreightContext webFreightContext;
        public static int MaxNumberOfCustomFields = 50;

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public ReferenceCustomObjectRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public ReferenceCustomObjectRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public ReferenceCustomObjectRepository()
        { }

        public ReferenceCustomObject GetSingleReferenceCustomObject(string id, int tenant)
        {
            return (from a in context.ReferenceCustomObjects.Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                    where a.Tenant == tenant && a.Id == id
                    select a).FirstOrDefault();
        }

        public List<ReferenceCustomObject> GetByObjectTableId(int tenant, string objectTableId)
        {
            return (from a in context.ReferenceCustomObjects
                    where a.Tenant == tenant && a.ObjectTableId == objectTableId
                    select a).ToList();
        }

        public void Add(ReferenceCustomObject entity)
        {
            context.ReferenceCustomObjects.Add(entity);
        }

        public void Remove(ReferenceCustomObject entity)
        {
            context.ReferenceCustomObjects.Attach(entity);
            context.ReferenceCustomObjects.Remove(entity);
        }

        public void Update(ReferenceCustomObject entity)
        {
            context.ReferenceCustomObjects.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReferenceCustomObject> All()
        {
            return context.ReferenceCustomObjects.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ReferenceCustomObject> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ReferenceCustomObject GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ReferenceCustomObject> GetReferenceCustomObjects(int tenant)
        {
            return context.ReferenceCustomObjects.Where(d => d.Tenant == tenant);
        }

        public IQueryable<ReferenceCustomObject> GetReferenceCustomObjectsByObjectTableId(int tenant, string objectTableId)
        {
            return context.ReferenceCustomObjects.Where(d => d.Tenant == tenant && d.ObjectTableId == objectTableId);
        }
    }
}
