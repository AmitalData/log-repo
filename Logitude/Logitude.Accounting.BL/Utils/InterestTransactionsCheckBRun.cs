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


        private void AddErrorRowList(string gLAccount_id, string displayNumber, string lastReportNum, 
            decimal glaccblz, decimal descblz, decimal accBlz, 
            decimal repBlz, 
            decimal before, string startdate, decimal inbetween, string repdate,
            decimal glaccftr, decimal descftr,
            decimal calc, decimal diff, string message)
        {
            string sep = ";";
            this.AddErrorRow($"{gLAccount_id}{sep}{displayNumber}{sep}{lastReportNum}{sep}{glaccblz}{sep}{descblz}{sep}{accBlz}{sep}{repBlz}{sep}{before}{sep}{startdate}{sep}{inbetween}{sep}{repdate}{sep}{glaccftr}{sep}{descftr}{sep}{calc}{sep}{diff}{sep}{message}");
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
                     && (gLAccountId == null || gLAccountId == "" || acc.Id == gLAccountId)
                     && (accountTypeCode == null || accountTypeCode == "" || acc.AccountTypeCode == accountTypeCode)
                     select new GLAccountMDatasDTO4Tester
                     {
                         AccountId = acc.Id,
                         // LocalName = acc.LocalName,
                         DisplayNumber = acc.DisplayNumber,
                         Tenant = acc.Tenant,
                         BalanceInLocalCurrency = md.BalanceInLocalCurrency, // GLAccount balance
                         //IntReportCloseBalance = acc.IntReportCloseBalance, // see 159870
                        InterestCalculationStartDate = acc.InterestCalculationStartDate,
    }
                    );

            var test1 = dbAllGLAccount.ToList();

            var dbAllDescendantsOnly =
                (from aco in dbAllGLAccount
                 join descendants in myGLAccountRepo.GetAll(_Tenant).Where(desc => desc.ActiveForInterest) on aco.AccountId equals descendants.ParentAccountId
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
                         DescBalanceInLocalCurrency = g.Sum(r => r.BalanceInLocalCurrency), // descendants' balance  
                         //DescIntReportCloseBalance = g.Sum(r => r.IntReportCloseBalance),
                     });
            var test3 = dbAllDescendants.ToList();

            var dbAllGLAccountInclDesc =
                     (
                        from acc in dbAllGLAccount
                        join descs in dbAllDescendants on acc.AccountId equals descs.ParentAccId
                        into accDescendantsJoin
                        from descs in accDescendantsJoin.DefaultIfEmpty()
                        select new GLAccountMDatasDTO4Tester2
                        {
                            AccountId = acc.AccountId,
                            //   LocalName = acc.LocalName,
                            DisplayNumber = acc.DisplayNumber,
                            //   Tenant = acc.Tenant,
                            GLAccountBalance = acc.BalanceInLocalCurrency,
                            DescBalance = descs != null ? descs.DescBalanceInLocalCurrency : 0m,
                            BalanceInLocalCurrency = descs != null ? acc.BalanceInLocalCurrency + descs.DescBalanceInLocalCurrency : acc.BalanceInLocalCurrency, // GLAccount and descendants 
                                                                                                                                                                 //IntReportCloseBalance = descs != null ? acc.IntReportCloseBalance + ad.DescIntReportCloseBalance : acc.IntReportCloseBalance, 
                            InterestCalculationStartDate = acc.InterestCalculationStartDate,

                        }
                     );


            var test4 = dbAllGLAccountInclDesc.ToList();



            ////////////////
            /// 2. Compute IntReportCloseBalances

            var calcIntReportCloseBalances =
                 (
                    from intRep in
                     myInterestReportRepo.GetAll(_Tenant)//.Where(intRep => intRep.InterestCalculationDate) //.OrderBy(intRep => intRep.InterestCalculationDate.Value)
                        .Where(intRep => (intRep.InterestReportStatusCode == "2" || intRep.InterestReportStatusCode == "4"))

                    where intRep.Tenant == _Tenant
                    join acc in dbAllGLAccountInclDesc on intRep.GLAccountId equals acc.AccountId
                    select new InterestReportDiff4Tester
                    {
                        AccountId = intRep.GLAccountId,
                        IntReportCloseBalance = intRep.CloseBalance.HasValue ? intRep.CloseBalance.Value : 0m, // last report open balance - of the GLAccount
                        LastReportNum = intRep.ReportNumber,
                        DisplayNumber = acc.DisplayNumber,
                        InterestCalculationDate = intRep.InterestCalculationDate,
                        InterestCalculationStartDate = acc.InterestCalculationStartDate,

                    }
                 );


            var test5 = calcIntReportCloseBalances.ToList();







            ////////////////
            /// 4. Assemble BalanceInLocalCurrency, IntReportCloseBalances, FutureInterestTrans


            var qAccOpen = (
                    from acc in dbAllGLAccountInclDesc
                    join intRep in calcIntReportCloseBalances on acc.AccountId equals intRep.AccountId
                    into accOpenBalanceJoin
                    from intRep in accOpenBalanceJoin.DefaultIfEmpty()
                        //        where acc.Tenant == _Tenant
                    select new InterestReportDiff4Tester5
                    {
                        AccountId = acc.AccountId,
                        //   LocalName = acc.LocalName,
                        DisplayNumber = acc.DisplayNumber,
                        //    Tenant = acc.Tenant,
                        BalanceInLocalCurrency = acc.BalanceInLocalCurrency,
                        IntReportCloseBalance = intRep != null ? intRep.IntReportCloseBalance : 0m,
                        LastReportNum = intRep != null ? intRep.LastReportNum : null,
                        DescBalance = acc.DescBalance,
                        GLAccountBalance = acc.GLAccountBalance, 
                        InterestCalculationDate = intRep != null ? intRep.InterestCalculationDate:null,
                        InterestCalculationStartDate = acc.InterestCalculationStartDate,

                    }
                    );

            var test11 = qAccOpen.ToList();
            var test11_bis = test11.GroupBy(t => t.AccountId).Select(g => g.OrderByDescending(t => t.InterestCalculationDate).First());



            ////////////
            /// 2.5. Compute "Before"

            var calcBeforeInterestTrans =
                     (
                        from acc in test11_bis
                        from
                        intTrans in myInterestTransactionRepository.GetAll(_Tenant)
                            .Where(intTrans => intTrans.GLAccountId == acc.AccountId 
                            && !intTrans.IsClosed 
                            && intTrans.InterestValueDate < acc.InterestCalculationStartDate)
                        group intTrans by intTrans.GLAccountId into g
                        select new InterestReportDiff4Tester3
                        {
                            AccountId = g.Key,
                            BeforeInterestTransactionsBalance = g.Sum(r => r.LocalAmount), // of the GLAccount
                            InbetweenInterestTransactionsBalance = 0m,
                            FutureInterestTransactionsBalance = 0m,
                        }
                     );

            var test_bef = calcBeforeInterestTrans.ToList();




            /// 2.6. Compute "Inbetween"

            var calcInbetweenInterestTrans =
                     (
                        from acc in test11_bis
                        from
                        intTrans in myInterestTransactionRepository.GetAll(_Tenant)
                            .Where(intTrans => intTrans.GLAccountId == acc.AccountId
                               && !intTrans.IsClosed
                               && intTrans.InterestValueDate >= acc.InterestCalculationStartDate
                               && intTrans.InterestValueDate <= acc.InterestCalculationDate)
                        group intTrans by intTrans.GLAccountId into g
                        select new InterestReportDiff4Tester3
                        {
                            AccountId = g.Key,
                            BeforeInterestTransactionsBalance = 0m,
                            InbetweenInterestTransactionsBalance = g.Sum(r => r.LocalAmount), // of the GLAccount
                            FutureInterestTransactionsBalance = 0m,
                        });

            var test_inbetw = calcInbetweenInterestTrans.ToList();





            ////////////////
            /// 3.Compute FutureInterestTrans


            var calcFutureInterestTrans =
                     (
                        //from intTrans in
                        //   myInterestTransactionRepository.GetAll(_Tenant).Where(intTrans => intTrans.InterestCalculationDate >= DateTime.Today.Date)
                        //where intTrans.Tenant == _Tenant
                        //join acc in dbAllGLAccountInclDesc on intTrans.GLAccountId equals acc.AccountId


                        from acc in test11_bis
                        from
                        intTrans in myInterestTransactionRepository.GetAll(_Tenant)
                            .Where(intTrans => intTrans.GLAccountId == acc.AccountId && !intTrans.IsClosed && intTrans.InterestValueDate > acc.InterestCalculationDate)
                        group intTrans by intTrans.GLAccountId into g
                        select new InterestReportDiff4Tester3
                        {
                            AccountId = g.Key,
                            BeforeInterestTransactionsBalance = 0m,
                            InbetweenInterestTransactionsBalance = 0m,
                            FutureInterestTransactionsBalance = g.Sum(r => r.LocalAmount), // of the GLAccount
                        }
                     );

            var test8 = calcFutureInterestTrans.ToList();



            var dbAllDescendantsOnly_1 =
            (from aco in test11_bis
             join descendants in myGLAccountRepo.GetAll(_Tenant).Where(desc => desc.ActiveForInterest) on aco.AccountId equals descendants.ParentAccountId
             where aco.Tenant == _Tenant
             select new GLAccountDescMDatasDTO4Tester()
             {
                 ParentAccId = descendants.ParentAccountId,
                 DescAccId = descendants.Id,
                 InterestCalculationDate = aco.InterestCalculationDate.Value,
             });
            var test2_1 = dbAllDescendantsOnly_1.ToList();







            var calcFutureInterestTransDesc =
                     (
                        //from intTrans in
                        //    myInterestTransactionRepository.GetAll(_Tenant).Where(intTrans => intTrans.InterestCalculationDate >= DateTime.Today.Date)
                        //where intTrans.Tenant == _Tenant
                        //join descendants in dbAllDescendantsOnly on intTrans.GLAccountId equals descendants.DescAccId

                        from descendants in dbAllDescendantsOnly_1
                        from
                        intTrans in myInterestTransactionRepository.GetAll(_Tenant)
                            .Where(intTrans => intTrans.GLAccountId == descendants.DescAccId && intTrans.InterestValueDate > descendants.InterestCalculationDate)
                        select new InterestReportDiffDesc4Tester1_5
                        {
                            ParentAccountId = descendants.ParentAccId,
                            LocalAmount = intTrans.LocalAmount,
                        }
                        into grs
                        group grs by grs.ParentAccountId into g
                        select new InterestReportDiffDesc4Tester2
                        {
                            ParentAccountId = g.Key,
                            DescFutureInterestTransactionsBalance = g.Sum(r => r.LocalAmount), // of the descendants
                        }
                     );
            var test9 = calcFutureInterestTransDesc.ToList();

            var calcFutureInterestTransInclDesc =
                     (
                        from acc in calcFutureInterestTrans
                        join descs in calcFutureInterestTransDesc on acc.AccountId equals descs.ParentAccountId
                        into accDescendantsJoin
                        from descs in accDescendantsJoin.DefaultIfEmpty()
                        select new InterestReportDiff4Tester4
                        {
                            AccountId = acc.AccountId,
                            //   LocalName = acc.LocalName,
                            //   DisplayNumber = acc.DisplayNumber,
                            //    Tenant = acc.Tenant,
                            FutureInterestTransactionsBalance = descs != null ? acc.FutureInterestTransactionsBalance + descs.DescFutureInterestTransactionsBalance : acc.FutureInterestTransactionsBalance,
                            GLAccountFutureBalance = acc.FutureInterestTransactionsBalance,
                            DescFutureBalance = descs != null ? descs.DescFutureInterestTransactionsBalance : 0m,

                            //       LastReportNum = acc.LastReportNum,
                        }
                     );

            var test10 = calcFutureInterestTransInclDesc.ToList();





            var qAccOpenFuture_b = (
        from aco in test11_bis
        join intTrans in calcBeforeInterestTrans on aco.AccountId equals intTrans.AccountId
        into accOpenBalanceBeforeJoin
        from intTrans in accOpenBalanceBeforeJoin.DefaultIfEmpty()
                    select new InterestReportDiff4Tester6
        {
            AccountId = aco.AccountId,
                        DisplayNumber = aco.DisplayNumber,
                        BalanceInLocalCurrency = aco.BalanceInLocalCurrency,
            IntReportCloseBalance = aco.IntReportCloseBalance,
            FutureInterestTransactionsBalance = 0m,
            LastReportNum = aco.LastReportNum,
            DescBalance = aco.DescBalance,
            GLAccountBalance = aco.GLAccountBalance,
            GLAccountFutureBalance = 0m,
            DescFutureBalance = 0m,
            InterestCalculationStartDate = aco.InterestCalculationStartDate,
            InterestCalculationDate = aco.InterestCalculationDate,
            BalanceBeforeStart = intTrans != null ? intTrans.BeforeInterestTransactionsBalance : 0m, 
            BalanceInbetween = 0m,
                    });
            var test12_b = qAccOpenFuture_b.ToList();


            var qAccOpenFuture_i = (
        from aco in qAccOpenFuture_b
        join intTrans in calcInbetweenInterestTrans on aco.AccountId equals intTrans.AccountId
        into accOpenBalanceFutureJoin
        from intTrans in accOpenBalanceFutureJoin.DefaultIfEmpty()
                    select new InterestReportDiff4Tester6
        {
            AccountId = aco.AccountId,
                        DisplayNumber = aco.DisplayNumber,
                        BalanceInLocalCurrency = aco.BalanceInLocalCurrency,
            IntReportCloseBalance = aco.IntReportCloseBalance,
            FutureInterestTransactionsBalance = 0m,
            LastReportNum = aco.LastReportNum,
            DescBalance = aco.DescBalance,
            GLAccountBalance = aco.GLAccountBalance,
            GLAccountFutureBalance = 0m,
            DescFutureBalance = 0m,
            InterestCalculationStartDate = aco.InterestCalculationStartDate,
            InterestCalculationDate = aco.InterestCalculationDate,
            BalanceBeforeStart = aco.BalanceBeforeStart,
            BalanceInbetween = intTrans != null ? intTrans.InbetweenInterestTransactionsBalance : 0m,
                    });
            var test12_i = qAccOpenFuture_i.ToList();



            var qAccOpenFuture_f = (
                    from aco in qAccOpenFuture_i
                    join intTrans in calcFutureInterestTransInclDesc on aco.AccountId equals intTrans.AccountId
                    into accOpenBalanceFutureJoin
                    from intTrans in accOpenBalanceFutureJoin.DefaultIfEmpty()
                    select new InterestReportDiff4Tester6
                    {
                        AccountId = aco.AccountId,
                        DisplayNumber = aco.DisplayNumber,
                        BalanceInLocalCurrency = aco.BalanceInLocalCurrency,
                        IntReportCloseBalance = aco.IntReportCloseBalance,
                        FutureInterestTransactionsBalance = intTrans != null ? intTrans.FutureInterestTransactionsBalance : 0m,
                        LastReportNum = aco.LastReportNum,
                        DescBalance = aco.DescBalance,
                        GLAccountBalance = aco.GLAccountBalance,
                        GLAccountFutureBalance = intTrans != null ? intTrans.GLAccountFutureBalance : 0m,
                        DescFutureBalance = intTrans != null ? intTrans.DescFutureBalance : 0m,
                        InterestCalculationStartDate = aco.InterestCalculationStartDate,
                        InterestCalculationDate = aco.InterestCalculationDate,
                        BalanceBeforeStart = aco.BalanceBeforeStart,
                        BalanceInbetween = aco.BalanceInbetween,
                    });
            var test12_f = qAccOpenFuture_f.ToList();


            /////////////
        //    var qAccOpenFuture = (
        //from aco in qAccOpenFuture_b
        //join intTrans in calcFutureInterestTransInclDesc on aco.AccountId equals intTrans.AccountId
        //into accOpenBalanceFutureJoin
        //from intTrans in accOpenBalanceFutureJoin.DefaultIfEmpty()
        //select new InterestReportDiff4Tester6
        //{
        //    AccountId = aco.AccountId,
        //    DisplayNumber = aco.DisplayNumber,
        //    BalanceInLocalCurrency = aco.BalanceInLocalCurrency,
        //    IntReportCloseBalance = aco.IntReportCloseBalance,
        //    FutureInterestTransactionsBalance = intTrans != null ? intTrans.FutureInterestTransactionsBalance : 0m,
        //    LastReportNum = aco.LastReportNum,
        //    DescBalance = aco.DescBalance,
        //    GLAccountBalance = aco.GLAccountBalance,
        //    GLAccountFutureBalance = intTrans != null ? intTrans.GLAccountFutureBalance : 0m,
        //    DescFutureBalance = intTrans != null ? intTrans.DescFutureBalance : 0m,
        //    InterestCalculationStartDate = aco.InterestCalculationStartDate,

        //});
        //    var test12 = qAccOpenFuture.ToList();




            ////////////////
            /// 5. Compute Difference



            var qDiff = (
                    from acof in qAccOpenFuture_f
                    where
                    (acof.IntReportCloseBalance + acof.FutureInterestTransactionsBalance + acof.BalanceBeforeStart + acof.BalanceInbetween - acof.BalanceInLocalCurrency >= 0.001m
                  || acof.IntReportCloseBalance + acof.FutureInterestTransactionsBalance + acof.BalanceBeforeStart + acof.BalanceInbetween - acof.BalanceInLocalCurrency <= -0.001m)
                    select new InterestReportDiff4Tester7
                    {
                        AccountId = acof.AccountId,
                        //   LocalName = acof.LocalName,
                        DisplayNumber = acof.DisplayNumber,
                        //    Tenant = acof.Tenant,
                        BalanceInLocalCurrency = acof.BalanceInLocalCurrency,
                        IntReportCloseBalance = acof.IntReportCloseBalance,
                        FutureInterestTransactionsBalance = acof.FutureInterestTransactionsBalance,
                        CalculatedInterestBalance = acof.IntReportCloseBalance + acof.FutureInterestTransactionsBalance + acof.BalanceBeforeStart + acof.BalanceInbetween,
                        Difference = acof.BalanceInLocalCurrency - (acof.IntReportCloseBalance + acof.FutureInterestTransactionsBalance + acof.BalanceBeforeStart + acof.BalanceInbetween),
                        LastReportNum = acof.LastReportNum,
                        DescBalance = acof.DescBalance,
                        GLAccountBalance = acof.GLAccountBalance,
                        GLAccountFutureBalance = acof.GLAccountFutureBalance,
                        DescFutureBalance = acof.DescFutureBalance,
                        BalanceBeforeStart = acof.BalanceBeforeStart,
                        BalanceInbetween = acof.BalanceInbetween, 
                        InterestCalculationStartDate = acof.InterestCalculationStartDate, 
                        InterestCalculationDate = acof.InterestCalculationDate,
                    }
                    );

            var qThe = qDiff;

            List<InterestReportDiff4Tester7> resultingList = qThe.ToList();
            resultingList.ForEach(q =>
            {
                q.ErrorCounter = _ErrorCounter++;
                this.AddErrorRowList(q.AccountId, q.DisplayNumber, q.LastReportNum, 
                    q.GLAccountBalance, q.DescBalance, q.BalanceInLocalCurrency, 
                    q.IntReportCloseBalance, 
                    q.BalanceBeforeStart, q.InterestCalculationStartDate.HasValue ? q.InterestCalculationStartDate.Value.ToString() : "", 
                    q.BalanceInbetween, q.InterestCalculationDate.HasValue ? q.InterestCalculationDate.Value.ToString() : "", 
                    q.GLAccountFutureBalance, q.DescFutureBalance,
                    q.CalculatedInterestBalance, q.Difference, "");
            });

            // }
        }



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
        public decimal IntReportCloseBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
        public DateTime? InterestCalculationDate { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }
    }

    //public class InterestReportDiff4Tester2
    //{
    //    public int Tenant { get; set; }

    //    public string AccountId { get; set; }

    //    public string DisplayNumber { get; set; }
    //    //     public string LocalName { get; set; }

    //    public decimal BalanceInLocalCurrency { get; set; }
    //    public decimal IntReportCloseBalance { get; set; }

    //    public decimal FutureInterestTransactionsBalance { get; set; }

    //    public decimal CalculatedInterestBalance { get; set; }
    //    public decimal Difference { get; set; }

    //    public int ErrorCounter { get; set; }

    //    public string LastReportNum { get; set; }

    //    public decimal GLAccountIntRepOpenBalance { get; set; }
    //    public decimal DescIntRepOpenBalance { get; set; }
    //}


    public class InterestReportDiff4Tester3
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //     public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal IntReportCloseBalance { get; set; }

        public decimal BeforeInterestTransactionsBalance { get; set; }
        public decimal InbetweenInterestTransactionsBalance { get; set; }
        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }
    }


    public class InterestReportDiff4Tester4
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //     public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal IntReportCloseBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }

        public decimal GLAccountFutureBalance { get; set; }
        public decimal DescFutureBalance { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }
    }


    public class InterestReportDiff4Tester5
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //   public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal DescBalance { get; set; }
        public decimal GLAccountBalance { get; set; }

        public decimal IntReportCloseBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }
        public DateTime? InterestCalculationDate { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }
    }


    public class InterestReportDiff4Tester6
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //   public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal DescBalance { get; set; }
        public decimal GLAccountBalance { get; set; }

        public decimal IntReportCloseBalance { get; set; }

        public decimal FutureInterestTransactionsBalance { get; set; }
        public decimal BalanceBeforeStart { get; set; }
        public decimal BalanceInbetween { get; set; }

        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }

        public decimal GLAccountFutureBalance { get; set; }
        public decimal DescFutureBalance { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }
        public DateTime? InterestCalculationDate { get; set; }

    }


    public class InterestReportDiff4Tester7
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //    public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal IntReportCloseBalance { get; set; }
        public decimal DescBalance { get; set; }
        public decimal GLAccountBalance { get; set; }


        public decimal FutureInterestTransactionsBalance { get; set; }
        public decimal BalanceBeforeStart { get; set; }
        public decimal BalanceInbetween { get; set; }
        public decimal CalculatedInterestBalance { get; set; }
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

        public string LastReportNum { get; set; }

        public decimal GLAccountFutureBalance { get; set; }
        public decimal DescFutureBalance { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }
        public DateTime? InterestCalculationDate { get; set; }


    }




    //public class InterestReportDiffDesc4Tester
    //{
    //    public string ParentAccountId { get; set; }
    //    public decimal DescIntReportCloseBalance { get; set; }
    //    public decimal DescFutureInterestTransactionsBalance { get; set; }
    //    public string LastReportNum { get; set; }

    //}

    public class InterestReportDiffDesc4Tester1_5
    {
        public string ParentAccountId { get; set; }
        public decimal LocalAmount { get; set; }
    }


    public class InterestReportDiffDesc4Tester2
    {
        public string ParentAccountId { get; set; }
        public decimal DescIntReportCloseBalance { get; set; }
        public decimal DescFutureInterestTransactionsBalance { get; set; }
        public string LastReportNum { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }

    }


    public class GLAccountDescMDatasDTO4Tester
    {
        public string ParentAccId { get; set; }
        public string DescAccId { get; set; }
        public decimal DescBalanceInLocalCurrency { get; set; }
        public decimal DescIntReportCloseBalance { get; set; }
        public DateTime InterestCalculationDate { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }
    }

    public class GLAccountDescMDatasDTO4Tester2
    {
        public string ParentAccId { get; set; }
        public string DescAccId { get; set; }
        public decimal DescBalanceInLocalCurrency { get; set; }
        public decimal DescIntReportCloseBalance { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }
    }



    public class GLAccountMDatasDTO4Tester
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //     public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal IntReportCloseBalance { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }

    }

    public class GLAccountMDatasDTO4Tester2
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        //   public string LocalName { get; set; }

        public decimal GLAccountBalance { get; set; }
        public decimal DescBalance { get; set; }
        public decimal BalanceInLocalCurrency { get; set; }
        public decimal IntReportCloseBalance { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }

    }


}