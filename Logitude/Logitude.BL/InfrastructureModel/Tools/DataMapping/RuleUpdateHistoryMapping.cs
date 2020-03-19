using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class RuleUpdateHistoryMapping
    {
        public static void MapEntity(RuleUpdateHistoryPM entityPM, RuleUpdateHistory entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
           
            }

            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.RuleCode = entityPM.RuleCode;
            entityPOCO.EventName = entityPM.EventName;
             
        }
    }
}
