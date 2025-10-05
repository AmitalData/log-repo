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
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Accounting.Data.EntityKeys;
using System.Runtime.Remoting.Contexts;
using Logitude.Accounting.Data.Enums;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.Customs.BL.EntityQueryServices;
using System.Windows.Forms;
using AccountingEntityValues = Logitude.Accounting.Data.Enums.AccountingEntityValues;
using System.Collections.Concurrent;
using System.Threading.Tasks;



namespace Logitude.Accounting.BL.Utils
{
    public class GLAccountInterestActivationBalanceBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;
        private string _AggregateKey;
        private List<string> _NoLines;
        private List<string> _WrongAction;
        //private List<string> _WrongSum;
        private List<string> _WrongSumToMatch;

        private const string WorksChartOfAccountTypeCode = "6";
        public const int LT_LinesMaximum_MIN = 2;
        public const int LT_LinesMaximum_MAX = 200;
        public const int MaxPageSize_MAX = 1000; 
        public const int MaxGLAccountsPerQuery_Def = 100;
        public DateTime InterestActivationDate;
        public DateTime ActionDate = DateTime.Today; // this may be not equal to args.ActionDate, when the WR runs later 
        public string LastMadeGLAccountId = "";
        public int MaxGLAccountsPerQuery = 100;
        private List<string> badList;
        private List<string> goodList;
        private List<string> madeList;
        private bool _errors = false;
        public GLAccountInterestActivationBalanceBatchResult _MyResult = new  GLAccountInterestActivationBalanceBatchResult();

        public GLAccountInterestActivationBalanceBatch()
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
        public void RunGLAccountInterestActivationBalance(GLAccountInterestActivationBalanceArgs gLAccountInterestActivationBalanceArgs)
        {
            try
            {
                int tenant = gLAccountInterestActivationBalanceArgs.Tenant;
                string myGLAccountId = gLAccountInterestActivationBalanceArgs.GLAccountId;
                string accountTypeCode = gLAccountInterestActivationBalanceArgs.AccountTypeCode;
                InterestActivationDate = gLAccountInterestActivationBalanceArgs.InterestActivationDate;
                LastMadeGLAccountId = gLAccountInterestActivationBalanceArgs.LastMadeGLAccountId;
                MaxGLAccountsPerQuery = gLAccountInterestActivationBalanceArgs.MaxGLAccountsPerQuery;
                ActionDate = gLAccountInterestActivationBalanceArgs.ActionDate;
                if (MaxGLAccountsPerQuery <= 0) MaxGLAccountsPerQuery = MaxGLAccountsPerQuery_Def;
                badList = new List<string>();
                goodList = new List<string>();
                madeList = new List<string>();
                _NoLines = new List<string>();
                _WrongAction = new List<string>();
                _WrongSumToMatch = new List<string>();

                IAccountingContext context = AccountingContext.GetContext(tenant);
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(context);
                LedgerTransactionListQueryService ledgerTransactionListQueryService = new LedgerTransactionListQueryService(context);

                GLAccountPM gLAccountPM = null;
                if (InterestActivationDate == DateTime.MinValue)
                {
                    this.AddErrorRow($"Missing GLAccount Interest Calculation Start Date");
                    _errors = true;
                }
                else if (String.IsNullOrWhiteSpace(myGLAccountId) && String.IsNullOrWhiteSpace(accountTypeCode))
                {
                    this.AddErrorRow($"GLAccount Id is empty");
                    _errors = true;
                }
                else if(!String.IsNullOrWhiteSpace(myGLAccountId))
                {
                    gLAccountPM = gLAccountQueryService.GetSinglePM(myGLAccountId, tenant);
                    gLAccountQueryService.GetComposition(new GLAccountKeys() { Id = myGLAccountId }, gLAccountPM);
                    if (gLAccountPM == null)
                    {
                        this.AddErrorRow($"GLAccount id={gLAccountPM} is not found in tenant {tenant}");
                        _errors = true;
                    }
                    else if (gLAccountPM.IsControlAccount.HasValue && gLAccountPM.IsControlAccount.Value == true)
                    {
                        this.AddErrorRow($"GLAccount id={gLAccountPM} display={gLAccountPM.DisplayNumber} is a control account");
                        _errors = true;
                    }

                }

                if (!_errors)
                {
                    if (String.IsNullOrEmpty(myGLAccountId))
                    {
                        // single or parent. (must be multi-currency)
                        if (gLAccountPM.ParentAccountId == null)
                        {
                            List<string> gLAccountIdList;
                            gLAccountIdList = gLAccountQueryService.GetNextGLAccountIdByTypeControlNoParent(tenant, accountTypeCode, false, LastMadeGLAccountId, MaxGLAccountsPerQuery);

                            if (gLAccountIdList != null && gLAccountIdList.Any())
                            {
                                _MyResult.LastMadeGLAccountId = gLAccountIdList.Last();
                                gLAccountIdList.ForEach(accId =>
                                {
                                    ActivationBalanceCalculation(accId, null, gLAccountInterestActivationBalanceArgs.InterestActivationDate, ActionDate, tenant, gLAccountQueryService);
                                });
                            }
                        }
                        else
                        {
                            // descendant. (must not be multi-currency)
                            List<string> gLAccountIdList;
                            gLAccountIdList = gLAccountQueryService.GetNextGLAccountIdByTypeControlDescendant(tenant, accountTypeCode, false, LastMadeGLAccountId, MaxGLAccountsPerQuery);

                            if (gLAccountIdList != null && gLAccountIdList.Any())
                            {
                                _MyResult.LastMadeGLAccountId = gLAccountIdList.Last();
                                gLAccountIdList.ForEach(accId =>
                                {
                                    ActivationBalanceCalculation(accId, null, gLAccountInterestActivationBalanceArgs.InterestActivationDate, ActionDate, tenant, gLAccountQueryService);
                                });
                            }
                        }
                    }
                    else
                    {
                        ActivationBalanceCalculation(myGLAccountId, gLAccountPM, gLAccountInterestActivationBalanceArgs.InterestActivationDate, ActionDate, tenant, gLAccountQueryService);
                    }
                }
                
                _ResponseText = $"Good: {_MyResult.SuccessAccountLineCount}, Bad: {_MyResult.BadAccountLineCount}, " +
                                $"{Environment.NewLine}Details: {_MyResult.MadeListText}" +
                                $"{Environment.NewLine}{(_MyResult.ErrorRowList?.Any() == true ? string.Join(Environment.NewLine, _MyResult.ErrorRowList) : "No errors")}";
            }

            catch //(Exception e)
            {
                throw; // new Exception($"InterestTransactionsCheckBRun failure {e.Message} Inner Exception: {e.InnerException.Message}", e);
            }
        }


        private void AddErrorRowList(string gLAccount_id, string displayNumber, string lastReportNum,
            decimal glaccblz, decimal descblz, decimal accBlz,
            decimal repBlz,
            decimal before, string startdate, decimal inbetween, string repdate,
            decimal glaccftr, decimal descftr,
            decimal calc, decimal diff, string message)
        {
            string sep = ";";
            this.AddErrorRow($"{gLAccount_id}{sep}{displayNumber}{sep}{lastReportNum}{sep}{glaccblz}{sep}{descblz}{sep}{accBlz}{sep}{repBlz}{sep}{before}{sep}{startdate}{sep}{inbetween}{sep}{repdate}{sep}{glaccftr}{sep}{descftr}{sep}{calc}{sep}{diff}{sep}{message}");
        }


        private void AddErrorRow(String errorLine)
        {
            this._MyResult.ErrorRowList.Add(errorLine);
            this._MyResult.BadAccountLineCount++;
        }

        public void ActivationBalanceCalculation(string gLAccountId, GLAccountPM accPM, DateTime interestActivationDate, DateTime actionDate, int _Tenant, GLAccountQueryService gLAccountQueryService)
        {
            const string OPEN_ = "OPEN_";
            IAccountingContext context = AccountingContext.GetContext(_Tenant);
            try
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(3)))
                {
                    try
                    {
                        _AggregateKey = "ActivationBalanceCalculation-" + gLAccountId; // VarChar 128 
                        LockIt(_Tenant);
                    }
                    catch (Exception eee)
                    {
                        AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Information);

                        throw;
                    }
                    GLAccountUpdateService gLAccountUpdateService = new GLAccountUpdateService(context, new Dictionary<string, IContext>(), _Tenant);
                    GLAccountCurrencyRepository gLAccountCurrencyRepository = new GLAccountCurrencyRepository(context); 
                    List<GLAccountCurrency> currencyAccounts = gLAccountCurrencyRepository.GetRelatedCurrenciesAccountByCustomerGLAccountAll(_Tenant, gLAccountId);
                    HashSet<string> concernedGLAccounts = new HashSet<string> { gLAccountId };
                    concernedGLAccounts.UnionWith(currencyAccounts.Select(x => x.GLAccountId));


                    InterestTransactionRepository myInterestTransactionRepository = new InterestTransactionRepository(context);
                    InterestTransactionQueryService myInterestTransactionService = new InterestTransactionQueryService(context);
                    InterestTransactionUpdateService myInterestTransactionUpdateService = new InterestTransactionUpdateService(context, new Dictionary<string, IContext>(), _Tenant);

                    var (amount_backward, backwardList) = ComputeValueAfter_AccDateBefore(context, _Tenant, concernedGLAccounts,
                            interestActivationDate, myInterestTransactionRepository, false);

                    if (ReportExists(concernedGLAccounts, interestActivationDate, context, _Tenant))
                    {
                        ////////////
                        /// 2.7. Compute our InterestReportId 
                        string FormatReportId(DateTime date) => OPEN_ + date.ToString("ddMMyyyy");
                        string ourInterestReportIdPre = OPEN_ + FormatReportId(actionDate);

                        ////////////
                        /// 3. Compute "Before" that are still open (to "close" them with "OPEN_")

                        var calcBeforeInterestTransPre =
                                 (
                                    from intTrans in myInterestTransactionRepository.GetAll(_Tenant)
                                        .Where(intTrans => concernedGLAccounts.Contains(intTrans.GLAccountId)
                                        && !intTrans.IsClosed
                                        && intTrans.InterestValueDate < interestActivationDate
                                        && (intTrans.InterestReportId == null || intTrans.InterestReportId == "null"))
                                    select new InterestTransactionBefore
                                    {
                                        Tenant = intTrans.Tenant,
                                        Id = intTrans.Id,
                                        LocalAmount = intTrans.LocalAmount,
                                    }
                                 );
                        decimal amount_beforePre = 0m;
                        List<InterestTransactionBefore> before_listPre = calcBeforeInterestTransPre != null ? calcBeforeInterestTransPre.ToList() : new List<InterestTransactionBefore>();

                        before_listPre.RemoveAll(item => backwardList.Any(b => b.Id == item.Id));
                        if (before_listPre != null && before_listPre.Any())
                        {
                            decimal? amount_before_qm = before_listPre.Sum(c => c.LocalAmount);
                            amount_beforePre = amount_before_qm.HasValue ? amount_before_qm.Value : 0m;
                        }



                        ////////////////
                        /// 5.Update InterestTransaction
                        if (before_listPre != null && before_listPre.Any())
                        {
                            UpdateInterestTransactions(
                                before_listPre, true,
                                myInterestTransactionService,
                                myInterestTransactionUpdateService,
                                ourInterestReportIdPre);
                        }
                    }



                    ClearPreviousActivation(gLAccountId, concernedGLAccounts, myInterestTransactionRepository, myInterestTransactionService, myInterestTransactionUpdateService, gLAccountQueryService, gLAccountUpdateService, OPEN_, _Tenant);

                    var myGLAccountRepo = new GLAccountRepository(context);

                    var myJournalRepository = new JournalRepository(context);
                    var myJournalLineRepository = new JournalLineRepository(context);

                    TenantQuery tenantQuery = new TenantQuery(_Tenant);
                    TenantPM tPM = tenantQuery.GetSinglePM(_Tenant);
                    string accountingCurrencyId = tPM.CurrencyId;


                    ///// 1. Compute BalanceInLocalCurrency

                    Decimal balanceInLocalCurrency = 0m;
                    var partialResults = new List<decimal>();



                    var totalsByAccount = gLAccountQueryService
                            .GetCurrencyBalancesByIdsV2(concernedGLAccounts, interestActivationDate, _Tenant);

                    foreach (var acc in concernedGLAccounts)
                    {
                        if (totalsByAccount.TryGetValue(acc, out var balances))
                        {
                            foreach (var bal in balances)
                            {
                                partialResults.Add(bal.LocalAmount ?? 0m);
                            }
                        }
                        else
                        {
                            partialResults.Add(0m); // if no rows for that account
                        }
                    }


                    balanceInLocalCurrency = partialResults.Sum();

                    


                    ////////////
                    /// 2.7. Compute our InterestReportId 
                    string actionDateStr = actionDate.ToString("dd.MM.yyyy").Replace(".", String.Empty);
                    string ourInterestReportId = OPEN_ + actionDateStr;




                    var (amount_after, forwardList) = ComputeValueAfter_AccDateBefore(context, _Tenant, concernedGLAccounts,
                            interestActivationDate, myInterestTransactionRepository, true);







                    ////////////
                    /// 3. Compute "Before" that are still open (to "close" them with "OPEN_")

                    var calcBeforeInterestTrans =
                                 (
                                    from intTrans in myInterestTransactionRepository.GetAll(_Tenant)
                                        .Where(intTrans => concernedGLAccounts.Contains(intTrans.GLAccountId)
                                        && !intTrans.IsClosed
                                        && intTrans.InterestValueDate < interestActivationDate)
                                    select new InterestTransactionBefore
                                    {
                                        Tenant = intTrans.Tenant,
                                        Id = intTrans.Id,
                                        LocalAmount = intTrans.LocalAmount,
                                    }
                                 );

                        List<InterestTransactionBefore> before_list = calcBeforeInterestTrans != null ? calcBeforeInterestTrans.ToList() : new List<InterestTransactionBefore>();



                        ////////////////
                        /// 4.Compute interestOpenBalance
                        decimal interestOpenBalance = balanceInLocalCurrency - amount_after + amount_backward ; // nis

                    
                        before_list.RemoveAll(item => backwardList.Any(b => b.Id == item.Id));


                    ////////////////
                    /// 5.Update InterestTransaction
                    if (before_list != null && before_list.Any())
                    {
                        UpdateInterestTransactions(
                                before_list, true,
                                myInterestTransactionService,
                                myInterestTransactionUpdateService,
                                ourInterestReportId);
                    }

                        ////////////////
                        /// 6.Update GLAccount.InterestOpenBalance and create new InterestTransaction


                        GLAccountPM gLAccountPM = accPM != null ? accPM : gLAccountQueryService.GetSinglePM(gLAccountId, _Tenant);
                        if (accPM == null) gLAccountQueryService.GetComposition(new GLAccountKeys() { Id = gLAccountId }, gLAccountPM);
                        if (gLAccountPM != null)
                        {
                            gLAccountPM.InterestOpenBalance = interestOpenBalance;
                            gLAccountPM.ChangeSetOp = ChangeSetOperation.Update;
                            gLAccountUpdateService.Update(gLAccountPM, true);
                        // Add to MadeList
                        _MyResult.MadeList.Add(new MadeListItem
                        {
                            DisplayNumber = gLAccountPM.DisplayNumber,
                            BalanceInLocalCurrency = balanceInLocalCurrency,
                            AmountAfter = amount_after,
                            AmountBackward = amount_backward,
                            InterestOpenBalance = interestOpenBalance
                        });
                    }




                    if (!String.IsNullOrEmpty(_AggregateKey))
                    {
                        TryDeleteLockRow(_Tenant);
                    }
                    scope.Complete();
                    _MyResult.SuccessAccountLineCount++;
                }//using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(3)))
            }
            catch (Exception e)
            {
                using (TransactionScope excScope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(1)))
                {
                    {
                        string errorMessage = e.Message.Split(new[] { '\r', '\n' }).FirstOrDefault();
                        _ResponseText = errorMessage;
                        _StatusCode = HttpStatusCode.InternalServerError;
                    }
                    if (!String.IsNullOrEmpty(_AggregateKey))
                    {
                        TryDeleteLockRow(_Tenant);
                    }
                    excScope.Complete();
                }

                var rootEx = e.GetBaseException();
                string message = rootEx.Message;
                string stackTrace = rootEx.StackTrace;

                _MyResult.BadAccountLineCount++;
                AddErrorRow($"Unexpected error: {message}");

                NetCommonHelper.Logger.DevLog.Instance.WriteError(
                    $"[ActivationBalance] Unexpected error occurred. " +
                    $"Message: {message} | StackTrace: {stackTrace}"
                );
            }
        }

        private void UpdateInterestTransactions(
                        List<InterestTransactionBefore> transactions, bool isClosedNewValue,
                        InterestTransactionQueryService myInterestTransactionService,
                        InterestTransactionUpdateService myInterestTransactionUpdateService,
                        string interestReportId)
        {
            int total_count = transactions.Count;
            int count = 0;
            int each = 100;

            transactions.ForEach(intt =>
            {
                count++;
                bool commit = (count >= total_count || count % each == 0);
                InterestTransactionPM interestTransactionPM = myInterestTransactionService.GetSingle(intt.Id, false, false);
                if (interestTransactionPM != null)
                {
                    interestTransactionPM.IsClosed = isClosedNewValue;
                    interestTransactionPM.ChangeSetOp = ChangeSetOperation.Update;
                    interestTransactionPM.InterestReportId = interestReportId;
                    myInterestTransactionUpdateService.Update(interestTransactionPM, commit);
                }
            });
        }


        private (decimal amountAfter, List<InterestTransactionBefore> resultingList) ComputeValueAfter_AccDateBefore(IAccountingContext context, int tenant, HashSet<string> concernedGLAccounts,
                        DateTime interestActivationDate, InterestTransactionRepository myInterestTransactionRepository, bool forward) // or else "backward"
        // forward:  value >= interestActivationDate, reg date <  interestActivationDate
        // backward: value <  interestActivationDate, reg date >= interestActivationDate
        {
            var journalAccEntityCodes = new HashSet<string>
                    {
                        AccountingEntityValues.Journal,
                        AccountingEntityValues.Adjustment,
                        AccountingEntityValues.BankAdjustment
                    };

            var calcAfterInterestTrans =
                from intTrans in myInterestTransactionRepository.GetAll(tenant)
                    .Where(intTrans => concernedGLAccounts.Contains(intTrans.GLAccountId)
                           && !intTrans.IsClosed && !intTrans.IsCancelled
                           && (forward ? intTrans.InterestValueDate >= interestActivationDate : intTrans.InterestValueDate < interestActivationDate)) //  forward: >= interestActivationDate, backward:  < interestActivationDate 

                    // Condition only matched for journalAccEntityCodes
                from j in context.Journals.Where(r => r.Tenant == tenant)
                    .Where(jrec => journalAccEntityCodes.Contains(intTrans.AccountingEntityCode)
                        && jrec.Id == intTrans.EntityId)
                    .DefaultIfEmpty()

                from jl in context.JournalLines.Where(r => r.Tenant == tenant)
                    .Where(jlrec => journalAccEntityCodes.Contains(intTrans.AccountingEntityCode)
                             && jlrec.JournalId == intTrans.EntityId
                             && jlrec.Line == intTrans.OriginalEntityLineNumber)
                    .DefaultIfEmpty()

                where // At least one of the following conditions is true:
                      // 1) Code not in journalAccEntityCodes
                    !journalAccEntityCodes.Contains(intTrans.AccountingEntityCode)

                    // 2) Journal line matched & its AccountingDate < interestActivationDate (as forward) or >= interestActivationDate (as backward)
                    || (jl != null && (forward ? jl.AccountingDate < interestActivationDate : jl.AccountingDate >= interestActivationDate))

                    // 3) No journal line matched & journal’s AccountingDate < interestActivationDate (as forward) or >= interestActivationDate (as backward)
                    || (jl == null && j != null && (forward ? j.AccountingDate < interestActivationDate : j.AccountingDate >= interestActivationDate))   

                select new InterestTransactionBefore
                {
                    Tenant = intTrans.Tenant,
                    Id = intTrans.Id,
                    LocalAmount = intTrans.LocalAmount,
                    EntityId = intTrans.EntityId,
                    AccountingEntityCode = intTrans.AccountingEntityCode,
                };

            List<InterestTransactionBefore> after_list =
                calcAfterInterestTrans?.ToList() ?? new List<InterestTransactionBefore>();

            decimal amount_after = 0m;

            var journalTransactions = new List<InterestTransactionBefore>();
            var aRInvoiceTransactions = new List<InterestTransactionBefore>();
            var aRPaymentTransactions = new List<InterestTransactionBefore>();

            if (after_list.Any())
            {
                journalTransactions = after_list
                    .Where(c => journalAccEntityCodes.Contains(c.AccountingEntityCode)).ToList();
                if (journalTransactions.Any())
                {
                    amount_after += journalTransactions.Sum(c => c.LocalAmount);
                }

                aRInvoiceTransactions =
                    after_list.Where(c => c.AccountingEntityCode == AccountingEntityValues.ARInvoice).ToList();

                aRPaymentTransactions =
                    after_list.Where(c => c.AccountingEntityCode == AccountingEntityValues.ARPayment).ToList();

                if (aRInvoiceTransactions.Any() || aRPaymentTransactions.Any())
                {
                    IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);

                    if (aRInvoiceTransactions.Any())
                    {
                        var aRInvoiceRepository = new ARInvoiceRepository(invoiceContext);
                        var invoiceIds = aRInvoiceTransactions.Select(x => x.EntityId).Distinct().ToHashSet();
                        var preexistingInvoiceIds = aRInvoiceRepository.GetInvoices() // if !forward, consider them as "newly arrived invoices"
                            .Where(inv => invoiceIds.Contains(inv.Id)
                                          && (forward ? inv.InvoiceDate < interestActivationDate : inv.InvoiceDate >= interestActivationDate))
                            .Select(inv => inv.Id)
                            .ToHashSet();

                        aRInvoiceTransactions = aRInvoiceTransactions
                            .Where(x => preexistingInvoiceIds.Contains(x.EntityId))
                            .ToList();

                        if (aRInvoiceTransactions.Any())
                        {
                            amount_after += aRInvoiceTransactions.Sum(c => c.LocalAmount);
                        }
                    }

                    if (aRPaymentTransactions.Any())
                    {
                        var aRPaymentRepository = new ARPaymentRepository(invoiceContext);
                        var paymentIds = aRPaymentTransactions.Select(x => x.EntityId).Distinct().ToHashSet();
                        var preexistingPaymentIds = aRPaymentRepository.GetARPayments() // if !forward, consider them as "newly arrived payments"
                            .Where(pmt => paymentIds.Contains(pmt.Id)
                                          && (forward ? pmt.RegisterDate < interestActivationDate : pmt.RegisterDate >= interestActivationDate))
                            .Select(pmt => pmt.Id)
                            .ToHashSet();

                        aRPaymentTransactions = aRPaymentTransactions
                            .Where(x => preexistingPaymentIds.Contains(x.EntityId))
                            .ToList();

                        if (aRPaymentTransactions.Any())
                        {
                            amount_after += aRPaymentTransactions.Sum(c => c.LocalAmount);
                        }
                    }
                }
            }
            // Create union list of all three
            var resultingList = journalTransactions
            .Concat(aRInvoiceTransactions)
            .Concat(aRPaymentTransactions).ToList();

            return (amount_after, resultingList);
        }




        private void ClearPreviousActivation(string mainGLAccount, HashSet<string> concernedGLAccounts, InterestTransactionRepository myInterestTransactionRepository, 
            InterestTransactionQueryService myInterestTransactionService, InterestTransactionUpdateService myInterestTransactionUpdateService,
            GLAccountQueryService gLAccountQueryService, GLAccountUpdateService gLAccountUpdateService, string open_, int tenant)
        {

            var calcBeforeInterestTrans =
                     (
                        from intTrans in myInterestTransactionRepository.GetAll(tenant)
                            .Where(intTrans => concernedGLAccounts.Contains(intTrans.GLAccountId)
                            && intTrans.IsClosed
                            && intTrans.InterestReportId.StartsWith(open_))
                        select new InterestTransactionBefore
                        {
                            Tenant = intTrans.Tenant,
                            Id = intTrans.Id,
                            LocalAmount = intTrans.LocalAmount,
                        }
                     );

            List<InterestTransactionBefore> before_list = calcBeforeInterestTrans.ToList();



            if (before_list != null &&  before_list.Any()) 
            {
                UpdateInterestTransactions(
                            before_list, false,
                            myInterestTransactionService,
                            myInterestTransactionUpdateService, null);
            }



            GLAccountPM gLAccountPM = gLAccountQueryService.GetSinglePM(mainGLAccount, tenant);
            gLAccountQueryService.GetComposition(new GLAccountKeys() { Id = mainGLAccount }, gLAccountPM);
            if (gLAccountPM != null)
            {
                gLAccountPM.InterestOpenBalance = null;
                gLAccountPM.ChangeSetOp = ChangeSetOperation.Update;
                gLAccountUpdateService.Update(gLAccountPM, true);
            }

        }
        private bool ReportExists(HashSet<string> concernedGLAccounts, DateTime interestActivationDate, IAccountingContext context, int tenant)
        {
            InterestReportRepository myInterestReportRepository = new InterestReportRepository(context);
            var q = from rep in myInterestReportRepository.GetAll(tenant)
                        .Where(rec => concernedGLAccounts.Contains(rec.GLAccountId) 
                        && rec.InterestReportStatusCode != InterestReportStatusCodes.Cancelled
                        && rec.InterestCalculationDate <=interestActivationDate) select rep.Id; 
            return q.Any();
        }

        private void TryDeleteLockRow(int tenant)
        {
            //GeneralLock
            {
                try
                {

                    var repo = new GeneralLockRepository(tenant);

                    repo.FastDelete(_AggregateKey, tenant);
                    _AggregateKey = "";
                }
                catch (Exception e)
                {
                    throw (e);
                }
            }
        }

        private void LockIt(int tenant)
        {
            var repo = new GeneralLockRepository(tenant);

            var lockPoco = repo.GetSingleGeneralLockNOWAIT(_AggregateKey, tenant);
            if (lockPoco == null)
            {

                repo.Add(new GeneralLock()
                {
                    Tenant = tenant,
                    GeneralKey = _AggregateKey,
                    CreatedAt = TenantServerConfigration.GetCurrentDateTime(tenant)
                });
                //  _logger.AppendLine("add GeneralLock");
                repo.SubmitChanges();

                lockPoco = repo.GetSingleGeneralLockNOWAIT(_AggregateKey, tenant);
            }

            if (lockPoco == null)
            {
                throw new Exception("lockPoco ==null");
            }
            else
            {
                //  _logger.AppendLine("Lock it ");
            }

        }





    }




    public class InterestTransactionBefore
    {
        public int Tenant { get; set; }

        public string Id { get; set; }
        public string EntityId { get; set; }
        public string AccountingEntityCode { get; set; }
        public decimal LocalAmount { get; set; }

    }

    public class GLAccountInterestActivationBalanceArgs
    {
        public int Tenant { get; set; }

        public string GLAccountId { get; set; }

        public string AccountTypeCode { get; set; }

        public DateTime InterestActivationDate { get; set; }

        public int BatchIt { get; set; }

        public string LastMadeGLAccountId { get; set; }

        public int MaxGLAccountsPerQuery { get; set; }
        public string CommunicationLogId { get; set; }

        public DateTime ActionDate { get; set; }

    }


    public class GLAccountInterestActivationBalanceBatchResult
    {
        public string LastMadeGLAccountId = "";
        public long SuccessAccountLineCount = 0;
        public long BadAccountLineCount = 0;
        public List<string> ErrorRowList = new List<string>();
        public List<MadeListItem> MadeList = new List<MadeListItem>();

        public string MadeListText
        {
            get
            {
                return string.Join("\n", MadeList.Select(item => $"DisplayNumber: {item.DisplayNumber}, BalanceInLocalCurrency: {item.BalanceInLocalCurrency}, AmountAfter: {item.AmountAfter}, AmountBackward: {item.AmountBackward}, InterestOpenBalance: {item.InterestOpenBalance}"));
            }
        }
    }

    public class MadeListItem
    {
        public string DisplayNumber { get; set; }
        public decimal BalanceInLocalCurrency { get; set; }
        public decimal AmountAfter { get; set; }
        public decimal InterestOpenBalance { get; set; }
        public decimal AmountBackward { get; set; }
    }


}
