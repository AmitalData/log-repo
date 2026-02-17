
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
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Infrastructure.BL.EntityDataMappings
{
   
   public partial class PriceStepsDataMapping: IMapping<PriceStepsPM, PriceSteps>
   {

        public void CustomPMToPOCO(PriceStepsPM entityPM, PriceSteps entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO);
        }

        public void CustomPOCOToPM(PriceStepsPM entityPM, PriceSteps entityPOCO)
        {
            Contact createdByContact = ContactRepository.GetSingleContact(entityPOCO.CreatedByUserId, entityPOCO.Tenant, true);
            if (createdByContact != null)
            {
                entityPM.CreatedByUserName = createdByContact.EnglishName;
            }
            Contact updatedByContact = ContactRepository.GetSingleContact(entityPOCO.UpdatedByUserId, entityPOCO.Tenant, true);
            if (createdByContact != null)
            {
                entityPM.UpdatedByUserName = updatedByContact.EnglishName;
            }
        }
        private void BuildSearchFields(PriceStepsPM entityPM, PriceSteps entityPOCO)
        {
            string searchFields = "";
            MethodHelper.AddToSearchFields(ref searchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref searchFields, entityPM.Steps);
            UserRepository userRepository = new UserRepository(entityPOCO.Tenant);
            User user = userRepository.GetSingleUser(entityPM.CreatedByUserId, entityPM.Tenant);
            if (user != null)
            {
                MethodHelper.AddToSearchFields(ref searchFields, user.Contact.EnglishName);
            }
            entityPM.SearchFields = searchFields;
            entityPOCO.SearchFields = searchFields;
        }
    }
}
   