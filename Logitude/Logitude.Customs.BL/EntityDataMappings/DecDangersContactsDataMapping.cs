
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
   
   public partial class DecDangersContactDataMapping: IMapping<DecDangersContactPM, DecDangersContact>
   {

        public void CustomPMToPOCO(DecDangersContactPM entityPM, DecDangersContact entityPOCO)
        {
            entityPOCO.DeclarationId = entityPM.DeclarationId;
         }

        public void CustomPOCOToPM(DecDangersContactPM entityPM, DecDangersContact entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   