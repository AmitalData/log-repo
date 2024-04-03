using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityUpdateServicesExt
{
    public interface IAccountingEntityJournalUpdateServiceExt
    {
        void Update(AccountingEntityJournalPM entityPM);
        void AddAccountingEntitieJournal(JournalPM journalPM, string actionName, string childEntityId = null);

    }
}
