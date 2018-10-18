
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
   
   public partial class FollowEntityDataMapping: IMapping<FollowEntityPM, FollowEntity>
   {

        public void CustomPMToPOCO(FollowEntityPM entityPM, FollowEntity entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
          
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
             
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(FollowEntityPM entityPM, FollowEntity entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   