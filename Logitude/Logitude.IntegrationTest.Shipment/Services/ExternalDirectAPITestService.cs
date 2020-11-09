using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.IntegrationTest.Core;
using Logitude.IntegrationTest.Core.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Services
{
    public class ExternalDirectAPITestService : IntegrationService
    {
        protected override string ApiController => "Direct";

        public async Task<Direct> GetDirect(string id)
        {
            Response = await RestClientService.GetAsync(ApiController + "/GetSingleDirect?id=" + id);

            Assert.IsTrue(Response.StatusCode == System.Net.HttpStatusCode.OK);

            Direct responseEntity = RestClientService.ParseResponse<Direct>(Response);
            return responseEntity;
        }

        public async Task<string> CreateDirect(Direct entityPM, bool isAsserting = true)
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
                Direct responseEntity = RestClientService.ParseResponse<Direct>(Response);
                return responseEntity.Id;
            }
        }

        public async Task<Direct> UpdateDirect(Direct entityPM, bool isAsserting = true)
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
                Direct responseEntity = RestClientService.ParseResponse<Direct>(Response);
                return responseEntity;
            }            
        }


    }
}
