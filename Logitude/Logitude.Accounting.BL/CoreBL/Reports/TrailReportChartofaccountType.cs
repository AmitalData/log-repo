
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
    class TrailReportChartofaccountType : TrailReportBase
    {


        public TrailReportChartofaccountType(TrailReportParam trailReportParam, int timeOutInMin)
            : base(trailReportParam, timeOutInMin) { }

        protected override void AdjustTrailReportFull()
        {
            var testNow = false; // true;// (new DateTime(2016, 12, 30) > DateTime.Now);




            IQueryable<TrailReportTemp> qAccumulateLocalAmountOnly_TotalStart_JoinAccounts_GroupByChartOfAccountsTypeCode =
                Init_LocalAmountOnly_TotalStart_JoinAccounts_GroupByChartOfAccountsTypeCode();

            IQueryable<TrailReportTemp> qAccumulateLocalAmountOnly_TotalDelta2End_JoinAccounts_GroupByChartOfAccountsTypeCode = Init_LocalAmountOnly_TotalDelta2End_JoinAccounts_GroupByChartOfAccountsTypeCode();

            IQueryable<TrailReportTemp> qAccumulate_LocalAmountTransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode = Init_LocalAmount_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode();

            IQueryable<TrailReportTemp> qAccumulate_LocalAmount_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All = null;

            if (true || !base.NotUsingControlAccount()) // in the level we take only cards==1 
            {
                //IQueryable<TrailReportTemp> qAccumulateLocalAmount_TransStart_JoinAccountsWhereIscontrolAccount_GroupByChartOfAccountsTypeCode
                //    = Init_LocalAmount_TransStart_JoinAccountsWhereIscontrolAccount_GroupByChartOfAccountsTypeCode();
                //qAccumulate_LocalAmount_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All =
                //    qAccumulate_LocalAmountTransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode.Union(
                //qAccumulateLocalAmount_TransStart_JoinAccountsWhereIscontrolAccount_GroupByChartOfAccountsTypeCode);
                qAccumulate_LocalAmount_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All =
                    qAccumulate_LocalAmountTransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode;
            }

            IQueryable<TrailReportTemp> qAccumulate_LocalAmount_TransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode = Init_LocalAmount_TransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode();

            IQueryable<TrailReportTemp> qAccumulate_LocalAmount_TransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All = null;
            if (true || !base.NotUsingControlAccount()) // in the level we take only cards==1 
            {
                // IQueryable<TrailReportTemp> qAccumulate_LocalAmounts_TransEnd_JoinAccountsWhereIsControlAccount_ChartOfAccountsTypeCode = Init_LocalAmounts_TransEnd_JoinAccountsWhereIsControlAccount_ChartOfAccountsTypeCode();
                //qAccumulate_LocalAmount_TransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All =
                //    qAccumulate_LocalAmount_TransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode.Union(qAccumulate_LocalAmounts_TransEnd_JoinAccountsWhereIsControlAccount_ChartOfAccountsTypeCode);
                qAccumulate_LocalAmount_TransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All =
                    qAccumulate_LocalAmount_TransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode;
            }
            IQueryable<TrailReportTemp> _QUnionAllMoneyData = qAccumulateLocalAmountOnly_TotalStart_JoinAccounts_GroupByChartOfAccountsTypeCode.Union(qAccumulate_LocalAmount_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All).Union(qAccumulateLocalAmountOnly_TotalDelta2End_JoinAccounts_GroupByChartOfAccountsTypeCode).Union(qAccumulate_LocalAmount_TransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All);

            bool addAllChatOfAccountTyps = true;
            if (addAllChatOfAccountTyps)
            {
                _QUnionAllMoneyData = AddAllChatOfAccountTypEmptyRows(_QUnionAllMoneyData);
            }


            if (testNow)
            {
                var test1111 = _QUnionAllMoneyData.ToList();
            }

            IQueryable<TrailReportM> qMapAllCurrencySum2TRail = GroupBy_AccountId_COAType_SumLocalAmount(_QUnionAllMoneyData);

            _QBaseTrailReportFull = qMapAllCurrencySum2TRail;
            if (testNow)
            {
                var tettt = _QBaseTrailReportFull.ToList();
            }

        }

        private static IQueryable<TrailReportM> GroupBy_AccountId_COAType_SumLocalAmount(IQueryable<TrailReportTemp> _QUnionAllMoneyData)
        {
            IQueryable<TrailReportM> qMapAllCurrencySum2TRail =
                //map All to TrailReportM Schem 
                //GetQMap2TrailReportM(_QUnionAllMoneyData);
                (from r in _QUnionAllMoneyData
                 group r by new
                 {
                     COAType = r.AccountId_COAType,
                     ///r.CurrencyId
                 } into g
                 select new TrailReportM()
                 {

                     ChartOfAcountType = g.Key.COAType,
                     ChartOfAcount1 = "",
                     ChartOfAcount2 = "",
                     ChartOfAcount3 = "",
                     ChartOfAcount4 = "",
                     ChartOfAcount5 = "",

                     GLAccountName = "",
                     GLAccountEnglish = "",

                     GLAccountId = "",
                     CurrencyId = "",// g.Key.CurrencyId,

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

        private IQueryable<TrailReportTemp> Init_LocalAmounts_TransEnd_JoinAccountsWhereIsControlAccount_ChartOfAccountsTypeCode()
        {
            var qAccumulateTranactionBeginOfMonthToDateTillToDateInculdeControl =
            //Accumulate Tranaction BeginOfMonth(ToDate) till ToDate
            (from totalCOAType in

                 (from trans in QBasePeriodTransaction_TransEnd_BeginOfMonthToDate_Till_ToDateInculde
                  join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == true)
                  on trans.ControlAccountId equals a.Id
                  select new { a.ChartOfAccountsTypeCode, trans.CurrencyId, trans.ForeignAmountCredit, trans.ForeignAmountDebit, trans.LocalAmountCredit, trans.LocalAmountDebit }
                 )
             group totalCOAType by new
             {
                 totalCOAType.ChartOfAccountsTypeCode,
                     //totalCOAType.CurrencyId
                 } into g
             select new TrailReportTemp
             {

                 AccountId_COAType = g.Key.ChartOfAccountsTypeCode,
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


             });
            return qAccumulateTranactionBeginOfMonthToDateTillToDateInculdeControl;
        }

        private IQueryable<TrailReportTemp> Init_LocalAmount_TransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode()
        {
            var qAccumulateTranactionBeginOfMonthToDateTillToDateInculde =
            //Accumulate Tranaction BeginOfMonth(ToDate) till ToDate
            (from totalCOAType in
                 (from trans in QBasePeriodTransaction_TransEnd_BeginOfMonthToDate_Till_ToDateInculde
                  join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == false)
                  on trans.AccountId equals a.Id
                  select new { a.ChartOfAccountsTypeCode, trans.CurrencyId, trans.ForeignAmountCredit, trans.ForeignAmountDebit, trans.LocalAmountCredit, trans.LocalAmountDebit }
                      )

             group totalCOAType by new
             {
                 totalCOAType.ChartOfAccountsTypeCode,
                 //totalCOAType.CurrencyId
             } into g
             select new TrailReportTemp
             {

                 AccountId_COAType = g.Key.ChartOfAccountsTypeCode,
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


             });
            return qAccumulateTranactionBeginOfMonthToDateTillToDateInculde;
        }

        private IQueryable<TrailReportTemp> Init_LocalAmount_TransStart_JoinAccountsWhereIscontrolAccount_GroupByChartOfAccountsTypeCode()
        {
            var qAccumulateTranactionBeginOfMonthFromTillFromDateNotIncludeControl =
                                  //Accumulate Tranaction BeginOfMonth(fromDate) till (FromDate-1d)
                                  (from totalCOAType in

                                       (from trans in QBasePeriodTransaction_TransStart_BeginOfMonthFromDate_TillFromDate_NotInclude
                                        join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == true)
                                       on trans.ControlAccountId equals a.Id
                                        select new
                                        {
                                            a.ChartOfAccountsTypeCode, //acc + control acc have to be the same (alex check !!)
                                trans.CurrencyId,
                                            trans.ForeignAmountCredit,
                                            trans.ForeignAmountDebit,
                                            trans.LocalAmountCredit,
                                            trans.LocalAmountDebit
                                        }
                                       )
                                   group totalCOAType by new
                                   {
                                       totalCOAType.ChartOfAccountsTypeCode,
                           //totalCOAType.CurrencyId
                       } into g
                                   select new TrailReportTemp
                                   {

                                       AccountId_COAType = g.Key.ChartOfAccountsTypeCode,
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

        private IQueryable<TrailReportTemp> Init_LocalAmount_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode()
        {
            var qAccumulateTranactionBeginOfMonthFromTillFromDateNotInclude =
                  //Accumulate Tranaction BeginOfMonth(fromDate) till (FromDate-1d)
                  (from totalCOAType in
                       (from trans in QBasePeriodTransaction_TransStart_BeginOfMonthFromDate_TillFromDate_NotInclude
                        join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == false)
                       on trans.AccountId equals a.Id
                        select new { a.ChartOfAccountsTypeCode, trans.CurrencyId, trans.ForeignAmountCredit, trans.ForeignAmountDebit, trans.LocalAmountCredit, trans.LocalAmountDebit }
                       )
                   group totalCOAType by new
                   {
                       totalCOAType.ChartOfAccountsTypeCode,
                       //totalCOAType.CurrencyId
                   } into g
                   select new TrailReportTemp
                   {

                       AccountId_COAType = g.Key.ChartOfAccountsTypeCode,
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

        private IQueryable<TrailReportTemp> Init_LocalAmountOnly_TotalDelta2End_JoinAccounts_GroupByChartOfAccountsTypeCode()
        {
            var qAccumulateTotalsFromStartOfMonthFromTilStartOfMonthTo = //Accumulate Totals From StartOfMonth(FromDate) Until StartOfMonth(ToDate) 
                 (from totalCOAType in

                      (from tot in QBasePeriodGLATotalByMonths_TotalDelta2End_FromBeginOfMonthFromDate_Til_BeginOfMonthToDate
                       join a in QBaseAllCardsAndDetailsAccType
                       on tot.AccountId equals a.Id
                       select new { a.ChartOfAccountsTypeCode, tot.CurrencyId, tot.ForeignAmountCredit, tot.ForeignAmountDebit, tot.LocalAmountCredit, tot.LocalAmountDebit }
                       )
                  group totalCOAType by new
                  {
                      totalCOAType.ChartOfAccountsTypeCode,
                      ///totalCOAType.CurrencyId
                  } into g
                  select new TrailReportTemp
                  {

                      AccountId_COAType = g.Key.ChartOfAccountsTypeCode,
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

        private IQueryable<TrailReportTemp> Init_LocalAmountOnly_TotalStart_JoinAccounts_GroupByChartOfAccountsTypeCode()
        {
            var qAccumulateTotalsFrom0BCTilNotIncludeStartOfMonthFromDate =
                        //Accumulate Totals From the beginning of  Account Use Until(NotInclude) (StartOfMonth)FromDate                
                        (from totalCOAType in

                             (from tot in QBasePeriodGLATotalByMonths_TotalStart_From0BC_TilNotInclude_BeginOfMonth_FromDate
                              join a in QBaseAllCardsAndDetailsAccType
                              on tot.AccountId equals a.Id
                              select new { a.ChartOfAccountsTypeCode, tot.CurrencyId, tot.ForeignAmountCredit, tot.ForeignAmountDebit, tot.LocalAmountCredit, tot.LocalAmountDebit }
                              )
                         group totalCOAType by new
                         {
                             totalCOAType.ChartOfAccountsTypeCode,
                 //totalCOAType.CurrencyId
             } into g
                         select new TrailReportTemp
                         {

                             AccountId_COAType = g.Key.ChartOfAccountsTypeCode,
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
            return qAccumulateTotalsFrom0BCTilNotIncludeStartOfMonthFromDate;
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
