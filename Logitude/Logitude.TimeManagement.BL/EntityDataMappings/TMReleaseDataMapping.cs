
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
   
   public partial class TMReleaseDataMapping: IMapping<TMReleasePM, TMRelease>
   {

        public void CustomPMToPOCO(TMReleasePM entityPM, TMRelease entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(TMReleasePM entityPM, TMRelease entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   