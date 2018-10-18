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
using static Logitude.Accounting.Data.EntityListQueryServices.ARPaymentChequeListQueryService;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public class ARPaymentChequeRedemptionService
    {
        private ARPaymentChequeFilter _Param;
        private IAccountingContext _AccountingContext;
        private bool _IsAccountingCurrencyRequested;
        private string _SearchByFilter;
        //private string _ForeignCurrencyId = null;
        public ARPaymentChequeRedemptionService(IAccountingContext accountingContext, ARPaymentChequeFilter param)
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

            this.Response = new ARPaymentChequeResponse();
            CheckParam();

            var TransactionFactoryWrapper = new TransactionFactoryWrapper();


            using (var scope = TransactionFactoryWrapper.GetTransaction())
            {

                var aRPaymentChequeRepository = new ARPaymentChequeRepository(_AccountingContext);


                _SearchByFilter = null;

                if (_Param.CallBack != null)
                {
                    _SearchByFilter = _Param.CallBack.SearchFields;
                }
                else
                {
                    _SearchByFilter = _Param.SearchFields;
                }



                var query =
                    aRPaymentChequeRepository.GetQueryValueDateAndNumber(_Param.Tenant, _Param.ChequeNumber, _Param.From, _Param.To,
                    _Param.CurrencyId, _SearchByFilter);



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
                if (!this.Response.OmitAllCheque)
                {
                }
                Response.MyARPaymentChequeList = list;
            }

        }

        private void BuildCallBack(IQueryable<Data.EntityPOCOs.ARPaymentCheque> query)
        {

            Response.MyARPaymentChequeList = Translate2ListMode(query);

            DateTime periodMaxCreateDate = DateTime.Now;
            int periodTotalRowCount = 0;
            Response.TotalRowCount = periodTotalRowCount;


 
            Response.SearchFields = _SearchByFilter;
            Response.OmitAllCheque = !String.IsNullOrWhiteSpace(_SearchByFilter);
            if (Response.OmitAllCheque)
            {
                return;
            }
        }




        private void ReCopyCallBack()
        {

            this.Response.TotalRowCount = _Param.CallBack.TotalRowCount;
            this.Response.SearchFields = _Param.CallBack.SearchFields;
            this.Response.OmitAllCheque = _Param.CallBack.OmitAllCheque;
        }

        public virtual List<ARPaymentChequeList> Translate2ListMode(IQueryable<Data.EntityPOCOs.ARPaymentCheque> QOrderValueDateAndIdByAccIdBetweenValueDateChequeNumber_AndCurrencyId)
        {
            var aRPaymentChequeListQueryService = new ARPaymentChequeListQueryService(_AccountingContext);
            var list = aRPaymentChequeListQueryService.GetARPaymentChequeListForceOrderByValueDateAndId(QOrderValueDateAndIdByAccIdBetweenValueDateChequeNumber_AndCurrencyId, _Param.PageSize, _Param.PageStartAtRecordIndex);
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
            if (string.IsNullOrWhiteSpace(_Param.ChequeNumber))
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
            if (!String.IsNullOrWhiteSpace(_Param.CallBack.SearchFields))
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




        public ARPaymentChequeResponse Response { get; set; }
    }



}
