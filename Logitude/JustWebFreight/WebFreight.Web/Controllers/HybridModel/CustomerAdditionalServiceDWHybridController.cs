using Logitude.BL.CommonDataModel.EntityDws;
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
    public class CustomerAdditionalServiceDWHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public List<CustomerAdditionalServiceDW> GetCustomerAdditionalServices([FromBody] object[] t)//(int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);

            CustomerAdditionalServiceDWWcfService CustomerAdditionalServiceDWWcfService = new CustomerAdditionalServiceDWWcfService();
            List<CustomerAdditionalServiceDW> listResponse = CustomerAdditionalServiceDWWcfService.GetCustomerAdditionalServices( tenant, ref response);
            return listResponse;
        }
    }
}
