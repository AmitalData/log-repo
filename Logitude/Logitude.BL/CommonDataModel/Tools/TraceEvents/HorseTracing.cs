using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class HorseTracing
    {
        public static void Trace(HorsePM entityPM, Horse entityPOCO, bool isNewEntity, string loggedContactId)
        {
            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRIT",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Horse",
                });
            }

            else
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Horse",
                });

                if (entityPM.Inactive && !entityPOCO.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "HRIN",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Horse",
                    });
                }

                else if (!entityPM.Inactive && entityPOCO.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "HRRC",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Horse",
                    });
                }
            }
        }
    }
}
