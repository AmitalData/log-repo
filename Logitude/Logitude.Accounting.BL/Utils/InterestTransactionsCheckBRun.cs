using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Accounting.BL.CloseTables;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;

namespace Logitude.Accounting.BL.Utils
{
    public class InterestTransactionsCheckBRun
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;
        private List<string> _NoLines;
        private List<string> _WrongAction;
        //private List<string> _WrongSum;
        private List<string> _WrongSumToMatch;
        long _counter = 0;
        private const string WorksChartOfAccountTypeCode = "6";
        public const int LT_LinesMaximum_MIN = 2;
        public const int LT_LinesMaximum_MAX = 200;
        public const int MaxPageSize_MAX = 1000;
        public const int MaxGLAccountsPerQuery_Def = 100;
        public string SpecificJournalId = "";
        public string LastMadeGLAccountId = "";
        public int MaxGLAccountsPerQuery = 100;
        private List<string> badList;
        private List<string> goodList;
        private List<string> madeList;
        public InterestTransactionsCheckBRunResult MyInterestTransactionsCheckBRunResult = new InterestTransactionsCheckBRunResult();

        public InterestTransactionsCheckBRun()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }
        public void RunInterestTransactionsCheckB(InterestTransactionsCheckBArg interestTransactionsCheckBArg)
        {
            try
            {
                DateTime fromDate = DateTime.MinValue;
                DateTime oldDate = DateTime.MinValue;
                //  decimal oldAmount = Decimal.MaxValue;
                int tenant = interestTransactionsCheckBArg.Tenant;
                string myGLAccountId = interestTransactionsCheckBArg.GLAccountId;
                SpecificJournalId = interestTransactionsCheckBArg.SpecificJournalId;
                LastMadeGLAccountId = interestTransactionsCheckBArg.LastMadeGLAccountId;
                MaxGLAccountsPerQuery = interestTransactionsCheckBArg.MaxGLAccountsPerQuery;
                if (MaxGLAccountsPerQuery <= 0) MaxGLAccountsPerQuery = MaxGLAccountsPerQuery_Def;
                badList = new List<string>();
                goodList = new List<string>();
                madeList = new List<string>();
                _NoLines = new List<string>();
                _WrongAction = new List<string>();
                //   _WrongSum = new List<string>();
                _WrongSumToMatch = new List<string>();

                // DateTime myUpToDueDate = interestTransactionsCheckBArg.UpToDueDate;

                IAccountingContext context = AccountingContext.GetContext(tenant);
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(context);
                LedgerTransactionListQueryService ledgerTransactionListQueryService = new LedgerTransactionListQueryService(context);
                CheckDbIntegrity(myGLAccountId, interestTransactionsCheckBArg.AccountTypeCode, tenant);


                _ResponseText = $"Good: {MyInterestTransactionsCheckBRunResult.SuccessAccountLineCount},  Bad: {MyInterestTransactionsCheckBRunResult.BadAccountLineCount}, \n  Lines: \n{String.Join("\n", MyInterestTransactionsCheckBRunResult.ErrorRowList.ToArray())}";

            }

            catch (Exception e)
            {
                throw new Exception($"InterestTransactionsCheckBRun failure {e.Message} Inner Exception: {e.InnerException.Message}", e);
            }
        }


        //private bool RunInterestTransactionsCheckB_OneAccount(InterestTransactionsCheckBArg interestTransactionsCheckBArg)
        //{
        //    bool rv_success = false;
        //    try
        //    {
        //        // DateTime fromDate = DateTime.MinValue;
        //        // string fromId = "";
        //        DateTime oldDate = DateTime.MinValue;
        //        string oldJournalId = "";
        //        decimal oldAmount = Decimal.MaxValue;
        //        bool runAgain = false;
        //        int tenant = interestTransactionsCheckBArg.Tenant;
        //        string myGLAccountId = interestTransactionsCheckBArg.GLAccountId;
        //        string displayNumber = "";
        //        if (!String.IsNullOrEmpty(myGLAccountId))
        //        {
        //            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
        //            GLAccountPM glac = gLAccountQueryService.GetSinglePM(myGLAccountId, tenant);
        //            if (glac != null)
        //            {
        //                displayNumber = glac.DisplayNumber;
        //            }
        //        }
        //        //   DateTime myUpToDueDate = interestTransactionsCheckBArg.UpToDueDate;

        //        if (String.IsNullOrWhiteSpace(myGLAccountId))
        //        {
        //            throw new Exception($"GLAccountId is missing");
        //        }
        //        else
        //        {
        //            bool success = false;
        //            bool toContinue = true;
        //            bool moveOn = false;
        //            do
        //            {
        //                success = false;
        //                InterestTransactionsGetNextGroupArgs getNextGroupArgs = new InterestTransactionsGetNextGroupArgs
        //                {
        //                    Tenant = tenant,
        //                    GLAccountId = myGLAccountId,
        //                    MIN = LT_LinesMaximum_MIN,
        //                    LT_LinesMaximum = interestTransactionsCheckBArg.LT_LinesMaximum > LT_LinesMaximum_MAX ? LT_LinesMaximum_MAX : interestTransactionsCheckBArg.LT_LinesMaximum,
        //                    MaxPageSize = interestTransactionsCheckBArg.MaxPageSize > MaxPageSize_MAX ? MaxPageSize_MAX : interestTransactionsCheckBArg.MaxPageSize,
        //                    RunAgain = runAgain,
        //                    MoveOn = moveOn,
        //                    OldDate = oldDate,
        //                    OldJournalId = oldJournalId,
        //                    OldAmount = oldAmount,
        //                    MaximalDifference = interestTransactionsCheckBArg.MaximalDifference,
        //                    SpecificJournalId = interestTransactionsCheckBArg.SpecificJournalId,
        //                    Stop = false,
        //                };
        //                runAgain = false;
        //                moveOn = false;
        //                List<JournalLT_GroupItem> journal_List = GetNextJournal_List(ref getNextGroupArgs);
        //               //// List<JournalLT_GroupItem> it_List = GetNextJournal_List(ref getNextGroupArgs);
        //                oldDate = getNextGroupArgs.OldDate;
        //                oldJournalId = getNextGroupArgs.OldJournalId;
        //                oldAmount = getNextGroupArgs.OldAmount;
        //                if (getNextGroupArgs.Stop || journal_List == null || journal_List.Count == 0
        //                          || !String.IsNullOrWhiteSpace(interestTransactionsCheckBArg.SpecificJournalId)) // Nothing retrieved from the DB, or a specific journal given
        //                {
        //                    toContinue = false;
        //                }
        //                if (journal_List == null || journal_List.Count == 0) // Some records retrieved but no sum<=MaxDiff
        //                {
        //                    runAgain = false;
        //                    moveOn = true;
        //                }
        //                else if (!(getNextGroupArgs.Stop || journal_List == null || journal_List.Count == 0))
        //                {
        //                    decimal actualDifference = getNextGroupArgs.ActualDifference;

        //                    success = ProcessOneJournal_List(tenant, myGLAccountId, displayNumber, journal_List, interestTransactionsCheckBArg.MaximalDifference, actualDifference);
        //                    if (success)
        //                    {
        //                        rv_success = true;
        //                        //     runAgain = true; // try more from the oldData, oldId
        //                        moveOn = true;
        //                    }
        //                    else
        //                    {
        //                        moveOn = true;
        //                    }
        //                }

        //            } while (toContinue);

        //        }
        //        return rv_success;

        //    }

        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
        //private bool ProcessOneJournal_List(int tenant, string myGLAccountId, string displayNumber, List<JournalLT_GroupItem> journal_List,
        //    decimal maximalDifference, decimal actualDifference)
        //{
        //    bool rv = false;
        //    try
        //    {
        //        IAccountingContext context = AccountingContext.GetContext(tenant);
        //        InterestTransactionQueryService interestTransactionQueryService = new InterestTransactionQueryService(context);
        //        foreach (JournalLT_GroupItem journal_LT_group in journal_List)
        //        {
        //            string journalId = journal_LT_group.JournalList.Id;
        //            string journalNum = journal_LT_group.JournalList.JournalNumber;
        //            string accountingEntityCode = journal_LT_group.JournalList.AccountingEntityCode;
        //            string accountingEntityReference = journal_LT_group.JournalList.AccountingEntityReference;
        //            List<LedgerTransactionList> lt_list = journal_LT_group.LT_List;
        //            switch (accountingEntityCode)
        //            {
        //                case AccountingEntityValues.ARInvoice:
        //                    ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(tenant);
        //                    ARInvoicePM aRInvoicePM = invoiceQuery.GetSingleInvoiceByInvoiceNumber(accountingEntityReference, tenant);
        //                    if (aRInvoicePM == null)
        //                    {   // old invoices - without invoice type 
        //                        string invoice_no = accountingEntityReference.Substring(1);
        //                        aRInvoicePM = invoiceQuery.GetSingleInvoiceByInvoiceNumber(invoice_no, tenant);

        //                    }
        //                    if (aRInvoicePM != null)
        //                    {
        //                        bool split_inv = aRInvoicePM.IsMultiCurrency;
        //                        if (split_inv)
        //                        {
        //                            GLAccountCurrencyQueryService gLAccountCurrencyQuery = new GLAccountCurrencyQueryService(tenant);
        //                            List<GLAccountCurrencyPM> gLAccountCurrencies = gLAccountCurrencyQuery.GetRelatedCurrenciesAccountByCustomerGLAccount(myGLAccountId, tenant);
        //                            if (gLAccountCurrencies == null)
        //                            {
        //                                CheckOneRefInv(interestTransactionQueryService, TranslateToInterestEntity(accountingEntityCode), aRInvoicePM.Id, accountingEntityReference,
        //                                    myGLAccountId, displayNumber, journalId, journalNum, tenant, aRInvoicePM.InvoiceLines, aRInvoicePM.TotalVATs);
        //                            }
        //                            else
        //                            {
        //                                var il_groups = aRInvoicePM.InvoiceLines.GroupBy(il => il.ForiegnCurrencyId).ToList();
        //                                List<String> currencies = new List<string>();
        //                                il_groups.ForEach(il =>
        //                                {
        //                                    if (!currencies.Contains(il.Key))
        //                                        currencies.Add(il.Key);
        //                                });

        //                                if (!currencies.Contains(aRInvoicePM.InvoiceCurrencyId))
        //                                    currencies.Add(aRInvoicePM.InvoiceCurrencyId);
        //                                bool was_local_curr = false;
        //                                currencies.ForEach(curr =>
        //                                {
        //                                    string glaccid_thiscurr = myGLAccountId;
        //                                    string gdacc_displ_thiscurr = displayNumber;
        //                                    GLAccountCurrencyPM glaccurPM = gLAccountCurrencies.Where(gc => gc.CurrencyId == curr).FirstOrDefault();
        //                                    if (glaccurPM == null || String.IsNullOrEmpty(glaccurPM.GLAccountId))
        //                                    {
        //                                        glaccid_thiscurr = myGLAccountId;
        //                                        gdacc_displ_thiscurr = displayNumber;
        //                                    }
        //                                    else
        //                                    {
        //                                        glaccid_thiscurr = glaccurPM.GLAccountId;
        //                                        gdacc_displ_thiscurr = glaccurPM.GLAccountNumber;

        //                                    }
        //                                    if (curr == aRInvoicePM.LocalCurrencyId)
        //                                    {
        //                                        was_local_curr = true;
        //                                        CheckOneRefInv(interestTransactionQueryService, TranslateToInterestEntity(accountingEntityCode), aRInvoicePM.Id, accountingEntityReference, glaccid_thiscurr, gdacc_displ_thiscurr, journalId, journalNum, tenant, aRInvoicePM.InvoiceLines, aRInvoicePM.TotalVATs, curr);
        //                                    }
        //                                    else
        //                                    {
        //                                        CheckOneRefInv(interestTransactionQueryService, TranslateToInterestEntity(accountingEntityCode), aRInvoicePM.Id, accountingEntityReference, glaccid_thiscurr, gdacc_displ_thiscurr, journalId, journalNum, tenant, aRInvoicePM.InvoiceLines, null, curr);
        //                                    }
        //                                });
        //                                if (was_local_curr == false && aRInvoicePM.TotalVATs != null && aRInvoicePM.TotalVATs.Count > 0)
        //                                {
        //                                    string glaccid_localcurr = myGLAccountId;
        //                                    string gdacc_displ_localcurr = displayNumber;
        //                                    GLAccountCurrencyPM glaccurPM_loc = gLAccountCurrencies.Where(gc => gc.CurrencyId == aRInvoicePM.LocalCurrencyId).FirstOrDefault();
        //                                    if (glaccurPM_loc == null || String.IsNullOrEmpty(glaccurPM_loc.GLAccountId))
        //                                    {
        //                                        glaccid_localcurr = myGLAccountId;
        //                                        gdacc_displ_localcurr = displayNumber;
        //                                    }
        //                                    else
        //                                    {
        //                                        glaccid_localcurr = glaccurPM_loc.GLAccountId;
        //                                        gdacc_displ_localcurr = glaccurPM_loc.GLAccountNumber;
        //                                    }
        //                                    CheckOneRefInv(interestTransactionQueryService, TranslateToInterestEntity(accountingEntityCode), aRInvoicePM.Id, accountingEntityReference, glaccid_localcurr, gdacc_displ_localcurr, journalId, journalNum, tenant, aRInvoicePM.InvoiceLines, aRInvoicePM.TotalVATs, aRInvoicePM.LocalCurrencyId);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            CheckOneRefInv(interestTransactionQueryService, TranslateToInterestEntity(accountingEntityCode), aRInvoicePM.Id, accountingEntityReference, myGLAccountId, displayNumber, journalId, journalNum, tenant, aRInvoicePM.InvoiceLines, aRInvoicePM.TotalVATs);
        //                        }
        //                    }
        //                    break;

        //                case AccountingEntityValues.ARPayment:
        //                    ARPaymentQuery aRPaymentQuery = new ARPaymentQuery(tenant);
        //                    ARPaymentPM aRPaymentPM = aRPaymentQuery.GetSinglePaymentByPaymentNumber_00(accountingEntityReference, tenant);
        //                    if (aRPaymentPM == null)
        //                    {   // old receipts - without invoice type in the end 
        //                        int len = accountingEntityReference.Length;
        //                        if (len > 0)
        //                        {
        //                            string inv_no = accountingEntityReference.Substring(0, len - 1);
        //                            aRPaymentPM = aRPaymentQuery.GetSinglePaymentByPaymentNumber_00(inv_no, tenant);
        //                        }

        //                    }
        //                    if (aRPaymentPM != null)
        //                    {
        //                        GLAccountPM glac_incurr = GetARPaymentGLAccount(aRPaymentPM.BillToId, tenant, aRPaymentPM.PaymentCurrencyId);
        //                        if (glac_incurr != null)
        //                            CheckOneRef(interestTransactionQueryService, TranslateToInterestEntity(accountingEntityCode), aRPaymentPM.Id, accountingEntityReference, glac_incurr.Id, glac_incurr.DisplayNumber, "", journalId, journalNum, tenant, lt_list);
        //                        else
        //                            CheckOneRef(interestTransactionQueryService, TranslateToInterestEntity(accountingEntityCode), aRPaymentPM.Id, accountingEntityReference, myGLAccountId, displayNumber, "", journalId, journalNum, tenant, lt_list);
        //                    }
        //                    break;

        //                case AccountingEntityValues.Journal:
        //                    CheckOneRef(interestTransactionQueryService, TranslateToInterestEntity(accountingEntityCode), journal_LT_group.JournalList.Id, accountingEntityReference, myGLAccountId, displayNumber, "", journalId, journalNum, tenant, lt_list);
        //                    break;

        //                case AccountingEntityValues.BankAdjustment:
        //                    CheckOneRef(interestTransactionQueryService, TranslateToInterestEntity(accountingEntityCode), journal_LT_group.JournalList.Id, accountingEntityReference, myGLAccountId, displayNumber, "", journalId, journalNum, tenant, lt_list);
        //                    break;

        //                default:
        //                    CheckOneRef(interestTransactionQueryService, TranslateToInterestEntity(accountingEntityCode), journal_LT_group.JournalList.Id, accountingEntityReference, myGLAccountId, displayNumber, "", journalId, journalNum, tenant, lt_list);
        //                    break;
        //            }
        //        }

        //        rv = true;
        //        return rv;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception($"InterestTransactionsCheckBRun failure {e.Message} Inner Exception: {e.InnerException.Message}", e);
        //    }
        //}
        //private string TranslateToInterestEntity(string accountingEntityCode)
        //{
        //    string rv = "3"; // Journal by default
        //    switch (accountingEntityCode)
        //    {
        //        case "2": // ARInvoice
        //            rv = "1";
        //            break;
        //        case "3": // ARPayment
        //            rv = "2";
        //            break;
        //        case "10": // Adjustment
        //            rv = "5";
        //            break;
        //        case "12": // Bank Adjustment
        //            rv = "5";
        //            break;
        //        case "11": // Year Transfer
        //            rv = "4"; // Open Balance
        //            break;
        //        default:
        //            rv = "3"; // Journal
        //            break;
        //    }
        //    return rv;
        //}



        //private void CheckOneRef(InterestTransactionQueryService interestTransactionQueryService, string accountingEntityCode, string accountingEntityId, string accountingEntityReference,
        //    string gLAccountId, string displayNumber, string currencyId, string journalId, string journalNum, int tenant, List<LedgerTransactionList> lt_list)
        //{
        //    decimal lt_group_total = lt_list.Sum(lt => lt.LocalAmountDebit - lt.LocalAmountCredit);
        //    List<InterestTransactionPM> itlist_j = interestTransactionQueryService.GetInterestTransactionPMsByEntityTypeCodeIdAccount(accountingEntityCode, accountingEntityId, gLAccountId, tenant);
        //    if (itlist_j != null && itlist_j.Count > 0)
        //    {
        //        decimal inttt_total = itlist_j.Sum(it => it.LocalAmount);
        //        if (inttt_total == lt_group_total)
        //        {
        //            this.MyInterestTransactionsCheckBRunResult.SuccessAccountLineCount++;
        //        }
        //        else
        //        {
        //            this.AddErrorRowList(gLAccountId, displayNumber, journalId, journalNum, accountingEntityCode, accountingEntityId, accountingEntityReference, currencyId, lt_group_total, inttt_total, "Not equal");
        //        }
        //    }
        //    else
        //    {
        //        this.AddErrorRowList(gLAccountId, displayNumber, journalId, journalNum, accountingEntityCode, accountingEntityId, accountingEntityReference, currencyId, lt_group_total, 0m, "Nothing found");
        //    }

        //}


        //private void CheckOneRefInv(InterestTransactionQueryService interestTransactionQueryService, string accountingEntityCode, string accountingEntityId, string accountingEntityReference,
        //    string gLAccountId, string displayNumber, string journalId, string journalNum, int tenant,
        //    List<ARInvoiceLinePM> invline_list, List<ARInvoiceTotalVATPM> vatline_list, string currencyId = null)
        //{
        //    decimal inv_group_total = 0m;
        //    if (invline_list != null && invline_list.Count > 0)
        //    {
        //        if (String.IsNullOrEmpty(currencyId))
        //            inv_group_total = invline_list.Sum(il => il.LocalCurrencyAmount.HasValue ? (decimal)il.LocalCurrencyAmount.Value : 0.0m);
        //        else
        //            inv_group_total = invline_list.Where(il => il.ForiegnCurrencyId == currencyId).Sum(il => il.LocalCurrencyAmount.HasValue ? (decimal)il.LocalCurrencyAmount.Value : 0.0m);
        //    }

        //    decimal vat_group_total = 0m;
        //    if (vatline_list != null && vatline_list.Count > 0)
        //        vat_group_total = vatline_list.Sum(vl => vl.LocalVATAmount.HasValue ? (decimal)vl.LocalVATAmount.Value : 0.0m);

        //    decimal inv_total = inv_group_total + vat_group_total;

        //    List<InterestTransactionPM> itlist_j = interestTransactionQueryService.GetInterestTransactionPMsByEntityTypeCodeIdAccountCurr(accountingEntityCode, accountingEntityId, gLAccountId, tenant, currencyId);
        //    if (itlist_j != null && itlist_j.Count > 0)
        //    {
        //        decimal inttt_total = itlist_j.Sum(it => it.LocalAmount);


        //        if (inttt_total == inv_total)
        //        {
        //            this.MyInterestTransactionsCheckBRunResult.SuccessAccountLineCount++;
        //        }
        //        else
        //        {
        //            this.AddErrorRowList(gLAccountId, displayNumber, journalId, journalNum, accountingEntityCode, accountingEntityId, accountingEntityReference, currencyId, inv_total, inttt_total, "Not equal");
        //        }
        //    }
        //    else
        //    {
        //        this.AddErrorRowList(gLAccountId, displayNumber, journalId, journalNum, accountingEntityCode, accountingEntityId, accountingEntityReference, currencyId, inv_total, 0m, "Nothing found");
        //    }

        //}

        //private GLAccountPM GetGLAccount(ARInvoicePM invoice, int tenant, string currencyId)
        //{
        //    GLAccountQueryService glAccountQuery = new GLAccountQueryService(tenant);
        //    if (invoice.BillToGLAccountId == null)
        //    {
        //        GLAccountPM debitGLAccount = getDebitGLAccount(invoice.BillToId, invoice.Tenant, glAccountQuery);
        //        GLAccountPM splittedAccount = glAccountQuery.GetSplittedByCurrencyGLAccount(debitGLAccount.Id, invoice.Tenant, currencyId);
        //        if (splittedAccount != null)
        //            return splittedAccount;
        //        else
        //            return debitGLAccount;
        //    }
        //    else
        //    {
        //        return glAccountQuery.GetSinglePM(invoice.BillToGLAccountId, invoice.Tenant);
        //    }
        //}


        //private GLAccountPM getDebitGLAccount(string billToId, int tenant, GLAccountQueryService glAccountQuery, string billToGLAccountId = null)
        //{
        //    GLAccountPM glaAccount = null;
        //    if (billToGLAccountId != null)
        //    {
        //        var billToGLAccount = glAccountQuery.GetSinglePM(billToGLAccountId, tenant);
        //        if (billToGLAccount.ChartOfAccountsTypeCode == WorksChartOfAccountTypeCode)
        //        {
        //            return billToGLAccount;
        //        }
        //    }
        //    CardRepository cardRep = new CardRepository(tenant);
        //    Card card = cardRep.GetSingleCard(billToId, tenant);
        //    if (card != null)
        //    {
        //        CheckTheCardGLAccount(card);
        //        glaAccount = glAccountQuery.GetSinglePM(card.GLAccountId, tenant);
        //    }

        //    return glaAccount;
        //}

        //private void CheckTheCardGLAccount(Card card)
        //{
        //    if (card.GLAccountId == null)
        //    {
        //        throw new Exception("The Bill To Card " + card.Code + " is not connected to a GLAccount ");
        //    }
        //}



        //private GLAccountPM GetARPaymentGLAccount(string billToId, int tenant, string currencyId)

        //{
        //    GLAccountPM glaAccount = null;
        //    GLAccountQueryService glAccountQuery = new GLAccountQueryService(tenant);
        //    CardRepository cardRep = new CardRepository(tenant);
        //    Card card = cardRep.GetSingleCard(billToId, tenant);
        //    if (card != null)
        //    {
        //        glaAccount = glAccountQuery.GetSinglePM(card.GLAccountId, tenant);

        //        if (glaAccount != null && glaAccount.IsMultiCurrency.Value)
        //        {
        //            string splitByCurrencyAccountId = GetAccountIdForGLAccountCurrency(glaAccount, currencyId);
        //            glaAccount = glAccountQuery.GetSinglePM(splitByCurrencyAccountId, tenant);
        //        }
        //        else return glaAccount;
        //    }


        //    return glaAccount;
        //}

        //private string GetAccountIdForGLAccountCurrency(GLAccountPM gLAccount, string paymentCurrencyId)
        //{
        //    GLAccountCurrencyRepository glAccountCurrencyRepository = new GLAccountCurrencyRepository(gLAccount.Tenant);
        //    GLAccountCurrency gLAccountCurrency = glAccountCurrencyRepository.GetEntityByCurrencyAndGLAccountId(gLAccount.Id, paymentCurrencyId, gLAccount.Tenant);
        //    if (gLAccountCurrency != null)
        //    {
        //        return gLAccountCurrency.GLAccountId;
        //    }
        //    else return gLAccount.Id;

        //}


        private void AddErrorRowList(string gLAccount_id, string displayNumber, string lastReportNum, decimal accBlz, decimal repBlz, decimal diff, string message)
        {
            string separator = ";";
            this.AddErrorRow($"{gLAccount_id}{separator}{displayNumber}{separator}{lastReportNum}{separator}{accBlz.ToString()}{separator}{repBlz.ToString()}{separator}{diff.ToString()}{separator}{message}");
        }


        private void AddErrorRow(String errorLine)
        {
            this.MyInterestTransactionsCheckBRunResult.ErrorRowList.Add(errorLine);
            this.MyInterestTransactionsCheckBRunResult.BadAccountLineCount++;
        }


        //private List<JournalLT_GroupItem> GetNextJournal_List(ref InterestTransactionsGetNextGroupArgs getNextGroupArgs)
        //{
        //    IAccountingContext context = AccountingContext.GetContext(getNextGroupArgs.Tenant);
        //    JournalListQueryService journalListQueryService = new JournalListQueryService(context);
        //    List<JournalLT_GroupItem> j_List;

        //    j_List = journalListQueryService.GetJournals_InterestTransactionsCheck(ref getNextGroupArgs);
        //    return j_List;
        //}




        public void CheckDbIntegrity(string gLAccountId, string accountTypeCode, int _Tenant)
        {
            int _ErrorCounter = 0;
      //      var sw = Stopwatch.StartNew();

            //       using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(10)))
            //       {
            var _AccountingContext = AccountingContext.GetContext(_Tenant);

                var myGLAccountRepo = new GLAccountRepository(_AccountingContext);
                var myGLAccountMoreDataRepo = new GLAccountMoreDataRepository(_AccountingContext);

                var myInterestReportRepo = new InterestReportRepository(_AccountingContext);

                var myInterestTransactionRepository = new InterestTransactionRepository(_AccountingContext);





                ////////////////
                /// 1. Compute BalanceInLocalCurrency

                var dbAllGLAccount =
                    (from acc in myGLAccountRepo.GetAll(_Tenant)   //.GetSingle(gLAccountId, _Tenant)
                     join md in myGLAccountMoreDataRepo.GetAll(_Tenant) on acc.Id equals md.AccountId
                     where acc.Tenant == _Tenant && acc.ActiveForInterest && acc.ParentAccountId == null
                     && (accountTypeCode == null || accountTypeCode == "" || acc.AccountTypeCode == accountTypeCode)
                     select new GLAccountMDatasDTO4Tester
                     {
                         AccountId = acc.Id,
                         LocalName = acc.LocalName,
                         DisplayNumber = acc.DisplayNumber,
                         Tenant = acc.Tenant,
                         BalanceInLocalCurrency = md.BalanceInLocalCurrency,
                         //InterestOpenBalance = acc.InterestOpenBalance, // see 159870
                     }
                    );


                var dbAllDescendantsOnly =
                    (from aco in dbAllGLAccount
                     join descendants in myGLAccountRepo.GetAll(_Tenant) on aco.AccountId equals descendants.ParentAccountId
                     where aco.Tenant == _Tenant
                     select new GLAccountDescMDatasDTO4Tester()
                     {
                         ParentAccId = descendants.ParentAccountId,
                         DescAccId = descendants.Id,
                     });


                var dbAllDescendants =
                    (from aco in dbAllGLAccount
                     join descendants in myGLAccountRepo.GetAll(_Tenant) on aco.AccountId equals descendants.ParentAccountId
                     join md in myGLAccountMoreDataRepo.GetAll(_Tenant) on descendants.Id equals md.AccountId
                     into accOpenBalanceDescJoin
                     from aod in accOpenBalanceDescJoin
                     where aco.Tenant == _Tenant
                     group aod by aco.AccountId into g
                     select new GLAccountDescMDatasDTO4Tester2()
                     {
                         ParentAccId = g.Key,
                         DescBalanceInLocalCurrency = g.Sum(r => r.BalanceInLocalCurrency),
                         //DescInterestOpenBalance = g.Sum(r => r.InterestOpenBalance),
                     });

                var dbAllGLAccountInclDesc =
                     (
                        from acc in dbAllGLAccount
                        join descs in dbAllDescendants on acc.AccountId equals descs.ParentAccId
                        into accDescendantsJoin
                        from ad in accDescendantsJoin.DefaultIfEmpty()
                        select new GLAccountMDatasDTO4Tester2
                        {
                            AccountId = acc.AccountId,
                            LocalName = acc.LocalName,
                            DisplayNumber = acc.DisplayNumber,
                            Tenant = acc.Tenant,
                            BalanceInLocalCurrency = ad != null ? acc.BalanceInLocalCurrency + ad.DescBalanceInLocalCurrency : acc.BalanceInLocalCurrency,
                            //InterestOpenBalance = ad != null ? acc.InterestOpenBalance + ad.DescInterestOpenBalance : acc.InterestOpenBalance, 
                        }
                     );





                ////////////////
                /// 2. Compute InterestOpenBalances

                var calcInterestOpenBalances =
                     (
                        from intRep in
                            myInterestReportRepo.GetAll(_Tenant).Where(intRep => intRep.CreateDateTime.HasValue).OrderBy(intRep => intRep.CreateDateTime.Value).Where(intRep => (intRep.InterestReportStatusCode == "2" || intRep.InterestReportStatusCode == "4"))
                        where intRep.Tenant == _Tenant
                        join acc in dbAllGLAccountInclDesc on intRep.GLAccountId equals acc.AccountId
                        select new InterestReportDiff4Tester
                        {
                            AccountId = intRep.GLAccountId,
                            InterestOpenBalance = intRep.OpenBalance.HasValue ? intRep.OpenBalance.Value : 0m,
                            LastReportNum = intRep.ReportNumber,
                        }
                     );


                var calcInterestOpenBalancesDesc =
                     (
                        from intRep in
                            myInterestReportRepo.GetAll(_Tenant).Where(intRep => intRep.CreateDateTime.HasValue).OrderBy(intRep => intRep.CreateDateTime.Value).Where(intRep => (intRep.InterestReportStatusCode == "2" || intRep.InterestReportStatusCode == "4"))
                        where intRep.Tenant == _Tenant
                        join descendants in dbAllDescendantsOnly on intRep.GLAccountId equals descendants.DescAccId
                        select new InterestReportDiffDesc4Tester
                        {
                            ParentAccountId = descendants.ParentAccId,
                            DescInterestOpenBalance = intRep.OpenBalance.HasValue ? intRep.OpenBalance.Value : 0m,
                            LastReportNum = intRep.ReportNumber,
                        }
                     );

                var calcInterestOpenBalancesInclDesc =
                     (
                        from acc in calcInterestOpenBalances
                        join descs in calcInterestOpenBalancesDesc on acc.AccountId equals descs.ParentAccountId
                        into accDescendantsJoin
                        from ad in accDescendantsJoin.DefaultIfEmpty()
                        select new InterestReportDiff4Tester2
                        {
                            AccountId = acc.AccountId,
                            LocalName = acc.LocalName,
                            DisplayNumber = acc.DisplayNumber,
                            Tenant = acc.Tenant,
                            InterestOpenBalance = ad != null ? acc.InterestOpenBalance + ad.DescInterestOpenBalance : acc.InterestOpenBalance,
                            LastReportNum = acc.LastReportNum,
                        }
                     );





                ////////////////
                /// 3.Compute FutureInterestTrans


                var calcFutureInterestTrans =
                     (
                        from intTrans in
                            myInterestTransactionRepository.GetAll(_Tenant).Where(intTrans => intTrans.InterestValueDate >= DateTime.Today.Date)
                        where intTrans.Tenant == _Tenant
                        join acc in dbAllGLAccountInclDesc on intTrans.GLAccountId equals acc.AccountId
                        group intTrans by intTrans.GLAccountId into g
                        select new InterestReportDiff4Tester3
                        {
                            AccountId = g.Key,
                            FutureInterestTransactionsBalance = g.Sum(r => r.LocalAmount),
                        }
                     );


                var calcFutureInterestTransDesc =
                     (
                        from intTrans in
                            myInterestTransactionRepository.GetAll(_Tenant).Where(intTrans => intTrans.InterestValueDate >= DateTime.Today.Date)
                        where intTrans.Tenant == _Tenant
                        join descendants in dbAllDescendantsOnly on intTrans.GLAccountId equals descendants.DescAccId
                        group intTrans by intTrans.GLAccountId into g
                        select new InterestReportDiffDesc4Tester2
                        {
                            ParentAccountId = g.Key,
                            DescFutureInterestTransactionsBalance = g.Sum(r => r.LocalAmount),
                        }
                     );

                var calcFutureInterestTransInclDesc =
                     (
                        from acc in calcFutureInterestTrans
                        join descs in calcFutureInterestTransDesc on acc.AccountId equals descs.ParentAccountId
                        into accDescendantsJoin
                        from ad in accDescendantsJoin.DefaultIfEmpty()
                        select new InterestReportDiff4Tester4
                        {
                            AccountId = acc.AccountId,
                            LocalName = acc.LocalName,
                            DisplayNumber = acc.DisplayNumber,
                            Tenant = acc.Tenant,
                            FutureInterestTransactionsBalance = ad != null ? acc.FutureInterestTransactionsBalance + ad.DescFutureInterestTransactionsBalance : acc.FutureInterestTransactionsBalance,
                            LastReportNum = acc.LastReportNum,
                        }
                     );





                ////////////////
                /// 4. Assemble BalanceInLocalCurrency, InterestOpenBalances, FutureInterestTrans


                var qAccOpen = (
                    from acc in dbAllGLAccountInclDesc
                    join intRep in calcInterestOpenBalancesInclDesc on acc.AccountId equals intRep.AccountId
                    into accOpenBalanceJoin
                    from ao in accOpenBalanceJoin.DefaultIfEmpty()
                    where acc.Tenant == _Tenant
                    select new InterestReportDiff4Tester5
                    {
                        AccountId = acc.AccountId,
                        LocalName = acc.LocalName,
                        DisplayNumber = acc.DisplayNumber,
                        Tenant = acc.Tenant,
                        BalanceInLocalCurrency = acc.BalanceInLocalCurrency,
                        InterestOpenBalance = ao != null ? ao.InterestOpenBalance : 0m,
                        LastReportNum = ao.LastReportNum,
                    });

                var qAccOpenFuture = (
                    from aco in qAccOpen
                    join intTrans in calcFutureInterestTransInclDesc on aco.AccountId equals intTrans.AccountId
                    into accOpenBalanceFutureJoin
                    from aof in accOpenBalanceFutureJoin.DefaultIfEmpty()
                    where aco.Tenant == _Tenant
                    select new InterestReportDiff4Tester6
                    {
                        AccountId = aco.AccountId,
                        LocalName = aco.LocalName,
                        DisplayNumber = aco.DisplayNumber,
                        Tenant = aco.Tenant,
                        BalanceInLocalCurrency = aco.BalanceInLocalCurrency,
                        InterestOpenBalance = aco.InterestOpenBalance,
                        FutureInterestTransactionsBalance = aof != null ? aof.FutureInterestTransactionsBalance : 0m,
                        LastReportNum = aco.LastReportNum,
                    });


                ////////////////
                /// 5. Compute Difference



                var qDiff = (
                    from acof in qAccOpenFuture
                    where
                    (acof.InterestOpenBalance + acof.FutureInterestTransactionsBalance - acof.BalanceInLocalCurrency >= 0.001m || acof.InterestOpenBalance + acof.FutureInterestTransactionsBalance - acof.BalanceInLocalCurrency <= -0.001m)
                    select new InterestReportDiff4Tester7
                    {
                        AccountId = acof.AccountId,
                        LocalName = acof.LocalName,
                        DisplayNumber = acof.DisplayNumber,
                        Tenant = acof.Tenant,
                        BalanceInLocalCurrency = acof.BalanceInLocalCurrency,
                        InterestOpenBalance = acof.InterestOpenBalance,
                        FutureInterestTransactionsBalance = acof.FutureInterestTransactionsBalance,
                        CalculatedInterestBalance = acof.InterestOpenBalance + acof.FutureInterestTransactionsBalance,
                        Difference = acof.BalanceInLocalCurrency - acof.BalanceInLocalCurrency,
                        LastReportNum = acof.LastReportNum,
                    }
                    );

                var qThe = qDiff;

                List<InterestReportDiff4Tester7> resultingList = qThe.ToList();
                resultingList.ForEach(q => 
                { 
                    q.ErrorCounter = _ErrorCounter++;
                    this.AddErrorRowList(q.AccountId, q.DisplayNumber, q.LastReportNum, q.BalanceInLocalCurrency, q.CalculatedInterestBalance, q.Difference, "");
                });

           // }
        }



        //private class ReconciableGroup
        //{
        //    public string _Acc { get; set; }
        //    public List<JournalLineReco> _LineGroup { get; set; }
        //    public ReconciableGroup(string reference, List<JournalLineReco> lineGroup)
        //    {
        //        _LineGroup = lineGroup;
        //        _Acc = reference;
        //    }
        //}


        //private class JournalLineReco
        //{

        //    public Decimal _valueToMatch { get; set; }
        //    public JournalLine _journalLine { get; set; }
        //    public LedgerTransaction _oneLineLedger { get; set; }
        //    public JournalLineReco(JournalLine journalLine, LedgerTransaction ledgerTransaction) //, decimal actualDifference)
        //    {
        //        this._journalLine = journalLine;
        //        this._valueToMatch = 0m;
        //        if (journalLine.ActionCode == "1")
        //        {
        //            //              this._valueToMatch = journalLine.LocalAmount + (journalLine.ExternalOpenAmount ?? 0m); // because in credit lines the ExternalOpenAmount is negative 
        //            //              this._valueToMatch = -(journalLine.LocalAmount + (journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 
        //            this._valueToMatch = ledgerTransaction.OpenAmount; ///+ (journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 
        //        }
        //        else if (journalLine.ActionCode == "2")
        //        {
        //            // this._valueToMatch = -(journalLine.LocalAmount - (journalLine.ExternalOpenAmount ?? 0m));
        //            //       this._valueToMatch = journalLine.LocalAmount - (journalLine.ExternalOpenAmount ?? 0m);
        //            this._valueToMatch = ledgerTransaction.OpenAmount; //+ (journalLine.ExternalOpenAmount ?? 0m); // because in credit lines the ExternalOpenAmount is negative 
        //        }
        //        else
        //        {
        //            this._valueToMatch = ledgerTransaction.OpenAmount;
        //        }
        //        this._oneLineLedger = ledgerTransaction; // ledgerTransactionQueryService.GetByJournalLineIdAndLine(journalLine.JournalId, journalLine.Line, journalLine.Tenant);
        //        this._oneLineLedger.AmountToReconcile = _valueToMatch;
        //    }
        //}

    }
    public class InterestTransactionsCheckBArg
    {
        public int Tenant { get; set; }
        public string GLAccountId { get; set; }

        public string AccountTypeCode { get; set; }

        // public DateTime UpToDueDate { get; set; }

        public int LT_LinesMaximum { get; set; }
        public int MaxPageSize { get; set; }

        public decimal MaximalDifference { get; set; }

        public string SpecificJournalId { get; set; }

        public string LastMadeGLAccountId { get; set; }
        public int MaxGLAccountsPerQuery { get; set; }

    }

    public class InterestTransactionsCheckBRunResult
    {
        public string LastMadeGLAccountId = "";
        public long SuccessAccountLineCount = 0;
        public long BadAccountLineCount = 0;
        public List<string> ErrorRowList = new List<string>();
    }

    public class InterestReportDiff4Tester
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }

    public class InterestReportDiff4Tester2
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }


    public class InterestReportDiff4Tester3
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }


    public class InterestReportDiff4Tester4
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }


    public class InterestReportDiff4Tester5
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }


    public class InterestReportDiff4Tester6
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }


    public class InterestReportDiff4Tester7
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }




    public class InterestReportDiffDesc4Tester
    {
        public string ParentAccountId { get; set; }
        public decimal DescInterestOpenBalance { get; set; }
        public decimal DescFutureInterestTransactionsBalance { get; set; }
        public string LastReportNum { get; set; }

    }

    public class InterestReportDiffDesc4Tester2
    {
        public string ParentAccountId { get; set; }
        public decimal DescInterestOpenBalance { get; set; }
        public decimal DescFutureInterestTransactionsBalance { get; set; }
        public string LastReportNum { get; set; }

    }


    public class GLAccountDescMDatasDTO4Tester
    {
        public string ParentAccId { get; set; }
        public string DescAccId { get; set; }
        public decimal DescBalanceInLocalCurrency { get; set; }
        public decimal DescInterestOpenBalance { get; set; }
    }

    public class GLAccountDescMDatasDTO4Tester2
    {
        public string ParentAccId { get; set; }
        public string DescAccId { get; set; }
        public decimal DescBalanceInLocalCurrency { get; set; }
        public decimal DescInterestOpenBalance { get; set; }
    }



    public class GLAccountMDatasDTO4Tester
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

    }

    public class GLAccountMDatasDTO4Tester2
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

    }


}