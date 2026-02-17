
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
using Simplog.Server.Infrastructure;
using Logitude.CRM.Data.Repsitories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class TicketClassificationDataMapping: IMapping<TicketClassificationPM, TicketClassification>
   {

        public void CustomPMToPOCO(TicketClassificationPM entityPM, TicketClassification entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.ParentId = entityPM.ParentId;
            entityPOCO.Inactive = entityPM.Inactive;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(TicketClassificationPM entityPM, TicketClassification entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ParentName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ManagerUserEmail);
            if (!string.IsNullOrEmpty(entityPOCO.ManagerUserId))
            {
                ContactRepository contactRepository = new ContactRepository(entityPOCO.Tenant);
                Contact contact = contactRepository.GetSingleContact(entityPOCO.ManagerUserId, entityPOCO.Tenant);
                if (contact != null)
                {
                    entityPM.ManagerUserEmail = contact.Email;
                }
            }

            TicketClassificationRepository ticketRepository = new TicketClassificationRepository(entityPOCO.Tenant);
            TicketClassification ticket = ticketRepository.GetSingle(entityPOCO.ParentId, entityPOCO.Tenant);
            if (ticket != null)
            {
                entityPM.ParentName = ticket.Name;
            }
        }

        private void BuildSearchFields(TicketClassificationPM entityPM, TicketClassification entityPOCO, bool p)
        {
            string result = entityPM.Name;

            entityPM.SearchFields = result;
            entityPOCO.SearchFields = result;
        }
   }
}
   