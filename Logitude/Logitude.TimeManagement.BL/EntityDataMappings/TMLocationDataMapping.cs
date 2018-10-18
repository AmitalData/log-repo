
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.EntityPMs; 
using Logitude.TimeManagement.Data;

namespace Logitude.TimeManagement.BL.EntityDataMappings
{
   
   public partial class TMLocationDataMapping: IMapping<TMLocationPM, TMLocation>
   {

        public void CustomPMToPOCO(TMLocationPM entityPM, TMLocation entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(TMLocationPM entityPM, TMLocation entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   