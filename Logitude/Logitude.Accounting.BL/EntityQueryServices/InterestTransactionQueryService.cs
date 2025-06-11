using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.Utilities;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.Accounting.Data.Enums;
using System.Text;
using System.Threading.Tasks;
using Journal = Logitude.Accounting.Data.EntityPOCOs.Journal;
using JournalLine = Logitude.Accounting.Data.EntityPOCOs.JournalLine;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class InterestTransactionQueryService
    {
        public List<InterestTransactionPM> GetInterestTransactionsForGlAccountAndInterestValueDate(InterestTransactionGetParameters interestTransactionGetParameters)
        {
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(interestTransactionGetParameters.Tenant);
            IQueryable<InterestTransaction> interestTransactions = interestTransactionRepository.GetInterestTransactionsForGlAccountAndInterestValueDate(interestTransactionGetParameters);

            List <InterestTransactionPM> interestTransactionPMs = MapInterestTransactionsPocosToPMs(interestTransactions);
            return interestTransactionPMs;
        }
 
        public List<InterestTransactionPM> GetInterestTransactionsByInterestReportId(string interestReportId, int tenant)
        {
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(tenant);
            List<InterestTransaction> interestTransactions = (from a in context.InterestTransactions
                                                                    where a.InterestReportId == interestReportId && a.Tenant== tenant select a).ToList();
            List<InterestTransactionPM> interestTransactionPMs = interestTransactions.Select(r => this.GetEntityPM(r)).ToList();
            return interestTransactionPMs;

        }

        public  InterestTransactionPM GetInterestTransactionPMByEntityId(string entityId, int tenant)
        {
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(tenant);
            InterestTransactionPM interestTransactionPM = (from a in context.InterestTransactions
                                                              where a.EntityId == entityId && a.Tenant == tenant
                                                              select new InterestTransactionPM()
                                                              {
                                                                  Id = a.Id,
                                                                  OriginalEntityLineNumber = a.OriginalEntityLineNumber,
                                                                  EntityId = a.EntityId,
                                                                  InterestEntityTypeCode = a.InterestEntityTypeCode,
                                                                  AccountingDate = a.AccountingDate,
                                                                  JournalId = a.JournalId

                                                              }).FirstOrDefault();

            return interestTransactionPM;

        }

        public List<InterestTransactionPM> GetInterestTransactionsByPaymentId(string entityId, int tenant)
        {
          
            List<InterestTransactionPM> interestTransactionPMs = (from a in context.InterestTransactions
                                                           where a.EntityId == entityId && a.Tenant == tenant && a.InterestEntityTypeCode == InterestEntities.ARPayment
                                                                  select new InterestTransactionPM()
                                                           {
                                                               Id = a.Id,
                                                               OriginalEntityLineNumber = a.OriginalEntityLineNumber,
                                                               EntityId = a.EntityId,
                                                               InterestEntityTypeCode = a.InterestEntityTypeCode,
                                                               InterestValueDate = a.InterestValueDate,
                                                               LocalAmount = a.LocalAmount,
                                                               ForeignAmount= a.ForeignAmount,
                                                                      AccountingDate = a.AccountingDate,
                                                                      JournalId = a.JournalId

                                                                  }).ToList();

            return interestTransactionPMs;

        }

        public InterestTransactionPM GetOpenBalanceInterestTransactionsByInterestReportId(string interestReportId, int tenant)
        {
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(tenant);
            InterestTransaction interestTransaction = (from a in context.InterestReportLines
                                                       where a.Tenant == tenant && a.InterestReportId == interestReportId && a.InterestTransaction.InterestEntityTypeCode == "4"
                                                       select a.InterestTransaction).FirstOrDefault();

            InterestTransactionPM interestTransactionPM = null;
            if (interestTransaction != null)
            {
              interestTransactionPM = this.GetEntityPM(interestTransaction);
            }
            
            return interestTransactionPM;

        }
        private List<InterestTransactionPM> MapInterestTransactionsPocosToPMs(IQueryable<InterestTransaction> interestTransactions)
        {
            List<InterestTransactionPM> interestTransactionPMs = new List<InterestTransactionPM>();
            foreach(InterestTransaction interestTransaction in interestTransactions)
            {
                InterestTransactionPM interestTransactionPM = this.GetEntityPM(interestTransaction, false);
                interestTransactionPMs.Add(interestTransactionPM);
            }
            return interestTransactionPMs;
        }

        public InterestTransactionPM GetTransactionByUniqueConstraintFields(InterestTransactionUniqueConstraintFields uniqueConstraintFields)
        {
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(uniqueConstraintFields.Tenant);
            InterestTransaction interestTransactions = (from a in context.InterestTransactions
                                                              where
                                                                  a.InterestEntityTypeCode == uniqueConstraintFields.InterestEntityTypeCode
                                                                  && a.OriginalEntityLineNumber == uniqueConstraintFields.OriginalEntityLineNumber
                                                                  && a.Tenant == uniqueConstraintFields.Tenant
                                                                  && a.GLAccountId == uniqueConstraintFields.GLAccountId
                                                                  && a.EntityId == uniqueConstraintFields.EntityId
                                                              select a).FirstOrDefault();

            InterestTransactionPM interestTransactionPMs = GetEntityPM(interestTransactions);
            return interestTransactionPMs;

        }

        public List<InterestTransactionPM> GetInterestTransactionPMsByEntityTypeCodeIdAccount(string typeCode, string entityId, string gLAccountId, int tenant)
        {
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(tenant);

            var lt_list = (from a in context.InterestTransactions
                                                        where
                                                            a.InterestEntityTypeCode == typeCode
                                                            && a.Tenant == tenant
                                                            && a.GLAccountId == gLAccountId
                                                            && a.EntityId == entityId
                     select a).ToList();

            List<InterestTransactionPM> interestTransactionPMs = new List<InterestTransactionPM>();
            lt_list.ForEach(intt => interestTransactionPMs.Add(GetEntityPM(intt)));
            return interestTransactionPMs;

        }
        public List<InterestTransactionPM> GetInterestTransactionPMsByEntityTypeCodeIdAccountCurr(string typeCode, string entityId, string gLAccountId, int tenant, string currId)
        {
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(tenant);

            var lt_list = (from a in context.InterestTransactions
                     where
                         a.InterestEntityTypeCode == typeCode
                         && a.Tenant == tenant
                         && a.GLAccountId == gLAccountId
                         && a.EntityId == entityId
                         && (String.IsNullOrEmpty(currId) || a.CurrencyId == currId)
                     select a).ToList();

            List<InterestTransactionPM> interestTransactionPMs = new List<InterestTransactionPM>();
            lt_list.ForEach(intt => interestTransactionPMs.Add(GetEntityPM(intt)));
            return interestTransactionPMs;

        }

        public InterestTransaction MapInterestTransactionNotes(InterestTransactionPM interestTransactionPM)
        {
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(interestTransactionPM.Tenant);
            var interestTransaction =interestTransactionRepository.GetSingle(interestTransactionPM.Id, interestTransactionPM.Tenant);
            interestTransaction.Notes = interestTransactionPM.Notes;
            interestTransactionRepository.Update(interestTransaction);
            interestTransactionRepository.SubmitChanges();


            return interestTransaction;
        }




        private static readonly Dictionary<string, Func<InterestTransaction, int, DateTime, DateTime>> accountingDateResolvers =
            new Dictionary<string, Func<InterestTransaction, int, DateTime, DateTime>>
            {
                { AccountingEntityValues.Journal, (tx, tenant, fallback) => GetJournalAccountingDate(tx, tenant, fallback) },
                { AccountingEntityValues.Adjustment, (tx, tenant, fallback) => GetJournalAccountingDate(tx, tenant, fallback) },
                { AccountingEntityValues.BankAdjustment, (tx, tenant, fallback) => GetJournalAccountingDate(tx, tenant, fallback) },
                { AccountingEntityValues.ARInvoice, (tx, tenant, fallback) => GetARInvoiceAccountingDate(tx, tenant, fallback) },
                { AccountingEntityValues.ARPayment, (tx, tenant, fallback) => GetARPaymentAccountingDate(tx, tenant, fallback) }
            };


        public DateTime GetAccountingDate(InterestTransaction interestTransaction, int tenant)
        {
            if (interestTransaction == null)
            {
                return DateTime.MinValue;
            }
            var accountingDate = interestTransaction.InterestValueDate;
            if (accountingDateResolvers.TryGetValue(interestTransaction.AccountingEntityCode, out var resolver))
            {
                return resolver(interestTransaction, tenant, accountingDate);
            }
            return accountingDate;
        }

        private static DateTime GetJournalAccountingDate(InterestTransaction interestTransaction, int tenant, DateTime fallbackDate)
        {
            var journalLineRepository = new JournalLineRepository(tenant);
            var journalLine = journalLineRepository.GetSingleJournalLine(
                interestTransaction.EntityId,
                interestTransaction.OriginalEntityLineNumber,
                tenant
            );

            if (journalLine != null)
            {
                return journalLine.AccountingDate;
            }

            var journalRepository = new JournalRepository(tenant);
            var journal = journalRepository.GetSingle(interestTransaction.EntityId, tenant);
            return journal?.AccountingDate ?? fallbackDate;
        }


        private static DateTime GetARInvoiceAccountingDate(InterestTransaction interestTransaction, int tenant, DateTime fallbackDate)
        {
            var invoiceRepository = new ARInvoiceRepository(tenant);
            var invoice = invoiceRepository.GetSingleARInvoice(interestTransaction.EntityId, tenant);
            return invoice?.InvoiceDate ?? fallbackDate;
        }


        private static DateTime GetARPaymentAccountingDate(InterestTransaction interestTransaction, int tenant, DateTime fallbackDate)
        {
            var paymentRepository = new ARPaymentRepository(tenant);
            var payment = paymentRepository.GetSingleARPayment(interestTransaction.EntityId, tenant);
            return payment?.RegisterDate ?? fallbackDate;
        }



    }
}
