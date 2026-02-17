
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
using Logitude.CRM.Data.EntityKeys;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.CRM.BL.EntityDataMappings
{
   
   public partial class OpportunityProductDataMapping: IMapping<OpportunityProductPM, OpportunityProduct>
   {
        public void CustomPMToPOCO(OpportunityProductPM entityPM, OpportunityProduct entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.OpportunityId);
            entityPOCO.OpportunityId = entityPM.OpportunityId;

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.OpportunityProductTypeCode);
            entityPOCO.OpportunityProductTypeCode = entityPM.OpportunityProductTypeCode;
        }

        public void CustomPOCOToPM(OpportunityProductPM entityPM, OpportunityProduct entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.OpportunityProductTypeName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.PrepaidCollectName);

            ProductTypeRepository rep = new ProductTypeRepository(entityPOCO.Tenant);
            ProductType type = rep.GetSingleProductType(entityPOCO.OpportunityProductTypeCode);
            if (type != null)
            {
                entityPM.OpportunityProductTypeName = type.Name;
            }

            if (!string.IsNullOrEmpty(entityPOCO.PrepaidCollectId))
            {
                PrepaidCollectRepository repo = new PrepaidCollectRepository(entityPOCO.Tenant);
                PrepaidCollect entity = repo.GetSinglePrepaidCollect(entityPOCO.PrepaidCollectId);

                if (entity != null)
                {
                    entityPM.PrepaidCollectName = entity.Name;
                }
            }
        }
   }
}
   