using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
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
    public class ExternalDirectAPITestService
    {
        private string apiController;
        public HttpResponseMessage HttpResponseMessage { get; private set; }
        public ExternalDirectAPITestService()
        {
            this.apiController = "Direct";
        }

        public async Task<string> CreateDirect(Direct entityPM)
        {
            HttpResponseMessage = await RestClientService.PostAsync(entityPM, apiController);
            Assert.IsTrue(HttpResponseMessage.StatusCode.ToString() == "OK");

            Direct servedShipment = RestClientService.ParseResponse<Direct>(HttpResponseMessage);
            return servedShipment.Id;
        }

        public async Task<Direct> UpdateDirect(Direct entityPM, string errorMessage = null)
        {
            HttpResponseMessage = await RestClientService.PutAsync(entityPM, apiController);

            if (errorMessage != null)
            {
                IntegrationTestException ex = RestClientService.ParseResponse<IntegrationTestException>(HttpResponseMessage);

                Assert.IsTrue(HttpResponseMessage.StatusCode.ToString() == "400");
                Assert.IsTrue(ex.ErrorMessage == errorMessage);
                return entityPM;
            }

            else
            {
                Assert.IsTrue(HttpResponseMessage.StatusCode.ToString() == "OK");

                Direct servedShipment = RestClientService.ParseResponse<Direct>(HttpResponseMessage);
                return servedShipment;
            }
        }

        public async Task<Direct> GetDirect(string id)
        {
            HttpResponseMessage = await RestClientService.GetAsync(apiController + "/GetSingleDirect?id=" + id);
            Assert.IsTrue(HttpResponseMessage.StatusCode.ToString() == "OK");

            Direct servedShipment = RestClientService.ParseResponse<Direct>(HttpResponseMessage);
            return servedShipment;
        }
    }
}
