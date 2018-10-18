 
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
   public partial class SupplierInvoiceFreightAmountRepository:IRepository<SupplierInvoiceFreightAmount>
   {
        
		public List<SupplierInvoiceFreightAmount> GetMulti(EntityKeyFields entityKeys)
        {

            SupplierInvoiceKeys supplierInovoiceKeys = entityKeys as SupplierInvoiceKeys;

            return (from a in context.SupplierInvoiceFreightAmounts
                    where a.DeclarationId == supplierInovoiceKeys.DeclarationId && a.InvoiceCounterKey == supplierInovoiceKeys.InvoiceCounterKey
                    select a).ToList();
        }

        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceFreightAmount>(rec => rec.DeclarationId == entityKeyFields.Id);
        }

   }

}
   