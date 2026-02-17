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
    public class MessagingStockTracing
    {
        public static void Trace(MessagingStockPM entityPM, MessagingStock poco, string loggedContactId, bool isNewEntity)
        {
            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = 0,
                    EventTypeCode = "CRMS",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "MessagingStock",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.IsCancelled && !poco.IsCancelled)
                {
                    notes = "Messaging Stock Cancelled";
                }

                else if (!entityPM.IsCancelled && poco.IsCancelled)
                {
                    notes = "Messaging Stock Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = 0,
                    EventTypeCode = "UPMS",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "MessagingStock",
                    Notes = notes,
                });
            }
        }
    }
}
