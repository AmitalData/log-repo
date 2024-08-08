using Logitude.BL.CommonDataModel.EntityLists;
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
    public class IncotermHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public List<IncotermList> GetIncoterms([FromBody] object[] t)//(ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);

            IncotermWcfService IncotermWcfService = new IncotermWcfService();
            List<IncotermList> listResponse = IncotermWcfService.GetIncoterms(ref response);
            return listResponse;
        }

        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(IncotermPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            IncotermPM incotermPM = JsonConvert.DeserializeObject<IncotermPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            IncotermWcfService IncotermWcfService = new IncotermWcfService();
            Response response = IncotermWcfService.Upsert(incotermPM, batch);
            return response;
        }
    }
}
