using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace AmitalCloud.Infrastructure.APITools.Helpers
{

    public class APIConnectionHelper : BaseInstance<APIConnectionHelper>
    {
        private HttpClient client;
        private void InitnClient(string baseuri)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseuri);


        }

        public APIConnectionHelper()
        {
            string baseurl = AmitalCloudSettings.AmitalURL;

#if DEBUG
            baseurl = "http://localhost:9996/";
#endif

            InitnClient(baseurl);
        }

        public T PostViaWebAPI<T, W>(string url, W entity)
        {
            T res = default(T);
            StringContent content = API_SerializeObject(entity);
            var response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                string retstr = response.Content.ReadAsStringAsync().Result;
                res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(retstr);
                return res;
            }
            return res;
        }
        private StringContent API_SerializeObject(object paramArray)
        {
            return new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(paramArray, Newtonsoft.Json.Formatting.None,
                           new JsonSerializerSettings
                           {
                               NullValueHandling = NullValueHandling.Ignore
                           }), UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

        }
    }
}
