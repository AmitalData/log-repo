
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
   
   public partial class QuoteOPComputedFieldDataMapping: IMapping<QuoteOPComputedFieldPM, QuoteOPComputedField>
   {

        public void CustomPMToPOCO(QuoteOPComputedFieldPM entityPM, QuoteOPComputedField entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(QuoteOPComputedFieldPM entityPM, QuoteOPComputedField entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   