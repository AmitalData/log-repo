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
    public class ReverseEngineerGLAccountBalance
    {

        private int _Tenant;
        private IAccountingContext _AccountingContext;
        private readonly string const_ThereIsntAnyGLAccounts= "There Isn't Any GLAccounts";
        private readonly string const_DifftotBalanceInLocalCurrencyMinusaccBalanceInLocalCurrency= "Diff = tot.BalanceInLocalCurrency-acc.BalanceInLocalCurrency";
        private readonly string const_ThereIsntAnyGLAccountTotalByMonths= "There Isn't Any GLAccountTotalByMonths";
        private readonly string const_TotalOpenAmountInTransactionDiffBalanceInLocalCurrency = 
            "  שקל -בדיקה שסך סכום פתוח של תנועות בכרטיס שווה ליתרה חשבונאית של כרטיס";
        private readonly string const_TotalOpenAmountInTransactionDiffBalanceInForeign = " - מטח בדיקה שסך סכום פתוח של תנועות בכרטיס שווה ליתרה חשבונאית של כרטיס";

        public ReverseEngineerGLAccountBalance(int currTenant)
        {
            // TODO: Complete member initialization

            this._Tenant = currTenant;
        }

        public void CheckDbIntegrity()
        {
            var sw = Stopwatch.StartNew();

            using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(10)))
            {
                _AccountingContext = AccountingContext.GetContext(_Tenant);

                var myGLAccountRepo = new GLAccountRepository(_AccountingContext);
                var myGLAccountMoreDataRepo = new GLAccountMoreDataRepository(_AccountingContext);

                var myGLAccountTotalByMonthRepo = new GLAccountTotalByMonthRepository(_AccountingContext);
                var myLedgerTransactionRepository = new LedgerTransactionRepository(_AccountingContext);

                var quaryAllGLAccount =
                    (from acc in myGLAccountRepo.GetAll(_Tenant)
                     join md in myGLAccountMoreDataRepo.GetAll(_Tenant) on acc.Id equals md.AccountId
                     where acc.Tenant == _Tenant
                     select new GLAccountBalanceDTO
                     {
                         AccountId = acc.Id,
                         BalanceInLocalCurrency = md.BalanceInLocalCurrency,
                         CHANGE_TYPE = ""
                     }

                         );

                //quaryAllControlAccount.First().BalanceInLocalCurrency
                var quaryablMonthTotals =
                 (
                from tot in
                    myGLAccountTotalByMonthRepo.GetAll(_Tenant).Where(tot => tot.DateTypeCode == GLAccountTotalDateTypeValues.Accountingdate)
                where tot.Tenant == _Tenant
                group tot by tot.AccountId into g
                select new GLAccountBalanceDTO
                {
                    AccountId = g.Key,
                    BalanceInLocalCurrency = g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit),
                    CHANGE_TYPE = ""

                }
                 );

                var qNotInTot =
                    (
                    from acc in quaryAllGLAccount
                    where acc.BalanceInLocalCurrency != null
                    where acc.BalanceInLocalCurrency != 0
                    join tot in quaryablMonthTotals
                    on acc.AccountId equals tot.AccountId
                    into joinTotT
                    from joinTotRec in joinTotT.DefaultIfEmpty()
                    where joinTotRec == null
                    select new GLAccountBalanceDTO
                    {
                        AccountId = acc.AccountId,
                        BalanceInLocalCurrency = acc.BalanceInLocalCurrency,
                        CHANGE_TYPE = const_ThereIsntAnyGLAccountTotalByMonths// "There Isn't Any GLAccountTotalByMonths"

                    }
                    );


                var qNotInGLAcc =
                    (
                    from tot in quaryablMonthTotals
                    join acc in quaryAllGLAccount
                    on tot.AccountId equals acc.AccountId
                    into joinAccT
                    from joinAccRec in joinAccT.DefaultIfEmpty()
                    where joinAccRec == null
                    select new GLAccountBalanceDTO
                    {
                        AccountId = tot.AccountId,
                        BalanceInLocalCurrency = tot.BalanceInLocalCurrency,
                        CHANGE_TYPE = const_ThereIsntAnyGLAccounts

                    }
                    );

                var qDiff = (
                    from tot in quaryablMonthTotals
                    join acc in quaryAllGLAccount
                    on tot.AccountId equals acc.AccountId
                    where (tot.BalanceInLocalCurrency - acc.BalanceInLocalCurrency >= 0.001m || tot.BalanceInLocalCurrency - acc.BalanceInLocalCurrency <= -0.001m)
                    select new GLAccountBalanceDTO
                    {
                        AccountId = tot.AccountId,
                        BalanceInLocalCurrency = tot.BalanceInLocalCurrency - acc.BalanceInLocalCurrency,
                        CHANGE_TYPE = const_DifftotBalanceInLocalCurrencyMinusaccBalanceInLocalCurrency// "Diff = tot.BalanceInLocalCurrency-acc.BalanceInLocalCurrency"

                    }
                    );



                var quaryablMonthTotalsForeignAmount =
                 (
                from tot in
                    myGLAccountTotalByMonthRepo.GetAll(_Tenant).Where(tot => tot.DateTypeCode == GLAccountTotalDateTypeValues.Accountingdate)
                where tot.Tenant == _Tenant
                group tot by tot.AccountId into g
                select new GLAccountBalanceDTO
                {
                    AccountId = g.Key,
                    BalanceInLocalCurrency = g.Sum(r => r.ForeignAmountDebit - r.ForeignAmountCredit),
                    CHANGE_TYPE = ""

                }
                 );
                //1   Foreign Currency
                IQueryable<GLAccountBalanceDTO> qTotalOpenAmountInTransactionDiffBalanceInForeign =
                GetTotalOpenAmountInTransactionDiffBalanceInForeign(myGLAccountRepo, myLedgerTransactionRepository, quaryablMonthTotalsForeignAmount);

                //0	Local Currency
                IQueryable<GLAccountBalanceDTO> qTotalOpenAmountInTransactionDiffBalanceInLocalCurrency = 
                    GetTotalOpenAmountInTransactionDiffBalanceInLocalCurrency(myGLAccountRepo, myLedgerTransactionRepository, quaryablMonthTotals);

                var qThe = qNotInTot.Union(qNotInGLAcc).Union(qDiff)
                    .Union(qTotalOpenAmountInTransactionDiffBalanceInLocalCurrency); ;
                bool UnionreturnsDistinctvalues = true;
                if (UnionreturnsDistinctvalues)
                {
                    qThe = qNotInTot.Concat(qNotInGLAcc).Concat(qDiff)
                        //.Concat(qTotalOpenAmountInTransactionDiffBalanceInLocalCurrency)
                        //.Concat(qTotalOpenAmountInTransactionDiffBalanceInForeign); 
                        ;
                }
                var myTotalOpenReconciliation= (qTotalOpenAmountInTransactionDiffBalanceInLocalCurrency
                        .Concat(qTotalOpenAmountInTransactionDiffBalanceInForeign)).ToList(); 
                var l = qThe.ToList();
                CompareReport = new CompareReportM()
                {
                    CompareReportName = "ReverseEngineerGLAccountBalance",
                    //rows = res,
                    GLAccountBalanceList = l,
                    TotalOpenReconciliation = myTotalOpenReconciliation,
                    Took = sw.Elapsed
                };
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
                CHANGE_TYPE = const_TotalOpenAmountInTransactionDiffBalanceInLocalCurrency
            }

            );
            return qTotalOpenAmountInTransactionDiffBalanceInLocalCurrency;
        }

        public CompareReportM CompareReport { get; set; }

        public void FIXCheckDbIntegrity()
        {
            CheckDbIntegrity();
            if (this.CompareReport.GLAccountBalanceList == null || this.CompareReport.GLAccountBalanceList.Count == 0)
            {
                throw new Exception("is ok - nothing done  !!!!");
            }
            if (this.CompareReport.GLAccountBalanceList.Any(r => r.CHANGE_TYPE != const_DifftotBalanceInLocalCurrencyMinusaccBalanceInLocalCurrency))
            {
                throw new Exception("meanwhile i do only  - Diff = tot.BalanceInLocalCurrency-acc.BalanceInLocalCurrency   - to do !!");
            }

            using (var scope = TransactionFactory.GetNewSerializableTransaction())
            {
                _AccountingContext = AccountingContext.GetContext(_Tenant);
                var myGLAccountTotalByMonthRepository = new GLAccountTotalByMonthRepository(_AccountingContext);
                var tUpdateDiff = this.CompareReport.GLAccountBalanceList.Where(r => r.CHANGE_TYPE == const_DifftotBalanceInLocalCurrencyMinusaccBalanceInLocalCurrency).ToList();

                var glList = tUpdateDiff.Select(r => r.AccountId).ToList();
                var myGLAccountMoreDataRepo = new GLAccountMoreDataRepository(_AccountingContext);
                var AccList=myGLAccountMoreDataRepo.GetAll(_Tenant).Where(r => glList.Contains(r.AccountId)).ToList();
                AccList.ForEach(poco =>
               {
                   poco.BalanceInLocalCurrency += tUpdateDiff.First(rDiff => rDiff.AccountId == poco.AccountId).BalanceInLocalCurrency;
                   myGLAccountMoreDataRepo.Update(poco);
               });
                myGLAccountMoreDataRepo.SubmitChanges();
                scope.Complete();
            }
        }
    }
    public class GLAccountBalanceDTO
    {
        public string AccountId { get; set; }

        public decimal BalanceInLocalCurrency { get; set; }

        public string CHANGE_TYPE { get; set; }
    }
}
