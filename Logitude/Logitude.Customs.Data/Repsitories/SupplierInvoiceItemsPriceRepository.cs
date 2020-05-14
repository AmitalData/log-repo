 
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
using System.Data.Entity;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class SupplierInvoiceItemsPriceRepository:IRepository<SupplierInvoiceItemsPrice>
   {
        
		public List<SupplierInvoiceItemsPrice> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<SupplierInvoiceItemsPrice> GetSupplierInvoiceItemsPricesForDeclarationId(string declarationId, int invoiceCounterKey,  List<int> itemsLineNumbers, int tenant)
        {
            return (from a in context.SupplierInvoiceItemsPrices 
                   where a.DeclarationId == declarationId && a.Tenant == tenant && a.InvoiceCounterKey== invoiceCounterKey && itemsLineNumbers.Contains(a.InvoiceItemLineNumber)
                   select a).ToList();
        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItemsPrice>(rec => rec.DeclarationId == entityKeyFields.DeclarationId && rec.InvoiceCounterKey == entityKeyFields.InvoiceCounterKey && supplierInvoiceItemsParentsLines.Contains(rec.InvoiceItemLineNumber));

        }

        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItemsPrice>(rec => rec.DeclarationId == entityKeyFields.Id);
        }
    }

}
   