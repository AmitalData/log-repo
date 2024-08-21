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
    public class OceanInsightsHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Insert([FromBody] object[] t)//(int Tenant, string ScacCode, string ReferenceNo, string Type)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            int Tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string ScacCode = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            string ReferenceNo = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            string Type = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);


            OceanInsightsWcfService OceanInsightsWcfService = new OceanInsightsWcfService();
            Response response = OceanInsightsWcfService.Insert(Tenant, ScacCode, ReferenceNo, Type);
            return response;
        }

        [System.Web.Http.HttpPost]
        public async Task<Response> UnitedRequest([FromBody] object[] t)//(int Tenant, string ScacCode, string ReferenceNo, string Type)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int Tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string ScacCode = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            string ReferenceNo = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            string Type = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);

            OceanInsightsWcfService OceanInsightsWcfService = new OceanInsightsWcfService();
            Response response = await OceanInsightsWcfService.UnitedRequest(Tenant, ScacCode, ReferenceNo, Type);
            return response;

        }

        [System.Web.Http.HttpPost]
        public Response OceanInsight([FromBody] object[] t)//(int Tenant, string ScacCode, string ReferenceNo, string Type)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            int Tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string ScacCode = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            string ReferenceNo = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            string Type = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);


            OceanInsightsWcfService OceanInsightsWcfService = new OceanInsightsWcfService();
            Response response = OceanInsightsWcfService.OceanInsight(Tenant, ScacCode, ReferenceNo, Type);
            return response;
        }


        [System.Web.Http.HttpPost]
        public Response GetStatus([FromBody] object[] t)//(string RequestId, string Type) 
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string RequestId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string Type = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            
            OceanInsightsWcfService OceanInsightsWcfService = new OceanInsightsWcfService();
            Response response = OceanInsightsWcfService.GetStatus(RequestId, Type);
            return response;
        }
    }
}
