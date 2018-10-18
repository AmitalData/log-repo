using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class OpportunityAdditionalServiceQueryService
    {
        public List<OpportunityAdditionalServicePM> GetOpportunityAdditionalServicesByOpportunityId(string opportunityId, int tenant)
        {
            List<OpportunityAdditionalServicePM> result = new List<OpportunityAdditionalServicePM>();
            List<OpportunityAdditionalService> opportunityAdditionalServices = repository.GetOpportunityAdditionalServicesByOpportunityId(opportunityId, tenant);

            foreach (OpportunityAdditionalService entity in opportunityAdditionalServices)
            {
                EntityPM = new OpportunityAdditionalServicePM();
                mapping.CustomPOCOToPM(EntityPM, entity);
                mapping.POCOToPM(EntityPM, entity);

                result.Add(EntityPM);
            }

            return result;
        }
    }
}
