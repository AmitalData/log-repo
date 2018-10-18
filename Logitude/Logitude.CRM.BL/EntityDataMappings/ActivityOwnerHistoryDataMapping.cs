
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
   
   public partial class ActivityOwnerHistoryDataMapping: IMapping<ActivityOwnerHistoryPM, ActivityOwnerHistory>
   {

        public void CustomPMToPOCO(ActivityOwnerHistoryPM entityPM, ActivityOwnerHistory entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ActivityOwnerHistoryPM entityPM, ActivityOwnerHistory entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   