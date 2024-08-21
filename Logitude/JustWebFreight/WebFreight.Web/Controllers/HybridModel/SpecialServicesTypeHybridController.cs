using Logitude.BL.ShipmentsModel.EntityPMs;
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
    public class SpecialServicesTypeHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(SpecialServicesTypePM entityPM, bool batch)
        {

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            SpecialServicesTypePM specialServicesTypePM = JsonConvert.DeserializeObject<SpecialServicesTypePM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            SpecialServicesTypeWcfService SpecialServicesTypeWcfService = new SpecialServicesTypeWcfService();
            Response response = SpecialServicesTypeWcfService.Upsert(specialServicesTypePM, batch);
            return response;
        }
    }
}
