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
using static Unifreight.BL.BL.QuoteOPPorts;

namespace Unifreight.BL.EntityQueryServices
{
    public class ETBPORTQueryService : EntityQueryService<ETBPORT, ETBPORTKeys, ETBPORTPM, object, ETBPORTKeys>
    {
        private AmitalContext MainContext;

        public ETBPORTQueryService(AmitalContext context)
        {
            MainContext = context;
            Repository = new ETBPORTRepository(context);
            mapping = new ETBPORTDataMapping();
        }

        public ETBPORTPM GetSingle(string PORTID, bool getFromCache)
        {
            var keys = new ETBPORTKeys() { PORTID = PORTID };
            return base.GetSingle(keys, false, getFromCache);
        }

        protected override Simplog.Server.Infrastructure.EntityKeyFields GetKeys(ETBPORT entityPOCO)
        {
            return new ETBPORTKeys() { PORTID = entityPOCO.PORTID };
        }

        public List<Ports> GetList(QueryOperations queryOperations)
        {
            var ETBPORTquery = (from ETBPORT in MainContext.ETBPORTs
                                join country in MainContext.CTBCOUNTRIES on ETBPORT.COUNTRYID equals country.COUNTRYID
                                select new { Name = ETBPORT.NAMEENG, Code = ETBPORT.PORTID, CountryName = country.NAMEENG, SEARCHENG = ETBPORT.SEARCHENG });

            foreach (var item in queryOperations.QueryFilterItems)
            {
                string val = item.FieldValue.ToString().ToLower();

                switch (item.FieldName)
                {
                    case "Name":
                        ETBPORTquery = ETBPORTquery.Where(o => o.Name.ToLower().Contains(val));
                        break;
                    case "Code":
                        ETBPORTquery = ETBPORTquery.Where(o => o.Code.ToLower().Contains(val));
                        break;
                    case "CountryName":
                        ETBPORTquery = ETBPORTquery.Where(o => o.CountryName.ToLower().Contains(val));
                        break;
                    case "SearchFields":
                        ETBPORTquery = ETBPORTquery.Where(o => o.SEARCHENG.ToLower().Contains(val));
                        break;
                }
            }

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                bool asc = queryOperations.SortDirectin.ToLower() == "ascending";

                switch (queryOperations.SortByColumnName)
                {
                    case "Name":
                        ETBPORTquery = asc ? ETBPORTquery.OrderBy(o => o.Name) : ETBPORTquery.OrderByDescending(o => o.Name);
                        break;

                    case "Code":
                        ETBPORTquery = asc ? ETBPORTquery.OrderBy(o => o.Code) : ETBPORTquery.OrderByDescending(o => o.Code);
                        break;

                    case "CountryName":
                        ETBPORTquery = asc ? ETBPORTquery.OrderBy(o => o.CountryName) : ETBPORTquery.OrderByDescending(o => o.CountryName);
                        break;

                    case "SearchFields":
                        ETBPORTquery = asc ? ETBPORTquery.OrderBy(o => o.SEARCHENG) : ETBPORTquery.OrderByDescending(o => o.SEARCHENG);
                        break;
                }
            }
            else
                ETBPORTquery = ETBPORTquery.OrderBy(o => o.Name);

            var res = ETBPORTquery.Select(o => new Ports
            {
                Name = o.Name,
                Code = o.Code,
                CountryName = o.CountryName,
            });

            res = res.Skip(queryOperations.PageIndex * queryOperations.PageSize);
            res = res.Take(queryOperations.PageSize);

            List<Ports> carrier = res.ToList();
            return carrier;
        }
    }
}

