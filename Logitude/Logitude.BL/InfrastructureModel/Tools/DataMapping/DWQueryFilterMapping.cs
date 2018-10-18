

using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class DWQueryFilterMapping
    {
        public static void MapEntity(DWQueryFilterPM entityPM, DWQueryFilter entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.IndexOrder = entityPM.IndexOrder;
            entityPOCO.IsPredefined = entityPM.IsPredefined;
            entityPOCO.DWObjectFieldId = entityPM.DWObjectFieldId;
            entityPOCO.Operator = entityPM.Operator;
            entityPOCO.PredefinedValue = entityPM.PredefinedValue;
            entityPOCO.PredefinedValue2 = entityPM.PredefinedValue2;
            entityPOCO.DWQueryId = entityPM.DWQueryId;

            entityPOCO.UserId = entityPM.UserId;
           

        }
    }
}
