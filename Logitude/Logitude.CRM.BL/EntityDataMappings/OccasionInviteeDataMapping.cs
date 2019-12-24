
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
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;

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

            BuildSearchFields(entityPM, entityPOCO);
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
                    CardContactRepository cardContactRepository = new CardContactRepository(entityPOCO.Tenant);
                    List<string> contactIds = new List<string>() { entityPOCO.ContactId };
                    var customersNames = cardContactRepository.GetCardsContactsForContactIds_Names(contactIds, entityPOCO.Tenant);
                    entityPM.CustomerName = customersNames;
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

        private void BuildSearchFields(OccasionInviteePM entityPM, OccasionInvitee entityPOCO)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPOCO.ContactId))
            {
                ContactRepository contactRepository = new ContactRepository(entityPOCO.Tenant);
                Contact contact = contactRepository.GetSingleContact(entityPOCO.ContactId, entityPOCO.Tenant);
                if (contact != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, contact.EnglishName);
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.OccasionId))
            {
                OccasionRepository occasionRepository = new OccasionRepository(entityPOCO.Tenant);
                Occasion occasion = occasionRepository.GetSingle(entityPOCO.OccasionId, entityPOCO.Tenant);
                if (occasion != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, occasion.Name);
                }
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
   