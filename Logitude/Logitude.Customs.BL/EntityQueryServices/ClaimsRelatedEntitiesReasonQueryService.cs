using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class ClaimsRelatedEntitiesReasonQueryService : EntityQueryService<ClaimsRelatedEntitiesReason, ClaimsRelatedEntitiesReasonKeys, ClaimsRelatedEntitiesReasonPM, ClaimsRelatedEntityPM, ClaimsRelatedEntityKeys>
    {
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, ClaimsRelatedEntitiesReasonPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            ClaimsRelatedEntitiesReasonKeys claimsRelatedEntitiesReasonKeys = entityKeys as ClaimsRelatedEntitiesReasonKeys;
            ClaimsRelatedEntsReasonsExpQueryService claimsRelatedEntsReasonsExpQueryService = new ClaimsRelatedEntsReasonsExpQueryService(context);

            entityPM.ClaimsRelatedEntsReasonsExps = claimsRelatedEntsReasonsExpQueryService.GetMulti(claimsRelatedEntitiesReasonKeys, true);

            base.GetComposition(entityKeys, entityPM);
        }

    }
}
