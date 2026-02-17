#if false
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public class AgingReportV1Service
    {
        private AgingReportParam _Param;
        private IAccountingContext _AccountingContext;
        public AgingReportV1Service(AgingReportParam param)
        {
            _Param = param;
        }

        public string RunReport()
        {
            CheckeParams();
            var listPeriods = new List<DateTime>();
            //22-02-16  _AgingReportParam.AgingForDate
            DateTime AgingForDateLastMonth1st = _Param.AgingForDate.AddMonths(-1);
            //22-01-16  AgingForDateLastMonth1st
            AgingForDateLastMonth1st = new DateTime(AgingForDateLastMonth1st.Year, AgingForDateLastMonth1st.Month, 1);
            //01-01-16  AgingForDateLastMonth1st NumberOfmonthsbackwards=5

            DateTime lessThan;
            for (int i = 0; i < (int)_Param.NumberOfmonthsbackwards; i++)
            {
                var currMonth = AgingForDateLastMonth1st.AddMonths((-1 * i));
                listPeriods.Add(currMonth);

                //if NumberOfmonthsbackwards==5 then 
                //01-01-16  -0M
                //01-12-15  -1M
                //01-11-15  -2M
                //01-10-15  -3M
                //01-09-15  -4M
            }

            lessThan = listPeriods.OrderBy(d => d).First();
            //lessThanExclusive ==01-09-15  



            List<string> acountListId = null;
            using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(10)))
            {
                _AccountingContext = AccountingContext.GetContext(_Param.Tenant);
                //var qsGLAccountTotalByMonth = new GLAccountTotalByMonthQueryService(_AccountingContext);
                var repoGLAccountTotalByMonth = new GLAccountTotalByMonthRepository(_AccountingContext);

                acountListId = GetAccountList();

                var accountingCurrencyId = AccountingSettingResolver.ResolveAccountingCurrencyId(_Param.Tenant);


                var qTotalByMonthAcc = repoGLAccountTotalByMonth.GetAll(_Param.Tenant)
                     .Where(rec => acountListId.Contains(rec.AccountId));
                if (!_Param.CurrenciesDetailedV1NotInUse.GetValueOrDefault())
                {
                    qTotalByMonthAcc = qTotalByMonthAcc.Where(rec => rec.CurrencyId == accountingCurrencyId);
                }





                var qPeriod =
                    (from period in listPeriods
                     join totalByMonth in qTotalByMonthAcc
                     on new { period.Year, period.Month }
                     equals
                     new { totalByMonth.Year, totalByMonth.Month }
                     select new PeriodM()
                     {
                         OrderDate = period,
                         AccountId = totalByMonth.AccountId,
                         CurrencyId = totalByMonth.CurrencyId,
                         Total = (totalByMonth.LocalAmountDebit - totalByMonth.LocalAmountCredit),
                     });




                var qLessThanExclusiveBasic =
                    (from rec in qTotalByMonthAcc
                     where
                     (rec.Year == lessThan.Year && rec.Month < lessThan.Month)
                     ||
                     rec.Year < lessThan.Year
                     group rec by new { rec.AccountId, rec.CurrencyId } into groupByAccCurrr

                     select new PeriodM()
                     {

                         OrderDate = lessThan,
                         OrderDateB4 = true,
                         AccountId = groupByAccCurrr.Key.AccountId,
                         CurrencyId = groupByAccCurrr.Key.CurrencyId,
                         Total = groupByAccCurrr.Sum(rec => rec.LocalAmountDebit - rec.LocalAmountCredit),
                     });





                var dbList = qPeriod.Union(qLessThanExclusiveBasic).ToList();
                dbList = dbList.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.OrderDate).ThenBy(rec => rec.CurrencyId).ToList();


                if (_Param.AgingMethod == AgingReportParam.MethodEnum.TotalByMonthFIFOMethod.ToString())
                {
                    var totalByMonthFIFO = new List<PeriodM>();
                    var currencyList = dbList.Select(r => r.CurrencyId).Distinct().ToList();
                    foreach (var currency in currencyList)
                    {
                        var currencyAging = dbList.Where(r => r.CurrencyId == currency).ToList();
                        currencyAging = ManipulateFifoPerCurrency(currencyAging);
                        totalByMonthFIFO.AddRange(currencyAging);
                    }
                    dbList = totalByMonthFIFO.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.OrderDate).ThenBy(rec => rec.CurrencyId).ToList();
                }

                dbList = dbList.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.OrderDate).ThenBy(rec => rec.CurrencyId).ToList();

                var orderLessThanExclusive = lessThan; ;//.AddMonths(-1);
                var myorderLessThanExclusive = new DateTime(orderLessThanExclusive.Year, orderLessThanExclusive.Month, 1);
                var listLessThanExclusivePeriods = new List<DateTime>() { orderLessThanExclusive };
                List<PeriodM> dummiesPeriodsList =
                GetAllDummiesPeriods(listPeriods, acountListId, accountingCurrencyId, myorderLessThanExclusive, listLessThanExclusivePeriods);


                var reportList = (from rec in dummiesPeriodsList.Union(dbList)
                                  group rec by new { rec.OrderDate, rec.OrderDateB4, rec.AccountId, rec.CurrencyId }
                                      into groupby
                                      select new PeriodM()
                                      {
                                          OrderDate = groupby.Key.OrderDate,
                                          OrderDateB4 = groupby.Key.OrderDateB4,
                                          AccountId = groupby.Key.AccountId,
                                          CurrencyId = groupby.Key.CurrencyId,
                                          Total = groupby.Sum(rec => rec.Total)
                                      }
                 ).ToList();


                reportList = (from r in reportList
                              orderby r.OrderDate, r.OrderDateB4 descending
                              select r
                              ).ToList();

                DataTable _PivotTable = reportList.ToPivotTable(
                    rec => rec.PeriodName,
                    rec => rec.AccountAndCurr, //new { rec.AccountId, rec.CurrencyId }, //rec.AccountId, //
                    recs => recs.Any() ? recs.Sum(rec => rec.Total) : 0);
                var xml = _PivotTable.ToJsonString();
                _PivotTable.TableName = "sss";
                xml = _PivotTable.ToXml();
                return xml;
            }
        }

        private List<PeriodM> ManipulateFifoPerCurrency(List<PeriodM> dbList)
        {
            if (dbList.Select(r => r.CurrencyId).Distinct().Count() != 1)
            {
                throw new Exception("ManipulateFifoPerCurrency:::dbList.Select( r=>r.CurrencyId).Distinct().Count()!=1");
            }
            var firstPlus = dbList.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.OrderDate).ThenBy(rec => rec.CurrencyId).ToList();
            return null;
        }

        private List<string> GetAccountList()
        {
            var acountListId = new List<string>();

            if (!string.IsNullOrWhiteSpace(_Param.CustomerId))
            {
                acountListId = new List<string>() { _Param.CustomerId };
            }
            else
            {
                var qsChartOfAccount = new ChartOfAccountQueryService(_AccountingContext);
                var res = qsChartOfAccount.GetChartOfAccountChildRecursive(_Param.ChartOfAccountIdV1NotInUse, _Param.Tenant);
                var chartOfAccountIdList = res.Select(rec => rec.Id).ToList();
                var qsGLAccount = new GLAccountQueryService(_AccountingContext);

                var listGLAccount = qsGLAccount.GetChildAccountsByChartOfAccountIdList(chartOfAccountIdList, _Param.Tenant);
                var listGLAccountid = listGLAccount.Select(rec => rec.Id).ToList();
                acountListId = new List<string>(listGLAccountid);
            }

            bool testMulti = false;
            if (testMulti)
            {
                acountListId = new List<string>(){
                    "1-33", "1-26","1-33","1-26","1-30","1-33","1-26" , "1-33"};
            }
            acountListId = acountListId.Distinct().ToList();

            return acountListId;
        }

        private static List<PeriodM> GetAllDummiesPeriods(List<DateTime> listPeriods, List<string> acountListId, string AccountingCurrencyId, DateTime myOrderDate, List<DateTime> listLessThanExclusivePeriods)
        {
            var cartesianProductPeriodAccount =
            (from period in listPeriods
             from AccountId in acountListId
             orderby period, AccountId
             select new PeriodM { AccountId = AccountId, OrderDate = period, CurrencyId = AccountingCurrencyId }
                                ).ToList();


            var cartesianProductLessThanExclusiveAccount =
                (from period in listLessThanExclusivePeriods
                 from AccountId in acountListId
                 orderby period, AccountId
                 select new PeriodM()
                 {
                     OrderDate = myOrderDate,
                     OrderDateB4 = true,
                     AccountId = AccountId,
                     CurrencyId = AccountingCurrencyId,
                     Total = 0,

                 })
                 .ToList();
            return cartesianProductPeriodAccount.Union(cartesianProductLessThanExclusiveAccount).ToList();


        }



        private static IEnumerable<PeriodM> NewMethod(List<DateTime> listPeriods, List<string> acountListId, IQueryable<Data.EntityPOCOs.GLAccountTotalByMonth> qTotalByMonthAcc, string AccountingCurrencyId)
        {
            var cartesianProductPeriodAccount =
                (from period in listPeriods
                 from AccountId in acountListId
                 orderby period, AccountId
                 select new { AccountId = AccountId, period, CurrencyId = AccountingCurrencyId }
                                    ).ToList();
            var qPeriod =
                (from periodAccount in cartesianProductPeriodAccount
                 join totalByMonth in qTotalByMonthAcc
                 on new { periodAccount.period.Year, periodAccount.period.Month, periodAccount.AccountId, periodAccount.CurrencyId }
                 equals
                 new { totalByMonth.Year, totalByMonth.Month, totalByMonth.AccountId, totalByMonth.CurrencyId }
                 into JoinR
                 from totalByMonthJoinR in JoinR.DefaultIfEmpty()

                 select new PeriodM()
                 {


                     OrderDate = periodAccount.period,
                     AccountId = totalByMonthJoinR == null ? periodAccount.AccountId : totalByMonthJoinR.AccountId,
                     CurrencyId = totalByMonthJoinR == null ? "" : totalByMonthJoinR.CurrencyId,
                     Total = totalByMonthJoinR == null ? 0 : (totalByMonthJoinR.LocalAmountDebit - totalByMonthJoinR.LocalAmountCredit),

                 });
            return qPeriod;
        }


        /*
Period	Acc	Currency	Total
3.2016	10012	USD	100.22
2.2016	10012	USD	111
1.2016	10012	USD	2522
11.2015	10012	USD	11
10.2015	10012	USD	6
9.2015	10012	USD	-2
8.2015	10012	USD	5
3.2016	10012	EUR	6
2.2016	10012	EUR	77
1.2016	10012	EUR	257
11.2015	10012	EUR	11
10.2015	10012	EUR	12
9.2015	10012	EUR	-2
8.2015	10012	EUR	33
3.2016	10099	USD	7
2.2016	10099	USD	75
1.2016	10099	USD	46
11.2015	10099	USD	11
10.2015	10099	USD	12
9.2015	10099	USD	-2
8.2015	10099	USD	33
3.2016	10099	EUR	689
2.2016	10099	EUR	111
1.2016	10099	EUR	2722
11.2015	10099	EUR	11
10.2015	10099	EUR	67766
9.2015	10099	EUR	-2
8.2015	10099	EUR	33

         * 
         * 
    Sum of Total	Column Labels							
    Row Labels	1.2016	2.2016	3.2016	8.2015	9.2015	10.2015	11.2015	Grand Total
    10012	2779	188	106.22	38	-4	18	22	3147.22
    EUR	257	77	6	33	-2	12	11	394
    USD	2522	111	100.22	5	-2	6	11	2753.22
    10099	2768	186	696	66	-4	67778	22	71512
    EUR	2722	111	689	33	-2	67766	11	71330
    USD	46	75	7	33	-2	12	11	182
    Grand Total	5547	374	802.22	104	-8	67796	44	74659.22
         */





        private void CheckeParams()
        {
            if (_Param.NumberOfmonthsbackwards < 1)
            {
                throw new Exception("NumberOfmonthsbackwards<1");
            }
            if (string.IsNullOrWhiteSpace(_Param.CustomerId) && string.IsNullOrWhiteSpace(_Param.ChartOfAccountIdV1NotInUse))
            {
                throw new Exception("string.IsNullOrWhiteSpace(_AgingReportParam.CustomerId) && string.IsNullOrWhiteSpace(_AgingReportParam.ChartOfAccountId)");
            }
        }

    }
}
#endif