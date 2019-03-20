using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Logitude.TimeManagement.BL.EntityPMs;
using Logitude.TimeManagement.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TimeManagement.BL.TraceEvents
{
    public class TMProjectTracing
    {
        public static void TraceUpdate(TMProjectPM entityPM, TMProject poco)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
          
                if (entityPM.Inactive && !poco.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "DCTC",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "TMProject",
                    });
                }

                else if (!entityPM.Inactive && poco.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "RCTV",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "TMProject",
                    });
                }


                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "UPEV",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "TMProject",
                    });
                
            
        }

        public static void TraceNew(TMProjectPM entityPM)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

         
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "TMProject",
                });
          
            
        }

    }
}
