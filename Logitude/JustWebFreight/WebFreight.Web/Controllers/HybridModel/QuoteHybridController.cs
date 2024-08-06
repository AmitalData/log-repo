using Logitude.BL.CommonDataModel.EntityDws;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;
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
    public class QuoteHybridController : ApiController
    {
        [System.Web.Http.HttpPost]
        public Response Upsert([FromBody] object[] t)//(QuotePM entityPM, bool batch)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            QuotePM quotePM = JsonConvert.DeserializeObject<QuotePM>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            bool batch = JsonConvert.DeserializeObject<bool>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);


            QuoteWcfService QuoteWcfService = new QuoteWcfService();
            Response response = QuoteWcfService.Upsert(quotePM, batch);
            return response;
        }

        [System.Web.Http.HttpPost]
        public Response UploadQuotationDocument([FromBody] object[] t)//(string quoteNumber, byte[] fileData, string fileExtension, string userId, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;


            string quoteNumber = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            byte[] fileData = JsonConvert.DeserializeObject<byte[]>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            string fileExtension = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            string userId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[4]), jsonSerializerSettings);


            QuoteWcfService QuoteWcfService = new QuoteWcfService();
            Response response = QuoteWcfService.UploadQuotationDocument(quoteNumber, fileData, fileExtension, userId, tenant);
            return response;
        }


        [System.Web.Http.HttpPost]
        public List<QuoteList> GetQuoteList([FromBody] object[] t)//(DataContracts.QuoteApiFilters filters, int tenant, ref Response response)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            DataContracts.QuoteApiFilters filters = JsonConvert.DeserializeObject<DataContracts.QuoteApiFilters>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            Response response = JsonConvert.DeserializeObject<Response>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            QuoteWcfService QuoteWcfService = new QuoteWcfService();
            List<QuoteList> listResponse = QuoteWcfService.GetQuoteList(filters, tenant, ref response);
            return listResponse;
        }


        [System.Web.Http.HttpPost]
        public Response CreateEvent([FromBody] object[] t)//(int tenant, string externalId, string quoteNumber, string userId, string eventTypeCode, DateTime logDate, DateTime eventDate, string notes)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string externalId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            string quoteNumber = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
            string userId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[3]), jsonSerializerSettings);
            string eventTypeCode = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[4]), jsonSerializerSettings);
            DateTime logDate = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(t[5]), jsonSerializerSettings);
            DateTime eventDate = JsonConvert.DeserializeObject<DateTime>(JsonConvert.SerializeObject(t[6]), jsonSerializerSettings);
            string notes = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[7]), jsonSerializerSettings);


            QuoteWcfService QuoteWcfService = new QuoteWcfService();
            Response response = QuoteWcfService.CreateEvent(tenant, externalId,quoteNumber, userId, eventTypeCode, logDate, eventDate, notes);
            return response;
        }

        [System.Web.Http.HttpPost]
        public Response BuildEventsList([FromBody] object[] t)//(int tenant, string quoteNumber, List<TraceEventPM> eventsList)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string quoteNumber = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            List<TraceEventPM> eventsList = JsonConvert.DeserializeObject<List<TraceEventPM>>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);
       
            QuoteWcfService QuoteWcfService = new QuoteWcfService();
            Response response = QuoteWcfService.BuildEventsList(tenant,quoteNumber, eventsList);
            return response;
        }

        [System.Web.Http.HttpPost]
        public Response DeleteQuoteEvent([FromBody] object[] t)//(string quoteNumber, string traceEventId, int tenant)
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;

            string quoteNumber = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[0]), jsonSerializerSettings);
            string traceEventId = JsonConvert.DeserializeObject<string>(JsonConvert.SerializeObject(t[1]), jsonSerializerSettings);
            int tenant = JsonConvert.DeserializeObject<int>(JsonConvert.SerializeObject(t[2]), jsonSerializerSettings);

            QuoteWcfService QuoteWcfService = new QuoteWcfService();
            Response response = QuoteWcfService.DeleteQuoteEvent(quoteNumber, traceEventId, tenant);
            return response;
        }
    }

    
}
