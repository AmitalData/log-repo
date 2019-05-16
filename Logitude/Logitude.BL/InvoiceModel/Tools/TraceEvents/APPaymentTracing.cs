using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.BL.InvoiceModel.Tools.TraceEvents
{
    public class APPaymentTracing
    {
        public static void Trace(EntityPMs.APPaymentPM entityPM, APPayment payment, bool isNewState)
        {
            string myEntityName = "APPayment";

            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(entityPM.Tenant);

            if (isNewState)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                });
            }

            else
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                });
            }

            if (entityPM.SetApproved)
            {
                if (payment.StatusCode != "AD" && entityPM.StatusCode == "AD")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "APPA",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = myEntityName,
                    });
                }
            }

            else if (entityPM.SetVoided)
            {
                if (payment.StatusCode != "VD" && entityPM.StatusCode == "VD")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "APPV",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = myEntityName,
                    });
                }
            }

            else if (entityPM.SetCancelApproval)
            {
                if (payment.StatusCode != "DR" && entityPM.StatusCode == "DR")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "APPC",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = myEntityName,
                    });
                }
            }
        }
    }
}
