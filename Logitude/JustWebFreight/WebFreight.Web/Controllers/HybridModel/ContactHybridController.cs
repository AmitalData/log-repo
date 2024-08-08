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

    public class ContactHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(ContactPM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            ContactPM contactPM = JsonConvert.DeserializeObject<ContactPM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            ContactWcfService ContactWcfService = new ContactWcfService();
            Response response = ContactWcfService.Upsert(contactPM, batch);
            return response;
        }


        [System.Web.Http.HttpPost]
        public ContactPM GetContactPMByEmail([FromBody] object[] t)//(string email, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string email = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ContactWcfService ContactWcfService = new ContactWcfService();
            ContactPM Response = ContactWcfService.GetContactPMByEmail(email, tenant, ref response);
            return Response;
        }


        [System.Web.Http.HttpPost]

        public ContactPM GetContactByExternalId([FromBody] object[] t)//(string externalId, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string externalId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ContactWcfService ContactWcfService = new ContactWcfService();
            ContactPM Response = ContactWcfService.GetContactByExternalId(externalId, tenant, ref response);
            return Response;
        }


        [System.Web.Http.HttpPost]

        public List<ContactList> GetContactList([FromBody] object[] t)//(DataContracts.ContactApiFilters filters, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            DataContracts.ContactApiFilters filters = JsonConvert.DeserializeObject<DataContracts.ContactApiFilters>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            ContactWcfService ContactWcfService = new ContactWcfService();
            List<ContactList> listResponse = ContactWcfService.GetContactList(filters, tenant, ref response);
            return listResponse;
        }
        

    }
}
