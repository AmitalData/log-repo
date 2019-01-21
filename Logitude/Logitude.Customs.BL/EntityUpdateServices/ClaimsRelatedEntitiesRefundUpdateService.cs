using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClaimsRelatedEntitiesRefundUpdateService
    {
        protected override void OnCreating(ClaimsRelatedEntitiesRefundPM entityPM, ClaimsRelatedEntityPM entityParentPM)
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
