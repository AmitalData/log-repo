using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.CoreBL
{
   public class GLAccountChequesTransactionsRetreivingService
    {
        int tenant;
        IAccountingContext accountingContext;
        bool showLocal;
        const string paymentIconCode = "PY";
        const string journalIconCode = "JR";


        public GLAccountChequesTransactionsRetreivingService(int Tenant, IAccountingContext context)
        {
            tenant = Tenant;
            this.accountingContext = context;
            this.showLocal = GetLoggedContactShowLocal(tenant);
        }

        public List<LedgerTransactionList> GetAccountChequesTransactions(string accountId)
        {
            List<LedgerTransactionList> arPaymentTransactions = GetARPaymentLedgerTransactions(accountId).OrderByDescending(d => d.PaymentValueDate).Distinct().ToList();
            List<LedgerTransactionList> externalTransactions = GetExternalTransactionsForAccount(accountId, tenant);

            arPaymentTransactions.AddRange(externalTransactions);
            return arPaymentTransactions;
        }

        private List<LedgerTransactionList> GetARPaymentLedgerTransactions(string accountId)
        {
            return (from transaction in accountingContext.LedgerTransactions
                    join journal in accountingContext.Journals on transaction.JournalId equals journal.Id  
                    join arpaymentcheque in accountingContext.ARPaymentCheques on 
                      journal.AccountingEntityId equals   arpaymentcheque.PaymentId 
                    join arpaymentchequeStatus in accountingContext.ARPaymentChequeStatuses on arpaymentcheque.StatusCode equals arpaymentchequeStatus.Code

                    where transaction.Tenant == tenant && transaction.AccountId == accountId && journal.AccountingEntityCode == AccountingEntityValues.ARPayment && 
                    transaction.Reference2 ==arpaymentcheque.ChequeNumber

                    select new LedgerTransactionList()
                    {
                        PaymentValueDate = arpaymentcheque.ValueDate,
                        PaymentChequeStatus = showLocal ? arpaymentchequeStatus.LocalName : arpaymentchequeStatus.EnglishName,
                        Source = journal.AccountingEntityReference,
                        SourceType = journal.AccountingEntityCode,
                        SourceNumber = journal.AccountingEntityReference,
                        LocalAmountCredit = transaction.LocalAmountCredit,
                        ForeignAmountCredit = transaction.ForeignAmountCredit,
                        Reference1 = transaction.Reference1,
                        Reference2 = transaction.Reference2,
                        Reference3 = transaction.Reference3,
                        JournalNumber = journal.JournalNumber,
                        Notes = transaction.Notes,
                        SourceId = journal.AccountingEntityId,
                        SourceTypeCode = AccountingEntityValues.ARPayment,
                        JournalId = journal.Id,
                        IconCode = paymentIconCode,
                        IsForeignAmountCreditPos = transaction.ForeignAmountCredit != 0,
                        IsLocalAmountCreditPos = transaction.LocalAmountCredit != 0,
                        CalculatedForeignAmount = transaction.ForeignAmountCredit != 0 ? transaction.ForeignAmountCredit : transaction.ForeignAmountDebit,
                        CalculatedLocalAmount = transaction.LocalAmountCredit != 0 ? transaction.LocalAmountCredit : transaction.LocalAmountDebit,
                        CurrencySign =transaction.Currency.Sign,

                    }).Distinct().ToList();
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
                        PaymentValueDate = trans.DueDate,
                        Source = journal.AccountingEntityReference,
                        SourceType = journal.AccountingEntityCode,
                        SourceNumber = journal.AccountingEntityReference,
                        JournalNumber = journal.JournalNumber,
                        LocalAmountCredit = trans.LocalAmountCredit,
                        ForeignAmountCredit = trans.ForeignAmountCredit,
                        Reference1 = trans.Reference1,
                        Reference2 = trans.Reference2,
                        Reference3 = trans.Reference3,
                        Notes = trans.Notes,
                        JournalId = journal.Id,
                        IconCode = journalIconCode,
                        SourceId = journal.Id,
                        SourceTypeCode = AccountingEntityValues.Journal,
                        IsForeignAmountCreditPos = trans.ForeignAmountCredit != 0,
                        IsLocalAmountCreditPos = trans.LocalAmountCredit != 0,
                        CalculatedForeignAmount = trans.ForeignAmountCredit != 0 ? trans.ForeignAmountCredit : trans.ForeignAmountDebit,
                        CalculatedLocalAmount = trans.LocalAmountCredit != 0 ? trans.LocalAmountCredit : trans.LocalAmountDebit,
                        CurrencySign = trans.Currency.Sign,
                    }).ToList();

        }

        private static DateTime GetCurrentDate(int tenant)
        {
            DateTime today = TenantServerConfigration.GetCurrentDateTime(tenant);
            today = new DateTime(today.Year, today.Month, today.Day, 11, 59, 59);
            return today;
        }

        public static bool GetLoggedContactShowLocal(int tenant)
        {
            ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            bool showLocal = !(bool)loggedcontact?.DontShowLocal;
            return showLocal;
        }

    }
}
