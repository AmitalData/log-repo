
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
   
   public partial class ClaimsRelatedEntsReasonsExpDataMapping: IMapping<ClaimsRelatedEntsReasonsExpPM, ClaimsRelatedEntsReasonsExp>
   {

        public void CustomPMToPOCO(ClaimsRelatedEntsReasonsExpPM entityPM, ClaimsRelatedEntsReasonsExp entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ClaimId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CounterKey);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ReasonLineNo);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNo);
            
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.ClaimId = entityPM.ClaimId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CounterKey = entityPM.CounterKey;
                entityPOCO.ReasonLineNo = entityPM.ReasonLineNo;
                entityPOCO.LineNo = entityPM.LineNo;              
            }
        }

        public void CustomPOCOToPM(ClaimsRelatedEntsReasonsExpPM entityPM, ClaimsRelatedEntsReasonsExp entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ClaimExplanationTypeName);

            if (entityPOCO.ClaimExplanationTypeCode != null)
            {
                ClaimExplanationCodeQueryService claimExplanationCodeQueryService = new ClaimExplanationCodeQueryService(entityPOCO.Tenant);
                ClaimExplanationCodePM processingReasonPM = claimExplanationCodeQueryService.GetSingle(entityPOCO.ClaimExplanationTypeCode, false, true);
                entityPM.ClaimExplanationTypeName = processingReasonPM.LocalName;
            }
        }
   }


}
   