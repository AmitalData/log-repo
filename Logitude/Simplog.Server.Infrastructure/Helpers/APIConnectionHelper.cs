using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Simplog.Server.Infrastructure.Helpers
{

    public class APIConnectionHelper : BaseClasses.BaseInstance<APIConnectionHelper>
    {
        //example:  var respnse = APIConnectionHelper.Instance.PostViaWebAPI<Response>("api/DocumentIn/Upsert", new object[] { docPM, false });

        private HttpClient client;
        private void InitnClient(string baseuri)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseuri);
        }

        public APIConnectionHelper()
        {
            string baseurl = LogitudeSettings.LogitudeURL;

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
                res = JsonConvert.DeserializeObject<T>(retstr);
                return res;
            }
            return res;
        }
        private StringContent API_SerializeObject(object paramArray)
        {
            return new StringContent(JsonConvert.SerializeObject(paramArray, Formatting.None,
                           new JsonSerializerSettings
                           {
                               NullValueHandling = NullValueHandling.Ignore
                           }), UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

        }
    }
}
