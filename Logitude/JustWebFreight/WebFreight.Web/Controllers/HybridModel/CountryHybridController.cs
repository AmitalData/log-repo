using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class CountryHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(CountryPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            CountryPM countryPM = JsonConvert.DeserializeObject<CountryPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            CountryWcfService CountryWcfService = new CountryWcfService();
            Response response = CountryWcfService.Upsert(countryPM, batch);
            return response;
        }

        [System.Web.Http.HttpPost]
        public List<Logitude.BL.CommonDataModel.EntityLists.CountryList> GetList([FromBody] object[] t)//(ApiSearchFilters filters, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            ApiSearchFilters filters = JsonConvert.DeserializeObject<ApiSearchFilters>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            CountryWcfService CountryWcfService = new CountryWcfService();
            List<Logitude.BL.CommonDataModel.EntityLists.CountryList> listResponse = CountryWcfService.GetList(filters, tenant, ref response);
            return listResponse;
        }

    }
}
