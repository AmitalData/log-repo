using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Accounting.BL.CloseTables;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;

namespace Logitude.Accounting.BL.Utils
{
    public class InterestTransactionsCheckBRun
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;
        private List<string> _NoLines;
        private List<string> _WrongAction;
        //private List<string> _WrongSum;
        private List<string> _WrongSumToMatch;
        long _counter = 0;
        private const string WorksChartOfAccountTypeCode = "6";
        public const int LT_LinesMaximum_MIN = 2;
        public const int LT_LinesMaximum_MAX = 200;
        public const int MaxPageSize_MAX = 1000;
        public const int MaxGLAccountsPerQuery_Def = 100;
        public string SpecificJournalId = "";
        public string LastMadeGLAccountId = "";
        public int MaxGLAccountsPerQuery = 100;
        private List<string> badList;
        private List<string> goodList;
        private List<string> madeList;
        public InterestTransactionsCheckBRunResult MyInterestTransactionsCheckBRunResult = new InterestTransactionsCheckBRunResult();

        public InterestTransactionsCheckBRun()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }
        public void RunInterestTransactionsCheckB(InterestTransactionsCheckBArg interestTransactionsCheckBArg)
        {
            try
            {
                DateTime fromDate = DateTime.MinValue;
                DateTime oldDate = DateTime.MinValue;
                //  decimal oldAmount = Decimal.MaxValue;
                int tenant = interestTransactionsCheckBArg.Tenant;
                string myGLAccountId = interestTransactionsCheckBArg.GLAccountId;
                SpecificJournalId = interestTransactionsCheckBArg.SpecificJournalId;
                LastMadeGLAccountId = interestTransactionsCheckBArg.LastMadeGLAccountId;
                MaxGLAccountsPerQuery = interestTransactionsCheckBArg.MaxGLAccountsPerQuery;
                if (MaxGLAccountsPerQuery <= 0) MaxGLAccountsPerQuery = MaxGLAccountsPerQuery_Def;
                badList = new List<string>();
                goodList = new List<string>();
                madeList = new List<string>();
                _NoLines = new List<string>();
                _WrongAction = new List<string>();
                //   _WrongSum = new List<string>();
                _WrongSumToMatch = new List<string>();

                // DateTime myUpToDueDate = interestTransactionsCheckBArg.UpToDueDate;

                IAccountingContext context = AccountingContext.GetContext(tenant);
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(context);
                LedgerTransactionListQueryService ledgerTransactionListQueryService = new LedgerTransactionListQueryService(context);
                CheckDbIntegrity(myGLAccountId, interestTransactionsCheckBArg.AccountTypeCode, tenant);


                _ResponseText = $"Good: {MyInterestTransactionsCheckBRunResult.SuccessAccountLineCount},  Bad: {MyInterestTransactionsCheckBRunResult.BadAccountLineCount}, \n  Lines: \n{String.Join("\n", MyInterestTransactionsCheckBRunResult.ErrorRowList.ToArray())}";

            }

            catch (Exception e)
            {
                throw new Exception($"InterestTransactionsCheckBRun failure {e.Message} Inner Exception: {e.InnerException.Message}", e);
            }
        }


        private void AddErrorRowList(string gLAccount_id, string displayNumber, string lastReportNum, decimal accBlz, decimal repBlz, decimal diff, string message)
        {
            string separator = ";";
            this.AddErrorRow($"{gLAccount_id}{separator}{displayNumber}{separator}{lastReportNum}{separator}{accBlz.ToString()}{separator}{repBlz.ToString()}{separator}{diff.ToString()}{separator}{message}");
        }


        private void AddErrorRow(String errorLine)
        {
            this.MyInterestTransactionsCheckBRunResult.ErrorRowList.Add(errorLine);
            this.MyInterestTransactionsCheckBRunResult.BadAccountLineCount++;
        }





        public void CheckDbIntegrity(string gLAccountId, string accountTypeCode, int _Tenant)
        {
            int _ErrorCounter = 0;
            //      var sw = Stopwatch.StartNew();

            //       using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(10)))
            //       {
            var _AccountingContext = AccountingContext.GetContext(_Tenant);

            var myGLAccountRepo = new GLAccountRepository(_AccountingContext);
            var myGLAccountMoreDataRepo = new GLAccountMoreDataRepository(_AccountingContext);

            var myInterestReportRepo = new InterestReportRepository(_AccountingContext);

            var myInterestTransactionRepository = new InterestTransactionRepository(_AccountingContext);




            ////////////////
            /// 1. Compute BalanceInLocalCurrency

            var dbAllGLAccount =
                    (from acc in myGLAccountRepo.GetAll(_Tenant)   //.GetSingle(gLAccountId, _Tenant)
                     join md in myGLAccountMoreDataRepo.GetAll(_Tenant) on acc.Id equals md.AccountId
                     where acc.Tenant == _Tenant && acc.ActiveForInterest && acc.ParentAccountId == null
                     && (accountTypeCode == null || accountTypeCode == "" || acc.AccountTypeCode == accountTypeCode)
                     select new GLAccountMDatasDTO4Tester
                     {
                         AccountId = acc.Id,
                         // LocalName = acc.LocalName,
                         DisplayNumber = acc.DisplayNumber,
                         Tenant = acc.Tenant,
                         BalanceInLocalCurrency = md.BalanceInLocalCurrency,
                         //InterestOpenBalance = acc.InterestOpenBalance, // see 159870
                     }
                    );

            var test1 = dbAllGLAccount.ToList();

            var dbAllDescendantsOnly =
                (from aco in dbAllGLAccount
                 join descendants in myGLAccountRepo.GetAll(_Tenant) on aco.AccountId equals descendants.ParentAccountId
                 where aco.Tenant == _Tenant
                 select new GLAccountDescMDatasDTO4Tester()
                 {
                     ParentAccId = descendants.ParentAccountId,
                     DescAccId = descendants.Id,
                 });
            var test2 = dbAllDescendantsOnly.ToList();


            var dbAllDescendants =
                    (from aco in dbAllGLAccount
                     join descendants in myGLAccountRepo.GetAll(_Tenant) on aco.AccountId equals descendants.ParentAccountId
                     join md in myGLAccountMoreDataRepo.GetAll(_Tenant) on descendants.Id equals md.AccountId
                     into accOpenBalanceDescJoin
                     from aod in accOpenBalanceDescJoin
                     where aco.Tenant == _Tenant
                     group aod by aco.AccountId into g
                     select new GLAccountDescMDatasDTO4Tester2()
                     {
                         ParentAccId = g.Key,
                         DescBalanceInLocalCurrency = g.Sum(r => r.BalanceInLocalCurrency),
                         //DescInterestOpenBalance = g.Sum(r => r.InterestOpenBalance),
                     });
            var test3 = dbAllDescendants.ToList();

            var dbAllGLAccountInclDesc =
                     (
                        from acc in dbAllGLAccount
                        join descs in dbAllDescendants on acc.AccountId equals descs.ParentAccId
                        into accDescendantsJoin
                        from ad in accDescendantsJoin.DefaultIfEmpty()
                        select new GLAccountMDatasDTO4Tester2
                        {
                            AccountId = acc.AccountId,
                            //   LocalName = acc.LocalName,
                            DisplayNumber = acc.DisplayNumber,
                            //   Tenant = acc.Tenant,
                            BalanceInLocalCurrency = ad != null ? acc.BalanceInLocalCurrency + ad.DescBalanceInLocalCurrency : acc.BalanceInLocalCurrency,
                            //InterestOpenBalance = ad != null ? acc.InterestOpenBalance + ad.DescInterestOpenBalance : acc.InterestOpenBalance, 
                        }
                     );


            var test4 = dbAllGLAccountInclDesc.ToList();



            ////////////////
            /// 2. Compute InterestOpenBalances

            var calcInterestOpenBalances =
                     (
                        from intRep in
                            myInterestReportRepo.GetAll(_Tenant).Where(intRep => intRep.CreateDateTime.HasValue).OrderBy(intRep => intRep.CreateDateTime.Value).Where(intRep => (intRep.InterestReportStatusCode == "2" || intRep.InterestReportStatusCode == "4"))
                        where intRep.Tenant == _Tenant
                        join acc in dbAllGLAccountInclDesc on intRep.GLAccountId equals acc.AccountId
                        select new InterestReportDiff4Tester
                        {
                            AccountId = intRep.GLAccountId,
                            InterestOpenBalance = intRep.OpenBalance.HasValue ? intRep.OpenBalance.Value : 0m,
                            LastReportNum = intRep.ReportNumber,
                            DisplayNumber = acc.DisplayNumber,
                        }
                     );

            var test5 = calcInterestOpenBalances.ToList();
            string testDisplayNo = test5.FirstOrDefault().DisplayNumber;

            var calcInterestOpenBalancesDesc =
                     (
                        from intRep in
                            myInterestReportRepo.GetAll(_Tenant).Where(intRep => intRep.CreateDateTime.HasValue).OrderBy(intRep => intRep.CreateDateTime.Value).Where(intRep => (intRep.InterestReportStatusCode == "2" || intRep.InterestReportStatusCode == "4"))
                        where intRep.Tenant == _Tenant
                        join descendants in dbAllDescendantsOnly on intRep.GLAccountId equals descendants.DescAccId
                        select new InterestReportDiffDesc4Tester
                        {
                            ParentAccountId = descendants.ParentAccId,
                            DescInterestOpenBalance = intRep.OpenBalance.HasValue ? intRep.OpenBalance.Value : 0m,
                            LastReportNum = intRep.ReportNumber,
                        }
                     );


            var test6 = calcInterestOpenBalancesDesc.ToList();

            var calcInterestOpenBalancesInclDesc =
                     (
                        from acc in calcInterestOpenBalances
                        join descs in calcInterestOpenBalancesDesc on acc.AccountId equals descs.ParentAccountId
                        into accDescendantsJoin
                        from ad in accDescendantsJoin.DefaultIfEmpty()
                        select new InterestReportDiff4Tester2
                        {
                            AccountId = acc.AccountId,// ad.ParentAccountId,
                                                      //   LocalName = acc.LocalName,
                            DisplayNumber = acc.DisplayNumber,
                            //  Tenant = ad.Tenant,
                            InterestOpenBalance = ad != null ? acc.InterestOpenBalance + ad.DescInterestOpenBalance : acc.InterestOpenBalance,
                            LastReportNum = acc.LastReportNum,
                        }
                     );


            var test7 = calcInterestOpenBalancesInclDesc.ToList();  ////



            ////////////////
            /// 3.Compute FutureInterestTrans


            var calcFutureInterestTrans =
                     (
                        from intTrans in
                            myInterestTransactionRepository.GetAll(_Tenant).Where(intTrans => intTrans.InterestValueDate >= DateTime.Today.Date)
                        where intTrans.Tenant == _Tenant
                        join acc in dbAllGLAccountInclDesc on intTrans.GLAccountId equals acc.AccountId
                        group intTrans by intTrans.GLAccountId into g
                        select new InterestReportDiff4Tester3
                        {
                            AccountId = g.Key,
                            FutureInterestTransactionsBalance = g.Sum(r => r.LocalAmount),
                        }
                     );

            var test8 = calcFutureInterestTrans.ToList();

            var calcFutureInterestTransDesc =
                     (
                        from intTrans in
                            myInterestTransactionRepository.GetAll(_Tenant).Where(intTrans => intTrans.InterestValueDate >= DateTime.Today.Date)
                        where intTrans.Tenant == _Tenant
                        join descendants in dbAllDescendantsOnly on intTrans.GLAccountId equals descendants.DescAccId
                        group intTrans by intTrans.GLAccountId into g
                        select new InterestReportDiffDesc4Tester2
                        {
                            ParentAccountId = g.Key,
                            DescFutureInterestTransactionsBalance = g.Sum(r => r.LocalAmount),
                        }
                     );
            var test9 = calcFutureInterestTransDesc.ToList();

            var calcFutureInterestTransInclDesc =
                     (
                        from acc in calcFutureInterestTrans
                        join descs in calcFutureInterestTransDesc on acc.AccountId equals descs.ParentAccountId
                        into accDescendantsJoin
                        from ad in accDescendantsJoin.DefaultIfEmpty()
                        select new InterestReportDiff4Tester4
                        {
                            AccountId = acc.AccountId,
                            //   LocalName = acc.LocalName,
                            //   DisplayNumber = acc.DisplayNumber,
                            //    Tenant = acc.Tenant,
                            FutureInterestTransactionsBalance = ad != null ? acc.FutureInterestTransactionsBalance + ad.DescFutureInterestTransactionsBalance : acc.FutureInterestTransactionsBalance,
                            //       LastReportNum = acc.LastReportNum,
                        }
                     );

            var test10 = calcFutureInterestTransInclDesc.ToList();




            ////////////////
            /// 4. Assemble BalanceInLocalCurrency, InterestOpenBalances, FutureInterestTrans


            var qAccOpen = (
                    from acc in dbAllGLAccountInclDesc
                    join intRep in calcInterestOpenBalancesInclDesc on acc.AccountId equals intRep.AccountId
                    into accOpenBalanceJoin
                    from ao in accOpenBalanceJoin.DefaultIfEmpty()
                        //        where acc.Tenant == _Tenant
                    select new InterestReportDiff4Tester5
                    {
                        AccountId = acc.AccountId,
                        //   LocalName = acc.LocalName,
                        DisplayNumber = acc.DisplayNumber,
                        //    Tenant = acc.Tenant,
                        BalanceInLocalCurrency = acc.BalanceInLocalCurrency,
                        InterestOpenBalance = ao != null ? ao.InterestOpenBalance : 0m,
                        LastReportNum = ao.LastReportNum,
                    });
            var test11 = qAccOpen.ToList();

            var qAccOpenFuture = (
                    from aco in qAccOpen
                    join intTrans in calcFutureInterestTransInclDesc on aco.AccountId equals intTrans.AccountId
                    into accOpenBalanceFutureJoin
                    from aof in accOpenBalanceFutureJoin.DefaultIfEmpty()
                        //   where aco.Tenant == _Tenant
                    select new InterestReportDiff4Tester6
                    {
                        AccountId = aco.AccountId,
                        //   LocalName = aco.LocalName,
                        DisplayNumber = aco.DisplayNumber,
                        //   Tenant = aco.Tenant,
                        BalanceInLocalCurrency = aco.BalanceInLocalCurrency,
                        InterestOpenBalance = aco.InterestOpenBalance,
                        FutureInterestTransactionsBalance = aof != null ? aof.FutureInterestTransactionsBalance : 0m,
                        LastReportNum = aco.LastReportNum,
                    });
            var test12 = qAccOpenFuture.ToList();


            ////////////////
            /// 5. Compute Difference



            var qDiff = (
                    from acof in qAccOpenFuture
                    where
                    (acof.InterestOpenBalance + acof.FutureInterestTransactionsBalance - acof.BalanceInLocalCurrency >= 0.001m
                  || acof.InterestOpenBalance + acof.FutureInterestTransactionsBalance - acof.BalanceInLocalCurrency <= -0.001m)
                    select new InterestReportDiff4Tester7
                    {
                        AccountId = acof.AccountId,
                        //   LocalName = acof.LocalName,
                        DisplayNumber = acof.DisplayNumber,
                        //    Tenant = acof.Tenant,
                        BalanceInLocalCurrency = acof.BalanceInLocalCurrency,
                        InterestOpenBalance = acof.InterestOpenBalance,
                        FutureInterestTransactionsBalance = acof.FutureInterestTransactionsBalance,
                        CalculatedInterestBalance = acof.InterestOpenBalance + acof.FutureInterestTransactionsBalance,
                        Difference = acof.BalanceInLocalCurrency - acof.BalanceInLocalCurrency,
                        LastReportNum = acof.LastReportNum,
                    }
                    );

            var qThe = qDiff;

            List<InterestReportDiff4Tester7> resultingList = qThe.ToList();
            resultingList.ForEach(q =>
            {
                q.ErrorCounter = _ErrorCounter++;
                this.AddErrorRowList(q.AccountId, q.DisplayNumber, q.LastReportNum, q.BalanceInLocalCurrency, q.CalculatedInterestBalance, q.Difference, "");
            });

            // }
        }



        //private class ReconciableGroup
        //{
        //    public string _Acc { get; set; }
        //    public List<JournalLineReco> _LineGroup { get; set; }
        //    public ReconciableGroup(string reference, List<JournalLineReco> lineGroup)
        //    {
        //        _LineGroup = lineGroup;
        //        _Acc = reference;
        //    }
        //}


        //private class JournalLineReco
        //{

        //    public Decimal _valueToMatch { get; set; }
        //    public JournalLine _journalLine { get; set; }
        //    public LedgerTransaction _oneLineLedger { get; set; }
        //    public JournalLineReco(JournalLine journalLine, LedgerTransaction ledgerTransaction) //, decimal actualDifference)
        //    {
        //        this._journalLine = journalLine;
        //        this._valueToMatch = 0m;
        //        if (journalLine.ActionCode == "1")
        //        {
        //            //              this._valueToMatch = journalLine.LocalAmount + (journalLine.ExternalOpenAmount ?? 0m); // because in credit lines the ExternalOpenAmount is negative 
        //            //              this._valueToMatch = -(journalLine.LocalAmount + (journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 
        //            this._valueToMatch = ledgerTransaction.OpenAmount; ///+ (journalLine.ExternalOpenAmount ?? 0m)); // because in credit lines the ExternalOpenAmount is negative 
        //        }
        //        else if (journalLine.ActionCode == "2")
        //        {
        //            // this._valueToMatch = -(journalLine.LocalAmount - (journalLine.ExternalOpenAmount ?? 0m));
        //            //       this._valueToMatch = journalLine.LocalAmount - (journalLine.ExternalOpenAmount ?? 0m);
        //            this._valueToMatch = ledgerTransaction.OpenAmount; //+ (journalLine.ExternalOpenAmount ?? 0m); // because in credit lines the ExternalOpenAmount is negative 
        //        }
        //        else
        //        {
        //            this._valueToMatch = ledgerTransaction.OpenAmount;
        //        }
        //        this._oneLineLedger = ledgerTransaction; // ledgerTransactionQueryService.GetByJournalLineIdAndLine(journalLine.JournalId, journalLine.Line, journalLine.Tenant);
        //        this._oneLineLedger.AmountToReconcile = _valueToMatch;
        //    }
        //}

    }
    public class InterestTransactionsCheckBArg
    {
        public int Tenant { get; set; }
        public string GLAccountId { get; set; }

        public string AccountTypeCode { get; set; }

        // public DateTime UpToDueDate { get; set; }

        public int LT_LinesMaximum { get; set; }
        public int MaxPageSize { get; set; }

        public decimal MaximalDifference { get; set; }

        public string SpecificJournalId { get; set; }

        public string LastMadeGLAccountId { get; set; }
        public int MaxGLAccountsPerQuery { get; set; }

    }

    public class InterestTransactionsCheckBRunResult
    {
        public string LastMadeGLAccountId = "";
        public long SuccessAccountLineCount = 0;
        public long BadAccountLineCount = 0;
        public List<string> ErrorRowList = new List<string>();
    }

    public class InterestReportDiff4Tester
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //     public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }

    public class InterestReportDiff4Tester2
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //     public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }


    public class InterestReportDiff4Tester3
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //     public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }


    public class InterestReportDiff4Tester4
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //     public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }


    public class InterestReportDiff4Tester5
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //   public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }


    public class InterestReportDiff4Tester6
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //   public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }


    public class InterestReportDiff4Tester7
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //    public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
    }




    public class InterestReportDiffDesc4Tester
    {
        public string ParentAccountId { get; set; }
        public decimal DescInterestOpenBalance { get; set; }
        public decimal DescFutureInterestTransactionsBalance { get; set; }
        public string LastReportNum { get; set; }

    }

    public class InterestReportDiffDesc4Tester2
    {
        public string ParentAccountId { get; set; }
        public decimal DescInterestOpenBalance { get; set; }
        public decimal DescFutureInterestTransactionsBalance { get; set; }
        public string LastReportNum { get; set; }

    }


    public class GLAccountDescMDatasDTO4Tester
    {
        public string ParentAccId { get; set; }
        public string DescAccId { get; set; }
        public decimal DescBalanceInLocalCurrency { get; set; }
        public decimal DescInterestOpenBalance { get; set; }
    }

    public class GLAccountDescMDatasDTO4Tester2
    {
        public string ParentAccId { get; set; }
        public string DescAccId { get; set; }
        public decimal DescBalanceInLocalCurrency { get; set; }
        public decimal DescInterestOpenBalance { get; set; }
    }



    public class GLAccountMDatasDTO4Tester
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //     public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

    }

    public class GLAccountMDatasDTO4Tester2
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //   public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

    }


}