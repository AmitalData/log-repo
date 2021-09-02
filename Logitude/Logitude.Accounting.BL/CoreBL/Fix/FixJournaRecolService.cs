
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Fix
{
    public class FixJournaRecolService
    {
        public void FixByJournalNumber(string journalNumber, int tenant)
        {
            var repo = new JournalRepository(tenant);
            var poco=repo.GetSingleJournalByNumber(journalNumber, tenant);
            Fix(poco.Id, tenant,false);
        }
        public void Fix(string journalId ,int tenant,bool  clearIt)
        {
#if true

            using (var scope= TransactionFactory.GetTransaction())
            {
                var accountingContext = AccountingContext.GetContext(tenant);
                var qs = new JournalQueryService(accountingContext);
                var journalPM = qs.GetSingle(journalId, true, false);
                if (journalPM.IsLedgerCreated)
                {
                    throw new Exception("IsLedgerCreated");
                }
                if (journalPM.IsVoided.GetValueOrDefault())
                {
                    throw new Exception("IsVoided not check ");
                }
                bool allowedFixAR = true;
                if (!allowedFixAR)
                {
                    //10	התאמה	Adjustment
                    //12	התאמת בנק	Bank Adjustment 
                    bool whileStreming = (journalPM.AccountingEntityCode == "10" || journalPM.AccountingEntityCode == "12");
                    if (!whileStreming)
                    {
                        throw new Exception("AccountingEntityCode =10Adjustment/12 Bank Adjustment ");
                    }
                }
                int sum = journalPM.JournalReconciles.Count + journalPM.JournalExternalReconciles.Count();
                if (sum == 0)
                {
                    throw new Exception("no line :pm.JournalReconciles.Count + pm.JournalExternalReconciles.Count()");
                }
                var legderRepo = new LedgerTransactionRepository(accountingContext);
                string ledgerTransactionId = legderRepo.GetAnyLedgerTransactionByJournalId(journalId, tenant);
                if (!string.IsNullOrWhiteSpace(ledgerTransactionId))
                {
                    throw new Exception($"GetAnyLedgerTransactionByJournalId {ledgerTransactionId}");
                }
                //UpdateInProgressReconcilation(accountingContext, journalPM);
                //UpdateInProgressExternalReconciles(accountingContext, journalPM);

                journalPM.JournalReconciles.ForEach(r => r.ChangeSetOp = ChangeSetOperation.Delete);
                journalPM.JournalExternalReconciles.ForEach(r => r.ChangeSetOp = ChangeSetOperation.Delete);
                journalPM.ChangeSetOp = ChangeSetOperation.Update;
                if (clearIt && (journalPM.AccountingEntityCode == "10" || journalPM.AccountingEntityCode == "12"))
                {
                    journalPM.StatusCode = "0";
                    journalPM.AccountingEntityCode = "1";

                }
                var journalUpdateService = new JournalUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), journalPM.Tenant);
                journalUpdateService.Update(journalPM, true);
                accountingContext.SaveChanges();
                scope.Complete();

            }
#endif
        }

        private static void UpdateInProgressExternalReconciles(IAccountingContext accountingContext, Def.EntityPMs.JournalPM journalPM)
        {
            if (journalPM.JournalExternalReconciles.Count > 0)
            {
                var myLedgerTransactionUpdateService = new LedgerTransactionUpdateService(accountingContext, new Dictionary<string, IContext>(), journalPM.Tenant);

                var listTransactionId = journalPM.JournalExternalReconciles.Select(r => r.LedgerTransactionId).ToList();
                if (listTransactionId.Count > 0)
                {
                    myLedgerTransactionUpdateService.Update_InProgressExternalReconcile(listTransactionId, journalPM.Tenant, true);
                }

                var listReconcileExternalPageLineId = journalPM.JournalExternalReconciles.Select(r => r.ReconcileExternalPageLineId).ToList();
                if (listReconcileExternalPageLineId.Count > 0)
                {
                    var reconcileExternalPageLineUpdateService = new ReconcileExternalPageLineUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), journalPM.Tenant);
                    reconcileExternalPageLineUpdateService.Update_InProgressExternalReconcile(listReconcileExternalPageLineId, journalPM.Tenant, false);
                    accountingContext.SaveChanges();
                }
            }
        }

        private static void UpdateInProgressReconcilation(IAccountingContext accountingContext, Def.EntityPMs.JournalPM journalPM)
        {
            if (journalPM.JournalReconciles.Count > 0)
            {
                var listIds = journalPM.JournalReconciles.Select(jr => jr.LedgerTransactionId).ToList();
                var myLedgerTransactionUpdateService = new LedgerTransactionUpdateService(accountingContext, new Dictionary<string, IContext>(), journalPM.Tenant);

                var listTransactionId = journalPM.JournalReconciles.Select(r => r.LedgerTransactionId).ToList();
                if (listTransactionId.Count > 0)
                {
                    myLedgerTransactionUpdateService.UpdateInReconcileProgress(listTransactionId, journalPM.Tenant, false);
                    accountingContext.SaveChanges();
                }

            }
        }
    }
}
