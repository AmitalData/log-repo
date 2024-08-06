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
    public class CustomerDWHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public List<CustomerDW> GetCustomers([FromBody] object[] t)//(int tenant, int skip, int take, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int skip = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int take = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);

            CustomerDWWcfService CustomerDWWcfService = new CustomerDWWcfService();
            List<CustomerDW> listResponse = CustomerDWWcfService.GetCustomers(tenant, skip, take, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public int GetCustomersCount([FromBody] object[] t)//(int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);

            CustomerDWWcfService CustomerDWWcfService = new CustomerDWWcfService();
            int Response = CustomerDWWcfService.GetCustomersCount(tenant, ref response);
            return Response;
        }


        [System.Web.Http.HttpPost]
        public List<CustomerDW> GetCustomersByUpdateDate([FromBody] object[] t)//(int tenant, DateTime updateDate, int skip, int take, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            DateTime updateDate = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int skip = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            int take = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[4]), jsonSerializerSettings);

            CustomerDWWcfService CustomerDWWcfService = new CustomerDWWcfService();
            List<CustomerDW> listResponse = CustomerDWWcfService.GetCustomersByUpdateDate(tenant, updateDate, skip, take, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public int GetCustomersCountByUpdateDate([FromBody] object[] t)//(int tenant, DateTime updateDate, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            DateTime updateDate = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            CustomerDWWcfService CustomerDWWcfService = new CustomerDWWcfService();
            int Response = CustomerDWWcfService.GetCustomersCountByUpdateDate(tenant, updateDate, ref response);
            return Response;
        }
    }
}
