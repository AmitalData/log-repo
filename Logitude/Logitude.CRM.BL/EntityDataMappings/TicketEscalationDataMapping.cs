
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
   public partial class TicketEscalationDataMapping: IMapping<TicketEscalationPM, TicketEscalation>
   {
        public void CustomPMToPOCO(TicketEscalationPM entityPM, TicketEscalation entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
        }

        public void CustomPOCOToPM(TicketEscalationPM entityPM, TicketEscalation entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.EscalationForName);
            if (!string.IsNullOrEmpty(entityPOCO.EscalationFor))
            {
                if (entityPOCO.EscalationFor == "FR")
                {
                    entityPM.EscalationForName = "First Response";
                }

                else
                {
                    entityPM.EscalationForName = "Resolve Within";
                }
            }
        }
   }
}
   