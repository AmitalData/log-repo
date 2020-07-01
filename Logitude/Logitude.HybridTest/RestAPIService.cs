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
    public class RestAPIService
    {
        public T GetEntityPMById<T>(string tableName, string id, string token = null)
        {
            IntegrationTestLoginParameters.Token = token ?? EnvironmentGlobalParams.MainTenantToken;
            
            string url = tableName + "/GetSingle?id=" + id + "&tenant=" + EnvironmentGlobalParams.MainTenant;
            return Request<T>(url);
        }

        public T GetEntityByUrl<T>(string url, string token = null)
        {
            IntegrationTestLoginParameters.Token = token ?? EnvironmentGlobalParams.MainTenantToken;

            return Request<T>(url);
        }

        public T Request<T>(string url)
        {
            T entityPM = default(T);
            Task.Run(async () =>
            {
                HttpResponseMessage response = await RestClientService.GetAsync(url);
                var stringResult = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    entityPM = JsonConvert.DeserializeObject<T>(stringResult);
            }).GetAwaiter().GetResult();
            return entityPM;
        }
    }
}
