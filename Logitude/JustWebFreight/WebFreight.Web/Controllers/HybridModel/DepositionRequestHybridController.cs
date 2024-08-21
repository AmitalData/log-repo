using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class DepositionLogboxRequestController : ApiController
    {
        [System.Web.Http.HttpPost]
        public async Task<Response> SendDepositionRequestTaskToLogBox([FromBody] object[] t)//(DepositionRequestPM depositionRequestPM)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            DepositionRequestPM depositionRequestPM = JsonConvert.DeserializeObject<DepositionRequestPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);

            DepositionRequestWcfService DepositionRequestWcfService = new DepositionRequestWcfService();
            Response response = await DepositionRequestWcfService.SendDepositionRequestTaskToLogBox(depositionRequestPM);
            return response;
        }
    }
}

