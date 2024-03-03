
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
using System.Data.Entity.Core.Objects;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    class TrailReportChartofaccount : TrailReportBase
    {

        
        
        public TrailReportChartofaccount(TrailReportParam trailReportParam,int timeOutInMin)
            : base(trailReportParam, timeOutInMin){}

        protected override void AdjustTrailReportFull()
        {
            var testNow = false;// (new DateTime(2016, 12, 30) > DateTime.Now);

            Disclaimer_InTrailReportByChartofaccountId_UsingCardsOnly_In_QBaseAllCardsAndDetailsAccType();
            Disclaimer_InTrailReportByChartofaccountId_Detailed_NotAllowed_Soo_UsingControlAccounts();



            var checkChartOfAcountType1LocalDebit1263p90 = false;
            if (checkChartOfAcountType1LocalDebit1263p90)
            {
                //Type =1263.90
                //Chartofaccount==1111.50+76.15	
                QBaseAllCardsAndDetailsAccType = QBaseAllCardsAndDetailsAccType.Where(coa => coa.ChartOfAccountsTypeCode == "1");
            }
            IQueryable<TrailReportTemp> qLocalAmountOnly_TotalStart_GroupByCOATypeId = Init_LocalAmountOnly_TotalStart_JoinAccounts_GroupByCOATypeId();

            IQueryable<TrailReportTemp> qLocalAmountOnly_TotalDelta2End_GroupByCOATypeId = Init_LocalAmountOnly_TotalDelta2End_JoinAccounts_GroupByCOATypeId();

            IQueryable<TrailReportTemp> qLocalAmountsOfAccountTransStart_JoinAccountsWhereNotControlAccount_GroupByCOATypeId =
                Init_LocalAmountsOfAccountTransStart_JoinAccountsNotControlAccount_GroupByCOATypeId();


            IQueryable<TrailReportTemp> qLocalAmountsOfAccountTransStart_GroupByCOATypeId_All = null;

            if (true || !base.NotUsingControlAccount()) // in the level we take only cards==1 
            {

                IQueryable<TrailReportTemp> qLocalAmountsOfAccountTransStart_JoinAccountsWhereIscontrolAccount_GroupByCOATypeId =
Init_LocalAmountsOfAccountTransStart_JoinAccountsWhereIscontrolAccount_GroupByCOATypeId();

                qLocalAmountsOfAccountTransStart_GroupByCOATypeId_All =
                    qLocalAmountsOfAccountTransStart_JoinAccountsWhereNotControlAccount_GroupByCOATypeId.Union(
                qLocalAmountsOfAccountTransStart_JoinAccountsWhereIscontrolAccount_GroupByCOATypeId);
            }


            if (checkChartOfAcountType1LocalDebit1263p90)
            {
                var l = QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == false);
                var vat = l.First(r => r.Id == "1-58");
            }

            IQueryable<TrailReportTemp> qLocalAmountsOfAccount_TransEnd_JoinAccountsNotControlAccount_GroupByCOATypeId =
                Init_LocalAmountsOfAccount_TransEnd_JoinAccountsNotControlAccount_GroupByCOATypeId();
            IQueryable<TrailReportTemp> qLocalAmountsOfAccountTransEnd_GroupByCOATypeId_All = null;
            if (true || !base.NotUsingControlAccount()) // in the level we take only cards==1 
            {
                IQueryable<TrailReportTemp> qLocalAmountsOfAccountTransEnd__JoinAccountsWhereIsControlAccount_GroupByCOATypeId =
                    Init_LocalAmountsOfAccountTransEnd__JoinAccountsWhereIsControlAccount_GroupByCOATypeId();
                qLocalAmountsOfAccountTransEnd_GroupByCOATypeId_All =
                    qLocalAmountsOfAccount_TransEnd_JoinAccountsNotControlAccount_GroupByCOATypeId.Union(
                    qLocalAmountsOfAccountTransEnd__JoinAccountsWhereIsControlAccount_GroupByCOATypeId);
            }

            //var sqlqAccumulateTranactionBeginOfMonthToDateTillToDateInculde = ((ObjectQuery)qAccumulateTranactionBeginOfMonthToDateTillToDateInculde).ToTraceString();
            if (testNow)
            {
                qLocalAmountsOfAccount_TransEnd_JoinAccountsNotControlAccount_GroupByCOATypeId.ToList();
            }


            IQueryable<TrailReportTemp> _QUnionAllMoneyData = qLocalAmountOnly_TotalStart_GroupByCOATypeId.Union(qLocalAmountsOfAccountTransStart_GroupByCOATypeId_All).Union(qLocalAmountOnly_TotalDelta2End_GroupByCOATypeId).Union(qLocalAmountsOfAccountTransEnd_GroupByCOATypeId_All);

            bool addAllChatOfAccountTyps = true;
            if (addAllChatOfAccountTyps)
            {
                _QUnionAllMoneyData = AddAllChatOfAccountTypEmptyRows(_QUnionAllMoneyData);
            }

            if (testNow)
            {
                var test1111 = _QUnionAllMoneyData.ToList();
            }
            ///var sql_QUnionAllMoneyData = ((ObjectQuery)_QUnionAllMoneyData).ToTraceString();

            IQueryable<TrailReportM> qMapAllCurrencySum2TRail = GroupByCOATypeIDSumLocalAmount(_QUnionAllMoneyData);

            Join5Hierarch2DataGroupSum(qMapAllCurrencySum2TRail);

            if (testNow)
            {
                var tettt = _QBaseTrailReportFull.ToList();
            }



        }

        private void Join5Hierarch2DataGroupSum(IQueryable<TrailReportM> qMapAllCurrencySum2TRail)
        {
            _QBaseTrailReportFull =
                            (from trailReportRow in
                                 (from chartf in QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy
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

                                      ChartOfAcountName1 = chartf.Level1Name,
                                      ChartOfAcountName2 = chartf.Level2Name,
                                      ChartOfAcountName3 = chartf.Level3Name,
                                      ChartOfAcountName4 = chartf.Level4Name,
                                      ChartOfAcountName5 = chartf.Level5Name,

                                      ChartOfAcountName1English = chartf.Level1English,
                                      ChartOfAcountName2English = chartf.Level2English,
                                      ChartOfAcountName3English = chartf.Level3English,
                                      ChartOfAcountName4English = chartf.Level4English,
                                      ChartOfAcountName5English = chartf.Level5English,

                                      ChartOfAcountCode1 = chartf.Level1Code,
                                      ChartOfAcountCode2 = chartf.Level2Code,
                                      ChartOfAcountCode3 = chartf.Level3Code,
                                      ChartOfAcountCode4 = chartf.Level4Code,
                                      ChartOfAcountCode5 = chartf.Level5Code,



                                      GLAccountName = chartf.GLAccountName,
                                      GLAccountEnglish = chartf.GLAccountEnglish,
                                      ChartOfAccountsTypeEnglish = chartf.ChartOfAccountsTypeEnglish,
                                      ChartOfAccountsEnglish = chartf.ChartOfAccountsEnglish,

                                      GLAccountId = chartf.GLAccountId,

                                      CurrencyId = groupJoinData.CurrencyId,


                                      LocalOpenBalance = groupJoinData.LocalOpenBalance,
                                      LocalDebit = groupJoinData.LocalDebit,
                                      LocalCredit = groupJoinData.LocalCredit,
                                      LocalCloseBalance = groupJoinData.LocalCloseBalance,



                                      ForeignOpenBalance = groupJoinData.ForeignOpenBalance,

                                      ForeignDebit = groupJoinData.ForeignDebit,

                                      ForeignCredit = groupJoinData.ForeignCredit,

                                      ForeignCloseBalance = groupJoinData.ForeignCloseBalance,
                                  }
                                  )
                             group trailReportRow by
                         new
                         {
                             trailReportRow.ChartOfAcountType,

                             trailReportRow.ChartOfAcount1,
                             trailReportRow.ChartOfAcount2,
                             trailReportRow.ChartOfAcount3,
                             trailReportRow.ChartOfAcount4,
                             trailReportRow.ChartOfAcount5,

                             trailReportRow.ChartOfAcountName1,
                             trailReportRow.ChartOfAcountName2,
                             trailReportRow.ChartOfAcountName3,
                             trailReportRow.ChartOfAcountName4,
                             trailReportRow.ChartOfAcountName5,

                             trailReportRow.ChartOfAcountName1English,
                             trailReportRow.ChartOfAcountName2English,
                             trailReportRow.ChartOfAcountName3English,
                             trailReportRow.ChartOfAcountName4English,
                             trailReportRow.ChartOfAcountName5English,

                             trailReportRow.ChartOfAcountCode1,
                             trailReportRow.ChartOfAcountCode2,
                             trailReportRow.ChartOfAcountCode3,
                             trailReportRow.ChartOfAcountCode4,
                             trailReportRow.ChartOfAcountCode5,



                 //trailReportRow.GLAccountName,

                 //trailReportRow.GLAccountId,

                 //trailReportRow.CurrencyId,

             }
                                 into gCOA

                             select new TrailReportM()
                             {
                                 ChartOfAcountType = gCOA.Key.ChartOfAcountType,
                                 ChartOfAcount1 = gCOA.Key.ChartOfAcount1,
                                 ChartOfAcount2 = gCOA.Key.ChartOfAcount2,
                                 ChartOfAcount3 = gCOA.Key.ChartOfAcount3,
                                 ChartOfAcount4 = gCOA.Key.ChartOfAcount4,
                                 ChartOfAcount5 = gCOA.Key.ChartOfAcount5,

                                 ChartOfAcountName1 = gCOA.Key.ChartOfAcountName1,
                                 ChartOfAcountName2 = gCOA.Key.ChartOfAcountName2,
                                 ChartOfAcountName3 = gCOA.Key.ChartOfAcountName3,
                                 ChartOfAcountName4 = gCOA.Key.ChartOfAcountName4,
                                 ChartOfAcountName5 = gCOA.Key.ChartOfAcountName5,

                                 ChartOfAcountName1English = gCOA.Key.ChartOfAcountName1English,
                                 ChartOfAcountName2English = gCOA.Key.ChartOfAcountName2English,
                                 ChartOfAcountName3English = gCOA.Key.ChartOfAcountName3English,
                                 ChartOfAcountName4English = gCOA.Key.ChartOfAcountName4English,
                                 ChartOfAcountName5English = gCOA.Key.ChartOfAcountName5English,

                                 ChartOfAcountCode1 = gCOA.Key.ChartOfAcountCode1,
                                 ChartOfAcountCode2 = gCOA.Key.ChartOfAcountCode2,
                                 ChartOfAcountCode3 = gCOA.Key.ChartOfAcountCode3,
                                 ChartOfAcountCode4 = gCOA.Key.ChartOfAcountCode4,
                                 ChartOfAcountCode5 = gCOA.Key.ChartOfAcountCode5,


                                 GLAccountName = "",//gCOA.Key.GLAccountName,

                     GLAccountId = "",//gCOA.Key.GLAccountId,

                     CurrencyId = "",


                                 LocalOpenBalance = gCOA.Sum(x => x.LocalOpenBalance),
                                 LocalDebit = gCOA.Sum(x => x.LocalDebit),
                                 LocalCredit = gCOA.Sum(x => x.LocalCredit),
                                 LocalCloseBalance = gCOA.Sum(x => x.LocalCloseBalance),



                                 ForeignOpenBalance = null,
                                 ForeignDebit = null,
                                 ForeignCredit = null,
                                 ForeignCloseBalance = null,
                             }

                        );
        }

        private static IQueryable<TrailReportM> GroupByCOATypeIDSumLocalAmount(IQueryable<TrailReportTemp> _QUnionAllMoneyData)
        {
            IQueryable<TrailReportM> qMapAllCurrencySum2TRail =
                            //map All to TrailReportM Schem 
                            //GetQMap2TrailReportM(_QUnionAllMoneyData);
                            (from r in _QUnionAllMoneyData
                             group r by new
                             {
                                 COAType = r.AccountId_COAType,
                     //r.CurrencyId
                 } into g
                             select new TrailReportM()
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

                                 ChartOfAcountName1English = "",
                                 ChartOfAcountName2English = "",
                                 ChartOfAcountName3English = "",
                                 ChartOfAcountName4English = "",
                                 ChartOfAcountName5English = "",

                                 ChartOfAcountCode1 = "",
                                 ChartOfAcountCode2 = "",
                                 ChartOfAcountCode3 = "",
                                 ChartOfAcountCode4 = "",
                                 ChartOfAcountCode5 = "",


                                 GLAccountName = "",

                                 GLAccountId = g.Key.COAType,
                                 CurrencyId = "",//g.Key.CurrencyId,

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
                                  + g.Sum(x => x.LocalAmountDebitTransEnd)
                                  - g.Sum(x => x.LocalAmountDebitTransStart)
                                 ),
                                 LocalCredit =
                                 (
                                  +g.Sum(x => x.LocalAmountCreditTotalDelta2End)
                                  + g.Sum(x => x.LocalAmountCreditTransEnd)
                                  - g.Sum(x => x.LocalAmountCreditTransStart)
                                 ),
                                 LocalCloseBalance =
                                 (
                                +g.Sum(x => x.LocalAmountDebitTotalStart)
                                 + g.Sum(x => x.LocalAmountDebitTotalDelta2End)
                                 + g.Sum(x => x.LocalAmountDebitTransEnd)
                                 - g.Sum(x => x.LocalAmountCreditTotalStart)
                                 - g.Sum(x => x.LocalAmountCreditTotalDelta2End)
                                 - g.Sum(x => x.LocalAmountCreditTransEnd)
                                 ),

                                 ForeignOpenBalance = 0,
                                 ForeignDebit = 0,
                                 ForeignCredit = 0,
                                 ForeignCloseBalance = 0,

                             });
            return qMapAllCurrencySum2TRail;
        }

        private IQueryable<TrailReportTemp> 
            Init_LocalAmountsOfAccountTransEnd__JoinAccountsWhereIsControlAccount_GroupByCOATypeId()
        {
            var qAccumulateTranactionBeginOfMonthToDateTillToDateInculdeControl =
        //Accumulate Tranaction BeginOfMonth(ToDate) till ToDate
        (from totalCOAType in


             (
          from trans in QBasePeriodTransaction_TransEnd_BeginOfMonthToDate_Till_ToDateInculde
          join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == true)
          on trans.ControlAccountId equals a.Id
          select new { a.Id, trans.CurrencyId, trans.ForeignAmountCredit, trans.ForeignAmountDebit, trans.LocalAmountCredit, trans.LocalAmountDebit }
             )
         group totalCOAType by new
         {
             totalCOAType.Id,
                 //totalCOAType.CurrencyId
             } into g
         select new TrailReportTemp
         {

             AccountId_COAType = g.Key.Id,
             CurrencyId = "",//g.Key.CurrencyId,

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


             ForeignAmountCreditTransEnd = 0,// g.Sum(x => x.ForeignAmountCredit),
                 ForeignAmountDebitTransEnd = 0,//g.Sum(x => x.ForeignAmountDebit),
                 LocalAmountCreditTransEnd = g.Sum(x => x.LocalAmountCredit),
             LocalAmountDebitTransEnd = g.Sum(x => x.LocalAmountDebit),


         }
         );
            return qAccumulateTranactionBeginOfMonthToDateTillToDateInculdeControl;
        }

        private void Disclaimer_InTrailReportByChartofaccountId_Detailed_NotAllowed_Soo_UsingControlAccounts()
        {
            
        }

        private void Disclaimer_InTrailReportByChartofaccountId_UsingCardsOnly_In_QBaseAllCardsAndDetailsAccType()
        {
            
        }

        private IQueryable<TrailReportTemp> Init_LocalAmountsOfAccount_TransEnd_JoinAccountsNotControlAccount_GroupByCOATypeId()
        {
            var qAccumulateTranactionBeginOfMonthToDateTillToDateInculde =
            //Accumulate Tranaction BeginOfMonth(ToDate) till ToDate
            (from totalCOAType in

                (
             from trans in QBasePeriodTransaction_TransEnd_BeginOfMonthToDate_Till_ToDateInculde
             join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == false)
             on trans.AccountId equals a.Id
             select new { a.Id, trans.CurrencyId, trans.ForeignAmountCredit, trans.ForeignAmountDebit, trans.LocalAmountCredit, trans.LocalAmountDebit }
                )
             group totalCOAType by new
             {
                 totalCOAType.Id,
                 //totalCOAType.CurrencyId
             } into g
             select new TrailReportTemp
             {

                 AccountId_COAType = g.Key.Id,
                 CurrencyId = "",//g.Key.CurrencyId,

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


                 ForeignAmountCreditTransEnd = 0,// g.Sum(x => x.ForeignAmountCredit),
                 ForeignAmountDebitTransEnd = 0,//g.Sum(x => x.ForeignAmountDebit),
                 LocalAmountCreditTransEnd = g.Sum(x => x.LocalAmountCredit),
                 LocalAmountDebitTransEnd = g.Sum(x => x.LocalAmountDebit),


             }
             );
            return qAccumulateTranactionBeginOfMonthToDateTillToDateInculde;
        }

        private IQueryable<TrailReportTemp> Init_LocalAmountsOfAccountTransStart_JoinAccountsWhereIscontrolAccount_GroupByCOATypeId()
        {
            var qAccumulateTranactionBeginOfMonthFromTillFromDateNotIncludeControl =
                                  //Accumulate Tranaction BeginOfMonth(fromDate) till (FromDate-1d)
                                  (from totalCOAType in

                                       (
                                   from trans in QBasePeriodTransaction_TransStart_BeginOfMonthFromDate_TillFromDate_NotInclude
                                   join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == true)
                                   on trans.ControlAccountId equals a.Id
                                   select new { a.Id, trans.CurrencyId, trans.ForeignAmountCredit, trans.ForeignAmountDebit, trans.LocalAmountCredit, trans.LocalAmountDebit }
                                       )

                                   group totalCOAType by new
                                   {
                                       totalCOAType.Id,
                           //totalCOAType.CurrencyId
                       } into g
                                   select new TrailReportTemp
                                   {

                                       AccountId_COAType = g.Key.Id,
                                       CurrencyId = "",///g.Key.CurrencyId,

                           ForeignAmountCreditTotalStart = 0,
                                       ForeignAmountDebitTotalStart = 0,
                                       LocalAmountCreditTotalStart = 0,
                                       LocalAmountDebitTotalStart = 0,



                                       ForeignAmountCreditTransStart = 0,// g.Sum(x => x.ForeignAmountCredit),
                           ForeignAmountDebitTransStart = 0,//g.Sum(x => x.ForeignAmountDebit),
                           LocalAmountCreditTransStart = g.Sum(x => x.LocalAmountCredit),
                                       LocalAmountDebitTransStart = g.Sum(x => x.LocalAmountDebit),


                                       ForeignAmountCreditTotalDelta2End = 0,
                                       ForeignAmountDebitTotalDelta2End = 0,
                                       LocalAmountCreditTotalDelta2End = 0,
                                       LocalAmountDebitTotalDelta2End = 0,


                                       ForeignAmountCreditTransEnd = 0,
                                       ForeignAmountDebitTransEnd = 0,
                                       LocalAmountCreditTransEnd = 0,
                                       LocalAmountDebitTransEnd = 0,


                                   });
            return qAccumulateTranactionBeginOfMonthFromTillFromDateNotIncludeControl;
        }

        private IQueryable<TrailReportTemp> Init_LocalAmountsOfAccountTransStart_JoinAccountsNotControlAccount_GroupByCOATypeId()
        {
            var qAccumulateTranactionBeginOfMonthFromTillFromDateNotInclude =
                  //Accumulate Tranaction BeginOfMonth(fromDate) till (FromDate-1d)
                  (from totalCOAType in
                       (
                   from trans in QBasePeriodTransaction_TransStart_BeginOfMonthFromDate_TillFromDate_NotInclude
                   join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == false)
                   on trans.AccountId equals a.Id
                   select new { a.Id, trans.CurrencyId, trans.ForeignAmountCredit, trans.ForeignAmountDebit, trans.LocalAmountCredit, trans.LocalAmountDebit }
                       )

                   group totalCOAType by new
                   {
                       totalCOAType.Id,
                       //totalCOAType.CurrencyId
                   } into g
                   select new TrailReportTemp
                   {

                       AccountId_COAType = g.Key.Id,
                       CurrencyId = "",///g.Key.CurrencyId,

                       ForeignAmountCreditTotalStart = 0,
                       ForeignAmountDebitTotalStart = 0,
                       LocalAmountCreditTotalStart = 0,
                       LocalAmountDebitTotalStart = 0,



                       ForeignAmountCreditTransStart = 0,// g.Sum(x => x.ForeignAmountCredit),
                       ForeignAmountDebitTransStart = 0,//g.Sum(x => x.ForeignAmountDebit),
                       LocalAmountCreditTransStart = g.Sum(x => x.LocalAmountCredit),
                       LocalAmountDebitTransStart = g.Sum(x => x.LocalAmountDebit),


                       ForeignAmountCreditTotalDelta2End = 0,
                       ForeignAmountDebitTotalDelta2End = 0,
                       LocalAmountCreditTotalDelta2End = 0,
                       LocalAmountDebitTotalDelta2End = 0,


                       ForeignAmountCreditTransEnd = 0,
                       ForeignAmountDebitTransEnd = 0,
                       LocalAmountCreditTransEnd = 0,
                       LocalAmountDebitTransEnd = 0,


                   });
            return qAccumulateTranactionBeginOfMonthFromTillFromDateNotInclude;
        }

        private IQueryable<TrailReportTemp> Init_LocalAmountOnly_TotalDelta2End_JoinAccounts_GroupByCOATypeId()
        {
            var qAccumulateTotalsFromStartOfMonthFromTilStartOfMonthTo = //Accumulate Totals From StartOfMonth(FromDate) Until StartOfMonth(ToDate) 
                 (from totalCOAType in
                      (
                          from tot in QBasePeriodGLATotalByMonths_TotalDelta2End_FromBeginOfMonthFromDate_Til_BeginOfMonthToDate
                          join a in QBaseAllCardsAndDetailsAccType
                          on tot.AccountId equals a.Id
                          select new { a.Id, tot.CurrencyId, tot.ForeignAmountCredit, tot.ForeignAmountDebit, tot.LocalAmountCredit, tot.LocalAmountDebit }
                          )
                  group totalCOAType by new
                  {
                      totalCOAType.Id,
                      ///totalCOAType.CurrencyId
                  } into g
                  select new TrailReportTemp
                  {

                      AccountId_COAType = g.Key.Id,
                      CurrencyId = "",//g.Key.CurrencyId,

                      ForeignAmountCreditTotalStart = 0,
                      ForeignAmountDebitTotalStart = 0,
                      LocalAmountCreditTotalStart = 0,
                      LocalAmountDebitTotalStart = 0,



                      ForeignAmountCreditTransStart = 0,
                      ForeignAmountDebitTransStart = 0,
                      LocalAmountCreditTransStart = 0,
                      LocalAmountDebitTransStart = 0,


                      ForeignAmountCreditTotalDelta2End = 0,// g.Sum(x => x.ForeignAmountCredit),
                      ForeignAmountDebitTotalDelta2End = 0,//g.Sum(x => x.ForeignAmountDebit),
                      LocalAmountCreditTotalDelta2End = g.Sum(x => x.LocalAmountCredit),
                      LocalAmountDebitTotalDelta2End = g.Sum(x => x.LocalAmountDebit),


                      ForeignAmountCreditTransEnd = 0,
                      ForeignAmountDebitTransEnd = 0,
                      LocalAmountCreditTransEnd = 0,
                      LocalAmountDebitTransEnd = 0,


                  });
            return qAccumulateTotalsFromStartOfMonthFromTilStartOfMonthTo;
        }

        private IQueryable<TrailReportTemp> Init_LocalAmountOnly_TotalStart_JoinAccounts_GroupByCOATypeId()
        {
            var qAccumulateTotalsFrom0BCTilNotIncludeBeginOfMonthFromDate =
            //Accumulate Totals From the beginning of  Account Use Until(NotInclude) (StartOfMonth)FromDate                
            (from totalCOAType in

                 (from tot in QBasePeriodGLATotalByMonths_TotalStart_From0BC_TilNotInclude_BeginOfMonth_FromDate
                  join a in QBaseAllCardsAndDetailsAccType
                  on tot.AccountId equals a.Id
                  select new { a.Id, tot.CurrencyId, tot.ForeignAmountCredit, tot.ForeignAmountDebit, tot.LocalAmountCredit, tot.LocalAmountDebit }
                  )
             group totalCOAType by new
             {
                 totalCOAType.Id,
                 //totalCOAType.CurrencyId
             } into g
             select new TrailReportTemp
             {

                 AccountId_COAType = g.Key.Id,
                 CurrencyId = "",
                 ForeignAmountCreditTotalStart = 0,// g.Sum(x => x.ForeignAmountCredit),
                 ForeignAmountDebitTotalStart = 0,//g.Sum(x => x.ForeignAmountDebit),
                 LocalAmountCreditTotalStart = g.Sum(x => x.LocalAmountCredit),
                 LocalAmountDebitTotalStart = g.Sum(x => x.LocalAmountDebit),


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
            return qAccumulateTotalsFrom0BCTilNotIncludeBeginOfMonthFromDate;
        }



        IQueryable<TrailReportTemp> AddAllChatOfAccountTypEmptyRows(IQueryable<TrailReportTemp> _QUnionAllMoneyData)
        {
            _QUnionAllMoneyData = _QUnionAllMoneyData.Concat(
            _AccountingContext.ChartOfAccountsTypes.Select(r => new TrailReportTemp()
            {
                AccountId_COAType = r.Code,

                CurrencyId = "",

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


                ForeignAmountCreditTransEnd = 0,
                ForeignAmountDebitTransEnd = 0,
                LocalAmountCreditTransEnd = 0,
                LocalAmountDebitTransEnd = 0,
            }));
            return _QUnionAllMoneyData;
        }


    }
}

