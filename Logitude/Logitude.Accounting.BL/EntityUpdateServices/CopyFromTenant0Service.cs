using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class CopyFromTenant0UpdateService : EntityUpdateService<CopyFromTenant0, CopyFromTenant0PM, EntityPM>
    {
      
        protected override void OnUpdating(CopyFromTenant0PM entityPM)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            entityPM.Tenant = authToken.Tenant;
            entityPM.CreateDate= DateTime.Now;

        }
        

    }
}
