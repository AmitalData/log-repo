using Intuit.Ipp.DataService;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class ShippingAgentHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(ShippingAgentPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            ShippingAgentPM shippingAgentPM = JsonConvert.DeserializeObject<ShippingAgentPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            ShippingAgentWcfService ShippingAgentWcfService = new ShippingAgentWcfService();
            Response response = ShippingAgentWcfService.Upsert(shippingAgentPM, batch);
            return response;
        }


        [System.Web.Http.HttpPost]
        public ShippingAgentPM GetShippingAgentPM([FromBody] object[] t)//(string code, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string code = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ShippingAgentWcfService ShippingAgentWcfService = new ShippingAgentWcfService();
            ShippingAgentPM Response = ShippingAgentWcfService.GetShippingAgentPM(code, tenant, ref response);
            return Response;
        }
    }
}
