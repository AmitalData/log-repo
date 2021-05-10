
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.Repositories;
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
        public void Fix(string journalId ,int tenant)
        {
#if false


            using (var scope= TransactionFactory.GetTransaction())
            {
                
                var qs = new JournalQueryService(tenant);
                var journalPM = qs.GetSingle(journalId, true,false);
                if(journalPM.IsLedgerCreated)
                {
                    throw new Exception("IsLedgerCreated");
                }
                if (journalPM.IsVoided.GetValueOrDefault())
                {
                    throw new Exception("IsVoided not check ");
                }
                //10	התאמה	Adjustment
                //12	התאמת בנק	Bank Adjustment 
                bool whileStreming = (journalPM.AccountingEntityCode == "10" || journalPM.AccountingEntityCode == "12");
                if (!whileStreming)
                {
                    throw new Exception("AccountingEntityCode =10Adjustment/12 Bank Adjustment ");
                }
                int sum = journalPM.JournalReconciles.Count + journalPM.JournalExternalReconciles.Count();
                if (sum==0)
                {
                    throw new Exception("no line :pm.JournalReconciles.Count + pm.JournalExternalReconciles.Count()");
                }
                var legderRepo = new LedgerTransactionRepository(tenant);
                string ledgerTransactionId = legderRepo.GetAnyLedgerTransactionByJournalId(journalId, tenant);
                if (!string.IsNullOrWhiteSpace(ledgerTransactionId))
                {
                    throw new Exception($"GetAnyLedgerTransactionByJournalId {ledgerTransactionId}");
                }
                if (journalPM.JournalReconciles.Count>0)
                {
                    var listIds= journalPM.JournalReconciles.Select(jr => jr.LedgerTransactionId).ToList();
                    var myLedgerTransactionUpdateService = new LedgerTransactionUpdateService(_AccountingContext, new Dictionary<string, IContext>(), _JournalPM.Tenant);

                    var listTransactionId = _JournalPM.JournalReconciles.Select(r => r.LedgerTransactionId).ToList();
                    if (listTransactionId.Count > 0)
                    {
                        myLedgerTransactionUpdateService.UpdateInReconcileProgress(listTransactionId, _JournalPM.Tenant, false);
                    }

                }





            }
#endif
        }
    }
}
