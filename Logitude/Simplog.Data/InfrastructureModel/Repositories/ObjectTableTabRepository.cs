using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ObjectTableTabRepository:IRepository<ObjectTableTab>
    {
        IWebFreightContext webFreightContext;
        public ObjectTableTabRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public ObjectTableTabRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public ObjectTableTab GetSingleObjectTableTab(string id)
        {
            return (from a in context.ObjectTableTabs
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<ObjectTableTab> GetObjectTableTabsByTenant(int tenant)
        {
            IQueryable<ObjectTableTab> tabs = from a in context.ObjectTableTabs
                                              where a.Tenant == tenant
                                              select a;
            return tabs;
        }

      
     

        public ObjectTableTabRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public void Add(ObjectTableTab entity)
        {
            context.ObjectTableTabs.Add(entity);
        }

        public void Remove(ObjectTableTab entity)
        {
            context.ObjectTableTabs.Attach(entity);
            context.ObjectTableTabs.Remove(entity);
        }

        public void Update(ObjectTableTab entity)
        {
            try
            {
                context.ObjectTableTabs.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ObjectTableTab> All()
        {
            return context.ObjectTableTabs.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectTableTab> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ObjectTableTab GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}