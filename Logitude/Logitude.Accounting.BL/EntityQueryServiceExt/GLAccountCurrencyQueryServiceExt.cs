using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServiceExt
{
   public class GLAccountCurrencyQueryServiceExt: IGLAccountCurrencyQueryServiceExt
    {
        public GLAccountCurrencyQueryServiceExt()
        {

        }
        public GLAccountCurrencyPM GetEntityByGLAccountId(string id , int tenant)
        {
            GLAccountCurrencyQueryService gLAccountCurrencyQueryService = new GLAccountCurrencyQueryService(tenant);
            return gLAccountCurrencyQueryService.GetEntityByGLAccountId(id, tenant);
        }
    }
}
