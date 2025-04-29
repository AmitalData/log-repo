using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class SearchIndexTenantHistoryRepository : IRepository<SearchIndexTenantHistory>
    {
        public readonly IWebFreightContext webFreightContext;
        
        public SearchIndexTenantHistoryRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public SearchIndexTenantHistoryRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public void Add(SearchIndexTenantHistory entity)
        {
            throw new NotImplementedException();
        }

        public List<SearchIndexTenantHistory> All()
        {
            throw new NotImplementedException();
        }

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

        public void SubmitChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(SearchIndexTenantHistory entity)
        {
            throw new NotImplementedException();
        }
    }
}
