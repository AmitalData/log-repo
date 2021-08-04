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
    public class ComputingPartnerTracing
    {
        public static void Trace(ComputingPartnerPM entityPM, ComputingPartner poco, bool isNewEntity)
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
                    ObjectTableName = "ComputingPartner",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !poco.InActive)
                {
                    notes = "Computing Partner Inactivated";
                }

                else if (!entityPM.InActive && poco.InActive)
                {
                    notes = "Computing Partner Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "ComputingPartner",
                    Notes = notes,
                });
            }
        }
    }
}