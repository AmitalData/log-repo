using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityQueryServicesExt
{
   public  interface IGLAccountCurrencyQueryServiceExt
    {
         GLAccountCurrencyPM GetEntityByGLAccountId(string id, int tenant);
    }
}
