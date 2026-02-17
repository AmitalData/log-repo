using Logitude.Customs.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ClaimsRelatedEntityUpdateService
    {

        protected override void OnCreating(ClaimsRelatedEntityPM entityPM, ClaimPM entityParentPM)
        {
            if (entityParentPM == null)
            {
                return;
            }
            entityPM.ClaimId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;

            int line = 0;
            if (entityParentPM.ClaimsRelatedEntities.Count > 0)
            {
                line = entityParentPM.ClaimsRelatedEntities.Max(d => d.EntityCounterKey);
            }

            entityPM.EntityCounterKey = line + 1;

            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void UpdateComposition(ClaimsRelatedEntityPM entityPM)
        {
            ClaimsRelatedEntitiesAmountUpdateService claimsRelatedEntitiesAmountUpdateService = new ClaimsRelatedEntitiesAmountUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            claimsRelatedEntitiesAmountUpdateService.UpdateMulti(entityPM.ClaimsRelatedEntitiesAmounts, entityPM.DeletedClaimsRelatedEntitiesAmounts, entityPM, false);

            ClaimsRelatedEntitiesReasonUpdateService claimsRelatedEntitiesReasonUpdateService = new ClaimsRelatedEntitiesReasonUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            claimsRelatedEntitiesReasonUpdateService.UpdateMulti(entityPM.ClaimsRelatedEntitiesReasons, entityPM.DeletedClaimsRelatedEntitiesReasons, entityPM, false);

            ClaimsRelatedEntsExpDeclarUpdateService claimsRelatedEntsExpDeclarUpdateService = new ClaimsRelatedEntsExpDeclarUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            claimsRelatedEntsExpDeclarUpdateService.UpdateMulti(entityPM.ClaimsRelatedEntsExpDeclars, entityPM.DeletedClaimsRelatedEntsExpDeclars, entityPM, false);

            ClaimsRelatedEntitiesSeizureUpdateService claimsRelatedEntitiesSeizureUpdateService = new ClaimsRelatedEntitiesSeizureUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            claimsRelatedEntitiesSeizureUpdateService.UpdateMulti(entityPM.ClaimsRelatedEntitiesSeizures, entityPM.DeletedClaimsRelatedEntitiesSeizures, entityPM, false);

            ClaimsRelatedEntitiesRefundUpdateService claimsRelatedEntitiesRefundUpdateService = new ClaimsRelatedEntitiesRefundUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            claimsRelatedEntitiesRefundUpdateService.UpdateMulti(entityPM.ClaimsRelatedEntitiesRefunds, entityPM.DeletedClaimsRelatedEntitiesRefunds, entityPM, false);

            base.UpdateComposition(entityPM);
        }
    }
}
