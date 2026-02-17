using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.BL.InvoiceModel.Tools.TraceEvents
{
    public class BankAccountLiteTracing
    {
        public static void Trace(EntityPMs.BankAccountLitePM entityPM, BankAccountLite payment, bool isNewState)
        {
            string myEntityName = "BankAccountLite";
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
            if (isNewState)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRBA",
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
                    EventTypeCode = "UPBA",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                });
            }         
        }
    }
}
