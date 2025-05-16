using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class SearchIndexRepository : IRepository<SearchIndex>
    {
        public readonly IWebFreightContext context;

        public SearchIndexRepository(int tenant)
        {
            context = WebFreightContext.GetContext(tenant);
        }

        public SearchIndexRepository(IWebFreightContext context)
        {
            this.context = context;
        }

        public void Add(SearchIndex entity)
        {
            throw new NotImplementedException();
        }

        public List<SearchIndex> All() => context.SearchIndexes.ToList();

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
            context.SaveChanges();            
        }

        public void Update(SearchIndex entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            context.SetAsModified(entity);

            if (context.GetActiveDbContext().Entry(entity).State == EntityState.Detached)
                context.SearchIndexes.Attach(entity);
            SubmitChanges();
        }
    }
}
