using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    //api/HybridModel
    public class ChargeTypeHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public List<ChargesTypeList> GetChargesTypes([FromBody] object[] t)//(int tenant, int skip, int take, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int skip = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int take = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);

            ChargeTypeWcfService ChargeTypeWcfService = new ChargeTypeWcfService();
            List<ChargesTypeList> listResponse = ChargeTypeWcfService.GetChargesTypes(tenant, skip, take,  ref response);
            return listResponse;

        }
    }
}