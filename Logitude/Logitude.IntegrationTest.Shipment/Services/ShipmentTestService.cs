using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Services
{
    public class ShipmentTestService
    {
        private string apiController;
        public HttpResponseMessage HttpResponseMessage { get; private set; }
        public ShipmentTestService()
        {
            this.apiController = "Shipment";
        }

        public async Task<string> CreateShipment(ShipmentPM entityPM)
        {
            HttpResponseMessage = await RestClientService.PostAsync(entityPM, apiController);
            Assert.IsTrue(HttpResponseMessage.StatusCode.ToString() == "OK");

            ShipmentPM servedShipment = RestClientService.ParseResponse<ShipmentPM>(HttpResponseMessage);
            return servedShipment.Id;
        }

        public async Task<ShipmentPM> UpdateShipment(ShipmentPM entityPM)
        {
            HttpResponseMessage = await RestClientService.PutAsync(entityPM, apiController);
            Assert.IsTrue(HttpResponseMessage.StatusCode.ToString() == "OK");

            ShipmentPM servedShipment = RestClientService.ParseResponse<ShipmentPM>(HttpResponseMessage);
            return servedShipment;
        }

        public async Task<ShipmentPM> GetShipment(string id)
        {
            HttpResponseMessage = await RestClientService.GetAsync(apiController + "/GetSingle?id=" + id);
            Assert.IsTrue(HttpResponseMessage.StatusCode.ToString() == "OK");

            ShipmentPM servedShipment = RestClientService.ParseResponse<ShipmentPM>(HttpResponseMessage);
            return servedShipment;
        }
    }
}
