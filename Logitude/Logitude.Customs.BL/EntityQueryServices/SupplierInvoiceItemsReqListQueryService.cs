using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Customs.BL.EntityDataMappings;

namespace Logitude.Customs.BL.EntityQueryServices
{


    public partial class SupplierInvoiceItemsReqListQueryService : EntityQueryService<SupplierInvoiceItemsReqList, SupplierInvoiceItemsReqListKeys, SupplierInvoiceItemsReqListPM, SIIRequestPM, SIIRequestKeys>
    {
        public SupplierInvoiceItemsReqListPM GetOrCreate(string siiRequestId, string declarationid, int invoicecounterkey, int invoiceitemlinenumber,int tenant)
        {
            var pm = GetSingle(declarationid, siiRequestId, invoicecounterkey, invoiceitemlinenumber, true, false);
            if(pm != null)
            {
                return pm;
            }

            var newPo = new SupplierInvoiceItemsReqList
            {
                Tenant = tenant,
                SIIRequestID = siiRequestId,
                DeclarationId = declarationid,
                LineNumber = 0, 
                InvoiceCounterKey = invoicecounterkey,
                InvoiceItemLineNumber = invoiceitemlinenumber,
            };
            
            var newPm = new SupplierInvoiceItemsReqListPM();
            SupplierInvoiceItemsReqListDataMapping mapping = new SupplierInvoiceItemsReqListDataMapping();
            mapping.CustomPOCOToPM(newPm, newPo);
            mapping.POCOToPM(newPm, newPo);
            return newPm;
        }
        public SupplierInvoiceItemsReqListPM GetRequestLine(string siiRequestId, string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int tenant)
        {
            SupplierInvoiceItemsReqList entity = repository.GetRequest(siiRequestId, declarationid, invoicecounterkey, invoiceitemlinenumber, tenant);
            if (entity == null)
            {
                return null;
            }
            var EntityPM = GetEntityPM(entity);
            return EntityPM;
        }
        public SupplierInvoiceItemsReqListPM GetLineForSiiStatusUpdate(string requestNumber,int lineNumber,string modelCode,int tenant)
        {
            var entity = repository.GetLineForSiiStatusUpdate(requestNumber, lineNumber, modelCode,tenant);

            if (entity == null)
                return null;

            return GetEntityPM(entity);
        }

    }
}
