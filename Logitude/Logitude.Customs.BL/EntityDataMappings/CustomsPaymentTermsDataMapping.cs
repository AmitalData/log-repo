
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
   
   public partial class CustomsPaymentTermDataMapping: IMapping<CustomsPaymentTermPM, CustomsPaymentTerm>
   {

        public void CustomPMToPOCO(CustomsPaymentTermPM entityPM, CustomsPaymentTerm entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CustomsPaymentTermPM entityPM, CustomsPaymentTerm entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   