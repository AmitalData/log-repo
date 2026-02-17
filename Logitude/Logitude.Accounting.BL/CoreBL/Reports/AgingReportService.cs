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
                //var qsGLAccountTotalByMonth = new GLAccountTotalByMonthQueryService(_AccountingContext);
                var repoGLAccountTotalByMonth = new GLAccountTotalByMonthRepository(_AccountingContext);
                var repoLedgerTransactionRepository = new LedgerTransactionRepository(_AccountingContext);
                _myGLAccountQueryService = new GLAccountQueryService(_AccountingContext);
                _myGLAccountRepository = new GLAccountRepository(_AccountingContext);


                FilterAccountPopulation();

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
                        var ss=qTotalByMonthAcc.ToList();
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
                         OpenDebit = groupByAccCurrr.Sum(rec => rec.LocalAmountCredit),

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
                     AccountId = (accRelatedCurrency!=null) ? accRelatedCurrency.MainGLAccountId : left_TotDB.AccountId,
                     
                     CurrencyId = left_TotDB.CurrencyId,
                     Total = left_TotDB.Total,

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
                                             Total = groupby.Sum(rec => rec.Total)
                                         }
                                        ).ToList();
                    }
                    foreach (var g in theDBList.ToLookup(r => new { r.AccountId, r.CurrencyId }))
                    {

                        var currencyAging = theDBList
                            .Where(r => r.AccountId == g.Key.AccountId)
                            .Where(r => r.CurrencyId == g.Key.CurrencyId).ToList();

                       
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

                var reportList = (from rec in /*dummiesPeriodsList.Union(dbList)*/ DBAndDummies
                                  group rec by new { rec.OrderDate, rec.OrderDateB4, rec.AccountId, rec.CurrencyId }
                                      into groupby
                                  select new PeriodM()
                                  {
                                      OrderDate = groupby.Key.OrderDate,
                                      OrderDateB4 = groupby.Key.OrderDateB4,
                                      AccountId = groupby.Key.AccountId,
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
                List<string> accountsIds = qTotalByMonthAcc.Select(d => d.AccountId).ToList();

                GLAccountListQueryService accountQS = new GLAccountListQueryService(_AccountingContext);
                IQueryable<GLAccountList> accountsList = accountQS.GetByIds(accountsIds,_Param.Tenant);

                List<PeriodMExtended> namedPeriods = (from line in reportList
                                    join account in accountsList on line.AccountId equals account.Id
                                    select new PeriodMExtended()
                                    {
                                        OrderDate = line.OrderDate,
                                        OrderDateB4 = line.OrderDateB4,
                                        AccountId = line.AccountId,
                                        CurrencyId = line.CurrencyId,
                                        Total = line.Total,
                                        AccountEnglishName = account.EnglishName,
                                        AccountLocalName = account.LocalName,
                                    }).ToList();
                //


                MyPeriodList = reportList;
                MyPeriodExtendedList = namedPeriods;
                DataTable _PivotTable = namedPeriods.ToPivotTable(
                    rec => rec.PeriodName,
                    rec => rec.AccountAndCurr, //new { rec.AccountId, rec.CurrencyId }, //rec.AccountId, //
                    recs => recs.Any() ? recs.Sum(rec => rec.Total) : 0);
                var xml = _PivotTable.ToJsonString();
                _PivotTable.TableName = "sss";
                xml = _PivotTable.ToXml();
                return xml;
            }
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
            dbList = dbList.OrderByDescending(rec => rec.OrderDate).ThenBy(r => r.OrderDateB4).ToList();

            //we’ll need to offset the credit from the later month to earlier debits:
            var firstPlusPeriod = dbList.FirstOrDefault(r => r.Total > 0);

            if (firstPlusPeriod == null ) return dbList;

            var biggerThanFirstList = dbList.Where(r => r.OrderDate > firstPlusPeriod.OrderDate).ToList();
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

        private void FilterAccountPopulation()
        {
            FilterGLAccountByParams();

            if (!_MainAccountIdList_ToFetchThenAggragrate.Any())
            {
                throw new Exception("No GLAccounts");
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
                return;
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

    }
    public class PeriodMExtended: PeriodM
    {
        public string AccountEnglishName { get; set; }
        public string AccountLocalName { get; set; }
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

    }


    class AccountCurrency
    {
        public string AccountId { get; set; }
        public string CurrencyId { get; set; }

    }
}
