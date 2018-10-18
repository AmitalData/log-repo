
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs; 
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class LBPTeamMemberDataMapping: IMapping<LBPTeamMemberPM, LBPTeamMember>
   {

        public void CustomPMToPOCO(LBPTeamMemberPM entityPM, LBPTeamMember entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.MemberTeamId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.AddedByUserId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.TeamId);

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.MemberTeamId = entityPM.MemberTeamId;
            entityPOCO.AddedByUserId = entityPM.AddedByUserId;
            entityPOCO.TeamId = entityPM.TeamId;
        }

        public void CustomPOCOToPM(LBPTeamMemberPM entityPM, LBPTeamMember entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   