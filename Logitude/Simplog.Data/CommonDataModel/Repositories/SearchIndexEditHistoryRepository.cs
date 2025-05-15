using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class SearchIndexEditHistoryRepository : IRepository<SearchIndexEditHistory>
    {
        readonly ICommonDataContext iContext;
        public SearchIndexEditHistoryRepository(int tenant)
        {
            if (tenant <= 0)
                throw new ArgumentOutOfRangeException(nameof(tenant), "tenant must be greater than 0");
            iContext = CommonDataContext.GetContext(tenant);
        }

        public SearchIndexEditHistoryRepository(ICommonDataContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context), "context cannot be null");
            iContext = context;
        }

        public void Add(SearchIndexEditHistory entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "entity cannot be null");
            Context.SearchIndexEditHistories.Add(entity);
        }

        public void Remove(SearchIndexEditHistory entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "entity cannot be null");
            Context.SearchIndexEditHistories.Attach(entity);
            Context.SearchIndexEditHistories.Remove(entity);
        }

        public void Update(SearchIndexEditHistory entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "entity cannot be null");
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

        public SearchIndexEditHistory GetSingleSearchIndexEditHistory(string id, int tenant) =>
            Context.SearchIndexEditHistories.AsNoTracking().FirstOrDefault(x => x.Id == id && x.Tenant == tenant);

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

        public IQueryable<SearchIndexEditHistory> GetSearchIndexEditHistories(int tenant) =>
            Context.SearchIndexEditHistories.AsNoTracking().Where(x => x.Tenant == tenant);

        public List<string> GetRecent(int tenant, string screen, string entname, string userId, int size = 50) =>
            Context.SearchIndexEditHistories
                .AsNoTracking()
                .Where(x => x.Tenant == tenant && x.Screen == screen && x.Entname == entname && x.CreatedByUserId == userId)
                .GroupBy(x => x.KeyVal)
                .Select(g => g.OrderByDescending(e => e.CreateDate).FirstOrDefault())
                .OrderByDescending(x => x.CreateDate)
                .Select(x => x.KeyVal)
                .Take(size)
                .ToList();

        public void RemoveOldSearchData(int tenant, string screen, DateTime toDateTime)
        {
            List<SearchIndexEditHistory> oldSearches = Context.SearchIndexEditHistories
                .Where(x => x.Tenant == tenant && x.Screen == screen && x.CreateDate < toDateTime)
                .ToList();
            oldSearches.ForEach(x => Context.SearchIndexEditHistories.Remove(x));
            SubmitChanges();

            List <SearchIndexEditHistory> duplicateSearches = Context.SearchIndexEditHistories
              .Where(x => x.Tenant == tenant && x.Screen == screen)
              .GroupBy(x => x.KeyVal)
              .Where(g => g.Count() > 1)
              .SelectMany(g => g.OrderByDescending(e => e.CreateDate).Skip(1))
              .ToList();
            duplicateSearches.ForEach(x => Context.SearchIndexEditHistories.Remove(x));
            SubmitChanges();

            List<SearchIndexEditHistory> not50Recents = Context.SearchIndexEditHistories
                .Where(x => x.Tenant == tenant && x.Screen == screen)
                .OrderByDescending(x => x.CreateDate)
                .Skip(50)
                .ToList();
            not50Recents.ForEach(x => Context.SearchIndexEditHistories.Remove(x));
            SubmitChanges();
        }
    }
}

