using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class SearchIndexRepository : IRepository<SearchIndex>
    {
        public readonly IWebFreightContext webFreightContext;

        public SearchIndexRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public SearchIndexRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public void Add(SearchIndex entity)
        {
            throw new NotImplementedException();
        }

        public List<SearchIndex> All()
        {
            throw new NotImplementedException();
        }

        public List<SearchIndex> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SearchIndex GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void Remove(SearchIndex entity)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(SearchIndex entity)
        {
            throw new NotImplementedException();
        }
    }
}
