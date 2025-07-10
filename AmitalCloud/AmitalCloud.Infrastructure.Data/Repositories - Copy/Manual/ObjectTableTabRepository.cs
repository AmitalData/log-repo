using System.Collections.Generic;
using System.Linq;
using AmitalCloud.Infrastructure.Data.Interfaces;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Context;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class ObjectTableTabRepository:IRepository<ObjectTableTab, string>
    {
        IAmitalCloudContext amitalCloudContext;
        public ObjectTableTabRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;

        }
        public ObjectTableTabRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
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
            amitalCloudContext = new AmitalCloudContext();
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

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectTableTab> GetMulti(IEntityKeyFields<ObjectTableTab,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ObjectTableTab GetSingle(IEntityKeyFields<ObjectTableTab,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}