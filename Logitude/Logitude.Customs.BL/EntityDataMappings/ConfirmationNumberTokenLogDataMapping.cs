
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
   
   public partial class ConfirmationNumberTokenLogDataMapping: IMapping<ConfirmationNumberTokenLogPM, ConfirmationNumberTokenLog>
   {

        public void CustomPMToPOCO(ConfirmationNumberTokenLogPM entityPM, ConfirmationNumberTokenLog entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ConfirmationNumberTokenLogPM entityPM, ConfirmationNumberTokenLog entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void PMToOldPM(ConfirmationNumberTokenLogPM entityPM, ConfirmationNumberTokenLogPM oldEntityPM)
        {
        }

        public void PMToPOCO(ConfirmationNumberTokenLogPM entityPM, ConfirmationNumberTokenLog entityPOCO)
        {
        }

        public void POCOToPM(ConfirmationNumberTokenLogPM entityPM, ConfirmationNumberTokenLog entityPOCO)
        {
        }
    }


}
   