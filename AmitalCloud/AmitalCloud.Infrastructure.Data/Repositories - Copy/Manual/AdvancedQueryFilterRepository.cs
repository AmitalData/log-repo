using System.Collections.Generic;
using System.Linq;
using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;


namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class AdvancedQueryFilterRepository : IRepository<AdvancedQueryFilter,string>
    {
        IAmitalCloudContext amitalCloudContext;
        public AdvancedQueryFilterRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;
        }

        public AdvancedQueryFilterRepository()
        {
            amitalCloudContext = new AmitalCloudContext();
        }
        public AdvancedQueryFilterRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
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

        public IQueryable<AdvancedQueryFilter> GetAdvancedQueryFiltersByTenantAndQuery(int tenant, string queryCode)
        {
            IQueryable<AdvancedQueryFilter> advancedFilters = from a in context.AdvancedQueryFilters
                                                              where (a.Tenant == tenant || a.Tenant == 0) && a.QueryCode == queryCode
                                                              select a;
            return advancedFilters;

        }

        public void RemoveFilter(ObjectField objectField, int tenant, string queryCode)
        {
            AdvancedQueryFilter advancedFilter = (from a in context.AdvancedQueryFilters
                                                  where a.Tenant == tenant && a.ObjectField.FieldCode == objectField.FieldCode && a.QueryCode == queryCode
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

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AdvancedQueryFilter> GetMulti(IEntityKeyFields<AdvancedQueryFilter,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AdvancedQueryFilter GetSingle(IEntityKeyFields<AdvancedQueryFilter,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<AdvancedQueryFilter> GetAdvancedQueryFiltersByTenantAndUserAndQuery(int tenant, string userId, string queryCode)
        {
            List<AdvancedQueryFilter> advancedFilters = (from a in context.AdvancedQueryFilters
                                                         where (a.Tenant == tenant && a.UserId == userId && a.QueryCode == queryCode) // || a.Tenant == 0
                                                         select a).ToList();

            return advancedFilters;
        }

        public List<AdvancedQueryFilter> GetAdvancedQueryFiltersByTenantAndAndQuery(int tenant, string queryCode)
        {
            List<AdvancedQueryFilter> advancedFilters = (from a in context.AdvancedQueryFilters
                                                         where (a.Tenant == tenant && a.QueryCode == queryCode) // || a.Tenant == 0
                                                         select a).ToList();

            return advancedFilters;
        }
    }
}
