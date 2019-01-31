using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class GLAccountMoreDataQueryService : EntityQueryService<GLAccountMoreData, GLAccountMoreDataKeys, GLAccountMoreDataPM, object, GLAccountMoreDataKeys>
    {

        internal List<GLAccountMoreDataPM> GetByGLAccountsIdList(List<string> GLAccountsIdList, int tenant)
        {
            var pms = (from a in repository.GetAll(tenant)
                       where GLAccountsIdList.Contains(a.AccountId) && a.Tenant == tenant
                       select a)
                       .ToList()
                       .Select(a => GetEntityPM(a))
                       .ToList();
            return pms;
        }
    }
}
