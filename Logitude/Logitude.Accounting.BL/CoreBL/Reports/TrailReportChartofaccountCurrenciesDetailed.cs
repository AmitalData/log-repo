
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

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    class TrailReportChartofaccountCurrenciesDetailed : TrailReportBase
    {


        public TrailReportChartofaccountCurrenciesDetailed(TrailReportParam trailReportParam, int timeOutInMin)
            : base(trailReportParam, timeOutInMin)
        {
        
            
        }

        protected override void AdjustTrailReportFull()
        {
            var testNow = false;// (new DateTime(2016, 12, 30) > DateTime.Now);


            Disclaimer_InTrailReportByChartofaccountId_UsingCardsOnly_In_QBaseAllCardsAndDetailsAccType();
            Disclaimer_InTrailReportByChartofaccountId_Detailed_NotAllowed_Soo_UsingControlAccounts();

            IQueryable<TrailReportTemp> qAccumulateTotals_TotalStart_JoinAccounts_GroupByCOATypeId =
                Init_TotalStart_JoinAccounts_GroupByCOATypeId();

            IQueryable<TrailReportTemp> qAccumulateTotals_TotalDelta2End_JoinAccounts_GroupByCOATypeId =
                Init_TotalDelta2End_JoinAccounts_GroupByCOATypeId();

            IQueryable<TrailReportTemp> qTransStart_JoinAccountsNotControlAccount_GroupByCOATypeId =
                Init_LocalAmountTransStart_JoinAccountsNotControlAccount_GroupByCOATypeId();

            IQueryable<TrailReportTemp> qTransStart_JoinAccountsNotControlAccount_GroupByCOATypeId_All = null;
            if (true)
            {
//                IQueryable<TrailReportTemp> qTransStart_JoinAccountsWhereIscontrolAccount_GroupByCOATypeId = Init_TransStart_JoinAccountsWhereIscontrolAccount_GroupByCOATypeId();

//                qTransStart_JoinAccountsNotControlAccount_GroupByCOATypeId_All =
//                    qTransStart_JoinAccountsNotControlAccount_GroupByCOATypeId.Union(
//qTransStart_JoinAccountsWhereIscontrolAccount_GroupByCOATypeId);
                qTransStart_JoinAccountsNotControlAccount_GroupByCOATypeId_All =
                    qTransStart_JoinAccountsNotControlAccount_GroupByCOATypeId;
            }

            IQueryable<TrailReportTemp> qAccumulateTransEnd__JoinAccountsNotControlAccount_GroupByCOATypeId = Init_TransEnd__JoinAccountsNotControlAccount_GroupByCOATypeId();
            IQueryable<TrailReportTemp> qAccumulateTransEnd__JoinAccountsNotControlAccount_GroupByCOATypeIdAll = null;
            if (true)
            {
                //IQueryable<TrailReportTemp> qAccumulateTransEnd__JoinAccountsWhereIsControlAccount_GroupByCOATypeId = Init_TransEnd__JoinAccountsWhereIsControlAccount_GroupByCOATypeId();

                //qAccumulateTransEnd__JoinAccountsNotControlAccount_GroupByCOATypeIdAll =
                //    qAccumulateTransEnd__JoinAccountsNotControlAccount_GroupByCOATypeId.Union(
                //qAccumulateTransEnd__JoinAccountsWhereIsControlAccount_GroupByCOATypeId);
                qAccumulateTransEnd__JoinAccountsNotControlAccount_GroupByCOATypeIdAll =
                    qAccumulateTransEnd__JoinAccountsNotControlAccount_GroupByCOATypeId;
            }


            IQueryable<TrailReportTemp> _QUnionAllMoneyData = qAccumulateTotals_TotalStart_JoinAccounts_GroupByCOATypeId.Union(qTransStart_JoinAccountsNotControlAccount_GroupByCOATypeId_All).Union(qAccumulateTotals_TotalDelta2End_JoinAccounts_GroupByCOATypeId).Union(qAccumulateTransEnd__JoinAccountsNotControlAccount_GroupByCOATypeIdAll);
            bool addAllChatOfAccountTyps = true;
            if (addAllChatOfAccountTyps)
            {
                _QUnionAllMoneyData = AddAllChatOfAccountTypEmptyRows(_QUnionAllMoneyData, _TrailReportParam.ChartOfAccountsTypeCodeList);
            }

            if (testNow)
            {
                var test1111 = _QUnionAllMoneyData.ToList();
            }

            IQueryable<TrailReportM> qMapAllCurrencySum2TRail = GroupByCOATypeIDSum(_QUnionAllMoneyData);

            if (testNow)
            {
            }

            IQueryable<TrailReportM> qTrailReportM_COAOuterJoinData = init_TrailReportM_COAOuterJoinData(qMapAllCurrencySum2TRail);

            if (testNow)
            {
            }
            GroupTrailSum(qTrailReportM_COAOuterJoinData);

            RemoveEmptyRowsWithoutCurrencies();

            if (testNow)
            {
                var tettt = _QBaseTrailReportFull.ToList();
            }

        }

        private void RemoveEmptyRowsWithoutCurrencies()
        {
            var removeEmptyCOAThatHaveData = true;

            //+		[0]	{COAT:1COA:AAACur:Local:OB:0D:0C:0CB:0Foreign:OB:0D:0C:0CB:0}	
            //+		[1]	{COAT:1COA:Customs IncomeCur:Local:OB:0D:0C:0CB:0Foreign:OB:0D:0C:0CB:0}	
            //+		[2]	{COAT:1COA:Haifa CustomersCur:Local:OB:0D:0C:0CB:0Foreign:OB:0D:0C:0CB:0}	
            //+		[3]	{COAT:1COA:TLV CustomersCur:Local:OB:0D:0C:0CB:0Foreign:OB:0D:0C:0CB:0}	 *****clean*****
            //+		[4]	{COAT:1COA:TLV CustomersCur:1-4418Local:OB:2217.00D:184.75C:0.00CB:2401.75Foreign:OB:554.28D:46.19C:0.00CB:600.47}	
            if (removeEmptyCOAThatHaveData)
            {

                var qRow2Clean =
                    (from trailReportRow in _QBaseTrailReportFull
                     group trailReportRow by new
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
                         trailReportRow.ChartOfAcountCode1,
                         trailReportRow.ChartOfAcountCode2,
                         trailReportRow.ChartOfAcountCode3,
                         trailReportRow.ChartOfAcountCode4,
                         trailReportRow.ChartOfAcountCode5,


                         //trailReportRow.CurrencyId,

                     }

                         into g2clean
                     where g2clean.Count() > 1

                     select new //CleanKeyM
                     {
                         ChartOfAcountType = g2clean.Key.ChartOfAcountType,
                         ChartOfAcount1 = g2clean.Key.ChartOfAcount1,
                         ChartOfAcount2 = g2clean.Key.ChartOfAcount2,
                         ChartOfAcount3 = g2clean.Key.ChartOfAcount3,
                         ChartOfAcount4 = g2clean.Key.ChartOfAcount4,
                         ChartOfAcount5 = g2clean.Key.ChartOfAcount5,
                         ChartOfAcountName1 = g2clean.Key.ChartOfAcountName1,
                         ChartOfAcountName2 = g2clean.Key.ChartOfAcountName2,
                         ChartOfAcountName3 = g2clean.Key.ChartOfAcountName3,
                         ChartOfAcountName4 = g2clean.Key.ChartOfAcountName4,
                         ChartOfAcountName5 = g2clean.Key.ChartOfAcountName5,
                         ChartOfAcountCode1 = g2clean.Key.ChartOfAcountCode1,
                         ChartOfAcountCode2 = g2clean.Key.ChartOfAcountCode2,
                         ChartOfAcountCode3 = g2clean.Key.ChartOfAcountCode3,
                         ChartOfAcountCode4 = g2clean.Key.ChartOfAcountCode4,
                         ChartOfAcountCode5 = g2clean.Key.ChartOfAcountCode5,




                         CurrencyId = string.Empty, //empty is not good CurrencyId = "",//gCOA.Key.CurrencyId,

                     }

                         );
                //_QBaseTrailReportFull =
                //                _QBaseTrailReportFull
                //                    .Except(qRow2Clean);
                _QBaseTrailReportFull =
                    //NonEquijoin
                    (from trailReportRow in _QBaseTrailReportFull

                     where qRow2Clean.Contains(
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
                         trailReportRow.ChartOfAcountCode1,
                         trailReportRow.ChartOfAcountCode2,
                         trailReportRow.ChartOfAcountCode3,
                         trailReportRow.ChartOfAcountCode4,
                         trailReportRow.ChartOfAcountCode5,



                         CurrencyId = trailReportRow.CurrencyId ?? string.Empty,//null <> empty 
                     }) == false
                     select trailReportRow);

            }
        }

        private void GroupTrailSum(IQueryable<TrailReportM> qTrailReportM_COAOuterJoinData)
        {
            _QBaseTrailReportFull =
                (
                from trailReportRow in qTrailReportM_COAOuterJoinData
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
                 trailReportRow.ChartOfAccountId,
                 //trailReportRow.GLAccountName,

                 //trailReportRow.GLAccountId,

                 trailReportRow.CurrencyId,

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

                    ChartOfAcountCode1 = gCOA.Key.ChartOfAcountCode1,
                    ChartOfAcountCode2 = gCOA.Key.ChartOfAcountCode2,
                    ChartOfAcountCode3 = gCOA.Key.ChartOfAcountCode3,
                    ChartOfAcountCode4 = gCOA.Key.ChartOfAcountCode4,
                    ChartOfAcountCode5 = gCOA.Key.ChartOfAcountCode5,

                    ChartOfAcountName1 = gCOA.Key.ChartOfAcountName1,
                    ChartOfAcountName2 = gCOA.Key.ChartOfAcountName2,
                    ChartOfAcountName3 = gCOA.Key.ChartOfAcountName3,
                    ChartOfAcountName4 = gCOA.Key.ChartOfAcountName4,
                    ChartOfAcountName5 = gCOA.Key.ChartOfAcountName5,



                    ChartOfAccountId = gCOA.Key.ChartOfAccountId,

                    GLAccountName = "",//gCOA.Key.GLAccountName,
                    GLAccountNumber = "",

                    GLAccountId = "",//gCOA.Key.GLAccountId,


                    ChartOfAcountName1English = gCOA.Key.ChartOfAcountName1English,
                    ChartOfAcountName2English = gCOA.Key.ChartOfAcountName2English,
                    ChartOfAcountName3English = gCOA.Key.ChartOfAcountName3English,
                    ChartOfAcountName4English = gCOA.Key.ChartOfAcountName4English,
                    ChartOfAcountName5English = gCOA.Key.ChartOfAcountName5English,

                    GLAccountEnglish = "",
                    ChartOfAccountsTypeEnglish = "",
                    ChartOfAccountsEnglish = "", 

                    CurrencyId = gCOA.Key.CurrencyId,


                    LocalOpenBalance = gCOA.Sum(x => x.LocalOpenBalance),
                    LocalDebit = gCOA.Sum(x => x.LocalDebit),
                    LocalCredit = gCOA.Sum(x => x.LocalCredit),
                    LocalCloseBalance = gCOA.Sum(x => x.LocalCloseBalance),



                    ForeignOpenBalance = gCOA.Sum(x => x.ForeignOpenBalance),
                    ForeignDebit = gCOA.Sum(x => x.ForeignDebit),
                    ForeignCredit = gCOA.Sum(x => x.ForeignCredit),
                    ForeignCloseBalance = gCOA.Sum(x => x.ForeignCloseBalance),
                }

            );
        }

        private IQueryable<TrailReportM> init_TrailReportM_COAOuterJoinData(IQueryable<TrailReportM> qMapAllCurrencySum2TRail)
        {
            return (
                from chartf in QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy
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
                }
                 );
        }

        private static IQueryable<TrailReportM> GroupByCOATypeIDSum(IQueryable<TrailReportTemp> _QUnionAllMoneyData)
        {
            IQueryable<TrailReportM> qMapAllCurrencySum2TRail =
                            //map All to TrailReportM Schem 
                            //GetQMap2TrailReportM(_QUnionAllMoneyData);
                            (from r in _QUnionAllMoneyData
                             group r by new
                             {
                                 COAType = r.AccountId_COAType,
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
                                 GLAccountName = "",
                                 GLAccountNumber = "",
                                 
                                 GLAccountId = g.Key.COAType,



                                 ChartOfAcountName1English = "",
                                 ChartOfAcountName2English = "",
                                 ChartOfAcountName3English = "",
                                 ChartOfAcountName4English = "",
                                 ChartOfAcountName5English = "",

                                 GLAccountEnglish = "",
                                 ChartOfAccountsTypeEnglish = "",
                                 ChartOfAccountsEnglish = "",

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
                                   +g.Sum(x => x.ForeignAmountDebitTotalStart)
                                 + g.Sum(x => x.ForeignAmountDebitTotalDelta2End)
                                 + g.Sum(x => x.ForeignAmountDebitTransEnd)
                                 - g.Sum(x => x.ForeignAmountCreditTotalStart)
                                 - g.Sum(x => x.ForeignAmountCreditTotalDelta2End)
                                 - g.Sum(x => x.ForeignAmountCreditTransEnd)
                                 ),

                             });
            return qMapAllCurrencySum2TRail;
        }

        private IQueryable<TrailReportTemp> Init_TransEnd__JoinAccountsWhereIsControlAccount_GroupByCOATypeId()
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
             totalCOAType.CurrencyId
         } into g
         select new TrailReportTemp
         {

             AccountId_COAType = g.Key.Id,
             CurrencyId = g.Key.CurrencyId,

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


             ForeignAmountCreditTransEnd = g.Sum(x => x.ForeignAmountCredit),
             ForeignAmountDebitTransEnd = g.Sum(x => x.ForeignAmountDebit),
             LocalAmountCreditTransEnd = g.Sum(x => x.LocalAmountCredit),
             LocalAmountDebitTransEnd = g.Sum(x => x.LocalAmountDebit),


         }
         );
            return qAccumulateTranactionBeginOfMonthToDateTillToDateInculdeControl;
        }

        private IQueryable<TrailReportTemp> Init_TransEnd__JoinAccountsNotControlAccount_GroupByCOATypeId()
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
                 totalCOAType.CurrencyId
             } into g
             select new TrailReportTemp
             {

                 AccountId_COAType = g.Key.Id,
                 CurrencyId = g.Key.CurrencyId,

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


                 ForeignAmountCreditTransEnd = g.Sum(x => x.ForeignAmountCredit),
                 ForeignAmountDebitTransEnd = g.Sum(x => x.ForeignAmountDebit),
                 LocalAmountCreditTransEnd = g.Sum(x => x.LocalAmountCredit),
                 LocalAmountDebitTransEnd = g.Sum(x => x.LocalAmountDebit),


             }
             );
            return qAccumulateTranactionBeginOfMonthToDateTillToDateInculde;
        }

        private IQueryable<TrailReportTemp> Init_TransStart_JoinAccountsWhereIscontrolAccount_GroupByCOATypeId()
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
                                   totalCOAType.CurrencyId
                               } into g
                               select new TrailReportTemp
                               {

                                   AccountId_COAType = g.Key.Id,
                                   CurrencyId = g.Key.CurrencyId,

                                   ForeignAmountCreditTotalStart = 0,
                                   ForeignAmountDebitTotalStart = 0,
                                   LocalAmountCreditTotalStart = 0,
                                   LocalAmountDebitTotalStart = 0,



                                   ForeignAmountCreditTransStart = g.Sum(x => x.ForeignAmountCredit),
                                   ForeignAmountDebitTransStart = g.Sum(x => x.ForeignAmountDebit),
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

        private IQueryable<TrailReportTemp> Init_LocalAmountTransStart_JoinAccountsNotControlAccount_GroupByCOATypeId()
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
                                   totalCOAType.CurrencyId
                               } into g
                               select new TrailReportTemp
                               {

                                   AccountId_COAType = g.Key.Id,
                                   CurrencyId = g.Key.CurrencyId,

                                   ForeignAmountCreditTotalStart = 0,
                                   ForeignAmountDebitTotalStart = 0,
                                   LocalAmountCreditTotalStart = 0,
                                   LocalAmountDebitTotalStart = 0,



                                   ForeignAmountCreditTransStart = g.Sum(x => x.ForeignAmountCredit),
                                   ForeignAmountDebitTransStart = g.Sum(x => x.ForeignAmountDebit),
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

        private IQueryable<TrailReportTemp> Init_TotalDelta2End_JoinAccounts_GroupByCOATypeId()
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
                                  totalCOAType.CurrencyId
                              } into g
                              select new TrailReportTemp
                              {

                                  AccountId_COAType = g.Key.Id,
                                  CurrencyId = g.Key.CurrencyId,

                                  ForeignAmountCreditTotalStart = 0,
                                  ForeignAmountDebitTotalStart = 0,
                                  LocalAmountCreditTotalStart = 0,
                                  LocalAmountDebitTotalStart = 0,



                                  ForeignAmountCreditTransStart = 0,
                                  ForeignAmountDebitTransStart = 0,
                                  LocalAmountCreditTransStart = 0,
                                  LocalAmountDebitTransStart = 0,


                                  ForeignAmountCreditTotalDelta2End = g.Sum(x => x.ForeignAmountCredit),
                                  ForeignAmountDebitTotalDelta2End = g.Sum(x => x.ForeignAmountDebit),
                                  LocalAmountCreditTotalDelta2End = g.Sum(x => x.LocalAmountCredit),
                                  LocalAmountDebitTotalDelta2End = g.Sum(x => x.LocalAmountDebit),


                                  ForeignAmountCreditTransEnd = 0,
                                  ForeignAmountDebitTransEnd = 0,
                                  LocalAmountCreditTransEnd = 0,
                                  LocalAmountDebitTransEnd = 0,


                              }
                              );
            return qAccumulateTotalsFromStartOfMonthFromTilStartOfMonthTo;
        }

        private IQueryable<TrailReportTemp> Init_TotalStart_JoinAccounts_GroupByCOATypeId()
        {
            var qAccumulateTotalsFrom0BCTilNotIncludeStartOfMonthFromDate =
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
                 totalCOAType.CurrencyId
             } into g
             select new TrailReportTemp
             {

                 AccountId_COAType = g.Key.Id,
                 CurrencyId = g.Key.CurrencyId,
                 ForeignAmountCreditTotalStart = g.Sum(x => x.ForeignAmountCredit),
                 ForeignAmountDebitTotalStart = g.Sum(x => x.ForeignAmountDebit),
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


             }
             );
            return qAccumulateTotalsFrom0BCTilNotIncludeStartOfMonthFromDate;
        }

        private void Disclaimer_InTrailReportByChartofaccountId_Detailed_NotAllowed_Soo_UsingControlAccounts()
        {
            
        }

        private void Disclaimer_InTrailReportByChartofaccountId_UsingCardsOnly_In_QBaseAllCardsAndDetailsAccType()
        {
            
        }

        IQueryable<TrailReportTemp> AddAllChatOfAccountTypEmptyRows(IQueryable<TrailReportTemp> _QUnionAllMoneyData, List<string> chartOfAccountsTypes)
        {
            var allChartTypes = _AccountingContext.ChartOfAccountsTypes.AsQueryable();
            IQueryable<Data.EntityPOCOs.ChartOfAccountsType> chartTypes = null;
            if (chartOfAccountsTypes != null && chartOfAccountsTypes.Count > 0)
            {
                chartTypes = allChartTypes.Where(coa => chartOfAccountsTypes.Contains(coa.Code));
            }
            else
            {
                chartTypes = allChartTypes;
            }

            _QUnionAllMoneyData = _QUnionAllMoneyData.Concat(
            chartTypes.Select(r => new TrailReportTemp()
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
    public class CleanKeyM
    {
        public string ChartOfAcountType;

        public string ChartOfAcount1 { get; set; }

        public string ChartOfAcount2 { get; set; }

        public string ChartOfAcount3 { get; set; }

        public string ChartOfAcount4 { get; set; }

        public string ChartOfAcount5 { get; set; }

        public string CurrencyId { get; set; }
    }
}

