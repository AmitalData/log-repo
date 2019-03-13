using Logitude.Server.Tools.Counters;
using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagement.BL.EntityUpdateServices
{
    public partial class TMEmployeeTimeUpdateService
    {
        protected override void OnCreating(TMEmployeeTimePM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("TMEmployeeTime", entityPM.Tenant);
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.FullDuration = entityPM.TimeInMinutes;
                entityPM.NeedsProrating = true;
            }
        }

        protected override void OnUpdating(TMEmployeeTimePM entityPM)
        {
            entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }
        }

        protected override void OnUpdating(EntityPMs.TMEmployeeTimePM entityPM, TMEmployeeTime entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                if(entityPM.TimeInMinutes != entityPOCO.TimeInMinutes)
                {
                    if (entityPM.ProratedDuration == 0)
                    {
                        entityPM.FullDuration = entityPM.TimeInMinutes;
                    }

                    entityPM.NeedsProrating = true;
                }
            }
        }

        protected override void AfterUpdating(TMEmployeeTimePM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            base.AfterUpdating(entityPM, entityParentPM);
            
        }

    }
}
