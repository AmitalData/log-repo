using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class ContactTracing
    {

        private static string GetSystemUserId(int tenant)
        {
            string loggedSystemEmail = "system@tenant" + tenant + ".com";
            UserRepository userRep = new UserRepository(tenant);
            User systemUser = userRep.GetSingleUserByEmail(loggedSystemEmail, tenant, true);            

            return systemUser?.Id;
        }

        public static void Trace(ContactPM entityPM, Contact poco, bool isNewEntity)
        {
            ContactQuery contactQuery = new ContactQuery(entityPM.Tenant);
            UserQuery userQuery = new UserQuery(entityPM.Tenant);

            ContactPM loggedContact = contactQuery.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), 0);

            string userId = null;

            if (loggedContact == null)
            {
                loggedContact = contactQuery.GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);
            }

            userId = loggedContact?.Id;
            var loggedUser = userQuery.GetSinglePMByEmail(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (string.IsNullOrWhiteSpace(userId) || loggedUser == null)
            {
                userId = GetSystemUserId(entityPM.Tenant);

                if (string.IsNullOrEmpty(userId)) return;
            }

            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRCO",
                    UserId = userId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Contact",
                });
            }
            else
            {
                string notes = "";
                if (entityPM.InActive && !poco.InActive)
                {
                    notes = "Contact Inactivated";
                }

                else if (!entityPM.InActive && poco.InActive)
                {
                    notes = "Contact Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPCO",
                    UserId = userId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Contact",
                    Notes = notes,
                });
            }
        }
    }
}
