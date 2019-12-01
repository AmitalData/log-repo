
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Reflection.Emit;
using System.Data.Entity;
using Logitude.Accounting.Def.EntityPMs;
using System.Diagnostics;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityQueryServices;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Accounting.BL.EntityUpdateServices;
using System.Web;
using Simplog.Data.Helpers;
using Logitude.BL.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.CoreBL
{
    public class AutomaticExternalReconcileService
    {
        public AutoSelectedExternalReconciliationLines AutomaticExternalReconcile(bool amountReconcile, bool referenceReconcile, bool refDateReconcile,
            string objectTableId, string entityId, string glAccountId, QueryOperations transactionQueryOperations, QueryOperations bankPageLineQueryOperations, int tenant)
        {

            int resultedArrayLimit = 100;
            List<string> pageLinesIds = new List<string>();
            List<string> transactionsLinesIds = new List<string>();

            //smoke validation
            if (amountReconcile == false && referenceReconcile == false && refDateReconcile == false) throw new ApplicationException("Please select at least one choice");
            if (objectTableId == null) throw new ApplicationException("objectTableId is not provided !!!");
            if (glAccountId == null) throw new ApplicationException("gl Account is not provided !!!");
            //


            // get filterd lines
            var accountingContext = AccountingContext.GetContext(tenant);
            List<MyPageLine> filteredPageLines = GetFilteredPageLines(bankPageLineQueryOperations, objectTableId, entityId, tenant);
            List<MyLedgerTransaction> filteredLedgerTransactions = GetFilteredLedgerTransactions(transactionQueryOperations, glAccountId, tenant);

            // prepare result array
            List<AutoSelectedExternalReconciliation> resultedArray = new List<AutoSelectedExternalReconciliation>();

            // loop on page lines
            if (filteredPageLines.Count > 0)
            {
                if (filteredLedgerTransactions.Count == 0) throw new ApplicationException("No ledger transactions found for this bank account!");
                int groupNumberCounter = 1;

                #region case: [Amount only]
                // case: [Amount only]
                if (amountReconcile && !referenceReconcile && !refDateReconcile)
                {

                    // Group Transactions by Amount
                    var groupedTransactions = filteredLedgerTransactions.GroupBy(x => x.Amount);
                    var groupedTransactionsList = groupedTransactions.ToList();

                    // Build dictionary
                    var groupedTransactionsDictionary = groupedTransactionsList.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction

                    foreach (var pageLine in filteredPageLines)
                    {

                        // Get matched transaction
                        List<MyLedgerTransaction> groubedTransactionsListByAmount;
                        groupedTransactionsDictionary.TryGetValue(pageLine.Amount, out groubedTransactionsListByAmount);

                        if (groubedTransactionsListByAmount != null)
                        {
                            foreach (MyLedgerTransaction transaction in groubedTransactionsListByAmount)
                            {
                                if (transaction != null)
                                {
                                    // 1- check if use before
                                    var item = resultedArray.Find(d => d.LedgerTransactionId == transaction.Id);

                                    if (item == null)
                                    {
                                        // 2- push to result array
                                        resultedArray.Add(new AutoSelectedExternalReconciliation() { LedgerTransactionId = null, BankPageLineId = pageLine.Id, GroupNumber = groupNumberCounter });
                                        pageLinesIds.Add(pageLine.Id);
                                        resultedArray.Add(new AutoSelectedExternalReconciliation() { LedgerTransactionId = transaction.Id, BankPageLineId = null, GroupNumber = groupNumberCounter });
                                        transactionsLinesIds.Add(transaction.Id);

                                        // 3- check result array length
                                        if (resultedArray.Count >= resultedArrayLimit)
                                            goto Finish;

                                        groupNumberCounter++;

                                        break;
                                    }
                                }
                            }
                        }

                    }
                }
                #endregion

                #region case: [Amount + Reference]
                // case: [Amount + Reference]
                if (amountReconcile && referenceReconcile && !refDateReconcile)
                {

                    // [Ref 1] Group Transactions by Amount, Build dictionary 
                    var groupedTransactions_ref1 = filteredLedgerTransactions.GroupBy(x => new AmountRefKey(x.Amount, x.Reference1));
                    var groupedTransactionsList_ref1 = groupedTransactions_ref1.ToList();
                    var groupedTransactionsDictionary_ref1 = groupedTransactionsList_ref1.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction

                    // [Ref 2] Group Transactions by Amount, Build dictionary 
                    var groupedTransactions_ref2 = filteredLedgerTransactions.GroupBy(x => new AmountRefKey(x.Amount, x.Reference2));
                    var groupedTransactionsList_ref2 = groupedTransactions_ref2.ToList();
                    var groupedTransactionsDictionary_ref2 = groupedTransactionsList_ref2.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction

                    // [Ref 3] Group Transactions by Amount, Build dictionary 
                    var groupedTransactions_ref3 = filteredLedgerTransactions.GroupBy(x => new AmountRefKey(x.Amount, x.Reference3));
                    var groupedTransactionsList_ref3 = groupedTransactions_ref3.ToList();
                    var groupedTransactionsDictionary_ref3 = groupedTransactionsList_ref3.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction


                    foreach (var pageLine in filteredPageLines)
                    {
                        if (pageLine.Reference == null)
                            continue;

                        // Get matched transaction
                        List<MyLedgerTransaction> groubedTransactionsListByAmount;
                        groupedTransactionsDictionary_ref1.TryGetValue(new AmountRefKey(pageLine.Amount, pageLine.Reference), out groubedTransactionsListByAmount);
                        if (groubedTransactionsListByAmount == null)
                            groupedTransactionsDictionary_ref2.TryGetValue(new AmountRefKey(pageLine.Amount, pageLine.Reference), out groubedTransactionsListByAmount);
                        if (groubedTransactionsListByAmount == null)
                            groupedTransactionsDictionary_ref3.TryGetValue(new AmountRefKey(pageLine.Amount, pageLine.Reference), out groubedTransactionsListByAmount);


                        if (groubedTransactionsListByAmount != null)
                        {
                            foreach (MyLedgerTransaction transaction in groubedTransactionsListByAmount)
                            {
                                if (transaction != null)
                                {
                                    // 1- check if use before
                                    var item = resultedArray.Find(d => d.LedgerTransactionId == transaction.Id);

                                    if (item == null)
                                    {
                                        // 2- push to result array
                                        resultedArray.Add(new AutoSelectedExternalReconciliation() { LedgerTransactionId = null, BankPageLineId = pageLine.Id, GroupNumber = groupNumberCounter });
                                        pageLinesIds.Add(pageLine.Id);
                                        resultedArray.Add(new AutoSelectedExternalReconciliation() { LedgerTransactionId = transaction.Id, BankPageLineId = null, GroupNumber = groupNumberCounter });
                                        transactionsLinesIds.Add(transaction.Id);

                                        // 3- check result array length
                                        if (resultedArray.Count >= resultedArrayLimit)
                                            goto Finish;

                                        groupNumberCounter++;

                                        break;
                                    }
                                }
                            }
                        }

                        
                    }



                }
                #endregion

                #region case: [Amount + ReferenceDate]
                // case: [Amount + ReferenceDate]
                if (amountReconcile && !referenceReconcile && refDateReconcile)
                {

                    // Group Transactions by Amount + ReferenceDate

                    var groupedTransactions = filteredLedgerTransactions.GroupBy(x => new AmountRefDateKey(x.Amount, x.DocumentDate));
                    //var groupedTransactions = filteredLedgerTransactions.GroupBy(x => new { x.Amount, x.Reference1 });
                    var groupedTransactionsList = groupedTransactions.ToList();

                    // Build dictionary
                    var groupedTransactionsDictionary = groupedTransactionsList.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction

                    foreach (var pageLine in filteredPageLines)
                    {
                        // Get matched transaction
                        List<MyLedgerTransaction> groubedTransactionsListByAmount;
                        groupedTransactionsDictionary.TryGetValue(new AmountRefDateKey(pageLine.Amount, pageLine.ReferenceDate), out groubedTransactionsListByAmount);

                        if (groubedTransactionsListByAmount != null)
                        {
                            foreach (MyLedgerTransaction transaction in groubedTransactionsListByAmount)
                            {
                                if (transaction != null)
                                {
                                    // 1- check if use before
                                    var item = resultedArray.Find(d => d.LedgerTransactionId == transaction.Id);

                                    if (item == null)
                                    {
                                        // 2- push to result array
                                        resultedArray.Add(new AutoSelectedExternalReconciliation() { LedgerTransactionId = null, BankPageLineId = pageLine.Id, GroupNumber = groupNumberCounter });
                                        pageLinesIds.Add(pageLine.Id);
                                        resultedArray.Add(new AutoSelectedExternalReconciliation() { LedgerTransactionId = transaction.Id, BankPageLineId = null, GroupNumber = groupNumberCounter });
                                        transactionsLinesIds.Add(transaction.Id);

                                        // 3- check result array length
                                        if (resultedArray.Count >= resultedArrayLimit)
                                            goto Finish;

                                        groupNumberCounter++;

                                        break;
                                    }
                                }
                            }
                        }
                        
                    }

                }
                #endregion

                #region case: [Amount + Reference + ReferenceDate]
                // case: [Amount + Reference + ReferenceDate]
                if (amountReconcile && referenceReconcile && refDateReconcile)
                {
                    // [Ref 1] Group Transactions by Amount + Reference + ReferenceDate , Build dictionary
                    var groupedTransactions_ref1 = filteredLedgerTransactions.GroupBy(x => new AmountRefRefDateKey(x.Amount, x.Reference1, x.DocumentDate));
                    var groupedTransactionsList_ref1 = groupedTransactions_ref1.ToList();
                    var groupedTransactionsDictionary_ref1 = groupedTransactionsList_ref1.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction

                    // [Ref 2] Group Transactions by Amount + Reference + ReferenceDate , Build dictionary
                    var groupedTransactions_ref2 = filteredLedgerTransactions.GroupBy(x => new AmountRefRefDateKey(x.Amount, x.Reference2, x.DocumentDate));
                    var groupedTransactionsList_ref2 = groupedTransactions_ref2.ToList();
                    var groupedTransactionsDictionary_ref2 = groupedTransactionsList_ref2.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction

                    // [Ref 3] Group Transactions by Amount + Reference + ReferenceDate , Build dictionary
                    var groupedTransactions_ref3 = filteredLedgerTransactions.GroupBy(x => new AmountRefRefDateKey(x.Amount, x.Reference3, x.DocumentDate));
                    var groupedTransactionsList_ref3 = groupedTransactions_ref3.ToList();
                    var groupedTransactionsDictionary_ref3 = groupedTransactionsList_ref3.ToDictionary(d => d.Key, d => d.ToList()); // Key: Amount, Value: List of transaction

                    foreach (var pageLine in filteredPageLines)
                    {
                        if (pageLine.Reference == null)
                            continue;

                        // Get matched transaction
                        List<MyLedgerTransaction> groubedTransactionsListByAmount;
                        groupedTransactionsDictionary_ref1.TryGetValue(new AmountRefRefDateKey(pageLine.Amount, pageLine.Reference, pageLine.ReferenceDate), out groubedTransactionsListByAmount);
                        if (groubedTransactionsListByAmount == null)
                            groupedTransactionsDictionary_ref2.TryGetValue(new AmountRefRefDateKey(pageLine.Amount, pageLine.Reference, pageLine.ReferenceDate), out groubedTransactionsListByAmount);
                        if (groubedTransactionsListByAmount == null)
                            groupedTransactionsDictionary_ref3.TryGetValue(new AmountRefRefDateKey(pageLine.Amount, pageLine.Reference, pageLine.ReferenceDate), out groubedTransactionsListByAmount);


                        if (groubedTransactionsListByAmount != null)
                        {
                            foreach (MyLedgerTransaction transaction in groubedTransactionsListByAmount)
                            {
                                if (transaction != null)
                                {
                                    // 1- check if use before
                                    var item = resultedArray.Find(d => d.LedgerTransactionId == transaction.Id);

                                    if (item == null)
                                    {
                                        // 2- push to result array
                                        resultedArray.Add(new AutoSelectedExternalReconciliation() { LedgerTransactionId = null, BankPageLineId = pageLine.Id, GroupNumber = groupNumberCounter });
                                        pageLinesIds.Add(pageLine.Id);
                                        resultedArray.Add(new AutoSelectedExternalReconciliation() { LedgerTransactionId = transaction.Id, BankPageLineId = null, GroupNumber = groupNumberCounter });
                                        transactionsLinesIds.Add(transaction.Id);

                                        // 3- check result array length
                                        if (resultedArray.Count >= resultedArrayLimit)
                                            goto Finish;

                                        groupNumberCounter++;


                                        break;
                                    }
                                }
                            }
                        }
                        
                    }
                }
                #endregion

            }
            else
            {
                //throw new ApplicationException("No page lines found in bank account!");
            }


            Finish:


            AutoSelectedExternalReconciliationLines result = new AutoSelectedExternalReconciliationLines();

            LedgerTransactionListQueryService query = new LedgerTransactionListQueryService(accountingContext);
            var transactions = query.GetTransactionsByIds(transactionsLinesIds);
            transactions.ForEach(line => { line.GroupHash = resultedArray.Find(d => d.LedgerTransactionId == line.Id).GroupNumber; });
            result.transactionLines = transactions.OrderBy(d=>d.GroupHash).ToList();

            ReconcileExternalPageLineListQueryService pageQuery = new ReconcileExternalPageLineListQueryService(accountingContext);
            var pageLines = pageQuery.GetPageLinesByIds(pageLinesIds);
            pageLines.ForEach(line => { line.GroupHash = resultedArray.Find(d => d.BankPageLineId == line.Id).GroupNumber; });
            result.pageLines = pageLines.OrderBy(d => d.GroupHash).ToList();

            result.Count = resultedArray.Count;


            // return result
            return result;
        }

        List<MyPageLine> GetFilteredPageLines(QueryOperations bankPageLineQueryOperations, string objectTableId, string entityId, int tenant)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageListQueryService query = new ReconcileExternalPageListQueryService(accountingContext);

            // Get list query with query filters 
            IQueryable<ReconcileExternalPageLineList> iQuerableList = query.getPageLinesByFilter(bankPageLineQueryOperations, objectTableId, entityId, tenant);

            // Ordering
            iQuerableList = iQuerableList.OrderByDescending(a => a.ReferenceDate);

            IQueryable<MyPageLine> pageLinesDTO = (from a in iQuerableList
                                                                      select new MyPageLine()
                                                                      {
                                                                          Id = a.Id,

                                                                          Amount = a.Amount,

                                                                          Reference = a.Reference,

                                                                          ReferenceDate = a.ReferenceDate,

                                                                      });
            return pageLinesDTO.ToList();

        }

        List<MyLedgerTransaction> GetFilteredLedgerTransactions(QueryOperations transactionQueryOperations, string glAccountId, int tenant)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService query = new LedgerTransactionListQueryService(accountingContext);

            // Get list query with query filters 
            IQueryable<LedgerTransactionList> iQuerableList = query.GetIquerableOpenReconciliationFilterList(transactionQueryOperations, glAccountId, tenant);

            IQueryable<MyLedgerTransaction> linesDTO = (from a in iQuerableList
                                                            select new MyLedgerTransaction()
                                                            {
                                                                Id = a.Id,

                                                                DocumentDate = a.DocumentDate,

                                                                Reference1 = a.Reference1,

                                                                Reference2 = a.Reference2,

                                                                Reference3 = a.Reference3,

                                                                OpenAmount = a.OpenAmount,

                                                                Amount = a.ForeignAmountDebit == 0 ? a.ForeignAmountCredit : a.ForeignAmountDebit,

                                                            });
            return linesDTO.ToList();

        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }


        private static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


      
        //Generate test records
        public void GenerateTestRecordsForExternalReco(string glAccountId, string bankAccountId, string type, int tenant)
        {

            if (glAccountId == null || bankAccountId == null) throw new ApplicationException("glAccountId or bankAccountId is not provided!!");

            int linesCount = 10;

            //
            // 1- Get default values

            // LoggedUser
            ContactPM loggedContact = GetLoggedContact(tenant);



            // Tenant
            TenantQuery tenantQuery = new TenantQuery(tenant);
            TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);

            // BankAccount
            BankAccountQueryService bankQuery = new BankAccountQueryService(tenant);
            BankAccountPM bankAccount = bankQuery.GetSingle(bankAccountId, false, false);
            if (bankAccount != null && bankAccount.GLAccountCurrencyId == null) throw new ApplicationException("bank account does not have currency!!");

            // GLAccount
            GLAccountQueryService glaQuery = new GLAccountQueryService(tenant);
            GLAccountPM glAccount = glaQuery.GetSingle(glAccountId, false, false);

            // Account Exchange Rate
            RatesTableRepository rateRepo = new RatesTableRepository(tenant);
            RatesTable rate;
            if (glAccount.IsMultiCurrency == true)
            {
                //TAKE CURRENCY OF BANK ACCOUNT
                rate = rateRepo.GetClosestRate(tenantPM.CurrencyId, bankAccount.GLAccountCurrencyId, tenant);
            }
            else
            {
                rate = rateRepo.GetClosestRate(tenantPM.CurrencyId, glAccount.CurrencyId, tenant);
            }
            if (rate == null ) throw new ApplicationException("no exchange rate found!!");

            int lastPageNumber = Convert.ToInt32(bankAccount.LastPageNumber);
            DateTime? lastPageToDate;
            decimal? lastPageClosedAmount;
            if (bankAccount.LastPageNumber == null)
            {
                lastPageToDate = TenantServerConfigration.GetCurrentDateTime(0);
                lastPageClosedAmount = 0;
            }
            else
            {
                lastPageToDate = bankAccount.LastPageEndDate.Value.AddDays(1);
                lastPageClosedAmount = bankAccount.LastPageCloseBalance;
            }
            DateTime lastPageToDateplust10 = lastPageToDate.Value.AddDays(linesCount+1);


            // 2- Build bank page and transactions 
            JournalPM journal = new JournalPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                UpdateDate = DateTime.Now,
                UpdatedByUserId = loggedContact.Id,
                ApproveDate = DateTime.Now,
                ApprovedByUserId = loggedContact.Id,
                
                IsVoided = false,
                QueueId = null,

                Tenant = tenant,
                //JournalNumber = xxx,
                CreateDate = DateTime.Now,
                AccountingDate = DateTime.Now,
                TypeCode = "0", // 0- Manual
                StatusCode = "2", // 2- Approved
                CreatedByUserId = loggedContact.Id,
                AccountingEntityCode = "1", // 1- Jounral

                //AccountingEntityReference = xxxx,
                //OriginalJournalId = xxxx,

                //UpdatedByUserName = xxxx,
                //ApprovedByUserName = xxxx,
                //SearchFields = xxxx,
                //VoidedByUserId = xxxx,
                //VoidDate = xxxx,
                //OriginalJournalName = xxxx,
                //VoidedByUserName = xxxx,
                //VoidedByJournalId = xxxx,
                //ExternalSystem = xxxx,
                //StatusLocalName = xxxx,

            };
            ObjectTable bankAccountObjectTable = GetBankAccountObjectTable(tenant);

            ReconcileExternalPagePM bankPage = new ReconcileExternalPagePM()
            {
                Id = "",
                Tenant = tenant,
                ApprovedByUserId = loggedContact.Id,
                CreatedByUserId = loggedContact.Id,
                CreateDate = DateTime.Now,

                GLAccountId = glAccountId,
                EntityId = bankAccountId,
                ObjectTableId = bankAccountObjectTable.Id,
                EntryTypeCode = "1", // 1- Manual

                StartBalance = lastPageClosedAmount.Value,
                CloseBalance = lastPageClosedAmount.Value,

                PageNo = lastPageNumber + 1,

                FromDate = lastPageToDate.Value,
                ToDate = lastPageToDateplust10,

                StatusCode = "2", // 2- Approved
                //ReconcileExternalPageLines,

                ChangeSetOp = ChangeSetOperation.Insert,
            };

            decimal linesSum = 0;

            switch (type)
            {
                #region type 1
                case "1":
                    {
                        int sm = 2;
                        for (int i = 0; i < linesCount; i++)
                        {
                            DateTime _referenceDate = (i > (linesCount / 2) ? lastPageToDate.Value.AddDays(1) : lastPageToDate.Value.AddDays(i));
                            decimal _amount = ((i + 1) * 100);
                            //
                            // Declare page line
                            ReconcileExternalPageLinePM pageLine = new ReconcileExternalPageLinePM()
                            {
                                ReconcileExternalPageId = bankPage.Id,
                                LineNumber = i + 1,

                                ReferenceDate = _referenceDate,
                                Amount = _amount,
                                //Reference = "",

                                IsReconciled = false,
                                Notes = "",
                                Tenant = tenant,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };

                            // Fill reference field
                            if (i < 4)
                                pageLine.Reference = "" + (i*10+100);
                            else if (i >= 4 && i < 7)
                                pageLine.Reference = "" + (i * 10 + 100);
                            else
                                pageLine.Reference = "" + (i * 10 + 100);

                            linesSum += pageLine.Amount;




                            //
                            // Declare journal line
                            JournalLinePM journalLine = new JournalLinePM()
                            {
                                ChangeSetOp = ChangeSetOperation.Insert,
                                Tenant = tenant,
                                //JournalId = journal.Id,
                                Line = i + 1,
                                ActionCode = "3", // 2- Credit & Debit

                                DebitAccountId = glAccountId,
                                CreditAccountId = glAccountId,
                                //DebitControlAccountId = xxxx,
                                //CreditControlAccountId = xxxx,

                                DocumentDate = _referenceDate,
                                AccountingDate = DateTime.Now,
                                DueDate = _referenceDate,

                                ForeignAmount = _amount,
                                LocalAmount = _amount * Convert.ToDecimal(rate.Rate),

                                CurrencyId = glAccount.CurrencyId,
                                ExchangeRate = Convert.ToDecimal(rate.Rate),

                                //Notes = xxxx,

                                //Reference2 = xxxx,
                                //Reference3 = xxxx,
                                //ActionName = xxxx,
                                //DebitControlAccountName = xxxx,
                                //CreditAccountName = xxxx,
                                //DebitAccountName = xxxx,
                                //CreditControlAccountName = xxxx,
                                //CreditControlAccountNumber = xxxx,
                                //DebitControlAccountNumber = xxxx,
                                ////CreditAccountNumber = xxxx,
                                //DebitAccountNumber = xxxx,
                                //CurrencyName = xxxx,
                                //CurrencyCode = xxxx,
                                //ActionTypeCode = xxxx,
                                //ExternalOpenAmount = xxxx,
                                //IsCreditAccountMulti = false,
                                //IsDebitAccountMulti = xxxx,

                            };

                            // Fill reference field
                            if (sm == 0)
                                journalLine.Reference1 = "" + (i * 10 + 1000);
                            else if (sm == 1)
                                journalLine.Reference2 = "" + (i * 10 + 1000);
                            else if (sm == 2)
                                journalLine.Reference3 = "" + (i * 10 + 1000);
                            if (sm == 0) sm = 2;
                            else if (sm > 0) sm--;


                            // Push lines
                            bankPage.ReconcileExternalPageLines.Add(pageLine);
                            journal.JournalLines.Add(journalLine);



                        }

                        break;
                    }
                #endregion


                #region type 2
                case "2":
                    {
                        int sm = 2;
                        for (int i = 0; i < linesCount; i=i+2)
                        {
                            DateTime _referenceDate = (i > (linesCount / 2) ? lastPageToDate.Value.AddDays(1) : lastPageToDate.Value.AddDays(i));
                            decimal _amount = ((i + 1) * 100);
                            //
                            // Declare page line
                            ReconcileExternalPageLinePM pageLine = new ReconcileExternalPageLinePM()
                            {
                                ReconcileExternalPageId = bankPage.Id,
                                LineNumber = i + 1,

                                ReferenceDate = _referenceDate,
                                Amount = _amount/2,
                                //Reference = "",

                                IsReconciled = false,
                                Notes = "",
                                Tenant = tenant,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };

                            // Fill reference field
                            if (i < 4)
                                pageLine.Reference = "" + (i * 10 + 1000);
                            else if (i >= 4 && i < 7)
                                pageLine.Reference = "" + (i * 10 + 1000);
                            else
                                pageLine.Reference = "" + (i * 10 + 1000);

                            linesSum += pageLine.Amount;

                            //
                            // Declare page line
                            ReconcileExternalPageLinePM pageLine2 = new ReconcileExternalPageLinePM()
                            {
                                ReconcileExternalPageId = bankPage.Id,
                                LineNumber = i + 2,

                                ReferenceDate = _referenceDate,
                                Amount = _amount/2,
                                //Reference = "",

                                IsReconciled = false,
                                Notes = "",
                                Tenant = tenant,
                                ChangeSetOp = ChangeSetOperation.Insert,

                            };

                            // Fill reference field
                            if (i < 4)
                                pageLine2.Reference = ""+(i * 10 + 1000);
                            else if (i >= 4 && i < 7)
                                pageLine2.Reference = ""+(i * 10 + 1000);
                            else
                                pageLine2.Reference = ""+(i * 10 + 1000);

                            linesSum += pageLine2.Amount;




                            //
                            // Declare journal line
                            JournalLinePM journalLine = new JournalLinePM()
                            {
                                ChangeSetOp = ChangeSetOperation.Insert,
                                Tenant = tenant,
                                //JournalId = journal.Id,
                                Line = i + 1,
                                ActionCode = "3", // 2- Credit & Debit

                                DebitAccountId = glAccountId,
                                CreditAccountId = glAccountId,
                                //DebitControlAccountId = xxxx,
                                //CreditControlAccountId = xxxx,

                                DocumentDate = _referenceDate,
                                AccountingDate = DateTime.Now,
                                DueDate = _referenceDate,

                                ForeignAmount = _amount,
                                LocalAmount = _amount * Convert.ToDecimal(rate.Rate),

                                CurrencyId = glAccount.CurrencyId,
                                ExchangeRate = Convert.ToDecimal(rate.Rate),

                                //Notes = xxxx,

                                //Reference2 = xxxx,
                                //Reference3 = xxxx,
                                //ActionName = xxxx,
                                //DebitControlAccountName = xxxx,
                                //CreditAccountName = xxxx,
                                //DebitAccountName = xxxx,
                                //CreditControlAccountName = xxxx,
                                //CreditControlAccountNumber = xxxx,
                                //DebitControlAccountNumber = xxxx,
                                ////CreditAccountNumber = xxxx,
                                //DebitAccountNumber = xxxx,
                                //CurrencyName = xxxx,
                                //CurrencyCode = xxxx,
                                //ActionTypeCode = xxxx,
                                //ExternalOpenAmount = xxxx,
                                //IsCreditAccountMulti = false,
                                //IsDebitAccountMulti = xxxx,

                            };

                            // Fill reference field
                            if (sm==0)
                                journalLine.Reference1 = "" + (i * 10 + 1000);
                            else if (sm == 1)
                                journalLine.Reference2 = "" + (i * 10 + 1000);
                            else if (sm == 2)
                                journalLine.Reference3 = "" + (i * 10 + 1000);
                            if (sm == 0) sm = 2;
                            else if (sm > 0) sm--;

                            // Push lines
                            bankPage.ReconcileExternalPageLines.Add(pageLine);
                            bankPage.ReconcileExternalPageLines.Add(pageLine2);
                            journal.JournalLines.Add(journalLine);



                        }

                        break;
                    }
                    #endregion

            }



            // تهذيب
            bankPage.CloseBalance = lastPageClosedAmount.Value + linesSum;



            // 3- Update entities
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            JournalUpdateService journalUpdateService = new JournalUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            journalUpdateService.Update(journal, true);

            ReconcileExternalPageUpdateService bankPageUpdateService = new ReconcileExternalPageUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            bankPageUpdateService.Update(bankPage, true);


        }
        private ObjectTable GetBankAccountObjectTable(int tenant)
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            ObjectTable bankAccountObjectTable = objectTableRepository.GetObjectTableByName("BankAccount", tenant, false);
            if (bankAccountObjectTable == null)
                throw new ApplicationException("No objectfield for BankAccount!");
            return bankAccountObjectTable;
        }
    }



    public class MyLedgerTransaction
    {
        public string Id { get; set; }

        //public DateTime AccountingDate { get; set; }

        public DateTime DocumentDate { get; set; }

        public decimal Amount { get; set; }
        //public string CurrencyId { get; set; }

        //public decimal ExchangeRate { get; set; }

        public string Reference1 { get; set; }

        public string Reference2 { get; set; }

        public string Reference3 { get; set; }

        public decimal OpenAmount { get; set; }
        //public string OpenAmountCurrencyId { get; set; }

    }

    public class MyPageLine
    {
        public string Id { get; set; }
        public DateTime ReferenceDate { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }

    }

    public class AutoSelectedExternalReconciliation
    {
        public string LedgerTransactionId { get; set; }
        public string BankPageLineId { get; set; }
        public int GroupNumber { get; set; }


    }
    public class AutoSelectedExternalReconciliationLines
    {
        public List<LedgerTransactionList> transactionLines { get; set; }
        public List<ReconcileExternalPageLineList> pageLines { get; set; }
        public int Count { get; set; }

    }

    struct Key
    {
        public readonly string Id;
        public readonly decimal Amount;
        public readonly string Reference;
        public readonly DateTime ReferenceDate;
        public Key(string id, decimal amount, string reference, DateTime referenceDate)
        {
            Id = id;
            Amount = amount;
            Reference = reference;
            ReferenceDate = referenceDate;
        }
    }
    struct AmountRefKey
    {
        public readonly decimal Amount;
        public readonly string Reference;
        public AmountRefKey(decimal amount, string reference)
        {
            Amount = amount;
            Reference = reference;
        }
    }
    struct AmountRefRefDateKey
    {
        public readonly decimal Amount;
        public readonly string Reference;
        public readonly DateTime ReferenceDate;
        public AmountRefRefDateKey(decimal amount, string reference, DateTime referenceDate)
        {
            Amount = amount;
            Reference = reference;
            ReferenceDate = referenceDate;
        }
    }
    struct AmountRefDateKey
    {
        public readonly decimal Amount;
        public readonly DateTime ReferenceDate;
        public AmountRefDateKey(decimal amount, DateTime referenceDate)
        {
            Amount = amount;
            ReferenceDate = referenceDate;
        }
    }

}

