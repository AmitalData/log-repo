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
        private List<string> _allIdAccounts;
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
                    _allIdAccounts = _Param.CallBack.AllIdAccounts;
                    _SearchByFilter = _Param.CallBack.SearchFields;
                }
                else
                {
                    _SearchByFilter = _Param.SearchFields;
                    var myGLAccountQueryService = new GLAccountQueryService(_AccountingContext);
//                    var hashsetallIdAccounts = myGLAccountQueryService.GetAllIdAccountsCat(_Param.Tenant, _Param.GLAccountId, _Param.Category1Id, _Param.Category2Id,
//                        _Param.Category3Id, _Param.Category4Id, _Param.Category5Id, _Param.IncludeChildAccounts);
                    var hashsetallIdAccounts = myGLAccountQueryService.GetAllIdAccountsTypeCat(_Param.Tenant, _Param.GLAccountId, _Param.Category1Id, _Param.Category2Id,
                        _Param.Category3Id, _Param.Category4Id, _Param.Category5Id, _Param.AccountTypeCode, _Param.ChartOfAccountsId, _Param.IncludeChildAccounts);
                    _allIdAccounts = new List<string>(hashsetallIdAccounts);
                }



                var query =
                    ledgerTransactionRepository.GetQueryOrderByDateTypeAndIdByRec(_Param.Tenant, _allIdAccounts, _Param.From, _Param.To,
                    _Param.CurrencyId, _SearchByFilter, _Param.IsReconciled, _Param.DateTypeCode);



                if (_Param.CallBack == null)
                {
                    BuildCallBack(query);


                }
                else // if callback
                {
                    ReCopyCallBack();
                }

                //int pageSize = 100; int curPageZeroBase = 0;
                var list = Translate2ListMode(query);
                if (!this.Response.OmitAllCardIndex)
                {
                }
                Response.MyLedgerTransactionList = list;
            }

        }

        private void BuildCallBack(IQueryable<Data.EntityPOCOs.LedgerTransaction> query)
        {
            //var BeginOfYearLocalAmountCardIndex = GetBeginOfYearLocalAmountCardIndex(_AccountingContext,_Param.From);

            Response.MyLedgerTransactionList = Translate2ListMode(query);

            DateTime periodMaxCreateDate = DateTime.Now;
            int periodTotalRowCount = 0;
            Response.TotalRowCount = periodTotalRowCount;


            Response.AllIdAccounts = _allIdAccounts;

            Response.SearchFields = _SearchByFilter;
            Response.OmitAllCardIndex = !String.IsNullOrWhiteSpace(_SearchByFilter);
            if (Response.OmitAllCardIndex)
            {
                return;
            }
        }



 
        private void ReCopyCallBack()
        {

            this.Response.AllIdAccounts = _Param.CallBack.AllIdAccounts;
            this.Response.TotalRowCount = _Param.CallBack.TotalRowCount;
            this.Response.SearchFields = _Param.CallBack.SearchFields;
            this.Response.OmitAllCardIndex = _Param.CallBack.OmitAllCardIndex;
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
