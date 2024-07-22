using CWXSD;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.WcfApi;
using static Dropbox.Api.Team.UserSelectorArg;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class ShipmentHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(ShipmentPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            ShipmentPM shipmentPM = JsonConvert.DeserializeObject<ShipmentPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            ShipmentWcfService ShipmentWcfService = new ShipmentWcfService();
            Response response = ShipmentWcfService.Upsert(shipmentPM, batch);
            return response;
        }


        [System.Web.Http.HttpPost]
        public Response Cancel([FromBody] object[] t)//(string shipmentNumber, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string shipmentNumber = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            ShipmentWcfService ShipmentWcfService = new ShipmentWcfService();
            Response response = ShipmentWcfService.Cancel(shipmentNumber, tenant);
            return response;
        }


        [System.Web.Http.HttpPost]
        public Response Delete([FromBody] object[] t)//(string shipmentNumber, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string shipmentNumber = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            ShipmentWcfService ShipmentWcfService = new ShipmentWcfService();
            Response response = ShipmentWcfService.Delete(shipmentNumber, tenant);
            return response;
        }


        [System.Web.Http.HttpPost]
        public Response CreateEvent([FromBody] object[] t)//(int tenant, string externalId, string shipmentNumber, string userId, string eventTypeCode, DateTime logDate, DateTime eventDate, string notes)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string externalId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            string shipmentNumber = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            string userId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);
            string eventTypeCode = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[4]), jsonSerializerSettings);
            DateTime logDate = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(t[5]), jsonSerializerSettings);
            DateTime eventDate = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(t[6]), jsonSerializerSettings);
            string notes = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[7]), jsonSerializerSettings);


            ShipmentWcfService ShipmentWcfService = new ShipmentWcfService();
            Response response = ShipmentWcfService.CreateEvent(tenant, externalId, shipmentNumber, userId, eventTypeCode, logDate, eventDate, notes);
            return response;
        }


        [System.Web.Http.HttpPost]
        public Response BuildEventsList([FromBody] object[] t)//(int tenant, string shipmentNumber, List<TraceEventPM> eventsList)
        {
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string shipmentNumber = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            List<TraceEventPM> eventsList = JsonConvert.DeserializeObject<List<TraceEventPM>>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ShipmentWcfService ShipmentWcfService = new ShipmentWcfService();
            Response response = ShipmentWcfService.BuildEventsList(tenant, shipmentNumber, eventsList);
            return response;
        }
        
        
        [System.Web.Http.HttpPost]
        public List<ShipmentList> GetShipmentList([FromBody] object[] t)//(DataContracts.ShipmentApiFilters filters, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            DataContracts.ShipmentApiFilters filters = JsonConvert.DeserializeObject<DataContracts.ShipmentApiFilters>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ShipmentWcfService ShipmentWcfService = new ShipmentWcfService();
            List<ShipmentList> listResponse = ShipmentWcfService.GetShipmentList(filters, tenant, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public Response DeleteShipmentEvent([FromBody] object[] t)//(string shipmentNumber, string traceEventId, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string shipmentNumber = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string traceEventId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);


            ShipmentWcfService ShipmentWcfService = new ShipmentWcfService();
            Response response = ShipmentWcfService.DeleteShipmentEvent(shipmentNumber, traceEventId, tenant);
            return response;
        }


    }
    
}
