using Logitude.Server.Tools;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.Inteface;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using static Unifreight.BL.BL.QuoteOPPortsHelper;

namespace Unifreight.BL.BL
{
    public class QuoteOPPorts
    {
        int tenant;
        public QuoteOPPorts(int tenant)
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

        public Ports GetFromCacheWithCountry(string DIRECTIONID, string TRANSPORTMODEID, string portId)
        {
            var context = GetAmitalContext(tenant);

            IGetSinglePortFromCacheWithCountry service = null;

            if (DIRECTIONID == "E" && TRANSPORTMODEID == "A")
                service = new ETBPORTQueryService(context);


            else if (DIRECTIONID == "E" && TRANSPORTMODEID == "O")
                service = new MTBPORTQueryService(context);


            else if (DIRECTIONID == "I" && TRANSPORTMODEID == "A")
                service = new ITBPORTQueryService(context);

            else if (DIRECTIONID == "I" && TRANSPORTMODEID == "O")
                service = new RTBPORTQueryService(context);
            else
                return null;

            Ports ports = service.GetSingleFromCacheWithCountry(portId);

            return ports;
        }

        public List<Ports> GetItemsList(string DIRECTIONID, string TRANSPORTMODEID, QueryOperations queryOperations, bool getFromCache = true)
        {
            if (getFromCache)
            {
                string cacheId = "ports" + DIRECTIONID + TRANSPORTMODEID + ";i:" + queryOperations.PageIndex + ";s:" + queryOperations.PageSize + ";d:" + queryOperations.SortDirectin + ";c:" + queryOperations.SortByColumnName + string.Join("", queryOperations.QueryFilterItems.Select(x => ";f:" + x.FieldName + ";v:" + x.FieldValue).ToArray());
                return CacheHelper.GetFromCache(cacheId, () => GetItemsList(DIRECTIONID, TRANSPORTMODEID, queryOperations, false));
            }

            IQueryable<Ports> query = GetBaseQuery(DIRECTIONID, TRANSPORTMODEID);
            if (query == null)
                return null;

            query = AddFilter(query, queryOperations);
            query = AddSort(query, queryOperations);

            if (queryOperations.PageSize != 0)
            {
                query = query.Skip(queryOperations.PageIndex * queryOperations.PageSize);
                query = query.Take(queryOperations.PageSize);
            }

            List<Ports> list = query.ToList();
            return list;
        }

        private IQueryable<Ports> GetBaseQuery(string DIRECTIONID, string TRANSPORTMODEID)
        {
            var MainContext = GetAmitalContext(tenant);

            IQueryable<QPorts> baseQ;

            if (DIRECTIONID == "E" && TRANSPORTMODEID == "A")
                baseQ = (from port in MainContext.ETBPORTs select new QPorts { NAMEENG = port.NAMEENG, PORTID = port.PORTID, COUNTRYID = port.COUNTRYID, SEARCHENG = port.SEARCHENG });

            else if (DIRECTIONID == "E" && TRANSPORTMODEID == "O")
                baseQ = (from port in MainContext.MTBPORTs select new QPorts { NAMEENG = port.NAMEENG, PORTID = port.PORTID, COUNTRYID = port.COUNTRYID, SEARCHENG = port.SEARCHENG });

            else if (DIRECTIONID == "I" && TRANSPORTMODEID == "A")
                baseQ = (from port in MainContext.ITBPORTs select new QPorts { NAMEENG = port.NAMEENG, PORTID = port.PORTID, COUNTRYID = port.COUNTRYID, SEARCHENG = port.SEARCHENG });

            else if (DIRECTIONID == "I" && TRANSPORTMODEID == "O")
                baseQ = (from port in MainContext.RTBPORTs select new QPorts { NAMEENG = port.NAMEENG, PORTID = port.PORTID, COUNTRYID = port.COUNTRYID, SEARCHENG = port.SEARCHENG });

            else 
                return null;

            baseQ  = baseQ.Where(port => port.COUNTRYID != null);

            IQueryable<Ports> q = baseQ.Join(MainContext.CTBCOUNTRIES, port => port.COUNTRYID, country => country.COUNTRYCODE,
                 (port, country) => new Ports { Name = port.NAMEENG, Code = port.PORTID, CountryName = country.NAMEENG, SEARCHENG = port.SEARCHENG });

            return q;
        }

        private IQueryable<Ports> AddFilter(IQueryable<Ports> query, QueryOperations queryOperations)
        {
            foreach (var item in queryOperations.QueryFilterItems)
            {
                string val = item.FieldValue.ToString().ToLower();

                switch (item.FieldName)
                {
                    case "Name":
                        query = query.Where(o => o.Name.ToLower().Contains(val));
                        break;
                    case "Code":
                        query = query.Where(o => o.Code.ToLower().Contains(val));
                        break;
                    case "CountryName":
                        query = query.Where(o => o.CountryName.ToLower().Contains(val));
                        break;
                    case "SearchFields":
                        query = query.Where(o => o.SEARCHENG.ToLower().Contains(val));
                        break;
                }
            }

            return query;
        }

        private static IQueryable<Ports> AddSort(IQueryable<Ports> query, QueryOperations queryOperations)
        {
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                bool asc = queryOperations.SortDirectin.ToLower() == "ascending";

                switch (queryOperations.SortByColumnName)
                {
                    case "Name":
                        query = asc ? query.OrderBy(o => o.Name) : query.OrderByDescending(o => o.Name);
                        break;

                    case "Code":
                        query = asc ? query.OrderBy(o => o.Code) : query.OrderByDescending(o => o.Code);
                        break;

                    case "CountryName":
                        query = asc ? query.OrderBy(o => o.CountryName) : query.OrderByDescending(o => o.CountryName);
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
    }
}
