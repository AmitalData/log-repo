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
    public class ExternalHouseAPITestService : IntegrationService
    {
        protected override string ApiController => "House";

        public async Task<House> GetHouse(string id)
        {
            Response = await RestClientService.GetAsync(ApiController + "/GetSingleHouse?id=" + id);

            Assert.IsTrue(Response.StatusCode == System.Net.HttpStatusCode.OK);

            House responseEntity = RestClientService.ParseResponse<House>(Response);
            return responseEntity;
        }

        public async Task<string> CreateHouse(House entityPM, bool isAsserting = true)
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
                House responseEntity = RestClientService.ParseResponse<House>(Response);
                return responseEntity.Id;
            }
        }

        public async Task<House> UpdateHouse(House entityPM, bool isAsserting = true)
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
                House responseEntity = RestClientService.ParseResponse<House>(Response);
                return responseEntity;
            }
        }

    }
}
