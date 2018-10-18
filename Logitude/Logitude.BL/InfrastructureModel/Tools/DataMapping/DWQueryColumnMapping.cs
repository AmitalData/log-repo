
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class DWQueryColumnMapping
    {
        public static void MapEntity(DWQueryColumnPM entityPM, DWQueryColumn entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.ColumnWidth = entityPM.ColumnWidth;
            entityPOCO.IndexOrder = entityPM.IndexOrder;
            entityPOCO.DWObjectFieldId = entityPM.DWObjectFieldId;
            entityPOCO.DWQueryId = entityPM.DWQueryId;
            entityPOCO.UserId = entityPM.UserId;

        }
    }
}
