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

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public class AgingReportService
    {
        private AgingReportParam _Param;
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

        public AgingReportService(AgingReportParam param)
        {
            _Param = param;
            var test = DateTime.Now < new DateTime(2016, 10, 1);
            if (test)
            {
                TestIt();
            }
            _TESTIT = false;

        }

        private void TestIt()
        {
            var b4 = new List<PeriodM>()  {
                new PeriodM(){AccountId="1", CurrencyId="1", OrderDate = new DateTime(2016 ,9,1) , OrderDateB4=false , Total=100},
                new PeriodM(){AccountId="1", CurrencyId="1", OrderDate = new DateTime(2016 ,8,1) , OrderDateB4=false , Total=-30},
                new PeriodM(){AccountId="1", CurrencyId="1", OrderDate = new DateTime(2016 ,7,1) , OrderDateB4=false , Total=50},
                new PeriodM(){AccountId="1", CurrencyId="1", OrderDate = new DateTime(2016 ,6,1) , OrderDateB4=false , Total=-50},
                new PeriodM(){AccountId="1", CurrencyId="1", OrderDate = new DateTime(2016 ,5,1) , OrderDateB4=false , Total=10},
                new PeriodM(){AccountId="1", CurrencyId="1", OrderDate = new DateTime(2016 ,5,1) , OrderDateB4=true, Total=-35},
            };
            Debug.WriteLine(b4);
            Debug.WriteLine("------");
            var after = ManipulateFifoPerAccCurr(b4);
            Debug.WriteLine(after);

        }

        public string RunReport()
        {
            CheckeParams();
            var listPeriods = new List<DateTime>();
            var minusMonth = -1;
            bool eyalSuppressMinusMonth = true;
            if (eyalSuppressMinusMonth) minusMonth = 0;
            //22-02-16  _AgingReportParam.AgingForDate
            DateTime AgingForDateLastMonth1st = _Param.AgingForDate.AddMonths(minusMonth);



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
                    return string.Empty ;// no accounts 
                }

                _AccountingCurrencyId = (new AccountingSettingResolver()).ResolveAccountingCurrencyId(_Param.Tenant);




                if (_Param.AgingMethod == AgingReportParam.MethodEnum.ReconcileOpenBalanceMethod.ToString())
                {

                    qTotalByMonthAcc = repoLedgerTransactionRepository
                        .GetQuerableReconcileOpenBalanceAsTotalByMonth(
                        _Param.Tenant,
                        _MainAccountIdList_ToFetchThenAggragrate,
                        null,//this._TryGetAllThenAggregate ? null : _AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount,
                        _AccountingCurrencyId,
                        _Param.GroupByDate == AgingReportParam.DateEnum.AccountingDate
                        );
                    if (_TESTIT)
                    {
                        var ss = qTotalByMonthAcc.ToList();
                    }
                    GLAccountReconcileDefintionChanged(qTotalByMonthAcc);
                }
                else
                {


                    /// Fetch Data Of Main GLAccount
                    var myDateTypeCode = _Param.GroupByDate == AgingReportParam.DateEnum.AccountingDate ? GLAccountTotalDateTypeValues.Accountingdate : GLAccountTotalDateTypeValues.DueDate;

                    qTotalByMonthAcc = repoGLAccountTotalByMonth.GetAll(_Param.Tenant).Where(tot => tot.DateTypeCode == myDateTypeCode)
                     .Where(rec => _MainAccountIdList_ToFetchThenAggragrate.Contains(rec.AccountId))
                     .Select(rec => new GLAccountTotalByMonthsDTOAging()
                     {
                         Tenant = rec.Tenant,
                         AccountId = rec.AccountId,
                         GLAccountCurrencyId = rec.AccountId,
                         CurrencyId = rec.CurrencyId,

                         Year = rec.Year,
                         Month = rec.Month,

                         LocalAmountCredit = rec.LocalAmountCredit,
                         LocalAmountDebit = rec.LocalAmountDebit,

                         ForeignAmountCredit = (decimal)rec.ForeignAmountCredit,
                         ForeignAmountDebit = (decimal)rec.ForeignAmountDebit,
                         CHANGE_TYPE = ""
                     });
                    //if (!_TryGetAllThenAggregate)
                    //{
                    //    if (_Param.AggregateByGLAccountCurrencies)
                    //    {
                    //        qTotalByMonthAcc = AggregateRelatedCurrencyAccount_AsMainAccount(qTotalByMonthAcc, repoGLAccountTotalByMonth, myDateTypeCode);

                    //    }
                    //}
                }

                if (_Param.AgingMethod == AgingReportParam.MethodEnum.ReconcileOpenBalanceMethod.ToString())
                {

                }

                //_notMultiCurrencyAccountListId = _GLAccounts.Where(rec => !rec.IsMultiCurrency.GetValueOrDefault()).Select(rec => rec.Id).ToList();
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
                         OpenDebit = groupByAccCurrr.Sum(rec => rec.ForeignAmountDebit)
                     });


                if (_TESTIT)
                {
                    var _1 = qPeriodInLocalAmount.ToList();
                    var _2 = qLessThanExclusiveBasicInLocal.ToList();
                    var _3 = qPeriodInForeign.ToList();
                    var _4 = qLessThanExclusiveBasicInForeign.ToList();
                }
                var theDBList =
                    qPeriodInLocalAmount.Union(qLessThanExclusiveBasicInLocal)
                    .Union(qPeriodInForeign).Union(qLessThanExclusiveBasicInForeign).ToList();
                theDBList = theDBList.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.OrderDate).ThenBy(rec => rec.CurrencyId)
                    .ToList();




                //if (_TryGetAllThenAggregate)
                {
                    if (_Param.AggregateByGLAccountCurrencies)
                    {

                        theDBList =
                (from left_TotDB in theDBList
                 join right_AccRelatedCurrency in _AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount on left_TotDB.AccountId equals right_AccRelatedCurrency.GLAccountId into JoinListAccRelatedCurrency
                 from accRelatedCurrency in JoinListAccRelatedCurrency.DefaultIfEmpty()
                 select new PeriodM()
                 {

                     OrderDate = left_TotDB.OrderDate,
                     OrderDateB4 = left_TotDB.OrderDateB4,
                     AccountId = (accRelatedCurrency != null) ? accRelatedCurrency.MainGLAccountId : left_TotDB.AccountId,
                     SplitAccountId = (accRelatedCurrency != null) ? left_TotDB.AccountId : null,
                     
                     CurrencyId = left_TotDB.CurrencyId,
                     Total = left_TotDB.Total,
                     OpenCredit = left_TotDB.OpenCredit,
                     OpenDebit = left_TotDB.OpenDebit,

                 }).ToList();


                    }



                    if (_Param.AggregateByGLAccountChildren)
                    {

                        theDBList =
                (from left_TotDB in theDBList
                 join right_AccChild in _GLAccountChildren_UseToAggregateAsLocalAmount on left_TotDB.AccountId equals right_AccChild.Id into JoinListAccRelatedCurrency
                 from accRelatedCurrency in JoinListAccRelatedCurrency.DefaultIfEmpty()
                 select new PeriodM()
                 {

                     OrderDate = left_TotDB.OrderDate,
                     OrderDateB4 = left_TotDB.OrderDateB4,
                     AccountId = (accRelatedCurrency != null) ? accRelatedCurrency.ParentAccountId : left_TotDB.AccountId,

                     CurrencyId = left_TotDB.CurrencyId,
                     Total = left_TotDB.Total
                 }).ToList();


                    }
                }


                if (_Param.AgingMethod == AgingReportParam.MethodEnum.TotalByMonthFIFOMethod.ToString())
                {
                    var totalByMonthFIFO = new List<PeriodM>();
                    ////var currencyList = theDBList.Select(r => r.CurrencyId).Distinct().ToList();

                    var dueMaybeAggregate_GroupIt = true;
                    if (dueMaybeAggregate_GroupIt)//insure only 1 rec per month !!!
                    {
                        theDBList = (from rec in theDBList
                                     group rec by new { rec.OrderDate, rec.OrderDateB4, rec.AccountId, rec.CurrencyId }
                                  into groupby
                                     select new PeriodM()
                                     {
                                         OrderDate = groupby.Key.OrderDate,
                                         OrderDateB4 = groupby.Key.OrderDateB4,
                                         AccountId = groupby.Key.AccountId,
                                         CurrencyId = groupby.Key.CurrencyId,
                                         Total = groupby.Sum(rec => rec.Total),
                                         OpenDebit = groupby.Sum(rec => rec.OpenDebit),
                                         OpenCredit = groupby.Sum(rec => rec.OpenCredit),
                                     }
                                        ).ToList();
                    }
                    foreach (var g in theDBList.ToLookup(r => new { r.AccountId, r.CurrencyId }))
                    {

                        var currencyAging = theDBList
                            .Where(r => r.AccountId == g.Key.AccountId)
                            .Where(r => r.CurrencyId == g.Key.CurrencyId).ToList();

                        currencyAging.ForEach(r=>
                        {
                            if (r.OpenDebit < 0)
                            {
                                r.OpenCredit -= r.OpenDebit;
                                r.OpenDebit = 0;
                            }

                            if (r.OpenCredit < 0)
                            {
                                r.OpenDebit -= r.OpenCredit;
                                r.OpenCredit = 0;
                            }

                        }

                        );
                        currencyAging = ManipulateFifoPerAccCurr(currencyAging);
                        totalByMonthFIFO.AddRange(currencyAging);
                    }
                    theDBList = totalByMonthFIFO.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.OrderDate).ThenBy(rec => rec.CurrencyId).ToList();
                }

                theDBList = theDBList.OrderBy(rec => rec.AccountId).ThenBy(rec => rec.OrderDate).ThenBy(rec => rec.CurrencyId).ToList();

                var orderLessThanExclusive = lessThan; ;//.AddMonths(-1);
                var myorderLessThanExclusive = new DateTime(orderLessThanExclusive.Year, orderLessThanExclusive.Month, 1);
                var listLessThanExclusivePeriods = new List<DateTime>() { orderLessThanExclusive };

                List<PeriodM> dummiesPeriodsList = BuildDummiesPeriod(listPeriods, myorderLessThanExclusive, listLessThanExclusivePeriods);


                //var dummiesWithoutDBRecord=  dummiesPeriodsList.Where(dummy => dbList.Any(db => db.AccountId != dummy.AccountId));
                var DBAndDummies = //dummiesWithoutDBRecord.Union(dbList);
                    dummiesPeriodsList.Union(theDBList);

                if (_AccountListRelatedCurrenciesAccount_List2Discard != null)
                {
                    DBAndDummies = DBAndDummies.Where(r => !_AccountListRelatedCurrenciesAccount_List2Discard.Contains(r.AccountId));

                }
                if (_Param.AggregateByGLAccountCurrencies)
                {
                    DBAndDummies = DBAndDummies.Where(r => !string.IsNullOrEmpty(r.CurrencyId));
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

                                  }
                 ).ToList();


                reportList = (from r in reportList
                              orderby r.OrderDate, r.OrderDateB4 descending
                              select r
                              ).ToList();

                // Adding accounts names
                List<string> accountsIds = (
                    (reportList.Select(d => d.AccountId))
                    .Union(
                    (reportList.Where(r=>!String.IsNullOrEmpty(r.SplitAccountId))).Select(d => d.SplitAccountId)))
                    .Distinct().ToList();

                GLAccountListQueryService accountQS = new GLAccountListQueryService(_AccountingContext);

                IQueryable<GLAccountList> q_accountsList = accountQS.GetByIds(accountsIds, _Param.Tenant);
                bool blanceCureency4SplitIsNeeded = true;
                if (!blanceCureency4SplitIsNeeded)
                {
                    if (_AccountListRelatedCurrenciesAccount_List2Discard != null)
                    {
                        q_accountsList = q_accountsList.Where(r => !_AccountListRelatedCurrenciesAccount_List2Discard.Contains(r.Id));

                    }
                }

                var myaccountsList = q_accountsList.ToList();
                TenantQuery tenantQuery = new TenantQuery(_Param.Tenant);
                var tenant = tenantQuery.GetSinglePM(_Param.Tenant);

                DateTime currentDate = TenantServerConfigration.GetCurrentDateTime(_Param.Tenant);
              
            List<PeriodMExtended> periodMExtendeds =
                    (from acc in q_accountsList
                     join moredata in _AccountingContext.GLAccountMoreDatas.Where(r => r.Tenant == _Param.Tenant)
                     on acc.Id equals moredata.AccountId into moredataJoinT
                     from moredata in moredataJoinT.DefaultIfEmpty()

                     join card in _AccountingContext.Cards.Where(r => r.Tenant == _Param.Tenant)
                      on acc.Id equals card.GLAccountId into cardJoinT
                     from card in cardJoinT.DefaultIfEmpty()

                     join cust in _AccountingContext.Customers.Where(r => r.Tenant == _Param.Tenant)
                     on card.Id equals cust.Id into custJoinT
                     from cust in custJoinT.DefaultIfEmpty()

                     join custOFiles in _AccountingContext.CustomerOpenFilesAmounts.Where(r => r.Tenant == _Param.Tenant)
                     on card.Id equals custOFiles.CustomerId into custOFilesJoinT
                     from custOFiles in custOFilesJoinT.DefaultIfEmpty()

                     let glaPeriod = _AccountingContext.GLAccountInterestPeriods.Where(r => r.Tenant == _Param.Tenant &&r.GLAccountId == acc.Id && r.PeriodStartDate <= currentDate)
                     .OrderByDescending(d=>d.PeriodStartDate).FirstOrDefault()
                     let basePeriod= _AccountingContext.InterestBasesPeriods.Where(d=>d.InterestBaseTypeId==glaPeriod.StandardInterestRateBaseId)
                     .OrderByDescending(d=>d.InterestBaseStartDate).FirstOrDefault()
                     //join accIntrestPeriods in _AccountingContext.GLAccountInterestPeriods.Where(r => r.Tenant == _Param.Tenant && r.PeriodStartDate <= currentDate)
                     //on acc.Id equals accIntrestPeriods.GLAccountId into intrestPeriodsJoin
                     //from accIntrestPeriods in intrestPeriodsJoin.DefaultIfEmpty()

                     select new PeriodMExtended()
                     {
                         AccountId = acc.Id,
                         AccountDisplayNumber = acc.DisplayNumber,
                         AccountInternalNumber = acc.InternalNumber,
                         InterestCreditLimit = acc.InterestCreditLimit,
                         AccountTermName = card.PaymentTerm.EnglishName,

                         CurrencyId = acc.ReconcileMethodCode == "0" ? tenant.CurrencyId : acc.CurrencyId,

                         AccountTermLocalName = card.PaymentTerm.LocalName,


                         //CurrencyId = acc.ReconcileMethodCode == "0" ? tenant.CurrencyId : acc.CurrencyId,



                         CurrencyCode = acc.CurrencyCode,
                         CreditLimitAmount =
                         //cust!=null?(double)cust.CreditLimitAmount:0,
                         cust != null ? (cust.CreditLimitAmount != null ? (double)cust.CreditLimitAmount : 0) : 0,

                         CreditStatusAmount_AsIs = cust != null ? (cust.CreditLimitAmount != null ? (double)cust.CreditLimitAmount : 0) : 0,
                         BalanceInLocalCurrency = moredata != null ? (decimal)moredata.BalanceInLocalCurrency : 0.00m,

                         LocalBalanceInDue = moredata != null ? (decimal)moredata.LocalBalanceInDue : 0.00m,
                         CustomerVatNumber = card.VatNumber,
                         GLAccountStandardInterestRate = (decimal)(glaPeriod.StandardAddInterestPercent == null ? 0  : basePeriod.InterestRate ==null? glaPeriod.StandardAddInterestPercent : glaPeriod.StandardAddInterestPercent+basePeriod.InterestRate),


                            //CreditStatusAmount= 
                            //((decimal)(cust.CreditLimitAmount.GetValueOrDefault())
                            //- (
                            //moredata.BalanceInLocalCurrency.GetValueOrDefault()
                            //+ moredata.TotalOpenChequesInLocalCur.GetValueOrDefault()
                            //+ moredata.TotFutureOpenChequesInLocalCur.GetValueOrDefault()
                            //+ custOFiles.TotalOpenFilesAmount//entityList.OpenShipments= SetCustomerOpenShipments(entityList);
                            //)
                            //),

                         TotalOpenShipments = custOFiles != null ? custOFiles.TotalOpenFilesAmount : 0,
                         TotalFutureOpenCheques = moredata != null ? (decimal)moredata.TotFutureOpenChequesInLocalCur : 0,
                         TotalOpenCheques = moredata != null ? (decimal)moredata.TotalOpenChequesInLocalCur : 0,

                         AccountEnglishName = acc.EnglishName,
                         AccountLocalName = acc.LocalName,
                         AccountCurrencyCode = acc.ReconcileMethodCode == "0" ? tenant.CurrencyCode : acc.CurrencyCode,
                         AccountPhone = card.Phone,

                     }

                 ).ToList();

                


                ///var list1=periodMExtendeds.ToList();
                bool checkIt = false;
                if (checkIt)
                {
                    var A = reportList.GroupBy(r => r.AccountId).Count();
                    var b = periodMExtendeds.GroupBy(r => r.AccountId).Count();
                    if (A != b)
                    {

                    }
                }
                RemoveDummies(ref reportList, myaccountsList, tenant);
                CurrencyQuery _CurrencyQuery = new CurrencyQuery(_Param.Tenant);
                var currencies = _CurrencyQuery.GetCurrenciesByTenantPM(_Param.Tenant).ToList();
                List<PeriodMExtended> namedPeriods = MapExtended(reportList, periodMExtendeds, currencies);
                //

                var allAccountingDateBalance = GetBalance(myaccountsList.Select(r => r.Id).AsQueryable(), GLAccountTotalDateTypeValues.Accountingdate);
                var allDueDateBalance = GetBalance(myaccountsList.Select(r => r.Id).AsQueryable(), GLAccountTotalDateTypeValues.DueDate);


                namedPeriods =
                    ( from r in namedPeriods
                    join a in allAccountingDateBalance on (r.AccountId /*, r.CurrencyId*/) equals (a.AccountId/*, a.CurrencyId*/) //into a
                      join d in allDueDateBalance on (r.AccountId/*, r.CurrencyId*/) equals (d.AccountId/*,d.CurrencyId*/)
                    select
                    new PeriodMExtended()
                    {
                        OrderDate = r.OrderDate,
                        OrderDateB4 = r.OrderDateB4,
                        AccountId = r.AccountId,
                        SplitAccountId =r.SplitAccountId,
                        CurrencyId = r.CurrencyId,
                        CurrencyCode = r.CurrencyCode,
                        Total = r.Total,
                        AccountEnglishName = r.AccountEnglishName,
                        AccountLocalName = r.AccountLocalName,
                        AccountDisplayNumber = r.AccountDisplayNumber,
                        AccountInternalNumber = r.AccountInternalNumber,
                        AccountCurrencyCode = r.AccountCurrencyCode,
                        AccountTermName = r.AccountTermName,
                        InterestCreditLimit = r.InterestCreditLimit,
                        CreditLimitAmount = r.CreditLimitAmount,
                        CreditStatusAmount_AsIs = r.CreditStatusAmount_AsIs,
                        CreditStatusAmount =
                        ((decimal)(r.CreditLimitAmount)
                        - (
                        r.BalanceInLocalCurrency
                        + r.TotalFutureOpenCheques
                        + r.TotalOpenCheques
                        + r.TotalOpenShipments
                        )
                        ),
                        OpenCredit = r.OpenCredit,
                        OpenDebit = r.OpenDebit,


                        BalanceInLocalCurrency = r.BalanceInLocalCurrency,
                        LocalBalanceInDue = r.LocalBalanceInDue,
                        TotalOpenShipments = r.TotalOpenShipments,
                        TotalFutureOpenCheques = r.TotalFutureOpenCheques,
                        TotalOpenCheques = r.TotalOpenCheques,
                        GLAccountStandardInterestRate=r.GLAccountStandardInterestRate,
                        AccountTermLocalName = r.AccountTermLocalName,
                        CustomerVatNumber = r.CustomerVatNumber,
                        BalanceInLocalAccountingDate =a.LocalAmountDebit-a.LocalAmountCredit,
                        BalanceInLocalDueDate = d.LocalAmountDebit - d.LocalAmountCredit,
                        
                    }
                    ).ToList();

                MyPeriodList = reportList;
                MyPeriodExtendedList = namedPeriods;
                string xml = string.Empty;
                if (_Param.BuildPivot)
                {
                    DataTable _PivotTable = namedPeriods.ToPivotTable(
                        rec => rec.PeriodName,
                        rec => rec.AccountAndCurr, //new { rec.AccountId, rec.CurrencyId }, //rec.AccountId, //
                        recs => recs.Any() ? recs.Sum(rec => rec.Total) : 0.00m);
                    xml = _PivotTable.ToJsonString();
                    _PivotTable.TableName = "sss";
                    xml = _PivotTable.ToXml();
                }
                return xml;
            }
        }
        private List<CurrencySum> GetBalance(IQueryable<string> listOfAccountId,string dateTypeCode)
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
_Param.AgingForDate.Date, false, true, true);

            
            var totals = (from rec in endAccountBalanceService.AccountBalance.verbose.CurrencySumUntillMounth.Union(endAccountBalanceService.AccountBalance.verbose.TheMounthCurrencySum)
                          group rec by new
                          { rec.AccountId
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
        private void RemoveDummies(ref List<PeriodM> reportList, List<GLAccountList> myaccountsList, Logitude.BL.CommonDataModel.EntityPMs.TenantPM tenant)
        {
            if (!this._Param.AggregateByGLAccountCurrencies)
            {
                
                reportList.Where(r=>r.CurrencyId==null).ToList()
                    .ForEach(r =>
                {

                    var acc = myaccountsList.First(m => m.Id == r.AccountId);
                    r.CurrencyId = acc.ReconcileMethodCode == "0" ? tenant.CurrencyId : acc.CurrencyId;
                });
                
            }

            reportList = (from a in reportList
                          group a by new { a.AccountId, a.CurrencyId, a.OrderDateB4, a.OrderDate } into g
                          select new PeriodM()
                          {
                              OrderDateB4 = g.First().OrderDateB4,
                              SplitAccountId = g.First().SplitAccountId,
                              OrderDate = g.First().OrderDate,
                              AccountId = g.Key.AccountId,
                              CurrencyId = g.Key.CurrencyId,


                              Total = g.Sum(r => r.Total),
                              OpenCredit = g.Sum(r => r.OpenCredit),
                              OpenDebit = g.Sum(r => r.OpenDebit),


                          })
                         .ToList();
                          


        }

        private static List<PeriodMExtended> MapExtended(List<PeriodM> reportList, List<PeriodMExtended> periodMExtendeds, /*IQueryable*/List<Logitude.BL.CommonDataModel.EntityPMs.CurrencyPM> currencies)
        {
            List<PeriodMExtended> namedPeriods = (from line in reportList
                                                 
                                                  //join account in periodMExtendeds
                                                  //  on line.AccountId equals account.AccountId into accJoin
                                                  //from account in accJoin.DefaultIfEmpty()
                                                  let account = periodMExtendeds.FirstOrDefault(account=> account.AccountId ==line.AccountId)
                                                  let splitAccount = periodMExtendeds.FirstOrDefault(account => account.AccountId == line.SplitAccountId)

                                                  //join currency in currencies
                                                  //  on line.CurrencyId equals currency.Id into currencyJoin
                                                  //from currency in currencyJoin.DefaultIfEmpty()
                                                  let currency= currencies.FirstOrDefault(r=>r.Id ==line.CurrencyId)

                                                  select new PeriodMExtended()
                                                  {
                                                      OrderDate = line.OrderDate,
                                                      OrderDateB4 = line.OrderDateB4,
                                                      AccountId = line.AccountId,
                                                      SplitAccountId = line.SplitAccountId,
                                                      CurrencyId = line.CurrencyId,
                                                      CurrencyCode = currency == null ? null : currency.Code,
                                                      Total = line.Total,
                                                      AccountEnglishName = account.AccountEnglishName,
                                                      AccountLocalName = account.AccountLocalName,
                                                      AccountDisplayNumber = account.AccountDisplayNumber,
                                                      AccountInternalNumber = account.AccountInternalNumber,
                                                      AccountCurrencyCode = account.AccountCurrencyCode,
                                                      AccountTermName = account.AccountTermName,

                                                      CreditLimitAmount = account.CreditLimitAmount,
                                                      InterestCreditLimit = account.InterestCreditLimit,
                                                      CreditStatusAmount_AsIs = account.CreditStatusAmount_AsIs,
                                                      BalanceInLocalCurrency = splitAccount!=null ? splitAccount.BalanceInLocalCurrency: account.BalanceInLocalCurrency,
                                                      LocalBalanceInDue = splitAccount!=null ? splitAccount.LocalBalanceInDue : account.LocalBalanceInDue,                                                      
                                                      TotalOpenShipments = account.TotalOpenShipments,
                                                      TotalFutureOpenCheques = account.TotalFutureOpenCheques,
                                                      TotalOpenCheques = account.TotalOpenCheques,
                                                      OpenCredit = line.OpenCredit,
                                                      OpenDebit = line.OpenDebit,
                                                      CreditStatusAmount = account.CreditStatusAmount,
                                                       GLAccountStandardInterestRate=account.GLAccountStandardInterestRate,
                                                       CustomerVatNumber = account.CustomerVatNumber,
                                                       AccountTermLocalName = account.AccountTermLocalName,
                                                       

                                                  }).ToList();
            return namedPeriods;
        }

        private IQueryable<GLAccountTotalByMonthsDTOAging> AggregateRelatedCurrencyAccount_AsMainAccount(IQueryable<GLAccountTotalByMonthsDTOAging> qTotalByMonthAcc, GLAccountTotalByMonthRepository repoGLAccountTotalByMonth, string myDateTypeCode)
        {
            var qTotalByMonthAccOfRelatedCurrenciesAccount =
                (from tot in repoGLAccountTotalByMonth.GetAll(_Param.Tenant).Where(tot => tot.DateTypeCode == myDateTypeCode)
                 join accRelated in _AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount on tot.AccountId equals accRelated.GLAccountId
                 select new GLAccountTotalByMonthsDTOAging()
                 {
                     Tenant = tot.Tenant,
                     AccountId = accRelated.MainGLAccountId,
                     GLAccountCurrencyId = accRelated.GLAccountId, ///RelatedCurrenciesAccount
                     CurrencyId = tot.CurrencyId,

                     Year = tot.Year,
                     Month = tot.Month,

                     LocalAmountCredit = tot.LocalAmountCredit,
                     LocalAmountDebit = tot.LocalAmountDebit,

                     ForeignAmountCredit = (decimal)tot.ForeignAmountCredit,
                     ForeignAmountDebit = (decimal)tot.ForeignAmountDebit,
                     CHANGE_TYPE = ""
                 });
            ///  union & Fetch Data Of RelatedCurrenciesAccount as main !!
            qTotalByMonthAcc =
                qTotalByMonthAcc
                .Union(qTotalByMonthAccOfRelatedCurrenciesAccount);
            return qTotalByMonthAcc;
        }

        private List<PeriodM> BuildDummiesPeriod(List<DateTime> listPeriods, DateTime myorderLessThanExclusive, List<DateTime> listLessThanExclusivePeriods)
        {
            IQueryable<AccountCurrency> accountCurrencyList = null;

            var accountCurrencyList_MultiUseAccountingCurrencyId =
            _MainAccountIdList_ToFetchThenAggragrate
            .Where(acc => !_AccountListId_PleaseTake_ForeignAmount.Contains(acc))
            .Select(acc => new AccountCurrency() { AccountId = acc, CurrencyId = _AccountingCurrencyId });
            
            if (this._Param.AggregateByGLAccountCurrencies)
            {
                var listRelated = this._AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount.Select(r => r.GLAccountId);
                accountCurrencyList_MultiUseAccountingCurrencyId =accountCurrencyList_MultiUseAccountingCurrencyId.Where(r => !listRelated.Contains(r.AccountId));
            }

            if (this._Param.AggregateByGLAccountChildren)
            {
                var listChildren = this._GLAccountChildren_UseToAggregateAsLocalAmount.Select(r => r.Id);
                accountCurrencyList_MultiUseAccountingCurrencyId =accountCurrencyList_MultiUseAccountingCurrencyId.Where(r => !listChildren.Contains(r.AccountId));
            }


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

        private List<PeriodM> ManipulateFifoPerAccCurr(List<PeriodM> dbList)
        {

            if (dbList.Select(r => new { r.AccountId, r.CurrencyId }).Distinct().Count() != 1)
            {
                throw new Exception("ManipulateFifoPerCurrency:::dbList.Select( r=>r.CurrencyId).Distinct().Count()!=1");
            }
            dbList = dbList.OrderBy(rec => rec.OrderDate).ThenByDescending(r => r.OrderDateB4).ToList();

            //we’ll need to offset the credit from the later month to earlier debits:
            var firstPlusPeriod = dbList.FirstOrDefault(r => r.OpenDebit > 0);

            if (firstPlusPeriod == null) return dbList;

            var biggerThanFirstList = dbList;//.Where(r => r.OrderDate > firstPlusPeriod.OrderDate).ToList();
            if (biggerThanFirstList.Count() == 0) return dbList;

            var firstMinusPeriod_ThatAfterFirstPlus = biggerThanFirstList.FirstOrDefault(r => r.OpenCredit > 0);

            if (firstPlusPeriod == null || firstMinusPeriod_ThatAfterFirstPlus == null)
            {
                return dbList;
            }

            var total1 = firstPlusPeriod.OpenDebit - firstMinusPeriod_ThatAfterFirstPlus.OpenCredit;
            if (total1 == 0)
            {
                //firstMinusPeriod_ThatAfterFirstPlus.Total = firstPlusPeriod.Total = 0;
                //firstMinusPeriod_ThatAfterFirstPlus.OpenDebit = firstPlusPeriod.OpenCredit = 0;
                firstPlusPeriod.OpenDebit = firstMinusPeriod_ThatAfterFirstPlus.OpenCredit = 0;
            }
            else if (total1 > 0) 
            {
                //firstPlusPeriod.OpenDebit > firstMinusPeriod_ThatAfterFirstPlus.OpenCredit
                firstPlusPeriod.OpenDebit -= firstMinusPeriod_ThatAfterFirstPlus.OpenCredit;
                firstMinusPeriod_ThatAfterFirstPlus.OpenCredit = 0;
            }
            else if (total1 < 0)
            {
                //firstPlusPeriod.OpenDebit < firstMinusPeriod_ThatAfterFirstPlus.OpenCredit
                firstMinusPeriod_ThatAfterFirstPlus.OpenCredit -= firstPlusPeriod.OpenDebit;
                firstPlusPeriod.OpenDebit = 0;
                
            }
            firstPlusPeriod.Total = firstPlusPeriod.OpenDebit - firstPlusPeriod.OpenCredit;
            firstMinusPeriod_ThatAfterFirstPlus.Total = firstMinusPeriod_ThatAfterFirstPlus.OpenDebit - firstMinusPeriod_ThatAfterFirstPlus.OpenCredit;


            return ManipulateFifoPerAccCurr(dbList);
        }
        private List<PeriodM> ManipulateFifoPerAccCurrTotal(List<PeriodM> dbList)
        {
            
            if (dbList.Select(r => new { r.AccountId, r.CurrencyId }).Distinct().Count() != 1)
            {
                throw new Exception("ManipulateFifoPerCurrency:::dbList.Select( r=>r.CurrencyId).Distinct().Count()!=1");
            }
            dbList = dbList.OrderBy(rec => rec.OrderDate).ThenByDescending(r => r.OrderDateB4).ToList();

            //we’ll need to offset the credit from the later month to earlier debits:
            var firstPlusPeriod = dbList.FirstOrDefault(r => r.Total > 0);

            if (firstPlusPeriod == null ) return dbList;

            var biggerThanFirstList = dbList;//.Where(r => r.OrderDate > firstPlusPeriod.OrderDate).ToList();
            if(biggerThanFirstList.Count() == 0) return dbList; 

            var firstMinusPeriod_ThatAfterFirstPlus = biggerThanFirstList.FirstOrDefault(r => r.Total < 0);

            if (firstPlusPeriod == null || firstMinusPeriod_ThatAfterFirstPlus == null)
            {
                return dbList;
            }

            var total1 = firstMinusPeriod_ThatAfterFirstPlus.Total + firstPlusPeriod.Total;
            if (total1 == 0)
            {
                firstMinusPeriod_ThatAfterFirstPlus.Total = firstPlusPeriod.Total = 0;
            }
            else if (total1 > 0)
            {
                firstPlusPeriod.Total = total1;
                firstMinusPeriod_ThatAfterFirstPlus.Total = 0;

            }
            else if (total1 < 0)
            {
                firstPlusPeriod.Total = 0;
                firstMinusPeriod_ThatAfterFirstPlus.Total = total1;
            }
            return ManipulateFifoPerAccCurr(dbList);
        }



        private List<PeriodM> ManipulateFifoPerAccCurr_BAD(List<PeriodM> dbList)
        {

            if (dbList.Select(r => new { r.AccountId, r.CurrencyId }).Distinct().Count() != 1)
            {
                throw new Exception("ManipulateFifoPerCurrency:::dbList.Select( r=>r.CurrencyId).Distinct().Count()!=1");
            }
            dbList = dbList.OrderByDescending(rec => rec.OrderDate).ThenBy(r => r.OrderDateB4).ToList();

            //we’ll need to offset the credit from the later month to earlier debits:
            var firstPlusPeriod = dbList.FirstOrDefault(r => r.Total > 0);

            if (firstPlusPeriod == null) return dbList;

            var biggerThanFirstList = dbList.Where(r => r.OrderDate > firstPlusPeriod.OrderDate).ToList();
            if (biggerThanFirstList.Count() == 0) return dbList;

            var firstMinusPeriod_ThatAfterFirstPlus = biggerThanFirstList.FirstOrDefault(r => r.Total < 0);

            if (firstPlusPeriod == null || firstMinusPeriod_ThatAfterFirstPlus == null)
            {
                return dbList;
            }

            var total1 = firstMinusPeriod_ThatAfterFirstPlus.Total + firstPlusPeriod.Total;
            if (total1 == 0)
            {
                firstMinusPeriod_ThatAfterFirstPlus.Total = firstPlusPeriod.Total = 0;
            }
            else if (total1 > 0)
            {
                firstPlusPeriod.Total = total1;
                firstMinusPeriod_ThatAfterFirstPlus.Total = 0;

            }
            else if (total1 < 0)
            {
                firstPlusPeriod.Total = 0;
                firstMinusPeriod_ThatAfterFirstPlus.Total = total1;
            }
            return ManipulateFifoPerAccCurr(dbList);
        }


        private bool FilterAccountPopulation()
        {
            FilterGLAccountByParams();

            if (!_MainAccountIdList_ToFetchThenAggragrate.Any())
            {
                MyPeriodList = new List<PeriodM>();
                MyPeriodExtendedList = new List<PeriodMExtended>();
                return false;// throw new Exception("No GLAccounts");
            }

            bool testMulti = false;
            if (testMulti)
            {
                _MainAccountIdList_ToFetchThenAggragrate = (new List<string>(){
                    "1-33", "1-26","1-33","1-26","1-30","1-33","1-26" , "1-33"}).AsQueryable<string>();
            }



            if (_Param.AggregateByGLAccountChildren)///AggregateByGLAccountChildren B4 AggregateByGLAccountCurrencies -to Children Have RelatedByCurrency !!!
            {
                _GLAccountChildren_UseToAggregateAsLocalAmount =
                    this._qAllAccAging4AccountTypeCode_CustomerOrVendor
                    //.Where(acc => _MainAccountIdList_ToFetchThenAggragrate.Contains(acc.ParentAccountId));
                    .Where(acc => _MainAccountIdList_ToFetchThenAggragrate.ToList().Contains(acc.ParentAccountId))
                    .ToList();

                Union_AccountIdList(_GLAccountChildren_UseToAggregateAsLocalAmount.Select(r => r.Id).AsQueryable<string>());

            }



            ///-דיפולט של המערכת כול כרטיס מגויל בפני עצמו – לכול כרטיס שורה אחת(ולא מוסיפים כרטיסי בנים מטבעיים) DetailedByGLAccountCurrencies=false  -- 
            ///DetailedByGLAccountCurrencies= True-כאשר מבקשים עם ריכוז לפי כרטיס אב(פיצול כרטיסים מטבעים ) – יהיה שורה אחת רק עבור כרטיס אב הסוכמת את האב והבנים לפי LOCALAMOUNT(לא מציגים את כרטיסי הבינים המטבעים)
            if (_Param.AggregateByGLAccountCurrencies)
            {
                Prepare_RelatedCurrenciesAccount_2Accumalte();

                ///DetailedByGLAccountCurrencies= True-כאשר מבקשים עם ריכוז לפי כרטיס אב(פיצול כרטיסים מטבעים ) – יהיה שורה אחת רק עבור כרטיס אב הסוכמת את האב והבנים לפי LOCALAMOUNT(לא מציגים את כרטיסי הבינים המטבעים)
                //if (_TryGetAllThenAggregate)
                {
                    Union_AccountIdList(_AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount.Select(r => r.GLAccountId));
                }
                //else
                //{
                //    RemoveFromMainAccountIdList_theRelatedAccCurrencies_ThatWillAccumalteAsLocal();
                //}

            }
            Create_WhichGLAccountWillShow_ForeignAmount();
            return true;
        }

        private void Create_WhichGLAccountWillShow_ForeignAmount()
        {
            if (_Param.AgingMethod == AgingReportParam.MethodEnum.ReconcileOpenBalanceMethod.ToString())
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

            AggregateByGLAccountCurrencies_RemoveFrom_ForeignAmountList();

            AggregateByGLAccountChildren_RemoveFrom_ForeignAmountList();

        }

        private void AggregateByGLAccountChildren_RemoveFrom_ForeignAmountList()
        {
            if (_Param.AggregateByGLAccountChildren)
            {

                var relatedList = _GLAccountChildren_UseToAggregateAsLocalAmount.Select(r => r.Id);
                _AccountListId_PleaseTake_ForeignAmount = _AccountListId_PleaseTake_ForeignAmount.Where(accForeign => !relatedList.Contains(accForeign));

            }
        }

        private void AggregateByGLAccountCurrencies_RemoveFrom_ForeignAmountList()
        {
            // by default related r in foreign Amount
            // But If Aggragrate Remove From _AccountListId_PleaseTake_ForeignAmount 
            if (_Param.AggregateByGLAccountCurrencies)
            {
                
                //if (_TryGetAllThenAggregate)
                {
                    var relatedList = _AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount.Select(r => r.GLAccountId);
                    _AccountListId_PleaseTake_ForeignAmount =_AccountListId_PleaseTake_ForeignAmount.Where(accForeign => !relatedList.Contains(accForeign));
                }
                //else
                //{
                //    _AccountListId_PleaseTake_ForeignAmount =
                //        _AccountListId_PleaseTake_ForeignAmount
                //    .Union
                //    (
                //        from accCurrency in _AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount
                //        select accCurrency.GLAccountId
                //     );
                //}
            }
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

        private void RemoveFromMainAccountIdList_theRelatedAccCurrencies_ThatWillAccumalteAsLocal()
        {
            var qAccountThatR_NotRelatedCurrenciesAccount =
                                _MainAccountIdList_ToFetchThenAggragrate.Where(mainAcc => !_AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount.Any(related => related.GLAccountId == mainAcc));
            _MainAccountIdList_ToFetchThenAggragrate = qAccountThatR_NotRelatedCurrenciesAccount.AsQueryable();
        }

        private void Prepare_RelatedCurrenciesAccount_2Accumalte()
        {
            var myGLAccountCurrencyRepository = new GLAccountCurrencyRepository(_AccountingContext);
            _AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount = myGLAccountCurrencyRepository
                .GetQRelatedCurrenciesAccountIdByCustomerGLAccount(_Param.Tenant, _MainAccountIdList_ToFetchThenAggragrate)
                //.Select(rec => rec.GLAccountId).ToList();
                ;
            _AccountListRelatedCurrenciesAccount_List2Discard = _AccountListRelatedCurrenciesAccount_UseToAggregateAsLocalAmount
          .Select(r => r.GLAccountId)
          .ToList();
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


            if (!string.IsNullOrWhiteSpace(
                String.Concat(_Param.Category1Id, _Param.Category2Id, _Param.Category3Id, _Param.Category4Id, _Param.Category5Id)
                ))
            {


                var listCustomerCategories = _myGLAccountRepository.GetQAccIdByAcountTypeCategories(_Param.Tenant,
                  this.GetAccountType(this._Param.Aging4AccountTypeCode),  //show only GLAccounts from type 2 (AccountType=2)
                 _Param.Category1Id, _Param.Category2Id, _Param.Category3Id, _Param.Category4Id, _Param.Category5Id);
                Join_AccountIdList(listCustomerCategories);

            }


            if (!String.IsNullOrWhiteSpace(_Param.CollectorId))
            {

                var qGLAccIdByCollectorId = _myGLAccountQueryService.GetQGLAccIdByCollectorId(_Param.Tenant, _Param.CollectorId, GetAccountType(_Param.Aging4AccountTypeCode));
                Join_AccountIdList(qGLAccIdByCollectorId);

            }
            if (!String.IsNullOrWhiteSpace(_Param.SalesmanId))
            {

                var qSalesmanUserId = _myGLAccountQueryService.GetQGLAccIdBySalesmanId(_Param.Tenant, _Param.SalesmanId, GetAccountType(_Param.Aging4AccountTypeCode));
                Join_AccountIdList(qSalesmanUserId);

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

        private void Join_AccountIdList(IQueryable<string> addMyQueryable)
        {
            if (_MainAccountIdList_ToFetchThenAggragrate == null)
            {
                _MainAccountIdList_ToFetchThenAggragrate = addMyQueryable;
            }
            else
            {
                _MainAccountIdList_ToFetchThenAggragrate =
                    (from a in _MainAccountIdList_ToFetchThenAggragrate
                     join a2 in addMyQueryable
                     on a equals a2
                     select a);

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

            if (this._Param.Aging4AccountTypeCode == AgingReportParam.Aging4AccountTypeCodeEnum.ControlAccountOnly1)
            {
                if (String.IsNullOrWhiteSpace(_Param.VendorCustomerId))
                {
                    throw new Exception("While Filtering by ControlAccount ,Please set Control Account Id in VendorCustomerId");
                }
                if (_Param.AgingMethod == AgingReportParam.MethodEnum.ReconcileOpenBalanceMethod.ToString())
                {
                    throw new Exception("While Filtering by ControlAccount ,the Reconcile Open Balance Method is providen");
                }
            }
            if (!String.IsNullOrWhiteSpace(_Param.SalesmanId))
            {
                ///throw new Exception("Sorry ,Salesman ?!?!?!  What 2do==" + _Param.SalesmanId);
            }
            if (this._Param.Aging4AccountTypeCode == AgingReportParam.Aging4AccountTypeCodeEnum.ControlAccountOnly1)
            {

                var fullPM = FullAccountingSettingQueryService.Get(_Param.Tenant);
                var list = new List<string>()
                {

                        fullPM.AirExportJobControlAccountId ,
                        fullPM.AirImportJobControlAccountId ,
                        fullPM.CustomerControlAccountId ,
                        fullPM.OceanExportJobControlAccountId ,
                        fullPM.OceanImportJobControlAccountId,
                        fullPM.VendorControlAccountId,
                        fullPM.FileControlAccountId,

                };
                if (!list.Contains(_Param.VendorCustomerId))
                {
                    throw new Exception("Filtering by ControlAccount (_Param.VendorCustomerId= " + _Param.VendorCustomerId + " ) ,Please set Control Account Id in VendorCustomerId");
                }

                //_Param
            }
            //if (string.IsNullOrWhiteSpace(_Param.VendorCustomerId))
            //{
            //    throw new Exception("string.IsNullOrWhiteSpace(_AgingReportParam.VendorCustomerId) ");
            //}
        }


        public List<PeriodM> MyPeriodList { get; set; }
        public List<PeriodMExtended> MyPeriodExtendedList { get; set; }
    }
    public class PeriodM
    {
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", this.AccountId, this.CurrencyId, PeriodName, this.Total);
        }



        public DateTime OrderDate { get; set; }
        public bool OrderDateB4 { get; set; }

        public string AccountId { get; set; }

        public string CurrencyId { get; set; }

        public decimal Total { get; set; }

        public decimal OpenCredit { get; set; }
        public decimal OpenDebit { get; set; }

        public string PeriodName
        {
            get
            {
                var month = OrderDate.Month.ToString() + "/" + OrderDate.Year.ToString();
                if (!OrderDateB4)
                {
                    return month;
                }
                else
                {
                    return "b4 " + month;
                }

            }
        }


        public string AccountAndCurr
        {
            get
            {
                return this.AccountId + " " + this.CurrencyId;
            }
        }

        public string CalcTotalCurrencyId
        {
            get
            {
                //CalcTotalCurrencyId = "0 NIS"
                return Total + " " + CurrencyId;
            }
        }

        public string SplitAccountId { get;  set; }
    }
    public class PeriodMExtended: PeriodM
    {
        public string AccountEnglishName { get; set; }
        public string AccountLocalName { get; set; }


        public string AccountDisplayNumber { get; set; }
        public string AccountInternalNumber { get; set; }
        public string AccountCurrencyCode { get; set; }
        //accountCardlist.Payment Term: //PaymentTermName = card.PaymentTerm == null ? null : card.PaymentTerm.EnglishName,
        public string AccountTermName { get; set; }
        public string AccountTermLocalName { get; set; }
        public string CurrencyCode { get; set; }
        public string CustomerVatNumber { get; set; }

        public decimal GLAccountStandardInterestRate { get; set; }

        /*
        var percentage = 0;
        if (this.GLAccountMoreData && this.accountCardlist)
        {
            percentage =
                (this.GLAccountMoreData.BalanceInLocalCurrency ? this.GLAccountMoreData.BalanceInLocalCurrency : 0)
            +   (this.GLAccountMoreData.TotalOpenChequesInLocalCur ? this.GLAccountMoreData.TotalOpenChequesInLocalCur : 0)
            +   (this.GLAccountMoreData.TotFutureOpenChequesInLocalCur ? this.GLAccountMoreData.TotFutureOpenChequesInLocalCur : 0)
            + (this.accountCardlist.OpenShipments?this.accountCardlist.OpenShipments:0 );

            this.accountTotal = percentage;

            if(this.accountCardlist.CreditLimitAmount && this.accountCardlist.CreditLimitAmount != 0)
                percentage = percentage / (this.accountCardlist.CreditLimitAmount ? this.accountCardlist.CreditLimitAmount : 0);
            else
                percentage = 0;

            this.creditStatusAmount = (this.accountCardlist.CreditLimitAmount ? this.accountCardlist.CreditLimitAmount : 0) - this.accountTotal;

        }
 
         */


        public string AccountPhone { get; set; }


        //ccountCardlist?accountCardlist.CreditLimitAmount:0>>entityList.CreditLimitAmount = entityPOCO.Customer.CreditLimitAmount;
        public double? CreditLimitAmount { get; set; }
        public decimal? InterestCreditLimit { get; set; }

        //this.creditStatusAmount = (this.accountCardlist.CreditLimitAmount ? this.accountCardlist.CreditLimitAmount : 0) - this.accountTotal;
        public decimal? CreditStatusAmount { get; set; }
        //this.accountCardlist.OpenShipments? this.accountCardlist.OpenShipments:0 
        public decimal? TotalOpenShipments { get; set; }
        //+   (this.GLAccountMoreData.TotFutureOpenChequesInLocalCur ? this.GLAccountMoreData.TotFutureOpenChequesInLocalCur : 0)
        public decimal? TotalFutureOpenCheques { get; set; }
        //+   (this.GLAccountMoreData.TotalOpenChequesInLocalCur ? this.GLAccountMoreData.TotalOpenChequesInLocalCur : 0)
        public decimal? TotalOpenCheques { get; set; }
        public double? CreditStatusAmount_AsIs { get; set; }
        public decimal? BalanceInLocalCurrency { get;  set; }
        public decimal? LocalBalanceInDue { get;  set; }
        public string SplitAccountId { get;  set; }
        public decimal BalanceInLocalAccountingDate { get; set; }
        public decimal BalanceInLocalDueDate { get; set; }
    }

    public class AgingReportParam
    {
        public AgingReportParam()
        {
            _explained_AggregateByGLAccountCurrencies = @"
EYAL:
גיול חובות לכרטיס רב מטבעי מבוצע על מטבע מקומי בלבד  == LOCALAMOUNT
גיול חובות לכרטיס חד מטבעי מבוצע על FOREIGNAMOUNT  + CURRENCY

FALSE=דיפולט של המערכת כל כרטיס מגוייל בפני עצמו – לכל כרטיס שורה אחת  => 
TRUE= כאשר מבקשים עם ריכוז לפי כרטיס אב (פיצול כרטיסים מטבעים ) – יהיה שורה אחת רק עבור כרטיס אב הסוכמת את האב והבנים לפי LOCALAMOUNT  (לא מציגים את כרטיסי הבינים  המטבעים)
";
        }
        private string _explained_AggregateByGLAccountCurrencies;

        public enum MethodEnum
        {
            TotalByMonthMethod,
            TotalByMonthFIFOMethod,
            ReconcileOpenBalanceMethod,
        }
        public enum DateEnum
        {
            DueDate,
            AccountingDate,
        }

        public enum Aging4AccountTypeCodeEnum : int
        {
            ControlAccountOnly1 = 1,
            Customer2 = 2,
            Vendor3,

        }


        public int Tenant { get; set; }

        public DateTime AgingForDate { get; set; }
        public //int
            uint NumberOfmonthsbackwards
        { get; set; }



        public string VendorCustomerId { get; set; }
        public Aging4AccountTypeCodeEnum Aging4AccountTypeCode { get; set; }



        public string Category1Id { get; set; }
        public string Category2Id { get; set; }
        public string Category3Id { get; set; }
        public string Category4Id { get; set; }
        public string Category5Id { get; set; }

        public string CollectorId { get; set; }
        public string SalesmanId { get; set; }



        //public string ChartOfAccountIdV1NotInUse { get; set; }
        //public bool? CurrenciesDetailedV1NotInUse { get; set; }



        //public bool WithReconciledTransactions { get; set; }


        ///-דיפולט של המערכת כול כרטיס מגויל בפני עצמו – לכול כרטיס שורה אחת(ולא מוסיפים כרטיסי בנים מטבעיים) DetailedByGLAccountCurrencies=false  -- 
        ///DetailedByGLAccountCurrencies= True-כאשר מבקשים עם ריכוז לפי כרטיס אב(פיצול כרטיסים מטבעים ) – יהיה שורה אחת רק עבור כרטיס אב הסוכמת את האב והבנים לפי LOCALAMOUNT(לא מציגים את כרטיסי הבינים המטבעים)
        public bool /*IncludeRelatedCurrenciesAccount*/
        AggregateByGLAccountCurrencies
        { get; set; }


        public bool AggregateByGLAccountChildren { get; set; }


        public string AgingMethod_Options { get; set; }


        public string AgingMethod { get; set; }
        public DateEnum GroupByDate { get; set; }

        public string GroupByDate_Options { get; set; }



        public string Aging4AccountTypeCode_Options { get; set; }

        //public string Explained_AggregateByGLAccountCurrencies => explained_AggregateByGLAccountCurrencies;


        public string Explained_AggregateByGLAccountCurrencies
        {
            get { return _explained_AggregateByGLAccountCurrencies; }
            set { _explained_AggregateByGLAccountCurrencies = value; }
        }

        public bool BuildPivot { get; set; }

    }


    class AccountCurrency
    {
        public string AccountId { get; set; }
        public string CurrencyId { get; set; }

    }
}
