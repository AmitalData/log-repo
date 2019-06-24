
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs; 
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class OccasionStatusDataMapping: IMapping<OccasionStatusPM, OccasionStatus>
   {

        public void CustomPMToPOCO(OccasionStatusPM entityPM, OccasionStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(OccasionStatusPM entityPM, OccasionStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   