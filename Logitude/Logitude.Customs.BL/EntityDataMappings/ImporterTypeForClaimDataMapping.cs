
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
   
   public partial class ImporterTypeForClaimDataMapping: IMapping<ImporterTypeForClaimPM, ImporterTypeForClaim>
   {

        public void CustomPMToPOCO(ImporterTypeForClaimPM entityPM, ImporterTypeForClaim entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ImporterTypeForClaimPM entityPM, ImporterTypeForClaim entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   