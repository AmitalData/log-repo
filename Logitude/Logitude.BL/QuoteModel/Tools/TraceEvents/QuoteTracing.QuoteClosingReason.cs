using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.Tools.TraceEvents
{
    public partial class QuoteTracing
    {
        public static void Trace(QuoteClosingReasonPM entityPM, QuoteClosingReason poco, bool isNewEntity)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "QuoteClosingReason",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.Inactive && !poco.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "MAIN",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "QuoteClosingReason",
                        Notes = notes,
                    });
                }

                else if (!entityPM.Inactive && poco.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "REAC",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "QuoteClosingReason",
                        Notes = notes,
                    });
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "QuoteClosingReason",
                    Notes = notes,
                });
            }
        }
    }
}
