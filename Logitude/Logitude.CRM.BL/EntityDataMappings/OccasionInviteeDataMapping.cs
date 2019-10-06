
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
using Logitude.CRM.Data.Repsitories;

namespace Logitude.CRM.BL.EntityDataMappings
{   
   public partial class OccasionInviteeDataMapping: IMapping<OccasionInviteePM, OccasionInvitee>
   {
        public void CustomPMToPOCO(OccasionInviteePM entityPM, OccasionInvitee entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.OccasionId);

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.OccasionId = entityPM.OccasionId;
        }

        public void CustomPOCOToPM(OccasionInviteePM entityPM, OccasionInvitee entityPOCO)
        {
            if (!string.IsNullOrEmpty(entityPOCO.ContactId))
            {
                ContactRepository contactRepository = new ContactRepository(entityPOCO.Tenant);
                Contact contact = contactRepository.GetSingleContact(entityPOCO.ContactId, entityPOCO.Tenant);
                if (contact != null)
                {
                    entityPM.ContactName = contact.EnglishName;
                    entityPM.ContactEmail = contact.Email;
                    entityPM.ContactMobile = contact.Mobile;
                    entityPM.ContactTel = contact.BusinessPhone;
                    entityPM.ContactPosition = contact.Position;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.OccasionId))
            {
                OccasionRepository occasionRepository = new OccasionRepository(entityPOCO.Tenant);
                Occasion occasion = occasionRepository.GetSingle(entityPOCO.OccasionId, entityPOCO.Tenant);
                if (occasion != null)
                {
                    entityPM.OccasionName = occasion.Name;
                }
            }
        }
    }
}
   