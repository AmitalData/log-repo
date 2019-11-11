
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ConsignmentPackDangerDataMapping: IMapping<ConsignmentPackDangerPM, ConsignmentPackDanger>
   {

        public void CustomPMToPOCO(ConsignmentPackDangerPM entityPM, ConsignmentPackDanger entityPOCO)
        {
            if (entityPM != null)
            {
                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.DangerousLineNo = entityPM.DangerousLineNo;
                entityPOCO.ConsignmentNumber = entityPM.ConsignmentNumber;
                entityPOCO.LineNumber = entityPM.LineNumber;

            }        }

        public void CustomPOCOToPM(ConsignmentPackDangerPM entityPM, ConsignmentPackDanger entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   