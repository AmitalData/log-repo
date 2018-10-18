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
    public class ARPaymentChequeUpdateServiceExt: IARPaymentChequeUpdateServiceExt
    {
        public ARPaymentChequeUpdateServiceExt()
        {

        }

        public void Update(ARPaymentChequePM entity)
        {
            IAccountingContext context = AccountingContext.GetContext(entity.Tenant);
            ARPaymentChequeUpdateService service = new ARPaymentChequeUpdateService(context,new Dictionary<string,Simplog.Server.Infrastructure.IContext>(), entity.Tenant);
            service.Update(entity, true);
        }
    }
}
