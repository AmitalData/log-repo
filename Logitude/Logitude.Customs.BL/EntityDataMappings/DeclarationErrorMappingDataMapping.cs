
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
   
   public partial class DeclarationErrorMappingDataMapping: IMapping<DeclarationErrorMappingPM, DeclarationErrorMapping>
   {

        public void CustomPMToPOCO(DeclarationErrorMappingPM entityPM, DeclarationErrorMapping entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
            }
        }

        public void CustomPOCOToPM(DeclarationErrorMappingPM entityPM, DeclarationErrorMapping entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   