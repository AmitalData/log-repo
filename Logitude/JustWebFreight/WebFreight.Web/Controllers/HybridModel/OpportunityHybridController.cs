using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.CRM.Data.EntityLists;
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
    public class OpportunityHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public List<Logitude.CRM.Data.EntityLists.OpportunityList> GetOpportunityList([FromBody] object[] t)//(string email, string searchText, int tenant, int skip, int take, DataContracts.OpportunityApiFilters filters, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string email = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string searchText = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            int skip = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);
            int take = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[4]), jsonSerializerSettings);
            DataContracts.OpportunityApiFilters filters = JsonConvert.DeserializeObject<DataContracts.OpportunityApiFilters>(JsonConvert.SerializeObject(t[5]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[6]), jsonSerializerSettings);

            OpportunityWcfService OpportunityWcfService = new OpportunityWcfService();
            List<Logitude.CRM.Data.EntityLists.OpportunityList> listResponse = OpportunityWcfService.GetOpportunityList(email, searchText, tenant, skip, take, filters, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public CustomerList GetCustomerListByOpportunityId([FromBody] object[] t)//(string opportunityId, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string opportunityId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            OpportunityWcfService OpportunityWcfService = new OpportunityWcfService();
            CustomerList Response = OpportunityWcfService.GetCustomerListByOpportunityId(opportunityId, tenant, ref response);
            return Response;
        }


        [System.Web.Http.HttpPost]
        public OpportunityList GetOpportunityListById([FromBody] object[] t)//(string id, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string id = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            OpportunityWcfService OpportunityWcfService = new OpportunityWcfService();
            OpportunityList Response = OpportunityWcfService.GetOpportunityListById(id, tenant, ref response);
            return Response;
        }
    }
}
