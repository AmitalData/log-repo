using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.Utilities;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public class LedgerTransactionBalanceService
    {
        private LedgerTransactionBalanceFilter _Param;
        private IAccountingContext _AccountingContext;
        private bool _IsAccountingCurrencyRequested;
        private IQueryable<string> _allIdAccounts;
        private string _SearchByFilter;
        private Stopwatch _sw;

        //private string _ForeignCurrencyId = null;
        public LedgerTransactionBalanceService(IAccountingContext accountingContext ,LedgerTransactionBalanceFilter param)
        {
            _AccountingContext = accountingContext;
            _Param = param;

            ///Look at  GLAccountReconcileSearchViewModel
            //var view = new VirtualQueryableCollectionView<Customer>() { LoadSize = pageSize, VirtualItemCount = customerProvider.FetchCount() };
            //view.ItemsLoading += (s, args) =>
            //{
            //    new Thread(() =>
            //    {
            //        Thread.Sleep(1000);
            //        view.Load(args.StartIndex, customerProvider.FetchRange(args.StartIndex, args.ItemCount));
            //    }).Start();
            //};
            //DataContext = view;
        }


        public void Run(bool isFromExcelGenerater=false)
        {
            var sw = Stopwatch.StartNew();
            _sw = Stopwatch.StartNew();
            DateTime? maxCreateDate = null;
            
            this.Response = new LedgerTransactionBalanceResponse();
            CheckParam();

            var TransactionFactoryWrapper = new TransactionFactoryWrapper();


            using (var scope = TransactionFactoryWrapper.GetTransaction())
            {

                var ledgerTransactionRepository = new LedgerTransactionRepository(_AccountingContext);


                _SearchByFilter = null;
                LogIt("start");
                if (_Param.CallBack != null)
                {
                    maxCreateDate = _Param.CallBack.MaxCreateAt;
                    if (_Param.CallBack.AllIdAccounts == null)
                    {
                        throw new Exception("Dear Programer it's about time to remap 'AllIdAccounts' Property");
                    }
                    _allIdAccounts = (new GLAccountRepository(_AccountingContext)).GetQId(_Param.CallBack.AllIdAccounts, _Param.Tenant);
                    _SearchByFilter = _Param.CallBack.SearchFields;
                }
                else
                {
                   
                    _SearchByFilter = _Param.SearchFields;
                    var myGLAccountQueryService = new GLAccountQueryService(_AccountingContext);

                    //var hashsetallIdAccounts = myGLAccountQueryService.GetAllIdAccounts(_Param.Tenant, _Param.GLAccountId, _Param.IncludeRelatedCurrenciesAccount, _Param.IncludeChildAccounts);
                    //_allIdAccounts =new List<string>(hashsetallIdAccounts);



                    var hashsetallIdAccounts = myGLAccountQueryService.GetQAllIdAccounts(_Param.Tenant, _Param.GLAccountId, _Param.IncludeRelatedCurrenciesAccount, _Param.IncludeChildAccounts);
                    _allIdAccounts = hashsetallIdAccounts;//new List<string>(hashsetallIdAccounts);
                }
                IQueryable<Data.EntityPOCOs.LedgerTransaction> qOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId = GetQOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId(maxCreateDate, ledgerTransactionRepository);

                if (_Param.CallBack == null)
                {
                    bool includeAccoutingDateLTransaction = false;
                    var startAccountBalanceService = GetStartAccountBalance(//includeChildAccounts, 
    includeAccoutingDateLTransaction);
                    LogIt("GetStartAccountBalance");
                    includeAccoutingDateLTransaction = true;
                    var endAccountBalanceService = GetEndAccountBalance(//includeChildAccounts, 
                        includeAccoutingDateLTransaction
                        );
                    LogIt("GetEndAccountBalance");
                    this.Response.YearTransferLedgerTransactionIds = startAccountBalanceService.YearTransferLedgerTransactionIds;

                    qOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId = RemoveYearTransferLedgerTrans(qOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId);

                    BuildCallBack(qOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId,
                        startAccountBalanceService, endAccountBalanceService);
                    LogIt("BuildCallBack");

                }
                else // if callback
                {
                    ReCopyCallBack();
                    qOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId = RemoveYearTransferLedgerTrans(qOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId);

                }

                //int pageSize = 100; int curPageZeroBase = 0;
                var list = Translate2ListMode(qOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId, isFromExcelGenerater);
                LogIt("Translate2ListMode");
                if (!this.Response.OmitAllBalance)
                {


                    decimal CumulativeForeignAmount = 0;//this.Response.StartBalanceForeign.GetValueOrDefault();
                    if (this.Response.StartBalanceForeignList.Any())
                    {
                        CumulativeForeignAmount =
                        this.Response.StartBalanceForeignList//.First()
                        .FirstOrDefault().BalanceForeign.GetValueOrDefault();
                    }
                    decimal CumulativeLocalAmount = this.Response.StartBalanceLocal.GetValueOrDefault();
                    //if (!this.Response.SuppressCumulativeDueMultiCurrencyInPeriod)
                    //{
                    MyBlance myBlance = GetStartBalanceOfCurrPage(qOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId);
                    CumulativeLocalAmount += myBlance.SumLocalAmount;
                    CumulativeForeignAmount += myBlance.SumForeignAmount;
                    LedgerTransactionHelper ledgerTransactionHelper = new LedgerTransactionHelper();
                    LogIt("b4 list");
                    list.ForEach(rec =>
                    {
                        decimal LocalAmountDebit = rec.LocalAmountDebit;
                        decimal LocalAmountCredit = rec.LocalAmountCredit;

                        CumulativeLocalAmount += (LocalAmountDebit - LocalAmountCredit);
                        rec.CumulativeLocalAmount = CumulativeLocalAmount;
                        if (!this.Response.SuppressCumulativeDueMultiCurrencyInPeriod.GetValueOrDefault())
                        {
                            CumulativeForeignAmount += (rec.ForeignAmountDebit - rec.ForeignAmountCredit);
                            rec.CumulativeForeignAmount = CumulativeForeignAmount;
                        }

                        MapLedgerTransactionLine(rec, ledgerTransactionHelper, isFromExcelGenerater);
                    });
                    LogIt("after list");
                    //}
                }
                Response.MyLedgerTransactionList = list;
            }
            this.Response.TookMS = sw.ElapsedMilliseconds;
            Debug.WriteLine("Response.TookMS:" + Response.TookMS.ToString());
        }

        private void MapLedgerTransactionLine(LedgerTransactionList rec , LedgerTransactionHelper ledgerTransactionHelper, bool isFromExcelGenerater)
        {

            rec.OriginalAmount = ledgerTransactionHelper.CalculateOriginalAmount(rec);
            rec.IconCode = ledgerTransactionHelper.getEntityIcon(rec.SourceTypeCode);
            rec.Source = rec.IconCode + " " + rec.SourceNumber;
            rec.IsLocalAmountCreditPos = rec.LocalAmountCredit != 0;
            rec.CalculatedLocalAmount = rec.LocalAmountCredit != 0 ? rec.LocalAmountCredit : rec.LocalAmountDebit;
            //rec.LocalAmountCredit = rec.LocalAmountCredit != 0 ? rec.LocalAmountCredit : rec.LocalAmountDebit;
            rec.IsCumulativeLocalAmountPos = rec.CumulativeLocalAmount < 0;
            rec.IsForeignAmountCreditPos = rec.ForeignAmountCredit != 0;
            rec.CalculatedForeignAmount = rec.ForeignAmountCredit != 0 ? rec.ForeignAmountCredit : rec.ForeignAmountDebit;
            //rec.ForeignAmountCredit = rec.ForeignAmountCredit != 0 ? rec.ForeignAmountCredit : rec.ForeignAmountDebit;
            rec.IsCumulativeForeignAmountPos = rec.CumulativeForeignAmount < 0;
            rec.IsOriginalAmountPos = rec.OpenAmount < 0;
            rec.IsForeignAmountPos = rec.ForeignAmountCredit != 0;
            rec.ForeignAmountCreditWithSign = rec.CalculatedForeignAmount + " " + rec.CurrencySign;
            rec.CumulativeForeignAmountSign = rec.CumulativeForeignAmount + " " + rec.CurrencySign;
            if (isFromExcelGenerater)
            {
                ledgerTransactionHelper.MapAmountWithNegativeValue(rec);
            }
        }
 
        private void LogIt(string mess)
        {

            mess = mess + ":Took:" + _sw.Elapsed.ToString();
            Debug.WriteLine(mess);

            _sw.Restart();
            //_StringBuilder.AppendLine(mess);
        }
        private IQueryable<Data.EntityPOCOs.LedgerTransaction> RemoveYearTransferLedgerTrans(IQueryable<Data.EntityPOCOs.LedgerTransaction> QOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId)
        {
            if (this.Response.YearTransferLedgerTransactionIds != null && this.Response.YearTransferLedgerTransactionIds.Count > 0)
            {
                QOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId =
                    QOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId
                    .Where(r =>
                    !this.Response.YearTransferLedgerTransactionIds.Contains(r.Id));
            }

            return QOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId;
        }

        private MyBlance GetStartBalanceOfCurrPage(IQueryable<Data.EntityPOCOs.LedgerTransaction> QOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId)
        {
            MyBlance myBlance = new MyBlance() { SumForeignAmount = 0, SumLocalAmount = 0 };
            if (_Param.PageStartAtRecordIndex > 0)
            {
                var ledgerPrevPages = QOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId
                                    .Take(_Param.PageStartAtRecordIndex);

                bool calcForeign = !this.Response.SuppressCumulativeDueMultiCurrencyInPeriod.GetValueOrDefault();
                var qprevTot =
                    (from lt in ledgerPrevPages
                     group lt by 1 into gb
                     select new MyBlance()
                     {
                         SumLocalAmount = gb.Sum(rec => rec.LocalAmountDebit - rec.LocalAmountCredit),
                         SumForeignAmount = calcForeign ? gb.Sum(rec => rec.ForeignAmountDebit - rec.ForeignAmountCredit) : 0,
                     });
                myBlance=qprevTot.FirstOrDefault();
                if (false)
                {


                    var qgGperiod =
                        //QOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId.Take(_Param.PageStartAtRecordIndex)
                        ledgerPrevPages
                                        .GroupBy(r => r);


                    IQueryable<MyBlance> qMyBlance = null;




                    if (!this.Response.SuppressCumulativeDueMultiCurrencyInPeriod.GetValueOrDefault())
                    {

                        qMyBlance = qgGperiod.Select(g => new MyBlance()
                        {

                            SumLocalAmount = g.Sum(rec => rec.LocalAmountDebit - rec.LocalAmountCredit),
                            SumForeignAmount = g.Sum(rec => rec.ForeignAmountDebit - rec.ForeignAmountCredit),

                        });

                    }
                    else
                    {
                        qMyBlance = qgGperiod.Select(g => new MyBlance()
                        {

                            SumLocalAmount = g.Sum(rec => rec.LocalAmountDebit - rec.LocalAmountCredit),
                            SumForeignAmount = 0,

                        });
                    }
                    myBlance = qMyBlance.GroupBy(g => g).Select(g => new MyBlance()
                    {
                        SumLocalAmount = g.Sum(r => r.SumLocalAmount),
                        SumForeignAmount = g.Sum(r => r.SumForeignAmount),
                    }).First();
                }
            }

            return myBlance;
        }

        private IQueryable<Data.EntityPOCOs.LedgerTransaction> GetQOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId(DateTime? maxCreateDate, LedgerTransactionRepository ledgerTransactionRepository)
        {
            
            
            switch (_Param.DateTypeCode)
            {
                case GLAccountTotalDateTypeValues.DueDate:
                case GLAccountTotalDateTypeValues.DocumentDate:
                case GLAccountTotalDateTypeValues.Accountingdate:
                    {
                        return ledgerTransactionRepository.GetQueryByDateType(_Param.Tenant, _allIdAccounts,
                            _Param.DateTypeCode, _Param.From, _Param.To,
                        _Param.CurrencyId, _SearchByFilter, maxCreateDate);
                    }
            
                    break;
                
                    
                
                default:
                {
                        return ledgerTransactionRepository.GetQueryOrderAccDateAndIdBy(_Param.Tenant, _allIdAccounts, _Param.From, _Param.To,
                _Param.CurrencyId, _SearchByFilter, maxCreateDate);
                    }
                    break;
            }
            
        }

        private void BuildCallBack(IQueryable<Data.EntityPOCOs.LedgerTransaction> QOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId
, AccountBalanceM startAccountBalanceService,
AccountBalanceM endAccountBalanceService)
        {
            var sw = Stopwatch.StartNew();
            bool includeChildAccounts = false;
            
            //var BeginOfYearLocalAmountBalance = GetBeginOfYearLocalAmountBalance(_AccountingContext,_Param.From);



            var qGperiod = (from r in QOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId
                            group r by 1 into g
                            select new
                            {
                                AllCurrencyId = g.Select(r => r.CurrencyId).Distinct(),
                                MaxCreateDate = g.Max(rec => rec.CreateDate),
                                SumLocalAmount = g.Sum(rec => rec.LocalAmountDebit - rec.LocalAmountCredit),
                                SumForeignAmount = g.Sum(rec => rec.ForeignAmountDebit - rec.ForeignAmountCredit),
                                Count = g.Count()
                            }
            );
            var myStatstic4period = qGperiod
                //.First()  = fail : Sequence contains no elements
                    .FirstOrDefault();
            Debug.WriteLine("BuildCallBack:" + sw.ElapsedMilliseconds);
            DateTime periodMaxCreateDate = DateTime.Now;
            decimal periodSumLocalAmount = 0;
            decimal periodSumForeignAmount = 0;
            int periodTotalRowCount = 0;
            var AllCurrencyId = new List<string>();
            if (myStatstic4period != null)
            {
                AllCurrencyId = myStatstic4period.AllCurrencyId.ToList();
                periodMaxCreateDate = myStatstic4period.MaxCreateDate;
                periodSumLocalAmount = myStatstic4period.SumLocalAmount;
                periodSumForeignAmount = myStatstic4period.SumForeignAmount;
                periodTotalRowCount = myStatstic4period.Count;
            }
            Response.MaxCreateAt = periodMaxCreateDate;
            Response.TotalRowCount = periodTotalRowCount;


            Response.AllIdAccounts = _allIdAccounts.ToList();

            Response.SearchFields = _SearchByFilter;
            Response.OmitAllBalance = !String.IsNullOrWhiteSpace(_SearchByFilter);
            if (Response.OmitAllBalance)
            {
                return;
            }

            Init1CurrAndSuppressCumuDueMultiCurrrency(AllCurrencyId);




            
            InitForeignList(startAccountBalanceService, endAccountBalanceService);

            Response.HaveAccountingQueued=(startAccountBalanceService.HaveAccountingQueued || endAccountBalanceService.HaveAccountingQueued);

            CheckSumLocalEqualDiffEndStart(startAccountBalanceService, endAccountBalanceService, periodSumLocalAmount, periodSumForeignAmount);




            var gLAccountTotalByMonthRepository = new GLAccountTotalByMonthRepository(_AccountingContext);
            this.Response.OpenBalanceForYearInLocalCurrency = gLAccountTotalByMonthRepository.GetLocalOpenBalanceForYearDateTypeCode(_Param.DateTypeCode, _Param.GLAccountId, _Param.From.Year, _Param.Tenant);
        }

        private void InitForeignList(AccountBalanceM startAccountBalanceService, AccountBalanceM endAccountBalanceService)
        {

            var featureForeignList = true;
            if (featureForeignList)//this.Response.SuppressCumulativeDueMultiCurrencyInPeriod) = Multi Currency 
            {
                var qTotals =
                    (from tot in startAccountBalanceService.Totals
                     select new CallBackBalance
                     {
                         CurrencyId = tot.CurrencyId,
                         BalanceForeign = tot.ForeignAmountDebit - tot.ForeignAmountCredit,
                         BalanceLocal = tot.LocalAmountDebit - tot.LocalAmountCredit

                     });
                if (!String.IsNullOrWhiteSpace(_Param.CurrencyId))
                {
                    qTotals = qTotals.Where(r => r.CurrencyId == _Param.CurrencyId);
                }
                this.Response.StartBalanceForeignList = qTotals.ToList();


                qTotals = (from tot in endAccountBalanceService.Totals
                           select new CallBackBalance
                           {
                               CurrencyId = tot.CurrencyId,
                               BalanceForeign = tot.ForeignAmountDebit - tot.ForeignAmountCredit,

                               BalanceLocal = tot.LocalAmountDebit - tot.LocalAmountCredit
                           });
                if (!String.IsNullOrWhiteSpace(_Param.CurrencyId))
                {
                    qTotals = qTotals.Where(r => r.CurrencyId == _Param.CurrencyId);
                }
                this.Response.EndBalanceForeignList = qTotals.ToList();


            }

            if (!String.IsNullOrWhiteSpace(_Param.CurrencyId))
            {

                this.Response.StartBalanceLocal = startAccountBalanceService.GetBalanceOfLocalAmount(_Param.CurrencyId).GetValueOrDefault();
                this.Response.EndBalanceLocal = endAccountBalanceService.GetBalanceOfLocalAmount(_Param.CurrencyId).GetValueOrDefault();


                this.Response.StartBalanceForeignList = startAccountBalanceService.GetCallBackBalanceOfCurrency(_Param.CurrencyId);
                this.Response.EndBalanceForeignList = endAccountBalanceService.GetCallBackBalanceOfCurrency(_Param.CurrencyId);

            }
            else
            {
                this.Response.StartBalanceForeignList = startAccountBalanceService.GetCallBackBalance();
                this.Response.EndBalanceForeignList = endAccountBalanceService.GetCallBackBalance();


                this.Response.StartBalanceLocal = startAccountBalanceService.GetBalanceOfLocalAmount().GetValueOrDefault();
                this.Response.EndBalanceLocal = endAccountBalanceService.GetBalanceOfLocalAmount().GetValueOrDefault();
            }
            
            

            this.Response.StartBalanceForeignList = this.Response.StartBalanceForeignList ?? new List<CallBackBalance>();
            this.Response.EndBalanceForeignList = this.Response.EndBalanceForeignList ?? new List<CallBackBalance>();
        }

        private void CheckSumLocalEqualDiffEndStart(AccountBalanceM startAccountBalanceService, AccountBalanceM endAccountBalanceService, decimal periodSumLocalAmount, decimal periodSumForeignAmount)
        {
            var periodSumLocal = this.Response.EndBalanceLocal - this.Response.StartBalanceLocal;
            if (periodSumLocal != periodSumLocalAmount)
            {
                throw new Exception("periodSum!=periodSumLocalAmount");
            }

            if (!this.Response.SuppressCumulativeDueMultiCurrencyInPeriod.GetValueOrDefault())
            {

                var currentCurrencyId = GetCurrCurrencyId();

                var //this.Response.
                    StartBalanceForeign = startAccountBalanceService.GetBalanceOfCurrency(currentCurrencyId);
                var //this.Response.
                    EndBalanceForeign = endAccountBalanceService.GetBalanceOfCurrency(currentCurrencyId);

                var periodSumForeign = //this.Response.
                    EndBalanceForeign - //this.Response.
                    StartBalanceForeign.GetValueOrDefault();
                if (periodSumForeign != periodSumForeignAmount)
                {
                    throw new Exception("periodSum(this.Response.EndBalanceForeign - this.Response.StartBalanceForeign.GetValueOrDefault())!=periodSumLocalAmount");
                }

            }
        }

        private void Init1CurrAndSuppressCumuDueMultiCurrrency(List<string> AllCurrencyId)
        {
            if (!String.IsNullOrWhiteSpace(_Param.CurrencyId))
            {
                AllCurrencyId.Remove(_Param.CurrencyId);
                if (AllCurrencyId.Count > 0)
                {
                    this.Response.SuppressCumulativeDueMultiCurrencyInPeriod = true;
                }

            }
            else
            {
                if (AllCurrencyId.Count <= 1)
                {
                    this.Response.SuppressCumulativeDueMultiCurrencyInPeriod = true;
                    if (AllCurrencyId.Count == 1)
                    {
                        this.Response.SuppressCumulativeDueMultiCurrencyInPeriod = false;
                        this.Response.Have1CurrencyIdInPeriod = AllCurrencyId.First();
                    }
                }
                else
                {
                    this.Response.SuppressCumulativeDueMultiCurrencyInPeriod = true;
                }
            }
            if (Response.TotalRowCount == 0)// No Row >> SuppressCumulativeDueMultiCurrencyInPeriod = true;
            {
                this.Response.SuppressCumulativeDueMultiCurrencyInPeriod = true;
            }
            this.Response.SuppressCumulativeDueMultiCurrencyInPeriod = this.Response.SuppressCumulativeDueMultiCurrencyInPeriod ?? false;
        }

        private void ReCopyCallBack()
        {
            this.Response.SuppressCumulativeDueMultiCurrencyInPeriod = _Param.CallBack.SuppressCumulativeDueMultiCurrencyInPeriod;

            this.Response.Have1CurrencyIdInPeriod = _Param.CallBack.Have1CurrencyIdInPeriod;
            this.Response.StartBalanceLocal = _Param.CallBack.StartBalanceLocal;
            this.Response.EndBalanceLocal = _Param.CallBack.EndBalanceLocal;

            //this.Response.StartBalanceForeign = _Param.CallBack.StartBalanceForeign;
            //this.Response.EndBalanceForeign = _Param.CallBack.EndBalanceForeign;
            this.Response.MaxCreateAt = _Param.CallBack.MaxCreateAt;
            this.Response.AllIdAccounts = _Param.CallBack.AllIdAccounts;
            this.Response.TotalRowCount = _Param.CallBack.TotalRowCount;
            this.Response.YearTransferLedgerTransactionIds = _Param.CallBack.YearTransferLedgerTransactionIds;

            this.Response.SearchFields = _Param.CallBack.SearchFields;
            this.Response.OmitAllBalance = _Param.CallBack.OmitAllBalance;

            this.Response.StartBalanceForeignList = _Param.CallBack.StartBalanceForeignList;
            this.Response.EndBalanceForeignList = _Param.CallBack.EndBalanceForeignList;
            this.Response.OpenBalanceForYearInLocalCurrency = _Param.CallBack.OpenBalanceForYearInLocalCurrency;

        }

        public virtual List<LedgerTransactionList> Translate2ListMode(IQueryable<Data.EntityPOCOs.LedgerTransaction> qOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId,bool isFromExcelGenerater)
        {
            var ledgerTransactionListQueryService = new LedgerTransactionListQueryService(_AccountingContext);
            var list = ledgerTransactionListQueryService.GetLedgerTransactionListForceOrderByDateTypeCodeAndId(qOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId,
                _Param, isFromExcelGenerater);
            return list;
        }

        

        private AccountBalanceM GetEndAccountBalance(
            //bool includeChildAccounts, 
            bool includeAccoutingDateLTransaction
            )
        {
            var endAccountBalanceService = new AccountBalanceByDateCodeService(_AccountingContext, _Param.Tenant,
                _Param.GLAccountId, 
                //includeChildAccounts, _Param.IncludeRelatedCurrenciesAccount
                _allIdAccounts
                );
            var To = _Param.To/*.AddDays(1)*/;
            bool openBalancePlease_ReCalcYearTransfer = false;
            endAccountBalanceService.CalculateBalance(
                openBalancePlease_ReCalcYearTransfer,
                _Param.DateTypeCode,To,
                _Param.CheckHaveAccountingQueued,
                includeAccoutingDateLTransaction, false);
            var endAccountBalance = endAccountBalanceService.AccountBalance;
            return endAccountBalance;
        }

        private AccountBalanceM GetStartAccountBalance(
            //bool includeChildAccounts, 
            bool includeAccoutingDateLTransaction)
        {
            var startAccountBalanceService = new AccountBalanceByDateCodeService(
                _AccountingContext, _Param.Tenant, _Param.GLAccountId, 
                //includeChildAccounts, _Param.IncludeRelatedCurrenciesAccount
                _allIdAccounts);

            bool openBalancePlease_ReCalcYearTransfer = true;//Yaron said this is Default !!!
            startAccountBalanceService.CalculateBalance(
                openBalancePlease_ReCalcYearTransfer,
                _Param.DateTypeCode /*GLAccountTotalDateTypeValues.Accountingdate*/,_Param.From,
                _Param.CheckHaveAccountingQueued,
                includeAccoutingDateLTransaction, false);
            var startAccountBalance = startAccountBalanceService.AccountBalance;
            return startAccountBalance;
        }

        private decimal GetBeginOfYearLocalAmountBalance(IAccountingContext _AccountingContext,DateTime From)
        {
            int lastYear = //_Param.
                From.Date.Year - 1;
            int month12 = 12;
            var BeginOfYearMonthQueryService = new GLAccountTotalByMonthQueryService(_AccountingContext);
            var BeginOfYearCurrencySum = BeginOfYearMonthQueryService
                //.GetCurrencySumUntillNotInclude(new List<string>() { _Param.GLAccountId }, lastYear, month12, _Param.Tenant);
                .GetCurrencySumUntillNotInclude(
                (new GLAccountRepository(_AccountingContext)).GetQId(new List<string>() { _Param.GLAccountId } , _Param.Tenant),

            lastYear, month12, _Param.Tenant);
            var BeginOfYearLocalAmountBalance = BeginOfYearCurrencySum.Sum(cSum => cSum.LocalAmountDebit - cSum.LocalAmountCredit);
            return BeginOfYearLocalAmountBalance;
        }

        

        private string GetCurrCurrencyId()
        {
            var currentCurrencyId = _Param.CurrencyId;
            if (String.IsNullOrWhiteSpace(currentCurrencyId))
            {
                currentCurrencyId = this.Response.Have1CurrencyIdInPeriod;
            }
            return currentCurrencyId;
        }


        private void CheckParam()
        {
            _Param.To = _Param.To.Date;
            _Param.From = _Param.From.Date;

            if (_AccountingContext == null)
            {
                throw new Exception("_AccountingContext is null (Developer Error )");
            }

            if (_Param.To < _Param.From)
            {
                throw new Exception("_Param.To < _Param.from");
            }
            if (string.IsNullOrWhiteSpace(_Param.GLAccountId))
            {
                throw new Exception("string.IsNullOrWhiteSpace(_Param.AccountId)");
            }

            if (string.IsNullOrWhiteSpace(_Param.CurrencyId))
            {
                //not must throw new Exception("string.IsNullOrWhiteSpace(_Param.CurrencyId)");
            }

            if (_Param.CallBack == null)
            {
                return;
            }
            if (!String.IsNullOrWhiteSpace(_Param.CallBack.SearchFields) &&
                (_Param.CallBack.StartBalanceLocal.HasValue || _Param.CallBack.EndBalanceLocal.HasValue)
                )
            {
                throw new Exception("if Was Search By Text then all Balance is Omit ");
            }
            if (!_Param.CallBack.TotalRowCount.HasValue)
            {
                throw new Exception("!_Param.CallBack.TotalRowCount.HasValue");
            }
            //if (!_Param.CallBack.StartBalance.HasValue)
            //{
            //    throw new Exception("!_Param.CallBack.OpenBalance.HasValue");
            //}
            //if (!_Param.CallBack.EndBalance.HasValue)
            //{
            //    throw new Exception("!_Param.CallBack.EndBalance.HasValue");
            //}
            if (!_Param.CallBack.MaxCreateAt.HasValue)
            {
                throw new Exception("!_Param.CallBack.LastCreateAt.HasValue");
            }
            if (string.IsNullOrWhiteSpace(_Param.DateTypeCode))
            {
                _Param.DateTypeCode = GLAccountTotalDateTypeValues.Accountingdate;
            }
            var myDateTypeCodeList = new List<string>() {
            GLAccountTotalDateTypeValues.Accountingdate,
            GLAccountTotalDateTypeValues.DueDate,
            GLAccountTotalDateTypeValues.DocumentDate
            };
            if (!myDateTypeCodeList.Any(r => r == _Param.DateTypeCode))
            {
                throw new Exception("DateTypeCode not in list {GLAccountTotalDateTypeValues.Accountingdate,GLAccountTotalDateTypeValues.DueDate,GLAccountTotalDateTypeValues.DocumentDate}");
            }



        }




        public LedgerTransactionBalanceResponse Response { get; set; }
    }

    class MyBlance
    {
        public decimal SumLocalAmount { get; internal set; }
        public decimal SumForeignAmount { get; internal set; }
    }


}
