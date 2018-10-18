
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
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Social.BL.EntityDataMappings
{
   
   public partial class PostLikeDataMapping: IMapping<PostLikePM, PostLike>
   {

        public void CustomPMToPOCO(PostLikePM entityPM, PostLike entityPOCO)
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

        public void CustomPOCOToPM(PostLikePM entityPM, PostLike entityPOCO)
        {
            UserRepository userRepository = new UserRepository(entityPOCO.Tenant);
            User user = userRepository.GetSingleUser(entityPOCO.UserId, entityPOCO.Tenant, false);
            if (user != null)
            {
                
                entityPM.UserName = user.Contact.EnglishName;
        
            }

            
          
        }
   }


}
   