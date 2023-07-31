 
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
using Logitude.Customs.Data.EntityMapping;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class SupplierInvoiceItemRepository:IRepository<SupplierInvoiceItem>
   {

        public List<SupplierInvoiceItem> GetMulti(EntityKeyFields entityKeys)
        {


            SupplierInvoiceKeys supplierInvoiceKeys = entityKeys as SupplierInvoiceKeys;

            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == supplierInvoiceKeys.DeclarationId && a.CounterKey == supplierInvoiceKeys.InvoiceCounterKey
                    select a).ToList();


        }
        public List<SupplierInvoiceItem> GetMulti(EntityKeyFields entityKeys, bool onlyParent )
        {

            SupplierInvoiceKeys supplierInvoiceKeys = entityKeys as SupplierInvoiceKeys;

            //return (from a in context.SupplierInvoiceItems
            //        where a.DeclarationId == supplierInvoiceKeys.DeclarationId && a.CounterKey == supplierInvoiceKeys.InvoiceCounterKey 
            //        select a).ToList();

            var q = (from a in context.SupplierInvoiceItems
                     where a.DeclarationId == supplierInvoiceKeys.DeclarationId && a.CounterKey == supplierInvoiceKeys.InvoiceCounterKey
                     select a);
            if (onlyParent)
            {
                q = q.Where(r => r.IsParent == true);
            }
            return q.ToList();
        }
        public List<SupplierInvoiceItem> GetSupplierInvoiceItemsByCounterKeys(string declarationId, List<int> counterKeys,List<int> lineNumbers, int tenant)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.Tenant == tenant && counterKeys.Contains(a.CounterKey) && lineNumbers.Contains(a.LineNumber)
                    select a).ToList();
        }

        public List<SupplierInvoiceItem> GetSupplierInvoiceItemsByCounterKeysAndLineNumbers(string declarationId, List<string> counterKeysLineNumbers, int tenant)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.Tenant == tenant && counterKeysLineNumbers.Contains(a.CounterKey + " " + a.LineNumber)
                    select a).ToList();
        }

        public List<SupplierInvoiceItem> GetSupplierInvoiceItemsByParent(string declarationId, int counterKey, int lineNumber, int tenant)
        {
            return (from a in context.SupplierInvoiceItems.Include("OriginCountry").Include("TradeAgreement").Include("AdditionalMeasurmentUnit").Include("InvoiceMeasurmentUnit").Include("StatisticMeasurmentUnit")
                    where a.DeclarationId == declarationId && a.Tenant == tenant && a.CounterKey == counterKey && a.ParentLineNumber == lineNumber && !a.IsParent
                    select a).ToList();
        }

        public int GetDeclarationCountOfSupplierInvoiceItems(int tenant, string declarationId, bool? SuppressIsParent = null)
        {

            if (SuppressIsParent.GetValueOrDefault())
            {
                return (from a in context.SupplierInvoiceItems
                        where a.DeclarationId == declarationId && a.Tenant == tenant //&& a.IsParent != true
                        select a).Count();
            }

            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.Tenant == tenant && a.IsParent != true
                    select a).Count();

        }

        public int ExistSupplierInvoiceItemsWithParent(int tenant, string declarationId)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.Tenant == tenant && a.IsParent == true

                    select a).Count();

        }
        
        public int ExistSupplierInvoiceItemsWithoutHash(int tenant, string declarationId)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.Tenant == tenant && a.ItemHash == null && a.IsParent != true
                    select a).Count();

        }

        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItem>(rec => rec.DeclarationId == entityKeyFields.DeclarationId && rec.CounterKey == entityKeyFields.InvoiceCounterKey && supplierInvoiceItemsParentsLines.Contains(rec.LineNumber));
        }

        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<SupplierInvoiceItem>(rec => rec.DeclarationId == entityKeyFields.Id);
        }

      
        public void GetWeeklyStatistic(int tenant, out int totDeclarationAbove10Items, out int totDeclarationAbove500Items)
        {
            totDeclarationAbove10Items = totDeclarationAbove500Items = -1;
            var g = context.SupplierInvoiceItems.GroupBy(r => r.DeclarationId);
            var gHavingMoreThen10 = g.Where(grp => grp.Count() > 10);
            //var cHavingMoreThen10 = gHavingMoreThen10.Count();
            var gHavingMoreThen500 = g.Where(grp => grp.Count() > 500);
            //var cHavingMoreThen500 = gHavingMoreThen500.Count();

            var q = (from a in g
                         //context.CustomsSettings /// Insteat Dual - Combine 2 qouries 
                         ///where (1==0)
                     select new
                     {
                         dummy = g.FirstOrDefault(),
                         q10 = gHavingMoreThen10.Count(),
                         q500 = gHavingMoreThen500.Count()
                     }
                    );
            var row111 = q.FirstOrDefault();
            if (row111 != null)
            {
                totDeclarationAbove10Items = row111.q10;
                totDeclarationAbove500Items = row111.q500;
            }
            return;



            var q10 =
                g.Where(grp => grp.Count() > 10)
                //.Select ( grp=> new { q10= grp.Count() ,g500=0 })  ;
                .Select(grp => new { q10 = grp.ToList().Count(), g500 = 0 });

            q10 =
            (from a in gHavingMoreThen10
             group a by a.Key into groupHavingMoreThen10
             select new { q10 = groupHavingMoreThen10.Count(), g500 = 0 }
             );
            var q500 =
                g.Where(grp => grp.Count() > 500)
            //.Select(grp => new { q10 = 0, g500 = grp.Count() });
            .Select(grp => new { q10 = 0, g500 = grp.ToList().Count() });

            q500 = (from a in g.Where(grp => grp.Count() > 500) 
                    group a by a.Key into groupHavingMoreThen500
                    select new { q10 = 0, g500 = groupHavingMoreThen500.Count() }
                    );
            var list1 = q10.Concat(q500).ToList();
            if (list1.Count > 0)
            {
                totDeclarationAbove10Items = list1.Max(r => r.q10);
                totDeclarationAbove500Items = list1.Max(r => r.g500);
            }

        }

        public List<SupplierInvoiceItem> GetSupplierInvoiceItemsByInvoice(string declarationId, int invoiceCounterKey)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.CounterKey == invoiceCounterKey
                    select a).ToList();
        }

        public int GetSupplierInvoiceItemsCountByInvoice(string declarationId, int invoiceCounterKey)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.CounterKey == invoiceCounterKey && !a.IsParent
                    select a).Count();
        }

        public List<SupplierInvoiceItem> GetSomeSupplierInvoiceItemsForInvoice(bool IsAccumulated, string declarationId, int invoiceCounterKey,int skip,int take, string type)
        {
            if (IsAccumulated && type == "parent")
            {
                return (from a in context.SupplierInvoiceItems
                        where a.DeclarationId == declarationId && a.CounterKey == invoiceCounterKey && a.IsParent
                        select a).OrderBy(d => d.SequenceNumeric).Skip(skip).Take(take).ToList();
            }
            else if (IsAccumulated && type == "child")
            {
                return (from a in context.SupplierInvoiceItems
                        where a.DeclarationId == declarationId && a.CounterKey == invoiceCounterKey && !a.IsParent
                        select a).OrderBy(d => d.SequenceNumeric).Skip(skip).Take(take).ToList();
            }
            else
            {
                return (from a in context.SupplierInvoiceItems
                        where a.DeclarationId == declarationId && a.CounterKey == invoiceCounterKey
                        select a).OrderBy(d => d.SequenceNumeric).Skip(skip).Take(take).ToList();
            }
        }

        public int GetSupplierInvoiceItemsCountForInvoice(string declarationId, int invoiceCounterKey)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.CounterKey == invoiceCounterKey
                    select a).OrderBy(d => d.SequenceNumeric).Count();
        }

        public int GetChildrenCountForInvoice(string declarationId, int invoiceCounterKey)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.CounterKey == invoiceCounterKey && !a.IsParent
                    select a).Count();
        }

        public int GetMaxLineNumber(string declarationId, int counterKey)
        {
            int maxLine = (from a in context.SupplierInvoiceItems
                           where a.DeclarationId == declarationId && a.CounterKey == counterKey
                           select a).Max(d => (int?)d.LineNumber) ?? 0;
            return maxLine;
        }


        public List<SupplierInvoiceItem> GetSupplierInvoiceItemsForDeclaration(string declarationId, int tenant)
        {
            return (from a in context.SupplierInvoiceItems.Include("OriginCountry").Include("TradeAgreement").Include("AdditionalMeasurmentUnit").Include("InvoiceMeasurmentUnit").Include("StatisticMeasurmentUnit")
                    where a.DeclarationId == declarationId
                    select a).ToList();
        }

        public List<SupplierInvoiceItem> GetSupplierInvoiceItemByClassificationCode(string declarationId, int tenant,string classificationCode)
        {
            return (from a in context.SupplierInvoiceItems.Include("OriginCountry").Include("TradeAgreement").Include("AdditionalMeasurmentUnit").Include("InvoiceMeasurmentUnit").Include("StatisticMeasurmentUnit")
                    where a.DeclarationId == declarationId &&  a.ClassificationCode ==classificationCode
                    select a).ToList();
        }

        public int GetSupplierInvoiceItemsCountForDeclaration(string declarationId, int tenant)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId
                    select a).Count();
        }

        public IQueryable<SupplierInvoiceItem> GetSupplierInvoiceItemsQueryForDeclaration(string declarationId, int tenant)
        {
            return (from a in context.SupplierInvoiceItems.Include("OriginCountry").Include("TradeAgreement")
                    where a.DeclarationId == declarationId
                    select a);
        }

     
        public decimal? GetTotalForeignCurrencyForInvoice(string declarationId, int counterKey, int tenant)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.CounterKey == counterKey && !a.IsParent
                    select a).Sum(d => d.ItemPrice);
        }

        public SupplierInvoiceItem GetSupplierInvoiceItemBySequenceNumeric(string declarationId,int counterKey, int sequenceNumeric)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.SequenceNumeric==sequenceNumeric && a.CounterKey == counterKey
                    select a).FirstOrDefault();
        }

        public SupplierInvoiceItem GetParentSupplierInvoiceItemBySequenceNumeric(string declarationId, int counterKey, int sequenceNumeric)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.SequenceNumeric == sequenceNumeric && a.CounterKey == counterKey && a.IsParent
                    select a).FirstOrDefault();
        }

        public int GetDeclarationCountOfSupplierInvoiceItemsForAccumulation(int tenant, string declarationId, List<int> invoicesCounterKeys)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && invoicesCounterKeys.Contains(a.CounterKey) && a.Tenant == tenant && a.IsParent != true
                    select a).Count();
        }

        public int ExistSupplierInvoiceItemsWithoutHashForAccumulation(int tenant, string declarationId, List<int> invoicesCounterKeys)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && invoicesCounterKeys.Contains(a.CounterKey) && a.Tenant == tenant && a.ItemHash == null && a.IsParent != true
                    select a).Count();
        }
        public List<SupplierInvoiceItem> GetSupplierInvoiceItemByInvoiceNumber(int tenant,string declarationId,string ItemCode)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.Tenant==tenant && a.ItemCode== ItemCode
                    select a).ToList();
        }

        public List<SupplierInvoiceItem> GetPreferenceDocumentNumberSupplierInvoiceItemByDeclarationId( string declarationId, int tenant)
        {
            return (from a in context.SupplierInvoiceItems 
                    where a.DeclarationId == declarationId && a.Tenant == tenant 
                    select a).ToList();
        }
    }

}
   