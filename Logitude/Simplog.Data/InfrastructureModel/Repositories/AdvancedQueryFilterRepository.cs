using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class AdvancedQueryFilterRepository:IRepository<AdvancedQueryFilter>
    {
        IWebFreightContext webFreightContext;
        public AdvancedQueryFilterRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public AdvancedQueryFilterRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public AdvancedQueryFilterRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public AdvancedQueryFilter GetSingleAdvancedQueryfilter(string id)
        {
            return (from a in context.AdvancedQueryFilters
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<AdvancedQueryFilter> GetAdvancedQueryFiltersByTenant(int tenant)
        {
            IQueryable<AdvancedQueryFilter> advancedFilters = from a in context.AdvancedQueryFilters
                                                              where a.Tenant == tenant && a.UserId == null
                                                              select a;
            return advancedFilters;

        }
      
        public IQueryable<AdvancedQueryFilter> GetAdvancedQueryFiltersByTenantAndQuery(int tenant,string queryId)
        {
            IQueryable<AdvancedQueryFilter> advancedFilters = from a in context.AdvancedQueryFilters
                                                              where (a.Tenant == tenant||a.Tenant==0)&&a.QueryId==queryId
                                                              select a;
            return advancedFilters;

        }

        public void RemoveFilter(ObjectField objectField, int tenant, string queryId)
        {
            AdvancedQueryFilter advancedFilter = (from a in context.AdvancedQueryFilters
                                                  where a.Tenant == tenant && a.ObjectField.FieldCode == objectField.FieldCode && a.QueryId == queryId
                                                  select a).FirstOrDefault();

            if (advancedFilter != null)
            {
                this.Remove(advancedFilter);
            }

            this.context.SaveChanges();
        }

        public void Add(AdvancedQueryFilter entity)
        {
            context.AdvancedQueryFilters.Add(entity);
        }

        public void Remove(AdvancedQueryFilter entity)
        {
            try
            {
                context.AdvancedQueryFilters.Attach(entity);
            }
            catch { }
            context.AdvancedQueryFilters.Remove(entity);
        }

        public void Update(AdvancedQueryFilter entity)
        {
            try
            {
                context.AdvancedQueryFilters.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<AdvancedQueryFilter> All()
        {
            return context.AdvancedQueryFilters.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AdvancedQueryFilter> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AdvancedQueryFilter GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<AdvancedQueryFilter> GetAdvancedQueryFiltersByTenantAndUserAndQuery(int tenant, string userId, string queryId)
        {
            List<AdvancedQueryFilter> advancedFilters = (from a in context.AdvancedQueryFilters
                                                        where (a.Tenant == tenant && a.UserId == userId && a.QueryId == queryId) // || a.Tenant == 0
                                                        select a).ToList();                                  
            
            return advancedFilters;
        }

        public List<AdvancedQueryFilter> GetAdvancedQueryFiltersByTenantAndAndQuery(int tenant, string queryId)
        {
            List<AdvancedQueryFilter> advancedFilters = (from a in context.AdvancedQueryFilters
                                                         where (a.Tenant == tenant  && a.QueryId == queryId) // || a.Tenant == 0
                                                         select a).ToList();

            return advancedFilters;
        }
    }
}
