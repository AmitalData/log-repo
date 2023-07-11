
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
   
   public partial class ClientIndicationDataMapping: IMapping<ClientIndicationPM, ClientIndication>
   {

        public void CustomPMToPOCO(ClientIndicationPM entityPM, ClientIndication entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.IndicationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.ClientId);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.IndicationId = entityPM.IndicationId;
                entityPOCO.ClientId = entityPM.ClientId;
            }
        }

        public void CustomPOCOToPM(ClientIndicationPM entityPM, ClientIndication entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.CustomerIndicationTypeName);

           
            if (entityPOCO.CustomerIndicationTypeID != null)
            {
                CustomerIndicationTypeQueryService entityQueryService = new CustomerIndicationTypeQueryService(entityPOCO.Tenant);
                CustomerIndicationTypePM entity = entityQueryService.GetSingle(entityPOCO.CustomerIndicationTypeID, false, true);
                entityPM.CustomerIndicationTypeName = entity.LocalName;
            }

        }
    }


}
   