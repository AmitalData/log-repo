using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeficitDecisionUpdateService : EntityUpdateService<DeficitDecision, DeficitDecisionPM, DeficitPM>
    {
        protected override void OnCreating(DeficitDecisionPM entityPM, DeficitPM entityParentPM)
        {
            if (entityParentPM == null)
            {
                return;
            }
            entityPM.DeficitId = entityParentPM.Id;
            entityPM.Tenant = entityPM.Tenant;

            base.OnCreating(entityPM, entityParentPM);

        }
    }
}
