
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
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class CorrespondenceDataMapping: IMapping<CorrespondencePM, Correspondence>
   {
        public void CustomPMToPOCO(CorrespondencePM entityPM, Correspondence entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
        }

        public void CustomPOCOToPM(CorrespondencePM entityPM, Correspondence entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ContactName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ContactEmail);

            if (!string.IsNullOrEmpty(entityPOCO.CreatedByContactId))
            {
                ContactRepository repContact = new ContactRepository(entityPOCO.Tenant);
                Contact contact = repContact.GetSingleContact(entityPOCO.CreatedByContactId, entityPOCO.Tenant);
                if (contact != null)
                {
                    entityPM.ContactName = contact.EnglishName;
                    entityPM.ContactEmail = contact.Email;                
                }
            }

        }
   }
}
   