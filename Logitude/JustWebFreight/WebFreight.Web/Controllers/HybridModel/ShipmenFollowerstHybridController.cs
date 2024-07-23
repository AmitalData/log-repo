using Logitude.BL.CommonDataModel.EntityLists;
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
    public class ShipmenFollowerstHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public List<ContactList> GetShipmentFollowersByShipmentNumber([FromBody] object[] t)//(string shipmentNumber, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string shipmentNumber = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ShipmenFollowerstWcfService ShipmenFollowerstWcfService = new ShipmenFollowerstWcfService();
            List<ContactList> listResponse = ShipmenFollowerstWcfService.GetShipmentFollowersByShipmentNumber(shipmentNumber, tenant, ref response);
            return listResponse;
        }
    }
}
