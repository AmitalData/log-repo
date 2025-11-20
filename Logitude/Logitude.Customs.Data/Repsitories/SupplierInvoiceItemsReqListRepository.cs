 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class SupplierInvoiceItemsReqListRepository:IRepository<SupplierInvoiceItemsReqList>
   {
        
		public List<SupplierInvoiceItemsReqList> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public SupplierInvoiceItemsReqList GetRequest(string siiRequestId, string declarationid, int invoicecounterkey, int invoiceitemlinenumber, int tenant)
        {
            return context.SupplierInvoiceItemsReqLists.FirstOrDefault(a =>
            a.Tenant == tenant &&
            a.SIIRequestID == siiRequestId &&
            a.DeclarationId == declarationid &&
            a.InvoiceCounterKey == invoicecounterkey &&
            a.InvoiceItemLineNumber == invoiceitemlinenumber);
        }

        public SupplierInvoiceItemsReqList GetLineForSiiStatusUpdate(string requestNumber,int linenumber,string modelCode,int tenant)
        {
            var query =from line in context.SupplierInvoiceItemsReqLists
                       join req in context.SIIRequests
                       on line.SIIRequestID equals req.Id
                       where  req.RequestNo == requestNumber && req.Tenant == tenant && line.LineNumber == linenumber && line.Tenant == tenant
                       select line;
            if (!string.IsNullOrWhiteSpace(modelCode))
            {
                query = query.Where(l => l.ItemNo == modelCode);
            }
            return query.FirstOrDefault();
        }
    }

}
   