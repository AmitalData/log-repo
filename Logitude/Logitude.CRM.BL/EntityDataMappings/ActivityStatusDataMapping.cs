
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
   
   public partial class ActivityStatusDataMapping: IMapping<ActivityStatusPM, ActivityStatus>
   {

        public void CustomPMToPOCO(ActivityStatusPM entityPM, ActivityStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ActivityStatusPM entityPM, ActivityStatus entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   