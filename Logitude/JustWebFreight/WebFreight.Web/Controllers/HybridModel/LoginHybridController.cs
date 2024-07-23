using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.WcfApi;
using WebFreight.Web.Helpers;


namespace WebFreight.Web.Controllers.HybridModel
{
    public class LoginHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response LoginByCredential([FromBody] object[] t)//(string email, APICredentialsParameters apiCredentialsParam)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string email = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            APICredentialsParameters apiCredentialsParam = JsonConvert.DeserializeObject<APICredentialsParameters>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);

            LoginWcfService LoginWcfService = new LoginWcfService();
            Response Response = LoginWcfService.LoginByCredential(email, apiCredentialsParam);
            return Response;
        }

        [System.Web.Http.HttpPost]
        public Response Login([FromBody] object[] t)//(string email, string password)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string email = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string password = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);

            LoginWcfService LoginWcfService = new LoginWcfService();
            Response Response = LoginWcfService.Login(email, password);
            return Response;
        }


        [System.Web.Http.HttpPost]
        public List<Helpers.TenantInfo> GetUserTenants([FromBody] object[] t)//(string email, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string email = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);

            LoginWcfService LoginWcfService = new LoginWcfService();
            List<Helpers.TenantInfo> listResponse = LoginWcfService.GetUserTenants(email, ref response);
            return listResponse;
        }

    }
}
