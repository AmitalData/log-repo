using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using System.Threading;
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.SystemLogs;
using System.Data.Entity.Core.Objects;
namespace Logitude.Accounting.BL.CoreBL
{
    public class DueLocalBalanceService
    {
        readonly decimal? decimalNull = null;
        readonly DateTime dateTimeMinValue = DateTime.MinValue;
        readonly DateTime dateTimeMaxValue = DateTime.MaxValue;
        private readonly DateTime _today;
        private readonly DateTime _thisBeginOfMonth;
        private readonly DateTime _lastMonth;
        private readonly List<string> _clientAndVendorType;
        private IQueryable<DueLocalBalanceM> _QDueLocalBalanceListToUpdate;
        private IQueryable<string> _QClientAndVendorTypeGLAccountIds;

        public DueLocalBalanceService()
        {

            _today = GetToday().Date;
            _thisBeginOfMonth = new DateTime(_today.Year, _today.Month, 1);
            _lastMonth = _today.AddMonths(-1);
            _clientAndVendorType = new List<string>() { "2", "3" }; //AccoutTypeCode= 2 or 3 (clients/Vendors)
        }

#if false
        #region MyRegion
        void RunOld(int tenant, List<string> accountIdList = null)
        {
            var qs = new GLAccountQueryService(tenant);

            var sw = Stopwatch.StartNew();
            try
            {

                accountIdList = accountIdList ?? qs.GetListByNextDueDate(tenant, _today, _clientAndVendorType).ToList();
                foreach (List<string> listBatch in accountIdList.Batch(100))
                {


                    using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(1)))
                    {
                        var accountingContext = AccountingContext.GetContext(tenant);
                        var repoGLAccountTotalByMonths = new GLAccountTotalByMonthRepository(accountingContext);
                        var repoLedgerTransactionRepository = new LedgerTransactionRepository(accountingContext);

                        var qDUEDATETotalLocalAmountLastMonth =
                            (from totDueDate in repoGLAccountTotalByMonths.GetQTotalByDateTypeCodeUntil(tenant, _lastMonth, listBatch.AsQueryable(), GLAccountTotalDateTypeValues.DueDate)
                             group totDueDate by totDueDate.AccountId into g
                             select new DueLocalBalanceM
                             {
                                 AccountId = g.Key,
                                 RealDueInLocal = 0,
                                 TotalLocalAmountLastMonthDue = g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit),
                                 TransLocalAmountDueMonth = decimalNull,
                                 TransNextDueDate = dateTimeMinValue,
                             });



                        var qDUEDATELedgerTransThisMonthInclusiveToDay =
                            (
                            from transDueDate in repoLedgerTransactionRepository.GetQBetweenDueInclusive(tenant, listBatch.AsQueryable(), _thisBeginOfMonth, _today)
                            group transDueDate by transDueDate.AccountId into g
                            select new DueLocalBalanceM
                            {
                                AccountId = g.Key,
                                RealDueInLocal = 0,
                                TotalLocalAmountLastMonthDue = decimalNull,
                                TransLocalAmountDueMonth = g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit),
                                TransNextDueDate = dateTimeMinValue,
                            });


                        var nextStep =
                            (from transDueDate in
                                 repoLedgerTransactionRepository.GetQDueGreaterthan(tenant, listBatch.AsQueryable(), _today)
                             //    accountingContext.LedgerTransactions
                             //where transDueDate.Tenant == tenant
                             //where listBatch.Contains(transDueDate.AccountId)
                             //where transDueDate.DueDate > today
                             group transDueDate by transDueDate.AccountId into g
                             select new DueLocalBalanceM
                             {
                                 AccountId = g.Key,
                                 RealDueInLocal = 0,
                                 TotalLocalAmountLastMonthDue = decimalNull,
                                 TransLocalAmountDueMonth = decimalNull,
                                 TransNextDueDate = g.Min(r => r.DueDate),
                             });

                        var all = qDUEDATETotalLocalAmountLastMonth.Union(qDUEDATELedgerTransThisMonthInclusiveToDay).Union(nextStep);

                        var theDueLocalBalanceListToUpdate =
                            (from m in all
                             group m by m.AccountId into g
                             select new DueLocalBalanceM
                             {
                                 AccountId = g.Key,
                                 RealDueInLocal = g.Sum(r => r.TotalLocalAmountLastMonthDue)??0 + g.Sum(r => r.TransLocalAmountDueMonth)??0,
                                 TotalLocalAmountLastMonthDue = decimalNull,
                                 TransLocalAmountDueMonth = decimalNull,
                                 TransNextDueDate = g.Min(r => r.TransNextDueDate),
                             }
                            )
                            .Where(g => g.TransNextDueDate != null || g.RealDueInLocal != null)
                            .ToList();

                        var myDefaultListToUpdate =
                        (
                        from myAccountId in listBatch
                        select new DueLocalBalanceM
                        {
                            AccountId = myAccountId,
                            RealDueInLocal = 0,
                            TotalLocalAmountLastMonthDue = decimalNull,
                            TransLocalAmountDueMonth = decimalNull,
                            TransNextDueDate = dateTimeMinValue,
                        }

                        )
                        .ToList();

                        var changedAccount = //theDueLocalBalanceListToUpdate
                            myDefaultListToUpdate
                            .Select(r => r.AccountId).ToList();
                        var pmList = qs.GetByGLAccountsIdList(changedAccount //listBatch
                            , tenant);

                        var myGLAccountUpdateServiceBalancePriv = new GLAccountUpdateServiceBalancePriv(accountingContext, new Dictionary<string, IContext>(), tenant);
                        //foreach (var item in theDueLocalBalanceListToUpdate)
                        foreach (var defaultItem in myDefaultListToUpdate)
                        {
                            var item2update = theDueLocalBalanceListToUpdate.FirstOrDefault(r => r.AccountId == defaultItem.AccountId);
                            item2update = item2update ?? defaultItem;
                            var pm = pmList.First(r => r.Id == item2update.AccountId);
                            if (pm.NextDueDate.GetValueOrDefault() == item2update.TransNextDueDate &&
                                pm.LocalBalanceInDue == item2update.RealDueInLocal)
                            {
                            }
                            else
                            {
                                if (item2update.RealDueInLocal!= 0)
                                {

                                }
                                var detaLocalBalanceInDue = pm.LocalBalanceInDue.GetValueOrDefault() - item2update.RealDueInLocal;
                                pm.LocalBalanceInDue = pm.LocalBalanceInDue.GetValueOrDefault() + detaLocalBalanceInDue;
                                pm.NextDueDate = item2update.TransNextDueDate;
                                pm.ChangeSetOp = ChangeSetOperation.Update;

                                myGLAccountUpdateServiceBalancePriv.Init(null, detaLocalBalanceInDue, item2update.TransNextDueDate);
                                myGLAccountUpdateServiceBalancePriv.Update(pm, true);
                            }

                        }
                        scope.Complete();
                    }
                    //Thread.Sleep(200000);//let Journal approval work 
                }

            }
            catch (Exception e)
            {
                LogMessagingUtil.Instance.AppendLine("DueLocalBalanceService Exception e=" + e.ToString());
                throw;
            }
            finally
            {
                LogMessagingUtil.Instance.AppendLine("DueLocalBalanceService tenant= " + tenant + "  accountIdList.Count=" + accountIdList.Count + ") took:" + sw.Elapsed.ToString());
            }
        }
        #endregion


#endif

        public void ReBuild(int tenant, string AccountId, bool fastRun = false)
        {
            int clientAndVendorTypeGLAccountIdsCount = -1;
            List<DueLocalBalanceM> myDueLocalBalanceListToUpdate;
            List<string> myClientAndVendorTypeGLAccountIds;
            var sw = Stopwatch.StartNew();
            try
            {

                using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(5)))
                {
                    var accountingContext = AccountingContext.GetContext(tenant);
                    if (fastRun)
                    {
                        InitDueLocalBalanceListToUpdate(accountingContext, tenant, AccountId, true, true);
                    }
                    else
                    {
                        InitDueLocalBalanceListToUpdate(accountingContext, tenant, AccountId, false, false);// WHY I CHANGE TO FALSE FALSE (FROM TRUE*2) 1 NO TIME 2 THE REVERSE DUE DATE RETURN LISt

                    }

                    myDueLocalBalanceListToUpdate = _QDueLocalBalanceListToUpdate.ToList();

                    myClientAndVendorTypeGLAccountIds = _QClientAndVendorTypeGLAccountIds.ToList();


                }
                clientAndVendorTypeGLAccountIdsCount = myClientAndVendorTypeGLAccountIds.Count;

                foreach (List<string> listBatch in myClientAndVendorTypeGLAccountIds.Batch(100))
                {
                    var myDefaultListToUpdate =
                        (
                        from myAccountId in listBatch
                        select new DueLocalBalanceM
                        {
                            AccountId = myAccountId,
                            RealDueInLocal = 0,
                            TotalLocalAmountLastMonthDue = decimalNull,
                            TotalForeignAmountLastMonthDue = decimalNull,
                            TransLocalAmountDueMonth = decimalNull,
                            TransForeignAmountDueMonth = decimalNull,
                            TransNextDueDate = dateTimeMinValue,
                        }

                        )
                        .ToList();

                    using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(5)))
                    {
                        var accountingContext = AccountingContext.GetContext(tenant);
                        //var myGLAccountQueryService = new GLAccountQueryService(accountingContext);
                        var myGLAccountMoreDataQueryService = new GLAccountMoreDataQueryService(accountingContext);
                        var myGLAccountMoreDataUpdateService
                            //= new GLAccountUpdateServiceBalancePriv(accountingContext, new Dictionary<string, IContext>(), tenant);
                            = new GLAccountMoreDataUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
                        var pmList = myGLAccountMoreDataQueryService.GetByGLAccountsIdList(listBatch, tenant);

                        foreach (var defaultItem in myDefaultListToUpdate)
                        {
                            var item2update = //_QDueLocalBalanceListToUpdate
                                myDueLocalBalanceListToUpdate
                                .FirstOrDefault(r => r.AccountId == defaultItem.AccountId);
                            item2update = item2update ?? defaultItem;
                            var pm = pmList.First(r => r.AccountId == item2update.AccountId);
                            if (pm.NextDueDate.GetValueOrDefault() == item2update.TransNextDueDate &&
                                pm.LocalBalanceInDue == item2update.RealDueInLocal &&
                                pm.ForeignBalanceInDue == item2update.RealDueInForeign
                                )
                            {
                            }
                            else
                            {
                                if (item2update.RealDueInLocal != 0)
                                {

                                }
                                var deltaLocalBalanceInDue = item2update.RealDueInLocal - pm.LocalBalanceInDue;
                                pm.LocalBalanceInDue = pm.LocalBalanceInDue + deltaLocalBalanceInDue;

                                var deltaForeignBalanceInDue = item2update.RealDueInForeign - pm.ForeignBalanceInDue;
                                pm.ForeignBalanceInDue = pm.ForeignBalanceInDue + deltaForeignBalanceInDue;

                                DateTime? nextDate = item2update.TransNextDueDate.Date;
                                if (nextDate == DateTime.MinValue || nextDate == DateTime.MinValue.Date)
                                {
                                    nextDate = null;
                                }
                                if (nextDate == DateTime.MaxValue || nextDate == DateTime.MaxValue.Date)
                                {
                                    nextDate = null;
                                }

                                pm.NextDueDate = nextDate;

                                pm.ChangeSetOp = ChangeSetOperation.Update;

                                //myGLAccountMoreDataUpdateService.Init(null, deltaLocalBalanceInDue, nextDate);
                                myGLAccountMoreDataUpdateService.Update(pm, true);
                            }

                        }
                        scope.Complete();
                    }
                    //Thread.Sleep(200000);//let Journal approval work 
                }

            }
            catch (Exception e)
            {
                LogMessagingUtil.Instance.AppendLine("DueLocalBalanceService Exception e=" + e.ToString());
                throw;
            }
            finally
            {

                LogMessagingUtil.Instance.AppendLine("DueLocalBalanceService tenant= " + tenant + "  accountIdList.Count=" + clientAndVendorTypeGLAccountIdsCount + ") took:" + sw.Elapsed.ToString());
            }
        }
        public List<DueLocalBalanceDiffM> ReverseEngineer(int tenant, string AccountId)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using (var scope = TransactionFactory.GetTransaction())
                {
                    var accountingContext = AccountingContext.GetContext(tenant);
                    InitDueLocalBalanceListToUpdate(accountingContext, tenant, AccountId, false, false);

                    var myGLAccountQueryService = new GLAccountQueryService(accountingContext);

                    var qClientAndVendorTypeGLAccount = myGLAccountQueryService.GetQByAccountTypeCodeList(tenant, _clientAndVendorType);
                    if (!String.IsNullOrWhiteSpace(AccountId))
                    {
                        qClientAndVendorTypeGLAccount = qClientAndVendorTypeGLAccount.Where(r => r.Id == AccountId);
                    }
                    var dbGLaccount =
                        (

                        from myGLAcc in qClientAndVendorTypeGLAccount
                        select new DueLocalBalanceM
                        {
                            AccountId = myGLAcc.Id,
                            RealDueInLocal = myGLAcc.LocalBalanceInDue == null ? 0 : (decimal)myGLAcc.LocalBalanceInDue,
                             RealDueInForeign = myGLAcc.ForeignBalanceInDue == null ? 0 : (decimal)myGLAcc.ForeignBalanceInDue,
                            TotalLocalAmountLastMonthDue = decimalNull,
                            TotalForeignAmountLastMonthDue = decimalNull,
                            TransLocalAmountDueMonth = decimalNull,
                            TransForeignAmountDueMonth = decimalNull,
                            TransNextDueDate = myGLAcc.NextDueDate == null
                                //? dateTimeMinValue : (DateTime)myGLAcc.NextDueDate,
                                ? dateTimeMaxValue : (DateTime)myGLAcc.NextDueDate,
                        }

                            );
                    //dbGLaccount.ToList();

                    var qDiff =
                        (
                   from db in dbGLaccount
                   join calc in _QDueLocalBalanceListToUpdate
                   on db.AccountId equals calc.AccountId into gjoin
                   from calcCanBeEmpty in gjoin.DefaultIfEmpty()

                       //where 
                       //!db.RealDueInLocal.Equals(calcCanBeEmpty.RealDueInLocal) ||
                       //!db.TransNextDueDate.Equals( calcCanBeEmpty.TransNextDueDate)


                   select new DueLocalBalanceDiffM()
                   {
                       AccountId = db.AccountId,
                       CalcDueInLocal = calcCanBeEmpty.RealDueInLocal == null ? 0 : calcCanBeEmpty.RealDueInLocal,
                       CalcDueInForeign= calcCanBeEmpty.RealDueInForeign == null ? 0 : calcCanBeEmpty.RealDueInForeign,
                       CalcNextDueDate =
                       //calcCanBeEmpty.TransNextDueDate == null ? dateTimeMinValue : calcCanBeEmpty.TransNextDueDate,
                       calcCanBeEmpty.TransNextDueDate == null ? dateTimeMaxValue : calcCanBeEmpty.TransNextDueDate,
                       DBDueInLocal = db.RealDueInLocal == null ? 0 : db.RealDueInLocal,
                       DBDueInForeign = db.RealDueInForeign == null ? 0 : db.RealDueInForeign,
                       //DBNextDueDate = db.TransNextDueDate == null ? dateTimeMinValue : db.TransNextDueDate,
                       DBNextDueDate = db.TransNextDueDate == null ? dateTimeMaxValue : db.TransNextDueDate,
                   }
                   );
                    qDiff =
                        (from a in qDiff
                         where (!a.CalcDueInLocal.Equals(a.DBDueInLocal) || !a.CalcDueInForeign.Equals(a.DBDueInForeign) ||  !a.CalcNextDueDate.Equals(a.DBNextDueDate))

                         select a);
                    var myDiffList = qDiff.Take(30).ToList();
                    myDiffList = myDiffList.Where(a => !a.CalcDueInLocal.Equals(a.DBDueInLocal) || !a.CalcDueInForeign.Equals(a.DBDueInForeign) || !a.CalcNextDueDate.Date.Equals(a.DBNextDueDate.Date)).ToList();
                    return myDiffList;


                }
            }
            catch (Exception e)
            {
                LogMessagingUtil.Instance.AppendLine("DueLocalBalanceService Exception e=" + e.ToString());
                throw;
            }
            finally
            {
                LogMessagingUtil.Instance.AppendLine("DueLocalBalanceService tenant= " + tenant + " took:" + sw.Elapsed.ToString());
            }
        }

        private void InitDueLocalBalanceListToUpdate(IAccountingContext accountingContext, int tenant, string AccountId, bool filterByNextDueDate, bool onlyWithActivity)
        {
            //using (var scope = TransactionFactory.GetTransaction())
            {
                accountingContext = accountingContext ?? AccountingContext.GetContext(tenant);
                var repoGLAccountTotalByMonths = new GLAccountTotalByMonthRepository(accountingContext);
                var repoLedgerTransactionRepository = new LedgerTransactionRepository(accountingContext);
                var myGLAccountQueryService = new GLAccountQueryService(accountingContext);

                IQueryable<string> qClientAndVendorTypeGLAccountIds = null;

                var qClientAndVendorTypeGLAccount = myGLAccountQueryService.GetQByAccountTypeCodeList(tenant, _clientAndVendorType);

                if (filterByNextDueDate)
                {
                    qClientAndVendorTypeGLAccount =
                        qClientAndVendorTypeGLAccount.Where(a => a.NextDueDate <= _today);

                }
                var qGlAccountIdIsMulti = qClientAndVendorTypeGLAccount.Select(r => new { r.Id, r.IsMultiCurrency });

                qClientAndVendorTypeGLAccountIds = qGlAccountIdIsMulti.Select(r => r.Id);
                if (!string.IsNullOrWhiteSpace(AccountId))
                {
                    qClientAndVendorTypeGLAccountIds = qClientAndVendorTypeGLAccountIds.Where(r => r == AccountId);
                }


                var qTotalLocalAmountLastMonthDue =
                    (from totDueDate in repoGLAccountTotalByMonths.GetQTotalByDateTypeCodeUntil(tenant, _lastMonth, qClientAndVendorTypeGLAccountIds, GLAccountTotalDateTypeValues.DueDate)
                     join a in qGlAccountIdIsMulti on totDueDate.AccountId equals a.Id
                     group totDueDate by new { totDueDate.AccountId, a.IsMultiCurrency } into g
                     select new DueLocalBalanceM
                     {
                         AccountId = g.Key.AccountId,
                         RealDueInLocal = 0,
                          RealDueInForeign=0,
                         TotalLocalAmountLastMonthDue = g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit),
                         TotalForeignAmountLastMonthDue = g.Key.IsMultiCurrency == true ? g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit) : g.Sum(r => r.ForeignAmountDebit - r.ForeignAmountCredit),
                         TransLocalAmountDueMonth = 0,
                         TransForeignAmountDueMonth = 0,
                         TransNextDueDate = dateTimeMaxValue,
                     });


                //var listBatch = qClientAndVendorTypeGLAccountIds.ToList();
                var qLedgerTrans =
                    (
                    from transDueDate in repoLedgerTransactionRepository.GetQBetweenDueInclusive(tenant, qClientAndVendorTypeGLAccountIds, _thisBeginOfMonth, _today)
                    join a in qGlAccountIdIsMulti on transDueDate.AccountId equals a.Id
                    group transDueDate by new { transDueDate.AccountId, a.IsMultiCurrency } into g
                    select new DueLocalBalanceM
                    {
                        AccountId = g.Key.AccountId,
                        RealDueInLocal = 0,
                         RealDueInForeign =0,
                        TotalLocalAmountLastMonthDue = 0,
                        TotalForeignAmountLastMonthDue = 0,
                        TransLocalAmountDueMonth = g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit),
                        TransForeignAmountDueMonth = g.Key.IsMultiCurrency == true ? g.Sum(r => r.LocalAmountDebit - r.LocalAmountCredit) : g.Sum(r => r.ForeignAmountDebit - r.ForeignAmountCredit),
                        TransNextDueDate = dateTimeMaxValue,
                    });


                var nextStep =
                    (from transDueDate in
                         repoLedgerTransactionRepository.GetQDueGreaterthan(tenant, qClientAndVendorTypeGLAccountIds, _today)
                         //    accountingContext.LedgerTransactions
                         //where transDueDate.Tenant == tenant
                         //where listBatch.Contains(transDueDate.AccountId)
                         //where transDueDate.DueDate > today
                     group transDueDate by transDueDate.AccountId into g
                     select new DueLocalBalanceM
                     {
                         AccountId = g.Key,
                         RealDueInLocal = 0,
                          RealDueInForeign=0,
                         TotalLocalAmountLastMonthDue = 0,
                         TotalForeignAmountLastMonthDue = 0,
                         TransLocalAmountDueMonth = 0,
                         TransForeignAmountDueMonth = 0,
                         TransNextDueDate = g.Min(r => r.DueDate),
                     });

                var all = qTotalLocalAmountLastMonthDue.Union(qLedgerTrans).Union(nextStep);
                bool UnionreturnsDistinctvalues = true;
                if (UnionreturnsDistinctvalues)
                {
                    all = qTotalLocalAmountLastMonthDue.Concat(qLedgerTrans).Concat(nextStep);
                }

                if (!string.IsNullOrWhiteSpace(AccountId))
                {
                    var allList = all.ToList();
                }
                var theDueLocalBalanceListToUpdate =
                (from m in all
                 group m by m.AccountId into g
                 select new DueLocalBalanceM
                 {
                     AccountId = g.Key,
                     //RealDueInLocal = g.Sum(r => r.TotalLocalAmountLastMonthDue) ?? 0 + g.Sum(r => r.TransLocalAmountDueMonth) ?? 0,
                     RealDueInLocal = g.Sum(r => r.TotalLocalAmountLastMonthDue + r.TransLocalAmountDueMonth) ?? 0,
                     RealDueInForeign = g.Sum(r => r.TotalForeignAmountLastMonthDue + r.TransForeignAmountDueMonth) ?? 0,
                     TotalLocalAmountLastMonthDue = decimalNull,
                     TotalForeignAmountLastMonthDue = decimalNull,
                     TransLocalAmountDueMonth = decimalNull,
                     TransForeignAmountDueMonth = decimalNull,
                     TransNextDueDate = g.Min(r => r.TransNextDueDate),
                 }
                );

                if (onlyWithActivity)
                {
                    theDueLocalBalanceListToUpdate =
                        theDueLocalBalanceListToUpdate
                        .Where(g => g.TransNextDueDate != dateTimeMaxValue || g.RealDueInLocal != null || g.RealDueInForeign != null);
                }




                if (!string.IsNullOrWhiteSpace(AccountId))
                {
                    var allList = theDueLocalBalanceListToUpdate.ToList();
                }
                _QDueLocalBalanceListToUpdate = theDueLocalBalanceListToUpdate;
                _QClientAndVendorTypeGLAccountIds = qClientAndVendorTypeGLAccountIds
                    //.ToList()
                    ;
            }
        }

        public virtual DateTime GetToday()
        {
            return DateTime.UtcNow.Date;
        }

        internal void RunAllTenants()
        {
            var qs = new GLAccountQueryService(0);
            DateTime today = GetToday().Date;
            DateTime thisBeginOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime lastMonth = today.AddMonths(-1);
            var clientAndVendorType = new List<string>() { "2", "3" }; //AccoutTypeCode= 2 or 3 (clients/Vendors)
            var tenantList = qs.GetTenantByNextDueDate(today, clientAndVendorType);
            foreach (var tenant in tenantList)
            {
                try
                {
                    this.ReBuild(tenant, "", true);
                }
                catch (Exception e)
                {

                    ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "DueLocalBalanceService" + this.GetType().Name, " : Run() Method", null);
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }

            }

        }
    }
    public class DueLocalBalanceDiffM
    {
        public string AccountId { get; set; }

        public decimal CalcDueInLocal { get; set; }
        public decimal CalcDueInForeign { get; set; }
        public DateTime CalcNextDueDate { get; set; }
        
        public decimal DBDueInLocal { get; set; }
        public decimal DBDueInForeign { get; set; }
        public DateTime DBNextDueDate { get; set; }
        
    }
    public class DueLocalBalanceM
    {


        public string AccountId { get; set; }



        public decimal? TotalLocalAmountLastMonthDue { get; set; }

        public decimal? TotalForeignAmountLastMonthDue { get; set; }

        public decimal? TransLocalAmountDueMonth { get; set; }

        public decimal? TransForeignAmountDueMonth;

        public DateTime TransNextDueDate { get; set; }

        public decimal RealDueInLocal { get; set; }
        public decimal RealDueInForeign { get; set; }
    }
}
