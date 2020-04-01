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
    public class BankCodeUpdateServiceExt : IBankCodeUpdateServiceExt
    {
        public BankCodeUpdateServiceExt()
        {

        }

        public void Update(BankCodePM entityPM)
        {
            IAccountingContext context = AccountingContext.GetContext(entityPM.Tenant);
            BankCodeUpdateService service = new BankCodeUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entityPM.Tenant);
            service.Update(entityPM, true);
        }
    }
}
