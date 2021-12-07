using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

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
        public List<Ports> GetPortsItemsList(string DIRECTIONID, string TRANSPORTMODEID, QueryOperations queryOperations)
        {

            if (DIRECTIONID == "E" && TRANSPORTMODEID == "A")
            {
                var queryservice = new ETBPORTQueryService(GetAmitalContext(tenant));
                var ETBPORTList = queryservice.GetList(queryOperations);
                return ETBPORTList;
            }
            else if (DIRECTIONID == "E" && TRANSPORTMODEID == "O")
            {
                var queryservice = new ETBPORTQueryService(GetAmitalContext(tenant));
                //var queryservice = new MTBCARRQueryService(GetAmitalContext(tenant));
                var MTBCARRList = queryservice.GetList(queryOperations);
                return MTBCARRList;
            }
            else if (DIRECTIONID == "I")
            {
                var queryservice = new ETBPORTQueryService(GetAmitalContext(tenant));
                //var queryservice = new ETBVENDQueryService(GetAmitalContext(tenant));
                var ETBVENDList = queryservice.GetList(queryOperations);
                return ETBVENDList;
            }
            return null;
        }
        public class Ports
        {
            public string Name;
            public string Code;
            public string CountryName;
        }
    }
}
