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
    public class DefaultTypeHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(DefaultTypePM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            DefaultTypePM defaultTypePM = JsonConvert.DeserializeObject<DefaultTypePM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            DefaultTypeWcfService DefaultTypeWcfService = new DefaultTypeWcfService();
            Response response = DefaultTypeWcfService.Upsert(defaultTypePM, batch);
            return response;
        }

    }
}
