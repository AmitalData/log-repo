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
    public class StateHybridController : ApiController
    {

        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(StatePM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            StatePM statePM = JsonConvert.DeserializeObject<StatePM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            StateWcfService StateWcfService = new StateWcfService();
            Response response = StateWcfService.Upsert(statePM, batch);
            return response;
        }


    }
}
