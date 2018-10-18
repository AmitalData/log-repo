using Logitude.Server.Tools.Counters;
using Logitude.TimeManagement.BL.EntityPMs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagement.BL.EntityUpdateServices
{
    public partial class TMOfficeHourUpdateService
    {
        protected override void OnCreating(TMOfficeHourPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("TMOfficeHour", entityPM.Tenant);
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                if(entityPM.RecordedEntryTime!=null)
                entityPM.EntryTime = entityPM.RecordedEntryTime;
                if (entityPM.RecordedExitTime != null)
                    entityPM.ExitTime = entityPM.RecordedExitTime;
            }
        }

        protected override void OnUpdating(TMOfficeHourPM entityPM)
        {
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
        }

    }
}
