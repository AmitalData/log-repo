using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class SchedulerLogsMapping
    {
        public static void MapEntity(SchedulerLogsPM schedulerLogsPM, SchedulerLogs schedulerLogs, bool isNewState)
        {
            if (isNewState)
            {
                schedulerLogs.Id = schedulerLogsPM.Id;
                schedulerLogs.Tenant = schedulerLogsPM.Tenant;
                schedulerLogs.CreateDate = TenantServerConfigration.GetCurrentDateTime(schedulerLogs.Tenant);
            

            }
            else
            {
                schedulerLogs.CreateDate = schedulerLogsPM.CreateDate; 
            }
            schedulerLogs.HistoryId = schedulerLogsPM.HistoryId;
            schedulerLogs.Log = schedulerLogsPM.Log; 
        }
    }
}