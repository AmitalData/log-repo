using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Simplog.Server.Infrastructure.Helpers
{

	public class APIConnectionHelper : BaseClasses.BaseInstance<APIConnectionHelper>
	{
		//example:  var respnse = APIConnectionHelper.Instance.PostViaWebAPI<Response>("api/DocumentIn/Upsert", new object[] { docPM, false });

		private  HttpClient client;
		private void InitnClient(string baseuri)
		{
			client = new HttpClient();
			client.BaseAddress = new Uri(baseuri);


		}

		public APIConnectionHelper()
		{
			string baseurl = LogitudeSettings.LogitudeURL; //"http://localhost:9996/";
			InitnClient(baseurl);
		}

		public T PostViaWebAPI<T,W>(string url, W entity)
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
		private  StringContent API_SerializeObject(object paramArray)
		{
			return new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(paramArray, Newtonsoft.Json.Formatting.None,
						   new JsonSerializerSettings
						   {
							   NullValueHandling = NullValueHandling.Ignore
						   }), UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

		}
	}
}
