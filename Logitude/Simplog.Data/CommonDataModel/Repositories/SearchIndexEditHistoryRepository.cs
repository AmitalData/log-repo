using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class SearchIndexEditHistoryRepository : IRepository<SearchIndexEditHistory>
    {
        readonly ICommonDataContext iContext;

        public SearchIndexEditHistoryRepository(int tenant)
        {
            iContext = CommonDataContext.GetContext(tenant);
        }

        public SearchIndexEditHistoryRepository(ICommonDataContext context)
        {
            iContext = context;
        }

        public IQueryable<SearchIndexEditHistory> GetAll() => Context.SearchIndexEditHistories.AsQueryable();

        public void Add(SearchIndexEditHistory entity)
        {
            Context.SearchIndexEditHistories.Add(entity);
        }

        public void Remove(SearchIndexEditHistory entity)
        {
            Context.SearchIndexEditHistories.Attach(entity);
            Context.SearchIndexEditHistories.Remove(entity);
        }

        public void Update(SearchIndexEditHistory entity)
        {
            Context.SearchIndexEditHistories.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<SearchIndexEditHistory> All() => Context.SearchIndexEditHistories.ToList();

        public ICommonDataContext Context => iContext;

        public List<SearchIndexEditHistory> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SearchIndexEditHistory GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SearchIndexEditHistory GetSingleSearchIndexEditHistory(int tenant, string id) =>
            Context.SearchIndexEditHistories.FirstOrDefault(x => x.Id == id && x.Tenant == tenant);

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

        public List<string> GetRecent(int tenant, string screen, string entname, string userId, int size = 50) =>
            Context.SearchIndexEditHistories
                .Where(x => x.Tenant == tenant && x.Screen == screen && x.Entname == entname && x.CreatedByUserId == userId)
                .OrderByDescending(x => x.CreateDate)
                .Select(x => x.KeyVal)
                .Distinct()
                .Take(size)
                .ToList();
    }
}
