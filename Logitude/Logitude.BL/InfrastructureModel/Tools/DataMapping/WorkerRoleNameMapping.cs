using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class WorkerRoleNameMapping
    {
        public static void MapEntity(WorkerRoleNamePM entityPM, WorkerRoleName entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Name = entityPM.Name;
                entityPOCO.CreateDate = TenantServerConfigration.GetCurrentDateTime(0);
                entityPOCO.WaitingStatus = entityPM.WaitingStatus;
            }
        }
    }
}
