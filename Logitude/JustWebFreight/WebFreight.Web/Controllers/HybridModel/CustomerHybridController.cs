using Intuit.Ipp.DataService;
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
using WebFreight.Web.Controllers.CustomsModel.Extended;
using WebFreight.Web.WcfApi;

namespace WebFreight.Web.Controllers.HybridModel
{
    public class CustomerHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(CustomerPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            CustomerPM customerPM = JsonConvert.DeserializeObject<CustomerPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            CustomerWcfService CustomerWcfService = new CustomerWcfService();
            Response response = CustomerWcfService.Upsert(customerPM, batch);
            return response;
        }


        [System.Web.Http.HttpPost]
        public List<CustomerList> GetCustomerList([FromBody] object[] t)//(string searchText, string email, bool myCustomer, int tenant, int skip, int take, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string searchText = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string email = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            bool myCustomer = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);
            int skip = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[4]), jsonSerializerSettings);
            int take = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[5]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[6]), jsonSerializerSettings);

            CustomerWcfService CustomerWcfService = new CustomerWcfService();
            List<CustomerList> listResponse = CustomerWcfService.GetCustomerList(searchText, email, myCustomer, tenant, skip, take, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public List<CustomerList> GetCustomerListByEmail([FromBody] object[] t)//(string email, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string email = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            CustomerWcfService CustomerWcfService = new CustomerWcfService();
            List<CustomerList> listResponse = CustomerWcfService.GetCustomerListByEmail(email, tenant, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public CustomerList GetCustomerListById([FromBody] object[] t)//(string id, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string id = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            CustomerWcfService CustomerWcfService = new CustomerWcfService();
            CustomerList listResponse = CustomerWcfService.GetCustomerListById(id, tenant, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public CustomerPM GetCustomerPM([FromBody] object[] t)//(DataContracts.CustomerApiFilters filters, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            DataContracts.CustomerApiFilters filters = JsonConvert.DeserializeObject<DataContracts.CustomerApiFilters>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            CustomerWcfService CustomerWcfService = new CustomerWcfService();
            CustomerPM Response = CustomerWcfService.GetCustomerPM(filters, tenant, ref response);
            return Response;
        }


        [System.Web.Http.HttpPost]
        public List<AddressPM> GetCustomerAddresses([FromBody] object[] t)//(DataContracts.CustomerApiFilters filters, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            DataContracts.CustomerApiFilters filters = JsonConvert.DeserializeObject<DataContracts.CustomerApiFilters>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            CustomerWcfService CustomerWcfService = new CustomerWcfService();
            List<AddressPM> listResponse = CustomerWcfService.GetCustomerAddresses(filters, tenant, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public List<ContactPM> GetCustomerContacts([FromBody] object[] t)//(DataContracts.CustomerApiFilters filters, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            DataContracts.CustomerApiFilters filters = JsonConvert.DeserializeObject<DataContracts.CustomerApiFilters>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            CustomerWcfService CustomerWcfService = new CustomerWcfService();
            List<ContactPM> listResponse = CustomerWcfService.GetCustomerContacts(filters, tenant, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public CustomerPM GetReadyForActivationCustomer([FromBody] object[] t)//(int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);

            CustomerWcfService CustomerWcfService = new CustomerWcfService();
            CustomerPM Response = CustomerWcfService.GetReadyForActivationCustomer(tenant, ref response);
            return Response;
        }


        [System.Web.Http.HttpPost]
        public Response RemoveFromCustomersQueue([FromBody] object[] t)//(Guid queueMessageLockToken, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            Guid queueMessageLockToken = JsonConvert.DeserializeObject<Guid>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);

            CustomerWcfService CustomerWcfService = new CustomerWcfService();
            Response Response = CustomerWcfService.RemoveFromCustomersQueue(queueMessageLockToken, tenant);
            return Response;
        }


        [System.Web.Http.HttpPost]
        public string GetActivationQuestionnaireAnswers([FromBody] object[] t)//(string QuestionnaireId, int tenant, string tableId, string entityId, string customername)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string QuestionnaireId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            string tableId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            string entityId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);
            string customername = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[4]), jsonSerializerSettings);


            CustomerWcfService CustomerWcfService = new CustomerWcfService();
            string response = CustomerWcfService.GetActivationQuestionnaireAnswers(QuestionnaireId, tenant, tableId, entityId, customername);
            return response;


        }

    }
}

