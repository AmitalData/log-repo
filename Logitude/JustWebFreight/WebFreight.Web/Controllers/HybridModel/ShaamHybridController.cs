using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.Controllers.WebServices.Services;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class ShaamHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public ApiToShaamRes CreateConfirmationNumber([FromBody] object[] t)//(string invoiceJson, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string invoiceJson = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            ShaamWcfService ShaamWcfService = new ShaamWcfService();
            ApiToShaamRes response = ShaamWcfService.CreateConfirmationNumber(invoiceJson, tenant);
            return response;
        }
    }
}
