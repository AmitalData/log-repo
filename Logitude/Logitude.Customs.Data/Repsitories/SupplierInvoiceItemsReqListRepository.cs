 
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
    }

}
   