using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel;

namespace Unifreight.BL.BL
{
    public class QuoteOPIncoterms
    {
        int tenant;
        List<AmitalContext> _AmitalContextList = new List<AmitalContext>();

        public QuoteOPIncoterms(int tenant)
        {
            this.tenant = tenant;
        }

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

        public List<Incoterms> GetItemsList(QueryOperations queryOperations)
        {
            IQueryable<Incoterms> query = GetBaseQuery();
            query = AddFilter(query, queryOperations);
            query = AddSort(query, queryOperations);
            query = query.Skip(queryOperations.PageIndex * queryOperations.PageSize);
            query = query.Take(queryOperations.PageSize);

            List<Incoterms> list = query.ToList();
            return list;
        }

        private IQueryable<Incoterms> GetBaseQuery()
        {
            var MainContext = GetAmitalContext(tenant);

            return (from Incoterm in MainContext.ETBPAYTRs
                    select new Incoterms { Name = Incoterm.NAMEENG, PTERMID = Incoterm.PTERMID, SEARCHENG = Incoterm.SEARCHENG }); ;
        }

        private IQueryable<Incoterms> AddFilter(IQueryable<Incoterms> query, QueryOperations queryOperations)
        {
            foreach (var item in queryOperations.QueryFilterItems)
            {
                string val = item.FieldValue.ToString().ToLower();

                switch (item.FieldName)
                {
                    case "Name":
                        query = query.Where(o => o.Name.ToLower().Contains(val));
                        break;

                    case "PTERMID":
                        query = query.Where(o => o.PTERMID.ToLower().Contains(val));
                        break;

                    case "SearchFields":
                        query = query.Where(o => o.SEARCHENG.ToLower().Contains(val));
                        break;
                }
            }

            return query;
        }

        private static IQueryable<Incoterms> AddSort(IQueryable<Incoterms> query, QueryOperations queryOperations)
        {
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                bool asc = queryOperations.SortDirectin.ToLower() == "ascending";

                switch (queryOperations.SortByColumnName)
                {
                    case "Name":
                        query = asc ? query.OrderBy(o => o.Name) : query.OrderByDescending(o => o.Name);
                        break;

                    case "PTERMID":
                        query = asc ? query.OrderBy(o => o.PTERMID) : query.OrderByDescending(o => o.PTERMID);
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

        public class Incoterms
        {
            public string PTERMID;
            public string Name;
            public string SEARCHENG;
        }
    }
}
