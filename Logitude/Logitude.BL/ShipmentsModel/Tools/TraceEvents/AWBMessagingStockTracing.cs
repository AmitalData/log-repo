using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.TraceEvents
{
    public class AWBMessagingStockTracing
    {
        public static void Trace(AWBMessagingStockPM entityPM, AWBMessagingStock poco, string loggedContactId, bool isNewEntity)
        {
            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = 0,
                    EventTypeCode = "CRMS",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "AWBMessagingStock",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.IsCancelled && !poco.IsCancelled)
                {
                    notes = "AWB Messaging Stock Cancelled";
                }

                else if (!entityPM.IsCancelled && poco.IsCancelled)
                {
                    notes = "AWB Messaging Stock Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = 0,
                    EventTypeCode = "UPMS",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "AWBMessagingStock",
                    Notes = notes,
                });
            }
        }
    }
}
