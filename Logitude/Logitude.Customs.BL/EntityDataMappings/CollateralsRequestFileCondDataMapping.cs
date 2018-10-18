
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

    public partial class CollateralsRequestFileCondDataMapping : IMapping<CollateralsRequestFileCondPM, CollateralsRequestFileCond>
   {

        public void CustomPMToPOCO(CollateralsRequestFileCondPM entityPM, CollateralsRequestFileCond entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.CustomsCollateralId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.LineNumber);
            AddPOCOPropertyName(POCOPropertyNames.ConditionCode);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.CustomsCollateralId = entityPM.CustomsCollateralId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.LineNumber = entityPM.LineNumber;
                entityPOCO.ConditionCode = entityPM.ConditionCode;
            }
        }

        public void CustomPOCOToPM(CollateralsRequestFileCondPM entityPM, CollateralsRequestFileCond entityPOCO)
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
   