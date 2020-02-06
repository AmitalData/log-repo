
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
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CargoSealDataMapping: IMapping<CargoSealPM, CargoSeal>
   {

        public void CustomPMToPOCO(CargoSealPM entityPM, CargoSeal entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CargoSealIdentifierId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SealNumber);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.CargoSealIdentifierId = entityPM.CargoSealIdentifierId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.SealNumber = entityPM.SealNumber;
            }
        }

        public void CustomPOCOToPM(CargoSealPM entityPM, CargoSeal entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.SealCompletenessStateName);
            //CustomMappedPMProperties.Add(PMPropertyNames.SealTypeName);
            //CustomMappedPMProperties.Add(PMPropertyNames.TapagTypeName);
            //CustomMappedPMProperties.Add(PMPropertyNames.ReferantName);

            if (entityPOCO.SealCompletenessStateCode != null)
            {
                SealCompletenesQueryService sealCompletenesQueryService = new SealCompletenesQueryService(entityPOCO.Tenant);
                SealCompletenesPM sealCompletenesPM = sealCompletenesQueryService.GetSingle(entityPOCO.SealCompletenessStateCode, false, true);
                entityPM.SealCompletenessStateName = sealCompletenesPM.LocalName;
            }
        }
   }


}
   