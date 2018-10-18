 
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
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class SupplierInvoiceItemsModRepository:IRepository<SupplierInvoiceItemsMod>
   {
        
		public List<SupplierInvoiceItemsMod> GetMulti(EntityKeyFields entityKeys)
        {

            SupplierInvoiceItemKeys supplierInvoiceItemKeys = entityKeys as SupplierInvoiceItemKeys;

            return (from a in context.SupplierInvoiceItemsMods
                    where a.DeclarationId == supplierInvoiceItemKeys.DeclarationId && a.InvoiceCounterKey == supplierInvoiceItemKeys.CounterKey && a.LineNumber == supplierInvoiceItemKeys.LineNumber
                    select a).ToList();
        }

        public int getLastModificationKey( string declarationId, int invoiceCounterKey, int lineNumber)
        {
            int key = 0;

            List<SupplierInvoiceItemsMod> modifications = (from a in context.SupplierInvoiceItemsMods
                                                                    where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey && a.LineNumber == lineNumber
                                                                    select a).ToList();

            if (modifications.Count() > 0)
            {

                key = modifications.Max(d => d.ModificationCounterKey);
            }
            return key;


        }


        public List<SupplierInvoiceItemsMod> GetSupplierInvoiceItemsModsForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            //return (from a in context.SupplierInvoiceItemsMods.Include("ModificationAndDiscountType").Include("CurrencyType")
            //        where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey
            //        select a).ToList();

            var q = (from a in context.SupplierInvoiceItemsMods.Include("ModificationAndDiscountType").Include("CurrencyType")
                     where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey
                     select a);
            if (FilterLine != null)
            {
                q = q.Where(r => FilterLine.Contains(r.LineNumber));
            }
            return q.ToList();
        }

        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItemsMod>(rec => rec.DeclarationId == entityKeyFields.Id);
        }

        public List<SupplierInvoiceItemsMod> GetSupplierInvoiceItemsModsForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey,List<int> itemsLineNumbers, int tenant)
        {
            return (from a in context.SupplierInvoiceItemsMods.Include("ModificationAndDiscountType").Include("CurrencyType")
                    where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey&&itemsLineNumbers.Contains(a.LineNumber)
                    select a).ToList();
        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItemsMod>(rec => rec.DeclarationId == entityKeyFields.DeclarationId && rec.InvoiceCounterKey == entityKeyFields.InvoiceCounterKey && supplierInvoiceItemsParentsLines.Contains(rec.LineNumber));

        }
    }

}
   