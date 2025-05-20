
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
   
   public partial class CB_RuleClassificationDataMapping: IMapping<CB_RuleClassificationPM, CB_RuleClassification>
   {

        public void CustomPMToPOCO(CB_RuleClassificationPM entityPM, CB_RuleClassification entityPOCO)
        {
            if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.CB_ID))
            {
                entityPOCO.CB_ID = entityPM.CB_ID;
            }
        }

        public void CustomPOCOToPM(CB_RuleClassificationPM entityPM, CB_RuleClassification entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   