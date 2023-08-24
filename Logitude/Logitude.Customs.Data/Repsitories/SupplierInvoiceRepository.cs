 
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
    public partial class SupplierInvoiceRepository : IRepository<SupplierInvoice>
    {

        public List<SupplierInvoice> GetMulti(EntityKeyFields entityKeys)
        {

            DeclarationKeys declarationKeys = entityKeys as DeclarationKeys;

            return (from a in context.SupplierInvoices
                    where a.DeclarationId == declarationKeys.Id
                    select a).ToList();
        }

        public SupplierInvoice GetSupplierInvoiceByInvoiceNumber(string invoiceNumber, int tenant)
        {
            return (from a in context.SupplierInvoices
                    where a.InvoiceNumber == invoiceNumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public List<SupplierInvoice> GetSupplierInvoicesByCounterKeys(string declarationId, List<int> counterKeys, int tenant)
        {
            return (from a in context.SupplierInvoices
                    where a.DeclarationId == declarationId && a.Tenant == tenant && counterKeys.Contains(a.InvoiceCounterKey)
                    select a).ToList();
        }

        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<SupplierInvoice>(rec => rec.DeclarationId == entityKeyFields.Id);
        }

        public List<SupplierInvoice> GetSupplierInvoicesForDeclaration(string declarationId, int tenant)
        {
            var q = (from a in context.SupplierInvoices
                     where a.Tenant== tenant && a.DeclarationId == declarationId
                     orderby a.SequenceNumeric
                     select a);
            return q.ToList();
        }

        public IQueryable<SupplierInvoice> GetSupplierInvoicesQueryForDeclaration(string declarationId, int tenant)
        {
            return (from a in context.SupplierInvoices
                    where a.DeclarationId == declarationId
                    select a);
        }


        public int GetSupplierInvoiceCountForDeclaration(string declarationId, int tenant)
        {
            return (from a in context.SupplierInvoices
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).Count();
        }

        public int? GetMaxSequenceNumeric(string declarationId, int tenant)
        {
            return (from a in context.SupplierInvoices
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).Max(d => d.SequenceNumeric);
        }

        public SupplierInvoice GetSupplierInvoiceBySequenceNumeric(string declarationId, int sequenceNumeric)
        {
            return (from a in context.SupplierInvoices
                    where a.DeclarationId == declarationId && a.SequenceNumeric == sequenceNumeric
                    select a).FirstOrDefault();
        }

        public SupplierInvoice GetSupplierInvoiceByCounterKey(string declarationId, int counterKey)
        {
            return (from a in context.SupplierInvoices
                    where a.DeclarationId == declarationId && a.InvoiceCounterKey == counterKey 
                    select a).FirstOrDefault();
        }

        public bool DeclarationIsAccumulated(string declarationId)
        {
            return (from a in context.SupplierInvoices
                    where a.DeclarationId == declarationId && a.IsAccumalated
                    select a).Any();
        }

        public int GetSupplierInvoiceToAccumulateCount(int tenant, string declarationId)
        {
            return (from a in context.SupplierInvoices
                    where a.DeclarationId == declarationId && a.Tenant == tenant && a.AccumalationStateCode == "2"
                    select a).Count();
        }

        public List<int> GetDeclarationSupplierInvoicesKeysForAccumulation(int tenant, string declarationId, bool onlyAlwaysAccumulate)
        {
            if (onlyAlwaysAccumulate)
            {
                return (from a in context.SupplierInvoices
                        where a.DeclarationId == declarationId && a.Tenant == tenant && a.AccumalationStateCode == "2"
                        select a.InvoiceCounterKey).ToList();
            }
            else
            {
                return (from a in context.SupplierInvoices
                        where a.DeclarationId == declarationId && a.Tenant == tenant && a.AccumalationStateCode != "3"
                        select a.InvoiceCounterKey).ToList();
            }
        }

        public SupplierInvoice CheckIfInvoiceNumberExists(string declarationId, string invoiceNumber,int invoiceCounterKey, int tenant)
        {
            SupplierInvoice exists = (from a in context.SupplierInvoices
                           where a.DeclarationId == declarationId && a.InvoiceNumber == invoiceNumber && a.InvoiceCounterKey!=invoiceCounterKey && a.Tenant == tenant
                           select a).FirstOrDefault();
            return exists;
        }

        public SupplierInvoice GetFirstInvoice(string declarationId)
        {
            SupplierInvoice firstInvoice = (from a in context.SupplierInvoices
             where a.DeclarationId == declarationId
             select a).OrderBy(d=>d.SequenceNumeric).FirstOrDefault();
            return firstInvoice;
        }

        public bool DoesAnyInvoiceHasFreight(string declarationId, int tenant)
        {
            bool exists = (from a in context.SupplierInvoiceFreightAmounts
                           where a.DeclarationId == declarationId && a.Amount > 0
                           select a).Any();
            return exists;
        }

        public List<SupplierInvoice> GetSupplierInvoicesWithoutTotalFrieght(int tenant)
        {
                return (from a in context.SupplierInvoices
                        where a.Tenant == tenant && a.TotalFreightInNIS == null && a.TotalFreightInFreightCurrency !=null && a.TotalFreightInFreightCurrency !=0
                        select a).ToList();
        }
        public List<string> GetDeclarationIdfromInvoiceNumber(string invoicenumber,int tenant)
        {
            return (from a in context.SupplierInvoices
                    where a.Tenant == tenant && a.InvoiceNumber == invoicenumber
                    select a.DeclarationId).ToList();
        }
    }
}
   