using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;

namespace Unifreight.BL.BL
{
    public class QuoteOpCarriers
    {
        int tenant;
        public QuoteOpCarriers(int tenant)
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

        public Carriers GetFromCache(string DIRECTIONID, string TRANSPORTMODEID, string AIRLINEID)
        {
            IDataFromDB data;

            if (DIRECTIONID == "E" && TRANSPORTMODEID == "A")
                data = (IDataFromDB)new ETBAIRLINEQueryService(GetAmitalContext(tenant)).GetSingle(AIRLINEID, true);
            else if (DIRECTIONID == "E" && TRANSPORTMODEID == "O")
                data = (IDataFromDB)new MTBCARRQueryService(GetAmitalContext(tenant)).GetSingle(AIRLINEID, true);
            else if (DIRECTIONID == "I")
                data = (IDataFromDB)new ETBVENDQueryService(GetAmitalContext(tenant)).GetSingle(AIRLINEID, true);
            else
                return null;

            return new Carriers
            {
                Name = data.NAMEENG,
                AIRLINE_ID = data.AIRLINEID,
                Prefix = data.AIRLINENUM,
                VENDOR_ID = TRANSPORTMODEID == "A" && DIRECTIONID == "I" ? data.VENDORPREFIX : ""
            };
        }

        public List<Carriers> GetCarriersItemsList(string DIRECTIONID, string TRANSPORTMODEID, QueryOperations queryOperations, bool getFromCache = true)
        {
            if (getFromCache)
            {
                string cacheId = "carrierList" + DIRECTIONID + TRANSPORTMODEID + ";i:" + queryOperations.PageIndex + ";s:" + queryOperations.PageSize + ";d:" + queryOperations.SortDirectin + ";c:" + queryOperations.SortByColumnName + string.Join("", queryOperations.QueryFilterItems.Select(x => ";f:" + x.FieldName + ";v:" + x.FieldValue).ToArray());
                return CacheHelper.GetFromCache(cacheId, () => GetCarriersItemsList(DIRECTIONID, TRANSPORTMODEID, queryOperations, false));
            }


            if (DIRECTIONID == "E" && TRANSPORTMODEID == "A")
            {
                var queryservice = new ETBAIRLINEQueryService(GetAmitalContext(tenant));
                var ETBAIRLINEList = queryservice.GetList(queryOperations);
                return ETBAIRLINEList;
            }
            else if (DIRECTIONID == "E" && TRANSPORTMODEID == "O")
            {
                var queryservice = new MTBCARRQueryService(GetAmitalContext(tenant));
                var MTBCARRList = queryservice.GetList(queryOperations);
                return MTBCARRList;
            }
            else if (DIRECTIONID == "I")
            {
                var queryservice = new ETBVENDQueryService(GetAmitalContext(tenant));
                var ETBVENDList = queryservice.GetList(queryOperations, TRANSPORTMODEID);
                return ETBVENDList;
            }
            return null;
        }
        public class Carriers
        {
            public string Name;
            public string AIRLINE_ID;
            public string Prefix;
            public string VENDOR_ID;
        }

        public interface IDataFromDB
        {
            string NAMEENG { get; set; }
            string AIRLINEID { get; set; }
            string AIRLINENUM { get; set; }
            string VENDORID { get; set; }
            string VENDORPREFIX { get; set; }
        }

    }
}
