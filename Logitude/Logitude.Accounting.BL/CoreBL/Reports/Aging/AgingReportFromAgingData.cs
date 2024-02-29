using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Server.Tools.Helpers;
using System.Reflection;

namespace Logitude.Accounting.BL.CoreBL.Reports.Aging
{
    public class AgingReportFromAgingData
    {
        private IAccountingContext _AccountingContext;
        private IQueryable<string> _MainAccountIdList_ToFetchThenAggragrate;
        private IQueryable<GLAccount> _qAllAccAging4AccountTypeCode_CustomerOrVendor;
        private string _AccountingCurrencyId;

        public AgingReportFromAgingData(Data.IAccountingContext accountingContext, IQueryable<string> mainAccountIdList_ToFetchThenAggragrate, IQueryable<GLAccount> qAllAccAging4AccountTypeCode_CustomerOrVendor, string _AccountingCurrencyId)
        {
            this._AccountingContext = accountingContext;
            this._MainAccountIdList_ToFetchThenAggragrate = mainAccountIdList_ToFetchThenAggragrate;
            this._qAllAccAging4AccountTypeCode_CustomerOrVendor = qAllAccAging4AccountTypeCode_CustomerOrVendor;
            this._AccountingCurrencyId = _AccountingCurrencyId;
        }
        public List<PeriodM> GetFromGLAccountAgingData(int tenant,List<DateTime> listPeriods, DateTime myorderLessThanExclusive)//, List<DateTime> listLessThanExclusivePeriods)
        {
            //_MainAccountIdList_ToFetchThenAggragrate
            var gLAccountAgingDataRepository = new GLAccountAgingDataRepository(_AccountingContext); ;
            var periodMs = new List<PeriodM>();
            var qlistGLAccountAgingData = gLAccountAgingDataRepository.GetByIds(tenant, _MainAccountIdList_ToFetchThenAggragrate);

            var qCurrency = (from ac in _qAllAccAging4AccountTypeCode_CustomerOrVendor
                     where _MainAccountIdList_ToFetchThenAggragrate.Contains(ac.Id)
                     where ac.IsMultiCurrency != true
                     select new { ac.Id, ac.CurrencyId });

            var qjoin = (from myGLAccountAgingData in qlistGLAccountAgingData
                         join curr in qCurrency on myGLAccountAgingData.AccountId equals curr.Id
                         into joinGroup 
                         from currencyData in joinGroup.DefaultIfEmpty()
                         select  new {  myGLAccountAgingData , currencyData }
                        );

            Logger.LogDebug("GetFromGLAccountAgingData Query \r\n {0} ", qjoin.ToTraceQuery());
            DateTime start = DateTime.Now;

            var listAgingCurrency = qjoin.ToList();

            Logger.LogDebug("GetFromGLAccountAgingData SUM duration {0} seconds ", (DateTime.Now - start).TotalSeconds);

            
            listPeriods = listPeriods.OrderByDescending(r => r).ToList();
            foreach (var AgingCurrency in listAgingCurrency)
            {
                int iDeltaFromFuture = 0;
                foreach (var dateTime in listPeriods)
                {

                    periodMs.Add(new PeriodM()
                    {
                        OrderDate = dateTime,
                        OrderAfterOpenRecordDueDate = iDeltaFromFuture == 0 ? true : false,
                        AccountId = AgingCurrency.myGLAccountAgingData.AccountId,
                        CurrencyId = AgingCurrency.currencyData!=null? AgingCurrency.currencyData.CurrencyId: _AccountingCurrencyId ,///groupByAccCurrr.Key.CurrencyId,///GLAccount that is not multi Currency Get Foreign 
                        Total = GetValue(AgingCurrency.myGLAccountAgingData, iDeltaFromFuture),
                        OpenCredit = 0,
                        OpenDebit = 0,
                        TotalOpenTransactions = 0,

                    });
                    iDeltaFromFuture++;
                }
                AddPastPeriod(myorderLessThanExclusive, periodMs, AgingCurrency.myGLAccountAgingData, iDeltaFromFuture, AgingCurrency.currencyData?.CurrencyId);

            }

            return periodMs;



        }

        private void AddPastPeriod(DateTime myorderLessThanExclusive, List<PeriodM> periodMs, GLAccountAgingData itemGLAccountAgingData, int iDeltaFromFuture, string CurrencyId)
        {
            decimal totalPast = itemGLAccountAgingData.PeriodPast.GetValueOrDefault();
            while (iDeltaFromFuture <= 6)//_Param.NumberOfmonthsbackwards)
            {
                totalPast += GetValue(itemGLAccountAgingData, iDeltaFromFuture);
                iDeltaFromFuture++;
            }

            //FutureDueDate
            periodMs.Add(new PeriodM()
            {
                OrderDate = myorderLessThanExclusive,
                OrderDateB4 = true,
                AccountId = itemGLAccountAgingData.AccountId,
                CurrencyId = String.IsNullOrWhiteSpace( CurrencyId )?   _AccountingCurrencyId : CurrencyId,///groupByAccCurrr.Key.CurrencyId,///GLAccount that is not multi Currency Get Foreign 
                Total = totalPast,
                OpenCredit = 0,
                OpenDebit = 0,
                TotalOpenTransactions = 0,

            });

        }

        private decimal GetValue(GLAccountAgingData itemGLAccountAgingData, int iDeltaFromFuture)
        {
            switch (iDeltaFromFuture)
            {
                case 0:
                    return itemGLAccountAgingData.PeriodFuture.GetValueOrDefault();
                case 1:
                    return itemGLAccountAgingData.Period0.GetValueOrDefault();
                case 2:
                    return itemGLAccountAgingData.Period1.GetValueOrDefault();
                case 3:
                    return itemGLAccountAgingData.Period2.GetValueOrDefault();
                case 4:
                    return itemGLAccountAgingData.Period3.GetValueOrDefault();
                case 5:
                    return itemGLAccountAgingData.Period4.GetValueOrDefault();
                case 6:
                    return itemGLAccountAgingData.Period5.GetValueOrDefault();

                default:
                    throw new Exception();


            }
        }
    }
}
