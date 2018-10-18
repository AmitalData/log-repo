using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClaimsRelatedEntsReasonsExpUpdateService
    {
        protected override void OnCreating(ClaimsRelatedEntsReasonsExpPM entityPM, ClaimsRelatedEntitiesReasonPM entityParentPM)
        {
            if (entityParentPM == null)
            {
                return;
            }
            entityPM.ClaimId = entityParentPM.ClaimId;
            entityPM.Tenant = entityParentPM.Tenant;
            entityPM.CounterKey = entityParentPM.CounterKey;
            entityPM.ReasonLineNo = entityParentPM.LineNo;

            int line = 0;
            if (entityParentPM.ClaimsRelatedEntsReasonsExps.Count > 0)
            {
                line = entityParentPM.ClaimsRelatedEntsReasonsExps.Max(d => d.LineNo);
            }
            entityPM.LineNo = line + 1;

            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
