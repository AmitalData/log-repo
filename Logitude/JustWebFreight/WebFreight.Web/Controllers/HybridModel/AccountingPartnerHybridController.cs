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
    public class AccountingPartnerHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(AccountingPartnerPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            AccountingPartnerPM accountingPartnerPM = JsonConvert.DeserializeObject<AccountingPartnerPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            AccountingPartnerWcfService AccountingPartnerWcfService = new AccountingPartnerWcfService();
            Response response = AccountingPartnerWcfService.Upsert(accountingPartnerPM, batch);
            return response;
        }


        [System.Web.Http.HttpPost]
        public AccountingPartnerPM GetAccountingPartnerPM([FromBody] object[] t)//(DataContracts.AccountingPartnerApiFilters filters, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            DataContracts.AccountingPartnerApiFilters filters = JsonConvert.DeserializeObject<DataContracts.AccountingPartnerApiFilters>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            AccountingPartnerWcfService AccountingPartnerWcfService = new AccountingPartnerWcfService();
            AccountingPartnerPM Response = AccountingPartnerWcfService.GetAccountingPartnerPM(filters, tenant, ref response);
            return Response;
        }
    }
}
