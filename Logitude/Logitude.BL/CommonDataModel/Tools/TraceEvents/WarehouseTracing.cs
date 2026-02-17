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
using System.Diagnostics;
using Simplog.Data.CommonDataModel.Repositories;
namespace Logitude.BL.CommonDataModel.Tools.TraceEvents
{
    public class WarehouseTracing
    {
        public static void Trace(WarehousePM entityPM, Warehouse poco, bool isNewEntity)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRWH",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Warehouse",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !poco.Card.InActive)
                {
                    notes = "Warehouse Inactivated";
                }

                else if (!entityPM.InActive && poco.Card.InActive)
                {
                    notes = "Warehouse Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPWH",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Warehouse",
                    Notes = notes,
                });
            }
        }

        public static void Trace(WarehousePM entityPM, Warehouse poco, bool isNewEntity,string loggedUserId)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetSinglePM(loggedUserId, entityPM.Tenant);

            if (isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRWH",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Warehouse",
                });
            }

            else
            {
                string notes = "";
                if (entityPM.InActive && !poco.Card.InActive)
                {
                    notes = "Warehouse Inactivated";
                }

                else if (!entityPM.InActive && poco.Card.InActive)
                {
                    notes = "Warehouse Activated";
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPWH",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Warehouse",
                    Notes = notes,
                });
            }
        }
    }
}
