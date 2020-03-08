
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
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SealTypeCode);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.UpdateTypeCode);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.UpdateReasonCode);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SealCompletenessStateCode);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.CargoSealIdentifierId = entityPM.CargoSealIdentifierId;
                entityPOCO.Tenant = entityPM.Tenant;
             

            }
            entityPOCO.Id = entityPM.Id;
            entityPOCO.SealNumber = entityPM.SealNumber;
            entityPOCO.SealTypeCode = entityPM.SealTypeCode;
                entityPOCO.UpdateTypeCode = entityPM.UpdateTypeCode;
                entityPOCO.UpdateReasonCode = entityPM.UpdateReasonCode;
                entityPOCO.SealCompletenessStateCode = entityPM.SealCompletenessStateCode;

           
        }

        public void CustomPOCOToPM(CargoSealPM entityPM, CargoSeal entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.SealCompletenessStateName);
            CustomMappedPMProperties.Add(PMPropertyNames.SealTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.UpdateReasonName);
            CustomMappedPMProperties.Add(PMPropertyNames.UpdateTypeName);

            if (entityPOCO.SealCompletenessStateCode != null)
            {
                SealCompletenesQueryService sealCompletenesQueryService = new SealCompletenesQueryService(entityPOCO.Tenant);
                SealCompletenesPM sealCompletenesPM = sealCompletenesQueryService.GetSingle(entityPOCO.SealCompletenessStateCode, false, true);
                if (sealCompletenesPM != null)
                {
                    entityPM.SealCompletenessStateName = sealCompletenesPM.LocalName;
                }
            }

            if (entityPOCO.SealTypeCode != null)
            {
                SealTypeQueryService sealTypeQueryService = new SealTypeQueryService(entityPOCO.Tenant);
                SealTypePM sealTypePM = sealTypeQueryService.GetSingle(entityPOCO.SealTypeCode, false, true);
                if(sealTypePM != null)
                {
                    entityPM.SealTypeName = sealTypePM.LocalName;
                }
            }

            if (entityPOCO.UpdateReasonCode != null)
            {
                SealUpdateReasonTypeQueryService sealUpdateReasonTypeQueryService = new SealUpdateReasonTypeQueryService(entityPOCO.Tenant);
                SealUpdateReasonTypePM sealUpdateReasonTypePM = sealUpdateReasonTypeQueryService.GetSingle(entityPOCO.UpdateReasonCode, false, true);
                if (sealUpdateReasonTypePM != null)
                {
                    entityPM.UpdateReasonName = sealUpdateReasonTypePM.LocalName;
                }
            }

            if (entityPOCO.UpdateTypeCode != null)
            {
                AmendmentTypeQueryService amendmentTypeQueryService = new AmendmentTypeQueryService(entityPOCO.Tenant);
                AmendmentTypePM amendmentTypePM = amendmentTypeQueryService.GetSingle(entityPOCO.UpdateTypeCode, false, true);
                if (amendmentTypePM != null)
                {
                    entityPM.UpdateTypeName = amendmentTypePM.LocalName;
                }
            }
        }
   }


}
   