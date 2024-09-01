
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
    class TrailReportGLAccount : TrailReportBase
    {

        string _accountId;

        public TrailReportGLAccount(TrailReportParam trailReportParam, int timeOutInMin)
            : base(trailReportParam, timeOutInMin) { }

        protected override void AdjustTrailReportFull()
        {
            var testNow = false;// (new DateTime(2016, 12, 30) > DateTime.Now);

            var checkChartOfAcountType1LocalDebit1263p90 = false;
            if (checkChartOfAcountType1LocalDebit1263p90)
            {
                //Type =1263.90
                //Chartofaccount==1111.50+76.15	
                QBaseAllCardsAndDetailsAccType = QBaseAllCardsAndDetailsAccType.Where(coa => coa.ChartOfAccountsTypeCode == "1");
            }
            if (_TrailReportParam.Suppress_ControlAccount)
            {
                QBaseAllCardsAndDetailsAccType = QBaseAllCardsAndDetailsAccType.Where(coa => coa.IsControlAccount == false);
            }

            IQueryable<TrailReportTemp> qLocalAmountOnly_TotalStart_GroupByAccount = Init_LocalAmountOnly_TotalStart_JoinAccounts_GroupByAccount();
            if (!String.IsNullOrWhiteSpace(_accountId))
            {
                //_DbLogger.AddExplainLog("qAccumulateTotalsFrom0BCTilNotIncludeStartOfMonthFromDate:TotalStart:");
                var l1 = qLocalAmountOnly_TotalStart_GroupByAccount.Where(r => r.AccountId_COAType == _accountId).ToList();
            }

            IQueryable<TrailReportTemp> qAccumulateLocalAmountOnly_TotalDelta2End_JoinAccounts_GroupByAccount = Init_LocalAmountOnly_TotalDelta2End_JoinAccounts_GroupByAccount();

            if (!String.IsNullOrWhiteSpace(_accountId))
            {
                //_DbLogger.AddExplainLog("qAccumulateTotalsFromStartOfMonthFromTilStartOfMonthTo:TotalDelta2End:");
                var l2 = qAccumulateLocalAmountOnly_TotalDelta2End_JoinAccounts_GroupByAccount.Where(r => r.AccountId_COAType == _accountId).ToList()
                    ;
            }

            IQueryable<TrailReportTemp> qAccumulateLocalAmountOnly_TransStart_JoinAccountsNotControlAccount_GroupByAccount = Init_LocalAmountOnly_LocalAmountTransStart_JoinAccountsNotControlAccount_GroupByAccount();
            IQueryable<TrailReportTemp> qAccumulateLocalAmountOnly_TransStart_GroupByAccount_All = qAccumulateLocalAmountOnly_TransStart_JoinAccountsNotControlAccount_GroupByAccount;
            if (!base.NotUsingControlAccount())
            {
                //IQueryable<TrailReportTemp> qAccumulate_LocalAmountOnly_TransStart_JoinAccountsWhereIscontrolAccount_GroupByAccount = Init_LocalAmountOnly_TransStart_JoinAccountsWhereIscontrolAccount_GroupByAccount();
                //qAccumulateLocalAmountOnly_TransStart_GroupByAccount_All =
                //qAccumulateLocalAmountOnly_TransStart_JoinAccountsNotControlAccount_GroupByAccount.Union(
                //qAccumulate_LocalAmountOnly_TransStart_JoinAccountsWhereIscontrolAccount_GroupByAccount);
                qAccumulateLocalAmountOnly_TransStart_GroupByAccount_All =
                qAccumulateLocalAmountOnly_TransStart_JoinAccountsNotControlAccount_GroupByAccount;
            }


            if (!String.IsNullOrWhiteSpace(_accountId))
            {
                //_DbLogger.AddExplainLog("qAccumulateTranactionBeginOfMonthFromTillFromDateNotInclude:");
                var l3 = qAccumulateLocalAmountOnly_TransStart_GroupByAccount_All.Where(r => r.AccountId_COAType == _accountId).ToList();
            }

            IQueryable<TrailReportTemp> qAccumulate_LocalAmountOnly_TransEnd__JoinAccountsNotControlAccount_GroupByAccount = Init_LocalAmountOnly_TransEnd__JoinAccountsNotControlAccount_GroupByAccount();
            IQueryable<TrailReportTemp> qLocalAmountOnly_LocalAmountTransEnd_GroupByAccount_All = qAccumulate_LocalAmountOnly_TransEnd__JoinAccountsNotControlAccount_GroupByAccount;
            if (!base.NotUsingControlAccount())
            {
                //IQueryable<TrailReportTemp> qAccumulate_LocalAmountOnly_TransEnd__JoinAccountsWhereIsControlAccount_GroupByAccount = Init_LocalAmountOnly_TransEnd__JoinAccountsWhereIsControlAccount_GroupByAccount();

                //qLocalAmountOnly_LocalAmountTransEnd_GroupByAccount_All =
                //qAccumulate_LocalAmountOnly_TransEnd__JoinAccountsNotControlAccount_GroupByAccount.Union(
                //qAccumulate_LocalAmountOnly_TransEnd__JoinAccountsWhereIsControlAccount_GroupByAccount);
                qLocalAmountOnly_LocalAmountTransEnd_GroupByAccount_All =
                qAccumulate_LocalAmountOnly_TransEnd__JoinAccountsNotControlAccount_GroupByAccount;
            }
            if (!String.IsNullOrWhiteSpace(_accountId))
            {
                ///_DbLogger.AddExplainLog("qAccumulateTranactionBeginOfMonthToDateTillToDateInculde:");
                var l4 = qLocalAmountOnly_LocalAmountTransEnd_GroupByAccount_All.Where(r => r.AccountId_COAType == _accountId).ToList();
            }



            IQueryable<TrailReportTemp> _QUnionAllMoneyData = qLocalAmountOnly_TotalStart_GroupByAccount.Union(qAccumulateLocalAmountOnly_TransStart_GroupByAccount_All).Union(qAccumulateLocalAmountOnly_TotalDelta2End_JoinAccounts_GroupByAccount).Union(qLocalAmountOnly_LocalAmountTransEnd_GroupByAccount_All);

            bool addAllChatOfAccountTyps = true;
            if (addAllChatOfAccountTyps)
            {
                _QUnionAllMoneyData = AddAllChatOfAccountTypEmptyRows(_QUnionAllMoneyData, _TrailReportParam.ChartOfAccountsTypeCodeList);
            }

            if (testNow)
            {
                var test1111 = _QUnionAllMoneyData.ToList();
            }
            /*
דוח מאזן בוחן
1ביררת המחדל -לא יצגו כרטיסים לא פעילים .
2לפי חיתוך\בחירה "הצג כרטיסים לא פעילים" – יצגו !
*** כרטיסים לא פעילים (יתרת פתיחה לתקופה 0 וללא תנועות  לטווח הדוח )

             */


            var qUnionAllMoneyDataGroupByAcc =
                (
                from r in _QUnionAllMoneyData
                group r by new
                {
                    AccountId = r.AccountId_COAType,
                    //r.CurrencyId
                }
                 );


            if (!_TrailReportParam.Suppress_DoNotShowCardWithoutActivity)
            {
                qUnionAllMoneyDataGroupByAcc =
                    qUnionAllMoneyDataGroupByAcc
                    .Where(g =>

                        g.Any(x => x.TotalDelta2End_AnyActivity == true) ||

                        g.Any(x => x.TransEnd_AnyActivity == true)
                    ||
                     (
                     +g.Sum(x => x.LocalAmountDebitTotalStart)
                     - g.Sum(x => x.LocalAmountCreditTotalStart)
                     + g.Sum(x => x.LocalAmountDebitTransStart)
                     - g.Sum(x => x.LocalAmountCreditTransStart)
                     ) != 0m

                     );

            }


            //IQueryable<TrailReportM> qMapAllCurrencySum2TRail = NewMethod(_QUnionAllMoneyData);
            IQueryable<TrailReportM> qMapAllCurrencySum2TRail =
              //map All to TrailReportM Schem 
              (from g in qUnionAllMoneyDataGroupByAcc
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
                   GLAccountId = g.Key.AccountId,


                   ChartOfAcountName1English = "",
                   ChartOfAcountName2English = "",
                   ChartOfAcountName3English = "",
                   ChartOfAcountName4English = "",
                   ChartOfAcountName5English = "",

                   GLAccountEnglish = "",
                   ChartOfAccountsTypeEnglish = "",
                   ChartOfAccountsEnglish = "",

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
                    +g.Sum(x => x.LocalAmountDebitTotalDelta2End)//// מצטברים מחודש כולל ועד חודש לא כולל
                    + g.Sum(x => x.LocalAmountDebitTransEnd)// תנעות מתחילת חודש אחרון כולל עד  תאריך הסיום + 1 לא כולל
                    - g.Sum(x => x.LocalAmountDebitTransStart)//// תנעות מכולל תחילת החודש  של מתאריך עד למתאריך -לא כולל    
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

            IQueryable<TrailReportM> joinq5LvlqMapAllCurrencySum2TRail = null;
            if (_TrailReportParam.Suppress_DoNotShowCardWithoutActivity)
            {

                joinq5LvlqMapAllCurrencySum2TRail =
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
            else
            {
                joinq5LvlqMapAllCurrencySum2TRail =
                  (from data in qMapAllCurrencySum2TRail
                   join chartf in QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy
                   on data.GLAccountId equals chartf.GLAccountId
                   //into groupJoin
                   //from groupJoinData in groupJoin //.DefaultIfEmpty()
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

                       CurrencyId = data.CurrencyId,


                       LocalOpenBalance = data.LocalOpenBalance,
                       LocalDebit = data.LocalDebit,
                       LocalCredit = data.LocalCredit,
                       LocalCloseBalance = data.LocalCloseBalance,



                       ForeignOpenBalance = data.ForeignOpenBalance,

                       ForeignDebit = data.ForeignDebit,

                       ForeignCredit = data.ForeignCredit,

                       ForeignCloseBalance = data.ForeignCloseBalance,
                   }
                      );

            }
            //SaveOld_QbaseTrailReportFull(qMapAllCurrencySum2TRail);
            _QBaseTrailReportFull =
                (from trailReportRow in joinq5LvlqMapAllCurrencySum2TRail
                 group trailReportRow by
             new
             {
                 trailReportRow.ChartOfAcountType,

                 trailReportRow.ChartOfAcount1,
                 trailReportRow.ChartOfAcount2,
                 trailReportRow.ChartOfAcount3,
                 trailReportRow.ChartOfAcount4,
                 trailReportRow.ChartOfAcount5,
                 trailReportRow.ChartOfAcountCode1,
                 trailReportRow.ChartOfAcountCode2,
                 trailReportRow.ChartOfAcountCode3,
                 trailReportRow.ChartOfAcountCode4,
                 trailReportRow.ChartOfAcountCode5,

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

                 trailReportRow.ChartOfAccountId,
                 trailReportRow.GLAccountName,
                 trailReportRow.GLAccountNumber,
                 trailReportRow.GLAccountId,

                 trailReportRow.ChartOfAccountsTypeEnglish,
                 trailReportRow.ChartOfAccountsEnglish,
                 trailReportRow.GLAccountEnglish,

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
                     GLAccountName = gCOA.Key.GLAccountName,
                     GLAccountNumber = gCOA.Key.GLAccountNumber,
                     GLAccountId = gCOA.Key.GLAccountId,


                     ChartOfAcountName1English = gCOA.Key.ChartOfAcountName1English,
                     ChartOfAcountName2English = gCOA.Key.ChartOfAcountName2English,
                     ChartOfAcountName3English = gCOA.Key.ChartOfAcountName3English,
                     ChartOfAcountName4English = gCOA.Key.ChartOfAcountName4English,
                     ChartOfAcountName5English = gCOA.Key.ChartOfAcountName5English,

                     GLAccountEnglish = gCOA.Key.GLAccountEnglish,
                     ChartOfAccountsTypeEnglish = gCOA.Key.ChartOfAccountsTypeEnglish,
                     ChartOfAccountsEnglish = gCOA.Key.ChartOfAccountsEnglish,

                     CurrencyId = /*gCOA.FirstOrDefault(r=>r.CurrencyId!=null).CurrencyId,*/ "",// gCOA.Key.CurrencyId,


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
            
            _QBaseTrailReportFull = (from gCOA in _QBaseTrailReportFull
                                     join a in QBaseAllCardsAndDetailsAccType
                                     on gCOA.GLAccountId equals a.Id
                                     select new TrailReportM()
                                     {
                                         ChartOfAcountType = gCOA.ChartOfAcountType,
                                         ChartOfAcount1 = gCOA.ChartOfAcount1,
                                         ChartOfAcount2 = gCOA.ChartOfAcount2,
                                         ChartOfAcount3 = gCOA.ChartOfAcount3,
                                         ChartOfAcount4 = gCOA.ChartOfAcount4,
                                         ChartOfAcount5 = gCOA.ChartOfAcount5,

                                         ChartOfAcountCode1 = gCOA.ChartOfAcountCode1,
                                         ChartOfAcountCode2 = gCOA.ChartOfAcountCode2,
                                         ChartOfAcountCode3 = gCOA.ChartOfAcountCode3,
                                         ChartOfAcountCode4 = gCOA.ChartOfAcountCode4,
                                         ChartOfAcountCode5 = gCOA.ChartOfAcountCode5,

                                         ChartOfAcountName1 = gCOA.ChartOfAcountName1,
                                         ChartOfAcountName2 = gCOA.ChartOfAcountName2,
                                         ChartOfAcountName3 = gCOA.ChartOfAcountName3,
                                         ChartOfAcountName4 = gCOA.ChartOfAcountName4,
                                         ChartOfAcountName5 = gCOA.ChartOfAcountName5,



                                         ChartOfAccountId = gCOA.ChartOfAccountId,
                                         GLAccountName = gCOA.GLAccountName,
                                         GLAccountNumber = gCOA.GLAccountNumber,
                                         GLAccountId = gCOA.GLAccountId,


                                         ChartOfAcountName1English = gCOA.ChartOfAcountName1English,
                                         ChartOfAcountName2English = gCOA.ChartOfAcountName2English,
                                         ChartOfAcountName3English = gCOA.ChartOfAcountName3English,
                                         ChartOfAcountName4English = gCOA.ChartOfAcountName4English,
                                         ChartOfAcountName5English = gCOA.ChartOfAcountName5English,

                                         GLAccountEnglish = gCOA.GLAccountEnglish,
                                         ChartOfAccountsTypeEnglish = gCOA.ChartOfAccountsTypeEnglish,
                                         ChartOfAccountsEnglish = gCOA.ChartOfAccountsEnglish,

                                         CurrencyId = a.IsMultiCurrency==true?null:(a.CurrencyId== _AccountingCurrencyId ? null: a.CurrencyId),  

                                         LocalOpenBalance = gCOA.LocalOpenBalance,
                                         LocalDebit = gCOA.LocalDebit,
                                         LocalCredit = gCOA.LocalCredit,
                                         LocalCloseBalance = gCOA.LocalCloseBalance,



                                         ForeignOpenBalance = a.IsMultiCurrency == true ? null : (a.CurrencyId == _AccountingCurrencyId ? null : gCOA.ForeignOpenBalance),
                                         
                                         ForeignDebit = a.IsMultiCurrency == true ? null : (a.CurrencyId == _AccountingCurrencyId ? null : gCOA.ForeignDebit),
                                         
                                         ForeignCredit = a.IsMultiCurrency == true ? null : (a.CurrencyId == _AccountingCurrencyId ? null : gCOA.ForeignCredit),
                                         
                                         ForeignCloseBalance = a.IsMultiCurrency == true ? null : (a.CurrencyId == _AccountingCurrencyId ? null : gCOA.ForeignCloseBalance),
                                         
                                     }

                                     );


            if (_TrailReportParam.DoNotShowCardWithLocalCloseBalanceEqualZero)
            {
                _QBaseTrailReportFull = _QBaseTrailReportFull.Where(r => r.LocalCloseBalance.HasValue &&   r.LocalCloseBalance != 0);
            }
            if (testNow)
            {
                var tettt = _QBaseTrailReportFull.ToList();
            }


        }

        private IQueryable<TrailReportTemp> Init_LocalAmountOnly_TransEnd__JoinAccountsWhereIsControlAccount_GroupByAccount()
        {
            var qAccumulateTranactionBeginOfMonthToDateTillToDateInculdeControl =
          (from totalCOAType in
                (
           from trans in QBasePeriodTransaction_TransEnd_BeginOfMonthToDate_Till_ToDateInculde
           join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == true)
           on trans.ControlAccountId equals a.Id
           select new
           {
               AccountId = trans.ControlAccountId, //trans.AccountId,
                                                   //trans.CurrencyId, 
                   trans.ForeignAmountCredit,
               trans.ForeignAmountDebit,
               trans.LocalAmountCredit,
               trans.LocalAmountDebit
           }

                )

           group totalCOAType by new
           {
               totalCOAType.AccountId,
                   //totalCOAType.CurrencyId
               } into g
           select new TrailReportTemp
           {

               AccountId_COAType = g.Key.AccountId,
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


               ForeignAmountCreditTransEnd = g.Sum(x => x.ForeignAmountCredit),
                   ForeignAmountDebitTransEnd = g.Sum(x => x.ForeignAmountDebit),
                   LocalAmountCreditTransEnd = g.Sum(x => x.LocalAmountCredit),
               LocalAmountDebitTransEnd = g.Sum(x => x.LocalAmountDebit),

               TotalDelta2End_AnyActivity = null,
               TransEnd_AnyActivity = g.Any(),
           }
           );
            return qAccumulateTranactionBeginOfMonthToDateTillToDateInculdeControl;
        }

        private IQueryable<TrailReportTemp> Init_LocalAmountOnly_TransEnd__JoinAccountsNotControlAccount_GroupByAccount()
        {
            var qAccumulateTranactionBeginOfMonthToDateTillToDateInculde =
            (from totalCOAType in
                 (from trans in QBasePeriodTransaction_TransEnd_BeginOfMonthToDate_Till_ToDateInculde
                  join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == false)
                  on trans.AccountId equals a.Id
                  select new
                  {
                      trans.AccountId,
                      //trans.CurrencyId, 
                      trans.ForeignAmountCredit,
                      trans.ForeignAmountDebit,
                      trans.LocalAmountCredit,
                      trans.LocalAmountDebit
                  }

                  )
             group totalCOAType by new
             {
                 totalCOAType.AccountId,
                 //totalCOAType.CurrencyId
             } into g
             select new TrailReportTemp
             {

                 AccountId_COAType = g.Key.AccountId,
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


                 ForeignAmountCreditTransEnd = g.Sum(x => x.ForeignAmountCredit),
                 ForeignAmountDebitTransEnd = g.Sum(x => x.ForeignAmountDebit),
                 LocalAmountCreditTransEnd = g.Sum(x => x.LocalAmountCredit),
                 LocalAmountDebitTransEnd = g.Sum(x => x.LocalAmountDebit),

                 TotalDelta2End_AnyActivity = null,
                 TransEnd_AnyActivity = g.Any(),
             }
             );
            return qAccumulateTranactionBeginOfMonthToDateTillToDateInculde;
        }

        private IQueryable<TrailReportTemp> Init_LocalAmountOnly_TransStart_JoinAccountsWhereIscontrolAccount_GroupByAccount()
        {
            var qAccumulateTranactionBeginOfMonthFromTillFromDateNotIncludeControl =
                   (from totalCOAType in

                        (
                        from trans in QBasePeriodTransaction_TransStart_BeginOfMonthFromDate_TillFromDate_NotInclude
                        join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == true)
                            on trans.ControlAccountId equals a.Id
                        select new
                        {
                            AccountId = trans.ControlAccountId,//trans.AccountId,
                                                               //trans.CurrencyId, 
                                trans.ForeignAmountCredit,
                            trans.ForeignAmountDebit,
                            trans.LocalAmountCredit,
                            trans.LocalAmountDebit
                        }
                        )

                    group totalCOAType by new
                    {
                        totalCOAType.AccountId,
                            //totalCOAType.CurrencyId
                        } into g
                    select new TrailReportTemp
                    {

                        AccountId_COAType = g.Key.AccountId,
                        CurrencyId = "",//g.Key.CurrencyId,

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

                        TotalDelta2End_AnyActivity = null,
                        TransEnd_AnyActivity = null,
                    }
          );
            return qAccumulateTranactionBeginOfMonthFromTillFromDateNotIncludeControl;
        }

        private IQueryable<TrailReportTemp> Init_LocalAmountOnly_LocalAmountTransStart_JoinAccountsNotControlAccount_GroupByAccount()
        {
            var qAccumulateTranactionBeginOfMonthFromTillFromDateNotInclude =
                  //Accumulate Tranaction BeginOfMonth(fromDate) till (FromDate-1d)
                  (from totalCOAType in

                       (
                        from trans in QBasePeriodTransaction_TransStart_BeginOfMonthFromDate_TillFromDate_NotInclude
                        join a in QBaseAllCardsAndDetailsAccType.Where(a => a.IsControlAccount == false)
                            on trans.AccountId equals a.Id
                        select new
                        {
                            trans.AccountId,
                            //trans.CurrencyId, 
                            trans.ForeignAmountCredit,
                            trans.ForeignAmountDebit,
                            trans.LocalAmountCredit,
                            trans.LocalAmountDebit
                        }
                        )

                   group totalCOAType by new
                   {
                       totalCOAType.AccountId,
                       //totalCOAType.CurrencyId
                   } into g
                   select new TrailReportTemp
                   {

                       AccountId_COAType = g.Key.AccountId,
                       CurrencyId = "",//g.Key.CurrencyId,

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

                       TotalDelta2End_AnyActivity = null,
                       TransEnd_AnyActivity = null,
                   }
                  );
            return qAccumulateTranactionBeginOfMonthFromTillFromDateNotInclude;
        }

        private IQueryable<TrailReportTemp> Init_LocalAmountOnly_TotalDelta2End_JoinAccounts_GroupByAccount()
        {
            var qAccumulateTotalsFromStartOfMonthFromTilStartOfMonthTo = //Accumulate Totals From StartOfMonth(FromDate) Until StartOfMonth(ToDate) 
                 (from totalCOAType in

                      (from tot in QBasePeriodGLATotalByMonths_TotalDelta2End_FromBeginOfMonthFromDate_Til_BeginOfMonthToDate //// מצטברים מחודש כולל ועד חודש לא כולל
                       join a in QBaseAllCardsAndDetailsAccType
                       on tot.AccountId equals a.Id
                       select new
                       {
                           tot.AccountId,
                           //tot.CurrencyId, 
                           tot.ForeignAmountCredit,
                           tot.ForeignAmountDebit,
                           tot.LocalAmountCredit,
                           tot.LocalAmountDebit
                       }
                       )
                  group totalCOAType by new
                  {
                      totalCOAType.AccountId,
                      //totalCOAType.CurrencyId
                  } into g
                  select new TrailReportTemp
                  {

                      AccountId_COAType = g.Key.AccountId,
                      CurrencyId = "",//g.Key.CurrencyId,

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
                      TotalDelta2End_AnyActivity = g.Any(),
                      TransEnd_AnyActivity = null,

                  }
                  );
            return qAccumulateTotalsFromStartOfMonthFromTilStartOfMonthTo;
        }

        private IQueryable<TrailReportTemp> Init_LocalAmountOnly_TotalStart_JoinAccounts_GroupByAccount()
        {
            var qAccumulateTotalsFrom0BCTilNotIncludeStartOfMonthFromDate =
            //Accumulate Totals From the beginning of  Account Use Until(NotInclude) (StartOfMonth)FromDate                
            (from totalCOAType in

                 (
                 from tot in
                     QBasePeriodGLATotalByMonths_TotalStart_From0BC_TilNotInclude_BeginOfMonth_FromDate
                 join a in QBaseAllCardsAndDetailsAccType

                  on tot.AccountId equals a.Id
                 select new
                 {
                     tot.AccountId,
                     //tot.CurrencyId , 
                     tot.ForeignAmountCredit,
                     tot.ForeignAmountDebit,
                     tot.LocalAmountCredit,
                     tot.LocalAmountDebit
                 }
                  )
             group totalCOAType by new
             {
                 totalCOAType.AccountId,
                 //totalCOAType.CurrencyId
             } into g
             select new TrailReportTemp
             {

                 AccountId_COAType = g.Key.AccountId,
                 CurrencyId = "",//g.Key.CurrencyId,
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


                 TotalDelta2End_AnyActivity = null,
                 TransEnd_AnyActivity = null,

             }
             );
            return qAccumulateTotalsFrom0BCTilNotIncludeStartOfMonthFromDate;
        }

        private void SaveOld_QbaseTrailReportFull(IQueryable<TrailReportM> qMapAllCurrencySum2TRail)
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
                          GLAccountNumber= chartf.GLAccountNumber,
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
                 trailReportRow.ChartOfAcountCode1,
                 trailReportRow.ChartOfAcountCode2,
                 trailReportRow.ChartOfAcountCode3,
                 trailReportRow.ChartOfAcountCode4,
                 trailReportRow.ChartOfAcountCode5,

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

                 trailReportRow.ChartOfAccountId,
                 trailReportRow.GLAccountName,
                 trailReportRow.GLAccountNumber,
                 trailReportRow.GLAccountId,

                 trailReportRow.ChartOfAccountsTypeEnglish,
                 trailReportRow.ChartOfAccountsEnglish,
                 trailReportRow.GLAccountEnglish,

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
                         GLAccountName = gCOA.Key.GLAccountName,
                         GLAccountNumber= gCOA.Key.GLAccountNumber,
                         GLAccountId = gCOA.Key.GLAccountId,


                         ChartOfAcountName1English = gCOA.Key.ChartOfAcountName1English,
                         ChartOfAcountName2English = gCOA.Key.ChartOfAcountName2English,
                         ChartOfAcountName3English = gCOA.Key.ChartOfAcountName3English,
                         ChartOfAcountName4English = gCOA.Key.ChartOfAcountName4English,
                         ChartOfAcountName5English = gCOA.Key.ChartOfAcountName5English,

                         GLAccountEnglish = gCOA.Key.GLAccountEnglish,
                         ChartOfAccountsTypeEnglish = gCOA.Key.ChartOfAccountsTypeEnglish,
                         ChartOfAccountsEnglish = gCOA.Key.ChartOfAccountsEnglish,

                         CurrencyId = "",// gCOA.Key.CurrencyId,


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


                TotalDelta2End_AnyActivity = null,
                TransEnd_AnyActivity = null,
            }));
            return _QUnionAllMoneyData;
        }

    }
}

