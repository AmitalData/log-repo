using Logitude.BL.CommonDataModel.EntityDws;
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
    public class PortHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(PortPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            PortPM portPM = JsonConvert.DeserializeObject<PortPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            PortWcfService PortWcfService = new PortWcfService();
            Response response = PortWcfService.Upsert(portPM, batch);
            return response;
        }

        [System.Web.Http.HttpPost]
        public List<Logitude.BL.CommonDataModel.EntityLists.PortList> GetList([FromBody] object[] t)//(ApiSearchFilters filters, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            ApiSearchFilters filters = JsonConvert.DeserializeObject<ApiSearchFilters>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            PortWcfService PortWcfService = new PortWcfService();
            List<Logitude.BL.CommonDataModel.EntityLists.PortList> listResponse = PortWcfService.GetList(filters,tenant, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public string GetPortId([FromBody] object[] t)//(DataContracts.PortApiFilters filters, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            DataContracts.PortApiFilters filters = JsonConvert.DeserializeObject<DataContracts.PortApiFilters>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);


            PortWcfService PortWcfService = new PortWcfService();
            string Response = PortWcfService.GetPortId(filters, tenant, ref response);
            return Response;
        }
    }
}
