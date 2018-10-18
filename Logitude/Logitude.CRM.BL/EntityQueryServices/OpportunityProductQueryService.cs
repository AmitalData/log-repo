using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.BL.EntityQueryServices
{
    public partial class OpportunityProductQueryService
    {
        public override void GetComposition(EntityKeyFields entityKeys, OpportunityProductPM entityPM)
        {
            ICRMContext context = MainContext as ICRMContext;

            OpportunityProductKeys keys = entityKeys as OpportunityProductKeys;

            OpportunityProductLocationQueryService queryLocationsService = new OpportunityProductLocationQueryService(context);
            entityPM.OpportunityProductLocations = queryLocationsService.GetMulti(keys, true);

            //OpportunityProductCompetitorQueryService queryCompetitorsService = new OpportunityProductCompetitorQueryService(context);
            //entityPM.OpportunityProductCompetitors = queryCompetitorsService.GetMulti(keys, true);
        }

        public List<OpportunityProductPM> GetOpportunityProductsByOpportunityId(string opportunityId, int tenant)
        {
            List<OpportunityProductPM> result = new List<OpportunityProductPM>();
            List<OpportunityProduct> opportunityProducts = repository.GetProductsByOpportunityId(opportunityId, tenant);

            foreach (OpportunityProduct entity in opportunityProducts)
            {
                EntityPM = new OpportunityProductPM();
                mapping.CustomPOCOToPM(EntityPM, entity);
                mapping.POCOToPM(EntityPM, entity);

                result.Add(EntityPM);
            }

            OpportunityProductLocationQueryService service = new OpportunityProductLocationQueryService(tenant);
            foreach (OpportunityProductPM item in result)
            {
                item.OpportunityProductLocations = service.GetOpportunityProductLocationByKeys(opportunityId, item.OpportunityProductTypeCode, tenant);
            }

            return result;
        }
    }
}
