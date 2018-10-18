
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
   
   public partial class SLAEscalationDataMapping: IMapping<SLAEscalationPM, SLAEscalation>
   {

        public void CustomPMToPOCO(SLAEscalationPM entityPM, SLAEscalation entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            entityPOCO.Id = entityPM.Id;
        }

        public void CustomPOCOToPM(SLAEscalationPM entityPM, SLAEscalation entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.TimeUnitName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.TimeIndicator);

            if (!string.IsNullOrEmpty(entityPOCO.EscalationActionTimeIndicator))
            {
                EscalationActionTimeIndicatorRepository repEscalation = new EscalationActionTimeIndicatorRepository(entityPOCO.Tenant);
                EscalationActionTimeIndicator escalation = repEscalation.GetSingle(entityPOCO.EscalationActionTimeIndicator);
                if (escalation != null)
                {
                    entityPM.TimeIndicator = escalation.Name;
                }
            }

            if (!string.IsNullOrEmpty(entityPOCO.EscalationTimeUnit))
            {
                TimeUnitRepository repTimeUnit = new TimeUnitRepository(entityPOCO.Tenant);
                TimeUnit time = repTimeUnit.GetSingle(entityPOCO.EscalationTimeUnit);
                if (time != null)
                {
                    entityPM.TimeUnitName = time.Name;
                }
            }

        }
   }
}
   