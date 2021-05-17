using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.Security;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class CurrencyTracing
    {
        public static void Trace(CurrencyPM entityPM, Currency poco, bool isNewEntity)
        {
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(entityPM.Tenant);
         //    loggedContact =  new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRCR",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Currency",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !poco.InActive)
                {
                    notes = "Currency Inactivated";
                }

                else if (!entityPM.InActive && poco.InActive)
                {
                    notes = "Currency Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPCR",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Currency",
                    Notes = notes,
                });
            }
        }
    }
}
