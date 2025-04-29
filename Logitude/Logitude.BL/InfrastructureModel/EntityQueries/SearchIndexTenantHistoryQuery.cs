using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    class SearchIndexTenantHistoryQuery
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
    }
}
