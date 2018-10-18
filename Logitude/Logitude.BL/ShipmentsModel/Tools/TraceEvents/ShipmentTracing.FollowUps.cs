using System;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.Tools.TraceEvents
{
    public partial class ShipmentTracing
    {
        internal static void TraceShipmentOnCreateDoneFollowUp(ShipmentPM entityPM, Shipment entityPoco, FollowUp followUp, string loggedContactId)
        {
            int tenant = entityPM.Tenant;
            EventTypeQuery eventTypeQuery = new EventTypeQuery(tenant);
            EventTypePM eventType = eventTypeQuery.GetSingleEventTypePM(followUp.EventTypeId, tenant);

            if (eventType.ManualActivatedFollowUp)
            {
                string objectTableName = "Shipment";
                ObjectTablePM objectTable = ObjectTabelQuery.GetObjectTableByCode(objectTableName, tenant);

                TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = followUp.DoneNote, Tenant = tenant, UserId = loggedContactId };
                entityPM.StatusId = EventTracer.CreateTraceEvent(traceEvent, "", tenant, loggedContactId, entityPM.Id, followUp.DoneNote, objectTableName, entityPoco.StatusId, null, false);
            }
        }

        internal static void TraceShipmentOnUpdateDoneFollowUp(ShipmentPM entityPM, Shipment entityPoco, ShipmentFollowUpPM followUpPm, string loggedContactId)
        {
            int tenant = entityPM.Tenant;

            EventTypeQuery eventTypeQuery = new EventTypeQuery(tenant);
            string objectTableName = "Shipment";
            ObjectTablePM objectTable = ObjectTabelQuery.GetObjectTableByCode(objectTableName, entityPM.Tenant);
            EventTypePM eventType = eventTypeQuery.GetSingleEventTypePM(followUpPm.EventTypeId, tenant);

            entityPM.StatusId = EventTracer.CreateTraceEvent(new TraceEvent(), "USHI", tenant, loggedContactId, entityPM.Id, null, objectTableName, entityPoco.StatusId, null, false);

            if (eventType.ManualActivatedFollowUp)
            {
                TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = followUpPm.DoneNote, Tenant = tenant, UserId = loggedContactId };

                if (followUpPm.DoneDateTime != null && followUpPm.DoneDateTime != new DateTime(0001, 01, 01))
                {
                    traceEvent.EventDateTime = followUpPm.DoneDateTime != null ? followUpPm.DoneDateTime.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                }

                else
                {
                    traceEvent.EventDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                }

                entityPM.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", tenant, loggedContactId, entityPM.Id, followUpPm.DoneNote, objectTableName, entityPoco.StatusId, null, false);
            }
        }
    }
}