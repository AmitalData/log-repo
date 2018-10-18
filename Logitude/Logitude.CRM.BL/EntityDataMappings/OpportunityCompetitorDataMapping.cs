
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
   
   public partial class OpportunityCompetitorDataMapping: IMapping<OpportunityCompetitorPM, OpportunityCompetitor>
   {
        public void CustomPMToPOCO(OpportunityCompetitorPM entityPM, OpportunityCompetitor entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CompetitorId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.OpportunityId);

            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.CompetitorId = entityPM.CompetitorId;
            entityPOCO.OpportunityId = entityPM.OpportunityId;
        }

        public void CustomPOCOToPM(OpportunityCompetitorPM entityPM, OpportunityCompetitor entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.EnglishName);

            CompetitorRepository rep = new CompetitorRepository(entityPOCO.Tenant);
            Competitor type = rep.GetSingleCompetitor(entityPOCO.CompetitorId, entityPOCO.Tenant);
            if (type != null)
            {
                entityPM.EnglishName = type.Name;
            }
        }
   }


}
   