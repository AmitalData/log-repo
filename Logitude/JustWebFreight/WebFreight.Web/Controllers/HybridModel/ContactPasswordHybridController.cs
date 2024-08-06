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
    public class ContactPasswordHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response ChangeContactPassword([FromBody] object[] t)//(string email, string oldPassword, string newPassword) 
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string email = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string oldPassword = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            string newPassword = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ContactPasswordService ContactPasswordService = new ContactPasswordService();
            Response response = ContactPasswordService.ChangeContactPassword(email, oldPassword, newPassword);
            return response;

        }
    }
}
