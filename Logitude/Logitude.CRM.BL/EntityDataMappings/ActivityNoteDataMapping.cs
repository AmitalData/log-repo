
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
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class ActivityNoteDataMapping: IMapping<ActivityNotePM, ActivityNote>
   {
        public void CustomPMToPOCO(ActivityNotePM entityPM, ActivityNote entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ActivityId);

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.ActivityId = entityPM.ActivityId;
        }

        public void CustomPOCOToPM(ActivityNotePM entityPM, ActivityNote entityPOCO)
        {
            UserRepository userRepository = new UserRepository(entityPOCO.Tenant);
            User createByUser = userRepository.GetSingleUser(entityPOCO.CreatedByUserId, entityPOCO.Tenant, false);
            User updateByUser = userRepository.GetSingleUser(entityPOCO.UpdatedByUserId, entityPOCO.Tenant, false);

            if (createByUser != null)
            {
                entityPM.CreatedByUserName = createByUser.Contact.EnglishName;
            }

            if (updateByUser != null)
            {
                entityPM.UpdatedByUserName = updateByUser.Contact.EnglishName;
            }
        }
   }


}
   