 
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
   public partial class SupplierInvoiceItemsTaxRepository:IRepository<SupplierInvoiceItemsTax>
   {
        
		public List<SupplierInvoiceItemsTax> GetMulti(EntityKeyFields entityKeys)
        {

            SupplierInvoiceItemKeys supplierInvoiceItemKeys = entityKeys as SupplierInvoiceItemKeys;

            return (from a in context.SupplierInvoiceItemsTaxes
                    where a.DeclarationId == supplierInvoiceItemKeys.DeclarationId && a.InvoiceCounterKey == supplierInvoiceItemKeys.CounterKey && a.LineNumber == supplierInvoiceItemKeys.LineNumber
                    select a).ToList();
        }

        public IQueryable<SupplierInvoiceItemsTax> GetSupplierInvoiceItemTaxesForDeclarationId(string declarationId, int tenant)
        {
            return from a in context.SupplierInvoiceItemsTaxes.Include("ParagraphType")
                   where a.DeclarationId == declarationId && a.Tenant == tenant
                   select a;
        }

        public IQueryable<SupplierInvoiceItemsTax> GetSupplierInvoiceItemTaxesForInvoiceItem(string declarationId, int invoiceCounterKey, int LineNumber, int tenant)
        {
            return from a in context.SupplierInvoiceItemsTaxes.Include("ParagraphType")
                   where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey && a.LineNumber == LineNumber //&& a.Tenant == tenant
                   select a;
        }


        public List<SupplierInvoiceItemsTax> GetSupplierInvoiceItemsTaxesForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            //return (from a in context.SupplierInvoiceItemsTaxes.Include("TradeAgreement").Include("ParagraphType").Include("MeasurmentUnit")
            //        where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey
            //        select a).ToList();

            var q = (from a in context.SupplierInvoiceItemsTaxes.Include("TradeAgreement").Include("ParagraphType").Include("MeasurmentUnit")
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
                .DeleteWhere<SupplierInvoiceItemsTax>(rec => rec.DeclarationId == entityKeyFields.Id);
        }

        public List<SupplierInvoiceItemsTax> GetSupplierInvoiceItemsTaxesForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers,int tenant)
        {
            return (from a in context.SupplierInvoiceItemsTaxes.Include("TradeAgreement").Include("ParagraphType").Include("MeasurmentUnit")
                    where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey&& itemsLineNumbers.Contains(a.LineNumber)
                    select a).ToList();
        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItemsTax>(rec => rec.DeclarationId == entityKeyFields.DeclarationId && rec.InvoiceCounterKey == entityKeyFields.InvoiceCounterKey && supplierInvoiceItemsParentsLines.Contains(rec.LineNumber));

        }
    }

}
   