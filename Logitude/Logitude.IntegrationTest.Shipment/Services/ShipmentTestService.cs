using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Services
{
    public class ShipmentTestService: IntegrationService
    {
        protected override string ApiController => "Shipment";

        public async Task<ShipmentPM> GetShipment(string id)
        {
            Response = await RestClientService.GetAsync(ApiController + "/GetSingle?id=" + id);

            Assert.IsTrue(Response.StatusCode == System.Net.HttpStatusCode.OK);

            ShipmentPM responseEntity = RestClientService.ParseResponse<ShipmentPM>(Response);
            return responseEntity;
        }

        public async Task<string> CreateShipment(ShipmentPM entityPM, bool isAsserting = true)
        {
            Response = await RestClientService.PostAsync(entityPM, ApiController);

            if (isAsserting)
            {
                Assert.IsTrue(Response.StatusCode == System.Net.HttpStatusCode.OK);
            }

            if (HasException)
            {
                return null;
            }

            else
            {
                ShipmentPM responseEntity = RestClientService.ParseResponse<ShipmentPM>(Response);
                return responseEntity.Id;
            }
        }

        public async Task<ShipmentPM> UpdateShipment(ShipmentPM entityPM, bool isAsserting = true)
        {
            Response = await RestClientService.PutAsync(entityPM, ApiController);

            if (isAsserting)
            {
                Assert.IsTrue(Response.StatusCode == System.Net.HttpStatusCode.OK);
            }

            if (HasException)
            {
                return entityPM;
            }

            else
            {
                ShipmentPM responseEntity = RestClientService.ParseResponse<ShipmentPM>(Response);
                return responseEntity;
            }
        }


    }
}
