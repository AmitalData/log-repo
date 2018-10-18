using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class HybridTenantThresholdMapping
    {
        public static void MapEntity(HybridTenantThresholdPM entityPM,HybridTenantThreshold entityPoco, bool isNewState)
        {
            if (isNewState)
            {
               
                entityPoco.Tenant = entityPM.Tenant;
            }

            entityPoco.WaitingThresold = entityPM.WaitingThresold;
            entityPoco.FailedThresold = entityPM.FailedThresold;

       
        }
    }
}