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
    public class UserIDNumberRequestHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response RequestUserIDNumber([FromBody] object[] t)//(UserIdNumberRequestPM userIdNumberRequestPM)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            UserIdNumberRequestPM userIdNumberRequestPM = JsonConvert.DeserializeObject<UserIdNumberRequestPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
           
            UserIDNumberRequestWcfService UserIDNumberRequestWcfService = new UserIDNumberRequestWcfService();
            Response response = UserIDNumberRequestWcfService.RequestUserIDNumber(userIdNumberRequestPM);
            return response;
        }
    }
}
