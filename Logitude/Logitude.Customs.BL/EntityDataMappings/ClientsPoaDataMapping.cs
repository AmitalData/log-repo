
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
   
   public partial class ClientsPoaDataMapping: IMapping<ClientsPoaPM, ClientsPoa>
   {

        public void CustomPMToPOCO(ClientsPoaPM entityPM, ClientsPoa entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ClientsPoaPM entityPM, ClientsPoa entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.PoaStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.PoaAuthorizationTypeName);

            if (entityPOCO.PoaStatus != null)
            {
                PoaStatusTypeLookUpQueryService entityQueryService = new PoaStatusTypeLookUpQueryService(entityPOCO.Tenant);
                PoaStatusTypeLookUpPM entity = entityQueryService.GetSingle(entityPOCO.PoaStatus, false, true);
                entityPM.PoaStatusName = entity.LocalName;
            }
            if (entityPOCO.PoaAuthorizationType != null)
            {
                PoaAuthorizationTypeLookupQueryService entityQueryService = new PoaAuthorizationTypeLookupQueryService(entityPOCO.Tenant);
                PoaAuthorizationTypeLookupPM entity = entityQueryService.GetSingle(entityPOCO.PoaAuthorizationType, false, true);
                entityPM.PoaAuthorizationTypeName = entity.LocalName;
            }

        }
   }


}
   