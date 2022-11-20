 
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

        public List<JournalReconcile> GetJournalReconcilesByLedgerTransactionsIds(List<string> ledgerTransactionsIds)
        {

            return (from a in context.JournalReconciles
                    join jr in context.Journals on a.JournalId equals jr.Id
                    where ledgerTransactionsIds.Contains(a.LedgerTransactionId) && jr.StatusCode != "3"
                    select a).ToList();
        }
    }

}
   