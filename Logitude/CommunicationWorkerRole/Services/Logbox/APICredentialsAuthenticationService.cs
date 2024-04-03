using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole.Services.Logbox
{
    public class APICredentialsAuthenticationService
    {
        public static string Authenticate(string uRI)
        {
            APICredentialsParameters APICredentialsParam = GetAPICredentialsParameters();
            using (var client = new HttpClient())
            {
                string AuthURI = uRI + "APIAuthentication";
                var serializedObject = JsonConvert.SerializeObject(APICredentialsParam);
                string mediaType = "application/json";
                var content = new StringContent(serializedObject, Encoding.UTF8, mediaType);
                var result = client.PostAsync(AuthURI, content);
                result.Wait();
                var serializedUser = result.Result.Content.ReadAsStringAsync().Result;
                ApiCredential User = JsonConvert.DeserializeObject<ApiCredential>(serializedUser);

                return User.Token;
            }
        }

        private static APICredentialsParameters GetAPICredentialsParameters()
        {
            return new APICredentialsParameters()
            {
                PrimaryKey = "8eb9c6e4-c1ca-43e5-8061-87a7adcdc5f8",
                SecondaryKey = "c2dd0ebf-20bf-4d44-916c-7f9000dce4ec"
            };
        }
    }
}
