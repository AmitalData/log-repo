
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
   
   public partial class DeficitDecisionDataMapping: IMapping<DeficitDecisionPM, DeficitDecision>
   {

        public void CustomPMToPOCO(DeficitDecisionPM entityPM, DeficitDecision entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.DeclarationId);
            AddPOCOPropertyName(POCOPropertyNames.DeficitId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.DeficitId = entityPM.DeficitId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(DeficitDecisionPM entityPM, DeficitDecision entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.RequestTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ApprovedProfessionName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.DecisionName);

            if (entityPOCO.RequestTypeCode != null)
            {
                RequestTypeQueryService requestTypeQueryService = new RequestTypeQueryService(entityPOCO.Tenant);
                RequestTypePM requestTypePM = requestTypeQueryService.GetSingle(entityPOCO.RequestTypeCode, false, true);
                if (requestTypePM != null)
                {
                    entityPM.RequestTypeName = requestTypePM.LocalName;
                }
            }

            if (entityPOCO.ApprovedProfessionCode != null)
            {
                ApprovedProfessionQueryService approvedProfessionQueryService = new ApprovedProfessionQueryService(entityPOCO.Tenant);
                ApprovedProfessionPM approvedProfessionPM = approvedProfessionQueryService.GetSingle(entityPOCO.ApprovedProfessionCode, false, true);
                if (approvedProfessionPM != null)
                {
                    entityPM.ApprovedProfessionName = approvedProfessionPM.LocalName;
                }
            }

            if (entityPOCO.DecisionCode != null)
            {
                DecisionTypeQueryService decisionTypeQueryService = new DecisionTypeQueryService(entityPOCO.Tenant);
                DecisionTypePM decisionTypePM = decisionTypeQueryService.GetSingle(entityPOCO.DecisionCode, false, true);
                if (decisionTypePM != null)
                {
                    entityPM.DecisionName = decisionTypePM.LocalName;
                }
            }
        }
   }


}
   