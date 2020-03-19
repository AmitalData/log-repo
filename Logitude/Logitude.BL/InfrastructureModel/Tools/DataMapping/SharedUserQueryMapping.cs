using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class SharedUserQueryMapping
    {
        public static void MapEntity(SharedUserQueryPM entityPM, SharedUserQuery entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Tenant = entityPM.Tenant;
            }
            
            entityPOCO.UserId = entityPM.UserId;
            entityPOCO.QueryId = entityPM.QueryId;
            entityPOCO.QueryCode = entityPM.QueryCode;

        }
    }
}
