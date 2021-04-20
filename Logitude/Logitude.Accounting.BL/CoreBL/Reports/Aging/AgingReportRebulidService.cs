using Logitude.Accounting.BL.CoreBL.Mapping;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Logitude.Server.Tools.Helpers;
using System.Diagnostics;
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using System.Data.Entity.SqlServer;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.Helpers;
using Logitude.Server.Tools;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityUpdateServices;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public class AgingReportRebulidService
    {
        private AgingReportRebulidParam _Param;
        private IAccountingContext _AccountingContext;
        IQueryable<string> /*_MainAccountIdList_withoutAccountThatHave2Aggregate_1RelatedAccCurrencies*/
            _MainAccountIdList_ToFetchThenAggragrate = null;
        //private IQueryable<Logitude.Accounting.Def.EntityPMs.GLAccountPM> _GLAccounts;
        private IQueryable<string> _AccountListId_PleaseTake_ForeignAmount;
        private IQueryable<GLAccountCurrency> _AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount;//Due Param IsAggregate - from GLAccountCurrency get TotalByMonth/Reconcile But Imitate As Parent
        private string _AccountingCurrencyId;
        private IQueryable<GLAccount> _qAllAccAging4AccountTypeCode_CustomerOrVendor;
        private GLAccountQueryService _myGLAccountQueryService;
        private GLAccountRepository _myGLAccountRepository;
        private List<GLAccount> _GLAccountChildren_UseToAggregateAsLocalAmount;
        private bool _TESTIT;
        private List<string> _AccountListRelatedCurrenciesAccount_List2Discard;

        //private bool _TryGetAllThenAggregate = true;

        public AgingReportRebulidService(AgingReportRebulidParam param)
        {
            _Param = param;
            

        }

    

        public void RunReport()
        {
            CheckeParams();

            List<DateTime> listPeriods;
            DateTime lessThan, graterThen_OpenTransactionsFutureDueDate;

            var listPeriodService = new ListPeriodService(_Param);
            listPeriodService.GetListPeriods(out listPeriods, out lessThan, out graterThen_OpenTransactionsFutureDueDate);

            IQueryable<GLAccountTotalByMonthsDTOAging> qTotalByMonthAcc = null;


            using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(10)))
            {
                _AccountingContext = AccountingContext.GetContext(_Param.Tenant);
                (_AccountingContext as System.Data.Entity.DbContext).Database.CommandTimeout = 300;

                //var qsGLAccountTotalByMonth = new GLAccountTotalByMonthQueryService(_AccountingContext);
                var repoGLAccountTotalByMonth = new GLAccountTotalByMonthRepository(_AccountingContext);
                var repoLedgerTransactionRepository = new LedgerTransactionRepository(_AccountingContext);
                _myGLAccountQueryService = new GLAccountQueryService(_AccountingContext);
                _myGLAccountRepository = new GLAccountRepository(_AccountingContext);


                if (!FilterAccountPopulation())
                {
                    return /*string.Empty*/;// no accounts 
                }

                _AccountingCurrencyId = (new AccountingSettingResolver()).ResolveAccountingCurrencyId(_Param.Tenant);




                //if (_Param.AgingMethod == AgingReportRebulidParam.MethodEnum.ReconcileOpenBalanceMethod.ToString())
                {

                    qTotalByMonthAcc = repoLedgerTransactionRepository
                        .GetQuerableReconcileOpenBalanceAsTotalByMonth(
                        _Param.Tenant,
                        _MainAccountIdList_ToFetchThenAggragrate,
                        null,//this._TryGetAllThenAggregate ? null : _AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount,
                        _AccountingCurrencyId,
                        false
                        );
                    if (_TESTIT)
                    {
                        var ss = qTotalByMonthAcc.ToList();
                    }
                    GLAccountReconcileDefintionChanged(qTotalByMonthAcc);
                }



                var qTotalByMonthAcc_NOTMultiCurrencySooGet_ForeignAmount = ///GLAccount that is not multi Currency Get Foreign 
                    qTotalByMonthAcc
                    .Where(rec => _AccountListId_PleaseTake_ForeignAmount.Contains(rec.AccountId));


                var qTotalByMonthAcc_ISMultiCurrencySooGet_LocalAmount = ///GLAccount that is not multi Currency Get Foreign 
                    qTotalByMonthAcc
                    .Where(rec => !_AccountListId_PleaseTake_ForeignAmount.Contains(rec.AccountId));



                var qPeriodInLocalAmount =
                (from period in listPeriods
                 join totalByMonth in /*qTotalByMonthAcc*/ qTotalByMonthAcc_ISMultiCurrencySooGet_LocalAmount
                 on new { period.Year, period.Month }
                 equals
                 new { totalByMonth.Year, totalByMonth.Month }
                 select new PeriodM()
                 {
                     OrderDate = period,
                     AccountId = totalByMonth.AccountId,
                     CurrencyId = _AccountingCurrencyId,///GLAccount that is multi Currency LocalAmount 
                     Total = (totalByMonth.LocalAmountDebit - totalByMonth.LocalAmountCredit),
                     OpenCredit = totalByMonth.LocalAmountCredit,
                     OpenDebit = totalByMonth.LocalAmountDebit,
                     TotalOpenTransactions = totalByMonth.TotalOpenTransactions,
                 });





                var qPeriodInForeign =
                    (from period in listPeriods
                     join totalByMonth in qTotalByMonthAcc_NOTMultiCurrencySooGet_ForeignAmount
                     on new { period.Year, period.Month }
                     equals
                     new { totalByMonth.Year, totalByMonth.Month }
                     select new PeriodM()
                     {
                         OrderDate = period,
                         AccountId = totalByMonth.AccountId,
                         CurrencyId = totalByMonth.CurrencyId,///GLAccount that is not multi Currency Get Foreign 
                         Total = ((decimal)totalByMonth.ForeignAmountDebit - (decimal)totalByMonth.ForeignAmountCredit),
                         OpenCredit = (decimal?)totalByMonth.ForeignAmountCredit ?? 0,
                         OpenDebit = (decimal?)totalByMonth.ForeignAmountDebit ?? 0,
                         TotalOpenTransactions = totalByMonth.TotalOpenTransactions,
                     });
                if (_TESTIT)
                {
                    var _222 = qPeriodInForeign.ToList();
                }

                var qLessThanExclusiveBasicInLocal =
                    (from rec in /*qTotalByMonthAcc*/ qTotalByMonthAcc_ISMultiCurrencySooGet_LocalAmount
                     where
                     (rec.Year == lessThan.Year && rec.Month < lessThan.Month)
                     ||
                     rec.Year < lessThan.Year
                     group rec by new { rec.AccountId } into groupByAccCurrr

                     select new PeriodM()
                     {

                         OrderDate = lessThan,
                         OrderDateB4 = true,
                         AccountId = groupByAccCurrr.Key.AccountId,
                         CurrencyId = _AccountingCurrencyId,
                         Total = groupByAccCurrr.Sum(rec => rec.LocalAmountDebit - rec.LocalAmountCredit),
                         OpenCredit = groupByAccCurrr.Sum(rec => rec.LocalAmountCredit),
                         OpenDebit = groupByAccCurrr.Sum(rec => rec.LocalAmountDebit),
                         TotalOpenTransactions = groupByAccCurrr.Sum(rec => rec.TotalOpenTransactions),


                     });


                var qLessThanExclusiveBasicInForeign =
                    (from rec in qTotalByMonthAcc_NOTMultiCurrencySooGet_ForeignAmount///GLAccount that is not multi Currency Get Foreign 
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
                         CurrencyId = groupByAccCurrr.Key.CurrencyId,///GLAccount that is not multi Currency Get Foreign 
                         Total = groupByAccCurrr.Sum(rec => (decimal)rec.ForeignAmountDebit - (decimal)rec.ForeignAmountCredit),
                         OpenCredit = groupByAccCurrr.Sum(rec => rec.ForeignAmountCredit),
                         OpenDebit = groupByAccCurrr.Sum(rec => rec.ForeignAmountDebit),
                         TotalOpenTransactions = groupByAccCurrr.Sum(r => r.TotalOpenTransactions),
                     });
                var qOpenTransactionsFutureDueDate_InLocal = Enumerable.Empty<PeriodM>().AsQueryable();
                var qOpenTransactionsFutureDueDate_InForeign = Enumerable.Empty<PeriodM>().AsQueryable();
                if (ToCalcOpenTransactionsFutureDueDate())
                {
                    qOpenTransactionsFutureDueDate_InLocal = GetOpenTransactionsFutureDueDate_InLocal(graterThen_OpenTransactionsFutureDueDate, qTotalByMonthAcc_ISMultiCurrencySooGet_LocalAmount);
                    qOpenTransactionsFutureDueDate_InForeign = GetqOpenTransactionsFutureDueDate_InForeign(graterThen_OpenTransactionsFutureDueDate, qTotalByMonthAcc_NOTMultiCurrencySooGet_ForeignAmount);
                }
                if (_TESTIT)
                {
                    var _1 = qPeriodInLocalAmount.ToList();
                    var _2 = qLessThanExclusiveBasicInLocal.ToList();
                    var _3 = qPeriodInForeign.ToList();
                    var _4 = qLessThanExclusiveBasicInForeign.ToList();

                    var _5 = qOpenTransactionsFutureDueDate_InLocal.ToList();
                    var _6 = qOpenTransactionsFutureDueDate_InForeign.ToList();
                }
                var theDBList =
                    qPeriodInLocalAmount.Union(qLessThanExclusiveBasicInLocal)
                    .Union(qPeriodInForeign).Union(qLessThanExclusiveBasicInForeign)
                    .Union(qOpenTransactionsFutureDueDate_InLocal).Union(qOpenTransactionsFutureDueDate_InForeign)
                    .ToList();
                theDBList = theDBList.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.OrderDate).ThenBy(rec => rec.CurrencyId)
                    .ToList();





                theDBList = theDBList.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.OrderDate).ThenBy(rec => rec.CurrencyId).ToList();

                var orderLessThanExclusive = lessThan; ;//.AddMonths(-1);
                var myorderLessThanExclusive = new DateTime(orderLessThanExclusive.Year, orderLessThanExclusive.Month, 1);
                var listLessThanExclusivePeriods = new List<DateTime>() { orderLessThanExclusive };
                listPeriods.Add(graterThen_OpenTransactionsFutureDueDate);
                List<PeriodM> dummiesPeriodsList = BuildDummiesPeriod(listPeriods, myorderLessThanExclusive, listLessThanExclusivePeriods);
                

                //var dummiesWithoutDBRecord=  dummiesPeriodsList.Where(dummy => dbList.Any(db => db.AccountId != dummy.AccountId));
                var DBAndDummies = //dummiesWithoutDBRecord.Union(dbList);
                    dummiesPeriodsList.Union(theDBList);

                if (_AccountListRelatedCurrenciesAccount_List2Discard != null)
                {
                    DBAndDummies = DBAndDummies.Where(r => !_AccountListRelatedCurrenciesAccount_List2Discard.Contains(r.AccountId));

                }

                var reportList = (from rec in /*dummiesPeriodsList.Union(dbList)*/ DBAndDummies
                                  group rec by new { rec.OrderDate, rec.OrderDateB4, rec.AccountId, rec.CurrencyId, rec.SplitAccountId }
                                      into groupby
                                  select new PeriodM()
                                  {
                                      OrderDate = groupby.Key.OrderDate,
                                      OrderDateB4 = groupby.Key.OrderDateB4,
                                      AccountId = groupby.Key.AccountId,
                                      SplitAccountId = groupby.Key.SplitAccountId,
                                      CurrencyId = groupby.Key.CurrencyId,
                                      Total = groupby.Sum(rec => rec.Total),
                                      OpenCredit = groupby.Sum(rec => rec.OpenCredit),
                                      OpenDebit = groupby.Sum(rec => rec.OpenDebit),
                                      TotalOpenTransactions = groupby.Sum(rec => rec.TotalOpenTransactions),


                                  }
                 ).ToList();


                reportList = (from r in reportList
                              orderby r.OrderDate, r.OrderDateB4 descending
                              select r
                              ).ToList();

                // Adding accounts names





                TenantQuery tenantQuery = new TenantQuery(_Param.Tenant);
                var tenant = tenantQuery.GetSinglePM(_Param.Tenant);

                DateTime currentDate = TenantServerConfigration.GetCurrentDateTime(_Param.Tenant);




                ///var list1=periodMExtendeds.ToList();
                bool checkIt = false;
                if (checkIt)
                {
                    var A = reportList.GroupBy(r => r.AccountId).Count();
                    ///var b = periodMExtendeds.GroupBy(r => r.AccountId).Count();

                }
                ///if (ToCalcOpenTransactionsFutureDueDate())
                {

                    reportList.ForEach(r =>
                    {

                        if (r.OrderDate == graterThen_OpenTransactionsFutureDueDate)
                        {
                            r.OrderAfterOpenrECODueDate = true;
                        }
                    });



                }

                MyPeriodList = reportList;
                //MyPeriodExtendedList = namedPeriods;

                //return xml;
            }
        }

        

        public List<GLAccountAgingDataPM> RebuildGLAccountAgingData()
        {
            using (var tran = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(5)))
            {
                _AccountingContext = AccountingContext.GetContext(_Param.Tenant);
                (_AccountingContext as System.Data.Entity.DbContext).Database.CommandTimeout = 300;
                _myGLAccountRepository = new GLAccountRepository(_AccountingContext);

                var accType = GetAccountType(_Param.Aging4AccountTypeCode);
                _qAllAccAging4AccountTypeCode_CustomerOrVendor = _myGLAccountRepository
                    //.GetQAllByAccountTypeCode(_Param.Tenant, GetAccountType(_Param.Aging4AccountTypeCode));
                    .GetAll(_Param.Tenant)
                    .Where(a => a.AccountTypeCode == accType);

                if (!String.IsNullOrWhiteSpace(_Param.VendorCustomerId))
                {
                    _qAllAccAging4AccountTypeCode_CustomerOrVendor = (from acc in _qAllAccAging4AccountTypeCode_CustomerOrVendor
                                                                      where acc.Id == _Param.VendorCustomerId
                                                                      select acc
                    );
                }

                var qs = new GLAccountAgingDataQueryService(_AccountingContext as IAccountingContext);
                List<GLAccountAgingDataPM> dbGLAccountAgingDataPMs = qs.Get(_Param.Tenant, _qAllAccAging4AccountTypeCode_CustomerOrVendor);

                var calcGLAccountAgingDataPMs = MapPeriod2AgingData(_Param.Tenant, this.MyPeriodList);
                var notEqualPMs =
                    (from db in dbGLAccountAgingDataPMs
                     join calc in calcGLAccountAgingDataPMs
                     on db.AccountId equals calc.AccountId
                     where
                     Math.Abs(db.PeriodPast.GetValueOrDefault() - calc.PeriodPast.GetValueOrDefault()) > 0.01m
                     || Math.Abs(db.Period0.GetValueOrDefault() - calc.Period0.GetValueOrDefault()) > 0.01m
                     || Math.Abs(db.Period1.GetValueOrDefault() - calc.Period1.GetValueOrDefault()) > 0.01m
                     || Math.Abs(db.Period2.GetValueOrDefault() - calc.Period2.GetValueOrDefault()) > 0.01m
                     || Math.Abs(db.Period3.GetValueOrDefault() - calc.Period3.GetValueOrDefault()) > 0.01m
                     || Math.Abs(db.Period4.GetValueOrDefault() - calc.Period4.GetValueOrDefault()) > 0.01m
                     || Math.Abs(db.Period5.GetValueOrDefault() - calc.Period5.GetValueOrDefault()) > 0.01m
                     || Math.Abs(db.PeriodFuture.GetValueOrDefault() - calc.PeriodFuture.GetValueOrDefault()) > 0.01m
                     || Math.Abs(db.TotalOpenTransactions.GetValueOrDefault() - calc.TotalOpenTransactions.GetValueOrDefault()) > 0.01m
                     select calc
                     ).ToList();
                LogMessagingUtil.Instance.AppendLine($"updATE notEqualPMs {notEqualPMs.Count()}");

                var us = new GLAccountAgingDataUpdateService(_AccountingContext as IAccountingContext,new Dictionary<string, Simplog.Server.Infrastructure.IContext>(),_Param.Tenant);
                us.UpdateMulti(notEqualPMs, new List<GLAccountAgingDataPM>(), new EntityPM(), false);
                _AccountingContext.SaveChanges();
                tran.Complete();

                return notEqualPMs;
            }


        }

        public static List<GLAccountAgingDataPM> MapPeriod2AgingData(int tenant,List<PeriodM> myPeriodList)
        {
            var calcGLAccountAgingDataPMs = new List<GLAccountAgingDataPM>();
            var qGAccountId = myPeriodList.GroupBy(r => r.AccountId);

            foreach (var periodMs in qGAccountId)
            {
                var listperiodMs = periodMs.ToList();
                if (periodMs.Select(r => r.CurrencyId).Distinct().Count() > 2)
                {
                    throw new Exception("only 1 CurrencyId");
                }
                if (periodMs.Count() != 8)
                {
                    listperiodMs = (from a in listperiodMs
                                    group a by new { a.AccountId, a.OrderDate, a.OrderDateB4 } into g

                                    select new PeriodM {
                                        AccountId= g.Key.AccountId, OrderDate= g.Key.OrderDate, OrderDateB4= g.Key.OrderDateB4,


                                        Total = g.Sum(r => r.Total) ,
                                        TotalOpenTransactions =g.Sum(r=>r.TotalOpenTransactions) }
                                    ).ToList();
                }
                if (listperiodMs.Count()!=8)
                {
                    throw new Exception("if (listperiodMs.Count()!=8)");
                }
                var pm = new GLAccountAgingDataPM()
                {
                    ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                    AccountId = periodMs.Key,
                    Tenant = tenant,
                    TotalOpenTransactions = listperiodMs.Sum(r => r.TotalOpenTransactions),
                    PeriodPast = listperiodMs.Skip(0).First().Total,
                    Period0 = listperiodMs.Skip(1).First().Total,
                    Period1 = listperiodMs.Skip(2).First().Total,
                    Period2 = listperiodMs.Skip(3).First().Total,
                    Period3 = listperiodMs.Skip(4).First().Total,
                    Period4 = listperiodMs.Skip(5).First().Total,
                    Period5 = listperiodMs.Skip(6).First().Total,
                    PeriodFuture = listperiodMs.Skip(7).First().Total,

                };
                calcGLAccountAgingDataPMs.Add(pm);

            }
            return calcGLAccountAgingDataPMs;
        }

        private List<CurrencySum> GetBalance(IQueryable<string> listOfAccountId, string dateTypeCode)
        {
            if (listOfAccountId is null)
            {
                throw new ArgumentNullException(nameof(listOfAccountId));
            }

            if (string.IsNullOrWhiteSpace(dateTypeCode))
            {
                throw new ArgumentException("message", nameof(dateTypeCode));
            }

            int tenant = _Param.Tenant;
            //DateTime endOfYearUserInput = _Param.AgingForDate.Date;
            var endAccountBalanceService = new AccountBalanceByDateCodeService(_AccountingContext, tenant, listOfAccountId.First(),
                 listOfAccountId
                );

            //var CalculateBalanceIsNotIncludeSo_endOfYearUserInputPlus1 = endOfYearUserInput.AddDays(1);
            bool openBalancePlease_ReCalcYearTransfer = true;//yaron :irrlavant end of year

            endAccountBalanceService.CalculateBalance(
true,
dateTypeCode /*GLAccountTotalDateTypeValues.Accountingdate*/,
_Param.AgingForDate.Date, false, true, true, false);


            var totals = (from rec in endAccountBalanceService.AccountBalance.verbose.CurrencySumUntillMounth.Union(endAccountBalanceService.AccountBalance.verbose.TheMounthCurrencySum)
                          group rec by new
                          {
                              rec.AccountId
                              //, rec.CurrencyId
                          }
                          into gCurrency
                          select new CurrencySum()
                          {
                              AccountId = gCurrency.Key.AccountId,
                              //CurrencyId = gCurrency.Key.CurrencyId,
                              LocalAmountCredit = gCurrency.Sum(rec => rec.LocalAmountCredit),
                              LocalAmountDebit = gCurrency.Sum(rec => rec.LocalAmountDebit),
                              ForeignAmountCredit = gCurrency.Sum(rec => rec.ForeignAmountCredit),
                              ForeignAmountDebit = gCurrency.Sum(rec => rec.ForeignAmountDebit)
                          }
                    ).ToList();


            //totals = (from rec in totals
            //          where
            //              (
            //              (rec.LocalAmountDebit - rec.LocalAmountCredit) != 0
            //              ||
            //              (rec.ForeignAmountDebit - rec.ForeignAmountCredit) != 0
            //              )
            //          select rec)
            //          .ToList();


            return totals;
        }
        private static IQueryable<PeriodM> GetqOpenTransactionsFutureDueDate_InForeign(DateTime graterThen_OpenTransactionsFutureDueDate, IQueryable<GLAccountTotalByMonthsDTOAging> qTotalByMonthAcc_NOTMultiCurrencySooGet_ForeignAmount)
        {
            IQueryable<PeriodM> qOpenTransactionsFutureDueDate_InForeign;
            qOpenTransactionsFutureDueDate_InForeign =
                (from rec in qTotalByMonthAcc_NOTMultiCurrencySooGet_ForeignAmount///GLAccount that is not multi Currency Get Foreign 
                 where
             (rec.Year == graterThen_OpenTransactionsFutureDueDate.Year && rec.Month >= graterThen_OpenTransactionsFutureDueDate.Month)
    ||
    rec.Year > graterThen_OpenTransactionsFutureDueDate.Year
                 group rec by new { rec.AccountId, rec.CurrencyId } into groupByAccCurrr

                 select new PeriodM()
                 {

                     OrderDate = graterThen_OpenTransactionsFutureDueDate,
                     OrderAfterOpenrECODueDate = true,
                     AccountId = groupByAccCurrr.Key.AccountId,
                     CurrencyId = groupByAccCurrr.Key.CurrencyId,///GLAccount that is not multi Currency Get Foreign 
                     Total = groupByAccCurrr.Sum(rec => (decimal)rec.ForeignAmountDebit - (decimal)rec.ForeignAmountCredit),
                     OpenCredit = groupByAccCurrr.Sum(rec => rec.ForeignAmountCredit),
                     OpenDebit = groupByAccCurrr.Sum(rec => rec.ForeignAmountDebit),

                     TotalOpenTransactions = groupByAccCurrr.Sum(rec => rec.TotalOpenTransactions),
                 });
            return qOpenTransactionsFutureDueDate_InForeign;
        }

        private IQueryable<PeriodM> GetOpenTransactionsFutureDueDate_InLocal(DateTime graterThen_OpenTransactionsFutureDueDate, IQueryable<GLAccountTotalByMonthsDTOAging> qTotalByMonthAcc_ISMultiCurrencySooGet_LocalAmount)
        {
            return (from rec in qTotalByMonthAcc_ISMultiCurrencySooGet_LocalAmount
                    where
                    (rec.Year == graterThen_OpenTransactionsFutureDueDate.Year && rec.Month >= graterThen_OpenTransactionsFutureDueDate.Month)
                    ||
                    rec.Year > graterThen_OpenTransactionsFutureDueDate.Year
                    group rec by new { rec.AccountId } into groupByAccCurrr

                    select new PeriodM()
                    {

                        OrderDate = graterThen_OpenTransactionsFutureDueDate,
                        OrderAfterOpenrECODueDate = true,
                        AccountId = groupByAccCurrr.Key.AccountId,
                        CurrencyId = _AccountingCurrencyId,
                        Total = groupByAccCurrr.Sum(rec => rec.LocalAmountDebit - rec.LocalAmountCredit),
                        OpenCredit = groupByAccCurrr.Sum(rec => rec.LocalAmountCredit),
                        OpenDebit = groupByAccCurrr.Sum(rec => rec.LocalAmountDebit),

                        TotalOpenTransactions = groupByAccCurrr.Sum(rec => rec.TotalOpenTransactions),

                    });
        }

        private static bool ToCalcOpenTransactionsFutureDueDate()
        {
            return true;
        }


        //private static IQueryable<PeriodM> GetqOpenTransactionsFutureDueDate_InForeign(DateTime graterThen_OpenTransactionsFutureDueDate, IQueryable<GLAccountTotalByMonthsDTOAging> qTotalByMonthAcc_NOTMultiCurrencySooGet_ForeignAmount)
        //{
        //    IQueryable<PeriodM> qOpenTransactionsFutureDueDate_InForeign;
        //    qOpenTransactionsFutureDueDate_InForeign =
        //        (from rec in qTotalByMonthAcc_NOTMultiCurrencySooGet_ForeignAmount///GLAccount that is not multi Currency Get Foreign 
        //                 where
        //             (rec.Year == graterThen_OpenTransactionsFutureDueDate.Year && rec.Month > graterThen_OpenTransactionsFutureDueDate.Month)
        //    ||
        //    rec.Year > graterThen_OpenTransactionsFutureDueDate.Year
        //         group rec by new { rec.AccountId, rec.CurrencyId } into groupByAccCurrr

        //         select new PeriodM()
        //         {

        //             OrderDate = graterThen_OpenTransactionsFutureDueDate,
        //             OrderAfterOpenrECODueDate = true,
        //             AccountId = groupByAccCurrr.Key.AccountId,
        //             CurrencyId = groupByAccCurrr.Key.CurrencyId,///GLAccount that is not multi Currency Get Foreign 
        //                     Total = groupByAccCurrr.Sum(rec => (decimal)rec.ForeignAmountDebit - (decimal)rec.ForeignAmountCredit),
        //             OpenCredit = groupByAccCurrr.Sum(rec => rec.ForeignAmountCredit),
        //             OpenDebit = groupByAccCurrr.Sum(rec => rec.ForeignAmountDebit)
        //         });
        //    return qOpenTransactionsFutureDueDate_InForeign;
        //}

        //private IQueryable<PeriodM> GetOpenTransactionsFutureDueDate_InLocal(DateTime graterThen_OpenTransactionsFutureDueDate, IQueryable<GLAccountTotalByMonthsDTOAging> qTotalByMonthAcc_ISMultiCurrencySooGet_LocalAmount)
        //{
        //    return (from rec in qTotalByMonthAcc_ISMultiCurrencySooGet_LocalAmount
        //            where
        //            (rec.Year == graterThen_OpenTransactionsFutureDueDate.Year && rec.Month > graterThen_OpenTransactionsFutureDueDate.Month)
        //            ||
        //            rec.Year > graterThen_OpenTransactionsFutureDueDate.Year
        //            group rec by new { rec.AccountId } into groupByAccCurrr

        //            select new PeriodM()
        //            {

        //                OrderDate = graterThen_OpenTransactionsFutureDueDate,
        //                OrderAfterOpenrECODueDate = true,
        //                AccountId = groupByAccCurrr.Key.AccountId,
        //                CurrencyId = _AccountingCurrencyId,
        //                Total = groupByAccCurrr.Sum(rec => rec.LocalAmountDebit - rec.LocalAmountCredit),
        //                OpenCredit = groupByAccCurrr.Sum(rec => rec.LocalAmountCredit),
        //                OpenDebit = groupByAccCurrr.Sum(rec => rec.LocalAmountDebit),

        //            });
        //}

        //private bool ToCalcOpenTransactionsFutureDueDate()
        //{
        //    return (_Param.AgingMethod == AgingReportRebulidParam.MethodEnum.ReconcileOpenBalanceMethod.ToString()
        //                        && _Param.GroupByDate == AgingReportRebulidParam.DateEnum.DueDate);
        //}

   
    
  
        private List<PeriodM> BuildDummiesPeriod(List<DateTime> listPeriods, DateTime myorderLessThanExclusive, List<DateTime> listLessThanExclusivePeriods)
        {
            IQueryable<AccountCurrency> accountCurrencyList = null;

            var accountCurrencyList_MultiUseAccountingCurrencyId =
            _MainAccountIdList_ToFetchThenAggragrate
            .Where(acc => !_AccountListId_PleaseTake_ForeignAmount.Contains(acc))
            .Select(acc => new AccountCurrency() { AccountId = acc, CurrencyId = _AccountingCurrencyId });

            


            accountCurrencyList = accountCurrencyList_MultiUseAccountingCurrencyId;

            var accountCurrencyListThat_RNotMultiCurrency = _myGLAccountRepository.//GetByGLAccountsIdList(_AccountListIdThatRNotMultiCurrency.ToList(), _Param.Tenant)
               GetAll(_Param.Tenant)
               .Where(r => _AccountListId_PleaseTake_ForeignAmount.Contains(r.Id))
               .Select(acc => new AccountCurrency() { AccountId = acc.Id, CurrencyId = acc.CurrencyId });
            accountCurrencyList = accountCurrencyList.Union(accountCurrencyListThat_RNotMultiCurrency);
            ;


            List<PeriodM> dummiesPeriodsList =
        GetDummiesPeriods(listPeriods,
         /*_AccountIdPopulation_LocalAmount, _AccountingCurrencyId */ accountCurrencyList
        , myorderLessThanExclusive, listLessThanExclusivePeriods);


            return dummiesPeriodsList;
        }


        private static void GLAccountReconcileDefintionChanged(IQueryable<GLAccountTotalByMonthsDTOAging> qTotalByMonthAcc)
        {
            ///Basel Arkaia <basel@AMITAL.CO.IL> Aging by reconciliation **** Check **** לשנות בטווח הבדיקה LEDEGER  את שדה OPENAMOUNTCUZRZRENCYID  לשונה מהגדרה שב GALACCOUNT
            //במידה ואני מזהה יבוצע קריסה של הדוח  ...//

            var qCheck = (from tot in qTotalByMonthAcc
                          group tot by new
                          {
                              tot.Tenant,
                              tot.AccountId,


                              //public string CurrencyId { get; set; }

                              tot.Year,
                              tot.Month

                          } into g
                          where g.Count() > 1
                          select new
                          {
                              g.Key.AccountId,
                              Year = g.Key.Year.ToString(),
                              Month = g.Key.Month.ToString(),
                              CurrList = g.Select(r => r.CurrencyId)
                          }

                    );

            //    
            var listEx = qCheck.ToList();
            if (listEx.Any())
            {
                var exString = listEx
                    .DefaultIfEmpty()
                    .Select(g =>
                        "For Account " + g.AccountId + " Date " + g.Year.ToString() + "." + g.Month.ToString()
                        + " There is MultiCurrency (GLAccount Reconcile Defintion Changed ) " + g.CurrList.Aggregate((b, f) => b + "," + f))
                        .Aggregate((b, a) => string.Concat(b, Environment.NewLine, a));

                //if (!string.IsNullOrWhiteSpace(exString))
                {
                    throw new Exception(exString);
                }
            }
        }

   

  
        private bool FilterAccountPopulation()
        {
            FilterGLAccountByParams();

            if (!_MainAccountIdList_ToFetchThenAggragrate.Any())
            {
                MyPeriodList = new List<PeriodM>();
                
                return false;// throw new Exception("No GLAccounts");
            }

            bool testMulti = false;
            if (testMulti)
            {
                _MainAccountIdList_ToFetchThenAggragrate = (new List<string>(){
                    "1-33", "1-26","1-33","1-26","1-30","1-33","1-26" , "1-33"}).AsQueryable<string>();
            }



            

   
            Create_WhichGLAccountWillShow_ForeignAmount();
            return true;
        }

        private void Create_WhichGLAccountWillShow_ForeignAmount()
        {
            ///if (_Param.AgingMethod == AgingReportRebulidParam.MethodEnum.ReconcileOpenBalanceMethod.ToString())
            {
                // when take from reconcile from 2018/10 the reconcile set the open amount  into FOreignAmount ALWAYS !!!
                _AccountListId_PleaseTake_ForeignAmount =
                                        (from ac in _qAllAccAging4AccountTypeCode_CustomerOrVendor
                                         where _MainAccountIdList_ToFetchThenAggragrate.Contains(ac.Id)
                                         //where ac.IsMultiCurrency != true
                                         select ac.Id);
                return;
            }


            AccountThatIsMulti_UseForeignAmount();

            

            

        }

     
    
        private void AccountThatIsMulti_UseForeignAmount()
        {
            _AccountListId_PleaseTake_ForeignAmount =
                                        (from ac in _qAllAccAging4AccountTypeCode_CustomerOrVendor
                                         where _MainAccountIdList_ToFetchThenAggragrate.Contains(ac.Id)
                                         where ac.IsMultiCurrency != true
                                         select ac.Id
                                         );
        }

       



        private void FilterGLAccountByParams()
        {
            _MainAccountIdList_ToFetchThenAggragrate = null;
            var accType = GetAccountType(_Param.Aging4AccountTypeCode);
            _qAllAccAging4AccountTypeCode_CustomerOrVendor = _myGLAccountRepository
                //.GetQAllByAccountTypeCode(_Param.Tenant, GetAccountType(_Param.Aging4AccountTypeCode));
                .GetAll(_Param.Tenant)
                .Where(a => a.AccountTypeCode == accType);

            if (!String.IsNullOrWhiteSpace(_Param.VendorCustomerId))
            {
                var qVendorCustomerId = (from acc in _qAllAccAging4AccountTypeCode_CustomerOrVendor
                                         where acc.Id == _Param.VendorCustomerId
                                         select acc.Id
                );
                Union_AccountIdList(qVendorCustomerId);
                //return;
            }


            


         

            if (_MainAccountIdList_ToFetchThenAggragrate == null)//Bug 42372: Reco - Problem with Code (MaxAmount)
            {
                //without any filter so get all by AccountTypeCode
                _MainAccountIdList_ToFetchThenAggragrate = _qAllAccAging4AccountTypeCode_CustomerOrVendor.Select(r => r.Id).AsQueryable<string>();

            }
            else
            {

                _MainAccountIdList_ToFetchThenAggragrate =
                    (from filterAccId in _MainAccountIdList_ToFetchThenAggragrate
                     join glAcc in _qAllAccAging4AccountTypeCode_CustomerOrVendor
                     on filterAccId equals glAcc.Id
                     select filterAccId);
            }

        }

     
        private void Union_AccountIdList(IQueryable<string> addMyQueryable)
        {
            if (_MainAccountIdList_ToFetchThenAggragrate == null)
            {
                _MainAccountIdList_ToFetchThenAggragrate = addMyQueryable;
            }
            else
            {
                _MainAccountIdList_ToFetchThenAggragrate = _MainAccountIdList_ToFetchThenAggragrate
                //.AddRange
                .Union
                (addMyQueryable);
            }

        }

        private string GetAccountType(Logitude.Accounting.BL.CoreBL.Reports.AgingReportParam.Aging4AccountTypeCodeEnum Aging4AccountTypeCode)
        {
            return ((int)Aging4AccountTypeCode).ToString();
        }

        private static List<PeriodM> GetDummiesPeriods(List<DateTime> listPeriods,
            //IQueryable<string> acountListId, string AccountingCurrencyId, 
            IQueryable<AccountCurrency> AccountCurrencyList,
            DateTime myOrderDate, List<DateTime> listLessThanExclusivePeriods)
        {
            var cartesianProductPeriodAccount =
            (from period in listPeriods
             from AccountCurrency in AccountCurrencyList
             orderby period, AccountCurrency.AccountId
             select new PeriodM { AccountId = AccountCurrency.AccountId, OrderDate = period, CurrencyId = AccountCurrency.CurrencyId }
                                ).ToList();


            var cartesianProductLessThanExclusiveAccount =
                (from period in listLessThanExclusivePeriods
                 from AccountCurrency in AccountCurrencyList
                 orderby period, AccountCurrency.AccountId
                 select new PeriodM()
                 {
                     OrderDate = myOrderDate,
                     OrderDateB4 = true,
                     AccountId = AccountCurrency.AccountId,
                     CurrencyId = AccountCurrency.CurrencyId,
                     Total = 0,

                 })
                 .ToList();
            return cartesianProductPeriodAccount.Union(cartesianProductLessThanExclusiveAccount).ToList();


        }







        private void CheckeParams()
        {

            if (_Param.NumberOfmonthsbackwards < 1)
            {
                throw new Exception("NumberOfmonthsbackwards<1");
            }

            if (this._Param.Aging4AccountTypeCode == AgingReportParam.Aging4AccountTypeCodeEnum.ControlAccountOnly1)
            {
                throw new Exception("ControlAccountOnly1");
               
            }
            
      
        }


        public List<PeriodM> MyPeriodList { get; set; }
        
    }
    public class AgingReportRebulidParam
    {
        public int Tenant { get; set; }
        public int NumberOfmonthsbackwards { get; internal set; }
        public DateTime AgingForDate { get; internal set; }
        public string VendorCustomerId { get; internal set; }
        public AgingReportParam.Aging4AccountTypeCodeEnum Aging4AccountTypeCode { get; internal set; }
    }

    public class ReconciliationUpdateAgingService
    {
        IAccountingContext MainContext;
        public ReconciliationUpdateAgingService(IAccountingContext mainContext)
        {
            this.MainContext = mainContext;
        }
        public GLAccountAgingDataPM GetDelta(bool cancelledAction, ReconciliationPM entityPM)
        {

            var gLAccountRepository = new GLAccountRepository(this.MainContext as IAccountingContext);
            var MyAcc = gLAccountRepository.GetSingle(entityPM.AccountId, entityPM.Tenant);
            if (!(MyAcc.AccountTypeCode == "2" || MyAcc.AccountTypeCode == "3"))
            {
                return new GLAccountAgingDataPM(); ;
            }

            int totalClose =  entityPM.ReconciliationLines.Where(r => !r.IsPartial).Count();
            var myTransactionIds = entityPM.ReconciliationLines.Select(r => r.TransactionId).ToList();
            var repo = new LedgerTransactionRepository(this.MainContext as IAccountingContext);
            var pocos = repo.GetLedgerTransactionsByIdList(myTransactionIds, entityPM.Tenant);
            var myJoin = (from l in pocos
                          join rl in entityPM.ReconciliationLines
                          on l.Id equals rl.TransactionId
                          select new { l.AccountId, LedgerTransactionId = l.Id, l.DueDate, rl.ReconciliationAmount }
                   );
            var qGLAccountTotalByMonthsDTOAging =
                (
                from r in myJoin
                group r by new
                {
                    r.DueDate.Year,
                    r.DueDate.Month,
                    r.AccountId
                }
                into gbDateMonth
                select new ReconciliationUpdateAgingM()
                {
                    AccountId = gbDateMonth.Key.AccountId,
                    Year = gbDateMonth.Key.Year,
                    Month = gbDateMonth.Key.Month,
                    Total = gbDateMonth.Sum(r => r.ReconciliationAmount),
                });
            List<DateTime> listPeriods;
            DateTime lessThan, graterThen_OpenTransactionsFutureDueDate;

            var listPeriodService = new ListPeriodService(
                new AgingReportRebulidParam()
                {
                    AgingForDate = DateTime.Now.Date,
                    NumberOfmonthsbackwards = 6
                });
            listPeriodService.GetListPeriods(out listPeriods, out lessThan, out graterThen_OpenTransactionsFutureDueDate);

            PeriodM myLess = GetLessPeriodM(qGLAccountTotalByMonthsDTOAging, lessThan);
            PeriodM myFuture = GetFuturePeriodPm(qGLAccountTotalByMonthsDTOAging, graterThen_OpenTransactionsFutureDueDate);

            var mainPeriods =
                            (from period in listPeriods

                             join totalByMonth in qGLAccountTotalByMonthsDTOAging
                             on new { period.Year, period.Month }
                             equals
                             new { totalByMonth.Year, totalByMonth.Month }
                             into JointotalByMonth
                             from totalByMonth in JointotalByMonth.DefaultIfEmpty()


                             select new PeriodM()
                             {
                                 OrderDate = period,
                                 Total = totalByMonth == null ? 0 : totalByMonth.Total,
                             }).ToList();

            int multi = -1;//***Close*** Transactions - when create reconcile
            if (cancelledAction)
            {
                multi = 1;//***Open*** Transactions - when cancell reconcile
            }

            var myGLAccountAgingDataPM = new GLAccountAgingDataPM()
            {
                ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update,
                AccountId = entityPM.AccountId,
                Tenant = entityPM.Tenant,
                PeriodPast = myLess == null ? 0 : myLess.Total * multi,
                Period0 = mainPeriods.Skip(0).First().Total* multi,
                Period1 = mainPeriods.Skip(1).First().Total * multi,
                Period2 = mainPeriods.Skip(2).First().Total * multi,
                Period3 = mainPeriods.Skip(3).First().Total * multi,
                Period4 = mainPeriods.Skip(4).First().Total * multi,
                Period5 = mainPeriods.Skip(5).First().Total * multi,
                PeriodFuture = myFuture == null ? 0 : myFuture.Total * multi,
                TotalOpenTransactions = totalClose * multi,
            };
            
            return myGLAccountAgingDataPM;
        }

        private static PeriodM GetFuturePeriodPm(IEnumerable<ReconciliationUpdateAgingM> qGLAccountTotalByMonthsDTOAging, DateTime graterThen_OpenTransactionsFutureDueDate)
        {
            var qFuture = (from rec in qGLAccountTotalByMonthsDTOAging
                           where
                           (rec.Year == graterThen_OpenTransactionsFutureDueDate.Year && rec.Month >= graterThen_OpenTransactionsFutureDueDate.Month)
                           ||
                           rec.Year > graterThen_OpenTransactionsFutureDueDate.Year
                           group rec by new { rec.AccountId } into groupByAccCurrr

                           select new PeriodM()
                           {

                               OrderDate = graterThen_OpenTransactionsFutureDueDate,
                               OrderAfterOpenrECODueDate = true,
                               AccountId = groupByAccCurrr.Key.AccountId,

                               Total = groupByAccCurrr.Sum(rec => rec.Total),


                           })
             .ToList();

            if (qFuture.Count > 1)
            {
                throw new Exception("if (qFuture.Count > 1)");
            }
            var myFuture = qFuture.FirstOrDefault();
            return myFuture;
        }

        private static PeriodM GetLessPeriodM(IEnumerable<ReconciliationUpdateAgingM> qGLAccountTotalByMonthsDTOAging, DateTime lessThan)
        {
            var qLessThanExclusiveBasicInLocal =
                   (from rec in qGLAccountTotalByMonthsDTOAging
                    where
                    (rec.Year == lessThan.Year && rec.Month < lessThan.Month)
                    ||
                    rec.Year < lessThan.Year
                    group rec by new { rec.AccountId } into groupByAccCurrr

                    select new PeriodM()
                    {

                        OrderDate = lessThan,
                        OrderDateB4 = true,
                        AccountId = groupByAccCurrr.Key.AccountId,

                        Total = groupByAccCurrr.Sum(rec => rec.Total),
                    })
                    .ToList();
            if (qLessThanExclusiveBasicInLocal.Count > 1)
            {
                throw new Exception("if (qLessThanExclusiveBasicInLocal.Count > 1)");
            }
            var myLess = qLessThanExclusiveBasicInLocal.FirstOrDefault();
            return myLess;
        }

        internal void UpdateDelta(GLAccountAgingDataPM deltaGLAccountAgingDataPM, bool commit)
        {
            var gLAccountAgingDataQueryService = new GLAccountAgingDataQueryService(
    MainContext);
            var dbGLAccountAgingDataPM=gLAccountAgingDataQueryService.GetSingle(deltaGLAccountAgingDataPM.AccountId, false, false);
            var gLAccountAgingDataUpdateService = new GLAccountAgingDataUpdateService(
    MainContext, new Dictionary<string, IContext>(), deltaGLAccountAgingDataPM.Tenant);

            dbGLAccountAgingDataPM.ChangeSetOp = ChangeSetOperation.Update;
            dbGLAccountAgingDataPM.PeriodPast += deltaGLAccountAgingDataPM.PeriodPast;
            dbGLAccountAgingDataPM.Period0 += deltaGLAccountAgingDataPM.Period0;
            dbGLAccountAgingDataPM.Period1 += deltaGLAccountAgingDataPM.Period1;
            dbGLAccountAgingDataPM.Period2 += deltaGLAccountAgingDataPM.Period2;
            dbGLAccountAgingDataPM.Period3 += deltaGLAccountAgingDataPM.Period3;
            dbGLAccountAgingDataPM.Period4 += deltaGLAccountAgingDataPM.Period4;
            dbGLAccountAgingDataPM.Period5 += deltaGLAccountAgingDataPM.Period5;
            dbGLAccountAgingDataPM.PeriodFuture += deltaGLAccountAgingDataPM.PeriodFuture;
            dbGLAccountAgingDataPM.TotalOpenTransactions += deltaGLAccountAgingDataPM.TotalOpenTransactions;

            gLAccountAgingDataUpdateService.Update(dbGLAccountAgingDataPM, false);
        }
    }
    class ReconciliationUpdateAgingM
    {
        internal decimal Total;

        public string AccountId { get; internal set; }
        public int Year { get; internal set; }
        public int Month { get; internal set; }
        public int TotalOpenTransactions { get; internal set; }
    }
    public class ListPeriodService
    {
        private AgingReportRebulidParam _Param;

        public ListPeriodService(AgingReportRebulidParam param)
        {
            _Param = param;
        }

        public void GetListPeriods(out List<DateTime> listPeriods, out DateTime lessThan, out DateTime graterThen_OpenTransactionsFutureDueDate)
        {
            var minusMonth = -1;
            bool eyalSuppressMinusMonth = true;
            if (eyalSuppressMinusMonth) minusMonth = 0;
            //22-02-16  _AgingReportRebulidParam.AgingForDate
            DateTime AgingForDateLastMonth1st = _Param.AgingForDate.AddMonths(minusMonth);



            //22-01-16  AgingForDateLastMonth1st
            AgingForDateLastMonth1st = new DateTime(AgingForDateLastMonth1st.Year, AgingForDateLastMonth1st.Month, 1);
            //01-01-16  AgingForDateLastMonth1st NumberOfmonthsbackwards=5
            listPeriods = new List<DateTime>();
            graterThen_OpenTransactionsFutureDueDate = DateTime.MaxValue;
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
            if (true/*ToCalcOpenTransactionsFutureDueDate()*/)
            {
                //var currMonth = AgingForDateLastMonth1st.AddMonths(+1);
                //listPeriods.Add(currMonth);
                //graterThen_OpenTransactionsFutureDueDate = listPeriods.OrderByDescending(d => d).First();
                graterThen_OpenTransactionsFutureDueDate = AgingForDateLastMonth1st.AddMonths(+1);
            }
            lessThan = listPeriods.OrderBy(d => d).First();

            //lessThanExclusive ==01-09-15  
        }
    }

    public class AgingReportRebulidTesterService
    {
        public void RebulidReconcile(int tenant, string reconcileId)
        {
            using (var scope = TransactionFactory.GetTransaction(/*TimeSpan.FromMinutes(10)*/))
            {
                var accountingContext = AccountingContext.GetContext(tenant);
                (accountingContext as System.Data.Entity.DbContext).Database.CommandTimeout = 300;




                var reconciliationQueryService = new ReconciliationQueryService(accountingContext);
                var reconciliationPM=reconciliationQueryService.GetSingle(reconcileId, true, false);
                var reconciliationUpdateAgingService = new ReconciliationUpdateAgingService(accountingContext);
                var deltaGLAccountAgingDataPM =reconciliationUpdateAgingService.GetDelta(false, reconciliationPM);
                //reconciliationUpdateAgingService.UpdateDelta(deltaPM, false);
                //accountingContext.SaveChanges();
                if (!string.IsNullOrWhiteSpace(deltaGLAccountAgingDataPM.AccountId))
                {
                    reconciliationUpdateAgingService.UpdateDelta(deltaGLAccountAgingDataPM, false);
                    accountingContext.SaveChanges();// MUST SAVE DUE NEW CONTEXT !!!

                }
                scope.Complete();

            }

        }
    }
}
