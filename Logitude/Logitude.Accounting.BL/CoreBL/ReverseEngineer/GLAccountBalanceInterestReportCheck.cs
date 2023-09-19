using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class GLAccountBalanceInterestReportCheck
    {

        private int _Tenant;
        private int _ErrorCounter;
        private IAccountingContext _AccountingContext;
        private readonly string const_ThereIsntAnyGLAccounts = "There Isn't Any GLAccounts";
        private readonly string const_DifftotBalanceInLocalCurrencyMinusaccBalanceInLocalCurrency = "Diff = tot.BalanceInLocalCurrency-acc.BalanceInLocalCurrency";
        private readonly string const_ThereIsntAnyGLAccountTotalByMonths = "There Isn't Any GLAccountTotalByMonths";
        private readonly string const_TotalOpenAmountInTransactionDiffBalanceInLocalCurrency =
            "  שקל -בדיקה שסך סכום פתוח של תנועות בכרטיס שווה ליתרה חשבונאית של כרטיס";
        private readonly string const_TotalOpenAmountInTransactionDiffBalanceInForeign = " - מטח בדיקה שסך סכום פתוח של תנועות בכרטיס שווה ליתרה חשבונאית של כרטיס";

        public GLAccountBalanceInterestReportCheck(int currTenant)
        {
            // TODO: Complete member initialization

            this._Tenant = currTenant;
            this._ErrorCounter = 1;
        }

        public void CheckDbIntegrity()
        {
            var sw = Stopwatch.StartNew();

            using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(10)))
            {
                _AccountingContext = AccountingContext.GetContext(_Tenant);

                var myGLAccountRepo = new GLAccountRepository(_AccountingContext);
                var myGLAccountMoreDataRepo = new GLAccountMoreDataRepository(_AccountingContext);

                var myInterestReportRepo = new InterestReportRepository(_AccountingContext);

                var myInterestTransactionRepository = new InterestTransactionRepository(_AccountingContext);





                ////////////////
                /// 1. Compute BalanceInLocalCurrency

                var dbAllGLAccount =
                    (from acc in myGLAccountRepo.GetAll(_Tenant)
                     join md in myGLAccountMoreDataRepo.GetAll(_Tenant) on acc.Id equals md.AccountId
                     where acc.Tenant == _Tenant && acc.ActiveForInterest && acc.ParentAccountId == null 
                     select new GLAccountMDatasDTO
                     {
                         AccountId = acc.Id,
                         LocalName = acc.LocalName,
                         DisplayNumber = acc.DisplayNumber,
                         Tenant = acc.Tenant,
                         BalanceInLocalCurrency = md.BalanceInLocalCurrency,
                         //InterestOpenBalance = acc.InterestOpenBalance, // see 159870
                     }
                    );


                var dbAllDescendantsOnly =
                    (from aco in dbAllGLAccount
                     join descendants in myGLAccountRepo.GetAll(_Tenant) on aco.AccountId equals descendants.ParentAccountId
                     where aco.Tenant == _Tenant
                     select new GLAccountDescMDatasDTO()
                     {
                         ParentAccId = descendants.ParentAccountId,
                         DescAccId = descendants.Id,
                     });


                var dbAllDescendants =
                    (from aco in dbAllGLAccount
                     join descendants in myGLAccountRepo.GetAll(_Tenant) on aco.AccountId equals descendants.ParentAccountId
                     join md in myGLAccountMoreDataRepo.GetAll(_Tenant) on descendants.Id equals md.AccountId
                     into accOpenBalanceDescJoin
                     from aod in accOpenBalanceDescJoin 
                     where aco.Tenant == _Tenant
                     group aod by aco.AccountId into g
                     select new GLAccountDescMDatasDTO2()
                     {
                         ParentAccId = g.Key,
                         DescBalanceInLocalCurrency = g.Sum(r => r.BalanceInLocalCurrency),
                         //DescInterestOpenBalance = g.Sum(r => r.InterestOpenBalance),
                     });

                var dbAllGLAccountInclDesc = 
                     (
                        from acc in dbAllGLAccount
                        join descs in dbAllDescendants on acc.AccountId equals descs.ParentAccId
                        into accDescendantsJoin
                        from ad in accDescendantsJoin.DefaultIfEmpty()
                        select new GLAccountMDatasDTO
                        {
                            AccountId = acc.AccountId,
                            LocalName = acc.LocalName,
                            DisplayNumber = acc.DisplayNumber,
                            Tenant = acc.Tenant,
                            BalanceInLocalCurrency = ad != null ? acc.BalanceInLocalCurrency + ad.DescBalanceInLocalCurrency : acc.BalanceInLocalCurrency,
                            //InterestOpenBalance = ad != null ? acc.InterestOpenBalance + ad.DescInterestOpenBalance : acc.InterestOpenBalance, 
                        }
                     );





                ////////////////
                /// 2. Compute InterestOpenBalances

                var calcInterestOpenBalances =
                     (
                        from intRep in
                            myInterestReportRepo.GetAll(_Tenant).Where(intRep => intRep.CreateDateTime.HasValue).OrderBy(intRep => intRep.CreateDateTime.Value).Where(intRep => (intRep.InterestReportStatusCode == "2" || intRep.InterestReportStatusCode == "4"))
                        where intRep.Tenant == _Tenant
                        join acc in dbAllGLAccountInclDesc on intRep.GLAccountId equals acc.AccountId
                        select new InterestReportDiff
                        {
                            AccountId = intRep.GLAccountId,
                            InterestOpenBalance = intRep.OpenBalance.HasValue ? intRep.OpenBalance.Value : 0m,
                        }
                     );


                var calcInterestOpenBalancesDesc =
                     (
                        from intRep in
                            myInterestReportRepo.GetAll(_Tenant).Where(intRep => intRep.CreateDateTime.HasValue).OrderBy(intRep => intRep.CreateDateTime.Value).Where(intRep => (intRep.InterestReportStatusCode == "2" || intRep.InterestReportStatusCode == "4"))
                        where intRep.Tenant == _Tenant
                        join descendants in dbAllDescendantsOnly on intRep.GLAccountId equals descendants.DescAccId
                        select new InterestReportDiffDesc
                        {
                            ParentAccountId = descendants.ParentAccId,
                            DescInterestOpenBalance = intRep.OpenBalance.HasValue ? intRep.OpenBalance.Value : 0m,
                        }
                     );

                var calcInterestOpenBalancesInclDesc =
                     (
                        from acc in calcInterestOpenBalances
                        join descs in calcInterestOpenBalancesDesc on acc.AccountId equals descs.ParentAccountId
                        into accDescendantsJoin
                        from ad in accDescendantsJoin.DefaultIfEmpty()
                        select new InterestReportDiff
                        {
                            AccountId = acc.AccountId,
                           // LocalName = acc.LocalName,
                            DisplayNumber = acc.DisplayNumber,
                            Tenant = acc.Tenant,
                            InterestOpenBalance = ad != null ? acc.InterestOpenBalance + ad.DescInterestOpenBalance : acc.InterestOpenBalance, 
                        }
                     );





                ////////////////
                /// 3.Compute FutureInterestTrans


                var calcFutureInterestTrans =
                     (
                        from intTrans in
                            myInterestTransactionRepository.GetAll(_Tenant).Where(intTrans => intTrans.InterestValueDate >= DateTime.Today.Date)
                        where intTrans.Tenant == _Tenant
                        join acc in dbAllGLAccountInclDesc on intTrans.GLAccountId equals acc.AccountId
                        group intTrans by intTrans.GLAccountId into g
                        select new InterestReportDiff
                        {
                            AccountId = g.Key,
                            FutureInterestTransactionsBalance = g.Sum(r => r.LocalAmount)
                        }
                     );


                var calcFutureInterestTransDesc =
                     (
                        from intTrans in
                            myInterestTransactionRepository.GetAll(_Tenant).Where(intTrans => intTrans.InterestValueDate >= DateTime.Today.Date)
                        where intTrans.Tenant == _Tenant
                        join descendants in dbAllDescendantsOnly on intTrans.GLAccountId equals descendants.DescAccId
                        group intTrans by intTrans.GLAccountId into g
                        select new InterestReportDiffDesc
                        {
                            ParentAccountId = g.Key,
                            DescFutureInterestTransactionsBalance = g.Sum(r => r.LocalAmount)
                        }
                     );

                var calcFutureInterestTransInclDesc =
                     (
                        from acc in calcFutureInterestTrans
                        join descs in calcFutureInterestTransDesc on acc.AccountId equals descs.ParentAccountId
                        into accDescendantsJoin
                        from ad in accDescendantsJoin.DefaultIfEmpty()
                        select new InterestReportDiff
                        {
                            AccountId = acc.AccountId,
                            LocalName = acc.LocalName,
                            DisplayNumber = acc.DisplayNumber,
                            Tenant = acc.Tenant,
                            FutureInterestTransactionsBalance = ad != null ? acc.FutureInterestTransactionsBalance + ad.DescFutureInterestTransactionsBalance : acc.FutureInterestTransactionsBalance,
                        }
                     );





                ////////////////
                /// 4. Assemble BalanceInLocalCurrency, InterestOpenBalances, FutureInterestTrans


                var qAccOpen = (
                    from acc in dbAllGLAccountInclDesc
                    join intRep in calcInterestOpenBalancesInclDesc on acc.AccountId equals intRep.AccountId
                    into accOpenBalanceJoin
                    from ao in accOpenBalanceJoin.DefaultIfEmpty()
                    where acc.Tenant == _Tenant
                    select new InterestReportDiff()
                    {
                        AccountId = acc.AccountId,
                        LocalName = acc.LocalName,
                        DisplayNumber = acc.DisplayNumber,
                        Tenant = acc.Tenant,
                        BalanceInLocalCurrency = acc.BalanceInLocalCurrency,
                        InterestOpenBalance = ao != null ? ao.InterestOpenBalance : 0m,
                    });

                var qAccOpenFuture = (
                    from aco in qAccOpen
                    join intTrans in calcFutureInterestTransInclDesc on aco.AccountId equals intTrans.AccountId
                    into accOpenBalanceFutureJoin
                    from aof in accOpenBalanceFutureJoin.DefaultIfEmpty()
                    where aco.Tenant == _Tenant
                    select new InterestReportDiff()
                    {
                        AccountId = aco.AccountId,
                        LocalName = aco.LocalName,
                        DisplayNumber = aco.DisplayNumber,
                        Tenant = aco.Tenant,
                        BalanceInLocalCurrency = aco.BalanceInLocalCurrency,
                        InterestOpenBalance = aco.InterestOpenBalance,
                        FutureInterestTransactionsBalance = aof != null ? aof.FutureInterestTransactionsBalance : 0m,
                    });


                ////////////////
                /// 5. Compute Difference



                var qDiff = (
                    from acof in qAccOpenFuture
                    where
                    (acof.InterestOpenBalance + acof.FutureInterestTransactionsBalance - acof.BalanceInLocalCurrency >= 0.001m || acof.InterestOpenBalance + acof.FutureInterestTransactionsBalance - acof.BalanceInLocalCurrency <= -0.001m)
                    select new InterestReportDiff
                    {
                        AccountId = acof.AccountId,
                        LocalName = acof.LocalName,
                        DisplayNumber = acof.DisplayNumber,
                        Tenant = acof.Tenant,
                        BalanceInLocalCurrency = acof.BalanceInLocalCurrency,
                        InterestOpenBalance = acof.InterestOpenBalance,
                        FutureInterestTransactionsBalance = acof.FutureInterestTransactionsBalance,
                        CalculatedInterestBalance = acof.InterestOpenBalance + acof.FutureInterestTransactionsBalance,
                        Difference = acof.BalanceInLocalCurrency - acof.BalanceInLocalCurrency,
                    }
                    ) ;

                var qThe = qDiff.Take(30);

                List<InterestReportDiff> resultingList = qThe.ToList();
                resultingList.ForEach(q => { q.ErrorCounter = _ErrorCounter++; });
                CompareReport = new CompareReportM()
                {
                    CompareReportName = "GLAccountBalanceInterestReportCheck",
                    //rows = res,
                    InterestReportDiffList = resultingList,
                    Took = sw.Elapsed
                };
          //      Convert2DisplayNumber(CompareReport.GLAccountBalanceList, _Tenant);
            }
        }
        private void Convert2DisplayNumber(List<GLAccountBalanceDTO> rows, int tenant)
        {
            if (rows == null)
            {
                return;
            }
            try
            {
                var AccountIdList = rows.Where(r => !string.IsNullOrWhiteSpace(r.AccountId)).Select(x => x.AccountId).Distinct().ToList();
                var repo = new GLAccountRepository(tenant);
                var res = repo.GetDisplayNumberList(AccountIdList.ToHashSet(), tenant);
                foreach (var item in rows)
                {
                    var display = res.FirstOrDefault(r => r.Key == item.AccountId);
                    if (string.IsNullOrEmpty(display.Value))
                    {
                        continue;
                    }
                    item.AccountDisplayNumber = display.Value;
                }
            }
            catch (Exception)
            {


            }
        }
        private IQueryable<GLAccountBalanceDTO> GetTotalOpenAmountInTransactionDiffBalanceInForeign(
            GLAccountRepository myGLAccountRepo,
            LedgerTransactionRepository myLedgerTransactionRepository,
            IQueryable<GLAccountBalanceDTO> quaryablMonthTotalsForeignAmount)
        {
            var qTotOpenAmountInTrans = (
                               from tran in myLedgerTransactionRepository.GetAll(_Tenant)
                               join acc in myGLAccountRepo.GetAll(_Tenant).Where(r => r.ReconcileMethodCode == "1")
                               on tran.AccountId equals acc.Id

                               group tran by tran.AccountId into gTransByAcc

                               select new GLAccountBalanceDTO
                               {
                                   AccountId = gTransByAcc.Key,

                                   BalanceInLocalCurrency = gTransByAcc.Sum(r => r.OpenAmount),
                                   BalanceInForeignCurrency = 0,
                                   CHANGE_TYPE = ""
                               }

                               );
            var qTotalOpenAmountInTransactionDiffBalanceInLocalCurrency = (
            from myTotalsForeignAmount in quaryablMonthTotalsForeignAmount
            join totOpenAmountInTrans in qTotOpenAmountInTrans
            on myTotalsForeignAmount.AccountId equals totOpenAmountInTrans.AccountId
            where (
            myTotalsForeignAmount.BalanceInLocalCurrency - totOpenAmountInTrans.BalanceInLocalCurrency >= 0.001m ||
            myTotalsForeignAmount.BalanceInLocalCurrency - totOpenAmountInTrans.BalanceInLocalCurrency <= -0.001m)
            select new GLAccountBalanceDTO
            {
                AccountId = myTotalsForeignAmount.AccountId,

                BalanceInLocalCurrency = myTotalsForeignAmount.BalanceInLocalCurrency - totOpenAmountInTrans.BalanceInLocalCurrency,
                BalanceInForeignCurrency = myTotalsForeignAmount.BalanceInForeignCurrency - totOpenAmountInTrans.BalanceInForeignCurrency,
                CHANGE_TYPE = const_TotalOpenAmountInTransactionDiffBalanceInForeign
            }

            );
            return qTotalOpenAmountInTransactionDiffBalanceInLocalCurrency;
        }
        private IQueryable<GLAccountBalanceDTO> GetTotalOpenAmountInTransactionDiffBalanceInLocalCurrency(GLAccountRepository myGLAccountRepo, LedgerTransactionRepository myLedgerTransactionRepository, IQueryable<GLAccountBalanceDTO> quaryablMonthTotals)
        {
            var qTotOpenAmountInTrans = (
                               from tran in myLedgerTransactionRepository.GetAll(_Tenant)
                               join acc in myGLAccountRepo.GetAll(_Tenant).Where(r => r.ReconcileMethodCode == "0")
                               on tran.AccountId equals acc.Id

                               group tran by tran.AccountId into gTransByAcc

                               select new GLAccountBalanceDTO
                               {
                                   AccountId = gTransByAcc.Key,

                                   BalanceInLocalCurrency = gTransByAcc.Sum(r => r.OpenAmount),
                                   BalanceInForeignCurrency = 0,
                                   CHANGE_TYPE = ""
                               }

                               );
            var qTotalOpenAmountInTransactionDiffBalanceInLocalCurrency = (
            from tot in quaryablMonthTotals
            join totOpenAmountInTrans in qTotOpenAmountInTrans
            on tot.AccountId equals totOpenAmountInTrans.AccountId
            where (tot.BalanceInLocalCurrency - totOpenAmountInTrans.BalanceInLocalCurrency >= 0.001m || tot.BalanceInLocalCurrency - totOpenAmountInTrans.BalanceInLocalCurrency <= -0.001m)
            select new GLAccountBalanceDTO
            {
                AccountId = tot.AccountId,

                BalanceInLocalCurrency = tot.BalanceInLocalCurrency - totOpenAmountInTrans.BalanceInLocalCurrency,
                BalanceInForeignCurrency = 0,
                CHANGE_TYPE = const_TotalOpenAmountInTransactionDiffBalanceInLocalCurrency
            }

            );
            return qTotalOpenAmountInTransactionDiffBalanceInLocalCurrency;
        }

        public CompareReportM CompareReport { get; set; }

    }

    public class GLAccountMDatasDTO
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }
        public decimal InterestOpenBalance { get; set; }

    }

    public class GLAccountDescMDatasDTO
    {
        public string ParentAccId { get; set; }
        public string DescAccId { get; set; }
        public decimal DescBalanceInLocalCurrency { get; set; }
        public decimal DescInterestOpenBalance { get; set; }
    }

    public class GLAccountDescMDatasDTO2
    {
        public string ParentAccId { get; set; }
        public string DescAccId { get; set; }
        public decimal DescBalanceInLocalCurrency { get; set; }
        public decimal DescInterestOpenBalance { get; set; }
    }

    public class InterestReportDiffDesc
    {
        public string ParentAccountId { get; set; }
        public decimal DescInterestOpenBalance { get; set; }
        public decimal DescFutureInterestTransactionsBalance { get; set; }
    }

    public class InterestReportDiff
    {
        public int Tenant { get; set; }

        public string AccountId { get; set; }

        public string DisplayNumber { get; set; }
        public string LocalName { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }  
        public decimal InterestOpenBalance { get; set; }  

        public decimal FutureInterestTransactionsBalance { get; set; }

        public decimal CalculatedInterestBalance { get; set; }  
        public decimal Difference { get; set; }

        public int ErrorCounter { get; set; }

    }
}
