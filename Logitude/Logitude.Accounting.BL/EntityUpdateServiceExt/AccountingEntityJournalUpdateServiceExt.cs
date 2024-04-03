using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServiceExt
{
    public class AccountingEntityJournalUpdateServiceExt : IAccountingEntityJournalUpdateServiceExt
    {
        
        public AccountingEntityJournalUpdateServiceExt()
        {

        }

        public void AddAccountingEntitieJournal(JournalPM journalPM, string actionName, string childEntityId = null)
        {
            IAccountingContext context = AccountingContext.GetContext(journalPM.Tenant);
            AccountingEntityJournalUpdateService service = new AccountingEntityJournalUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), journalPM.Tenant);
            service.AddAccountingEntitieJournal(journalPM, actionName, childEntityId);
        }

        public void Update(AccountingEntityJournalPM entityPM)
        {
            IAccountingContext context = AccountingContext.GetContext(entityPM.Tenant);
            AccountingEntityJournalUpdateService service = new AccountingEntityJournalUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            service.Update(entityPM, true);
        }
    }
}
