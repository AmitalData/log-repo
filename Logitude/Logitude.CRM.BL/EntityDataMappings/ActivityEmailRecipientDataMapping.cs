
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
   
   public partial class ActivityEmailRecipientDataMapping: IMapping<ActivityEmailRecipientPM, ActivityEmailRecipient>
   {

        public void CustomPMToPOCO(ActivityEmailRecipientPM entityPM, ActivityEmailRecipient entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ActivityId);

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.ActivityId = entityPM.ActivityId;
        }

        public void CustomPOCOToPM(ActivityEmailRecipientPM entityPM, ActivityEmailRecipient entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   