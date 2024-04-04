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
    public class GLAccountUpdateServiceExt : IGLAccountUpdateServiceExt
    {
        public GLAccountUpdateServiceExt()
        {

        }

        public void Update(GLAccountPM entity)
        {
            IAccountingContext context = AccountingContext.GetContext(entity.Tenant);
            GLAccountUpdateService service = new GLAccountUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entity.Tenant);
            service.Update(entity, true);
        }

        public void Create(GLAccountPM entity)
        {
            IAccountingContext context = AccountingContext.GetContext(entity.Tenant);
            GLAccountUpdateService service = new GLAccountUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), entity.Tenant);
            service.Update(entity, true);
        }

        public void UpdateGLAccountWithAdditionalData(string accountId,int tenant, string excludeCardId, string excludeContactId, string includeContactId)
        {
            IAccountingContext context = AccountingContext.GetContext(tenant);
            GLAccountUpdateService service = new GLAccountUpdateService(context, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            service.UpdateGLAccountWithAdditionalData(accountId, tenant, excludeCardId, excludeContactId, includeContactId);
        }
    }
}
