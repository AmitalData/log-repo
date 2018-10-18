using System;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.Tools.TraceEvents
{
    public partial class ShipmentTracing
    {
        private static void TraceShipmentRoutingData(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, EventTypeQuery eventTypeQuery, ObjectTablePM objectTable, string serviceContextUser, string loggedContactId)
        {
            int tenant = entityPM.Tenant;
            TraceEventRepository traceeventRep = new TraceEventRepository(tenant);

            if (entityPoco.PreCarriageATA == null && entityPM.PreCarriageATA != null)
            {
                EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("PRCA", tenant);
                TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = tenant, UserId = loggedContactId };
                traceEvent.EventDateTime = entityPM.PreCarriageATA != null ? entityPM.PreCarriageATA.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                DateTime? statusDate = entityPoco.StatusDate;
                entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", tenant, loggedContactId, entityPM.Id, "To " + entityPM.PreCarriageToPortName, "Shipment", entityPoco.StatusId, null, false, ref statusDate);
                if (entityPoco.StatusDate != statusDate)
                {
                    entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }
                entityPoco.StatusDate = statusDate;
                entityPM.StatusId = entityPoco.StatusId;
                entityPM.StatusDate = entityPoco.StatusDate;
                if (entityPoco.ShipmentLevelCode == "D" || entityPoco.ShipmentLevelCode == "C")
                {
                    entityMasterData.StatusDate = entityPoco.StatusDate;
                    entityMasterData.StatusId = entityPoco.StatusId;
                }
            }

            if (entityPoco.PreCarriageATA != null && entityPM.PreCarriageATA == null)
            {
                EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("PRCA", tenant);
                TraceEvent traceEvent = traceeventRep.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
                DeleteShipmentTraceEvent(entityPM, traceEvent.Id, tenant, false);
                if (entityPoco.StatusDate != entityPM.StatusDate)
                {
                    entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }
                entityPoco.StatusId = entityPM.StatusId;
                entityPoco.StatusDate = entityPM.StatusDate;
                if (entityPoco.ShipmentLevelCode == "D" || entityPoco.ShipmentLevelCode == "C")
                {
                    entityMasterData.StatusDate = entityPoco.StatusDate;
                    entityMasterData.StatusId = entityPoco.StatusId;
                }
            }

            if (entityPoco.PreCarriageATD == null && entityPM.PreCarriageATD != null)
            {
                EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("PRCD", tenant);
                TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = tenant, UserId = loggedContactId };
                traceEvent.EventDateTime = entityPM.PreCarriageATD != null ? entityPM.PreCarriageATD.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                DateTime? statusDate = entityPoco.StatusDate;
                entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", tenant, loggedContactId, entityPM.Id, "From " + entityPM.PreCarriageFromPortName, "Shipment", entityPoco.StatusId, null, false, ref statusDate);
                if (entityPoco.StatusDate != statusDate)
                {
                    entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }
                entityPoco.StatusDate = statusDate;
                entityPM.StatusId = entityPoco.StatusId;
                entityPM.StatusDate = entityPoco.StatusDate;
                if (entityPoco.ShipmentLevelCode == "D" || entityPoco.ShipmentLevelCode == "C")
                {
                    entityMasterData.StatusDate = entityPoco.StatusDate;
                    entityMasterData.StatusId = entityPoco.StatusId;
                }
            }

            if (entityPoco.PreCarriageATD != null && entityPM.PreCarriageATD == null)
            {
                EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("PRCD", tenant);
                TraceEvent traceEvent = traceeventRep.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
                DeleteShipmentTraceEvent(entityPM, traceEvent.Id, tenant, false);
                if (entityPoco.StatusDate != entityPM.StatusDate)
                {
                    entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }
                entityPoco.StatusId = entityPM.StatusId;
                entityPoco.StatusDate = entityPM.StatusDate;
                if (entityPoco.ShipmentLevelCode == "D" || entityPoco.ShipmentLevelCode == "C")
                {
                    entityMasterData.StatusDate = entityPoco.StatusDate;
                    entityMasterData.StatusId = entityPoco.StatusId;
                }
            }

            if (entityPoco.OnCarriageATA == null && entityPM.OnCarriageATA != null)
            {
                EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("ONCA", tenant);
                TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = tenant, UserId = loggedContactId };
                traceEvent.EventDateTime = entityPM.OnCarriageATA != null ? entityPM.OnCarriageATA.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                DateTime? statusDate = entityPoco.StatusDate;
                entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", tenant, loggedContactId, entityPM.Id, "To " + entityPM.OnCarriageToPortName, "Shipment", entityPoco.StatusId, null, false, ref statusDate);
                if (entityPoco.StatusDate != statusDate)
                {
                    entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }
                entityPoco.StatusDate = statusDate;
                entityPM.StatusId = entityPoco.StatusId;
                entityPM.StatusDate = entityPoco.StatusDate;
                if (entityPoco.ShipmentLevelCode == "D" || entityPoco.ShipmentLevelCode == "C")
                {
                    entityMasterData.StatusDate = entityPoco.StatusDate;
                    entityMasterData.StatusId = entityPoco.StatusId;
                }
            }

            if (entityPoco.OnCarriageATA != null && entityPM.OnCarriageATA == null)
            {
                EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("ONCA", tenant);
                TraceEvent traceEvent = traceeventRep.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
                DeleteShipmentTraceEvent(entityPM, traceEvent.Id, tenant, false);
                if (entityPoco.StatusDate != entityPM.StatusDate)
                {
                    entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }
                entityPoco.StatusId = entityPM.StatusId;
                entityPoco.StatusDate = entityPM.StatusDate;
                if (entityPoco.ShipmentLevelCode == "D" || entityPoco.ShipmentLevelCode == "C")
                {
                    entityMasterData.StatusDate = entityPoco.StatusDate;
                    entityMasterData.StatusId = entityPoco.StatusId;
                }
            }

            if (entityPoco.OnCarriageATD == null && entityPM.OnCarriageATD != null)
            {
                EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("ONCD", tenant);
                TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = tenant, UserId = loggedContactId };
                traceEvent.EventDateTime = entityPM.OnCarriageATD != null ? entityPM.OnCarriageATD.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                DateTime? statusDate = entityPoco.StatusDate;
                entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", tenant, loggedContactId, entityPM.Id, "From " + entityPM.OnCarriageFromPortName, "Shipment", entityPoco.StatusId, null, false, ref statusDate);
                if (entityPoco.StatusDate != statusDate)
                {
                    entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }
                entityPoco.StatusDate = statusDate;
                entityPM.StatusId = entityPoco.StatusId;
                entityPM.StatusDate = entityPoco.StatusDate;
                if (entityPoco.ShipmentLevelCode == "D" || entityPoco.ShipmentLevelCode == "C")
                {
                    entityMasterData.StatusDate = entityPoco.StatusDate;
                    entityMasterData.StatusId = entityPoco.StatusId;
                }
            }

            if (entityPoco.OnCarriageATD != null && entityPM.OnCarriageATD == null)
            {
                EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("ONCD", tenant);
                TraceEvent traceEvent = traceeventRep.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
                DeleteShipmentTraceEvent(entityPM, traceEvent.Id, tenant, false);
                if (entityPoco.StatusDate != entityPM.StatusDate)
                {
                    entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                }
                entityPoco.StatusId = entityPM.StatusId;
                entityPoco.StatusDate = entityPM.StatusDate;
                if (entityPoco.ShipmentLevelCode == "D" || entityPoco.ShipmentLevelCode == "C")
                {
                    entityMasterData.StatusDate = entityPoco.StatusDate;
                    entityMasterData.StatusId = entityPoco.StatusId;
                }
            }
        }
    }
}
