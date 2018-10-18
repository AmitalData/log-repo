
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
   
   public partial class GuaranteeConditionDataMapping: IMapping<GuaranteeConditionPM, GuaranteeCondition>
   {

       public void CustomPMToPOCO(GuaranteeConditionPM entityPM, GuaranteeCondition entityPOCO)
       {
           AddPOCOPropertyName(POCOPropertyNames.Id);
           AddPOCOPropertyName(POCOPropertyNames.Tenant);
           if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
           {
               entityPOCO.Id = entityPM.Id;
               entityPOCO.Tenant = entityPM.Tenant;
               entityPOCO.GuaranteeId = entityPM.GuaranteeId;
           }
       }

       public void CustomPOCOToPM(GuaranteeConditionPM entityPM, GuaranteeCondition entityPOCO)
       {
           CustomMappedPMProperties.Add(PMPropertyNames.ReturnConditionName);

           if (entityPOCO.ReturnConditionCode != null)
           {
               ReturnConditionQueryService entityQuery = new ReturnConditionQueryService(entityPM.Tenant);
               ReturnConditionPM entity = entityQuery.GetSingle(entityPOCO.ReturnConditionCode, false, false);
               if (entity != null)
                   entityPM.ReturnConditionName = entity.LocalName;
           }


       }
   }


}
   