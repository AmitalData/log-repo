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

                // Ensure the GLAccount is not a (currency) descendant of a (multi-currency) GLAccount
                if (!String.IsNullOrEmpty(myGLAccountId))
                {
                    GLAccountPM gLAccountPM = gLAccountQueryService.GetSinglePM(myGLAccountId, tenant);
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
                    else if (gLAccountPM.ParentAccountId != null)
                    {
                        this.AddErrorRow($"GLAccount id={gLAccountPM} display={gLAccountPM.DisplayNumber} is a descendant of {gLAccountPM.ParentAccountId}");
                        _errors = true;
                    }
                }

                if (!_errors)
                {
                    if (String.IsNullOrEmpty(myGLAccountId))
                    {
                        List<string> gLAccountIdList;
                        gLAccountIdList = gLAccountQueryService.GetNextGLAccountIdByTypeControlNoParent(tenant, accountTypeCode, false, LastMadeGLAccountId, MaxGLAccountsPerQuery);

                        if (gLAccountIdList != null && gLAccountIdList.Count > 0)
                        {
                            _MyResult.LastMadeGLAccountId = gLAccountIdList.Last();
                            gLAccountIdList.ForEach(accId =>
                            {
                                ActivationBalanceCalculation(accId, gLAccountInterestActivationBalanceArgs.InterestActivationDate, tenant);
                            });
                        }

                    }
                    else
                    {
                        ActivationBalanceCalculation(myGLAccountId, gLAccountInterestActivationBalanceArgs.InterestActivationDate, tenant);
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





        public void ActivationBalanceCalculation(string gLAccountId, DateTime interestActivationDate, int _Tenant)
        {
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


                    var _AccountingContext = AccountingContext.GetContext(_Tenant);

                    var myGLAccountRepo = new GLAccountRepository(context);
                    // var myGLAccountMoreDataRepo = new GLAccountMoreDataRepository(context);

                    //    var myInterestReportRepo = new InterestReportRepository(context);

                    var myInterestTransactionRepository = new InterestTransactionRepository(context);



                    //////////////// 
                    /// 1. Compute BalanceInLocalCurrency
                    string totalDateType = "1"; // accounting date 
                    bool SumOpenTransactions = false;
                    bool IncludeRelatedCurrenciesAccount = false;
                    AccountBalanceByDateCodeService ac = new AccountBalanceByDateCodeService(null, _Tenant, gLAccountId, null);
                    ac.ReSetAccountList(false, IncludeRelatedCurrenciesAccount);
                    bool openBalancePlease_ReCalcYearTransfer = //true;//Yaron said this is Default !!!
                        (interestActivationDate.Day == 1 && interestActivationDate.Month == 1);
                    ac.CalculateBalance(
                        openBalancePlease_ReCalcYearTransfer,
                        totalDateType, interestActivationDate, false, true, false,
                        false, SumOpenTransactions);

                    ac.AccountBalance.LogMessage = null;


                    decimal? balance_qm = ac.AccountBalance.GetBalanceOfLocalAmount().GetValueOrDefault();
                    decimal balance_on_act_date = balance_qm.HasValue ? balance_qm.Value : 0m;



                    ////////////
                    /// 2.5. Compute "Before"

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
                    if (before_list != null)
                    {
                        decimal? amount_before_qm = before_list.Select(c => c.LocalAmount).Sum();
                        amount_before = amount_before_qm.HasValue ? amount_before_qm.Value : 0m;
                    }


                    ////////////////
                    /// 3.Compute interestOpenBalance
                    decimal interestOpenBalance = amount_before + balance_on_act_date;




                    ////////////////
                    /// 4.Update InterestTransaction
                    if (before_list != null)
                    {
                        int total_count = before_list.Count;
                        int count = 0;
                        int each = 100;
                        InterestTransactionQueryService myInterestTransactionService = new InterestTransactionQueryService(context);
                        InterestTransactionUpdateService myInterestTransactionUpdateService = new InterestTransactionUpdateService(context, new Dictionary<string, IContext>(), _Tenant);

                        before_list.ForEach(intt =>
                            {
                                count++;
                                bool commit = (count >= total_count || count % each == 0);
                                InterestTransactionPM interestTransactionPM = myInterestTransactionService.GetSingle(intt.Id, false, false);
                                if (interestTransactionPM != null)
                                {
                                    interestTransactionPM.IsClosed = true;
                                    interestTransactionPM.ChangeSetOp = ChangeSetOperation.Update;
                                    interestTransactionPM.InterestReportId = "OPEN";
                                    myInterestTransactionUpdateService.Update(interestTransactionPM, commit);
                                }
                            }
                            );
                    }



                    ////////////////
                    /// 5.Update GLAccount.InterestOpenBalance

                    GLAccountUpdateService gLAccountUpdateService = new GLAccountUpdateService(context, new Dictionary<string, IContext>(), _Tenant);

                    GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(context);
                    GLAccountPM gLAccountPM = gLAccountQueryService.GetSinglePM(gLAccountId, _Tenant);
                    if (gLAccountPM != null)
                    {
                        gLAccountPM.InterestOpenBalance = interestOpenBalance;
                        gLAccountPM.ChangeSetOp = ChangeSetOperation.Update;
                        gLAccountUpdateService.Update(gLAccountPM, true);

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

    }


    public class GLAccountInterestActivationBalanceBatchResult
    {
        public string LastMadeGLAccountId = "";
        public long SuccessAccountLineCount = 0;
        public long BadAccountLineCount = 0;
        public List<string> ErrorRowList = new List<string>();
    }


}
