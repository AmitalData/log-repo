using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.BL.InvoiceModel.Tools.TraceEvents
{
    public class ARPaymentTracing
    {
        public static void Trace(EntityPMs.ARPaymentPM entityPM, ARPayment payment, bool isNewState)
        {
            string myEntityName = "ARPayment";

            ContactPM loggedContact = GetLoggedContactPM(entityPM.Tenant);


            if (isNewState)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRPY",
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
                    EventTypeCode = "UPPY",
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
                        EventTypeCode = "ARPA",
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
                        EventTypeCode = "ARPV",
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
                        EventTypeCode = "ARPC",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = myEntityName,
                    });
                }
            }        
        }
        
        public static ContactPM GetLoggedContactPM(int tenant)
        {
            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }
    }
}
