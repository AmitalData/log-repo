using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Reports
{
    public abstract class TrailReportBase : Logitude.Accounting.BL.CoreBL.Reports.ITrailReportBase
    {
        protected TrailReportParam _TrailReportParam = null;
        protected readonly int __TimeOutInMinutes = 129;
        //private System.Transactions.TransactionScope _TransactionScope;
        protected IAccountingContext _AccountingContext;
        private FullAccountingSettingPM _FullAccountingSetting;
        protected string _AccountingCurrencyId;
        protected IQueryable<AccountCOAM> QBaseAllCardsAndDetailsAccType;
        private IQueryable<ChartOfAccount5LevelM> _QAllChartOfAccountFlattenBy5LevelofHierarchy;

        protected IEnumerable //IQueryable
            <TrailReportM> _QBaseTrailReportFull = null;
        //protected DbContextBase.IDbContextLogger _DbLogger;




        DateTime _FromBeginOfMonth;

        DateTime _ToBeginOfMonth;

        protected IQueryable<GLAccountTotalByMonthsDTO>
            //מצטברים מתחילת חיי הכרטיסים עד תחילת החודש של FROMDATE לא כולל
            QBasePeriodGLATotalByMonths_TotalStart_From0BC_TilNotInclude_BeginOfMonth_FromDate;


        // מצטברים מתחילת חודש  מתאריך כולל ועד  תחילת חודש  עד לא כולל
        protected IQueryable<GLAccountTotalByMonthsDTO> QBasePeriodGLATotalByMonths_TotalDelta2End_FromBeginOfMonthFromDate_Til_BeginOfMonthToDate;

        // תנעות מכולל תחילת החודש  של מתאריך עד למתאריך -לא כולל    
        protected IQueryable<Data.EntityPOCOs.LedgerTransaction> QBasePeriodTransaction_TransStart_BeginOfMonthFromDate_TillFromDate_NotInclude;



        // תנעות מתחילת חודש אחרון כולל עד  תאריך הסיום + 1 לא כולל
        protected IQueryable<Data.EntityPOCOs.LedgerTransaction> QBasePeriodTransaction_TransEnd_BeginOfMonthToDate_Till_ToDateInculde;

        protected IQueryable<ChartOfAccount5LevelM> QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy;


        public TrailReportBase(TrailReportParam trailReportParam, int timeOutInMinutes)
        {
            _TrailReportParam = trailReportParam;
            __TimeOutInMinutes = timeOutInMinutes;

        }


        public List<TrailReportM> Execute()
        {
            _TrailReportParam.FromDate = _TrailReportParam.FromDate.Date;
            _TrailReportParam.ToDate = _TrailReportParam.ToDate.Date;
            _FromBeginOfMonth = new DateTime(_TrailReportParam.FromDate.Year, _TrailReportParam.FromDate.Month, 1);

            _ToBeginOfMonth = new DateTime(_TrailReportParam.ToDate.Year, _TrailReportParam.ToDate.Month, 1);




                _AccountingContext = AccountingContext.GetContext(_TrailReportParam.Tenant);
                //_DbLogger = (_AccountingContext as DbContextBase).CreateLogger();

                _FullAccountingSetting = //Hope From Cache
                    FullAccountingSettingQueryService
                    .Get(_TrailReportParam.Tenant);

                _AccountingCurrencyId = (new AccountingSettingResolver()).ResolveAccountingCurrencyId(_TrailReportParam.Tenant);

                GetGLAccountCardPopulationByParam();

                Create4MainQueriesPeriod();

                CreateQBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy();

                AdjustTrailReportFull();

                if (_QBaseTrailReportFull == null)
                {
                    throw new Exception("(_QBaseTrailReportFull==null)");
                }
                var myOutputReport = _QBaseTrailReportFull.ToList();
                var myTotalRow =
                    (from r in
                         myOutputReport
                     group r by 1 into g
                     select new TrailReportM()
                     {
                         ChartOfAcount1 = "Total",
                         LocalOpenBalance = g.Sum(r => r.LocalOpenBalance),
                         LocalDebit = g.Sum(r => r.LocalDebit),
                         LocalCredit = g.Sum(r => r.LocalCredit),
                         LocalCloseBalance = g.Sum(r => r.LocalCloseBalance),


                         ForeignOpenBalance = g.Sum(r => r.ForeignOpenBalance),
                         ForeignDebit = g.Sum(r => r.ForeignDebit),
                         ForeignCredit = g.Sum(r => r.ForeignCredit),
                         ForeignCloseBalance = g.Sum(r => r.ForeignCloseBalance),

                     }).FirstOrDefault();

                myOutputReport.Add(myTotalRow);


                DbLog = "";// _DbLogger.ToString();
                return myOutputReport;
        
        }
        private void CreateQBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy()
        {
            var qsChartOfAccount = new ChartOfAccountQueryService(_AccountingContext);
            _QAllChartOfAccountFlattenBy5LevelofHierarchy = //Flatten ChartOfAccount By 5 Level hierarchy
                qsChartOfAccount
                .GetQChartOfAccount5LevelM(_TrailReportParam.Tenant, null, null
                ///,_TrailReportParam.MyTrailReportLevel == TrailReportLevel.ChartofaccountType
                );
            QBaseAllCardsAndDetialsAccTypeBy5LevelHierarchy =
            JoinEachAccountWithHisChartOfAccount5hierarchy(QBaseAllCardsAndDetailsAccType);
        }

        private void Create4MainQueriesPeriod()
        {
            IQueryable<LedgerTransaction> qYearTransferLedgerTransaction = Enumerable.Empty<LedgerTransaction>().AsQueryable();
            if (_FromBeginOfMonth.Month == 1 && _FromBeginOfMonth.Day == 1)
            {
                
                var myLedgerTransactionRepository = new LedgerTransactionRepository(_AccountingContext);
                qYearTransferLedgerTransaction = myLedgerTransactionRepository
                    .GetYearTransferLedgerTransaction(null, _FromBeginOfMonth.Year, _TrailReportParam.Tenant);

            }

            var qYearTransferLedgerTransactionTotByMonth =
(from lt in qYearTransferLedgerTransaction
 group lt by new
 {
     lt.AccountId,
     lt.CurrencyId
 }
into groupBy_currency
 select new GLAccountTotalByMonthsDTO()
 //Data.EntityPOCOs.GLAccountTotalByMonth()
 {
     Tenant = _TrailReportParam.Tenant,
     AccountId = groupBy_currency.Key.AccountId,
     
     CurrencyId = groupBy_currency.Key.CurrencyId,

     Year = _FromBeginOfMonth.Year,
     Month = _FromBeginOfMonth.Month,
     


     LocalAmountDebit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.LocalAmountDebit),
     LocalAmountCredit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.LocalAmountCredit),

     ForeignAmountDebit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.ForeignAmountDebit),
     ForeignAmountCredit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.ForeignAmountCredit),

     CHANGE_TYPE="",
     DateTypeValue = GLAccountTotalDateTypeValues.Accountingdate,
 });


            

            QBasePeriodGLATotalByMonths_TotalStart_From0BC_TilNotInclude_BeginOfMonth_FromDate =
                 (from tot in _AccountingContext.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == GLAccountTotalDateTypeValues.Accountingdate)
                  where tot.Tenant == _TrailReportParam.Tenant
                  where tot.Year < _FromBeginOfMonth.Year ||
                  (tot.Year == _FromBeginOfMonth.Year &&
                     tot.Month < _FromBeginOfMonth.Month)
                  select
                  new GLAccountTotalByMonthsDTO()
                  {
                      Tenant = tot.Tenant,

                      AccountId = tot.AccountId,
                      CurrencyId = tot.CurrencyId,

                      Year = tot.Year,
                      Month = tot.Month,


                      LocalAmountDebit = tot.LocalAmountDebit,
                      LocalAmountCredit = tot.LocalAmountCredit,

                      ForeignAmountDebit = tot.ForeignAmountDebit,
                      ForeignAmountCredit = tot.ForeignAmountCredit,

                      CHANGE_TYPE = "",
                      DateTypeValue = tot.DateTypeCode
                  }
                     );
            QBasePeriodGLATotalByMonths_TotalStart_From0BC_TilNotInclude_BeginOfMonth_FromDate = 
                ( from a in 
                      QBasePeriodGLATotalByMonths_TotalStart_From0BC_TilNotInclude_BeginOfMonth_FromDate.Concat(qYearTransferLedgerTransactionTotByMonth)
                  group a by new { a.Tenant, a.AccountId, a.DateTypeValue, a.Year,  a.Month, a.CurrencyId }
                  into groupBy_currency
                  select new GLAccountTotalByMonthsDTO()
                  {
                      Tenant = groupBy_currency.Key.Tenant,
                      AccountId = groupBy_currency.Key.AccountId,
                      CurrencyId = groupBy_currency.Key.CurrencyId,

                      Year = groupBy_currency.Key.Year,
                      Month = groupBy_currency.Key.Month,
                      


                      LocalAmountDebit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.LocalAmountDebit),
                      LocalAmountCredit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.LocalAmountCredit),

                      ForeignAmountDebit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.ForeignAmountDebit),
                      ForeignAmountCredit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.ForeignAmountCredit),

                      CHANGE_TYPE="",
                      DateTypeValue = groupBy_currency.Key.DateTypeValue,
                  }
                  );

            bool testIt = false;
            if (testIt)
            {
                var res = QBasePeriodGLATotalByMonths_TotalStart_From0BC_TilNotInclude_BeginOfMonth_FromDate.ToList();
            }


                QBasePeriodGLATotalByMonths_TotalDelta2End_FromBeginOfMonthFromDate_Til_BeginOfMonthToDate =
                (from tot in _AccountingContext.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode == GLAccountTotalDateTypeValues.Accountingdate)
                 where tot.Tenant == _TrailReportParam.Tenant


                 where tot.Year > _FromBeginOfMonth.Year ||
             //(tot.Year == _FromBeginOfMonth.Year && tot.Month > _FromBeginOfMonth.Month)
             (tot.Year == _FromBeginOfMonth.Year && tot.Month >= _FromBeginOfMonth.Month)

                 where tot.Year < _ToBeginOfMonth.Year ||
                 (tot.Year == _ToBeginOfMonth.Year && tot.Month < _ToBeginOfMonth.Month)
                 select
                 new GLAccountTotalByMonthsDTO()
                 {
                     Tenant = tot.Tenant,

                     AccountId = tot.AccountId,
                     CurrencyId = tot.CurrencyId,

                     Year = tot.Year,
                     Month = tot.Month,


                     LocalAmountDebit = tot.LocalAmountDebit,
                     LocalAmountCredit = tot.LocalAmountCredit,

                     ForeignAmountDebit = tot.ForeignAmountDebit,
                     ForeignAmountCredit = tot.ForeignAmountCredit,

                     CHANGE_TYPE = "",
                     DateTypeValue = tot.DateTypeCode
                 }
                 );

            var qYearTransferLedgerMinus_4TotalDelta2End =
         qYearTransferLedgerTransactionTotByMonth.Select(r => new GLAccountTotalByMonthsDTO()
         {
             Tenant = r.Tenant,
             AccountId = r.AccountId,

             CurrencyId = r.CurrencyId,

             Year = _FromBeginOfMonth.Year,
             Month = _FromBeginOfMonth.Month,



             LocalAmountDebit = -1 * r.LocalAmountDebit,
             LocalAmountCredit = -1 * r.LocalAmountCredit,

             ForeignAmountDebit = -1 * r.ForeignAmountDebit,
             ForeignAmountCredit = -1 * r.ForeignAmountCredit,

             CHANGE_TYPE = "",
             DateTypeValue = r.DateTypeValue,

         });

            QBasePeriodGLATotalByMonths_TotalDelta2End_FromBeginOfMonthFromDate_Til_BeginOfMonthToDate =
                (from a in
                     QBasePeriodGLATotalByMonths_TotalDelta2End_FromBeginOfMonthFromDate_Til_BeginOfMonthToDate.Concat(qYearTransferLedgerMinus_4TotalDelta2End)
                 group a by new { a.Tenant, a.AccountId, a.DateTypeValue, a.Year, a.Month, a.CurrencyId }
                  into groupBy_currency
                 select new GLAccountTotalByMonthsDTO()
                 {
                     Tenant = groupBy_currency.Key.Tenant,
                     AccountId = groupBy_currency.Key.AccountId,
                     CurrencyId = groupBy_currency.Key.CurrencyId,

                     Year = groupBy_currency.Key.Year,
                     Month = groupBy_currency.Key.Month,



                     LocalAmountDebit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.LocalAmountDebit),
                     LocalAmountCredit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.LocalAmountCredit),

                     ForeignAmountDebit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.ForeignAmountDebit),
                     ForeignAmountCredit = groupBy_currency.Sum(ltYearTransfer => ltYearTransfer.ForeignAmountCredit),

                     CHANGE_TYPE = "",
                     DateTypeValue = groupBy_currency.Key.DateTypeValue,
                 }
                  );



            // תנעות מכולל תחילת החודש  של מתאריך עד למתאריך -לא כולל    
            QBasePeriodTransaction_TransStart_BeginOfMonthFromDate_TillFromDate_NotInclude = (
                from trans in _AccountingContext.LedgerTransactions
                where trans.Tenant == _TrailReportParam.Tenant
                //fromBeginOfMonth:20160101 until (Not INclude)_TrailReportParam.FromDate:20160113

                where trans.AccountingDate >= _FromBeginOfMonth
                where trans.AccountingDate < _TrailReportParam.FromDate//Not INclude 
                select trans
                   );

          


            var toDateAdd1Day = _TrailReportParam.ToDate.AddDays(1);//INclude //
            QBasePeriodTransaction_TransEnd_BeginOfMonthToDate_Till_ToDateInculde =// תנעות מתחילת חודש אחרון כולל עד  תאריך הסיום + 1 לא כולל
                (
                from trans in _AccountingContext.LedgerTransactions
                where trans.Tenant == _TrailReportParam.Tenant
                //toBeginOfMonth:20160201 until (InculdeAllTransOf)_TrailReportParam.ToDate:20160215
                where trans.AccountingDate >= _ToBeginOfMonth  //20160201
                where trans.AccountingDate <
                toDateAdd1Day //_TrailReportParam.ToDate.AddDays(1)//INclude //==20160216 
                select trans
                );

        }

        private GLAccountTotalByMonthsDTO toDTO(GLAccountTotalByMonth tot)
        {
            return new GLAccountTotalByMonthsDTO()
            {
                Tenant = tot.Tenant,

                AccountId = tot.AccountId,
                CurrencyId = tot.CurrencyId,

                Year = tot.Year,
                Month = tot.Month,


                LocalAmountDebit = tot.LocalAmountDebit,
                LocalAmountCredit = tot.LocalAmountCredit,

                ForeignAmountDebit = tot.ForeignAmountDebit,
                ForeignAmountCredit = tot.ForeignAmountCredit,

                CHANGE_TYPE = "",
                DateTypeValue = tot.DateTypeCode
            };
        }

        private void GetGLAccountCardPopulationByParam()
        {
            var repoGLAccount = new GLAccountRepository(_AccountingContext);
            var myQBaseAllCardsAndDetailsAccType = //Get The Account List
                repoGLAccount.
                GetQAllCardsAndDetailsAccType(
                _TrailReportParam.Tenant,
                GetClientContolAcc(_FullAccountingSetting),
                GetVendorContolAcc(_FullAccountingSetting),
                GetJobContolAcc(_FullAccountingSetting),
                GetFileContolAcc(_FullAccountingSetting))
                //.Where(a => !a.Inactive)
                ;
            if (_TrailReportParam.ChartOfAccountsIdList.Count > 0)
            {
                myQBaseAllCardsAndDetailsAccType = myQBaseAllCardsAndDetailsAccType
                    .Where(r => _TrailReportParam.ChartOfAccountsIdList.Contains(r.ChartOfAccountsId));
            }
            if (_TrailReportParam.ChartOfAccountsTypeCodeList.Count > 0)
            {
                myQBaseAllCardsAndDetailsAccType = myQBaseAllCardsAndDetailsAccType
                    .Where(r => _TrailReportParam.ChartOfAccountsTypeCodeList.Contains(r.ChartOfAccountsTypeCode));
            }

            if (!String.IsNullOrWhiteSpace(_TrailReportParam.Category1))
            {
                myQBaseAllCardsAndDetailsAccType = myQBaseAllCardsAndDetailsAccType
                    .Where(a => a.Category1Id == _TrailReportParam.Category1);
            }
            if (!String.IsNullOrWhiteSpace(_TrailReportParam.Category2))
            {
                myQBaseAllCardsAndDetailsAccType = myQBaseAllCardsAndDetailsAccType
                    .Where(a => a.Category2Id == _TrailReportParam.Category2);
            }
            if (!String.IsNullOrWhiteSpace(_TrailReportParam.Category3))
            {
                myQBaseAllCardsAndDetailsAccType = myQBaseAllCardsAndDetailsAccType
                    .Where(a => a.Category3Id == _TrailReportParam.Category3);
            }
            if (!String.IsNullOrWhiteSpace(_TrailReportParam.Category4))
            {
                myQBaseAllCardsAndDetailsAccType = myQBaseAllCardsAndDetailsAccType
                    .Where(a => a.Category4Id == _TrailReportParam.Category4);
            }
            if (!String.IsNullOrWhiteSpace(_TrailReportParam.Category5))
            {
                myQBaseAllCardsAndDetailsAccType = myQBaseAllCardsAndDetailsAccType
                    .Where(a => a.Category5Id == _TrailReportParam.Category5);
            }


            QBaseAllCardsAndDetailsAccType = (
                from a in myQBaseAllCardsAndDetailsAccType
                select new AccountCOAM //Made 4 Short(Projoction) +Algant+Fast SQL
                {
                    Id = a.Id,
                    Tenant = a.Tenant,
                    AccountTypeCode = a.AccountTypeCode,
                    EnglishName = a.EnglishName,
                    ChartOfAccountsTypeCode = a.ChartOfAccountsTypeCode,
                    ChartOfAccountsId = a.ChartOfAccountsId,
                    IsControlAccount = a.IsControlAccount,
                    ParentId = a.ParentAccountId,
                    DisplayNumber = a.DisplayNumber,
                    LocalName = a.LocalName,
                    CurrencyId = a.CurrencyId,
                    IsMultiCurrency = a.IsMultiCurrency,

                }
                );
        }

        protected abstract void AdjustTrailReportFull();


        private IQueryable<ChartOfAccount5LevelM> JoinEachAccountWithHisChartOfAccount5hierarchy(IQueryable<AccountCOAM> qAllCardsAndDetialsAccType)
        {
            IQueryable<ChartOfAccount5LevelM> qAllCardsAndDetialsAccTypeBy5LevelHierarchy =
                //Join Each Account With is ChartOfAccount 5 hierarchy
            (from aGL in qAllCardsAndDetialsAccType
             join chart in _QAllChartOfAccountFlattenBy5LevelofHierarchy
             on aGL.ChartOfAccountsId  //ChartOfAccountsId  is must (not null)
             equals chart.LeafId
             //into groupJoin
             select new ChartOfAccount5LevelM()
             {
                 Tenant = aGL.Tenant,


                 Level1Id = chart.Level1Id,
                 Level1Name = chart.Level1Name,
                 Level1Code =  chart.Level1Code,
                 Level1English = chart.Level1English,


                 Level2Id = chart.Level2Id,
                 Level2Name = chart.Level2Name,
                 Level2Code = chart.Level2Code,
                 Level2English = chart.Level2English,

                 Level3Id = chart.Level3Id,
                 Level3Name = chart.Level3Name,
                 Level3Code = chart .Level3Code,
                 Level3English = chart.Level3English,

                 Level4Id = chart.Level4Id,
                 Level4Name = chart.Level4Name,
                 Level4Code = chart.Level4Code,
                 Level4English = chart.Level4English,

                 Level5Id = chart.Level5Id,
                 Level5Name = chart.Level5Name,
                 Level5Code  = chart.Level5Code,
                 Level5English = chart.Level5English,


                 GLAccountId = aGL.Id,
                 GLAccountName = aGL.LocalName,
                 GLAccountNumber = aGL.DisplayNumber,
               
                 ChartOfAccountId=aGL.ChartOfAccountsId,
                 ChartOfAccountTypeCode = //aGL.AccountTypeCode,
                 aGL.ChartOfAccountsTypeCode,

                 ChartOfAccountsTypeEnglish = chart.ChartOfAccountsTypeEnglish,
                 ChartOfAccountsEnglish = chart.ChartOfAccountsEnglish,
                 GLAccountEnglish = chart.GLAccountEnglish,

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
            List<string> controlAccountId_list = new List<string>() ;
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



        //public void Dispose()
        //{
        //    //_DbLogger.Dispose();
        //    //_TransactionScope.Dispose();
        //}

        public string DbLog { get; set; }

        internal bool NotUsingControlAccount()
        {
            return (
                _TrailReportParam.DetailedControlClients==true &&
                _TrailReportParam.DetailedControlFile == true &&
                _TrailReportParam.DetailedControlJob == true &&
                _TrailReportParam.DetailedControlVendors == true
                );
        }
    }

    public static class TrailReportFactory
    {
        public static ITrailReportBase CreateNew(TrailReportParam trailReportParam)
        {

           
            if (ReportLevel.GLAccount != trailReportParam.MyTrailReportLevel && ! trailReportParam.Skip)
            {
               


                if (trailReportParam.DetailedControlClients
                    ||
                    trailReportParam.DetailedControlFile
                    ||
                    trailReportParam.DetailedControlJob
                    ||
                    trailReportParam.DetailedControlVendors
                    ||
                    trailReportParam.Suppress_DoNotShowCardWithoutActivity
                    ||
                    trailReportParam.DoNotShowCardWithLocalCloseBalanceEqualZero
                    )
                {
                    throw new Exception("Only in TrailReportLevel.GLAccount DetailedControl is allowed !!!");
                }
                if (!string.IsNullOrWhiteSpace(trailReportParam.Category1)
                    ||
                    !string.IsNullOrWhiteSpace(trailReportParam.Category2)
                    ||
                    !string.IsNullOrWhiteSpace(trailReportParam.Category3)
                    ||
                    !string.IsNullOrWhiteSpace(trailReportParam.Category4)
                    ||
                    !string.IsNullOrWhiteSpace(trailReportParam.Category5)

                    )
                {
                    throw new Exception("Only in TrailReportLevel.GLAccount Fillter by Category is allowed !!!");
                }


            }

            if (
trailReportParam.ChartOfAccountsIdList.Count > 0
&&
trailReportParam.ChartOfAccountsTypeCodeList.Count > 0
)
            {
                throw new Exception("אנחנו נאפשר למשתמש להשתמש רק באחד מבין 2 הפילטרים החדשים!!!"
                    + Environment.NewLine
                    + " ChartOfAccountsIdList/ChartOfAccountsTypeCodeList "
                    );
            }

            switch (trailReportParam.MyTrailReportLevel)
            {
                case ReportLevel.ChartofaccountType:
                    if (trailReportParam.ChartOfAccountsIdList.Count > 0)
                    {
                       // throw new Exception("in  ChartofaccountType level - ChartOfAccountsIdList  is not  allowed !!!");
                    }
                    if (trailReportParam.ChartOfAccountsTypeCodeList.Count > 0)
                    {
                        // throw new Exception("in  ChartofaccountType level - ChartOfAccountsTypeCodeList  is not  allowed !!!");
                    }

                    break;
                case ReportLevel.Chartofaccount:
                    if (trailReportParam.ChartOfAccountsIdList.Count > 0)
                    {
                        ///throw new Exception("in  ChartofaccountType level - ChartOfAccountsIdList  is not  allowed !!!");
                    }

                    break;
                case ReportLevel.GLAccount:
                default:
                    break;
            }

            switch (trailReportParam.MyTrailReportLevel)
            {
                case ReportLevel.ChartofaccountType:
                    if (trailReportParam.CurrenciesDetailed)
                    {
                        return new TrailReportChartofaccountTypeCurrenciesDetailed(trailReportParam, 30);
                    }
                    else
                    {
                        return new TrailReportChartofaccountType(trailReportParam, 30);
                    }
                    break;
                case ReportLevel.Chartofaccount:
                    if (trailReportParam.CurrenciesDetailed)
                    {

                        return new TrailReportChartofaccountCurrenciesDetailed(trailReportParam, 120);
                    }
                    else
                    {

                        return new TrailReportChartofaccount(trailReportParam, 90);

                    }
                    break;
                case ReportLevel.GLAccount:
                default:
                    if (trailReportParam.CurrenciesDetailed)
                    {
                        return new TrailReportGLAccountCurrenciesDetailed(trailReportParam, 130);

                    }
                    else
                    {
                        return new TrailReportGLAccount(trailReportParam, 130);
                    }
                    break;
                
                    break;
            }

        }
    }

    public class AccountCOAM
    {

        public string Id { get; set; }

        public string ChartOfAccountsTypeCode { get; set; }

        public string ChartOfAccountsId { get; set; }

        public int Tenant { get; set; }

        public string AccountTypeCode { get; set; }

        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public bool? IsControlAccount { get; set; }
        public string DisplayNumber { get; set; }
        public string ParentId { get; set; }
        public string CurrencyId { get;  set; }
        public bool? IsMultiCurrency { get;  set; }
    }
}
