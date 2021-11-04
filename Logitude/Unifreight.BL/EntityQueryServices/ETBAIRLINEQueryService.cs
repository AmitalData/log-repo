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
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System.Reflection;
using static Unifreight.BL.BL.QuoteOpCarriers;

namespace Unifreight.BL.EntityQueryServices
{
    public class ETBAIRLINEQueryService : EntityQueryService<ETBAIRLINE, ETBAIRLINEKeys, ETBAIRLINEPM, object, ETBAIRLINEKeys>
    {
        private AmitalContext MainContext;


        public ETBAIRLINEQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ETBAIRLINERepository(context);
            mapping = new ETBAIRLINEDataMapping();
        }

        public ETBAIRLINEPM GetSingle(string AIRLINEID, bool getFromCache)
        {
            var keys = new ETBAIRLINEKeys() { AIRLINEID = AIRLINEID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ETBAIRLINE entityPOCO)
        {
            return new ETBAIRLINEKeys() { AIRLINEID = entityPOCO.AIRLINEID };
        }
        public List<Carriers> GetList(QueryOperations queryOperations)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            var ETBAIRLINEquery = (from a in MainContext.ETBAIRLINEs
                                   select a);
            var cols = new Dictionary<string, string>()
            {
                { "Name", "NAMEENG" },
                { "AIRLINE_ID", "AIRLINEID" },
                { "Prefix", "AIRLINENUM" },
            };


            foreach (var item in queryOperations.QueryFilterItems)
            {
                switch (item.FieldName)
                {
                    case "Name":
                        ETBAIRLINEquery = ETBAIRLINEquery.Where(o => o.NAMEENG.Contains(item.FieldValue.ToString()));
                        break;
                    case "AIRLINE_ID":
                        ETBAIRLINEquery = ETBAIRLINEquery.Where(o => o.AIRLINEID.Contains(item.FieldValue.ToString()));
                        break;
                    case "Prefix":
                        ETBAIRLINEquery = ETBAIRLINEquery.Where(o => o.AIRLINENUM.Contains(item.FieldValue.ToString()));
                        break;
                    case "SearchFields":
                        ETBAIRLINEquery = ETBAIRLINEquery.Where(o => o.SEARCHENG.Contains(item.FieldValue.ToString()));
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
                            ETBAIRLINEquery = ETBAIRLINEquery.OrderBy(o => o.NAMEENG);
                        }
                        else
                        {
                            ETBAIRLINEquery = ETBAIRLINEquery.OrderByDescending(o => o.NAMEENG);
                        }
                        break;
                    case "AIRLINE_ID":
                        if (cols.ContainsKey(queryOperations.SortByColumnName) && queryOperations.SortDirectin.ToLower() == "ascending")
                        {
                            ETBAIRLINEquery = ETBAIRLINEquery.OrderBy(o => o.AIRLINEID);
                        }
                        else
                        {
                            ETBAIRLINEquery = ETBAIRLINEquery.OrderByDescending(o => o.AIRLINEID);
                        }
                        break;
                    case "Prefix":
                        if (cols.ContainsKey(queryOperations.SortByColumnName) && queryOperations.SortDirectin.ToLower() == "ascending")
                        {
                            ETBAIRLINEquery = ETBAIRLINEquery.OrderBy(o => o.AIRLINENUM);
                        }
                        else
                        {
                            ETBAIRLINEquery = ETBAIRLINEquery.OrderByDescending(o => o.AIRLINENUM);
                        }
                        break;
                    case "SearchFields":
                        if (cols.ContainsKey(queryOperations.SortByColumnName) && queryOperations.SortDirectin.ToLower() == "ascending")
                        {
                            ETBAIRLINEquery = ETBAIRLINEquery.OrderBy(o => o.SEARCHENG);
                        }
                        else
                        {
                            ETBAIRLINEquery = ETBAIRLINEquery.OrderByDescending(o => o.SEARCHENG);
                        }
                        break;
                }
                
            }
            else
            {
                ETBAIRLINEquery = ETBAIRLINEquery.OrderBy(o=>o.NAMEENG);
            }
            var res=ETBAIRLINEquery.Select(o => new Carriers
            {
                 Name = o.NAMEENG,
                 AIRLINE_ID = o.AIRLINEID,
                 Prefix = o.AIRLINENUM,
             });
            res = res.Skip(queryOperations.PageIndex * queryOperations.PageSize);
            res = res.Take(queryOperations.PageSize);
            
            List<Carriers> carrier = res.ToList();
            return carrier;
        }
       

    }
}





