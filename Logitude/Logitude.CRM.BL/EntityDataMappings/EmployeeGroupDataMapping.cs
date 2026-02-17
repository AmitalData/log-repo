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
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.CRM.BL.EntityDataMappings
{   
   public partial class EmployeeGroupDataMapping: IMapping<EmployeeGroupPM, EmployeeGroup>
   {
        public void CustomPMToPOCO(EmployeeGroupPM entityPM, EmployeeGroup entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }

        public void CustomPOCOToPM(EmployeeGroupPM entityPM, EmployeeGroup entityPOCO)
        {
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

        }

        private void BuildSearchFields(EmployeeGroupPM entityPM, EmployeeGroup entityPOCO, bool p)
        {
            string result = entityPM.Name;

            entityPM.SearchFields = result;
            entityPOCO.SearchFields = result;
        }
   }
}
   