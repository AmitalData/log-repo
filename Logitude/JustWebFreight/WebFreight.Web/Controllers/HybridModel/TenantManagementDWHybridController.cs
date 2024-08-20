using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.BL.GlobalModel.EntityDws;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class TenantManagementDWHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public List<TenantManagementDW> GetTenantManagements([FromBody] object[] t)//(int tenant, int skip, int take, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int skip = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int take = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);

            TenantManagementDWWcfService TenantManagementDWWcfService = new TenantManagementDWWcfService();
            List<TenantManagementDW> listrespone = TenantManagementDWWcfService.GetTenantManagements(tenant, skip, take,  ref response);
            return listrespone;
        }
    }
}



