
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

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class SLALineDataMapping: IMapping<SLALinePM, SLALine>
   {

        public void CustomPMToPOCO(SLALinePM entityPM, SLALine entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
        }

        public void CustomPOCOToPM(SLALinePM entityPM, SLALine entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.SeverityName);

            if (!string.IsNullOrEmpty(entityPOCO.SeverityId))
            {
                TicketSeverityRepository repSeverity = new TicketSeverityRepository(entityPOCO.Tenant);
                TicketSeverity severity = repSeverity.GetSingle(entityPOCO.SeverityId, entityPOCO.Tenant);
                if (severity != null)
                {
                    entityPM.SeverityName = severity.Name;
                }
            }
        }
   }


}
   