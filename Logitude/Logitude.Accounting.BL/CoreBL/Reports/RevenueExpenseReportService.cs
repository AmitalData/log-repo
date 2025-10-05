using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public class RevenueExpenseReportService //:IDisposable
    {
        /*
לתאריך - כמובן כולל היום 
הדוח כולל עד 500 כרטיסים - אין צורך למיטב 
        */

        protected RevenueExpenseReportParam _RevenueExpenseReportParam = null;
        protected readonly int __TimeOutInMinutes = 129;
        //private System.Transactions.TransactionScope _TransactionScope;
        //protected IAccountingContext _AccountingContext;
        private FullAccountingSettingPM _FullAccountingSetting;
        protected IQueryable<AccountCOAM> QAllRevenueExpenseCardsCOAM;
        private IQueryable<ChartOfAccount5LevelM> _QAllChartOfAccountFlattenBy5LevelofHierarchy;

        protected IEnumerable //IQueryable
            <RevenueExpenseReportM> _QBaseRevenueExpenseReportFull = null;
        //private DbContextBase.IDbContextLogger _DbLogger;






        







        
        


        protected IQueryable<ChartOfAccount5LevelM> QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy;
        

        public RevenueExpenseReportService(RevenueExpenseReportParam RevenueExpenseReportParam, int timeOutInMinutes)
        {
            _RevenueExpenseReportParam = RevenueExpenseReportParam;
            __TimeOutInMinutes = timeOutInMinutes;

        }

        public   List<RevenueExpenseReportM> result;
        public List<RevenueExpenseReportM> Execute()
        {

            _RevenueExpenseReportParam.FromDate = _RevenueExpenseReportParam.FromDate.Date;
            _RevenueExpenseReportParam.ToDate = _RevenueExpenseReportParam.ToDate.Date;

            if (_RevenueExpenseReportParam.FromDatePeriod2.HasValue && _RevenueExpenseReportParam.ToDatePeriod2.HasValue)
            {
                _RevenueExpenseReportParam.FromDatePeriod2 = _RevenueExpenseReportParam.FromDatePeriod2.Value.Date; ;
                _RevenueExpenseReportParam.ToDatePeriod2 = _RevenueExpenseReportParam.ToDatePeriod2.Value.Date;
            }
            else
            {
                //ignore !!!
                _RevenueExpenseReportParam.FromDatePeriod2 = _RevenueExpenseReportParam.ToDatePeriod2 = null;

            }

       




                var accountingContext = AccountingContext.GetContext(_RevenueExpenseReportParam.Tenant);
                //_DbLogger = (_AccountingContext as DbContextBase).CreateLogger();

                _FullAccountingSetting = //Hope From Cache
                    FullAccountingSettingQueryService
                    .Get(_RevenueExpenseReportParam.Tenant);


                var repoGLAccount = new GLAccountRepository(accountingContext);
                var myQAllRevenueExpenseCards = repoGLAccount.GetQAllRevenueExpenseCards(_RevenueExpenseReportParam.Tenant)
                    //.Where(a => !a.Inactive)
                    ;


                QAllRevenueExpenseCardsCOAM = (
                    from a in myQAllRevenueExpenseCards
                    select new AccountCOAM //Made 4 Short(Projoction) +Algant+Fast SQL
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        AccountTypeCode = a.AccountTypeCode,
                        EnglishName = a.EnglishName,
                        ChartOfAccountsTypeCode = a.ChartOfAccountsTypeCode,
                        ChartOfAccountsId = a.ChartOfAccountsId,
                        ParentId = a.ParentAccountId,
                        DisplayNumber = a.DisplayNumber,
                        LocalName = a.LocalName,
                    }
                    );

                var qsChartOfAccount = new ChartOfAccountQueryService(accountingContext);
                _QAllChartOfAccountFlattenBy5LevelofHierarchy = //Flatten ChartOfAccount By 5 Level hierarchy
                    qsChartOfAccount.GetQChartOfAccount5LevelM(_RevenueExpenseReportParam.Tenant, _RevenueExpenseReportParam.ChartOfAccountsTypes, _RevenueExpenseReportParam.ChartOfAccounts); // send the filtered ids are selected, from ChartOfAccountsTypes and ChartOfAccounts tables from UI #192454

                QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy =
                JoinEachAccountWithisChartOfAccount5hierarchy(QAllRevenueExpenseCardsCOAM);

                IQueryable<TrailReportTemp> QUnionAllCurrSummary = GetMoneyDataPerDate(
                    _RevenueExpenseReportParam.FromDate, _RevenueExpenseReportParam.ToDate,

                    _RevenueExpenseReportParam.Tenant, accountingContext);

                string debugAccId = "";//"1-216621"
                if (!string.IsNullOrWhiteSpace(debugAccId))
                {
                    var myData = QUnionAllCurrSummary.Where(r => r.AccountId_COAType == debugAccId).ToList();
                }
                IQueryable<RevenueExpenseReportM> qAllMoneySideRevenueExpenseReportM
                    =
                    (from r in QUnionAllCurrSummary
                     group r by new
                     {
                         AccountId = r.AccountId_COAType,
                         //r.CurrencyId
                     } into g
                     select new RevenueExpenseReportM()
                     {

                         ChartOfAcountType = "",
                         ChartOfAcount1 = "",
                         ChartOfAcount2 = "",
                         ChartOfAcount3 = "",
                         ChartOfAcount4 = "",
                         ChartOfAcount5 = "",
                         ChartOfAcountName1 = "",
                         ChartOfAcountName2 = "",
                         ChartOfAcountName3 = "",
                         ChartOfAcountName4 = "",
                         ChartOfAcountName5 = "",



                         ChartOfAcountCode1 = "",
                         ChartOfAcountCode2 = "",
                         ChartOfAcountCode3 = "",
                         ChartOfAcountCode4 = "",
                         ChartOfAcountCode5 = "",



                         GLAccountName = "",
                         GLAccountNumber = "",
                         GLAccountId = g.Key.AccountId,
                         GLAccountEnglish = "",
                         ChartOfAccountId = "",

                         ChartOfAcountName1English = "",
                         ChartOfAcountName2English = "",
                         ChartOfAcountName3English = "",
                         ChartOfAcountName4English = "",
                         ChartOfAcountName5English = "",

                         LocalCloseBalancePeriod1 =
                         (
                         +g.Sum(x => x.LocalAmountDebitTransStart)
                         - g.Sum(x => x.LocalAmountCreditTransStart)

                         + g.Sum(x => x.LocalAmountDebitTotalDelta2End)
                         - g.Sum(x => x.LocalAmountCreditTotalDelta2End)
                         + g.Sum(x => x.LocalAmountDebitTransEnd)
                         - g.Sum(x => x.LocalAmountCreditTransEnd)
                         ),
                         LocalCloseBalancePeriod2 = 0

                     });



                IQueryable<TrailReportTemp> QUnionAllCurrSummaryPeriod2 = Enumerable.Empty<TrailReportTemp>().AsQueryable();
                if (_RevenueExpenseReportParam.FromDatePeriod2.HasValue)
                {
                    QUnionAllCurrSummaryPeriod2 = GetMoneyDataPerDate(
                        _RevenueExpenseReportParam.FromDatePeriod2.GetValueOrDefault(), _RevenueExpenseReportParam.ToDatePeriod2.GetValueOrDefault(),
                        _RevenueExpenseReportParam.Tenant, accountingContext);


                    if (!string.IsNullOrWhiteSpace(debugAccId))
                    {
                        var myData = QUnionAllCurrSummaryPeriod2.Where(r => r.AccountId_COAType == debugAccId).ToList();
                    }

                    IQueryable<RevenueExpenseReportM> qAllMoneySideRevenueExpenseReportMPeriod2
           =
           (from r in QUnionAllCurrSummaryPeriod2
            group r by new
            {
                AccountId = r.AccountId_COAType,
                //r.CurrencyId
            } into g
            select new RevenueExpenseReportM()
            {

                ChartOfAcountType = "",
                ChartOfAcount1 = "",
                ChartOfAcount2 = "",
                ChartOfAcount3 = "",
                ChartOfAcount4 = "",
                ChartOfAcount5 = "",
                ChartOfAcountName1 = "",
                ChartOfAcountName2 = "",
                ChartOfAcountName3 = "",
                ChartOfAcountName4 = "",
                ChartOfAcountName5 = "",


                ChartOfAcountCode1 = "",
                ChartOfAcountCode2 = "",
                ChartOfAcountCode3 = "",
                ChartOfAcountCode4 = "",
                ChartOfAcountCode5 = "",



                GLAccountName = "",
                GLAccountNumber = "",
                GLAccountId = g.Key.AccountId,
                GLAccountEnglish = "",
                ChartOfAccountId = "",

                ChartOfAcountName1English = "",
                ChartOfAcountName2English = "",
                ChartOfAcountName3English = "",
                ChartOfAcountName4English = "",
                ChartOfAcountName5English = "",

                LocalCloseBalancePeriod1 = 0,
                LocalCloseBalancePeriod2 = (
                +g.Sum(x => x.LocalAmountDebitTransStart)
                - g.Sum(x => x.LocalAmountCreditTransStart)

                + g.Sum(x => x.LocalAmountDebitTotalDelta2End)
                - g.Sum(x => x.LocalAmountCreditTotalDelta2End)
                + g.Sum(x => x.LocalAmountDebitTransEnd)
                - g.Sum(x => x.LocalAmountCreditTransEnd)
                )

            });


                    qAllMoneySideRevenueExpenseReportM =
                        (from a in qAllMoneySideRevenueExpenseReportM.Union(qAllMoneySideRevenueExpenseReportMPeriod2)
                         group a by a.GLAccountId into gbGLAccountId

                         select new RevenueExpenseReportM()
                         {

                             ChartOfAcountType = "",
                             ChartOfAcount1 = "",
                             ChartOfAcount2 = "",
                             ChartOfAcount3 = "",
                             ChartOfAcount4 = "",
                             ChartOfAcount5 = "",
                             ChartOfAcountName1 = "",
                             ChartOfAcountName2 = "",
                             ChartOfAcountName3 = "",
                             ChartOfAcountName4 = "",
                             ChartOfAcountName5 = "",


                             ChartOfAcountCode1 = "",
                             ChartOfAcountCode2 = "",
                             ChartOfAcountCode3 = "",
                             ChartOfAcountCode4 = "",
                             ChartOfAcountCode5 = "",



                             GLAccountName = "",
                             GLAccountNumber = "",
                             GLAccountId = gbGLAccountId.Key,
                             GLAccountEnglish = "",
                             ChartOfAccountId = "",

                             ChartOfAcountName1English = "",
                             ChartOfAcountName2English = "",
                             ChartOfAcountName3English = "",
                             ChartOfAcountName4English = "",
                             ChartOfAcountName5English = "",

                             LocalCloseBalancePeriod1 = gbGLAccountId.Sum(a => a.LocalCloseBalancePeriod1),
                             LocalCloseBalancePeriod2 = gbGLAccountId.Sum(a => a.LocalCloseBalancePeriod2)


                         });


                }


                IQueryable<RevenueExpenseReportM> _QTrailReportFull = null;
                _QTrailReportFull =
            ///join 
            (from chartf in QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy
             join data in qAllMoneySideRevenueExpenseReportM
             on chartf.GLAccountId equals data.GLAccountId
             into groupJoin
             from groupJoinData in groupJoin.DefaultIfEmpty()
             select new RevenueExpenseReportM()
             {
                 ChartOfAcountType = chartf.ChartOfAccountTypeCode,
                 ChartOfAcount1 = chartf.Level1Id,
                 ChartOfAcount2 = chartf.Level2Id,
                 ChartOfAcount3 = chartf.Level3Id,
                 ChartOfAcount4 = chartf.Level4Id,
                 ChartOfAcount5 = chartf.Level5Id,
                 ChartOfAcountName1 = chartf.Level1Name,
                 ChartOfAcountName2 = chartf.Level2Name,
                 ChartOfAcountName3 = chartf.Level3Name,
                 ChartOfAcountName4 = chartf.Level4Name,
                 ChartOfAcountName5 = chartf.Level5Name,


                 ChartOfAcountCode1 = chartf.Level1Code,
                 ChartOfAcountCode2 = chartf.Level2Code,
                 ChartOfAcountCode3 = chartf.Level3Code,
                 ChartOfAcountCode4 = chartf.Level4Code,
                 ChartOfAcountCode5 = chartf.Level5Code,



                 GLAccountName = chartf.GLAccountName,
                 GLAccountNumber = chartf.GLAccountNumber,
                 GLAccountId = chartf.GLAccountId,
				 GLAccountEnglish = chartf.GLAccountEnglish,
                 ChartOfAccountId = chartf.ChartOfAccountId,


                 ChartOfAcountName1English = chartf.Level1English,
                 ChartOfAcountName2English = chartf.Level2English,
                 ChartOfAcountName3English = chartf.Level3English,
                 ChartOfAcountName4English = chartf.Level4English,
                 ChartOfAcountName5English = chartf.Level5English,

                 LocalCloseBalancePeriod1 = groupJoinData.LocalCloseBalancePeriod1,
                 LocalCloseBalancePeriod2 = groupJoinData.LocalCloseBalancePeriod2,


             });
                if (_RevenueExpenseReportParam.MyRevenueExpenseReportLevel == ReportLevel.GLAccount)
                {
                    switch (_RevenueExpenseReportParam.MyCardFilter)
                    {
                        case RevenueExpenseReportParam.CardFilterEnum.DoNotShowCardWithZeroBalance:
                            _QTrailReportFull =
                                _QTrailReportFull
                                //.Where(r => r.LocalCloseBalancePeriod1 != null)
                                //.Where(r => r.LocalCloseBalancePeriod1 != 0m)
                                .Where(r =>
                                (r.LocalCloseBalancePeriod1 != null && r.LocalCloseBalancePeriod1 != 0m)
                                ||
                                (r.LocalCloseBalancePeriod2 != null && r.LocalCloseBalancePeriod2 != 0m)
                                )
                               ;
                            break;

                        case RevenueExpenseReportParam.CardFilterEnum.ShowCardsWithActivity_EvenBalanceItsZero:
                            _QTrailReportFull = CreateFullTrailReportQuery(qAllMoneySideRevenueExpenseReportM, QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy);
                            break;


                        case RevenueExpenseReportParam.CardFilterEnum.ShowCardsWithActivity_AndBalanceNotZero:
                            _QTrailReportFull = CreateFullTrailReportQuery(qAllMoneySideRevenueExpenseReportM, QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy)
                                    .Where(r => (r.LocalCloseBalancePeriod1 != null && r.LocalCloseBalancePeriod1 != 0m)
                                                ||
                                                (r.LocalCloseBalancePeriod2 != null && r.LocalCloseBalancePeriod2 != 0m));

                             break;
                        case RevenueExpenseReportParam.CardFilterEnum.ShowAllCard:

                        default:
                            //already Put all
                            break;
                    }
                }
                else
                {
                    if (_RevenueExpenseReportParam.MyCardFilter != RevenueExpenseReportParam.CardFilterEnum.DoNotShowCardWithZeroBalance)
                    {
                        throw new Exception("5");
                    }
                }


                var _QTrailReportCOALevel = _QTrailReportFull;

                switch (_RevenueExpenseReportParam.MyRevenueExpenseReportLevel)
                {
                    case ReportLevel.ChartofaccountType:
                        _QTrailReportCOALevel = (from row in _QTrailReportCOALevel
                                                 group row by
                                                 new
                                                 {
                                                     row.ChartOfAcountType,
                                                     //row.ChartOfAcount1,
                                                     //row.ChartOfAcount2,
                                                     //row.ChartOfAcount3,
                                                     //row.ChartOfAcount4,
                                                     //row.ChartOfAcount5,
                                                     //row.GLAccountName,
                                                     //row.GLAccountId
                                                     //row.CurrencyId,

                                                 }
                                                     into groupTrailOnlyCOAType
                                                 select new RevenueExpenseReportM()
                                                 {
                                                     ChartOfAcountType = groupTrailOnlyCOAType.Key.ChartOfAcountType,
                                                     ChartOfAcount1 = "",//groupTrailOnlyCOAType.Key.ChartOfAcount1,
                                                     ChartOfAcount2 = "",//groupTrailOnlyCOAType.Key.ChartOfAcount2,
                                                     ChartOfAcount3 = "",//groupTrailOnlyCOAType.Key.ChartOfAcount3,
                                                     ChartOfAcount4 = "",//groupTrailOnlyCOAType.Key.ChartOfAcount4,
                                                     ChartOfAcount5 = "",//groupTrailOnlyCOAType.Key.ChartOfAcount5,
                                                     ChartOfAcountName1 = "",
                                                     ChartOfAcountName2 = "",
                                                     ChartOfAcountName3 = "",
                                                     ChartOfAcountName4 = "",
                                                     ChartOfAcountName5 = "",


                                                     ChartOfAcountCode1 = "",
                                                     ChartOfAcountCode2 = "",
                                                     ChartOfAcountCode3 = "",
                                                     ChartOfAcountCode4 = "",
                                                     ChartOfAcountCode5 = "",


                                                     GLAccountName = "",//= groupTrailOnlyCOAType.Key.GLAccountName,
                                                     GLAccountNumber = "",
                                                     GLAccountId = "",// groupTrailOnlyCOAType.Key.GLAccountId,
                                                     GLAccountEnglish = "",

                                                     ChartOfAccountId = "",

                                                     ChartOfAcountName1English = "",
                                                     ChartOfAcountName2English = "",
                                                     ChartOfAcountName3English = "",
                                                     ChartOfAcountName4English = "",
                                                     ChartOfAcountName5English = "",

                                                     LocalCloseBalancePeriod1 = groupTrailOnlyCOAType.Sum(x => x.LocalCloseBalancePeriod1),
                                                     LocalCloseBalancePeriod2 = groupTrailOnlyCOAType.Sum(x => x.LocalCloseBalancePeriod2),

                                                 }
                );

                        break;
                    case ReportLevel.Chartofaccount:
                        //tenantChartOfAccount5Level
                        _QTrailReportCOALevel = (from row in _QTrailReportCOALevel
                                                 group row by
                                                 new
                                                 {
                                                     row.ChartOfAcountType,
                                                     row.ChartOfAcount1,
                                                     row.ChartOfAcount2,
                                                     row.ChartOfAcount3,
                                                     row.ChartOfAcount4,
                                                     row.ChartOfAcount5,
                                                     row.ChartOfAcountName1,
                                                     row.ChartOfAcountName2,
                                                     row.ChartOfAcountName3,
                                                     row.ChartOfAcountName4,
                                                     row.ChartOfAcountName5,

                                                     row.ChartOfAcountName1English,
                                                     row.ChartOfAcountName2English,
                                                     row.ChartOfAcountName3English,
                                                     row.ChartOfAcountName4English,
                                                     row.ChartOfAcountName5English,

                                                     row.ChartOfAcountCode1,
                                                     row.ChartOfAcountCode2,
                                                     row.ChartOfAcountCode3,
                                                     row.ChartOfAcountCode4,
                                                     row.ChartOfAcountCode5,


                                                     //row.GLAccountName,
                                                     //row.GLAccountId
                                                     //row.CurrencyId,

                                                 }
                                                     into groupTrailOnlyCOAType
                                                 select new RevenueExpenseReportM()
                                                 {
                                                     ChartOfAcountType = groupTrailOnlyCOAType.Key.ChartOfAcountType,
                                                     ChartOfAcount1 = groupTrailOnlyCOAType.Key.ChartOfAcount1,
                                                     ChartOfAcount2 = groupTrailOnlyCOAType.Key.ChartOfAcount2,
                                                     ChartOfAcount3 = groupTrailOnlyCOAType.Key.ChartOfAcount3,
                                                     ChartOfAcount4 = groupTrailOnlyCOAType.Key.ChartOfAcount4,
                                                     ChartOfAcount5 = groupTrailOnlyCOAType.Key.ChartOfAcount5,
                                                     ChartOfAcountName1 = groupTrailOnlyCOAType.Key.ChartOfAcountName1,
                                                     ChartOfAcountName2 = groupTrailOnlyCOAType.Key.ChartOfAcountName2,
                                                     ChartOfAcountName3 = groupTrailOnlyCOAType.Key.ChartOfAcountName3,
                                                     ChartOfAcountName4 = groupTrailOnlyCOAType.Key.ChartOfAcountName4,
                                                     ChartOfAcountName5 = groupTrailOnlyCOAType.Key.ChartOfAcountName5,


                                                     ChartOfAcountCode1 = groupTrailOnlyCOAType.Key.ChartOfAcountCode1,
                                                     ChartOfAcountCode2 = groupTrailOnlyCOAType.Key.ChartOfAcountCode2,
                                                     ChartOfAcountCode3 = groupTrailOnlyCOAType.Key.ChartOfAcountCode3,
                                                     ChartOfAcountCode4 = groupTrailOnlyCOAType.Key.ChartOfAcountCode4,
                                                     ChartOfAcountCode5 = groupTrailOnlyCOAType.Key.ChartOfAcountCode5,


                                                     GLAccountName = "",//= groupTrailOnlyCOAType.Key.GLAccountName,
                                                     GLAccountNumber = "",
                                                     GLAccountId = "",// groupTrailOnlyCOAType.Key.GLAccountId,
                                                     GLAccountEnglish = "",
                                                     ChartOfAccountId = "",
                                                     // CurrencyId = groupTrailOnlyCOAType.Key.CurrencyId,


                                                     ChartOfAcountName1English = groupTrailOnlyCOAType.Key.ChartOfAcountName1English,
                                                     ChartOfAcountName2English = groupTrailOnlyCOAType.Key.ChartOfAcountName2English,
                                                     ChartOfAcountName3English = groupTrailOnlyCOAType.Key.ChartOfAcountName3English,
                                                     ChartOfAcountName4English = groupTrailOnlyCOAType.Key.ChartOfAcountName4English,
                                                     ChartOfAcountName5English = groupTrailOnlyCOAType.Key.ChartOfAcountName5English,

                                                     LocalCloseBalancePeriod1 = groupTrailOnlyCOAType.Sum(x => x.LocalCloseBalancePeriod1),
                                                     LocalCloseBalancePeriod2 = groupTrailOnlyCOAType.Sum(x => x.LocalCloseBalancePeriod2),

                                                 }
                );
                        break;
                    case ReportLevel.GLAccount:


                    default:
                        break;
                }
                _QBaseRevenueExpenseReportFull = _QTrailReportCOALevel;
                if (_QBaseRevenueExpenseReportFull == null)
                {
                    throw new Exception("(_QBaseRevenueExpenseReportFull==null)");
                }
                var l = _QBaseRevenueExpenseReportFull.ToList();
                DbLog = "";// _DbLogger.ToString();
                result = l;
                return l;
            
        }

        private  static IQueryable<TrailReportTemp> GetMoneyDataPerDate(
            DateTime FromDate,DateTime ToDate,
            int tenant, IAccountingContext accountingContext)
        {

            DateTime TODatebeginOfMonth = new DateTime(ToDate.Year, ToDate.Month, 1);
            var toDateAdd1Day = ToDate.AddDays(1);//INclude //
            DateTime FROMDateNextMonth = new DateTime(FromDate.Year, FromDate.Month, 1).AddMonths(1);
            DateTime FROMDateMinus1Day = FromDate.Date.AddDays(-1);

            IQueryable<TrailReportTemp> qTempTransStart = GetTransStartSection(FromDate, ToDate, tenant, accountingContext, FROMDateNextMonth, FROMDateMinus1Day);

            IQueryable<TrailReportTemp> qTempTotal = GetTotalByMonth(tenant, accountingContext, TODatebeginOfMonth, FROMDateNextMonth);

            IQueryable<TrailReportTemp> qTempTrans = GetTransEndSection(FromDate, ToDate, tenant, accountingContext, TODatebeginOfMonth, toDateAdd1Day, FROMDateMinus1Day);

            IQueryable<TrailReportTemp> _QUnionAllCurrSummary =
                null;//(qTempTotal).Union(qTempTrans);

            bool UnionreturnsDistinctvalues = true;
            if (UnionreturnsDistinctvalues)
            {
                _QUnionAllCurrSummary =
                       qTempTransStart.Concat(qTempTotal).Concat(qTempTrans);
            }

            return _QUnionAllCurrSummary;
        }

        private static IQueryable<TrailReportTemp> GetTransEndSection(DateTime FromDate, DateTime ToDate, int tenant, IAccountingContext accountingContext, DateTime TODatebeginOfMonth, DateTime toDateAdd1Day, DateTime FROMDateMinus1Day)
        {
            IQueryable<Data.EntityPOCOs.LedgerTransaction> QBaseTranactionBeginOfMonthToDateTillToDateInculde;
            QBaseTranactionBeginOfMonthToDateTillToDateInculde =
                (
                from trans in accountingContext.LedgerTransactions
                where trans.Tenant == tenant
                //toBeginOfMonth:20160201 until (InculdeAllTransOf)_RevenueExpenseReportParam.ToDate:20160215
                where trans.AccountingDate >= TODatebeginOfMonth  //20160201
                where trans.AccountingDate < toDateAdd1Day //_RevenueExpenseReportParam.ToDate.AddDays(1)//INclude //==20160216 
                select trans
                );

            if (new DateTime(FromDate.Year, FromDate.Month, 1) == new DateTime(ToDate.Year, ToDate.Month, 1))
            {
                QBaseTranactionBeginOfMonthToDateTillToDateInculde =
                    (
                    from trans in accountingContext.LedgerTransactions
                    where trans.Tenant == tenant
                    //toBeginOfMonth:20160201 until (InculdeAllTransOf)_RevenueExpenseReportParam.ToDate:20160215
                    where trans.AccountingDate > FROMDateMinus1Day  //20160201
                    where trans.AccountingDate < toDateAdd1Day //_RevenueExpenseReportParam.ToDate.AddDays(1)//INclude //==20160216 
                    select trans
                    );

            }
            if (FromDate.Month == 1 && FromDate.Day == 1)
            {

                var myLedgerTransactionRepository = new LedgerTransactionRepository(accountingContext);
                var qYearTransferLedgerTransaction = myLedgerTransactionRepository
                    .GetYearTransferLedgerTransaction(null, FromDate.Year, tenant);

                QBaseTranactionBeginOfMonthToDateTillToDateInculde =
                    QBaseTranactionBeginOfMonthToDateTillToDateInculde
                    .Where(r => !(qYearTransferLedgerTransaction.Select(yt => yt.Id)).Contains(r.Id));

            }
            var qTempTrans = (from r in QBaseTranactionBeginOfMonthToDateTillToDateInculde
                              select new TrailReportTemp()
                              {
                                  AccountId_COAType = r.AccountId,
                                  //CurrencyId = r.CurrencyId,
                                  //ForeignAmountCreditTotalStart = 0,
                                  //ForeignAmountDebitTotalStart = 0,
                                  //LocalAmountCreditTotalStart = 0,
                                  //LocalAmountDebitTotalStart = 0,


                                  //ForeignAmountCreditTransStart = 0,
                                  //ForeignAmountDebitTransStart = 0,
                                  LocalAmountCreditTransStart = 0,
                                  LocalAmountDebitTransStart = 0,


                                  //ForeignAmountCreditTotalDelta2End = 0,
                                  //ForeignAmountDebitTotalDelta2End = 0,
                                  LocalAmountCreditTotalDelta2End = 0,
                                  LocalAmountDebitTotalDelta2End = 0,


                                  //ForeignAmountCreditTransEnd = 0,// r.ForeignAmountCredit,
                                  //ForeignAmountDebitTransEnd = 0,// r.ForeignAmountDebit,
                                  LocalAmountCreditTransEnd = r.LocalAmountCredit,
                                  LocalAmountDebitTransEnd = r.LocalAmountDebit,

                              });
            return qTempTrans;
        }

        private static IQueryable<TrailReportTemp> GetTotalByMonth(int tenant, IAccountingContext accountingContext, DateTime TODatebeginOfMonth, DateTime FROMDateNextMonth)
        {
            IQueryable<Data.EntityPOCOs.GLAccountTotalByMonth> QBaseTotalsFromBirthTilStartOfMonthTo =
                (from tot in accountingContext.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == GLAccountTotalDateTypeValues.AccountingDate)

                 where tot.Tenant == tenant
                 select tot           
                 );

            if (FROMDateNextMonth.Year > TODatebeginOfMonth.Year)
            {
                QBaseTotalsFromBirthTilStartOfMonthTo =
                    (from tot in QBaseTotalsFromBirthTilStartOfMonthTo
                     where (tot.Year == -1111)
                     select tot
                );
            }
            if (FROMDateNextMonth.Year == TODatebeginOfMonth.Year )
            {
                if ( FROMDateNextMonth.Month < TODatebeginOfMonth.Month)
                {
                    QBaseTotalsFromBirthTilStartOfMonthTo =
                   (from tot in QBaseTotalsFromBirthTilStartOfMonthTo
                    where
                    (tot.Year == FROMDateNextMonth.Year && tot.Month >= FROMDateNextMonth.Month && tot.Month < TODatebeginOfMonth.Month)
                    select tot
               );
                }
                else
                {
                    QBaseTotalsFromBirthTilStartOfMonthTo =
                   (from tot in QBaseTotalsFromBirthTilStartOfMonthTo
                    where (tot.Year == -1111)
                    select tot
               );
                }
               

            }
            if (FROMDateNextMonth.Year < TODatebeginOfMonth.Year)
            {

                QBaseTotalsFromBirthTilStartOfMonthTo =
                    (from tot in QBaseTotalsFromBirthTilStartOfMonthTo

                     where
                     (tot.Year == FROMDateNextMonth.Year && tot.Month >= FROMDateNextMonth.Month) ||
                     (tot.Year > FROMDateNextMonth.Year && tot.Year < TODatebeginOfMonth.Year) ||
                     (tot.Year == TODatebeginOfMonth.Year && tot.Month < TODatebeginOfMonth.Month)
                     select tot
                );
            }


            var qTempTotal = (from tot in QBaseTotalsFromBirthTilStartOfMonthTo
                              select new TrailReportTemp()
                              {
                                  AccountId_COAType = tot.AccountId,
                                  //CurrencyId = tot.CurrencyId,
                                  //ForeignAmountCreditTotalStart = 0,
                                  //ForeignAmountDebitTotalStart = 0,
                                  //LocalAmountCreditTotalStart = 0,
                                  //LocalAmountDebitTotalStart = 0,


                                  //ForeignAmountCreditTransStart = 0,
                                  //ForeignAmountDebitTransStart = 0,
                                  LocalAmountCreditTransStart = 0,
                                  LocalAmountDebitTransStart = 0,


                                  //ForeignAmountCreditTotalDelta2End = 0,//tot.ForeignAmountCredit,
                                  //ForeignAmountDebitTotalDelta2End = 0,//tot.ForeignAmountDebit,
                                  LocalAmountCreditTotalDelta2End = tot.LocalAmountCredit,
                                  LocalAmountDebitTotalDelta2End = tot.LocalAmountDebit,


                                  //ForeignAmountCreditTransEnd = 0,
                                  //ForeignAmountDebitTransEnd = 0,
                                  LocalAmountCreditTransEnd = 0,
                                  LocalAmountDebitTransEnd = 0,

                              });
            return qTempTotal;
        }

        private static IQueryable<TrailReportTemp> GetTransStartSection(DateTime FromDate, DateTime ToDate, int tenant, IAccountingContext accountingContext, DateTime FROMDateNextMonth, DateTime FROMDateMinus1Day)
        {
            IQueryable<Data.EntityPOCOs.LedgerTransaction> QBaseTranactionFROMDateTillFROMDateNextOfMonthNotInclude = (
                from trans in accountingContext.LedgerTransactions
                where trans.Tenant == tenant
                select trans
                );
            if (new DateTime(FromDate.Year, FromDate.Month, 1) < new DateTime(ToDate.Year, ToDate.Month, 1))
            {
                QBaseTranactionFROMDateTillFROMDateNextOfMonthNotInclude =
                    (
                    from trans in QBaseTranactionFROMDateTillFROMDateNextOfMonthNotInclude
                        //toBeginOfMonth:20160201 until (InculdeAllTransOf)_RevenueExpenseReportParam.ToDate:20160215
                    where trans.AccountingDate > FROMDateMinus1Day  //20160201
                    where trans.AccountingDate < FROMDateNextMonth
                    select trans
                    );
            }
            else
            {
                QBaseTranactionFROMDateTillFROMDateNextOfMonthNotInclude =
                    (
                    from trans in QBaseTranactionFROMDateTillFROMDateNextOfMonthNotInclude
                    where trans.Id == "-1 not valid id"
                    select trans
                    );
                
            }
            if (FromDate.Month == 1 && FromDate.Day == 1)
            {

                var myLedgerTransactionRepository = new LedgerTransactionRepository(accountingContext);
                var qYearTransferLedgerTransaction = myLedgerTransactionRepository
                    .GetYearTransferLedgerTransaction(null, FromDate.Year, tenant);

                QBaseTranactionFROMDateTillFROMDateNextOfMonthNotInclude =
                    QBaseTranactionFROMDateTillFROMDateNextOfMonthNotInclude
                    .Where(r => !(qYearTransferLedgerTransaction.Select(yt => yt.Id)).Contains(r.Id));

            }
            var qTempTransStart = (from r in QBaseTranactionFROMDateTillFROMDateNextOfMonthNotInclude
                                   select new TrailReportTemp()
                                   {
                                       AccountId_COAType = r.AccountId,
                                       //CurrencyId = r.CurrencyId,
                                       //ForeignAmountCreditTotalStart = 0,
                                       //ForeignAmountDebitTotalStart = 0,
                                       //LocalAmountCreditTotalStart = 0,
                                       //LocalAmountDebitTotalStart = 0,


                                       //ForeignAmountCreditTransStart = 0,
                                       //ForeignAmountDebitTransStart = 0,
                                       LocalAmountCreditTransStart = r.LocalAmountCredit,
                                       LocalAmountDebitTransStart = r.LocalAmountDebit,


                                       //ForeignAmountCreditTotalDelta2End = 0,
                                       //ForeignAmountDebitTotalDelta2End = 0,
                                       LocalAmountCreditTotalDelta2End = 0,
                                       LocalAmountDebitTotalDelta2End = 0,


                                       //ForeignAmountCreditTransEnd = 0,// r.ForeignAmountCredit,
                                       //ForeignAmountDebitTransEnd = 0,// r.ForeignAmountDebit,
                                       LocalAmountCreditTransEnd = 0,
                                       LocalAmountDebitTransEnd = 0,

                                   });
            return qTempTransStart;
        }

        public void InteractiveCheck()
        {
            if (_RevenueExpenseReportParam.MyRevenueExpenseReportLevel == ReportLevel.GLAccount)
            {

            }
            else
            {
                if (_RevenueExpenseReportParam.MyCardFilter != RevenueExpenseReportParam.CardFilterEnum.DoNotShowCardWithZeroBalance)
                {
                    throw new Exception("MyCardFilter should be  DoNotShowCardWithZeroBalance");
                }
            }
        }


        private static IQueryable<RevenueExpenseReportM> CreateFullTrailReportQuery(IQueryable<RevenueExpenseReportM> qAllMoneySideRevenueExpenseReportM, IQueryable<ChartOfAccount5LevelM> QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy)
        {
            return (from data in qAllMoneySideRevenueExpenseReportM
                    join chartf in QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy
                    on data.GLAccountId equals chartf.GLAccountId
                    select new RevenueExpenseReportM()
                    {
                        ChartOfAcountType = chartf.ChartOfAccountTypeCode,
                        ChartOfAcount1 = chartf.Level1Id,
                        ChartOfAcount2 = chartf.Level2Id,
                        ChartOfAcount3 = chartf.Level3Id,
                        ChartOfAcount4 = chartf.Level4Id,
                        ChartOfAcount5 = chartf.Level5Id,

                        ChartOfAcountName1 = chartf.Level1Name,
                        ChartOfAcountName2 = chartf.Level2Name,
                        ChartOfAcountName3 = chartf.Level3Name,
                        ChartOfAcountName4 = chartf.Level4Name,
                        ChartOfAcountName5 = chartf.Level5Name,


                        ChartOfAcountCode1 = chartf.Level1Code,
                        ChartOfAcountCode2 = chartf.Level2Code,
                        ChartOfAcountCode3 = chartf.Level3Code,
                        ChartOfAcountCode4 = chartf.Level4Code,
                        ChartOfAcountCode5 = chartf.Level5Code,

                        GLAccountName = chartf.GLAccountName,
                        GLAccountNumber = chartf.GLAccountNumber,
                        GLAccountId = chartf.GLAccountId,
                        GLAccountEnglish = chartf.GLAccountEnglish,

                        ChartOfAccountId = chartf.ChartOfAccountId,


                        ChartOfAcountName1English = chartf.Level1English,
                        ChartOfAcountName2English = chartf.Level2English,
                        ChartOfAcountName3English = chartf.Level3English,
                        ChartOfAcountName4English = chartf.Level4English,
                        ChartOfAcountName5English = chartf.Level5English,

                        LocalCloseBalancePeriod1 = data.LocalCloseBalancePeriod1,
                        LocalCloseBalancePeriod2 = data.LocalCloseBalancePeriod2,
                    });
        }




        private IQueryable<ChartOfAccount5LevelM> JoinEachAccountWithisChartOfAccount5hierarchy(IQueryable<AccountCOAM> _QAllCardsAndDetialsAccType)
        {
            IQueryable<ChartOfAccount5LevelM> qAllCardsAndDetialsAccTypeBy5LevelHierarchy =
                //Join Each Account With is ChartOfAccount 5 hierarchy
            (from aGL in _QAllCardsAndDetialsAccType
             join chart in _QAllChartOfAccountFlattenBy5LevelofHierarchy
             on aGL.ChartOfAccountsId  //ChartOfAccountsId  is must (not null)
             equals chart.LeafId
             //into groupJoin
             select new ChartOfAccount5LevelM()
             {
                 Tenant = aGL.Tenant,


                 Level1Id = chart.Level1Id,
                 Level1Name = chart.Level1Name,
                 Level1Code=chart.Level1Code,
                 Level1English = chart.Level1English,

                 Level2Id = chart.Level2Id,
                 Level2Name = chart.Level2Name,
                 Level2Code = chart.Level2Code,
                 Level2English = chart.Level2English,

                 Level3Id = chart.Level3Id,
                 Level3Name = chart.Level3Name,
                 Level3Code = chart.Level3Code,
                 Level3English = chart.Level3English,

                 Level4Id = chart.Level4Id,
                 Level4Name = chart.Level4Name,
                 Level4Code = chart.Level4Code,
                 Level4English = chart.Level4English,

                 Level5Id = chart.Level5Id,
                 Level5Name = chart.Level5Name,
                 Level5Code = chart.Level5Code,
                 Level5English = chart.Level5English,

                 GLAccountId = aGL.Id,
                 GLAccountName = aGL.LocalName,
                 GLAccountNumber= aGL.DisplayNumber,
                 ChartOfAccountId = aGL.ChartOfAccountsId,
                 ChartOfAccountTypeCode = //aGL.AccountTypeCode,
                 aGL.ChartOfAccountsTypeCode,

				 ChartOfAccountsTypeEnglish =  chart.ChartOfAccountsTypeEnglish,
                 ChartOfAccountsEnglish = chart.ChartOfAccountsEnglish,
                 GLAccountEnglish = aGL.EnglishName,

                 LeafId = chart.LeafId,


             }
                 );
            return qAllCardsAndDetialsAccTypeBy5LevelHierarchy;
        }




        //public void Dispose()
        //{
        //    //bool testTimeout = false;
        //    //if (testTimeout)
        //    //{
        //    //    Thread.Sleep(TimeSpan.FromMinutes(10));
        //    //} 
        //    ///_DbLogger.Dispose();
        //    //_TransactionScope.Dispose();
        //}

        public string DbLog { get; set; }
    }






    public class RevenueExpenseReportParam
    {
        /*
לתאריך - כמובן כולל היום 
הדוח כולל עד 500 כרטיסים - אין צורך למיטב 
        */

        //filter for alll!!!
        public int Tenant { get; set; }

        //filter for Money!!!
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


        public DateTime? FromDatePeriod2 { get; set; }
        public DateTime? ToDatePeriod2 { get; set; }

        //filter the GLAccount (if COATypeLevel) + Group by 
        public ReportLevel MyRevenueExpenseReportLevel { get; set; }
        public List<string> ChartOfAccountsTypes { get; set; }
        public List<string> ChartOfAccounts{ get; set; }




        public CardFilterEnum MyCardFilter { get; set; }

        
        public enum CardFilterEnum
        {
            DoNotShowCardWithZeroBalance=0,//Default
            ShowCardsWithActivity_EvenBalanceItsZero=1,
            ShowAllCard=2,
            ShowCardsWithActivity_AndBalanceNotZero = 3
        }
    }
     
    public class RevenueExpenseReportM
    {


        public string ChartOfAcountType { get; set; }
        public string ChartOfAcount1 { get; set; }
        public string ChartOfAcount2 { get; set; }
        public string ChartOfAcount3 { get; set; }
        public string ChartOfAcount4 { get; set; }
        public string ChartOfAcount5 { get; set; }
        public string ChartOfAcountName1 { get; set; }
        public string ChartOfAcountName2 { get; set; }
        public string ChartOfAcountName3 { get; set; }
        public string ChartOfAcountName4 { get; set; }
        public string ChartOfAcountName5 { get; set; }
        public string ChartOfAcountName1English { get; set; }
        public string ChartOfAcountName2English { get; set; }
        public string ChartOfAcountName3English { get; set; }
        public string ChartOfAcountName4English { get; set; }
        public string ChartOfAcountName5English { get; set; }
        public string ChartOfAcountCode1 { get; set; }
        public string ChartOfAcountCode2 { get; set; }
        public string ChartOfAcountCode3 { get; set; }
        public string ChartOfAcountCode4 { get; set; }
        public string ChartOfAcountCode5 { get; set; }
 

        public string GLAccountName { get; set; }

        public string GLAccountId { get; set; }
        public string GLAccountNumber { get; set; }
        public string ChartOfAccountId { get; set; }

        public decimal? LocalCloseBalancePeriod1 { get; set; }

        public decimal? LocalCloseBalancePeriod2 { get; set; }

        // public RevenueExpenseEnum MyRevenueExpenseEnum { get; set; }

        public string GLAccountEnglish { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("COAT:" + this.ChartOfAcountType);
            sb.Append("COA:" + this.ChartOfAcount1 +
        this.ChartOfAcount2 +
        this.ChartOfAcount3 +
        this.ChartOfAcount4 +
        this.ChartOfAcount5);
            if (!String.IsNullOrWhiteSpace(this.GLAccountName + this.GLAccountId))
            {
                sb.Append("ACC:" + this.GLAccountName + "," + this.GLAccountId);
            }


           




            sb.Append("Local:");

            sb.Append("CB:").Append(this.LocalCloseBalancePeriod1.GetValueOrDefault().ToString());
            sb.Append("CB2:").Append(this.LocalCloseBalancePeriod2.GetValueOrDefault().ToString());


            return sb.ToString();
        }
        public enum RevenueExpenseEnum
        {

            Revenues = 1,
            Expenses = 2
        }
    }

}
