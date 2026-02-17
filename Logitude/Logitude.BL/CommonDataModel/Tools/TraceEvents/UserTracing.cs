using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class UserTracing
    {
        public static void Trace(UserPM entityPM, User poco, bool isNewEntity)
        {
            ContactPM loggedContact = new ContactQuery(0).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), 0);

            if (loggedContact == null)
            {
                loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
            }

            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRUS",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "User",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !poco.Contact.InActive)
                {
                    notes = "User Inactivated";
                }

                else if (!entityPM.InActive && poco.Contact.InActive)
                {
                    notes = "User Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPUS",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "User",
                    Notes = notes,
                });

                if (entityPM.ExpirationDate != poco.ExpirationDate)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "EXUP",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "User",
                    });
                }

                // Roles Changed 
                if (!string.IsNullOrEmpty(entityPM.UserRolesNamesList))
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "RCUS",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = "User",
                        Notes = "Changed from " + "{" + entityPM.UserRolesNamesList_db.TrimEnd(',') + "}" + " to " + "{" + entityPM.UserRolesNamesList.TrimEnd(',') + "}",
                    });
                }
                entityPM.UserRolesNamesList = null;
                entityPM.UserRolesNamesList_db = null;
            }
        }
    }
}
