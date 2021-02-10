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
    public class ExternalMasterAPITestService : IntegrationService
    {
        protected override string ApiController => "Master";

        public async Task<Master> GetMaster(string id)
        {
            Response = await RestClientService.GetAsync(ApiController + "/GetSingleMaster?id=" + id);

            Assert.IsTrue(Response.StatusCode == System.Net.HttpStatusCode.OK);

            Master responseEntity = RestClientService.ParseResponse<Master>(Response);
            return responseEntity;
        }

        public async Task<string> CreateMaster(Master entityPM, bool isAsserting = true)
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
                Master responseEntity = RestClientService.ParseResponse<Master>(Response);
                return responseEntity.Id;
            }
        }

        public async Task<Master> UpdateMaster(Master entityPM, bool isAsserting = true)
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
                Master responseEntity = RestClientService.ParseResponse<Master>(Response);
                return responseEntity;
            }
        }

    }
}
