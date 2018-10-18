 
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
   public partial class SupplierInvoiceItemsLevyRepository:IRepository<SupplierInvoiceItemsLevy>
   {
        
		public List<SupplierInvoiceItemsLevy> GetMulti(EntityKeyFields entityKeys)
        {

            SupplierInvoiceItemKeys supplierInvoiceItemKeys = entityKeys as SupplierInvoiceItemKeys;

            return (from a in context.SupplierInvoiceItemsLevies
                    where a.DeclarationId == supplierInvoiceItemKeys.DeclarationId && a.InvoiceCounterKey == supplierInvoiceItemKeys.CounterKey && a.InvoiceItemLineNumber == supplierInvoiceItemKeys.LineNumber
                    select a).ToList();
        }

        public List<SupplierInvoiceItemsLevy> GetSupplierInvoiceItemsLeviesForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            //return (from a in context.SupplierInvoiceItemsLevies.Include("TradeLevyExamptType")
            //        where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey
            //        select a).ToList();

            var q = (from a in context.SupplierInvoiceItemsLevies.Include("TradeLevyExamptType")
                     where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey
                     select a);
            if (FilterLine != null)
            {
                q = q.Where(r => FilterLine.Contains(r.InvoiceItemLineNumber));
            }
            return q.ToList();
        }

        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItemsLevy>(rec => rec.DeclarationId == entityKeyFields.Id);
        }

        public List<SupplierInvoiceItemsLevy> GetSupplierInvoiceItemsLeviesForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            return (from a in context.SupplierInvoiceItemsLevies.Include("TradeLevyExamptType")
                    where a.DeclarationId == declarationId && a.InvoiceCounterKey == invoiceCounterKey && itemsLineNumbers.Contains(a.InvoiceItemLineNumber)
                    select a).ToList();
        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItemsLevy>(rec => rec.DeclarationId == entityKeyFields.DeclarationId && rec.InvoiceCounterKey == entityKeyFields.InvoiceCounterKey && supplierInvoiceItemsParentsLines.Contains(rec.InvoiceItemLineNumber));

        }
    }

}
   