
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class IntegrityCheckStatusDataMapping: IMapping<IntegrityCheckStatusPM, IntegrityCheckStatus>
   {

        public void CustomPMToPOCO(IntegrityCheckStatusPM entityPM, IntegrityCheckStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(IntegrityCheckStatusPM entityPM, IntegrityCheckStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   