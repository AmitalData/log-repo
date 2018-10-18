
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
   
   public partial class FollowerDataMapping: IMapping<FollowerPM, Follower>
   {

        public void CustomPMToPOCO(FollowerPM entityPM, Follower entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.FolloweeUserId);
            AddPOCOPropertyName(POCOPropertyNames.FollowerUserId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.FolloweeUserId = entityPM.FolloweeUserId;
                entityPOCO.FollowerUserId = entityPM.FollowerUserId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(FollowerPM entityPM, Follower entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   