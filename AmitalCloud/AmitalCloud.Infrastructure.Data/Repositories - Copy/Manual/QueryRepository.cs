using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class QueryRepository:IRepository<Query, string>
    {
        IAmitalCloudContext amitalCloudContext;

        public QueryRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;
        }

        public QueryRepository()
        {
            amitalCloudContext = new AmitalCloudContext();
        }

        public QueryRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
        }

        public Query GetSingleQuery(string Id)
        {
            return (from a in context.Queries.Include("ObjectTable")
                   where a.Id == Id
                    select a).FirstOrDefault();
        }

        public Query GetSingleQueryByUniqueCode(string UniqueCode)
        {
            return (from a in context.Queries.Include("ObjectTable")
                    where a.UniqueCode == UniqueCode
                    select a).FirstOrDefault();
        }

        public Query GetSingleQueryByCode(string code,int tenant)
        {
            return (from a in context.Queries.Include("ObjectTable")
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
      
        public IQueryable<Query> GetQueriesByTenant(int tenant)
        {
            IQueryable<Query> queries = from a in context.Queries.Include("ObjectTable")
                                        where a.Tenant == tenant
                                        select a;
            return queries.OrderBy(d => d.IndexOrder);
        }

        public IQueryable<Query> GetQueriesByOrigionalQueryTenant(string origionalQueryCode,int tenant)
        {
            IQueryable<Query> queries = from a in context.Queries
                                        where a.Tenant == tenant && a.OriginalQueryCode == origionalQueryCode
                                        select a;
            return queries.OrderBy(d => d.IndexOrder);
        }
    

        public IQueryable<Query> GetQueriesByTenantSystemLevel(int tenant)
        {
            IQueryable<Query> queries = from a in context.Queries.Include("ObjectTable")
                                        where a.Tenant == tenant&&a.SystemLevel==true&&a.UserId==null
                                        select a;
            return queries.OrderBy(d => d.IndexOrder);
        }
   
        public IQueryable<Query> GetQueriesByTenantTenantLevel(int tenant)
        {
            IQueryable<Query> queries = from a in context.Queries.Include("ObjectTable")
                                        where a.Tenant == tenant && a.TenantLevel == true&&a.UserId==null
                                        select a;
            return queries.OrderBy(d => d.IndexOrder);
        }

        public Query GetDefaultQueryByObjectTableIdAndTenant(string objectTableId, int tenant)
        {
            Query query = (from a in context.Queries
                                        where a.ObjectTableId == objectTableId && a.Tenant == tenant && a.SystemLevel && a.IsDefault
                                         select a).FirstOrDefault();
            return query;
        }

        public void Add(Query entity)
        {
            context.Queries.Add(entity);
        }

        public void Remove(Query entity)
        {
            context.Queries.Attach(entity);
            context.Queries.Remove(entity);
        }

        public void Update(Query entity)
        {
            context.Queries.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Query> All()
        {
            return context.Queries.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Query> GetMulti(IEntityKeyFields<Query,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Query GetSingle(IEntityKeyFields<Query,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}