
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
   
   public partial class QuoteOPStageDataMapping: IMapping<QuoteOPStagePM, QuoteOPStage>
   {

        public void CustomPMToPOCO(QuoteOPStagePM entityPM, QuoteOPStage entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(QuoteOPStagePM entityPM, QuoteOPStage entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   