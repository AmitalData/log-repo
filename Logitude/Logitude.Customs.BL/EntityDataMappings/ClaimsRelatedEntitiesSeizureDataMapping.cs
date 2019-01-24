
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ClaimsRelatedEntitiesSeizureDataMapping: IMapping<ClaimsRelatedEntitiesSeizurePM, ClaimsRelatedEntitiesSeizure>
   {

        public void CustomPMToPOCO(ClaimsRelatedEntitiesSeizurePM entityPM, ClaimsRelatedEntitiesSeizure entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ClaimsRelatedEntitiesSeizurePM entityPM, ClaimsRelatedEntitiesSeizure entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.SeizureMethodName);
            CustomMappedPMProperties.Add(PMPropertyNames.SeizureFactorName);

            if (entityPOCO.SeizureMethodCode != null)
            {
                SeizureMethodTypeQueryService seizureMethodTypeQueryService = new SeizureMethodTypeQueryService(entityPOCO.Tenant);
                SeizureMethodTypePM seizureMethodTypePM = seizureMethodTypeQueryService.GetSingle(entityPOCO.SeizureMethodCode, false, true);
                entityPM.SeizureMethodName = seizureMethodTypePM.LocalName;
            }

            if (entityPOCO.SeizureFactorCode != null)
            {
                SeizureFactorTypeQueryService seizureFactorTypeQueryService = new SeizureFactorTypeQueryService(entityPOCO.Tenant);
                SeizureFactorTypePM seizureFactorTypePM = seizureFactorTypeQueryService.GetSingle(entityPOCO.SeizureFactorCode, false, true);
                entityPM.SeizureFactorName = seizureFactorTypePM.LocalName;
            }
        }
   }


}
   