
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
   
   public partial class PostDataMapping: IMapping<PostPM, Post>
   {

        public void CustomPMToPOCO(PostPM entityPM, Post entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(PostPM entityPM, Post entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CreatedByUserName);

            UserRepository userRepository = new UserRepository(entityPOCO.Tenant);
            User user = userRepository.GetSingleUser(entityPOCO.CreatedById, entityPOCO.Tenant, false);
            if (user != null)
            {
                entityPM.CreatedByUserName = user.Contact.EnglishName;
                entityPM.UserImageDetailId = user.Contact.ImageDetailId;
                entityPM.IndexColor = user.Contact.IndexColor;
            }

        }
   }


}
   