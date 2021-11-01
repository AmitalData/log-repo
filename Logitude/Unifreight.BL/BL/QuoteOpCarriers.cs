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
        public List<Carriers> GetCarriersItemsList(string DIRECTIONID, string TRANSPORTMODEID, QueryOperations queryOperations)
        {

            if (DIRECTIONID == "E" && TRANSPORTMODEID == "A")
            {
                var queryservice = new ETBAIRLINEQueryService(GetAmitalContext(tenant));
                var ETBAIRLINEList = queryservice.GetList(queryOperations);
            }
            if (DIRECTIONID == "E" && TRANSPORTMODEID == "O")
            {
                var queryservice = new MTBCARRQueryService(GetAmitalContext(tenant));
                var MTBCARRList = queryservice.GetList(queryOperations);
                return MTBCARRList;
            }
            if (DIRECTIONID == "I")
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
    }
}
