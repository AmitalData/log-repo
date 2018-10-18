using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class OpportunityProductLocationQueryService
    {
        public List<OpportunityProductLocationPM> GetOpportunityProductLocationByKeys(string opportunityId, string productCode, int tenant)
        {
            OpportunityProductKeys keys = new OpportunityProductKeys() { OpportunityId = opportunityId, OpportunityProductTypeCode = productCode };

            List<OpportunityProductLocationPM> result = new List<OpportunityProductLocationPM>();
            List<OpportunityProductLocation> opportunityProductLocations = repository.GetMulti(keys);

            foreach (OpportunityProductLocation entity in opportunityProductLocations)
            {
                EntityPM = new OpportunityProductLocationPM();
                mapping.CustomPOCOToPM(EntityPM, entity);
                mapping.POCOToPM(EntityPM, entity);

                result.Add(EntityPM);
            }

            return result;
        }
    }
}
