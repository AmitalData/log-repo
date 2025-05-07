using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class SearchIndexQuery
    {
        SearchIndexRepository repository;

        public SearchIndexQuery(IWebFreightContext webFreightContext)
        {
            repository = new SearchIndexRepository(webFreightContext);
        }

        public SearchIndexQuery(int tenant)
        {
            repository = new SearchIndexRepository(tenant);
        }

        public List<SearchIndex> GetAll() => repository.All();

        public void Update(SearchIndex entity) => repository.Update(entity);
    }
}
