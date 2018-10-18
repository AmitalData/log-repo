
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
   
   public partial class RevaluationStatusDataMapping: IMapping<RevaluationStatusPM, RevaluationStatus>
   {

        public void CustomPMToPOCO(RevaluationStatusPM entityPM, RevaluationStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(RevaluationStatusPM entityPM, RevaluationStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   