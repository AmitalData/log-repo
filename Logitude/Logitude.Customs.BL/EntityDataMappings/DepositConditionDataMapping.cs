
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
   
   public partial class DepositConditionDataMapping: IMapping<DepositConditionPM, DepositCondition>
   {

        public void CustomPMToPOCO(DepositConditionPM entityPM, DepositCondition entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.DepositId);
            AddPOCOPropertyName(POCOPropertyNames.DepositConditionCode);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
      
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.DepositId = entityPM.DepositId;
                entityPOCO.DepositConditionCode = entityPM.DepositConditionCode;
                entityPOCO.Tenant = entityPM.Tenant;
         
            }
        }

        public void CustomPOCOToPM(DepositConditionPM entityPM, DepositCondition entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.DepositConditionName);
            if (entityPOCO.DepositConditionCode != null)
            {
                ReturnConditionQueryService returnConditionQueryService = new ReturnConditionQueryService(entityPOCO.Tenant);
                ReturnConditionPM returnCondition = returnConditionQueryService.GetSingle(entityPOCO.DepositConditionCode, false, true);
               
                entityPM.DepositConditionName = returnCondition.LocalName;
            }

        }
   }


}
   