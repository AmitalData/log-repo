using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel;

namespace Unifreight.BL.BL
{
    public class QuoteOPSpecialServices
    {
        int tenant;
        public QuoteOPSpecialServices(int tenant)
        {
            this.tenant = tenant;
        }
        List<AmitalContext> _AmitalContextList = new List<AmitalContext>();
        AmitalContext GetAmitalContext(int tenant)
        {
            var tenantAmitalContext = _AmitalContextList.FirstOrDefault(rec => rec.TenantSeed == tenant);
            if (tenantAmitalContext == null)
            {

                tenantAmitalContext = AmitalContext.GetContext(tenant);
                _AmitalContextList.Add(tenantAmitalContext);
            }
            return tenantAmitalContext;
        }

        public List<SpecialServices> GetItemsList(string DIRECTIONID, string TRANSSpecialServiceMODEID, QueryOperations queryOperations, bool getFromCache = true)
        {
            if (getFromCache)
            {
                string cacheId = "specialServices" + DIRECTIONID + TRANSSpecialServiceMODEID + ";i:" + queryOperations.PageIndex + ";s:" + queryOperations.PageSize + ";d:" + queryOperations.SortDirectin + ";c:" + queryOperations.SortByColumnName + string.Join("", queryOperations.QueryFilterItems.Select(x => ";f:" + x.FieldName + ";v:" + x.FieldValue).ToArray());
                return CacheHelper.GetFromCache(cacheId, () => GetItemsList(DIRECTIONID, TRANSSpecialServiceMODEID, queryOperations, false));
            }
            IQueryable<SpecialServices> query = GetBaseQuery(DIRECTIONID, TRANSSpecialServiceMODEID);
            query = AddFilter(query, queryOperations);
            query = AddSort(query, queryOperations);
            query = query.Skip(queryOperations.PageIndex * queryOperations.PageSize);
            query = query.Take(queryOperations.PageSize);

            List<SpecialServices> list = query.ToList();
            return list;
        }

        private IQueryable<SpecialServices> GetBaseQuery(string DIRECTIONID, string TRANSSpecialServiceMODEID)
        {
            var MainContext = GetAmitalContext(tenant);

            if (DIRECTIONID == "E" && TRANSSpecialServiceMODEID == "A")
                return (from SpecialService in MainContext.ETBSERLVs
                        select new SpecialServices { Name = SpecialService.NAMEENG, SERVLEVEL_ID = SpecialService.SERVLEVELID, SEARCHENG = SpecialService.SEARCHENG });

            return (from SpecialService in MainContext.GTBSERLVs
                    select new SpecialServices { Name = SpecialService.NAMEENG, SERVLEVEL_ID = SpecialService.SERVLEVELID, SEARCHENG = SpecialService.SEARCHENG }); ;
        }

        private IQueryable<SpecialServices> AddFilter(IQueryable<SpecialServices> query, QueryOperations queryOperations)
        {
            foreach (var item in queryOperations.QueryFilterItems)
            {
                string val = item.FieldValue.ToString().ToLower();

                switch (item.FieldName)
                {
                    case "Name":
                        query = query.Where(o => o.Name.ToLower().Contains(val));
                        break;

                    case "SERVLEVEL_ID":
                        query = query.Where(o => o.SERVLEVEL_ID.ToLower().Contains(val));
                        break;

                    case "SearchFields":
                        query = query.Where(o => o.SEARCHENG.ToLower().Contains(val));
                        break;
                }
            }

            return query;
        }

        private static IQueryable<SpecialServices> AddSort(IQueryable<SpecialServices> query, QueryOperations queryOperations)
        {
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                bool asc = queryOperations.SortDirectin.ToLower() == "ascending";

                switch (queryOperations.SortByColumnName)
                {
                    case "Name":
                        query = asc ? query.OrderBy(o => o.Name) : query.OrderByDescending(o => o.Name);
                        break;

                    case "SERVLEVEL_ID":
                        query = asc ? query.OrderBy(o => o.SERVLEVEL_ID) : query.OrderByDescending(o => o.SERVLEVEL_ID);
                        break;

                      case "SearchFields":
                        query = asc ? query.OrderBy(o => o.SEARCHENG) : query.OrderByDescending(o => o.SEARCHENG);
                        break;
                }
            }
            else
                query = query.OrderBy(o => o.Name);
            return query;
        }

        public class SpecialServices
        {
            public string SERVLEVEL_ID;
            public string Name;
            public string SEARCHENG;
        }
    }
}
