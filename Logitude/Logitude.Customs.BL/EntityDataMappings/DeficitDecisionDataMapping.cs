
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class DeficitDecisionDataMapping: IMapping<DeficitDecisionPM, DeficitDecision>
   {

        public void CustomPMToPOCO(DeficitDecisionPM entityPM, DeficitDecision entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.DeclarationId);
            AddPOCOPropertyName(POCOPropertyNames.TapagId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.TapagId = entityPM.TapagId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(DeficitDecisionPM entityPM, DeficitDecision entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   