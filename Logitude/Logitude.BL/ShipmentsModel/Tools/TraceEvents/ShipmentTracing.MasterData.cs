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
        private static void TraceMasterData(ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, EventTypeQuery eventTypeQuery, string serviceContextUser, ObjectTablePM objectTable, string loggedContactId, bool isNewEntity)
        {
            if (entityPM.ShipmentLevelCode == "D" || entityPM.ShipmentLevelCode == "C")
            {
                int tenant = entityPM.Tenant;
                TraceEventRepository traceEventRepository = new TraceEventRepository(tenant);

                if (entityMasterData.MainCarriageATD == null && entityPM.MainCarriageATD != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("DEP", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = tenant, UserId = loggedContactId };
                        if (traceEvent != null)
                        {
                            traceEvent.EventDateTime = entityPM.MainCarriageATD != null ? entityPM.MainCarriageATD.Value : TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            DateTime? statusDate = entityPoco.StatusDate;
                            entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", entityPM.Tenant, loggedContactId, entityPM.Id, null, "Shipment", entityPoco.StatusId, null, false, ref statusDate, entityPM.CustomerId);
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = statusDate;

                            if (entityPoco.StatusDate != statusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = statusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }

                if (entityMasterData.MainCarriageATD != null && entityPM.MainCarriageATD == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("DEP", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
                        if (traceEvent != null)
                        {
                            DeleteShipmentTraceEvent(entityPM, traceEvent.Id, entityPM.Tenant, false);
                            entityPoco.StatusId = entityPM.StatusId;
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = entityPM.StatusDate;
                            if (entityPoco.StatusDate != entityPM.StatusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = entityPM.StatusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }

                if (entityMasterData.MainCarriageATA == null && entityPM.MainCarriageATA != null && entityPM.Transshipment1FromPortId == null && entityPM.Transshipment2FromPortId == null && entityPM.Transshipment3FromPortId == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("ARR", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = entityPM.Tenant, UserId = loggedContactId };
                        if (traceEvent != null)
                        {
                            traceEvent.EventDateTime = entityPM.MainCarriageATA != null ? entityPM.MainCarriageATA.Value : TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            DateTime? statusDate = entityPoco.StatusDate;
                            entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", entityPM.Tenant, loggedContactId, entityPM.Id, null, "Shipment", entityPoco.StatusId, null, false, ref statusDate, entityPM.CustomerId);
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = statusDate;

                            if (entityPoco.StatusDate != statusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = statusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }

                if (entityMasterData.MainCarriageATA != null && entityPM.MainCarriageATA == null && entityPM.Transshipment1FromPortId == null && entityPM.Transshipment2FromPortId == null && entityPM.Transshipment3FromPortId == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("ARR", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
                        if (traceEvent != null)
                        {
                            DeleteShipmentTraceEvent(entityPM, traceEvent.Id, entityPM.Tenant, false);
                            entityPoco.StatusId = entityPM.StatusId;
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = entityPM.StatusDate;

                            if (entityPoco.StatusDate != entityPM.StatusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = entityPM.StatusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }

                if (entityMasterData.Transshipment1ATA == null && entityPM.Transshipment1ATA != null && entityPM.Transshipment2FromPortId == null && entityPM.Transshipment3FromPortId == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("T1AR", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = entityPM.Tenant, UserId = loggedContactId };
                        if (traceEvent != null)
                        {
                            traceEvent.EventDateTime = entityPM.Transshipment1ATA != null ? entityPM.Transshipment1ATA.Value : TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            DateTime? statusDate = entityPoco.StatusDate;
                            entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", entityPM.Tenant, loggedContactId, entityPM.Id, null, "Shipment", entityPoco.StatusId, null, false, ref statusDate, entityPM.CustomerId);
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = statusDate;


                            if (entityPoco.StatusDate != statusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = statusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }

                if (entityMasterData.Transshipment1ATA != null && entityPM.Transshipment1ATA == null && entityPM.Transshipment2FromPortId == null && entityPM.Transshipment3FromPortId == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("T1AR", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
                        if (traceEvent != null)
                        {
                            DeleteShipmentTraceEvent(entityPM, traceEvent.Id, entityPM.Tenant, false);
                            entityPoco.StatusId = entityPM.StatusId;
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = entityPM.StatusDate;

                            if (entityPoco.StatusDate != entityPM.StatusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = entityPM.StatusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }

                if (entityMasterData.Transshipment1ATD != null && entityPM.Transshipment1ATD == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("T1DP", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
                        if (traceEvent != null)
                        {
                            DeleteShipmentTraceEvent(entityPM, traceEvent.Id, entityPM.Tenant, false);
                            entityPoco.StatusId = entityPM.StatusId;
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = entityPM.StatusDate;

                            if (entityPoco.StatusDate != entityPM.StatusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = entityPM.StatusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }

                if (entityMasterData.Transshipment1ATD == null && entityPM.Transshipment1ATD != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("T1DP", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = entityPM.Tenant, UserId = loggedContactId };
                        if (traceEvent != null)
                        {
                            traceEvent.EventDateTime = entityPM.Transshipment1ATD != null ? entityPM.Transshipment1ATD.Value : TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            DateTime? statusDate = entityPoco.StatusDate;
                            entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", entityPM.Tenant, loggedContactId, entityPM.Id, null, "Shipment", entityPoco.StatusId, null, false, ref statusDate, entityPM.CustomerId);
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = statusDate;


                            if (entityPoco.StatusDate != statusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = statusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }

                if (entityMasterData.Transshipment2ATA == null && entityPM.Transshipment2ATA != null && entityPM.Transshipment3FromPortId == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("T2AR", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = entityPM.Tenant, UserId = loggedContactId };
                        if (traceEvent != null)
                        {
                            DateTime? statusDate = entityPoco.StatusDate;
                            traceEvent.EventDateTime = entityPM.Transshipment2ATA != null ? entityPM.Transshipment2ATA.Value : TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", entityPM.Tenant, loggedContactId, entityPM.Id, null, "Shipment", entityPoco.StatusId, null, false, ref statusDate, entityPM.CustomerId);
                            entityMasterData.StatusId = entityPoco.StatusId;

                            if (entityPoco.StatusDate != statusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = statusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }

                if (entityMasterData.Transshipment2ATD == null && entityPM.Transshipment2ATD != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("T2DP", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = entityPM.Tenant, UserId = loggedContactId };
                        if (traceEvent != null)
                        {
                            traceEvent.EventDateTime = entityPM.Transshipment2ATD != null ? entityPM.Transshipment2ATD.Value : TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            DateTime? statusDate = entityPoco.StatusDate;
                            entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", entityPM.Tenant, loggedContactId, entityPM.Id, null, "Shipment", entityPoco.StatusId, null, false, ref statusDate, entityPM.CustomerId);
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = statusDate;

                            if (entityPoco.StatusDate != statusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = statusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }


                if (entityMasterData.Transshipment2ATA != null && entityPM.Transshipment2ATA == null && entityPM.Transshipment3FromPortId == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("T2AR", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
                        if (traceEvent != null)
                        {
                            DeleteShipmentTraceEvent(entityPM, traceEvent.Id, entityPM.Tenant, false);
                            entityPoco.StatusId = entityPM.StatusId;
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = entityPM.StatusDate;

                            if (entityPoco.StatusDate != entityPM.StatusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = entityPM.StatusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;

                        }
                    }
                }

                if (entityMasterData.Transshipment2ATD != null && entityPM.Transshipment2ATD == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("T2DP", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
                        if (traceEvent != null)
                        {
                            DeleteShipmentTraceEvent(entityPM, traceEvent.Id, entityPM.Tenant, false);
                            entityPoco.StatusId = entityPM.StatusId;
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = entityPM.StatusDate;

                            if (entityPoco.StatusDate != entityPM.StatusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = entityPM.StatusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }

                if (entityMasterData.Transshipment3ATA == null && entityPM.Transshipment3ATA != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("T3AR", tenant);
                    if (eventType != null)
                    {
                        DateTime? statusDate = entityPoco.StatusDate;
                        TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = entityPM.Tenant, UserId = loggedContactId };
                        if (traceEvent != null)
                        {
                            traceEvent.EventDateTime = entityPM.Transshipment3ATA != null ? entityPM.Transshipment3ATA.Value : TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", entityPM.Tenant, loggedContactId, entityPM.Id, null, "Shipment", entityPoco.StatusId, null, false, ref statusDate, entityPM.CustomerId);
                            entityMasterData.StatusId = entityPoco.StatusId;
                            if (entityPoco.StatusDate != statusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = statusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;

                        }
                    }
                }

                if (entityMasterData.Transshipment3ATD == null && entityPM.Transshipment3ATD != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("T3DP", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = entityPM.Tenant, UserId = loggedContactId };
                        if (traceEvent != null)
                        {
                            traceEvent.EventDateTime = entityPM.Transshipment3ATD != null ? entityPM.Transshipment3ATD.Value : TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            DateTime? statusDate = entityPoco.StatusDate;
                            entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", entityPM.Tenant, loggedContactId, entityPM.Id, null, "Shipment", entityPoco.StatusId, null, false, ref statusDate, entityPM.CustomerId);
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = statusDate;

                            if (entityPoco.StatusDate != statusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = statusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }

                if (entityMasterData.Transshipment3ATA != null && entityPM.Transshipment3ATA == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("T3AR", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
                        if (traceEvent != null)
                        {
                            DeleteShipmentTraceEvent(entityPM, traceEvent.Id, entityPM.Tenant, false);
                            entityPoco.StatusId = entityPM.StatusId;
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = entityPM.StatusDate;
                            if (entityPoco.StatusDate != entityPM.StatusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = entityPM.StatusDate;

                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }

                if (entityMasterData.Transshipment3ATD != null && entityPM.Transshipment3ATD == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("T3DP", tenant);
                    if (eventType != null)
                    {
                        TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
                        if (traceEvent != null)
                        {
                            DeleteShipmentTraceEvent(entityPM, traceEvent.Id, entityPM.Tenant, false);
                            entityPoco.StatusId = entityPM.StatusId;
                            entityMasterData.StatusId = entityPoco.StatusId;
                            entityMasterData.StatusDate = entityPM.StatusDate;
                            if (entityPoco.StatusDate != entityPM.StatusDate)
                            {
                                entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                            }
                            entityPoco.StatusDate = entityPM.StatusDate;
                            entityPM.StatusId = entityPoco.StatusId;
                            entityPM.StatusDate = entityPoco.StatusDate;
                        }
                    }
                }
            }
        }
    }
}