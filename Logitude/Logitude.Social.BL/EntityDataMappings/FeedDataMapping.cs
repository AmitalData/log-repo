
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
   
   public partial class FeedDataMapping: IMapping<FeedPM, Feed>
   {

        public void CustomPMToPOCO(FeedPM entityPM, Feed entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.PostId);
            AddPOCOPropertyName(POCOPropertyNames.UserId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.PostId = entityPM.PostId;
                entityPOCO.UserId = entityPM.UserId;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(FeedPM entityPM, Feed entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   