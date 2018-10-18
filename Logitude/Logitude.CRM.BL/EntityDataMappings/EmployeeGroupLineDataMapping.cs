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

namespace Logitude.CRM.BL.EntityDataMappings
{   
   public partial class EmployeeGroupLineDataMapping: IMapping<EmployeeGroupLinePM, EmployeeGroupLine>
   {
        public void CustomPMToPOCO(EmployeeGroupLinePM entityPM, EmployeeGroupLine entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.EmployeeGroupId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.UserId);

            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.EmployeeGroupId = entityPM.EmployeeGroupId;
            entityPOCO.UserId = entityPM.UserId;
        }

        public void CustomPOCOToPM(EmployeeGroupLinePM entityPM, EmployeeGroupLine entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }
}
   