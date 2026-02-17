
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
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class SLAEscalationRecepientDataMapping: IMapping<SLAEscalationRecepientPM, SLAEscalationRecepient>
   {

        public void CustomPMToPOCO(SLAEscalationRecepientPM entityPM, SLAEscalationRecepient entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.SLAEscalationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.PreDefinitionId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.UserId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.SLAEscalationId = entityPM.SLAEscalationId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Id = entityPM.Id;
                entityPOCO.UserId = entityPM.UserId;
                entityPOCO.PreDefinitionId = entityPM.PreDefinitionId;
            }
        }

        public void CustomPOCOToPM(SLAEscalationRecepientPM entityPM, SLAEscalationRecepient entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.PreDefinitionName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.UserName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.UserEmail);

            if (!string.IsNullOrEmpty(entityPOCO.PreDefinitionId))
            {
                EscalationPreDefinitionRepository predefinitionRep = new EscalationPreDefinitionRepository(entityPOCO.Tenant);
                EscalationPreDefinition predefinition = predefinitionRep.GetSingle(entityPOCO.PreDefinitionId);
                if (predefinition != null)
                {
                    entityPM.PreDefinitionName = predefinition.Name;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.UserId))
            {
                ContactRepository contactRepository = new ContactRepository(entityPOCO.Tenant);
                Contact contact = contactRepository.GetSingleContact(entityPOCO.UserId, entityPOCO.Tenant);
                if (contact != null)
                {
                    entityPM.UserName = contact.EnglishName;
                    entityPM.UserEmail = contact.Email;
                }
            }
        }
   }
}
   