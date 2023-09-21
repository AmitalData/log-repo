
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
   
   public partial class DeclarationStatusDataMapping: IMapping<DeclarationStatusPM, DeclarationStatus>
   {

        public void CustomPMToPOCO(DeclarationStatusPM entityPM, DeclarationStatus entityPOCO)
        {
            entityPOCO.DeclarationId = entityPM.DeclarationId;
            entityPOCO.LineNumber = entityPM.LineNumber;

        }

        public void CustomPOCOToPM(DeclarationStatusPM entityPM, DeclarationStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   