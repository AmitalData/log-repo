using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class ReconcileMethodUpdateService : EntityUpdateService<ReconcileMethod, ReconcileMethodPM, EntityPM>
    {
        protected override void OnCreating(ReconcileMethodPM entityPM, EntityPM entityParentPM)
        {
            // entityPM.Code = IdCounter.GetNumber("Accounting.ReconcileMethod", entityPM.Tenant);
        }
    }
}
