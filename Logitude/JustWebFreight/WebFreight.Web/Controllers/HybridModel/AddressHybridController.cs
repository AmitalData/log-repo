using Growl.Connector;
using Logitude.BL.CommonDataModel.EntityPMs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebFreight.Web.WcfApi;
using Logitude.Server.Tools;
using Response = Logitude.Server.Tools.Response;



namespace WebFreight.Web.Controllers.HybridModel
{
    //api/HybridModel
    public class AddressHybridController : ApiController
    {
        [System.Web.Http.HttpPost]

        public Response Upsert([FromBody] object[] t)//(AddressPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            AddressPM addressPM = JsonConvert.DeserializeObject<AddressPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            AddressWcfService AddressWcfService = new AddressWcfService();
            Response response = AddressWcfService.Upsert(addressPM, batch);
            return response;
        }

        [System.Web.Http.HttpPost]

        public AddressPM GetAddressByExternalId([FromBody] object[] t)//(string externalId, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string externalId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            AddressWcfService AddressWcfService = new AddressWcfService();
            AddressPM Response = AddressWcfService.GetAddressByExternalId(externalId, tenant, ref response);
            return Response;
        }
    }
}