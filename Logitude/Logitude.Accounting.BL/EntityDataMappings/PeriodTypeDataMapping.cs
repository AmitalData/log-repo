
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
   
   public partial class PeriodTypeDataMapping: IMapping<PeriodTypePM, PeriodType>
   {

        public void CustomPMToPOCO(PeriodTypePM entityPM, PeriodType entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(PeriodTypePM entityPM, PeriodType entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   