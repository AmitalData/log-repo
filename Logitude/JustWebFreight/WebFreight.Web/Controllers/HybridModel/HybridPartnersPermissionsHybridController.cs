using Logitude.BL.CommonDataModel.EntityLists;
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
    public class HybridPartnersPermissionsHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(int allowedTenant, int allowedByPartnerTenant, bool inActive)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            int allowedTenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int allowedByPartnerTenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            bool inActive = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);


            HybridPartnersPermissionsWcfService HybridPartnersPermissionsWcfService = new HybridPartnersPermissionsWcfService();
            Response response = HybridPartnersPermissionsWcfService.Upsert(allowedTenant, allowedByPartnerTenant, inActive);
            return response;
        }

        [System.Web.Http.HttpPost]
        public List<HybridPartnerList> GetAllowedPartners([FromBody] object[] t)//(int hybridPartnerTenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int hybridPartnerTenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);

            HybridPartnersPermissionsWcfService HybridPartnersPermissionsWcfService = new HybridPartnersPermissionsWcfService();
            List<HybridPartnerList> listResponse = HybridPartnersPermissionsWcfService.GetAllowedPartners(hybridPartnerTenant, ref response);
            return listResponse;
        }
    }
}
