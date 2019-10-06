
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
   
   public partial class OccasionInviteeDataMapping: IMapping<OccasionInviteePM, OccasionInvitee>
   {

        public void CustomPMToPOCO(OccasionInviteePM entityPM, OccasionInvitee entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(OccasionInviteePM entityPM, OccasionInvitee entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   