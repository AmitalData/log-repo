 
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
   public partial class JournalExternalReconcileRepository:IRepository<JournalExternalReconcile>
   {
        
		public List<JournalExternalReconcile> GetMulti(EntityKeyFields entityKeys)
        {

            JournalKeys journalKeys = entityKeys as JournalKeys;

            return (from a in context.JournalExternalReconciles
                    where a.JournalId == journalKeys.Id
                    select a).ToList();
        }

        public List<JournalExternalReconcile> GetJournalExternalReconcilesByLedgerTransactionsIds(List<string> ledgerTransactionsIds)
        {

            return (from a in context.JournalExternalReconciles
                    join jr in context.Journals on a.JournalId equals jr.Id
                    where ledgerTransactionsIds.Contains(a.LedgerTransactionId) && jr.StatusCode != "3"
                    select a).ToList();
        }
    }

}
   