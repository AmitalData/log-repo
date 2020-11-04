using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Services
{
    public class ShipmentIntegrationService
    {
        private string apiController;
        public ShipmentIntegrationService()
        {
            this.apiController = "ShipmentIntegration";
        }

        //public async Task<ShipmentPM> GetTenantCounterSettings(string counterCode)
        //{
        //    HttpResponseMessage httpResponseMessage = await RestClientService.GetAsync(apiController + "/GetSingle?id=" + id);
        //    Assert.IsTrue(httpResponseMessage.StatusCode.ToString() == "OK");

        //    ShipmentPM servedShipment = RestClientService.ParseResponse<ShipmentPM>(httpResponseMessage);
        //    return servedShipment;
        //}
    }
}
