using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class CustomerGroupTracing
    {
        public static void Trace(CustomerGroupPM entityPM, bool isNewEntity)
        {
            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = entityPM.UpdatedByUserId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "CustomerGroup",
                });
            }

            else
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = entityPM.UpdatedByUserId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "CustomerGroup",
                });
            }
        }
    }
}
