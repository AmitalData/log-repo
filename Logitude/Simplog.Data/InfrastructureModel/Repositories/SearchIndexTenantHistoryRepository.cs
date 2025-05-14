using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class SearchIndexTenantHistoryRepository : IRepository<SearchIndexTenantHistory>
    {
        public readonly IWebFreightContext context;
        
        public SearchIndexTenantHistoryRepository(int tenant)
        {
            context = WebFreightContext.GetContext(tenant);
        }

        public SearchIndexTenantHistoryRepository(IWebFreightContext context)
        {
            this.context = context;
        }
        public void Add(SearchIndexTenantHistory entity)
        {
            throw new NotImplementedException();
        }

        public List<SearchIndexTenantHistory> All() => context.SearchIndexTenantHistories.ToList();

        public List<SearchIndexTenantHistory> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SearchIndexTenantHistory GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void Remove(SearchIndexTenantHistory entity)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges() => context.SaveChanges();        
        public void Update(SearchIndexTenantHistory entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            context.SetAsModified(entity);
            if (context.GetActiveDbContext(). Entry(entity).State == EntityState.Detached)
                context.SearchIndexTenantHistories.Attach(entity);
            SubmitChanges();
        }
    }
}
