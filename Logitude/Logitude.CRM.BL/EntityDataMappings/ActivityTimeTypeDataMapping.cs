
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
   
   public partial class ActivityTimeTypeDataMapping: IMapping<ActivityTimeTypePM, ActivityTimeType>
   {

        public void CustomPMToPOCO(ActivityTimeTypePM entityPM, ActivityTimeType entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ActivityTimeTypePM entityPM, ActivityTimeType entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   