using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class GLAccountRecocileDataQueryService : EntityQueryService<GLAccountRecocileData, GLAccountRecocileDataKeys, GLAccountRecocileDataPM, object, GLAccountRecocileDataKeys>
    {

        public bool Exists(string accountId, int tenant)
        {
            return repository.GetAll(tenant)
                             .Any(a => a.AccountId == accountId && a.Tenant == tenant);
        }
    }
}
