using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    //api/HybridModel
    public class AirlineHybridController : ApiController
    {

        [System.Web.Http.HttpPost]

        public Response Upsert([FromBody] object[] t)//(AirlinePM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            AirlinePM airlinePM = JsonConvert.DeserializeObject<AirlinePM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            AirlineWcfService AirlineWcfService = new AirlineWcfService();
            Response response = AirlineWcfService.Upsert(airlinePM, batch);
            return response;
        }


        [System.Web.Http.HttpPost]

        public AirlinePM GetAirlinePM([FromBody] object[] t)//(string code, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string code = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            AirlineWcfService AirlineWcfService = new AirlineWcfService();
            AirlinePM Response = AirlineWcfService.GetAirlinePM(code, tenant, ref response);
            return Response;
        }
    }
}