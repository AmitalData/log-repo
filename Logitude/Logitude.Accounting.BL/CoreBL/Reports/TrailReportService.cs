using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using System.Threading;

using Logitude.Accounting.Def.EntityPMs;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public class TrailReportService : IDisposable
    {
        TrailReportParam _TrailReportParam = null;
        public TrailReportService(TrailReportParam trailReportParam)
        {
            _TrailReportParam = trailReportParam;
            //var trailReportChartofaccountType = new TrailReportChartofaccountType();
            //var trailReportChartofaccount= new TrailReportChartofaccount();
            //var trailReportGLAccount = new TrailReportGLAccount();
        }
        int __TimeOutInMinutes = 129;
        protected IQueryable<ChartOfAccount5LevelM> _QAllChartOfAccountFlattenBy5LevelofHierarchy;
        protected FullAccountingSettingPM _FullAccountingSetting;
        protected IQueryable<ChartOfAccount5LevelM> _QAllCardsAndDetialsAccTypeBy5LevelHierarchy;
        protected IQueryable<TrailReportTemp> _QAccumulateTotalsFrom0BCTilNotIncludeStartOfMonthFromDate;
        protected IQueryable<TrailReportTemp> _QAccumulateTotalsFromStartOfMonthFromTilStartOfMonthTo;
        protected IQueryable<TrailReportTemp> _QAccumulateTranactionBeginOfMonthFromTillFromDateNotInclude;
        protected IQueryable<TrailReportTemp> _QAccumulateTranactionBeginOfMonthToDateTillToDateInculde;
        protected IQueryable<TrailReportM> _QTrailReportFull;
        protected IQueryable<TrailReportM> _QTrailReportCOALevel;
        protected IQueryable<TrailReportM> _QTrailReportCOALevelACurrenciesDetailed;
        private System.Transactions.TransactionScope _TransactionScope;
        private IQueryable<Data.EntityPOCOs.GLAccount> _QAllCardsAndDetialsAccType;
        private IAccountingContext _AccountingContext;
        public void ParamValidation()
        {
        }
        public void Prepare()
        {
            var testNow = false;// (new DateTime(2016, 12, 30) > DateTime.Now);

            if (_TrailReportParam.FromDate < _TrailReportParam.ToDate)
            {
            }
            else
            {
                throw new Exception("!(_TrailReportParam.FromDate < _TrailReportParam.ToDate)");
            }
            _TrailReportParam.FromDate = _TrailReportParam.FromDate.Date;
            _TrailReportParam.ToDate = _TrailReportParam.ToDate.Date;
            var fromBeginOfMonth = new DateTime(_TrailReportParam.FromDate.Year, _TrailReportParam.FromDate.Month, 1);

            var toBeginOfMonth = new DateTime(_TrailReportParam.ToDate.Year, _TrailReportParam.ToDate.Month, 1);

            _TransactionScope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(__TimeOutInMinutes)); //snapshot isolation performance

            _AccountingContext = AccountingContext.GetContext(_TrailReportParam.Tenant);
            //var qsFullAccountingSetting =  new FullAccountingSettingQueryService(accountingContext );
            var repoTotalByMonth = new GLAccountTotalByMonthRepository(_AccountingContext);
            var repoLedgerTransaction = new LedgerTransactionRepository(_AccountingContext);
            var repoGLAccount = new GLAccountRepository(_AccountingContext);
            var qsChartOfAccount = new ChartOfAccountQueryService(_AccountingContext);
            _QAllChartOfAccountFlattenBy5LevelofHierarchy = //Flatten ChartOfAccount By 5 Level hierarchy
                qsChartOfAccount
                .GetQChartOfAccount5LevelM(_TrailReportParam.Tenant, _TrailReportParam.ChartOfAccountsTypeCodeList, _TrailReportParam.ChartOfAccountsIdList
                ///,_TrailReportParam.MyTrailReportLevel == TrailReportLevel.ChartofaccountType
                );
            if (testNow)
            {
                var test111 = _QAllChartOfAccountFlattenBy5LevelofHierarchy.ToList();
            }


            _FullAccountingSetting = //Hope From Cache
                FullAccountingSettingQueryService
                .Get(_TrailReportParam.Tenant);


            _QAllCardsAndDetialsAccType = //Get The Account List
                repoGLAccount.
                GetQAllCardsAndDetailsAccType(_TrailReportParam.Tenant,
                GetClientContolAcc(_FullAccountingSetting),
                GetVendorContolAcc(_FullAccountingSetting),
                GetJobContolAcc(_FullAccountingSetting),
                GetFileContolAcc(_FullAccountingSetting))
                //.Where(a => !a.Inactive)
                ;



            _QAllCardsAndDetialsAccTypeBy5LevelHierarchy =
            JoinEachAccountWithisChartOfAccount5hierarchy(_QAllCardsAndDetialsAccType);
            if (testNow)
            {
                var test1 = _QAllCardsAndDetialsAccTypeBy5LevelHierarchy.ToList();
            }


            _QAccumulateTotalsFrom0BCTilNotIncludeStartOfMonthFromDate =
                //Accumulate Totals From the beginning of  Account Use Until(NotInclude) (StartOfMonth)FromDate
                GetQTotalsOpenBalanceMonth(fromBeginOfMonth, repoTotalByMonth);


            _QAccumulateTotalsFromStartOfMonthFromTilStartOfMonthTo = //Accumulate Totals From StartOfMonth(FromDate) Until StartOfMonth(ToDate) 
                GetQTotalsDeltaUntilCloseBalance(fromBeginOfMonth, toBeginOfMonth, repoTotalByMonth);




            _QAccumulateTranactionBeginOfMonthFromTillFromDateNotInclude =
                //Accumulate Tranaction BeginOfMonth(fromDate) till (FromDate-1d)
                GetQTransFrom4OpenBalance(fromBeginOfMonth, repoLedgerTransaction);


            _QAccumulateTranactionBeginOfMonthToDateTillToDateInculde =
                //Accumulate Tranaction BeginOfMonth(ToDate) till ToDate
                GetQTransTo4CloseBalance(toBeginOfMonth, repoLedgerTransaction);


            IQueryable<TrailReportTemp> _QUnionAllCurrSummary = _QAccumulateTotalsFrom0BCTilNotIncludeStartOfMonthFromDate.Union(_QAccumulateTranactionBeginOfMonthFromTillFromDateNotInclude).Union(_QAccumulateTotalsFromStartOfMonthFromTilStartOfMonthTo).Union(_QAccumulateTranactionBeginOfMonthToDateTillToDateInculde);
            if (testNow)
            {
                var test1111 = _QUnionAllCurrSummary.ToList();
            }


            IQueryable<TrailReportM> qMapAllCurrencySum2TRail =
                //map All to TrailReportM Schem 
                GetQAllTrailReportM(_QUnionAllCurrSummary);



            _QTrailReportFull =
                ///join 
            (from chartf in _QAllCardsAndDetialsAccTypeBy5LevelHierarchy
             join data in qMapAllCurrencySum2TRail
             on chartf.GLAccountId equals data.GLAccountId
             into groupJoin
             from groupJoinData in groupJoin.DefaultIfEmpty()
             select new TrailReportM()
             {
                
                 ChartOfAcountType = chartf.ChartOfAccountTypeCode,
                 ChartOfAcount1 = chartf.Level1Id,
                 ChartOfAcount2 = chartf.Level2Id,
                 ChartOfAcount3 = chartf.Level3Id,
                 ChartOfAcount4 = chartf.Level4Id,
                 ChartOfAcount5 = chartf.Level5Id,

                 ChartOfAcountCode1 = chartf.Level1Code,
                 ChartOfAcountCode2 = chartf.Level2Code,
                 ChartOfAcountCode3 = chartf.Level3Code,
                 ChartOfAcountCode4 = chartf.Level4Code,
                 ChartOfAcountCode5 = chartf.Level5Code,

                 ChartOfAcountName1 = chartf.Level1Name,
                 ChartOfAcountName2 = chartf.Level2Name,
                 ChartOfAcountName3 = chartf.Level3Name,
                 ChartOfAcountName4 = chartf.Level4Name,
                 ChartOfAcountName5 = chartf.Level5Name,





                 ChartOfAccountId = chartf.ChartOfAccountId,
                 GLAccountName = chartf.GLAccountName,
                 GLAccountNumber = chartf.GLAccountNumber,
                 GLAccountId = chartf.GLAccountId,


                 ChartOfAcountName1English = chartf.Level1English,
                 ChartOfAcountName2English = chartf.Level2English,
                 ChartOfAcountName3English = chartf.Level3English,
                 ChartOfAcountName4English = chartf.Level4English,
                 ChartOfAcountName5English = chartf.Level5English,

                 GLAccountEnglish = chartf.GLAccountEnglish,
                 ChartOfAccountsTypeEnglish = chartf.ChartOfAccountsTypeEnglish,
                 ChartOfAccountsEnglish = chartf.ChartOfAccountsEnglish,

                 CurrencyId = groupJoinData.CurrencyId,


                 LocalOpenBalance = groupJoinData.LocalOpenBalance,
                 LocalDebit = groupJoinData.LocalDebit,
                 LocalCredit = groupJoinData.LocalCredit,
                 LocalCloseBalance = groupJoinData.LocalCloseBalance,



                 ForeignOpenBalance = groupJoinData.ForeignOpenBalance,

                 ForeignDebit = groupJoinData.ForeignDebit,

                 ForeignCredit = groupJoinData.ForeignCredit,

                 ForeignCloseBalance = groupJoinData.ForeignCloseBalance,
             });
            if (testNow)
            {
                var tettt = _QTrailReportFull.ToList();
            }


            _QTrailReportCOALevel = _QTrailReportFull;

            switch (_TrailReportParam.MyTrailReportLevel)
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
                                                 row.CurrencyId,

                                             }
                                                 into groupTrailOnlyCOAType
                                                 select new TrailReportM()
                                                 {
                                                     ChartOfAcountType = groupTrailOnlyCOAType.Key.ChartOfAcountType,
                                                     ChartOfAcount1 = "",//groupTrailOnlyCOAType.Key.ChartOfAcount1,
                                                     ChartOfAcount2 = "",//groupTrailOnlyCOAType.Key.ChartOfAcount2,
                                                     ChartOfAcount3 = "",//groupTrailOnlyCOAType.Key.ChartOfAcount3,
                                                     ChartOfAcount4 = "",//groupTrailOnlyCOAType.Key.ChartOfAcount4,
                                                     ChartOfAcount5 = "",//groupTrailOnlyCOAType.Key.ChartOfAcount5,

                                                     ChartOfAcountCode1 = "",
                                                     ChartOfAcountCode2 = "",
                                                     ChartOfAcountCode3 = "",
                                                     ChartOfAcountCode4 = "",
                                                     ChartOfAcountCode5 = "",

                                                     ChartOfAcountName1 = "",
                                                     ChartOfAcountName2 = "",
                                                     ChartOfAcountName3 = "",
                                                     ChartOfAcountName4 = "",
                                                     ChartOfAcountName5 = "",




                                                     ChartOfAccountId = "",
                                                     GLAccountName = "",//= groupTrailOnlyCOAType.Key.GLAccountName,
                                                     GLAccountNumber = "",
                                                     GLAccountId = "",// groupTrailOnlyCOAType.Key.GLAccountId,



                                                     ChartOfAcountName1English = "",
                                                     ChartOfAcountName2English = "",
                                                     ChartOfAcountName3English = "",
                                                     ChartOfAcountName4English = "",
                                                     ChartOfAcountName5English = "",

                                                     GLAccountEnglish = "",
                                                     ChartOfAccountsTypeEnglish = "",
                                                     ChartOfAccountsEnglish = "", 

                                                     CurrencyId = groupTrailOnlyCOAType.Key.CurrencyId,


                                                     LocalOpenBalance = groupTrailOnlyCOAType.Sum(x => x.LocalOpenBalance),
                                                     LocalDebit = groupTrailOnlyCOAType.Sum(x => x.LocalDebit),
                                                     LocalCredit = groupTrailOnlyCOAType.Sum(x => x.LocalCredit),
                                                     LocalCloseBalance = groupTrailOnlyCOAType.Sum(x => x.LocalCloseBalance),



                                                     ForeignOpenBalance = groupTrailOnlyCOAType.Sum(x => x.ForeignOpenBalance),
                                                     ForeignDebit = groupTrailOnlyCOAType.Sum(x => x.ForeignDebit),
                                                     ForeignCredit = groupTrailOnlyCOAType.Sum(x => x.ForeignCredit),
                                                     ForeignCloseBalance = groupTrailOnlyCOAType.Sum(x => x.ForeignCloseBalance),
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


                                                 row.ChartOfAcountCode1,
                                                 row.ChartOfAcountCode2,
                                                 row.ChartOfAcountCode3,
                                                 row.ChartOfAcountCode4,
                                                 row.ChartOfAcountCode5,

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

                                                 
                                                 //row.GLAccountName,
                                                 //row.GLAccountId
                                                 row.CurrencyId,

                                             }
                                                 into groupTrailOnlyCOAType
                                                 select new TrailReportM()
                                                 {
                                                     ChartOfAcountType = groupTrailOnlyCOAType.Key.ChartOfAcountType,
                                                     ChartOfAcount1 = groupTrailOnlyCOAType.Key.ChartOfAcount1,
                                                     ChartOfAcount2 = groupTrailOnlyCOAType.Key.ChartOfAcount2,
                                                     ChartOfAcount3 = groupTrailOnlyCOAType.Key.ChartOfAcount3,
                                                     ChartOfAcount4 = groupTrailOnlyCOAType.Key.ChartOfAcount4,
                                                     ChartOfAcount5 = groupTrailOnlyCOAType.Key.ChartOfAcount5,


                                                     ChartOfAcountCode1 = groupTrailOnlyCOAType.Key.ChartOfAcountCode1,
                                                     ChartOfAcountCode2 = groupTrailOnlyCOAType.Key.ChartOfAcountCode2,
                                                     ChartOfAcountCode3 = groupTrailOnlyCOAType.Key.ChartOfAcountCode3,
                                                     ChartOfAcountCode4 = groupTrailOnlyCOAType.Key.ChartOfAcountCode4,
                                                     ChartOfAcountCode5 = groupTrailOnlyCOAType.Key.ChartOfAcountCode5,

                                                     ChartOfAcountName1 = groupTrailOnlyCOAType.Key.ChartOfAcountName1,
                                                     ChartOfAcountName2 = groupTrailOnlyCOAType.Key.ChartOfAcountName2,
                                                     ChartOfAcountName3 = groupTrailOnlyCOAType.Key.ChartOfAcountName3,
                                                     ChartOfAcountName4 = groupTrailOnlyCOAType.Key.ChartOfAcountName4,
                                                     ChartOfAcountName5 = groupTrailOnlyCOAType.Key.ChartOfAcountName5,



                                                     ChartOfAccountId = "",
                                                     GLAccountName = "",//= groupTrailOnlyCOAType.Key.GLAccountName,
                                                     GLAccountNumber="",
                                                     GLAccountId = "",// groupTrailOnlyCOAType.Key.GLAccountId,



                                                     ChartOfAcountName1English = groupTrailOnlyCOAType.Key.ChartOfAcountName1English,
                                                     ChartOfAcountName2English = groupTrailOnlyCOAType.Key.ChartOfAcountName2English,
                                                     ChartOfAcountName3English = groupTrailOnlyCOAType.Key.ChartOfAcountName3English,
                                                     ChartOfAcountName4English = groupTrailOnlyCOAType.Key.ChartOfAcountName4English,
                                                     ChartOfAcountName5English = groupTrailOnlyCOAType.Key.ChartOfAcountName5English,

                                                     GLAccountEnglish = "",
                                                     ChartOfAccountsTypeEnglish = "",
                                                     ChartOfAccountsEnglish = "", 

                                                     CurrencyId = groupTrailOnlyCOAType.Key.CurrencyId,


                                                     LocalOpenBalance = groupTrailOnlyCOAType.Sum(x => x.LocalOpenBalance),
                                                     LocalDebit = groupTrailOnlyCOAType.Sum(x => x.LocalDebit),
                                                     LocalCredit = groupTrailOnlyCOAType.Sum(x => x.LocalCredit),
                                                     LocalCloseBalance = groupTrailOnlyCOAType.Sum(x => x.LocalCloseBalance),



                                                     ForeignOpenBalance = groupTrailOnlyCOAType.Sum(x => x.ForeignOpenBalance),
                                                     ForeignDebit = groupTrailOnlyCOAType.Sum(x => x.ForeignDebit),
                                                     ForeignCredit = groupTrailOnlyCOAType.Sum(x => x.ForeignCredit),
                                                     ForeignCloseBalance = groupTrailOnlyCOAType.Sum(x => x.ForeignCloseBalance),
                                                 }
            );
                    break;
                case ReportLevel.GLAccount:


                default:
                    break;
            }

            _QTrailReportCOALevelACurrenciesDetailed = _QTrailReportCOALevel;
            if (!_TrailReportParam.CurrenciesDetailed)
            {
                _QTrailReportCOALevelACurrenciesDetailed =
                    (from row in _QTrailReportCOALevel
                     group row by
                     new
                     {
                         row.ChartOfAcountType,
                         row.ChartOfAcount1,
                         row.ChartOfAcount2,
                         row.ChartOfAcount3,
                         row.ChartOfAcount4,
                         row.ChartOfAcount5,


                         row.ChartOfAcountCode1,
                         row.ChartOfAcountCode2,
                         row.ChartOfAcountCode3,
                         row.ChartOfAcountCode4,
                         row.ChartOfAcountCode5,

                         row.ChartOfAcountName1,
                         row.ChartOfAcountName2,
                         row.ChartOfAcountName3,
                         row.ChartOfAcountName4,
                         row.ChartOfAcountName5,




                         row.ChartOfAccountId,
                         row.GLAccountName,
                         row.GLAccountNumber,
                         row.GLAccountId,


                         row.ChartOfAcountName1English,
                         row.ChartOfAcountName2English,
                         row.ChartOfAcountName3English,
                         row.ChartOfAcountName4English,
                         row.ChartOfAcountName5English,

                         row.GLAccountEnglish,
                         row.ChartOfAccountsTypeEnglish,
                         row.ChartOfAccountsEnglish


                     }
                         into groupTrailOnlyCurrency
                         select new TrailReportM()
              {
                  ChartOfAcountType = groupTrailOnlyCurrency.Key.ChartOfAcountType,
                  ChartOfAcount1 = groupTrailOnlyCurrency.Key.ChartOfAcount1,
                  ChartOfAcount2 = groupTrailOnlyCurrency.Key.ChartOfAcount2,
                  ChartOfAcount3 = groupTrailOnlyCurrency.Key.ChartOfAcount3,
                  ChartOfAcount4 = groupTrailOnlyCurrency.Key.ChartOfAcount4,
                  ChartOfAcount5 = groupTrailOnlyCurrency.Key.ChartOfAcount5,

                    ChartOfAcountCode1 = groupTrailOnlyCurrency.Key.ChartOfAcountCode1,
                    ChartOfAcountCode2 = groupTrailOnlyCurrency.Key.ChartOfAcountCode2,
                    ChartOfAcountCode3 = groupTrailOnlyCurrency.Key.ChartOfAcountCode3,
                    ChartOfAcountCode4 = groupTrailOnlyCurrency.Key.ChartOfAcountCode4,
                    ChartOfAcountCode5 = groupTrailOnlyCurrency.Key.ChartOfAcountCode5,

                    ChartOfAcountName1 = groupTrailOnlyCurrency.Key.ChartOfAcountName1,
                  ChartOfAcountName2 = groupTrailOnlyCurrency.Key.ChartOfAcountName2,
                  ChartOfAcountName3 = groupTrailOnlyCurrency.Key.ChartOfAcountName3,
                  ChartOfAcountName4 = groupTrailOnlyCurrency.Key.ChartOfAcountName4,
                 ChartOfAcountName5 = groupTrailOnlyCurrency.Key.ChartOfAcountName5,



                    ChartOfAccountId = groupTrailOnlyCurrency.Key.ChartOfAccountId,
                    GLAccountName = groupTrailOnlyCurrency.Key.GLAccountName,
                    GLAccountNumber=groupTrailOnlyCurrency.Key.GLAccountNumber,
                  GLAccountId = groupTrailOnlyCurrency.Key.GLAccountId,


							 ChartOfAcountName1English = groupTrailOnlyCurrency.Key.ChartOfAcountName1English,
                             ChartOfAcountName2English = groupTrailOnlyCurrency.Key.ChartOfAcountName2English,
                             ChartOfAcountName3English = groupTrailOnlyCurrency.Key.ChartOfAcountName3English,
                             ChartOfAcountName4English = groupTrailOnlyCurrency.Key.ChartOfAcountName4English,
                             ChartOfAcountName5English = groupTrailOnlyCurrency.Key.ChartOfAcountName5English,

                             GLAccountEnglish = groupTrailOnlyCurrency.Key.GLAccountEnglish,
                  ChartOfAccountsTypeEnglish = groupTrailOnlyCurrency.Key.ChartOfAccountsTypeEnglish,
                  ChartOfAccountsEnglish = groupTrailOnlyCurrency.Key.ChartOfAccountsEnglish,


                  CurrencyId = "",


                  LocalOpenBalance = groupTrailOnlyCurrency.Sum(x => x.LocalOpenBalance),
                  LocalDebit = groupTrailOnlyCurrency.Sum(x => x.LocalDebit),
                  LocalCredit = groupTrailOnlyCurrency.Sum(x => x.LocalCredit),
                  LocalCloseBalance = groupTrailOnlyCurrency.Sum(x => x.LocalCloseBalance),



                  ForeignOpenBalance = 0,
                  ForeignDebit = 0,
                  ForeignCredit = 0,
                  ForeignCloseBalance = 0,
              }
            );

            }



        }

       



    



        private IQueryable<ChartOfAccount5LevelM> JoinEachAccountWithisChartOfAccount5hierarchy(IQueryable<Data.EntityPOCOs.GLAccount> _QAllCardsAndDetialsAccType)
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
                 Level1English = chart.Level1English,

                 Level2Id = chart.Level2Id,
                 Level2Name = chart.Level2Name,
                 Level2English = chart.Level2English,

                 Level3Id = chart.Level3Name,
                 Level3Name = chart.Level3Name,
                 Level3English = chart.Level3English,

                 Level4Id = chart.Level4Name,
                 Level4Name = chart.Level4Name,
                 Level4English = chart.Level4English,

                 Level5Id = chart.Level5Name,
                 Level5Name = chart.Level5Name,
                 Level5English = chart.Level5English,


                 GLAccountId = aGL.Id,
                 GLAccountName = aGL.LocalName,
                 GLAccountNumber = aGL.DisplayNumber,
                 ChartOfAccountId=aGL.ChartOfAccountsId,
                 ChartOfAccountTypeCode = aGL.AccountTypeCode,

                 ChartOfAccountsTypeEnglish = chart.ChartOfAccountsTypeEnglish,
                 ChartOfAccountsEnglish = chart.ChartOfAccountsEnglish,
                 GLAccountEnglish = aGL.EnglishName,

                 LeafId = chart.LeafId,


             }
                 );
            return qAllCardsAndDetialsAccTypeBy5LevelHierarchy;
        }

        private string GetClientContolAcc(FullAccountingSettingPM fullAccountingSetting)
        {
            string controlAccountId = "";
            if (_TrailReportParam.DetailedControlClients)
            {
                if (!String.IsNullOrWhiteSpace(fullAccountingSetting.CustomerControlAccountId))
                {
                    controlAccountId = fullAccountingSetting.CustomerControlAccountId;
                }

            }
            return controlAccountId;
        }
        private string GetVendorContolAcc(FullAccountingSettingPM fullAccountingSetting)
        {
            string vendorControlAccountId = "";
            if (_TrailReportParam.DetailedControlVendors)
            {
                if (!String.IsNullOrWhiteSpace(fullAccountingSetting.VendorControlAccountId))
                {
                    vendorControlAccountId = fullAccountingSetting.VendorControlAccountId;
                }

            }
            return vendorControlAccountId;
        }

        protected List<string> GetJobContolAcc(FullAccountingSettingPM fullAccountingSetting)
        {
            List<string> controlAccountId_list = new List<string>();
            if (_TrailReportParam.DetailedControlJob)
            {
                if (!String.IsNullOrWhiteSpace(fullAccountingSetting.AirExportJobControlAccountId))
                {
                    controlAccountId_list.Add(fullAccountingSetting.AirExportJobControlAccountId);
                }
                if (!String.IsNullOrWhiteSpace(fullAccountingSetting.AirImportJobControlAccountId))
                {
                    controlAccountId_list.Add(fullAccountingSetting.AirImportJobControlAccountId);
                }
                if (!String.IsNullOrWhiteSpace(fullAccountingSetting.OceanExportJobControlAccountId))
                {
                    controlAccountId_list.Add(fullAccountingSetting.OceanExportJobControlAccountId);
                }

                if (!String.IsNullOrWhiteSpace(fullAccountingSetting.OceanImportJobControlAccountId))
                {
                    controlAccountId_list.Add(fullAccountingSetting.OceanImportJobControlAccountId);
                }

            }
            return controlAccountId_list;
        }
        private string GetFileContolAcc(FullAccountingSettingPM fullAccountingSetting)
        {
            string controlAccountId = "";
            if (_TrailReportParam.DetailedControlFile)
            {
                if (!String.IsNullOrWhiteSpace(fullAccountingSetting.FileControlAccountId))
                {
                    controlAccountId = fullAccountingSetting.FileControlAccountId;
                }

            }
            return controlAccountId;
        }


        private static IQueryable<TrailReportM> GetQAllTrailReportM(IQueryable<TrailReportTemp> allCurrSummary)
        {
            var allTrailReportM = (from r in allCurrSummary
                                   group r by new
                                   {
                                       AccountId = r.AccountId_COAType,
                                       r.CurrencyId
                                   } into g
                                   select new TrailReportM()
                                   {

                                       ChartOfAcountType = "",
                                       ChartOfAcount1 = "",
                                       ChartOfAcount2 = "",
                                       ChartOfAcount3 = "",
                                       ChartOfAcount4 = "",
                                       ChartOfAcount5 = "",

                                       GLAccountName = "",
                                       GLAccountNumber="",
                                       GLAccountId = g.Key.AccountId,
                                       CurrencyId = g.Key.CurrencyId,
                                       
                                       LocalOpenBalance =
                                       (
                                       +g.Sum(x => x.LocalAmountDebitTotalStart)
                                       - g.Sum(x => x.LocalAmountCreditTotalStart)
                                       + g.Sum(x => x.LocalAmountDebitTransStart)
                                       - g.Sum(x => x.LocalAmountCreditTransStart)
                                       ),

                                       LocalDebit =
                                       (
                                        +g.Sum(x => x.LocalAmountDebitTotalDelta2End)
                                        - g.Sum(x => x.LocalAmountDebitTransStart)
                                        + g.Sum(x => x.LocalAmountDebitTransEnd)
                                       ),
                                       LocalCredit =
                                       (
                                        +g.Sum(x => x.LocalAmountCreditTotalDelta2End)
                                        - g.Sum(x => x.LocalAmountCreditTransStart)
                                        + g.Sum(x => x.LocalAmountCreditTransEnd)
                                       ),
                                       LocalCloseBalance =
                                       (
                                       +g.Sum(x => x.LocalAmountDebitTotalDelta2End)
                                       - g.Sum(x => x.LocalAmountCreditTotalDelta2End)
                                       + g.Sum(x => x.LocalAmountDebitTransEnd)
                                       - g.Sum(x => x.LocalAmountCreditTransEnd)
                                       ),

                                       ForeignOpenBalance =
                                       (
                                       +g.Sum(x => x.ForeignAmountDebitTotalStart)
                                       - g.Sum(x => x.ForeignAmountCreditTotalStart)
                                       + g.Sum(x => x.ForeignAmountDebitTransStart)
                                       - g.Sum(x => x.ForeignAmountCreditTransStart)
                                       ),

                                       ForeignDebit =
                                       (
                                        +g.Sum(x => x.ForeignAmountDebitTotalDelta2End)
                                        - g.Sum(x => x.ForeignAmountDebitTransStart)
                                        + g.Sum(x => x.ForeignAmountDebitTransEnd)
                                       ),
                                       ForeignCredit =
                                       (
                                        +g.Sum(x => x.ForeignAmountCreditTotalDelta2End)
                                        - g.Sum(x => x.ForeignAmountCreditTransStart)
                                        + g.Sum(x => x.ForeignAmountCreditTransEnd)
                                       ),
                                       ForeignCloseBalance =
                                       (
                                       +g.Sum(x => x.ForeignAmountDebitTotalDelta2End)
                                       - g.Sum(x => x.ForeignAmountCreditTotalDelta2End)
                                       + g.Sum(x => x.ForeignAmountDebitTransEnd)
                                       - g.Sum(x => x.ForeignAmountCreditTransEnd)
                                       ),

                                   });
            return allTrailReportM;
        }

        private IQueryable<TrailReportTemp> GetQTransTo4CloseBalance(DateTime toBeginOfMonth, LedgerTransactionRepository repoLedgerTransaction)
        {
            var qTransTo4CloseBalance =
            repoLedgerTransaction.GetQLedgerTransactionGroupBETWEENinclusive(
                toBeginOfMonth,
                _TrailReportParam.ToDate, // INclude 
                _TrailReportParam.Tenant)
                .Select(r => new TrailReportTemp()
                {
                    AccountId_COAType = r.AccountId,
                    CurrencyId = r.CurrencyId,
                    ForeignAmountCreditTotalStart = 0,
                    ForeignAmountDebitTotalStart = 0,
                    LocalAmountCreditTotalStart = 0,
                    LocalAmountDebitTotalStart = 0,


                    ForeignAmountCreditTransStart = 0,
                    ForeignAmountDebitTransStart = 0,
                    LocalAmountCreditTransStart = 0,
                    LocalAmountDebitTransStart = 0,


                    ForeignAmountCreditTotalDelta2End = 0,
                    ForeignAmountDebitTotalDelta2End = 0,
                    LocalAmountCreditTotalDelta2End = 0,
                    LocalAmountDebitTotalDelta2End = 0,


                    ForeignAmountCreditTransEnd = r.ForeignAmountCredit,
                    ForeignAmountDebitTransEnd = r.ForeignAmountDebit,
                    LocalAmountCreditTransEnd = r.LocalAmountCredit,
                    LocalAmountDebitTransEnd = r.LocalAmountDebit,

                });
            return qTransTo4CloseBalance;
        }

        private IQueryable<TrailReportTemp> GetQTransFrom4OpenBalance(DateTime fromBeginOfMonth, LedgerTransactionRepository repoLedgerTransaction)
        {
            var qTransFrom4OpenBalance =
            repoLedgerTransaction.GetQLedgerTransactionGroupBETWEENinclusive(
                fromBeginOfMonth,
                _TrailReportParam.FromDate.AddDays(-1), //Not INclude
                _TrailReportParam.Tenant)
                 .Select(r => new TrailReportTemp()
                 {
                     AccountId_COAType = r.AccountId,
                     CurrencyId = r.CurrencyId,
                     
                     ForeignAmountCreditTotalStart = 0,
                     ForeignAmountDebitTotalStart = 0,
                     LocalAmountCreditTotalStart = 0,
                     LocalAmountDebitTotalStart = 0,


                     ForeignAmountCreditTransStart = r.ForeignAmountCredit,
                     ForeignAmountDebitTransStart = r.ForeignAmountDebit,
                     LocalAmountCreditTransStart = r.LocalAmountCredit,
                     LocalAmountDebitTransStart = r.LocalAmountDebit,


                     ForeignAmountCreditTotalDelta2End = 0,
                     ForeignAmountDebitTotalDelta2End = 0,
                     LocalAmountCreditTotalDelta2End = 0,
                     LocalAmountDebitTotalDelta2End = 0,


                     ForeignAmountCreditTransEnd = 0,
                     ForeignAmountDebitTransEnd = 0,
                     LocalAmountCreditTransEnd = 0,
                     LocalAmountDebitTransEnd = 0,

                 });
            return qTransFrom4OpenBalance;
        }

        private IQueryable<TrailReportTemp> GetQTotalsDeltaUntilCloseBalance(DateTime fromBeginOfMonth, DateTime toBeginOfMonth, GLAccountTotalByMonthRepository repoTotalByMonth)
        {
            var qTotalsDeltaUntilCloseBalance =
                repoTotalByMonth.GetQAllCurrencySumRange(
                fromBeginOfMonth.Year, fromBeginOfMonth.Month,
                toBeginOfMonth.Year, toBeginOfMonth.Month,
                _TrailReportParam.Tenant)
                .Select(r => new TrailReportTemp()
                {
                    AccountId_COAType = r.AccountId,
                    CurrencyId = r.CurrencyId,
                    ForeignAmountCreditTotalStart = 0,
                    ForeignAmountDebitTotalStart = 0,
                    LocalAmountCreditTotalStart = 0,
                    LocalAmountDebitTotalStart = 0,


                    ForeignAmountCreditTransStart = 0,
                    ForeignAmountDebitTransStart = 0,
                    LocalAmountCreditTransStart = 0,
                    LocalAmountDebitTransStart = 0,


                    ForeignAmountCreditTotalDelta2End = r.ForeignAmountCredit,
                    ForeignAmountDebitTotalDelta2End = r.ForeignAmountDebit,
                    LocalAmountCreditTotalDelta2End = r.LocalAmountCredit,
                    LocalAmountDebitTotalDelta2End = r.LocalAmountDebit,


                    ForeignAmountCreditTransEnd = 0,
                    ForeignAmountDebitTransEnd = 0,
                    LocalAmountCreditTransEnd = 0,
                    LocalAmountDebitTransEnd = 0,

                });
            return qTotalsDeltaUntilCloseBalance;
        }

        private IQueryable<TrailReportTemp> GetQTotalsOpenBalanceMonth(DateTime fromBeginOfMonth, GLAccountTotalByMonthRepository repoTotalByMonth)
        {
            var qTotalsOpenBalanceMonth = repoTotalByMonth.GetQAllCurrencySumUntillNotIncludeGByAccIdCurrId(fromBeginOfMonth.Year, fromBeginOfMonth.Month, _TrailReportParam.Tenant)
                .Select(r => new TrailReportTemp()
                {
                    AccountId_COAType = r.AccountId,
                    CurrencyId = r.CurrencyId,
                    ForeignAmountCreditTotalStart = r.ForeignAmountCredit,
                    ForeignAmountDebitTotalStart = r.ForeignAmountDebit,
                    LocalAmountCreditTotalStart = r.LocalAmountCredit,
                    LocalAmountDebitTotalStart = r.LocalAmountDebit,


                    ForeignAmountCreditTransStart = 0,
                    ForeignAmountDebitTransStart = 0,
                    LocalAmountCreditTransStart = 0,
                    LocalAmountDebitTransStart = 0,


                    ForeignAmountCreditTotalDelta2End = 0,
                    ForeignAmountDebitTotalDelta2End = 0,
                    LocalAmountCreditTotalDelta2End = 0,
                    LocalAmountDebitTotalDelta2End = 0,


                    ForeignAmountCreditTransEnd = 0,
                    ForeignAmountDebitTransEnd = 0,
                    LocalAmountCreditTransEnd = 0,
                    LocalAmountDebitTransEnd = 0,


                });
            return qTotalsOpenBalanceMonth;
        }

        public void Dispose()
        {
            if (_TransactionScope != null)
            {
                _TransactionScope.Dispose();
                _TransactionScope = null;
            }

        }
    }
    public class TrailReportTemp
    {


        public string AccountId_COAType { get; set; }
        public string CurrencyId { get; set; }

        public decimal? ForeignAmountCreditTotalStart { get; set; }
        public decimal? ForeignAmountDebitTotalStart { get; set; }
        public decimal? LocalAmountCreditTotalStart { get; set; }
        public decimal? LocalAmountDebitTotalStart { get; set; }


        public decimal? ForeignAmountCreditTransStart { get; set; }
        public decimal? ForeignAmountDebitTransStart { get; set; }
        public decimal? LocalAmountCreditTransStart { get; set; }
        public decimal? LocalAmountDebitTransStart { get; set; }//// תנעות מכולל תחילת החודש  של מתאריך עד למתאריך -לא כולל    


        public decimal? ForeignAmountCreditTotalDelta2End { get; set; }
        public decimal? ForeignAmountDebitTotalDelta2End { get; set; }
        public decimal? LocalAmountCreditTotalDelta2End { get; set; }
        public decimal? LocalAmountDebitTotalDelta2End { get; set; }//// מצטברים מחודש כולל ועד חודש לא כולל


        public decimal? ForeignAmountCreditTransEnd { get; set; }
        public decimal? ForeignAmountDebitTransEnd { get; set; }
        public decimal? LocalAmountCreditTransEnd { get; set; }
        public decimal? LocalAmountDebitTransEnd { get; set; }// תנעות מתחילת חודש אחרון כולל עד  תאריך הסיום + 1 לא כולל


        public bool? TotalDelta2End_AnyActivity { get; set; }
        public bool? TransEnd_AnyActivity { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("accid_COAT:" + this.AccountId_COAType);
            sb.Append("Cur:" + this.CurrencyId);




            sb.Append("TotalStart:");
            sb.Append("Local:");
            sb.Append("D" + this.LocalAmountDebitTotalStart.GetValueOrDefault().ToString());
            sb.Append("C" + this.LocalAmountCreditTotalStart.GetValueOrDefault().ToString());
            sb.Append("Foreign:");
            sb.Append("D" +this.ForeignAmountDebitTotalStart.GetValueOrDefault().ToString());
            sb.Append("C" + this.ForeignAmountCreditTotalStart.GetValueOrDefault().ToString());

            sb.Append("TransStart:");
            sb.Append("Local:");
            sb.Append("D" + this.LocalAmountDebitTransStart.GetValueOrDefault().ToString());
            sb.Append("C" + this.LocalAmountCreditTransStart.GetValueOrDefault().ToString());
            sb.Append("Foreign:");
            sb.Append("D" + this.ForeignAmountDebitTransStart.GetValueOrDefault().ToString());
            sb.Append("C" + this.ForeignAmountCreditTransStart.GetValueOrDefault().ToString());

            sb.Append("TotalDelta2End:");
            sb.Append("Local:");
            sb.Append("D" + this.LocalAmountDebitTotalDelta2End.GetValueOrDefault().ToString());
            sb.Append("C" + this.LocalAmountCreditTotalDelta2End.GetValueOrDefault().ToString());
            sb.Append("Foreign:");
            sb.Append("D" + this.ForeignAmountDebitTotalDelta2End.GetValueOrDefault().ToString());
            sb.Append("C" + this.ForeignAmountCreditTotalDelta2End.GetValueOrDefault().ToString());


            sb.Append("TransEnd:");
            sb.Append("Local:");
            sb.Append("D" + this.LocalAmountDebitTransEnd.GetValueOrDefault().ToString());
            sb.Append("C" + this.LocalAmountCreditTransEnd.GetValueOrDefault().ToString());
            sb.Append("Foreign:");
            sb.Append("D" + this.ForeignAmountDebitTransEnd.GetValueOrDefault().ToString());
            sb.Append("C" + this.ForeignAmountCreditTransEnd.GetValueOrDefault().ToString());

            return sb.ToString();
        }

    }
    public class TrailReportM
    {

        public string ChartOfAccountTypeName { get; set; }
        public string ChartOfAcountType { get; set; }
        public int? ChartOfAcountTypeOrder { get; set; }
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
        public string ChartOfAccountsTypeEnglish { get; set; }
        public string ChartOfAccountsEnglish { get; set; }

        public string GLAccountName { get; set; }
        public string GLAccountEnglish { get; set; }
        public string GLAccountNumber { get; set; }

        public string GLAccountId { get; set; }
        public string CurrencyId { get; set; }
        public string ChartOfAccountId { get; set; }

        public decimal? LocalOpenBalance { get; set; }
        public decimal? LocalDebit { get; set; }
        public decimal? LocalCredit { get; set; }
        public decimal? LocalCloseBalance { get; set; }



        public decimal? ForeignOpenBalance { get; set; }

        public decimal? ForeignDebit { get; set; }

        public decimal? ForeignCredit { get; set; }

        public decimal? ForeignCloseBalance { get; set; }


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


            sb.Append("Cur:" + this.CurrencyId);




            sb.Append("Local:");
            sb.Append("OB:").Append(this.LocalOpenBalance.GetValueOrDefault().ToString());

            sb.Append("D:").Append(this.LocalDebit.GetValueOrDefault().ToString());
            sb.Append("C:").Append(this.LocalCredit.GetValueOrDefault().ToString());

            sb.Append("CB:").Append(this.LocalCloseBalance.GetValueOrDefault().ToString());



            sb.Append("Foreign:");
            sb.Append("OB:").Append(this.ForeignOpenBalance.GetValueOrDefault().ToString());

            sb.Append("D:").Append(this.ForeignDebit.GetValueOrDefault().ToString());
            sb.Append("C:").Append(this.ForeignCredit.GetValueOrDefault().ToString());

            sb.Append("CB:").Append(this.ForeignCloseBalance.GetValueOrDefault().ToString());


            return sb.ToString();
        }
    }
    public class TrailReportM2
    {

        public string ChartOfAccountTypeName { get; set; }
        public string ChartOfAcountType { get; set; }
        public int? ChartOfAcountTypeOrder { get; set; }
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
        public string ChartOfAccountsTypeEnglish { get; set; }
        public string ChartOfAccountsEnglish { get; set; }

        public string GLAccountName { get; set; }
        public string GLAccountEnglish { get; set; }
        public string GLAccountNumber { get; set; }

        public string GLAccountId { get; set; }
        public string CurrencyId { get; set; }
        public string ChartOfAccountId { get; set; }

        public decimal? LocalOpenBalance { get; set; }
        public decimal? LocalDebit { get; set; }
        public decimal? LocalCredit { get; set; }
        public decimal? LocalCloseBalance { get; set; }



        public decimal? ForeignOpenBalance { get; set; }

        public decimal? ForeignDebit { get; set; }

        public decimal? ForeignCredit { get; set; }

        public decimal? ForeignCloseBalance { get; set; }


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


            sb.Append("Cur:" + this.CurrencyId);




            sb.Append("Local:");
            sb.Append("OB:").Append(this.LocalOpenBalance.GetValueOrDefault().ToString());

            sb.Append("D:").Append(this.LocalDebit.GetValueOrDefault().ToString());
            sb.Append("C:").Append(this.LocalCredit.GetValueOrDefault().ToString());

            sb.Append("CB:").Append(this.LocalCloseBalance.GetValueOrDefault().ToString());



            sb.Append("Foreign:");
            sb.Append("OB:").Append(this.ForeignOpenBalance.GetValueOrDefault().ToString());

            sb.Append("D:").Append(this.ForeignDebit.GetValueOrDefault().ToString());
            sb.Append("C:").Append(this.ForeignCredit.GetValueOrDefault().ToString());

            sb.Append("CB:").Append(this.ForeignCloseBalance.GetValueOrDefault().ToString());


            return sb.ToString();
        }
    }
    public class TrailReportParam
    {
        //filter for alll!!!
        public int Tenant { get; set; }

        //filter for Money!!!
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        //filter the GLAccount ?!?!?
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string Category3 { get; set; }
        public string Category4 { get; set; }
        public string Category5 { get; set; }

        //filter the GLAccount ?!?!?
        public bool DetailedControlClients { get; set; }
        public bool DetailedControlVendors { get; set; }
        public bool DetailedControlJob { get; set; }
        public bool DetailedControlFile { get; set; }
        public bool Skip { get; set; }
        //filter the GLAccount (if COATypeLevel) + Group by 
        public ReportLevel MyTrailReportLevel { get; set; }

        //Group by 
        public bool CurrenciesDetailed { get; set; }

        /// <summary>
        /// לאחר שיחה הבהרה עם אייל 
        ///דוח מאזן בוחן
        ///1ביררת המחדל -לא יצגו כרטיסים לא פעילים .
        ///2לפי חיתוך\בחירה "הצג כרטיסים לא פעילים" – יצגו !
        //*** כרטיסים לא פעילים (יתרת פתיחה לתקופה 0 וללא תנועות  לטווח הדוח )
        /// </summary>
        public bool Suppress_DoNotShowCardWithoutActivity { get; set; }
        public bool Suppress_ControlAccount { get; set; }

        public bool IsRevenueExpenseReport { get; set; }



        #region Task 60992: Trial Balance- New filters + multiple choice
        /// <summary>
        /// new filter Chart Of Account type with multiple choice option.
        /// </summary>
        List<string> _GLAccountLevel_ChartOfAccountsTypeCodeList;
        public List<string> ChartOfAccountsTypeCodeList {
            get { 
                _GLAccountLevel_ChartOfAccountsTypeCodeList = _GLAccountLevel_ChartOfAccountsTypeCodeList ?? new List<string>();
                return _GLAccountLevel_ChartOfAccountsTypeCodeList;
            }
            set
            {
                _GLAccountLevel_ChartOfAccountsTypeCodeList = value;
            }
        }
        /// <summary>
        /// change the filter Chart Of Account to  multiple choice option. 
        /// </summary>
        List<string> _GLAccountLevel_ChartOfAccountsIdList;
        public List<string> ChartOfAccountsIdList 
        {
            get {
                _GLAccountLevel_ChartOfAccountsIdList = _GLAccountLevel_ChartOfAccountsIdList ?? new List<string>();
                return _GLAccountLevel_ChartOfAccountsIdList;
            }
            set { _GLAccountLevel_ChartOfAccountsIdList = value; }
        }

        public bool DoNotShowCardWithLocalCloseBalanceEqualZero { get;  set; }
        #endregion

    }


    public enum ReportLevel
    {
        ChartofaccountType=1,
        Chartofaccount=2,
        GLAccount=3
    }
}
