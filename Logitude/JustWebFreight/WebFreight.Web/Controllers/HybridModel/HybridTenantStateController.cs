using Logitude.BL.CommonDataModel.EntityPMs;
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
    public class HybridTenantStateController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(HybridTenantStatePM entitypm, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            HybridTenantStatePM hybridTenantStatePM = JsonConvert.DeserializeObject<HybridTenantStatePM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            HybridTenantStateWcfService HybridTenantStateWcfService = new HybridTenantStateWcfService();
            Response response = HybridTenantStateWcfService.Upsert(hybridTenantStatePM, batch);
            return response;
        }
    }
}
