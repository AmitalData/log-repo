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
    public class ShippingAgentTracing
    {
        public static void Trace(ShippingAgentPM entityPM, ShippingAgent poco, bool isNewEntity)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRSA",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ShippingAgent",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !poco.Card.InActive)
                {
                    notes = "Shipping Agent Inactivated";
                }

                else if (!entityPM.InActive && poco.Card.InActive)
                {
                    notes = "Shipping Agent Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPSA",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ShippingAgent",
                    Notes = notes,
                });
            }
        }
    }
}