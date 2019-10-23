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
    public class AccountingPartnerTracing
    {
        public static void Trace(AccountingPartnerPM entityPM, AccountingPartner poco, bool isNewEntity)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRAC",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "AccountingPartner",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !poco.Card.InActive)
                {
                    notes = "AccountingPartner Inactivated";
                }

                else if (!entityPM.InActive && poco.Card.InActive)
                {
                    notes = "AccountingPartner Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAC",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "AccountingPartner",
                    Notes = notes,
                });
            }
        }
    }
}