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
    public class JournalUpdateServiceExt : IJournalUpdateServiceExt
    {
        
        public JournalUpdateServiceExt()
        {

        }

        public void Update(JournalPM journal)
        {
            IAccountingContext context = AccountingContext.GetContext(journal.Tenant);
            JournalUpdateService service = new JournalUpdateService(context, new Dictionary<string,Simplog.Server.Infrastructure.IContext>(),journal.Tenant);
            service.Update(journal, true);
        }
    }
}
