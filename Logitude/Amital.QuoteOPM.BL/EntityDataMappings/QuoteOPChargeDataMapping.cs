
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs; 
using Amital.QuoteOPM.Data;

namespace Amital.QuoteOPM.BL.EntityDataMappings
{
   
   public partial class QuoteOPChargeDataMapping: IMapping<QuoteOPChargePM, QuoteOPCharge>
   {

        public void CustomPMToPOCO(QuoteOPChargePM entityPM, QuoteOPCharge entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(QuoteOPChargePM entityPM, QuoteOPCharge entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   