using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
   public class DWQueryMapping
    {
        public static void MapEntity(DWQueryPM entityPM, DWQuery entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
            entityPOCO.CreatedDate = entityPM.CreatedDate;
            entityPOCO.UpdateByUserId = entityPM.UpdateByUserId;
            entityPOCO.UpdatedDate = entityPM.UpdatedDate;
            entityPOCO.SQLString = entityPM.SQLString;
            //entityPOCO.DWObjectTableCode = entityPM.DWObjectTableCode;


        }
    }
}
