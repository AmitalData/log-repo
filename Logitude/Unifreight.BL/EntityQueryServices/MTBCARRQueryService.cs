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
    public class MTBCARRQueryService : EntityQueryService<MTBCARR, MTBCARRKeys, MTBCARRPM, object, MTBCARRKeys>
    {
        private AmitalContext MainContext;

        public MTBCARRQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new MTBCARRRepository(context);
            mapping = new MTBCARRDataMapping();
        }

        public MTBCARRPM GetSingle(string AIRLINEID, bool getFromCache)
        {
            var keys = new MTBCARRKeys() { AIRLINEID = AIRLINEID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(MTBCARR entityPOCO)
        {
            return new MTBCARRKeys() { AIRLINEID = entityPOCO.AIRLINEID };
        }
        public List<Carriers> GetList(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            var MTBCARRquery = (from a in MainContext.MTBCARRs
                                select a);
            var cols = new Dictionary<string, string>()
            {
                { "Name", "NAMEENG" },
                { "AIRLINE_ID", "AIRLINEID" },
                { "Prefix", "" },
            };


            foreach (var item in queryOperations.QueryFilterItems)
            {
                switch (item.FieldName)
                {
                    case "Name":
                        MTBCARRquery = MTBCARRquery.Where(o => o.NAMEENG.ToLower().Contains(item.FieldValue.ToString().ToLower()));
                        break;
                    case "AIRLINE_ID":
                        MTBCARRquery = MTBCARRquery.Where(o => o.AIRLINEID.ToLower().Contains(item.FieldValue.ToString().ToLower()));
                        break;
                    case "SearchFields":
                        MTBCARRquery = MTBCARRquery.Where(o => o.SEARCHENG.ToLower().Contains(item.FieldValue.ToString().ToLower()));
                        break;

                }
            }

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                switch (queryOperations.SortByColumnName)
                {
                    case "Name":
                        if (cols.ContainsKey(queryOperations.SortByColumnName) && queryOperations.SortDirectin.ToLower() == "ascending")
                        {
                            MTBCARRquery = MTBCARRquery.OrderBy(o => o.NAMEENG);
                        }
                        else
                        {
                            MTBCARRquery = MTBCARRquery.OrderByDescending(o => o.NAMEENG);
                        }
                        break;
                    case "AIRLINE_ID":
                        if (cols.ContainsKey(queryOperations.SortByColumnName) && queryOperations.SortDirectin.ToLower() == "ascending")
                        {
                            MTBCARRquery = MTBCARRquery.OrderBy(o => o.AIRLINEID);
                        }
                        else
                        {
                            MTBCARRquery = MTBCARRquery.OrderByDescending(o => o.AIRLINEID);
                        }
                        break;
                    case "SearchFields":
                        if (cols.ContainsKey(queryOperations.SortByColumnName) && queryOperations.SortDirectin.ToLower() == "ascending")
                        {
                            MTBCARRquery = MTBCARRquery.OrderBy(o => o.SEARCHENG);
                        }
                        else
                        {
                            MTBCARRquery = MTBCARRquery.OrderByDescending(o => o.SEARCHENG);
                        }
                        break;

                }

            }
            else
            {
                MTBCARRquery = MTBCARRquery.OrderBy(o => o.NAMEENG);
            }
            var res = MTBCARRquery.Select(o => new Carriers
            {
                Name = o.NAMEENG,
                AIRLINE_ID = o.AIRLINEID,
                Prefix = "",
            });

            if (queryOperations.PageSize != 0)
            {
                res = res.Skip(queryOperations.PageIndex * queryOperations.PageSize);
                res = res.Take(queryOperations.PageSize);
            }

            List<Carriers> carrier = res.ToList();
            return carrier;
        }
    }
}

