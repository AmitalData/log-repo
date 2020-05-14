 
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
   public partial class SupplierInvoicePaymentRepository:IRepository<SupplierInvoicePayment>
   {
        
		public List<SupplierInvoicePayment> GetMulti(EntityKeyFields entityKeys)
        {

            SupplierInvoiceKeys supplierInvoiceKeys = entityKeys as SupplierInvoiceKeys;

            return (from a in context.SupplierInvoicePayments
                    where a.DeclarationId == supplierInvoiceKeys.DeclarationId && a.InvoiceCounterKey == supplierInvoiceKeys.InvoiceCounterKey
                    select a).ToList();
        }

   }

}
   