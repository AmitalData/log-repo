using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class JournalActionTypeQueryService : EntityQueryService<JournalActionType, JournalActionTypeKeys, JournalActionTypePM, object, JournalActionTypeKeys>
    {
        public bool CheckWhetherCodeExists(string code, string id, int tenant)
        {
            return this.repository.CheckWhetherCodeExists(code, id, tenant);
        }


    }
}
