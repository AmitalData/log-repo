using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public class LedgerTransactionCardIndexService
    {
        private LedgerTransactionCardIndexFilter _Param;
        private IAccountingContext _AccountingContext;
        private bool _IsAccountingCurrencyRequested;
        private IQueryable<string> _allIdAccounts;
        private string _SearchByFilter;
        //private string _ForeignCurrencyId = null;
        public LedgerTransactionCardIndexService(IAccountingContext accountingContext, LedgerTransactionCardIndexFilter param)
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


        public void Run()
        {
           // DateTime? maxCreateDate = null;

            this.Response = new LedgerTransactionCardIndexResponse();
            CheckParam();

            var TransactionFactoryWrapper = new TransactionFactoryWrapper();


            using (var scope = TransactionFactoryWrapper.GetTransaction())
            {

                var ledgerTransactionRepository = new LedgerTransactionRepository(_AccountingContext);


                _SearchByFilter = null;

                if (_Param.CallBack != null)
                {
                    if (_Param.CallBack.AllIdAccounts == null)
                    {
                        throw new Exception("Dear Programmer it's about time to remap the 'AllIdAccounts' Property");
                    }
                    _allIdAccounts = (new GLAccountRepository(_AccountingContext)).GetQId(_Param.CallBack.AllIdAccounts, _Param.Tenant); //_Param.CallBack.AllIdAccounts;
                    _SearchByFilter = _Param.CallBack.SearchFields;
                }
                else
                {
                    _SearchByFilter = _Param.SearchFields;
                    var myGLAccountQueryService = new GLAccountQueryService(_AccountingContext);
//                    var hashsetallIdAccounts = myGLAccountQueryService.GetAllIdAccountsCat(_Param.Tenant, _Param.GLAccountId, _Param.Category1Id, _Param.Category2Id,
//                        _Param.Category3Id, _Param.Category4Id, _Param.Category5Id, _Param.IncludeChildAccounts);
                    var qAllIdAccounts = myGLAccountQueryService.GetAllIdAccountsTypeCat(_Param.Tenant, _Param.GLAccountId, _Param.Category1Id, _Param.Category2Id,
                        _Param.Category3Id, _Param.Category4Id, _Param.Category5Id, _Param.AccountTypeCode, _Param.ChartOfAccountsId, _Param.IncludeChildAccounts);
                    _allIdAccounts = qAllIdAccounts;// new List<string>(hashsetallIdAccounts);
                }



                IQueryable<Data.EntityPOCOs.LedgerTransaction> query =
                    ledgerTransactionRepository.GetQueryOrderByDateTypeAndIdByRec(_Param.Tenant, _allIdAccounts, _Param.From, _Param.To,
                    _Param.CurrencyId, _SearchByFilter, _Param.IsReconciled, _Param.DateTypeCode);



                if (_Param.CallBack == null)
                {
                    bool includeAccoutingDateLTransaction = false;
                    var startAccountBalanceService = GetStartAccountBalance(//includeChildAccounts, 
                        includeAccoutingDateLTransaction);
                    includeAccoutingDateLTransaction = true;
                    var endAccountBalanceService = GetEndAccountBalance(//includeChildAccounts, 
                        includeAccoutingDateLTransaction
                        );
                    this.Response.YearTransferLedgerTransactionIds = startAccountBalanceService.YearTransferLedgerTransactionIds;

                    query = RemoveYearTransferLedgerTrans(query);
                    BuildCallBack(query);


                }
                else // if callback
                {
                    ReCopyCallBack();
                    query = RemoveYearTransferLedgerTrans(query);
                }

                //int pageSize = 100; int curPageZeroBase = 0;
                var list = Translate2ListMode(query);
                if (!this.Response.OmitAllCardIndex)
                {
                    decimal CumulativeForeignAmount = 0;
                    if (this.Response.StartBalanceForeignList.Any())
                    {
                        CumulativeForeignAmount =
                        this.Response.StartBalanceForeignList
                        .FirstOrDefault().BalanceForeign.GetValueOrDefault();
                    }
                    decimal CumulativeLocalAmount = this.Response.StartBalanceLocal.GetValueOrDefault();
                    MyBlance myBlance = GetStartBalanceOfCurrPage(query);
                    CumulativeLocalAmount += myBlance.SumLocalAmount;
                    CumulativeForeignAmount += myBlance.SumForeignAmount;


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

                    });
                }
                Response.MyLedgerTransactionList = list;
            }

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
                _Param.DateTypeCode, To, includeAccoutingDateLTransaction, false);
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
                _Param.DateTypeCode /*GLAccountTotalDateTypeValues.Accountingdate*/, _Param.From, includeAccoutingDateLTransaction, false);
            var startAccountBalance = startAccountBalanceService.AccountBalance;
            return startAccountBalance;
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
                myBlance = qprevTot.FirstOrDefault();
            }

            return myBlance;
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



        private void BuildCallBack(IQueryable<Data.EntityPOCOs.LedgerTransaction> query)
        {
            //var BeginOfYearLocalAmountCardIndex = GetBeginOfYearLocalAmountCardIndex(_AccountingContext,_Param.From);

            Response.MyLedgerTransactionList = Translate2ListMode(query);

            DateTime periodMaxCreateDate = DateTime.Now;
            int periodTotalRowCount = 0;
            Response.TotalRowCount = periodTotalRowCount;


            Response.AllIdAccounts = _allIdAccounts.ToList();

            Response.SearchFields = _SearchByFilter;
            Response.OmitAllCardIndex = !String.IsNullOrWhiteSpace(_SearchByFilter);
            if (Response.OmitAllCardIndex)
            {
                return;
            }
        }



 
        private void ReCopyCallBack()
        {
            this.Response.SuppressCumulativeDueMultiCurrencyInPeriod = _Param.CallBack.SuppressCumulativeDueMultiCurrencyInPeriod;
   //         this.Response.Have1CurrencyIdInPeriod = _Param.CallBack.Have1CurrencyIdInPeriod;

            this.Response.AllIdAccounts = _Param.CallBack.AllIdAccounts;
            this.Response.TotalRowCount = _Param.CallBack.TotalRowCount;
            this.Response.YearTransferLedgerTransactionIds = _Param.CallBack.YearTransferLedgerTransactionIds;
            this.Response.SearchFields = _Param.CallBack.SearchFields;
            this.Response.OmitAllCardIndex = _Param.CallBack.OmitAllCardIndex;
            this.Response.StartBalanceForeignList = _Param.CallBack.StartBalanceForeignList;
            this.Response.EndBalanceForeignList = _Param.CallBack.EndBalanceForeignList;
    //        this.Response.OpenBalanceForYearInLocalCurrency = _Param.CallBack.OpenBalanceForYearInLocalCurrency;
        }

        public virtual List<LedgerTransactionList> Translate2ListMode(IQueryable<Data.EntityPOCOs.LedgerTransaction> QOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId)
        {
            var ledgerTransactionListQueryService = new LedgerTransactionListQueryService(_AccountingContext);
            var list = ledgerTransactionListQueryService.GetLedgerTransactionListForceOrderByDateTypeCodeAndId(QOrderAccDateAndIdByAccIdBetweenAccDateMaxCreateLimit_AndCurrencyId,"1", _Param.PageSize, _Param.PageStartAtRecordIndex);
            return list;
        }



 

        private string GetCurrCurrencyId()
        {
            var currentCurrencyId = _Param.CurrencyId;
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
                //not a must throw new Exception("string.IsNullOrWhiteSpace(_Param.AccountId)");
            }

            if (string.IsNullOrWhiteSpace(_Param.CurrencyId))
            {
                //not a must throw new Exception("string.IsNullOrWhiteSpace(_Param.CurrencyId)");
            }

            if (_Param.CallBack == null)
            {
                return;
            }
            if (!String.IsNullOrWhiteSpace(_Param.CallBack.SearchFields) )
            {
                throw new Exception("Search by text resulted in an empty Card Index");
            }
            if (!_Param.CallBack.TotalRowCount.HasValue)
            {
                throw new Exception("!_Param.CallBack.TotalRowCount.HasValue");
            }
            //if (!_Param.CallBack.StartCardIndex.HasValue)
            //{
            //    throw new Exception("!_Param.CallBack.OpenCardIndex.HasValue");
            //}
            //if (!_Param.CallBack.EndCardIndex.HasValue)
            //{
            //    throw new Exception("!_Param.CallBack.EndCardIndex.HasValue");
            //}

        }




        public LedgerTransactionCardIndexResponse Response { get; set; }
    }



}
