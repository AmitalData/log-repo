 
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
   public partial class SupplierInvoiceUCRRepository:IRepository<SupplierInvoiceUCR>
   {
        
		public List<SupplierInvoiceUCR> GetMulti(EntityKeyFields entityKeys)
        {

            SupplierInvoiceKeys supplierInvoiceKeys = entityKeys as SupplierInvoiceKeys;

            return (from a in context.SupplierInvoiceUCRs
                    where a.DeclarationId == supplierInvoiceKeys.DeclarationId && a.InvoiceCounterKey == supplierInvoiceKeys.InvoiceCounterKey
                    select a).ToList();
        }

   }

}
   