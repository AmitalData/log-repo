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
    public class CashBookUpdateServiceExt : ICashBookUpdateServiceExt
    {
        public CashBookUpdateServiceExt()
        {

        }

        public void Update(CashBookPM entityPM)
        {
            IAccountingContext context = AccountingContext.GetContext(entityPM.Tenant);
            CashBookUpdateService service = new CashBookUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(),entityPM.Tenant);
            service.Update(entityPM, true);
        }
    }
}
