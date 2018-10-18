using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

namespace Logitude.BL.ShipmentsModel.Tools.TraceEvents
{
    public partial class ShipmentTracing
    {
        public static void TracePickUpDelivery_00(ShipmentPickUpDeliveryPM itemPM, ShipmentPickUpDelivery itemPoco, ShipmentPM entityPM, Shipment entityPoco, ShipmentMasterData entityMasterData, string loggedContactId)
        {
            int tenant = entityPM.Tenant;
            EventTypeQuery eventTypeQuery = new EventTypeQuery(tenant);
            string objectTableName = "Shipment";

            ObjectTabelQuery ObjectTabelQuery = new ObjectTabelQuery(tenant);
            ObjectTablePM objectTable = ObjectTabelQuery.GetObjectTableByName("Shipment", tenant);

            TraceEventRepository traceEventRepository = new TraceEventRepository(tenant);

            if (itemPM.PickUpDeliveryTypeCode == "PICK")
            {
                if (itemPoco.ATA == null && itemPM.ATA != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("RCS", itemPM.Tenant);
                    TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = tenant, UserId = loggedContactId, };
                    traceEvent.EventDateTime = itemPM.ATA != null ? itemPM.ATA.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                    DateTime? statusDate = entityPoco.StatusDate;
                    entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", tenant, loggedContactId, entityPM.Id, null, objectTableName, entityPoco.StatusId, null, false, ref statusDate);
                    if (entityPoco.StatusDate != statusDate)
                    {
                        entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    }
                    entityPoco.StatusDate = statusDate;
                    entityPoco.LastStatusLogDate = statusDate;
                    entityPM.StatusDate = statusDate;
                    entityPM.StatusId = entityPoco.StatusId;
                    if (entityPoco.ShipmentLevelCode == "D" || entityPoco.ShipmentLevelCode == "C")
                    {
                        entityMasterData.StatusDate = entityPoco.StatusDate;
                        entityMasterData.StatusId = entityPoco.StatusId;
                    }
                }

                if (itemPoco.ATA != null && itemPM.ATA == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("RCS", itemPM.Tenant);
                    TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
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

                if (itemPoco.ATD == null && itemPM.ATD != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("PICD", itemPM.Tenant);
                    TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = tenant, UserId = loggedContactId, };
                    traceEvent.EventDateTime = itemPM.ATD != null ? itemPM.ATD.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                    DateTime? statusDate = entityPoco.StatusDate;
                    entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", tenant, loggedContactId, entityPM.Id, null, objectTableName, entityPoco.StatusId, null, false, ref statusDate);
                    if (entityPoco.StatusDate != statusDate)
                    {
                        entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    }
                    entityPoco.StatusDate = statusDate;
                    entityPoco.LastStatusLogDate = statusDate;
                    entityPM.StatusDate = statusDate;
                    entityPM.StatusId = entityPoco.StatusId;
                    if (entityPoco.ShipmentLevelCode == "D" || entityPoco.ShipmentLevelCode == "C")
                    {
                        entityMasterData.StatusDate = entityPoco.StatusDate;
                        entityMasterData.StatusId = entityPoco.StatusId;
                    }
                }

                if (itemPoco.ATD != null && itemPM.ATD == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("PICD", itemPM.Tenant);
                    TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
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

            else
            {
                if (itemPoco.ATA == null && itemPM.ATA != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("PIOD", itemPM.Tenant);
                    TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = tenant, UserId = loggedContactId };
                    traceEvent.EventDateTime = itemPM.ATA != null ? itemPM.ATA.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                    DateTime? statusDate = entityPoco.StatusDate;
                    entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", tenant, loggedContactId, entityPM.Id, null, objectTableName, entityPoco.StatusId, null, false, ref statusDate);
                    entityPM.StatusId = entityPoco.StatusId;
                    if (entityPoco.StatusDate != statusDate)
                    {
                        entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    }
                    entityPoco.StatusDate = statusDate;
                    entityPoco.LastStatusLogDate = statusDate;
                    entityPM.StatusDate = statusDate;
                    entityPM.StatusId = entityPoco.StatusId;
                    if (entityPoco.ShipmentLevelCode == "D" || entityPoco.ShipmentLevelCode == "C")
                    {
                        entityMasterData.StatusDate = entityPoco.StatusDate;
                        entityMasterData.StatusId = entityPoco.StatusId;
                    }
                }

                if (itemPoco.ATA != null && itemPM.ATA == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("PIOD", itemPM.Tenant);
                    TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
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

                if (itemPoco.ATD == null && itemPM.ATD != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("DELD", itemPM.Tenant);
                    TraceEvent traceEvent = new TraceEvent() { Id = Guid.NewGuid().ToString(), ObjectTableId = objectTable.Id, LogDateTime = TenantServerConfigration.GetCurrentDateTime(itemPM.Tenant), EntityId = entityPM.Id, EventTypeId = eventType.Id, Notes = eventType.EnglishName, Tenant = tenant, UserId = loggedContactId };
                    traceEvent.EventDateTime = itemPM.ATD != null ? itemPM.ATD.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                    DateTime? statusDate = entityPoco.StatusDate;
                    entityPoco.StatusId = EventTracer.CreateTraceEvent(traceEvent, "USHI", tenant, loggedContactId, entityPM.Id, null, objectTableName, entityPoco.StatusId, null, false, ref statusDate);
                    entityPM.StatusId = entityPoco.StatusId;
                    if (entityPoco.StatusDate != statusDate)
                    {
                        entityPoco.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                    }
                    entityPoco.StatusDate = statusDate;
                    entityPoco.LastStatusLogDate = statusDate;
                    entityPM.StatusDate = statusDate;
                    entityPM.StatusId = entityPoco.StatusId;
                    if (entityPoco.ShipmentLevelCode == "D" || entityPoco.ShipmentLevelCode == "C")
                    {
                        entityMasterData.StatusDate = entityPoco.StatusDate;
                        entityMasterData.StatusId = entityPoco.StatusId;
                    }
                }

                if (itemPoco.ATD != null && itemPM.ATD == null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("DELD", itemPM.Tenant);
                    TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
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

        public static void TraceDeletedPickUpDelivery_00(ShipmentPickUpDelivery itemPoco, ShipmentPM entityPM, ShipmentMasterData entityMasterData, Shipment entityPoco)
        {
            int tenant = entityPM.Tenant;

            TraceEventRepository traceEventRepository = new TraceEventRepository(tenant);
            EventTypeQuery eventTypeQuery = new EventTypeQuery(tenant);

            if (itemPoco.PickUpDeliveryTypeCode == "PICK")
            {
                if (itemPoco.ATA != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("RCS", tenant);
                    TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
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

                if (itemPoco.ATD != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("PICD", tenant);
                    TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
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

            else
            {
                if (itemPoco.ATA != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("PIOD", tenant);
                    TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
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

                if (itemPoco.ATD != null)
                {
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("DELD", tenant);
                    TraceEvent traceEvent = traceEventRepository.GetSingleTraceEventByEntityId(entityPM.Id, eventType.Id, tenant);
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

        public static void DeleteShipmentTraceEvent(ShipmentPM shipmentPM, string traceEventId, int tenant, bool external)
        {
            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            EntityStatusRepository entityStatusRep = new EntityStatusRepository(tenant);
            TraceEvent traceEvent = traceEventRep.GetSingleTraceEvent(traceEventId);
            traceEvent.Deleted = true;
            traceEventRep.Update(traceEvent);
            traceEventRep.SubmitChanges();

            List<TraceEvent> traceEventList = traceEventRep.GetTraceEventsByTenantByEntityId(tenant, shipmentPM.Id).ToList();
            TraceEvent previousEvent = null;
            foreach (TraceEvent e in traceEventList)
            {
                if (!e.Deleted)
                {
                    if (previousEvent != null)
                    {
                        if (e.EventType.EntityStatus != null)
                        {
                            if (e.EventType.EntityStatus.StatusWeight > previousEvent.EventType.EntityStatus.StatusWeight)
                            {
                                previousEvent = e;
                            }
                        }
                    }
                    else
                    {
                        if (e.EventType.EntityStatus != null)
                        {
                            previousEvent = e;
                        }
                    }
                }
            }
           
            if (previousEvent != null)
            {
                shipmentPM.StatusId = previousEvent.EventType.EntityStatusId;
                shipmentPM.StatusDate = previousEvent.EventDateTime;

            }
            else
            {
                EntityStatus orderStatus = entityStatusRep.GetSingleEntityStatusByCode("SHOR", tenant);
                shipmentPM.StatusId = orderStatus.Id;
                shipmentPM.StatusDate = shipmentPM.CreateDateTime;

            }
            if (external)
            {
                IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
                Shipment shipment = shipmentRepository.GetSingleShipment(shipmentPM.Id, shipmentPM.Tenant);
                shipment.StatusId = shipmentPM.StatusId;//previousEvent.EventType.EntityStatusId;
                shipment.StatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                shipment.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                shipmentRepository.Update(shipment);
                shipmentRepository.SubmitChanges();

                if (shipment.ShipmentLevelCode != "H")
                {
                    ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(shipmentsContext);
                    ShipmentMasterData masterData = shipmentMasterDataRepository.GetSingleMasterData(shipment.Id);
                    if (masterData != null)
                    {
                        masterData.StatusId = shipment.StatusId;
                        masterData.StatusDate = shipment.StatusDate;

                        shipmentMasterDataRepository.Update(masterData);
                        shipmentMasterDataRepository.SubmitChanges();
                    }
                }
            }
        }
    }
}
