using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.APIDataContract.ApiV1
{
   public partial class GLAccountMoreDataQueryService
    {
       public bool CheckIfGLAccountHasMoreData(string id, int tenant)
        {
            Logitude.Accounting.BL.EntityQueryServices.GLAccountMoreDataQueryService accountMoreDataQueryService = new EntityQueryServices.GLAccountMoreDataQueryService(tenant);
            return accountMoreDataQueryService.CheckIfGLAccountHasMoreDataRecord(id, tenant);
        }


    }
}
