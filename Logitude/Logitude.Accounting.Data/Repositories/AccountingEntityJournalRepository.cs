 
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
   public partial class AccountingEntityJournalRepository:IRepository<AccountingEntityJournal>
   {
        
		public List<AccountingEntityJournal> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public bool Exists(int tenant, string accountingEntityId, string accountingEntityCode, string action, string childEntityId)
        {
            return (from a in context.AccountingEntitiesJournals
                    where a.AccountingEntityId == accountingEntityId && a.AccountingEntityCode == accountingEntityCode
                    && a.Action == action && a.ChildEntityId == childEntityId
                    && a.Tenant == tenant
                    select a).Any();
        }

    }

}
   