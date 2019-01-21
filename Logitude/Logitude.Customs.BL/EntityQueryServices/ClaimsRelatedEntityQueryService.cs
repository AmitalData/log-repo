using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class ClaimsRelatedEntityQueryService : EntityQueryService<ClaimsRelatedEntity, ClaimsRelatedEntityKeys, ClaimsRelatedEntityPM, ClaimPM, ClaimKeys>
    {
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, ClaimsRelatedEntityPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            ClaimsRelatedEntityKeys claimKeys = entityKeys as ClaimsRelatedEntityKeys;
            ClaimsRelatedEntitiesAmountQueryService claimsRelatedEntitiesAmountQueryService = new ClaimsRelatedEntitiesAmountQueryService(context);
            ClaimsRelatedEntitiesReasonQueryService claimsRelatedEntitiesReasonQueryService = new ClaimsRelatedEntitiesReasonQueryService(context);
            ClaimsRelatedEntsExpDeclarQueryService claimsRelatedEntsExpDeclarQueryService = new ClaimsRelatedEntsExpDeclarQueryService(context);
            ClaimsRelatedEntitiesSeizureQueryService claimsRelatedEntitiesSeizureQueryService = new ClaimsRelatedEntitiesSeizureQueryService(context);
            ClaimsRelatedEntitiesRefundQueryService claimsRelatedEntitiesRefundQueryService = new ClaimsRelatedEntitiesRefundQueryService(context);

            entityPM.ClaimsRelatedEntitiesAmounts = claimsRelatedEntitiesAmountQueryService.GetMulti(claimKeys, true);
            entityPM.ClaimsRelatedEntitiesReasons = claimsRelatedEntitiesReasonQueryService.GetMulti(claimKeys, true);
            entityPM.ClaimsRelatedEntsExpDeclars = claimsRelatedEntsExpDeclarQueryService.GetMulti(claimKeys, true);
            entityPM.ClaimsRelatedEntitiesSeizures = claimsRelatedEntitiesSeizureQueryService.GetMulti(claimKeys, true);
            entityPM.ClaimsRelatedEntitiesRefunds = claimsRelatedEntitiesRefundQueryService.GetMulti(claimKeys, true);

            base.GetComposition(entityKeys, entityPM);
        }

        public string GetClaimIdByTapagNumber(string fileNumber, int numeral, int tenant)
        {
            if (String.IsNullOrWhiteSpace(fileNumber) || numeral == 0) return "";
            return repository.GetClaimIdByTapagNumber(fileNumber, numeral, tenant);
        }

        public ClaimsRelatedEntityPM GetRelatedEntityByTapagNumber(string fileNumber, int numeral, int tenant)
        {
            if (String.IsNullOrWhiteSpace(fileNumber) || numeral == 0) return null;
            ClaimsRelatedEntity claimsRelatedEntity = repository.GetRelatedEntityByTapagNumber(fileNumber, numeral, tenant);
            ClaimsRelatedEntityPM ClaimsRelatedEntityPM = GetEntityPM(claimsRelatedEntity);
            return ClaimsRelatedEntityPM;
        }

        public List<ClaimsRelatedEntityPM> GetCREsByCounterKeys(string claimId, List<int> keys, int tenant)
        {
            List<ClaimsRelatedEntity> claimsRelatedEntities = repository.GetCREsByCounterKeys(claimId, keys, tenant);
            return (from a in claimsRelatedEntities
                    select new ClaimsRelatedEntityPM()
                    {
                        ClaimId = a.ClaimId,
                        EntityCounterKey = a.EntityCounterKey,
                    }).OrderBy(d => d.EntityCounterKey).ToList();
        }

    }
}
