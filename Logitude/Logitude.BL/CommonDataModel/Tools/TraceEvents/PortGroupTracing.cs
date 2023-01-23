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

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class PortGroupTracing
    {
        public static void Trace(PortGroupPM entityPM, PortGroup poco, bool isNewEntity)
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
                    ObjectTableName = "PortGroup",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.Inactive && !poco.Inactive)                
                    notes = "PortGroup Inactivated";                

                else if (!entityPM.Inactive && poco.Inactive)                
                    notes = "PortGroup Activated";                

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "PortGroup",
                    Notes = notes,
                });
            }
        }
    }
}
