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
using Logitude.Accounting.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
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
                if (String.IsNullOrWhiteSpace(myGLAccountId) && String.IsNullOrWhiteSpace(accountTypeCode))
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
                    //else if (gLAccountPM.ParentAccountId != null)
                    //{
                    //    this.AddErrorRow($"GLAccount id={gLAccountPM} display={gLAccountPM.DisplayNumber} is a descendant of {gLAccountPM.ParentAccountId}");
                    //    _errors = true;
                    //}
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

                            if (gLAccountIdList != null && gLAccountIdList.Count > 0)
                            {
                                _MyResult.LastMadeGLAccountId = gLAccountIdList.Last();
                                gLAccountIdList.ForEach(accId =>
                                {
                                    ActivationBalanceCalculation(accId, gLAccountInterestActivationBalanceArgs.InterestActivationDate, ActionDate, tenant);
                                });
                            }
                        }
                        else
                        {
                            // descendant. (must not be multi-currency)
                            List<string> gLAccountIdList;
                            gLAccountIdList = gLAccountQueryService.GetNextGLAccountIdByTypeControlDescendant(tenant, accountTypeCode, false, LastMadeGLAccountId, MaxGLAccountsPerQuery);

                            if (gLAccountIdList != null && gLAccountIdList.Count > 0)
                            {
                                _MyResult.LastMadeGLAccountId = gLAccountIdList.Last();
                                gLAccountIdList.ForEach(accId =>
                                {
                                    ActivationBalanceCalculation(accId, gLAccountInterestActivationBalanceArgs.InterestActivationDate, ActionDate, tenant);
                                });
                            }
                        }
                    }
                    else
                    {
                        ActivationBalanceCalculation(myGLAccountId, gLAccountInterestActivationBalanceArgs.InterestActivationDate, ActionDate, tenant);
                    }
                }

                _ResponseText = $"Good: {_MyResult.SuccessAccountLineCount},  Bad: {_MyResult.BadAccountLineCount}, \n  Lines: \n{String.Join("\n", _MyResult.ErrorRowList.ToArray())}";

            }

            catch (Exception e)
            {
                throw new Exception($"InterestTransactionsCheckBRun failure {e.Message} Inner Exception: {e.InnerException.Message}", e);
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





        public void ActivationBalanceCalculation(string gLAccountId, DateTime interestActivationDate, DateTime actionDate, int _Tenant)
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

                    GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(context);
                    InterestTransactionRepository myInterestTransactionRepository = new InterestTransactionRepository(context);
                    InterestTransactionQueryService myInterestTransactionService = new InterestTransactionQueryService(context);
                    InterestTransactionUpdateService myInterestTransactionUpdateService = new InterestTransactionUpdateService(context, new Dictionary<string, IContext>(), _Tenant);

                    if (!ReportExists(gLAccountId, context, _Tenant))
                    {


                        ClearPreviousActivation(gLAccountId, myInterestTransactionRepository, myInterestTransactionService, myInterestTransactionUpdateService, gLAccountQueryService, gLAccountUpdateService, OPEN_, _Tenant);



                        var myGLAccountRepo = new GLAccountRepository(context);
                        // var myGLAccountMoreDataRepo = new GLAccountMoreDataRepository(context);

                        //    var myInterestReportRepo = new InterestReportRepository(context);

                        var myJournalRepository = new JournalRepository(context);

                        TenantQuery tenantQuery = new TenantQuery(_Tenant);
                        TenantPM tPM = tenantQuery.GetSinglePM(_Tenant);
                        string accountingCurrencyId = tPM.CurrencyId;

                        //////////////// 
                        ///// 1. Compute BalanceInLocalCurrency
                        //string totalDateType = "1"; // accounting date 
                        //bool SumOpenTransactions = false;
                        //bool IncludeRelatedCurrenciesAccount = false;
                        //AccountBalanceByDateCodeService ac = new AccountBalanceByDateCodeService(null, _Tenant, gLAccountId, null);
                        //ac.ReSetAccountList(false, IncludeRelatedCurrenciesAccount);
                        //bool openBalancePlease_ReCalcYearTransfer = //true;//Yaron said this is Default !!!
                        //    (interestActivationDate.Day == 1 && interestActivationDate.Month == 1);
                        //ac.CalculateBalance(
                        //    openBalancePlease_ReCalcYearTransfer,
                        //    totalDateType, interestActivationDate, false, true, false,
                        //    false, SumOpenTransactions);

                        //ac.AccountBalance.LogMessage = null;


                        //decimal? balance_qm = ac.AccountBalance.GetBalanceOfLocalAmount().GetValueOrDefault();
                        //decimal balance_on_act_date = balance_qm.HasValue ? balance_qm.Value : 0m;


                        ///// 1. Compute future AMITAL BalanceInLocalCurrency
                        var myLedgerTransactionRepository = new LedgerTransactionRepository(context);
                        var qLedgerTrans =

                                    from lt in myLedgerTransactionRepository.GetAll(_Tenant)
                                        .Where(ltrec => ltrec.AccountId == gLAccountId)
                                    join j in myJournalRepository.GetAll(_Tenant).Where(jrec => jrec.ExternalSystem == "AMITAL")
                                    on lt.JournalId equals j.Id
                                    select
                                     lt.LocalAmountDebit - lt.LocalAmountCredit;

                        decimal ledgerTransAmt = 0m;
                        if (qLedgerTrans != null)
                        {
                            List<Decimal> ledgerTransAmounts = qLedgerTrans.ToList();
                            if (ledgerTransAmounts.Count > 0)    ledgerTransAmt = ledgerTransAmounts.Sum();
                        }


                        ////////////
                        /// 2.7. Compute our InterestReportId 
                        string actionDateStr = actionDate.ToString("dd.MM.yyyy").Replace(".", String.Empty);
                        string ourInterestReportId = OPEN_ + actionDateStr;


                        ////////////
                        /// 2. Compute "After" interestActivationDate, that are still open 

                        var calcAfterInterestTrans =
                                  (
                                     from intTrans in myInterestTransactionRepository.GetAll(_Tenant)
                                         .Where(intTrans => intTrans.GLAccountId == gLAccountId
                                         && !intTrans.IsClosed
                                         && intTrans.InterestValueDate >= interestActivationDate
                                         && intTrans.InterestEntityTypeCode == "3")
                                     join j in myJournalRepository.GetAll(_Tenant).Where(jrec => jrec.ExternalSystem == "AMITAL")
                                     on intTrans.EntityId equals j.Id
                                     select new InterestTransactionBefore
                                     {
                                         Tenant = intTrans.Tenant,
                                         Id = intTrans.Id,
                                         LocalAmount = intTrans.LocalAmount,
                                     }
                                  );
                        decimal amount_after = 0m;
                        List<InterestTransactionBefore> after_list = calcAfterInterestTrans.ToList();
                        if (after_list != null && after_list.Count > 0)
                        {
                            decimal? amount_after_qm = after_list.Select(c => c.LocalAmount).Sum();
                            amount_after = amount_after_qm.HasValue ? amount_after_qm.Value : 0m;
                        }



                        ////////////
                        /// 3. Compute "Before" that are still open (to "close" them with "OPEN_")

                        var calcBeforeInterestTrans =
                                 (
                                    from intTrans in myInterestTransactionRepository.GetAll(_Tenant)
                                        .Where(intTrans => intTrans.GLAccountId == gLAccountId
                                        && !intTrans.IsClosed
                                        && intTrans.InterestValueDate < interestActivationDate)
                                    select new InterestTransactionBefore
                                    {
                                        Tenant = intTrans.Tenant,
                                        Id = intTrans.Id,
                                        LocalAmount = intTrans.LocalAmount,
                                    }
                                 );
                        decimal amount_before = 0m;
                        List<InterestTransactionBefore> before_list = calcBeforeInterestTrans.ToList();
                        if (before_list != null && before_list.Count > 0)
                        {
                            decimal? amount_before_qm = before_list.Select(c => c.LocalAmount).Sum();
                            amount_before = amount_before_qm.HasValue ? amount_before_qm.Value : 0m;
                        }


                        ////////////////
                        /// 4.Compute interestOpenBalance
                        decimal interestOpenBalance = ledgerTransAmt - amount_after + amount_before; // nis




                        ////////////////
                        /// 5.Update InterestTransaction
                        if (before_list != null && before_list.Count > 0)
                        {
                            int total_count = before_list.Count;
                            int count = 0;
                            int each = 100;

                            before_list.ForEach(intt =>
                                {
                                    count++;
                                    bool commit = (count >= total_count || count % each == 0);
                                    InterestTransactionPM interestTransactionPM = myInterestTransactionService.GetSingle(intt.Id, false, false);
                                    if (interestTransactionPM != null)
                                    {
                                        interestTransactionPM.IsClosed = true;
                                        interestTransactionPM.ChangeSetOp = ChangeSetOperation.Update;
                                        interestTransactionPM.InterestReportId = ourInterestReportId;
                                        myInterestTransactionUpdateService.Update(interestTransactionPM, commit);
                                    }
                                }
                                );
                        }



                        ////////////////
                        /// 6.Update GLAccount.InterestOpenBalance and create new InterestTransaction


                        GLAccountPM gLAccountPM = gLAccountQueryService.GetSinglePM(gLAccountId, _Tenant);
                        gLAccountQueryService.GetComposition(new GLAccountKeys() { Id = gLAccountId }, gLAccountPM);
                        if (gLAccountPM != null)
                        {
                            //if (!String.IsNullOrWhiteSpace(gLAccountPM.ParentAccountId))
                            //{
                            //    InterestTransactionPM interestTransaction = new InterestTransactionPM()
                            //    {
                            //        InterestEntityTypeCode = "4", // Open Balance 
                            //        EntityId = gLAccountPM.Id,
                            //        AccountingEntityCode = "1", // GLAccount ?
                            //        OriginalEntityLineNumber = 1,
                            //        LocalAmount = interestOpenBalance,
                            //        GLAccountId = gLAccountPM.ParentAccountId,
                            //        ForeignAmount = interestOpenBalance,
                            //        InterestValueDate = actionDate,
                            //        Tenant = _Tenant,
                            //        ChangeSetOp = ChangeSetOperation.Insert,
                            //        CurrencyId = accountingCurrencyId,  // NIS
                            //    };
                            //    IInterestTransactionUpdateServiceExt interestTransactionUpdateService = ContainerAccessor.Container.Resolve(typeof(IInterestTransactionUpdateServiceExt), "InterestTransactionUpdateServiceExt", new ParameterOverride("", 1)) as IInterestTransactionUpdateServiceExt;
                            //    interestTransactionUpdateService.Create(interestTransaction);

                            //}


                            gLAccountPM.InterestOpenBalance = interestOpenBalance;
                            gLAccountPM.ChangeSetOp = ChangeSetOperation.Update;
                            gLAccountUpdateService.Update(gLAccountPM, true);

                        }

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
                        //       UpdateARPaymentChequeStatus(id, tenant, "2", context);
                    }
                    if (!String.IsNullOrEmpty(_AggregateKey))
                    {
                        TryDeleteLockRow(_Tenant);
                    }
                    excScope.Complete();
                }

            }

            // }
        }


        private void ClearPreviousActivation(string gLAccountId, InterestTransactionRepository myInterestTransactionRepository, 
            InterestTransactionQueryService myInterestTransactionService, InterestTransactionUpdateService myInterestTransactionUpdateService,
            GLAccountQueryService gLAccountQueryService, GLAccountUpdateService gLAccountUpdateService, string open_, int tenant)
        {

            var calcBeforeInterestTrans =
                     (
                        from intTrans in myInterestTransactionRepository.GetAll(tenant)
                            .Where(intTrans => intTrans.GLAccountId == gLAccountId
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



            if (before_list != null &&  before_list.Count > 0) 
            {
                int total_count = before_list.Count;
                int count = 0;
                int each = 100;

                before_list.ForEach(intt =>
                {
                    count++;
                    bool commit = (count >= total_count || count % each == 0);
                    InterestTransactionPM interestTransactionPM = myInterestTransactionService.GetSingle(intt.Id, false, false);
                    if (interestTransactionPM != null)
                    {
                        interestTransactionPM.IsClosed = false;
                        interestTransactionPM.ChangeSetOp = ChangeSetOperation.Update;
                        interestTransactionPM.InterestReportId = null;
                        myInterestTransactionUpdateService.Update(interestTransactionPM, commit);
                    }
                }
                    );
            }



            GLAccountPM gLAccountPM = gLAccountQueryService.GetSinglePM(gLAccountId, tenant);
            gLAccountQueryService.GetComposition(new GLAccountKeys() { Id = gLAccountId }, gLAccountPM);
            if (gLAccountPM != null)
            {
                gLAccountPM.InterestOpenBalance = null;
                gLAccountPM.ChangeSetOp = ChangeSetOperation.Update;
                gLAccountUpdateService.Update(gLAccountPM, true);
            }

        }
        private bool ReportExists(string gLAccountId, IAccountingContext context, int tenant)
        {
            InterestReportRepository myInterestReportRepository = new InterestReportRepository(context);
            var q = from rep in myInterestReportRepository.GetAll(tenant)
                        .Where(rec => rec.GLAccountId == gLAccountId && rec.InterestReportStatusCode != InterestReportStatusCodes.Cancelled) select rep.Id; // Cancelled=="3"
            if (q.Count() > 0) { return true; }

            return false;
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
    }


}
