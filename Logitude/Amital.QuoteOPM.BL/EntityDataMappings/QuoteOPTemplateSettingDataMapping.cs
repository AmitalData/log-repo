
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
   
   public partial class QuoteOPTemplateSettingDataMapping: IMapping<QuoteOPTemplateSettingPM, QuoteOPTemplateSetting>
   {

        public void CustomPMToPOCO(QuoteOPTemplateSettingPM entityPM, QuoteOPTemplateSetting entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(QuoteOPTemplateSettingPM entityPM, QuoteOPTemplateSetting entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   