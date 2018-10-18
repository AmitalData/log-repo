using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using WebFreight.Web.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IQuoteWcfService" in both code and config file together.
    [ServiceContract]
    public interface IQuoteWcfService
    {
        [OperationContract]
        Response Upsert(QuotePM entityPM, bool batch);

        [OperationContract]
        Response UploadQuotationDocument(string quoteNumber, byte[] fileData, string fileExtension, string userId, int tenant);

        [OperationContract]
        List<QuoteList> GetQuoteList(QuoteApiFilters filters, int tenant, ref Response response);


        [OperationContract]
        Response CreateEvent(int tenant, string externalId, string quoteNumber, string userId, string eventTypeCode, DateTime logDate, DateTime eventDate, string notes);

        [OperationContract]
        Response BuildEventsList(int tenant, string quoteNumber, List<TraceEventPM> eventsList);

        [OperationContract]
        Response DeleteQuoteEvent(string quoteNumber, string traceEventId, int tenant);
    }
}
