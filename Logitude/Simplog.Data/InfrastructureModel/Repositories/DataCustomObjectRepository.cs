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
    public class DataCustomObjectRepository : IRepository<DataCustomObject>
    {
        IWebFreightContext webFreightContext;
        public static int MaxNumberOfCustomFields = 50;

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public DataCustomObjectRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public DataCustomObjectRepository()
        { }

        public DataCustomObject GetSingleDataCustomObject(string id, int tenant)
        {
            return (from a in context.DataCustomObjects.Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                    where a.Tenant == tenant && a.Id == id
                    select a).FirstOrDefault();
        }

        public List<DataCustomObject> GetByObjectTableId(int tenant, string objectTableId)
        {
            return (from a in context.DataCustomObjects.Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                    where a.Tenant == tenant && a.ObjectTableId == objectTableId
                    select a).ToList();
        }

        public DataCustomObjectRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(DataCustomObject entity)
        {
            context.DataCustomObjects.Add(entity);
        }

        public void Remove(DataCustomObject entity)
        {
            context.DataCustomObjects.Attach(entity);
            context.DataCustomObjects.Remove(entity);
        }

        public void Update(DataCustomObject entity)
        {
            context.DataCustomObjects.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DataCustomObject> All()
        {
            return context.DataCustomObjects.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<DataCustomObject> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public DataCustomObject GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DataCustomObject> GetDataCustomObjects(int tenant)
        {
            return context.DataCustomObjects.Where(d => d.Tenant == tenant);
        }
        public IQueryable<DataCustomObject> GetDataCustomObjectsByObjectTableId(int tenant, string objectTableId)
        {
            return context.DataCustomObjects.Where(d => d.Tenant == tenant && d.ObjectTableId == objectTableId);
        }
    }
}
