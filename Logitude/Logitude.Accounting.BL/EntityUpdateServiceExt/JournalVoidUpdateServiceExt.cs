using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServiceExt
{
    public class JournalVoidUpdateServiceExt : IJournalVoidUpdateServiceExt
    {
        public void Update(JournalPM journalPM, StornoOverrideM stornoOverrideM)
        {

            // Void it!
            var tenant = journalPM.Tenant;
            var MyContext = AccountingContext.GetContext(tenant);
            var service = new JournalVoidUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            //var stornoOverrideM = new StornoOverrideM()
            //{
            //    AccountingEntityCode = journalPM.AccountingEntityCode,
            //    AccountingEntityId = journalPM.AccountingEntityId,
            //    AccountingEntityReference = journalPM.AccountingEntityReference,
            //};
            service.VoidJournal(journalPM.Id, tenant, stornoOverrideM);

        }

    }
}
