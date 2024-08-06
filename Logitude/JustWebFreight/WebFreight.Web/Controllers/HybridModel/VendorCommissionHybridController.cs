using Logitude.Customs.Def.EntityPMs;
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
    public class VendorCommissionHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(VendorCommissionPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            VendorCommissionPM vendorCommissionPM = JsonConvert.DeserializeObject<VendorCommissionPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            VendorCommissionWcfService VendorCommissionWcfService = new VendorCommissionWcfService();
            Response response = VendorCommissionWcfService.Upsert(vendorCommissionPM, batch);
            return response;
        }

        [System.Web.Http.HttpPost]
        public Response Delete([FromBody] object[] t)//(VendorCommissionPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            VendorCommissionPM vendorCommissionPM = JsonConvert.DeserializeObject<VendorCommissionPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            VendorCommissionWcfService VendorCommissionWcfService = new VendorCommissionWcfService();
            Response response = VendorCommissionWcfService.Delete(vendorCommissionPM, batch);
            return response;
        }

    }
}
