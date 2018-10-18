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
    public partial class PeriodTypeUpdateService : EntityUpdateService<PeriodType, PeriodTypePM, EntityPM>
    {
        protected override void OnCreating(PeriodTypePM entityPM, EntityPM entityParentPM)
        {
            // entityPM.Code = IdCounter.GetNumber("PeriodType", entityPM.Tenant);
        }
    }
}
