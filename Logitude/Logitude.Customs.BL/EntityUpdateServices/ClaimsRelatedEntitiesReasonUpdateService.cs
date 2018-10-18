using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClaimsRelatedEntitiesReasonUpdateService
    {
        protected override void OnCreating(ClaimsRelatedEntitiesReasonPM entityPM, ClaimsRelatedEntityPM entityParentPM)
        {
            if (entityParentPM == null)
            {
                return;
            }
            entityPM.ClaimId = entityParentPM.ClaimId;
            entityPM.CounterKey = entityParentPM.EntityCounterKey;
            entityPM.Tenant = entityParentPM.Tenant;

            int line = 0;
            if (entityParentPM.ClaimsRelatedEntitiesReasons.Count > 0)
            {
                line = entityParentPM.ClaimsRelatedEntitiesReasons.Max(d => d.LineNo);
            }
            entityPM.LineNo = line + 1;
        }

        protected override void UpdateComposition(ClaimsRelatedEntitiesReasonPM entityPM)
        {
            ClaimsRelatedEntsReasonsExpUpdateService claimsRelatedEntsReasonsExpUpdateService = new ClaimsRelatedEntsReasonsExpUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            claimsRelatedEntsReasonsExpUpdateService.UpdateMulti(entityPM.ClaimsRelatedEntsReasonsExps, entityPM.DeletedClaimsRelatedEntsReasonsExps, entityPM, false);

            base.UpdateComposition(entityPM);
        }
    }

}
