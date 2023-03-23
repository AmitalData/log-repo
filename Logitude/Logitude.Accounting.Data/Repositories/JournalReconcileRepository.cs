 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class JournalReconcileRepository:IRepository<JournalReconcile>
   {
        
		public List<JournalReconcile> GetMulti(EntityKeyFields entityKeys)
        {

            JournalKeys journalKeys = entityKeys as JournalKeys;

            return (from a in context.JournalReconciles
                    where a.JournalId == journalKeys.Id
                    select a).ToList();
        }

        public List<ReconciliationLine> GetReconciliationLinesByLedgerTransactionsIds(List<string> ledgerTransactionsIds)
        {

            return (from rl in context.ReconciliationLines
                    join r in context.Reconciliations on rl.ReconciliationId equals r.Id
                    where ledgerTransactionsIds.Contains(rl.TransactionId) && !r.IsCancelled
                    select rl).ToList();
        }

        public List<JournalReconcile> GetJournalReconcilesForJournalsWithoutLedgers(List<string> ledgerTransactionsIds, int tenant)
        {

            return (from jr in context.JournalReconciles
                    join j in context.Journals on jr.JournalId equals j.Id
                    where j.Tenant == tenant && j.StatusCode == "2" && j.IsLedgerCreated == false &&  ledgerTransactionsIds.Contains(jr.LedgerTransactionId)
                    select jr).ToList();
        }
    }

}
   