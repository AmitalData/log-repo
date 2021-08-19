
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
   
   public partial class QuoteOPPriceStepsDataMapping: IMapping<QuoteOPPriceStepsPM, QuoteOPPriceSteps>
   {

        public void CustomPMToPOCO(QuoteOPPriceStepsPM entityPM, QuoteOPPriceSteps entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.QuoteOPId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.QuoteOPChargeId);
            //throw new NotImplementedException();
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.QuoteOPId = entityPM.QuoteOPId;
                entityPOCO.QuoteOPChargeId = entityPM.Id;
            }

        }

        public void CustomPOCOToPM(QuoteOPPriceStepsPM entityPM, QuoteOPPriceSteps entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   