
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
   
   public partial class ClaimsRelatedEntitiesReasonDataMapping: IMapping<ClaimsRelatedEntitiesReasonPM, ClaimsRelatedEntitiesReason>
   {

        public void CustomPMToPOCO(ClaimsRelatedEntitiesReasonPM entityPM, ClaimsRelatedEntitiesReason entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ClaimId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CounterKey);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNo);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.ClaimId = entityPM.ClaimId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CounterKey = entityPM.CounterKey;
                entityPOCO.LineNo = entityPM.LineNo;
            }
        }

        public void CustomPOCOToPM(ClaimsRelatedEntitiesReasonPM entityPM, ClaimsRelatedEntitiesReason entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ReasonListTypeName);

            if (entityPOCO.ReasonListTypeCode != null)
            {
                ProcessingReasonQueryService processingReasonQueryService = new ProcessingReasonQueryService(entityPOCO.Tenant);
                ProcessingReasonPM processingReasonPM = processingReasonQueryService.GetSingle(entityPOCO.ReasonListTypeCode, false, true);
                entityPM.ReasonListTypeName = processingReasonPM.LocalName;
            }
        }
   }


}
   