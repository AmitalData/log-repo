
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
   
   public partial class CustomsCollateralsConditionDataMapping: IMapping<CustomsCollateralsConditionPM, CustomsCollateralsCondition>
   {

        public void CustomPMToPOCO(CustomsCollateralsConditionPM entityPM, CustomsCollateralsCondition entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.CustomsCollateralId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.ConditionCode);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.CustomsCollateralId = entityPM.CustomsCollateralId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.ConditionCode = entityPM.ConditionCode;
            }
        }

        public void CustomPOCOToPM(CustomsCollateralsConditionPM entityPM, CustomsCollateralsCondition entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.ConditionName);

            if (entityPOCO.ConditionCode != null)
            {
                ReturnConditionQueryService returnConditionQueryService = new ReturnConditionQueryService(entityPOCO.Tenant);
                ReturnConditionPM returnCondition = returnConditionQueryService.GetSingle(entityPOCO.ConditionCode, false, true);
                entityPM.ConditionName = returnCondition.LocalName;
            }


        }
   }


}
   