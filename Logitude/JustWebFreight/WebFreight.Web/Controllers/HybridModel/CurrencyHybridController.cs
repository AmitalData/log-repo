using Logitude.BL.CommonDataModel.EntityLists;
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
    public class CurrencyHybridController : ApiController
    {
        [HttpPost]
        public List<CurrencyList> GetList([FromBody] object[] t)//(ApiSearchFilters filters, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            ApiSearchFilters filters = JsonConvert.DeserializeObject<ApiSearchFilters>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            CurrencyWcfService CurrencyWcfService = new CurrencyWcfService();
            List<CurrencyList> listResponse = CurrencyWcfService.GetList(filters, tenant, ref response);
            return listResponse;
        }

        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(CurrencyPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            CurrencyPM currencyPM = JsonConvert.DeserializeObject<CurrencyPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            CurrencyWcfService CurrencyWcfService = new CurrencyWcfService();
            Response response = CurrencyWcfService.Upsert(currencyPM, batch);
            return response;
        }
    }
}
