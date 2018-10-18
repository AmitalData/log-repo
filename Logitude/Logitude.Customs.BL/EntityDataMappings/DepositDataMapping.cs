
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
   
   public partial class DepositDataMapping: IMapping<DepositPM, Deposit>
   {

        public void CustomPMToPOCO(DepositPM entityPM, Deposit entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
      

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;

            }
        }

        public void CustomPOCOToPM(DepositPM entityPM, Deposit entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.EntityTypeName);

            EntityTypeLookupQueryService entityTypeLookupQueryService = new EntityTypeLookupQueryService(entityPOCO.Tenant);
            EntityTypeLookupPM entityTypeLookup = entityTypeLookupQueryService.GetSingle(entityPOCO.EntityTypeCode, false  , true);
            if (entityTypeLookup != null)
            {
                entityPM.EntityTypeName = entityTypeLookup.LocalName;
              
            }

        }
   }


}
   