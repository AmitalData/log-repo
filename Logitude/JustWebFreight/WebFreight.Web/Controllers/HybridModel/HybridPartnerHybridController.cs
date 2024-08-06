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
    public class HybridPartnerHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public List<HybridPartnerList> GetMislakaPartners([FromBody] object[] t)//(int tenant, ref Logitude.Server.Tools.Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);

            HybridPartnerWcfService HybridPartnerWcfService = new HybridPartnerWcfService();
            List<HybridPartnerList> listResponse = HybridPartnerWcfService.GetMislakaPartners( tenant, ref response);
            return listResponse;
        }
    }
}
