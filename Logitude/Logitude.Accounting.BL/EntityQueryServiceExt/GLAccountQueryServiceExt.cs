using Logitude.Accounting.BL.APIDataContract.ApiV1;
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
    public class GLAccountQueryServiceExt : IGLAccountQueryServiceExt
    {
        public GLAccountQueryServiceExt()
        {

        }

        public GLAccountPM GetSingleGLAccountPM(string id, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);
            return query.GetSinglePM(id, tenant);
        }



        public GLAccountPM GetGLAccountByDisplayNumber(string number, int tenant)
        {
            EntityQueryServices.GLAccountQueryService query = new EntityQueryServices.GLAccountQueryService(tenant);
           
     
            GLAccountPM gLAccount = query.GetSinglePMByDisplayNumber(number, tenant);

            return gLAccount;
              
           
           
        }

       


    }
}
