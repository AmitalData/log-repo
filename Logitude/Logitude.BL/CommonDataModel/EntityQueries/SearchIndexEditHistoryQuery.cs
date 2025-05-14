using System;
using System.Collections.Generic;
using Logitude.BL.CommonDataModel.EntityLists;
using System.Linq;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Newtonsoft.Json;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class SearchIndexEditHistoryQuery
    {
        SearchIndexEditHistoryRepository repository;
        public SearchIndexEditHistoryQuery(int tenant)
        {
            repository = new SearchIndexEditHistoryRepository(tenant);
        }

        public SearchIndexEditHistoryQuery(SearchIndexEditHistoryRepository roleRepository)
        {
            if (roleRepository == null)
                throw new ArgumentNullException(nameof(roleRepository), "roleRepository cannot be null");

            repository = roleRepository;
        }

        public SearchIndexEditHistoryPM GetSinglePM(string id, int tenant)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id", "id cannot be null or empty");

            SearchIndexEditHistory searchIndexEditHistory = repository.GetSingleSearchIndexEditHistory(id, tenant);
            if (searchIndexEditHistory == null)
                throw new Exception($"searchIndexEditHistory not found for id: {id}, tenatn: {tenant},");

            return new SearchIndexEditHistoryPM()
            {
                Id = searchIndexEditHistory.Id,
                Tenant = searchIndexEditHistory.Tenant,
                CreateDate = searchIndexEditHistory.CreateDate,
                CreatedByUserId = searchIndexEditHistory.CreatedByUserId,
                Screen = searchIndexEditHistory.Screen,
                ScreenParam = searchIndexEditHistory.ScreenParam,
                Entname = searchIndexEditHistory.Entname,
                KeyVal = searchIndexEditHistory.KeyVal,
            };
        }

        public List<dynamic> GetRecent(int tenant, string screen, string entname, string userId, int size = 50)
        {
            if (string.IsNullOrEmpty(screen))
                throw new ArgumentNullException("screen", "screen cannot be null or empty");
            if (string.IsNullOrEmpty(entname))
                throw new ArgumentNullException("entname", "entname cannot be null or empty");
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException("userId", "userId cannot be null or empty");

            return repository.GetRecent(tenant, screen, entname, userId, size)
                .Select(jsonString => JsonConvert.DeserializeObject<object>(jsonString))
                .ToList();
        }

        public IQueryable<SearchIndexEditHistoryList> GetIQueryableEntityList(IQueryable<SearchIndexEditHistory> iQueryable) =>
            iQueryable?.Select(x => new SearchIndexEditHistoryList()
            {
                Id = x.Id,
                Tenant = x.Tenant,
                CreateDate = x.CreateDate,
            });

        public void RemoveOldSearchData(int tenant, string screen, DateTime toDateTime)
        {
            if (string.IsNullOrEmpty(screen))
                throw new ArgumentNullException("screen", "screen cannot be null or empty");

            repository.RemoveOldSearchData(tenant, screen, toDateTime);
        }
    }
}
