using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial class GLAccountFollowUpDataQueryService
    {
        public GLAccountFollowUpDataPM GetSinglePMByAccountId(string accountId, int tenant)
        {
            GLAccountFollowUpData accountfollowUpData = repository.GetSingleByAccountId(accountId, tenant);
            return GetEntityPM(accountfollowUpData);
        }
    }
}
