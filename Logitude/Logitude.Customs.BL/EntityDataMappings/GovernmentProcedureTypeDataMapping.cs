
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
   
   public partial class GovernmentProcedureTypeDataMapping: IMapping<GovernmentProcedureTypePM, GovernmentProcedureType>
   {

        public void CustomPMToPOCO(GovernmentProcedureTypePM entityPM, GovernmentProcedureType entityPOCO)
        {

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Code);
            
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Code = entityPM.Code;
            }
        }

        public void CustomPOCOToPM(GovernmentProcedureTypePM entityPM, GovernmentProcedureType entityPOCO)
        {
        
        }
   }


}
   