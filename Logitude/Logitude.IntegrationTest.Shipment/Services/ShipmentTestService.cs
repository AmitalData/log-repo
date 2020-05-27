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
        public async Task<string> CreateShipment(ShipmentPM entityPM)
        {
            HttpResponseMessage httpResponseMessage = await RestClientService.PostAsync(entityPM, "shipment");
            Assert.IsTrue(httpResponseMessage.StatusCode.ToString() == "OK");

            ShipmentPM servedShipment = RestClientService.ParseResponse<ShipmentPM>(httpResponseMessage);
            return servedShipment.Id;
        }

        public async Task<ShipmentPM> UpdateShipment(ShipmentPM entityPM)
        {
            HttpResponseMessage httpResponseMessage = await RestClientService.PutAsync(entityPM, "shipment");
            Assert.IsTrue(httpResponseMessage.StatusCode.ToString() == "OK");

            ShipmentPM servedShipment = RestClientService.ParseResponse<ShipmentPM>(httpResponseMessage);
            return servedShipment;
        }

        public async Task<ShipmentPM> GetShipment(string id)
        {
            HttpResponseMessage httpResponseMessage = await RestClientService.GetAsync("Shipment/GetSingle?id=" + id);
            Assert.IsTrue(httpResponseMessage.StatusCode.ToString() == "OK");

            ShipmentPM servedShipment = RestClientService.ParseResponse<ShipmentPM>(httpResponseMessage);
            return servedShipment;
        }
    }
}
