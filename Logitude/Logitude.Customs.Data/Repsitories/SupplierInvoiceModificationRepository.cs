 
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
   public partial class SupplierInvoiceModificationRepository:IRepository<SupplierInvoiceModification>
   {
        
		public List<SupplierInvoiceModification> GetMulti(EntityKeyFields entityKeys)
        {

            SupplierInvoiceKeys supplierInvoiceItemKeys = entityKeys as SupplierInvoiceKeys;

            return (from a in context.SupplierInvoiceModifications
                    where a.DeclarationId == supplierInvoiceItemKeys.DeclarationId && a.InvoiceCounterKey == supplierInvoiceItemKeys.InvoiceCounterKey
                    select a).ToList();
        }

        public int getLastModificationKey(string declarationId, int invoiceCounterKey)
        {
            int key = 0;
            List<SupplierInvoiceModification> modifications = (from a in context.SupplierInvoiceModifications
                                                                    where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey
                                                                    select a).ToList();

            if (modifications.Count() > 0)
            {

                key = modifications.Max(d => d.ModificationCounterKey);
            }
            return key;


        }
        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceModification>(rec => rec.DeclarationId == entityKeyFields.Id);
        }

        public List<SupplierInvoiceModification> GetSupplierInvoiceModificationsForDeclaration(string declarationId)
        {
            return (from a in context.SupplierInvoiceModifications
                    where a.DeclarationId == declarationId
                    select a).ToList();
        }

        public SupplierInvoiceModification ChekIfModWithCurrencyExist( string declarationId, int counterKey, string currencyCode, int tenant)
        {
            return (from a in context.SupplierInvoiceModifications
                    where a.DeclarationId == declarationId && a.InvoiceCounterKey == counterKey
                    select a).FirstOrDefault();
        }

   }

}
   