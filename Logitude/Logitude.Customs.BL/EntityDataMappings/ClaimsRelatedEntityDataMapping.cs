
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ClaimsRelatedEntityDataMapping: IMapping<ClaimsRelatedEntityPM, ClaimsRelatedEntity>
   {

        public void CustomPMToPOCO(ClaimsRelatedEntityPM entityPM, ClaimsRelatedEntity entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ClaimId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.EntityCounterKey);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.ClaimId = entityPM.ClaimId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.EntityCounterKey = entityPM.EntityCounterKey;
            }
        }

        public void CustomPOCOToPM(ClaimsRelatedEntityPM entityPM, ClaimsRelatedEntity entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ClaimEntityTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.ContinuousMessagesTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.SeconderyClaimEntityName);
            CustomMappedPMProperties.Add(PMPropertyNames.CourtName);
            CustomMappedPMProperties.Add(PMPropertyNames.WarehouseTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.DecisionName);
            CustomMappedPMProperties.Add(PMPropertyNames.ContinuousRequestTypeName);

            if (entityPOCO.ClaimEntityTypeCode != null)
            {
                ClaimEntityQueryService claimEntityQueryService = new ClaimEntityQueryService(entityPOCO.Tenant);
                ClaimEntityPM claimEntityPM = claimEntityQueryService.GetSingle(entityPOCO.ClaimEntityTypeCode, false, true);
                entityPM.ClaimEntityTypeName = claimEntityPM.LocalName;
            }

            if (entityPOCO.ContinuousMessagesTypeCode != null)
            {
                ContinuousMessagesTypeCodeQueryService continuousMessagesTypeCodeQueryService = new ContinuousMessagesTypeCodeQueryService(entityPOCO.Tenant);
                ContinuousMessagesTypeCodePM continuousMessagesTypeCodePM = continuousMessagesTypeCodeQueryService.GetSingle(entityPOCO.ContinuousMessagesTypeCode, false, true);
                entityPM.ContinuousMessagesTypeName = continuousMessagesTypeCodePM.LocalName;
            }

            if (entityPOCO.SeconderyClaimEntityCode != null)
            {
                ClaimEntityQueryService claimEntityQueryService = new ClaimEntityQueryService(entityPOCO.Tenant);
                ClaimEntityPM claimEntityPM = claimEntityQueryService.GetSingle(entityPOCO.SeconderyClaimEntityCode, false, true);
                entityPM.WarehouseTypeName = claimEntityPM.LocalName;
            }

            if (entityPOCO.CourtCode != null)
            {
                CourtInstanceQueryService courtInstanceQueryService = new CourtInstanceQueryService(entityPOCO.Tenant);
                CourtInstancePM courtInstancePM = courtInstanceQueryService.GetSingle(entityPOCO.CourtCode, false, true);
                entityPM.CourtName = courtInstancePM.LocalName;
            }

            if (entityPOCO.WarehouseTypeCode != null)
            {
                SiteLookupQueryService siteLookupQueryService = new SiteLookupQueryService(entityPOCO.Tenant);
                SiteLookupPM siteLookupPM = siteLookupQueryService.GetSingle(entityPOCO.WarehouseTypeCode, false, true);
                entityPM.WarehouseTypeName = siteLookupPM.LocalName;
            }

            if (string.IsNullOrWhiteSpace(entityPOCO.TapagNumber))
            {
                entityPM.IsSendClaimsRelatedEntity = true;
            }
            else
            {
                entityPM.IsSendClaimsRelatedEntity = false;
            }

            if (entityPOCO.DecisionCode != null)
            {
                DecisionTypeQueryService decisionTypeQueryService = new DecisionTypeQueryService(entityPOCO.Tenant);
                DecisionTypePM decisionTypePM = decisionTypeQueryService.GetSingle(entityPOCO.DecisionCode, false, true);
                if(decisionTypePM != null)
                {
                    entityPM.DecisionName = decisionTypePM.LocalName;
                }
            }

            if (entityPOCO.ContinuousRequestTypeCode != null)
            {
                ContinuousRequestTypeQueryService continuousRequestTypeQueryService = new ContinuousRequestTypeQueryService(entityPOCO.Tenant);
                ContinuousRequestTypePM continuousRequestTypePM = continuousRequestTypeQueryService.GetSingle(entityPOCO.ContinuousRequestTypeCode, false, true);
                entityPM.ContinuousRequestTypeName = continuousRequestTypePM.LocalName;
            }
        }
   }


}
   