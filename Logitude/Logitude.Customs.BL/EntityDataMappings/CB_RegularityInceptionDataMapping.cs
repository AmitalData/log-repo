
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
   
   public partial class CB_RegularityInceptionDataMapping: IMapping<CB_RegularityInceptionPM, CB_RegularityInception>
   {

        public void CustomPMToPOCO(CB_RegularityInceptionPM entityPM, CB_RegularityInception entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CB_RegularityInceptionPM entityPM, CB_RegularityInception entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   