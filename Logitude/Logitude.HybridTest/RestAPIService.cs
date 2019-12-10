using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.IntegrationTest.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Logitude.IntegrationTest.Core.Login;

namespace Logitude.HybridTest
{
    class RestAPIService
    {
        public T GetEntityPMById<T>(string tableName, string id)
        {
            IntegrationTestLoginParameters.Token = EnvironmentGlobalParams.MainToken;
            T entityPM = default(T);
            Task.Run(async () =>
            {
                HttpResponseMessage response = await RestClientService.GetAsync( tableName + "/GetSingle?id=" + id +"&tenant=" + EnvironmentGlobalParams.MainTenant);
                var stringResult = response.Content.ReadAsStringAsync().Result;
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    entityPM = JsonConvert.DeserializeObject<T>(stringResult);
            }).GetAwaiter().GetResult();
            return entityPM;
        }
    }
}
