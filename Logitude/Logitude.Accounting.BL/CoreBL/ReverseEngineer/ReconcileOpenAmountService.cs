using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ReverseEngineer
{
    public class ReconcileOpenAmountService
    {

        public List<LedgerOpenAmountRecoDiffM> GetLedgerOpenAmountDiff(int tenant, int yyyy)
        {
            using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(30)))
            {

                var _AccountingContext = AccountingContext.GetContext(tenant);
                if ((_AccountingContext as System.Data.Entity.DbContext).Database.CommandTimeout < 1200)//wrokerrole mode !!!
                {
                    (_AccountingContext as System.Data.Entity.DbContext).Database.CommandTimeout = 1200;
                }

                var myGLAccountRepository = new GLAccountRepository(_AccountingContext);
                var myLedgerTransactionRepository = new LedgerTransactionRepository(_AccountingContext);
                var myJournalRepository = new JournalRepository(_AccountingContext);
                var myReconciliationRepository = new ReconciliationRepository(_AccountingContext);
                var myReconciliationLineRepository = new ReconciliationLineRepository(_AccountingContext);

                var qTotalReconciliationAmountPerTranId = (from rl in myReconciliationLineRepository.GetAll(tenant)
                                                           join r in myReconciliationRepository.GetAll(tenant).Where(r=>!r.IsCancelled)
                                                           on rl.ReconciliationId equals r.Id

                                                           group rl by rl.TransactionId into grl
                                                           select new LedgerOpenAmountRecoDiffM
                                                           {
                                                               LedgerTransactionId = grl.Key,
                                                               OpenAmount = 0,
                                                               LedgerAmount = 0,
                                                               TotalReconciliationAmount = grl.Sum(r => r.ReconciliationAmount),
                                                               ReconcileMethod=""
                                                               
                                                           }
                         );

                string LocalCurrency = ((int)ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString();
                string ForeignCurrency = ((int)ReconcileMethodPM.ReconcileMethodEnum.ForeignCurrency).ToString();

                DateTime stratDate = new DateTime(yyyy, 1, 1);
                DateTime endDate = new DateTime(yyyy + 1, 1, 1);
                /////
                ///כחלק מהתוכנית לבדיקת תקינות מערכת
                ///יש לבדוק תנועות פתוחות שמקורם אינו חיצוני(לא ממפס)
                //  סך התנועה צריך להיות שווה לסכום פתוח כל ההתאמות של אותה תנועה
                var q = (from l in myLedgerTransactionRepository.GetAll(tenant)
                         .Where(r => r.IsReconciled == false)
                         .Where(r => r.AccountingDate >= stratDate && r.AccountingDate < endDate)
                         join j in myJournalRepository.GetAll(tenant).Where(r => String.IsNullOrEmpty(r.ExternalNo)) on l.JournalId equals j.Id
                         join acc in myGLAccountRepository.GetAll(tenant) on l.AccountId equals acc.Id
                         join rl in qTotalReconciliationAmountPerTranId on l.Id equals rl.LedgerTransactionId into outerj_rl
                         from subRL in outerj_rl.DefaultIfEmpty()
                         select new LedgerOpenAmountRecoDiffM
                         {
                             LedgerTransactionId = l.Id,
                             OpenAmount = l.OpenAmount,
                             LedgerAmount = (acc.ReconcileMethodCode == LocalCurrency) ? (l.LocalAmountDebit - l.LocalAmountCredit) : (l.ForeignAmountDebit - l.ForeignAmountCredit),
                             TotalReconciliationAmount = (subRL.TotalReconciliationAmount != null) ? subRL.TotalReconciliationAmount : 0,
                             ReconcileMethod = (acc.ReconcileMethodCode == LocalCurrency) ? "LocalCurrency" : "ForeignCurrency"


                         });

                q = q.Where(r => r.OpenAmount + r.TotalReconciliationAmount != r.LedgerAmount);
                var list = q.ToList();

                return list;
            }
        }
    }
    public class LedgerOpenAmountRecoDiffM
    {
        public string LedgerTransactionId { get;  set; }
        public decimal? OpenAmount { get; set; }
        public decimal? LedgerAmount { get; set; }
        public decimal? TotalReconciliationAmount { get; set; }
        public string ReconcileMethod { get; set; }
    }
}