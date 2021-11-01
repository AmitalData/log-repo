using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Unifreight.BL.EntityDataMappings;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using static Unifreight.BL.BL.QuoteOpCarriers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace Unifreight.BL.EntityQueryServices
{

    public class ETBVENDQueryService : EntityQueryService<ETBVEND, ETBVENDKeys, ETBVENDPM, object, ETBVENDKeys>
    {
        private AmitalContext MainContext;
        public ETBVENDQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ETBVENDRepository(context);
            mapping = new ETBVENDDataMapping();
        }

        public ETBVENDPM GetSingle(string VENDORID, bool getFromCache)
        {
            var keys = new ETBVENDKeys() { VENDORID = VENDORID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ETBVEND entityPOCO)
        {
            return new ETBVENDKeys() { VENDORID = entityPOCO.VENDORID };
        }
        public List<Carriers> GetList(QueryOperations queryOperations,string TRANSPORTMODEID)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            var ETBVENDquery = (from a in MainContext.ETBVENDs
                                select a);
            var cols = new Dictionary<string, string>()
            {
                { "Name", "NAMEENG" },
                { "VENDOR_ID", "VENDORID" },
                { "Prefix", "VENDORPREFIX" },
            };


            foreach (var item in queryOperations.QueryFilterItems)
            {
                switch (item.FieldName)
                {
                    case "Name":
                        ETBVENDquery = ETBVENDquery.Where(o => o.NAMEENG.Contains(item.FieldValue.ToString()));
                        break;
                    case "VENDOR_ID":
                        ETBVENDquery = ETBVENDquery.Where(o => o.VENDORID.Contains(item.FieldValue.ToString()));
                        break;
                    case "Prefix":
                        ETBVENDquery = ETBVENDquery.Where(o => o.VENDORPREFIX.Contains(item.FieldValue.ToString()));
                        break;
                }
            }

            //extra filters
            ETBVENDquery = this.GetSpecialFilters(ETBVENDquery,TRANSPORTMODEID);


            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                switch (queryOperations.SortByColumnName)
                {
                    case "Name":
                        if (cols.ContainsKey(queryOperations.SortByColumnName) && queryOperations.SortDirectin.ToLower() == "ascending")
                        {
                            ETBVENDquery = ETBVENDquery.OrderBy(o => o.NAMEENG);
                        }
                        else
                        {
                            ETBVENDquery = ETBVENDquery.OrderByDescending(o => o.NAMEENG);
                        }
                        break;
                    case "VENDOR_ID":
                        if (cols.ContainsKey(queryOperations.SortByColumnName) && queryOperations.SortDirectin.ToLower() == "ascending")
                        {
                            ETBVENDquery = ETBVENDquery.OrderBy(o => o.VENDORID);
                        }
                        else
                        {
                            ETBVENDquery = ETBVENDquery.OrderByDescending(o => o.VENDORID);
                        }
                        break;
                    case "Prefix":
                        if (cols.ContainsKey(queryOperations.SortByColumnName) && queryOperations.SortDirectin.ToLower() == "ascending")
                        {
                            ETBVENDquery = ETBVENDquery.OrderBy(o => o.VENDORPREFIX);
                        }
                        else
                        {
                            ETBVENDquery = ETBVENDquery.OrderByDescending(o => o.VENDORPREFIX);
                        }
                        break;
                }

            }
            else
            {
                ETBVENDquery = ETBVENDquery.OrderBy(o => o.NAMEENG);
            }
            var res = ETBVENDquery.Select(o => new Carriers
            {
                Name = o.NAMEENG,
                VENDOR_ID = o.VENDORID,
                Prefix = TRANSPORTMODEID=="A"?o.VENDORPREFIX:"",
            });
            res = res.Skip(queryOperations.PageIndex * queryOperations.PageSize);
            res = res.Take(queryOperations.PageSize);

            List<Carriers> carrier = res.ToList();
            return carrier;
        }
        public IQueryable<ETBVEND> GetSpecialFilters(IQueryable<ETBVEND> ETBVENDquery,string TRANSPORTMODEID)
        {
            if(TRANSPORTMODEID == "O")
            {
                ETBVENDquery = GetFilterForOcean(ETBVENDquery);
            }
            if(TRANSPORTMODEID == "A")
            {
                ETBVENDquery = GetFilterForAir(ETBVENDquery);
            }
            return ETBVENDquery;
        }
        public IQueryable<ETBVEND> GetFilterForAir(IQueryable<ETBVEND> ETBVENDquery)
        {
            return ETBVENDquery.Where(o => o.ISHANDAGNT == "A");
        }
        public IQueryable<ETBVEND> GetFilterForOcean(IQueryable<ETBVEND> ETBVENDquery)
        {
            return ETBVENDquery.Where(o => o.ISHANDAGNT == "S");
        }
    }
}
}

