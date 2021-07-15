
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
   
   public partial class QuoteOPCostChargeDataMapping: IMapping<QuoteOPCostChargePM, QuoteOPCostCharge>
   {

        public void CustomPMToPOCO(QuoteOPCostChargePM entityPM, QuoteOPCostCharge entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(QuoteOPCostChargePM entityPM, QuoteOPCostCharge entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   