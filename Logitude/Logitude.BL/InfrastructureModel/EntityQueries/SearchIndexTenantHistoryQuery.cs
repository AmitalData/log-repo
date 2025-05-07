using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Collections.Generic;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class SearchIndexTenantHistoryQuery
    {
        SearchIndexTenantHistoryRepository repository;

        public SearchIndexTenantHistoryQuery(IWebFreightContext webFreightContext)
        {
            repository = new SearchIndexTenantHistoryRepository(webFreightContext);
        }

        public SearchIndexTenantHistoryQuery(int tenant)
        {
            repository = new SearchIndexTenantHistoryRepository(tenant);
        }

        public List<SearchIndexTenantHistory> GetAll() => repository.All();

        public void Update(SearchIndexTenantHistory entity) => repository.Update(entity);
    }
}
