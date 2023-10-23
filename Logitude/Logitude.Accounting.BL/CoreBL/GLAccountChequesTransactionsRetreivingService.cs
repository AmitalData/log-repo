using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
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
        bool _IsFutureOpenCheques = false;
        bool _IsUnpaidChecks = false;

        public GLAccountChequesTransactionsRetreivingService(int Tenant, IAccountingContext context, bool isFutureOpenCheques = false, bool isUnpaidChecks = false)
        {
            tenant = Tenant;
            this.accountingContext = context;
            this.showLocal = GetLoggedContactShowLocal(tenant);
            _IsFutureOpenCheques = isFutureOpenCheques;
            _IsUnpaidChecks = isUnpaidChecks;
        }

        public List<LedgerTransactionList> GetAccountChequesTransactions(string accountId, string sortBy, string sortDirection,string cardId = "")
        {
            GLAccountMoreDataRepository gLAccountMoreDataRepository = new GLAccountMoreDataRepository(tenant);

            List<LedgerTransactionList> arPaymentTransactions = gLAccountMoreDataRepository.GetAllChecks(cardId, tenant, isFuture: _IsFutureOpenCheques,showLocal: showLocal);
            /*GetARPaymentLedgerTransactions(accountId).Distinct().ToList();*/
            //List<LedgerTransactionList> externalTransactions = GetExternalTransactionsForAccount(accountId, tenant);

            //arPaymentTransactions.AddRange(externalTransactions);
            if (sortDirection == "Descending")
            {
                arPaymentTransactions = arPaymentTransactions.OrderByDescending(a => a.GetType().GetProperty(sortBy).GetValue(a, null)).ToList();
            }
            else if (!string.IsNullOrWhiteSpace(sortBy))
            {
                arPaymentTransactions = arPaymentTransactions.OrderBy(a => a.GetType().GetProperty(sortBy).GetValue(a, null)).ToList();
            }
            else {
                arPaymentTransactions = arPaymentTransactions.OrderByDescending(d => d.PaymentValueDate).ToList();
            }
            
            //a.GetType().GetProperty(columnName).GetValue(a, null)
            return arPaymentTransactions;
        }

        private List<LedgerTransactionList> GetARPaymentLedgerTransactions(string accountId)
        {
            DateTime today = GetCurrentDate(tenant);
            var query = (from transaction in accountingContext.LedgerTransactions
                    join journal in accountingContext.Journals on transaction.JournalId equals journal.Id  
                    join arpaymentcheque in accountingContext.ARPaymentCheques on 
                      journal.AccountingEntityId equals   arpaymentcheque.PaymentId 
                    join arpaymentchequeStatus in accountingContext.ARPaymentChequeStatuses on arpaymentcheque.StatusCode equals arpaymentchequeStatus.Code

                    where transaction.Tenant == tenant && transaction.AccountId == accountId && journal.AccountingEntityCode == AccountingEntityValues.ARPayment && 
                    transaction.Reference2 ==arpaymentcheque.ChequeNumber && arpaymentcheque.StatusCode != ARPaymentChequeStatuses.ReturnToCustomer 
                    && arpaymentcheque.StatusCode != ARPaymentChequeStatuses.Redeemed

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
                        InternalNote= transaction.InternalNote,
                        UpdateDateTime= transaction.UpdateDateTime,
                        UpdatedByUserName= transaction.UpdatedByUserName,
                        SourceId = journal.AccountingEntityId,
                        SourceTypeCode = AccountingEntityValues.ARPayment,
                        JournalId = journal.Id,
                        IconCode = paymentIconCode,
                        IsForeignAmountCreditPos = transaction.ForeignAmountCredit != 0,
                        IsLocalAmountCreditPos = transaction.LocalAmountCredit != 0,
                        CalculatedForeignAmount = transaction.ForeignAmountCredit != 0 ? transaction.ForeignAmountCredit : transaction.ForeignAmountDebit,
                        CalculatedLocalAmount = transaction.LocalAmountCredit != 0 ? transaction.LocalAmountCredit : transaction.LocalAmountDebit,
                        CurrencySign =transaction.Currency.Sign,
                        Tenant = transaction.Tenant
                    });

            if (_IsFutureOpenCheques) {
                query = query.Where(x => x.PaymentValueDate > today);
            }
            if (_IsUnpaidChecks)
            {
                query = query.Where(x => x.PaymentValueDate <= today);
            }

            return query.Distinct().ToList();
        }

        private List<LedgerTransactionList> GetExternalTransactionsForAccount(string accountId, int tenant)
        {
            DateTime today = GetCurrentDate(tenant);

            var query = (from trans in accountingContext.LedgerTransactions
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
                        InternalNote = trans.InternalNote,
                        UpdateDateTime = trans.UpdateDateTime,
                        UpdatedByUserName = trans.UpdatedByUserName,
                        JournalId = journal.Id,
                        IconCode = journalIconCode,
                        SourceId = journal.Id,
                        SourceTypeCode = AccountingEntityValues.Journal,
                        IsForeignAmountCreditPos = trans.ForeignAmountCredit != 0,
                        IsLocalAmountCreditPos = trans.LocalAmountCredit != 0,
                        CalculatedForeignAmount = trans.ForeignAmountCredit != 0 ? trans.ForeignAmountCredit : trans.ForeignAmountDebit,
                        CalculatedLocalAmount = trans.LocalAmountCredit != 0 ? trans.LocalAmountCredit : trans.LocalAmountDebit,
                        CurrencySign = trans.Currency.Sign,
                    });
            if (_IsFutureOpenCheques)
            {
                query = query.Where(x => x.PaymentValueDate > today);
            }

            return query.ToList();
        }
        public int GetAccountChequesTransactionsCount(string accountId)
        {
            DateTime today = GetCurrentDate(tenant);
            var query1 = (from transaction in accountingContext.LedgerTransactions
                          join journal in accountingContext.Journals on transaction.JournalId equals journal.Id
                          join arpaymentcheque in accountingContext.ARPaymentCheques on
                            journal.AccountingEntityId equals arpaymentcheque.PaymentId
                          join arpaymentchequeStatus in accountingContext.ARPaymentChequeStatuses on arpaymentcheque.StatusCode equals arpaymentchequeStatus.Code

                          where transaction.Tenant == tenant && transaction.AccountId == accountId && journal.AccountingEntityCode == AccountingEntityValues.ARPayment &&
                          transaction.Reference2 == arpaymentcheque.ChequeNumber && arpaymentcheque.StatusCode != ARPaymentChequeStatuses.ReturnToCustomer
                          && arpaymentcheque.StatusCode != ARPaymentChequeStatuses.Redeemed

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
                              InternalNote = transaction.InternalNote,
                              UpdateDateTime = transaction.UpdateDateTime,
                              UpdatedByUserName = transaction.UpdatedByUserName,
                              SourceId = journal.AccountingEntityId,
                              SourceTypeCode = AccountingEntityValues.ARPayment,
                              JournalId = journal.Id,
                              IconCode = paymentIconCode,
                              IsForeignAmountCreditPos = transaction.ForeignAmountCredit != 0,
                              IsLocalAmountCreditPos = transaction.LocalAmountCredit != 0,
                              CalculatedForeignAmount = transaction.ForeignAmountCredit != 0 ? transaction.ForeignAmountCredit : transaction.ForeignAmountDebit,
                              CalculatedLocalAmount = transaction.LocalAmountCredit != 0 ? transaction.LocalAmountCredit : transaction.LocalAmountDebit,
                              CurrencySign = transaction.Currency.Sign,
                              Tenant = transaction.Tenant
                          }).Distinct();
                                              

            var query2 = (from trans in accountingContext.LedgerTransactions
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
                                                 InternalNote = trans.InternalNote,
                                                 UpdateDateTime = trans.UpdateDateTime,
                                                 UpdatedByUserName = trans.UpdatedByUserName,
                                                 JournalId = journal.Id,
                                                 IconCode = journalIconCode,
                                                 SourceId = journal.Id,
                                                 SourceTypeCode = AccountingEntityValues.Journal,
                                                 IsForeignAmountCreditPos = trans.ForeignAmountCredit != 0,
                                                 IsLocalAmountCreditPos = trans.LocalAmountCredit != 0,
                                                 CalculatedForeignAmount = trans.ForeignAmountCredit != 0 ? trans.ForeignAmountCredit : trans.ForeignAmountDebit,
                                                 CalculatedLocalAmount = trans.LocalAmountCredit != 0 ? trans.LocalAmountCredit : trans.LocalAmountDebit,
                                                 CurrencySign = trans.Currency.Sign,
                                             });
            if (_IsFutureOpenCheques) {
                query1 = query1.Where(x => x.PaymentValueDate > today);
                query2 = query2.Where(x => x.PaymentValueDate > today);
            }

            if (_IsUnpaidChecks)
            {
                query1 = query1.Where(x => x.PaymentValueDate <= today);
                query2 = query2.Where(x => x.PaymentValueDate <= today);
            }
            return query1.Count() + query2.Count();
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
public struct ARPaymentChequeStatuses
{
    public const string ReturnToCustomer = "5";
    public const string Redeemed = "6";
}