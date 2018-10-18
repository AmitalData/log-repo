
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
   
   public partial class OpportunityAdditionalServiceDataMapping: IMapping<OpportunityAdditionalServicePM, OpportunityAdditionalService>
   {

        public void CustomPMToPOCO(OpportunityAdditionalServicePM entityPM, OpportunityAdditionalService entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.AdditionalServiceId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.OpportunityId);

            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.AdditionalServiceId = entityPM.AdditionalServiceId;
            entityPOCO.OpportunityId = entityPM.OpportunityId;
        }

        public void CustomPOCOToPM(OpportunityAdditionalServicePM entityPM, OpportunityAdditionalService entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.EnglishName);

            AdditionalServiceRepository rep = new AdditionalServiceRepository(entityPOCO.Tenant);
            AdditionalService type = rep.GetSingleAdditionalService(entityPOCO.AdditionalServiceId, entityPOCO.Tenant);
            if (type != null)
            {
                entityPM.EnglishName = type.Name;
            }
        }
   }


}
   