using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class QueryRepository:IRepository<Query>
    {
        IWebFreightContext webFreightContext;

        public QueryRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public QueryRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public QueryRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
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
            IQueryable<Query> queries = from a in webFreightContext.Queries.Include("ObjectTable")
                                        where a.Tenant == tenant
                                        select a;
            return queries.OrderBy(d => d.IndexOrder);
        }

        public IQueryable<Query> GetQueriesByOrigionalQueryTenant(string origionalQueryCode,int tenant)
        {
            IQueryable<Query> queries = from a in webFreightContext.Queries
                                        where a.Tenant == tenant && a.OriginalQueryCode == origionalQueryCode
                                        select a;
            return queries.OrderBy(d => d.IndexOrder);
        }
    

        public IQueryable<Query> GetQueriesByTenantSystemLevel(int tenant)
        {
            IQueryable<Query> queries = from a in webFreightContext.Queries.Include("ObjectTable")
                                        where a.Tenant == tenant&&a.SystemLevel==true&&a.UserId==null
                                        select a;
            return queries.OrderBy(d => d.IndexOrder);
        }
   
        public IQueryable<Query> GetQueriesByTenantTenantLevel(int tenant)
        {
            IQueryable<Query> queries = from a in webFreightContext.Queries.Include("ObjectTable")
                                        where a.Tenant == tenant && a.TenantLevel == true&&a.UserId==null
                                        select a;
            return queries.OrderBy(d => d.IndexOrder);
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

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Query> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Query GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}