
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
   
   public partial class ActivityPriorityDataMapping: IMapping<ActivityPriorityPM, ActivityPriority>
   {

        public void CustomPMToPOCO(ActivityPriorityPM entityPM, ActivityPriority entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ActivityPriorityPM entityPM, ActivityPriority entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   