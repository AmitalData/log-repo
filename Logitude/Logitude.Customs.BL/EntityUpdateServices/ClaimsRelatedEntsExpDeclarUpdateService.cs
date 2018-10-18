using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClaimsRelatedEntsExpDeclarUpdateService
    {
        protected override void OnCreating(ClaimsRelatedEntsExpDeclarPM entityPM, ClaimsRelatedEntityPM entityParentPM)
        {
            if (entityParentPM == null)
            {
                return;
            }
            entityPM.ClaimId = entityParentPM.ClaimId;
            entityPM.Tenant = entityParentPM.Tenant;
            entityPM.CounterKey = entityParentPM.EntityCounterKey;

            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
