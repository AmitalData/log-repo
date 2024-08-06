using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.BL.CommonDataModel.EntityLists;
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
    public class PaymentTermHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public List<PaymentTermList> GetPaymentTerms([FromBody] object[] t)//(ref Response response, int tenant)
        {

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
     
            PaymentTermWcfService PaymentTermWcfService = new PaymentTermWcfService();
            List<PaymentTermList> listResponse = PaymentTermWcfService.GetPaymentTerms(ref response, tenant);
            return listResponse;
        }
}
