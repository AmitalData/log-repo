using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityPOCOs;

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

        public List<Ports> GetItemsList(string DIRECTIONID, string TRANSPORTMODEID, QueryOperations queryOperations)
        {
            IQueryable<Ports> query = GetBaseQuery(DIRECTIONID, TRANSPORTMODEID);
            query = AddFilter(query, queryOperations);
            query = AddSort(query, queryOperations);
            query = query.Skip(queryOperations.PageIndex * queryOperations.PageSize);
            query = query.Take(queryOperations.PageSize);

            List<Ports> list = query.ToList();
            return list;
        }

        private IQueryable<Ports> GetBaseQuery(string DIRECTIONID, string TRANSPORTMODEID)
        {
            var MainContext = GetAmitalContext(tenant);

            if (DIRECTIONID == "E" && TRANSPORTMODEID == "A")
                return (from port in MainContext.ETBPORTs
                        join country in MainContext.CTBCOUNTRIES on port.COUNTRYID equals country.COUNTRYID
                        select new Ports { Name = port.NAMEENG, Code = port.PORTID, CountryName = country.NAMEENG, SEARCHENG = port.SEARCHENG });

            else if (DIRECTIONID == "E" && TRANSPORTMODEID == "O")
                return (from port in MainContext.MTBPORTs
                        join country in MainContext.CTBCOUNTRIES on port.COUNTRYID equals country.COUNTRYID
                        select new Ports { Name = port.NAMEENG, Code = port.PORTID, CountryName = country.NAMEENG, SEARCHENG = port.SEARCHENG });

            else if (DIRECTIONID == "I" && TRANSPORTMODEID == "A")
                return (from port in MainContext.ITBPORTs
                        join country in MainContext.CTBCOUNTRIES on port.COUNTRYID equals country.COUNTRYID
                        select new Ports { Name = port.NAMEENG, Code = port.PORTID, CountryName = country.NAMEENG, SEARCHENG = port.SEARCHENG });

            else if (DIRECTIONID == "I" && TRANSPORTMODEID == "O")
                return (from port in MainContext.RTBPORTs
                        join country in MainContext.CTBCOUNTRIES on port.COUNTRYID equals country.COUNTRYID
                        select new Ports { Name = port.NAMEENG, Code = port.PORTID, CountryName = country.NAMEENG, SEARCHENG = port.SEARCHENG });

            return null;
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

        public class Ports
        {
            public string Name;
            public string Code;
            public string CountryName;
            public string SEARCHENG;
        }
    }
}
