using System;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Logitude.BL.QuoteModel.Tools.TraceEvents
{
    public partial class QuoteTracing
    {
        public static void TraceQuoteOnCreateDoneFollowUp(QuotePM entityPM, Quote entityPoco, FollowUp followUp, string loggedContactId)
        {
            int tenant = entityPM.Tenant;
            EventTypeQuery eventTypeQuery = new EventTypeQuery(tenant);
            EventTypePM eventType = eventTypeQuery.GetSingleEventTypePM(followUp.EventTypeId, tenant);

            if (eventType != null)
            {
                if (eventType.ManualActivatedFollowUp)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = eventType.Code,
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Quote",
                        Notes = followUp.DoneNote,
                    });
                }
            }
        }

        public static void TraceQuoteOnUpdateDoneFollowUp(QuotePM entityPM, Quote entityPoco, QuoteFollowUpPM followUpPm, string loggedContactId)
        {
            int tenant = entityPM.Tenant;

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = tenant,
                EventTypeCode = "UPQT",
                UserId = loggedContactId,
                EntityId = entityPM.Id,
                ObjectTableName = "Quote",
            });


            EventTypeQuery eventTypeQuery = new EventTypeQuery(tenant);
            EventTypePM eventType = eventTypeQuery.GetSingleEventTypePM(followUpPm.EventTypeId, tenant);

            if (eventType != null)
            {
                if (eventType.ManualActivatedFollowUp)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = eventType.Code,
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "Quote",
                        Notes = followUpPm.DoneNote,
                        EventDateTime = followUpPm.DoneDateTime,
                    });
                }
            }
        }

    }
}