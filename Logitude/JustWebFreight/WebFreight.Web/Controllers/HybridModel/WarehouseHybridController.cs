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
    public class WarehouseHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(WarehousePM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            WarehousePM warehousePM = JsonConvert.DeserializeObject<WarehousePM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            WarehouseWcfService WarehouseWcfService = new WarehouseWcfService();
            Response response = WarehouseWcfService.Upsert(warehousePM, batch);
            return response;
        }


        [System.Web.Http.HttpPost]
        public WarehousePM GetWarehousePM([FromBody] object[] t)//(string code, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string code = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            WarehouseWcfService WarehouseWcfService = new WarehouseWcfService();
            WarehousePM Response = WarehouseWcfService.GetWarehousePM(code, tenant, ref response);
            return Response;
        }
    }
}
