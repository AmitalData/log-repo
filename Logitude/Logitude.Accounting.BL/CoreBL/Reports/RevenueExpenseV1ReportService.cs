
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
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
    public class RevenueExpenseV1ReportService : IDisposable
    {
        /*
לתאריך - כמובן כולל היום 
הדוח כולל עד 500 כרטיסים - אין צורך למיטב 
        */

        protected RevenueExpenseReportParam _RevenueExpenseReportParam = null;
        protected readonly int __TimeOutInMinutes = 129;
        private System.Transactions.TransactionScope _TransactionScope;
        protected IAccountingContext _AccountingContext;
        private FullAccountingSettingPM _FullAccountingSetting;
        protected IQueryable<AccountCOAM> QAllRevenueExpenseCardsCOAM;
        private IQueryable<ChartOfAccount5LevelM> _QAllChartOfAccountFlattenBy5LevelofHierarchy;

        protected IEnumerable //IQueryable
            <RevenueExpenseReportM> _QBaseRevenueExpenseReportFull = null;
        private DbContextBase.IDbContextLogger _DbLogger;






        DateTime _ToBeginOfMonth;







        protected IQueryable<Data.EntityPOCOs.LedgerTransaction> QBaseTranactionBeginOfMonthToDateTillToDateInculde;
        protected IQueryable<Data.EntityPOCOs.GLAccountTotalByMonth> QBaseTotalsFromBirthTilStartOfMonthTo;


        protected IQueryable<ChartOfAccount5LevelM> QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy;


        public RevenueExpenseV1ReportService(RevenueExpenseReportParam RevenueExpenseReportParam, int timeOutInMinutes)
        {
            _RevenueExpenseReportParam = RevenueExpenseReportParam;
            __TimeOutInMinutes = timeOutInMinutes;

        }

        public List<RevenueExpenseReportM> result;
        public List<RevenueExpenseReportM> Execute()
        {

            _RevenueExpenseReportParam.ToDate = _RevenueExpenseReportParam.ToDate.Date;

            _ToBeginOfMonth = new DateTime(_RevenueExpenseReportParam.ToDate.Year, _RevenueExpenseReportParam.ToDate.Month, 1);
            _TransactionScope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(__TimeOutInMinutes)); //snapshot isolation performance

            _AccountingContext = AccountingContext.GetContext(_RevenueExpenseReportParam.Tenant);
            _DbLogger = (_AccountingContext as DbContextBase).CreateLogger();

            _FullAccountingSetting = //Hope From Cache
                FullAccountingSettingQueryService
                .Get(_RevenueExpenseReportParam.Tenant);


            var repoGLAccount = new GLAccountRepository(_AccountingContext);
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



            QBaseTotalsFromBirthTilStartOfMonthTo =
               (from tot in _AccountingContext.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == GLAccountTotalDateTypeValues.AccountingDate)

                where tot.Tenant == _RevenueExpenseReportParam.Tenant

                where tot.Year < _ToBeginOfMonth.Year ||
                (tot.Year == _ToBeginOfMonth.Year && tot.Month < _ToBeginOfMonth.Month)
                select tot
                );
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
                                  //LocalAmountCreditTransStart = 0,
                                  //LocalAmountDebitTransStart = 0,


                                  //ForeignAmountCreditTotalDelta2End = 0,//tot.ForeignAmountCredit,
                                  //ForeignAmountDebitTotalDelta2End = 0,//tot.ForeignAmountDebit,
                                  LocalAmountCreditTotalDelta2End = tot.LocalAmountCredit,
                                  LocalAmountDebitTotalDelta2End = tot.LocalAmountDebit,


                                  //ForeignAmountCreditTransEnd = 0,
                                  //ForeignAmountDebitTransEnd = 0,
                                  LocalAmountCreditTransEnd = 0,
                                  LocalAmountDebitTransEnd = 0,

                              });




            var toDateAdd1Day = _RevenueExpenseReportParam.ToDate.AddDays(1);//INclude //
            QBaseTranactionBeginOfMonthToDateTillToDateInculde =
                (
                from trans in _AccountingContext.LedgerTransactions
                where trans.Tenant == _RevenueExpenseReportParam.Tenant
                //toBeginOfMonth:20160201 until (InculdeAllTransOf)_RevenueExpenseReportParam.ToDate:20160215
                where trans.AccountingDate >= _ToBeginOfMonth  //20160201
                where trans.AccountingDate <
                toDateAdd1Day //_RevenueExpenseReportParam.ToDate.AddDays(1)//INclude //==20160216 
                select trans
                );

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
                                  //LocalAmountCreditTransStart = 0,
                                  //LocalAmountDebitTransStart = 0,


                                  //ForeignAmountCreditTotalDelta2End = 0,
                                  //ForeignAmountDebitTotalDelta2End = 0,
                                  LocalAmountCreditTotalDelta2End = 0,
                                  LocalAmountDebitTotalDelta2End = 0,


                                  //ForeignAmountCreditTransEnd = 0,// r.ForeignAmountCredit,
                                  //ForeignAmountDebitTransEnd = 0,// r.ForeignAmountDebit,
                                  LocalAmountCreditTransEnd = r.LocalAmountCredit,
                                  LocalAmountDebitTransEnd = r.LocalAmountDebit,

                              });

            var qsChartOfAccount = new ChartOfAccountQueryService(_AccountingContext);
            _QAllChartOfAccountFlattenBy5LevelofHierarchy = //Flatten ChartOfAccount By 5 Level hierarchy
                qsChartOfAccount
                .GetQChartOfAccount5LevelM(_RevenueExpenseReportParam.Tenant, _RevenueExpenseReportParam.ChartOfAccountsTypes, _RevenueExpenseReportParam.ChartOfAccounts); // send the filtered ids are selected, from ChartOfAccountsTypes and ChartOfAccounts tables from UI #192454

            QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy =
            JoinEachAccountWithisChartOfAccount5hierarchy(QAllRevenueExpenseCardsCOAM);





            IQueryable<TrailReportTemp> _QUnionAllCurrSummary =
                (qTempTotal).Union(qTempTrans);
            bool UnionreturnsDistinctvalues = true;
            if (UnionreturnsDistinctvalues)
            {
                _QUnionAllCurrSummary =
                    (qTempTotal).Concat(qTempTrans);
            }
            string debugAccId = "";//"1-216621"
            if (!string.IsNullOrWhiteSpace(debugAccId))
            {
                var myData = _QUnionAllCurrSummary.Where(r => r.AccountId_COAType == debugAccId).ToList();
            }


            IQueryable<RevenueExpenseReportM> qAllMoneySideRevenueExpenseReportM
                =
                (from r in _QUnionAllCurrSummary
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
                     ChartOfAccountId = "",

                     ChartOfAcountName1English = "",
                     ChartOfAcountName2English = "",
                     ChartOfAcountName3English = "",
                     ChartOfAcountName4English = "",
                     ChartOfAcountName5English = "",

                     LocalCloseBalancePeriod1 =
                     (
                     +g.Sum(x => x.LocalAmountDebitTotalDelta2End)
                     - g.Sum(x => x.LocalAmountCreditTotalDelta2End)
                     + g.Sum(x => x.LocalAmountDebitTransEnd)
                     - g.Sum(x => x.LocalAmountCreditTransEnd)
                     ),

                 });


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

             });
            if (_RevenueExpenseReportParam.MyRevenueExpenseReportLevel == ReportLevel.GLAccount)
            {
                switch (_RevenueExpenseReportParam.MyCardFilter)
                {
                    case RevenueExpenseReportParam.CardFilterEnum.DoNotShowCardWithZeroBalance:
                        _QTrailReportFull =
                            _QTrailReportFull
                            .Where(r => r.LocalCloseBalancePeriod1 != null && r.LocalCloseBalancePeriod1 != 0m);
                        break;


                    case RevenueExpenseReportParam.CardFilterEnum.ShowCardsWithActivity_EvenBalanceItsZero:
                        _QTrailReportFull = CreateFullTrailReportQuery(qAllMoneySideRevenueExpenseReportM, QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy);
                        break;


                    case RevenueExpenseReportParam.CardFilterEnum.ShowCardsWithActivity_AndBalanceNotZero:
                        _QTrailReportFull = CreateFullTrailReportQuery(qAllMoneySideRevenueExpenseReportM, QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy)
                            .Where(r => r.LocalCloseBalancePeriod1 != null && r.LocalCloseBalancePeriod1 != 0m);

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

                                                 ChartOfAccountId = "",


                                                 ChartOfAcountName1English = "",
                                                 ChartOfAcountName2English = "",
                                                 ChartOfAcountName3English = "",
                                                 ChartOfAcountName4English = "",
                                                 ChartOfAcountName5English = "",

                                                 LocalCloseBalancePeriod1 = groupTrailOnlyCOAType.Sum(x => x.LocalCloseBalancePeriod1),

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
                                                 ChartOfAccountId = "",
                                                 // CurrencyId = groupTrailOnlyCOAType.Key.CurrencyId,


                                                 ChartOfAcountName1English = groupTrailOnlyCOAType.Key.ChartOfAcountName1English,
                                                 ChartOfAcountName2English = groupTrailOnlyCOAType.Key.ChartOfAcountName2English,
                                                 ChartOfAcountName3English = groupTrailOnlyCOAType.Key.ChartOfAcountName3English,
                                                 ChartOfAcountName4English = groupTrailOnlyCOAType.Key.ChartOfAcountName4English,
                                                 ChartOfAcountName5English = groupTrailOnlyCOAType.Key.ChartOfAcountName5English,

                                                 LocalCloseBalancePeriod1 = groupTrailOnlyCOAType.Sum(x => x.LocalCloseBalancePeriod1),

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
            DbLog = _DbLogger.ToString();
            result = l;
            return l;
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
                        ChartOfAccountId = chartf.ChartOfAccountId,
                        GLAccountEnglish = chartf.GLAccountEnglish,


                        ChartOfAcountName1English = chartf.Level1English,
                        ChartOfAcountName2English = chartf.Level2English,
                        ChartOfAcountName3English = chartf.Level3English,
                        ChartOfAcountName4English = chartf.Level4English,
                        ChartOfAcountName5English = chartf.Level5English,

                        LocalCloseBalancePeriod1 = data.LocalCloseBalancePeriod1,
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
                 Level1Code = chart.Level1Code,
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
                 GLAccountNumber = aGL.DisplayNumber,
                 ChartOfAccountId = aGL.ChartOfAccountsId,
                 ChartOfAccountTypeCode = //aGL.AccountTypeCode,
                 aGL.ChartOfAccountsTypeCode,

                 ChartOfAccountsTypeEnglish = chart.ChartOfAccountTypeCode,
                 ChartOfAccountsEnglish = chart.ChartOfAccountTypeCode,
                 GLAccountEnglish = aGL.EnglishName,

                 LeafId = chart.LeafId,


             }
                 );
            return qAllCardsAndDetialsAccTypeBy5LevelHierarchy;
        }




        public void Dispose()
        {
            bool testTimeout = false;
            if (testTimeout)
            {
                Thread.Sleep(TimeSpan.FromMinutes(10));
            }
            _DbLogger.Dispose();
            _TransactionScope.Dispose();
        }

        public string DbLog { get; set; }
    }






}
