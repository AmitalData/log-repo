using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationConsAcceptanceUpdateService
    {

        protected override void OnCreating(DeclarationConsAcceptancePM entityPM, EntityPM entityParentPM)
        {
            ICustomContext context = MainContext as CustomContext;

            DeclarationConsAcceptanceQueryService declarationConsAcceptanceQueryService = new DeclarationConsAcceptanceQueryService(context);
            int? maxCounterKey = declarationConsAcceptanceQueryService.GetMaxCounterKey(entityPM.DeclarationId, entityPM.Tenant);
            if (maxCounterKey != null)
            {
                entityPM.LineNumber = maxCounterKey.Value + 1;
            }

            base.OnCreating(entityPM, entityParentPM);
        }
        // removed the composition link since the usage in client without composition . declaration id is filled in client --mohammad merge to main.
        //protected override void OnCreating(DeclarationConsAcceptancePM entityPM, DeclarationPM entityParentPM)
        //{
        //    if(string.IsNullOrEmpty(entityPM.DeclarationId))
        //    {
        //        entityPM.DeclarationId = entityParentPM.Id;
        //        entityPM.Tenant = entityParentPM.Tenant;
        //    }

        //    ICustomContext context = MainContext as CustomContext;

        //    DeclarationConsAcceptanceQueryService declarationConsAcceptanceQueryService = new DeclarationConsAcceptanceQueryService(context);
        //    int? maxCounterKey = declarationConsAcceptanceQueryService.GetMaxCounterKey(entityPM.DeclarationId, entityPM.Tenant);
        //    if (maxCounterKey != null)
        //    {
        //        entityPM.LineNumber = maxCounterKey.Value + 1;
        //    }

        //    //base.OnCreating(entityPM, entityParentPM);
        //}

        protected override void OnUpdating(DeclarationConsAcceptancePM entityPM)
        {
            base.OnUpdating(entityPM);
        }
    }
}
