
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class DecCargoSplitConDataMapping: IMapping<DecCargoSplitConPM, DecCargoSplitCon>
   {

        public void CustomPMToPOCO(DecCargoSplitConPM entityPM, DecCargoSplitCon entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationCargoSplitId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.LineNumber);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.DeclarationCargoSplitId = entityPM.DeclarationCargoSplitId;
                entityPOCO.LineNumber = entityPM.LineNumber;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(DecCargoSplitConPM entityPM, DecCargoSplitCon entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ConditionName);
            if (entityPOCO.ConditionCode != null)
            {
                TreatmentWayQueryService treatmentWayQueryService = new TreatmentWayQueryService(entityPOCO.Tenant);
                TreatmentWayPM TreatmentWay = treatmentWayQueryService.GetSingle(entityPOCO.ConditionCode, false, true);
                entityPM.ConditionName = TreatmentWay.LocalName;
            }

            CustomMappedPMProperties.Add(PMPropertyNames.ProcedureCurrentName);
            if (entityPOCO.ProcedureCurrentCode != null)
            {
                GovernmentProcedureTypeQueryService governmentProcedureTypeQueryService = new GovernmentProcedureTypeQueryService(entityPOCO.Tenant);
                GovernmentProcedureTypePM governmentProcedureType = governmentProcedureTypeQueryService.GetSingle(entityPOCO.ProcedureCurrentCode, false, true);
                entityPM.ProcedureCurrentName = governmentProcedureType.LocalName;
            }
        }
   }


}
   