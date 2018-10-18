
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
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class DeclarationConstraintDataMapping: IMapping<DeclarationConstraintPM, DeclarationConstraint>
   {

        public void CustomPMToPOCO(DeclarationConstraintPM entityPM, DeclarationConstraint entityPOCO)
        {
            //throw new NotImplementedException();
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationID);
            entityPOCO.DeclarationID = entityPM.DeclarationID;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConstraintNumber);
            entityPOCO.ConstraintNumber = entityPM.ConstraintNumber;
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            entityPOCO.Tenant = entityPM.Tenant;
        }

        public void CustomPOCOToPM(DeclarationConstraintPM entityPM, DeclarationConstraint entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ConstraintTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ConstraintStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ApprovalDecisionName);
            


            if (entityPOCO.ConstraintTypeCode != null)
            {
                ConstraintProcessTypeQueryService constraintProcessTypeQueryService = new ConstraintProcessTypeQueryService(entityPOCO.Tenant);
                ConstraintProcessTypePM constraintType = constraintProcessTypeQueryService.GetSingle(entityPOCO.ConstraintTypeCode, false, true);
                entityPM.ConstraintTypeName = constraintType.LocalName;
            }

            if (entityPOCO.ConstraintStatusCode != null)
            {
                ConstraintStatusQueryService constraintStatusQueryService = new ConstraintStatusQueryService(entityPOCO.Tenant);
                ConstraintStatusPM constraintStatus = constraintStatusQueryService.GetSingle(entityPOCO.ConstraintStatusCode, false, true); //Mirit 07/05/15 Task 13151
                entityPM.ConstraintStatusName = constraintStatus.LocalName;
            }

            if (entityPOCO.ApprovalDecision != null)
            {
                ConstraintApprovalDecisionQueryService constraintApprovalDecisionQueryService = new ConstraintApprovalDecisionQueryService(entityPOCO.Tenant);
                ConstraintApprovalDecisionPM approvalDecision = constraintApprovalDecisionQueryService.GetSingle(entityPOCO.ApprovalDecision, false, true);
                entityPM.ApprovalDecisionName = approvalDecision.LocalName;
            }

       
        }
   }


}
   