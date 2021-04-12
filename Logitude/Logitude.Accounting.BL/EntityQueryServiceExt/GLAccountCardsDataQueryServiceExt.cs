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
   public  class GLAccountCardsDataQueryServiceExt: IGLAccountCardsDataQueryServiceExt
    {
        public GLAccountCardsDataQueryServiceExt()
        {

        }

        public GLAccountCardsDataPM GetSingleGLAccountCardsData(string id, int tenant)
        {
            GLAccountCardsDataQueryService query = new GLAccountCardsDataQueryService(tenant);
            return query.GetSingle(id, false, false);
        }
    }
}
