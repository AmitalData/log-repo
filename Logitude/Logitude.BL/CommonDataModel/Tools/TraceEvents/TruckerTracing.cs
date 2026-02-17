using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class TruckerTracing
    {
        public static void Trace(TruckerPM entityPM, Trucker poco, bool isNewEntity)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRTR",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Trucker",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !poco.Card.InActive)
                {
                    notes = "Trucker Inactivated";
                }

                else if (!entityPM.InActive && poco.Card.InActive)
                {
                    notes = "Trucker Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPTR",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Trucker",
                    Notes = notes,
                });
            }
        }
    }
}