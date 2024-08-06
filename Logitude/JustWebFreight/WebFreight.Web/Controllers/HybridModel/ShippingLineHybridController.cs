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
    public class ShippingLineHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(ShippingLinePM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            ShippingLinePM shippingLinePM = JsonConvert.DeserializeObject<ShippingLinePM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            ShippingLineWcfService ShippingLineWcfService = new ShippingLineWcfService();
            Response response = ShippingLineWcfService.Upsert(shippingLinePM, batch);
            return response;
        }

        [System.Web.Http.HttpPost]
        public ShippingLinePM GetShippingLinePM([FromBody] object[] t)//(string code, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string code = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ShippingLineWcfService ShippingLineWcfService = new ShippingLineWcfService();
            ShippingLinePM Response = ShippingLineWcfService.GetShippingLinePM(code, tenant, ref response);
            return Response;
        }
    }
}
