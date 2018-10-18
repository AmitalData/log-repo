
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
   
   public partial class ClaimsRelatedEntitiesAmountDataMapping: IMapping<ClaimsRelatedEntitiesAmountPM, ClaimsRelatedEntitiesAmount>
   {

        public void CustomPMToPOCO(ClaimsRelatedEntitiesAmountPM entityPM, ClaimsRelatedEntitiesAmount entityPOCO)
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

        public void CustomPOCOToPM(ClaimsRelatedEntitiesAmountPM entityPM, ClaimsRelatedEntitiesAmount entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.PaymentTypeName);

            if (entityPOCO.PaymentTypeCode != null)
            {
                ParagraphTypeQueryService paragraphTypeQueryService = new ParagraphTypeQueryService(entityPOCO.Tenant);
                ParagraphTypePM paragraphTypePM = paragraphTypeQueryService.GetSingle(entityPOCO.PaymentTypeCode, false, true);
                entityPM.PaymentTypeName = paragraphTypePM.LocalName;
            }
        }
   }


}
   