using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Data.Context;
using System.Data.Entity;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class SharedUserQueryRepository : IRepository<SharedUserQuery, string>
    {
        IAmitalCloudContext amitalCloudContext;

        public SharedUserQueryRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;
        }
        
        public SharedUserQueryRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
        }

        public SharedUserQuery GetSingleSharedUserQuery(string id, int tenant)
        {
            return (from a in context.SharedUserQueries.Include("User").Include("Query")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public List<SharedUserQuery> GetAllByQueryCode(string UniqueCode)
        {
            return context.SharedUserQueries.Where(d => d.QueryCode == UniqueCode).ToList();
        }

        public void Add(SharedUserQuery entity)
        {
            context.SharedUserQueries.Add(entity);
        }

        public void Remove(SharedUserQuery entity)
        {
            context.SharedUserQueries.Attach(entity);
            context.SharedUserQueries.Remove(entity);
        }

        public void Update(SharedUserQuery entity)
        {
            context.SharedUserQueries.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SharedUserQuery> All()
        {
            return context.SharedUserQueries.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<SharedUserQuery> GetMulti(IEntityKeyFields<SharedUserQuery,string> entityKeys)
        {
            throw new NotImplementedException();
        }

        public SharedUserQuery GetSingle(IEntityKeyFields<SharedUserQuery,string> entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
