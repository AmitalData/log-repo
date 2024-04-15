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
    class TrailReportChartofaccountTypeCurrenciesDetailed : TrailReportBase
    {


        public TrailReportChartofaccountTypeCurrenciesDetailed(TrailReportParam trailReportParam, int timeOutInMin)
            : base(trailReportParam, timeOutInMin) { }

        protected override void AdjustTrailReportFull()
        {
            var testNow = true;// (new DateTime(2016, 12, 30) > DateTime.Now);


            IQueryable<TrailReportTemp> qAccumulate_TotalStart_JoinAccounts_GroupByChartOfAccountsTypeCode =
                Init_TotalStart_JoinAccounts_GroupByChartOfAccountsTypeCode();

            IQueryable<TrailReportTemp> qAccumulate_TotalDelta2End_JoinAccounts_GroupByChartOfAccountsTypeCode = Init_TotalDelta2End_JoinAccounts_GroupByChartOfAccountsTypeCode();

            IQueryable<TrailReportTemp> qAccumulate_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode = Init_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode();

            IQueryable<TrailReportTemp> qAccumulate_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All = null;
            if (true)// in the level we take only cards==1 
            {
                IQueryable<TrailReportTemp> qAccumulate_TransStart_JoinAccountsWhereIscontrolAccount_GroupByChartOfAccountsTypeCode = Init_TransStart_JoinAccountsWhereIscontrolAccount_GroupByChartOfAccountsTypeCode();

                qAccumulate_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All =
                    qAccumulate_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode.Union(
                qAccumulate_TransStart_JoinAccountsWhereIscontrolAccount_GroupByChartOfAccountsTypeCode);
            }

            IQueryable<TrailReportTemp> qAccumulateTransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All = null;
            IQueryable<TrailReportTemp> qAccumulateTransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode = Init_TransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode();
            if (true)// in the level we take only cards==1 
            {
                IQueryable<TrailReportTemp> qAccumulate_TransEnd_JoinAccountsWhereIsControlAccount_ChartOfAccountsTypeCode = Init_TransEnd_JoinAccountsWhereIsControlAccount_ChartOfAccountsTypeCode();
                qAccumulateTransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All =
                    qAccumulateTransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode.Union(
                    qAccumulate_TransEnd_JoinAccountsWhereIsControlAccount_ChartOfAccountsTypeCode);
            }

            IQueryable<TrailReportTemp> _QUnionAllMoneyData = qAccumulate_TotalStart_JoinAccounts_GroupByChartOfAccountsTypeCode.Union(qAccumulate_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All).Union(qAccumulate_TotalDelta2End_JoinAccounts_GroupByChartOfAccountsTypeCode).Union(qAccumulateTransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode_All);

            bool addAllChatOfAccountTyps = true;
            if (addAllChatOfAccountTyps)
            {
                _QUnionAllMoneyData = AddAllChatOfAccountTypEmptyRows(_QUnionAllMoneyData);
            }
            if (testNow)
            {
                var test1111 = _QUnionAllMoneyData.ToList();
            }

            IQueryable<TrailReportM> qMapAllCurrencySum2TRail = GroupBy_AccountId_COAType_Sum(_QUnionAllMoneyData);

            _QBaseTrailReportFull = qMapAllCurrencySum2TRail;
            if (testNow)
            {
                var tettt = _QBaseTrailReportFull.ToList();
            }
        }

        private static IQueryable<TrailReportM> GroupBy_AccountId_COAType_Sum(IQueryable<TrailReportTemp> _QUnionAllMoneyData)
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

                      ChartOfAcountType = g.Key.COAType,
                      ChartOfAcount1 = "",
                      ChartOfAcount2 = "",
                      ChartOfAcount3 = "",
                      ChartOfAcount4 = "",
                      ChartOfAcount5 = "",

                      GLAccountName = "",
                      GLAccountEnglish = "",

                      GLAccountId = "",
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
                       + g.Sum(x => x.ForeignAmountDebitTransEnd)
                       - g.Sum(x => x.ForeignAmountDebitTransStart)
                      ),
                      ForeignCredit =
                      (
                       +g.Sum(x => x.ForeignAmountCreditTotalDelta2End)
                       + g.Sum(x => x.ForeignAmountCreditTransEnd)
                       - g.Sum(x => x.ForeignAmountCreditTransStart)
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

        private IQueryable<TrailReportTemp> Init_TransEnd_JoinAccountsWhereIsControlAccount_ChartOfAccountsTypeCode()
        {
            var qAccumulateTranactionBeginOfMonthToDateTillToDateInculdeControl =
                        //Accumulate Tranaction BeginOfMonth(ToDate) till ToDate
                        (from totalCOAType in
                             (from trans in QBasePeriodTransaction_TransEnd_BeginOfMonthToDate_Till_ToDateInculde
                              join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == false)
                              on trans.AccountId equals a.Id
                              select new { a.ChartOfAccountsTypeCode, trans.CurrencyId, trans.ForeignAmountCredit, trans.ForeignAmountDebit, trans.LocalAmountCredit, trans.LocalAmountDebit }
                              )
                              .Union
                              (from trans in QBasePeriodTransaction_TransEnd_BeginOfMonthToDate_Till_ToDateInculde
                               join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == true)
                               on trans.ControlAccountId equals a.Id
                               select new { a.ChartOfAccountsTypeCode, trans.CurrencyId, trans.ForeignAmountCredit, trans.ForeignAmountDebit, trans.LocalAmountCredit, trans.LocalAmountDebit }
                              )
                         group totalCOAType by new
                         {
                             totalCOAType.ChartOfAccountsTypeCode,
                             totalCOAType.CurrencyId
                         } into g
                         select new TrailReportTemp
                         {

                             AccountId_COAType = g.Key.ChartOfAccountsTypeCode,
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


                         });
            return qAccumulateTranactionBeginOfMonthToDateTillToDateInculdeControl;
        }

        private IQueryable<TrailReportTemp> Init_TransEnd_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode()
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
                             totalCOAType.CurrencyId
                         } into g
                         select new TrailReportTemp
                         {

                             AccountId_COAType = g.Key.ChartOfAccountsTypeCode,
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


                         });
            return qAccumulateTranactionBeginOfMonthToDateTillToDateInculde;
        }

        private IQueryable<TrailReportTemp> Init_TransStart_JoinAccountsWhereIscontrolAccount_GroupByChartOfAccountsTypeCode()
        {
            var qAccumulateTranactionBeginOfMonthFromTillFromDateNotIncludeControl =
                //Accumulate Tranaction BeginOfMonth(fromDate) till (FromDate-1d)
                (from totalCOAType in

                     (from trans in QBasePeriodTransaction_TransStart_BeginOfMonthFromDate_TillFromDate_NotInclude
                      join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == true)
                          on trans.ControlAccountId equals a.Id
                      select new { a.ChartOfAccountsTypeCode, trans.CurrencyId, trans.ForeignAmountCredit, trans.ForeignAmountDebit, trans.LocalAmountCredit, trans.LocalAmountDebit }
                      )
                 group totalCOAType by new
                 {
                     totalCOAType.ChartOfAccountsTypeCode,
                     totalCOAType.CurrencyId
                 } into g
                 select new TrailReportTemp
                 {

                     AccountId_COAType = g.Key.ChartOfAccountsTypeCode,
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

        private IQueryable<TrailReportTemp> Init_TransStart_JoinAccountsNotControlAccount_GroupByChartOfAccountsTypeCode()
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
                                     totalCOAType.CurrencyId
                                 } into g
                                 select new TrailReportTemp
                                 {

                                     AccountId_COAType = g.Key.ChartOfAccountsTypeCode,
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

        private IQueryable<TrailReportTemp> Init_TotalDelta2End_JoinAccounts_GroupByChartOfAccountsTypeCode()
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
                                  totalCOAType.CurrencyId
                              } into g
                              select new TrailReportTemp
                              {

                                  AccountId_COAType = g.Key.ChartOfAccountsTypeCode,
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


                              });
            return qAccumulateTotalsFromStartOfMonthFromTilStartOfMonthTo;
        }

        private IQueryable<TrailReportTemp> Init_TotalStart_JoinAccounts_GroupByChartOfAccountsTypeCode()
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
                             totalCOAType.CurrencyId
                         } into g
                         select new TrailReportTemp
                         {

                             AccountId_COAType = g.Key.ChartOfAccountsTypeCode,
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
