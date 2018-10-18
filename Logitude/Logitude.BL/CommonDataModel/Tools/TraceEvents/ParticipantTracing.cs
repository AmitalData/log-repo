using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class ParticipantTracing
    {
        public static void Trace(ParticipantPM entityPM, Participant poco, bool isNewEntity)
        {
            ContactPM loggedContact = null;
            if (HttpContext.Current != null)
            {
                loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
            }
            else
            {
                loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly("system@tenant0.com", entityPM.Tenant);
            }

            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRPC",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Participant",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !poco.Card.InActive)
                {
                    notes = "Participant Inactivated";
                }

                else if (!entityPM.InActive && poco.Card.InActive)
                {
                    notes = "Participant Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPPC",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Participant",
                    Notes = notes,
                });
            }
        }
    }
}
