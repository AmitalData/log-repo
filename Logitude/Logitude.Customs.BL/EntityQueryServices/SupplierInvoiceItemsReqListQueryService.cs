using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Customs.BL.EntityDataMappings;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SupplierInvoiceItemsReqListQueryService : EntityQueryService<SupplierInvoiceItemsReqList, SupplierInvoiceItemsReqListKeys, SupplierInvoiceItemsReqListPM, object, SupplierInvoiceItemsReqListKeys>
    {
        public SupplierInvoiceItemsReqListPM GetOrCreate(string siiRequestId, string declarationid, int linenumber, int invoicecounterkey, int invoiceitemlinenumber,int tenant)
        {
            var pm = GetSingle(declarationid, linenumber, siiRequestId, invoicecounterkey, invoiceitemlinenumber, true, false);
            if(pm != null)
            {
                return pm;
            }

            var newPo = new SupplierInvoiceItemsReqList
            {
                Tenant = tenant,
                DeclarationId = declarationid,
                LineNumber = 1,  // not clear what linenumber shud be right now so we return 1 for now -- linenumber is key 
                InvoiceCounterKey = invoicecounterkey,
                InvoiceItemLineNumber = invoiceitemlinenumber,
            };
            
            var newPm = new SupplierInvoiceItemsReqListPM();
            SupplierInvoiceItemsReqListDataMapping mapping = new SupplierInvoiceItemsReqListDataMapping();
            mapping.CustomPOCOToPM(newPm, newPo);
            mapping.POCOToPM(newPm, newPo);
            return newPm;
        }
        public SupplierInvoiceItemsReqListPM GetRequest(string siiRequestId, string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int tenant)
        {
            SupplierInvoiceItemsReqList entity = repository.GetRequest(siiRequestId, declarationid, invoicecounterkey, invoiceitemlinenumber, tenant);
            if (entity == null)
            {
                return null;
            }
            var EntityPM = GetEntityPM(entity);
            return EntityPM;
        }
    }
}
