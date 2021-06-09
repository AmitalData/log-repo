using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
   public class GLAccountChequesTransactionsRetreivingService
    {
        int tenant;
        IAccountingContext accountingContext;
        public GLAccountChequesTransactionsRetreivingService(int Tenant, IAccountingContext context)
        {
            tenant = Tenant;
            this.accountingContext = context;
        }

        public List<LedgerTransactionList> GetAccountChequesTransactions(string accountId)
        {
            List<LedgerTransactionList> arPaymentTransactions = GetARPaymentLedgerTransactions(accountId);
            List<LedgerTransactionList> externalTransactions = GetExternalTransactionsForAccount(accountId, tenant);

            arPaymentTransactions.AddRange(externalTransactions);
            return arPaymentTransactions;
        }

        private List<LedgerTransactionList> GetARPaymentLedgerTransactions(string accountId)
        {
            const string AccountingEntity_ARPayment = "3";

            return (from transaction in accountingContext.LedgerTransactions
                    join journal in accountingContext.Journals on
                    transaction.JournalId equals journal.Id
                    join arpaymentcheque in accountingContext.ARPaymentCheques on journal.AccountingEntityId equals arpaymentcheque.PaymentId
                    join arpaymentchequeStatus in accountingContext.ARPaymentChequeStatuses on arpaymentcheque.StatusCode equals arpaymentchequeStatus.Code
                    where transaction.Tenant == tenant && journal.AccountingEntityCode == AccountingEntity_ARPayment && transaction.AccountId == accountId
                    select new LedgerTransactionList()
                    {
                        PaymentValueDate = arpaymentcheque.ValueDate,
                        PaymentChequeStatus = arpaymentchequeStatus.LocalName,
                        Source = journal.AccountingEntityReference,
                        LocalAmountCredit = transaction.LocalAmountCredit,
                        ForeignAmountCredit = transaction.ForeignAmountCredit,
                        Reference1 = transaction.Reference1,
                        Reference2 = transaction.Reference2,
                        Reference3 = transaction.Reference3,
                        JournalNumber = journal.JournalNumber,
                        Notes = transaction.Notes

                    }).ToList();
        }

        private List<LedgerTransactionList> GetExternalTransactionsForAccount(string accountId, int tenant)
        {
            DateTime today = GetCurrentDate(tenant);

            return (from trans in accountingContext.LedgerTransactions
                    join journal in accountingContext.Journals on trans.JournalId equals journal.Id
                    where
                        journal.ExternalSystem != null
                    && trans.AccountId == accountId
                    && trans.Tenant == tenant
                    && trans.DueDate > today
                    && trans.LocalAmountCredit != 0
                    select new LedgerTransactionList
                    {
                        JournalNumber = journal.JournalNumber,
                        LocalAmountCredit = trans.LocalAmountCredit,
                        ForeignAmountCredit = trans.ForeignAmountCredit,
                        Reference1 = trans.Reference1,
                        Reference2 = trans.Reference2,
                        Reference3 = trans.Reference3,
                        Notes = trans.Notes,

                    }).ToList();

        }

        private static DateTime GetCurrentDate(int tenant)
        {
            DateTime _today = TenantServerConfigration.GetCurrentDateTime(tenant);
            _today = new DateTime(_today.Year, _today.Month, _today.Day, 11, 59, 59);
            return _today;
        }

    }
}
