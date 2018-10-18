
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.BL.EntityPMs; 
using Logitude.Social.Data;
using Simplog.Server.Infrastructure;

namespace Logitude.Social.BL.EntityDataMappings
{
   
   public partial class GroupMemberDataMapping: IMapping<GroupMemberPM, GroupMember>
   {

        public void CustomPMToPOCO(GroupMemberPM entityPM, GroupMember entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.GroupId);
            AddPOCOPropertyName(POCOPropertyNames.UserId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.GroupId = entityPM.GroupId;
                entityPOCO.UserId = entityPM.UserId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(GroupMemberPM entityPM, GroupMember entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   