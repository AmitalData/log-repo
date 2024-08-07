using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Customs.Def.EntityPMs;
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
    public class CustomBankCardHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(CustomBankPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            CustomBankPM customBankPM = JsonConvert.DeserializeObject<CustomBankPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            CustomBankCardWcfService CustomBankCardWcfService = new CustomBankCardWcfService();
            Response response = CustomBankCardWcfService.Upsert(customBankPM, batch);
            return response;
        }

        [System.Web.Http.HttpPost]
        public Response Delete([FromBody] object[] t)//(string bankId, string cardId, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string bankId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string cardId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            CustomBankCardWcfService CustomBankCardWcfService = new CustomBankCardWcfService();
            Response response = CustomBankCardWcfService.Delete(bankId, cardId, tenant);
            return response;
        }
    }
}
