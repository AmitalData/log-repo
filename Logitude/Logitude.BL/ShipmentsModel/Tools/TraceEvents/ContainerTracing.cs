using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;

namespace Logitude.BL.ShipmentsModel.Tools.TraceEvents
{
    public class ContainerTracing
    {

        public static void Trace(ContainerPM entityPM, Container container, bool isNewEntity)
        {
            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = entityPM.CreatedByUserId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Container",
                });
            }
            else {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = entityPM.UpdatedByUserId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Container",
                });
            }

            TraceClosedEvent(entityPM, container);
        }

        private static void TraceClosedEvent(ContainerPM entityPM, Container container)
        {
            if (container.IsClosed && !entityPM.IsClosed)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "ROCO",
                    UserId = entityPM.CreatedByUserId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Container",
                });
            }
       
            if (!container.IsClosed && entityPM.IsClosed)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CODC",
                    UserId = entityPM.CreatedByUserId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Container",
                });
            }
        }
    }
}
