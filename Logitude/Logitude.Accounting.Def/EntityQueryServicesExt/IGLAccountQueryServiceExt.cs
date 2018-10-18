using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityQueryServicesExt
{
    public interface IGLAccountQueryServiceExt
    {
        GLAccountPM GetSingleGLAccountPM(string id, int tenant);
     //   GLAccount GetGLAccountById(string id, int tenant);
    }
}
