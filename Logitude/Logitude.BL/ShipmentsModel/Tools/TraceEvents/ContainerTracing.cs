using System;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.DataContracts;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.Tools.TraceEvents
{
    public class ContainerTracing
    {
        private string loggedContactId;
        private bool isNewEntity;
        ContainerPM containerPM;
        Container container;
        private int tenant;
        private string objectTableName = "Container";
        private string objectTableId;
        private List<EventType> allEventTypes;
        private List<EntityStatus> allEntityStatuses;
        private IWebFreightContext objectContext;
        private EventTypeRepository eventTypeRepository;
        private TraceEventRepository traceEventRepository;
        private EntityStatusRepository entityStatusRepository;
        private ObjectTableRepository objectTabelRepository;
        string eventUserId = null;
        string customerCareUserEmail = null;
        EventType eventType;
        TraceEvent previousEvent = null;
        public ContainerTracing(ContainerPM containerPM, Container container, bool isNewEntity)
        {
            this.containerPM = containerPM;
            this.container = container;
            this.isNewEntity = isNewEntity;
            this.tenant = this.containerPM.Tenant;
            this.InitalizeServices();
        }

        private void InitalizeServices()
        {
            this.objectContext = WebFreightContext.GetContext(tenant);
            this.objectTabelRepository = new ObjectTableRepository(objectContext);
            this.traceEventRepository = new TraceEventRepository(objectContext);
            this.eventTypeRepository = new EventTypeRepository(objectContext);
            this.entityStatusRepository = new EntityStatusRepository(objectContext);
            this.GetShipmentObjectTable();
            this.SetAllEvents();
            this.SetAllStatuses();
        }
        private void GetShipmentObjectTable()
        {
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            this.objectTableId = objectTable.Id;
        }
        private void SetAllEvents()
        {
            this.allEventTypes = eventTypeRepository.GetEventTypesByTenantAndObjectTableId(tenant, objectTableId).ToList();
        }
        private void SetAllStatuses()
        {
            this.allEntityStatuses = entityStatusRepository.GetEntityStatusByTenantAndObjectTableId(tenant, objectTableId).ToList();
        }

        private void CreateTraceEventWithNotes(string eventTypeCode, DateTime? eventDateTime, string notes)
        {
            this.CreateTraceEvent(eventTypeCode, eventDateTime, notes);
        }
        private void CreateTraceEventWithOutNotes(string eventTypeCode, DateTime? eventDateTime)
        {
            this.CreateTraceEvent(eventTypeCode, eventDateTime, "");
        }

        private void CreateTraceEvent(string eventTypeCode, DateTime? eventDateTime ,string notes)
        {
            this.CreateTraceEvent(new EventStatusTracerArgs()
            {
                Tenant = tenant,
                UserId = loggedContactId,
                EntityId = containerPM.Id,
                ObjectTableName = objectTableName,
                OldStatusId = container.StatusId,
                Notes = notes,
                EventTypeCode = eventTypeCode,
                EventDateTime = eventDateTime,
            });
        }

        public void CreateTraceEvent(EventStatusTracerArgs args)
        {
            this.HandleEventDates(args);
            this.ValidateEvent(args);
            this.GetEventUser(args);
            this.HandleEventStatus(args);
            this.AddNewTraceEvent(args);
            this.UpdateEventCustomFieldValue(args);
        }
        private void HandleEventDates(EventStatusTracerArgs args)
        {
            if (args.LogDateTime == null)
            {
                args.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            }
            else if (args.LogDateTime.Value.Year == 1)
            {
                args.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            }

            if (args.EventDateTime == null)
            {
                args.EventDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            }
            else if (args.EventDateTime.Value.Year == 1)
            {
                args.EventDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            }
        }
        private void ValidateEvent(EventStatusTracerArgs args)
        {
            if (string.IsNullOrEmpty(args.EventTypeCode))
            {
                return;
            }
            eventType = allEventTypes.Where(d => d.Code == args.EventTypeCode).FirstOrDefault();
            if (eventType == null)
            {
                throw new Exception("Event Type is not recognized:" + args.EventTypeCode);
            }
        }
        private void GetEventUser(EventStatusTracerArgs args)
        {
            if (eventUserId != null)
            {
                return;
            }
            eventUserId = args.UserId;
            this.GetCustomerCareUser();
        }

        private void GetCustomerCareUser()
        {
            if (tenant == 0)
            {
                return;
            }
            UserRepository userRepository = new UserRepository(0);
            User user = userRepository.GetSingleUser(eventUserId, 0, true);
            if (user == null)
            {
                return;
            }
            User systemUser = userRepository.GetSingleUserByEmail("system@tenant" + tenant + ".com", tenant, true);
            if (systemUser != null)
            {
                eventUserId = systemUser.Id;
            }
            customerCareUserEmail = user.Contact.Email;
        }
        private void HandleEventStatus(EventStatusTracerArgs args)
        {
            ComputeEventStatus(args);
        }

        private void ComputeEventStatus(EventStatusTracerArgs args)
        {
            if (args.IsAddedManually)
            {
                return;
            }
            EventType newEventType = allEventTypes.Where(d => d.Code == args.EventTypeCode).FirstOrDefault();
            if (newEventType.EntityStatusId == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(args.OldStatusId))
            {
                containerPM.StatusId = newEventType.EntityStatusId;
                container.StatusId = containerPM.StatusId;
            }
            else
            {
                this.HabdleNewStatussWeight(newEventType, args);
            }
        }
        private void HabdleNewStatussWeight(EventType newEventType, EventStatusTracerArgs args)
        {
            EntityStatus newEntityStatus = allEntityStatuses.Where(d => d.Id == newEventType.EntityStatusId).FirstOrDefault();
            EntityStatus oldEntityStatus = allEntityStatuses.Where(d => d.Id == args.OldStatusId).FirstOrDefault();

            if (newEntityStatus.StatusWeight >= oldEntityStatus.StatusWeight)
            {
                containerPM.StatusId = newEventType.EntityStatusId;
                container.StatusId = containerPM.StatusId;
            }
        }

        private void AddNewTraceEvent(EventStatusTracerArgs args)
        {
            TraceEvent myTraceEvent = new TraceEvent()
            {
                Id = Guid.NewGuid().ToString(),
                Tenant = tenant,
                EntityId = args.EntityId,
                EventTypeId = eventType.Id,
                ObjectTableId = this.objectTableId,
                LogDateTime = args.LogDateTime.Value,
                EventDateTime = args.EventDateTime.Value,
                ExternalId = args.ExternalId,
                IsAddedManually = args.IsAddedManually,
                UserId = eventUserId,
                CustomerCareUserEmail = customerCareUserEmail,
                Notes = args.Notes,
                Location = args.StatusLocation,
            };
            this.traceEventRepository.Add(myTraceEvent);
            this.traceEventRepository.SubmitChanges();
            objectContext.SaveChanges();
        }

        private void UpdateEventCustomFieldValue(EventStatusTracerArgs args)
        {
            if (!string.IsNullOrEmpty(eventType.CustomField))
            {
                EventCustomFieldUpdateService.UpdateEventCustomFieldValue(new UpdateEventCustomFieldArgs() { CustomField = eventType.CustomField, EventDateTime = args.EventDateTime.Value, Entity = containerPM, EntityId = args.EntityId, ObjectTableName = args.ObjectTableName, Tenant = args.Tenant });
            }
        }

        private void DeleteTraceEvent(string eventTypeCode)
        {
            if (string.IsNullOrEmpty(eventTypeCode))
            {
                return;
            }

            EventType deletedEventType = allEventTypes.Where(d => d.Code == eventTypeCode).FirstOrDefault();

            if (deletedEventType == null)
            {
                return;
            }

            this.UpdateAllTraceEventsToDelete(deletedEventType);
            this.GetPreviousEventAfterDeletion(deletedEventType);
            this.SetContainerStatusesFields();
        }

        private void UpdateAllTraceEventsToDelete(EventType deletedEventType)
        {
            List<TraceEvent> AllEventTraces = this.traceEventRepository.GetAllTraceEventsByEventType(containerPM.Id, deletedEventType.Id, tenant).ToList();
            if (AllEventTraces == null || (AllEventTraces?.Count == 0))
            {
                return;
            }
            foreach (TraceEvent iTraceEvent in AllEventTraces)
            {
                iTraceEvent.Deleted = true;
                traceEventRepository.Update(iTraceEvent);
            }

            traceEventRepository.SubmitChanges();
        }
        private void GetPreviousEventAfterDeletion(EventType deletedEventType)
        {
            if (string.IsNullOrEmpty(deletedEventType.EntityStatusId))
            {
                return;
            }
            List<TraceEvent> traceEvents = this.GetTraceEventList();
            this.previousEvent = null;
            foreach (TraceEvent traceEvent in traceEvents)
            {
                this.HandlePreviousEvent(traceEvent);
            }
        }
        private List<TraceEvent> GetTraceEventList()
        {
            var traceEvents = (from a in objectContext.TraceEvent.Include("EventType").Include("EventType.EntityStatus")
                               where a.Tenant == tenant
                               && a.EntityId == containerPM.Id
                               && a.ObjectTableId == objectTableId
                               && a.EventType.EntityStatus != null
                               && a.Deleted == false
                               select a).ToList();

            return traceEvents;
        }

        private void HandlePreviousEvent(TraceEvent traceEvent)
        {

            if (previousEvent == null)
            {
                previousEvent = traceEvent;
            }
            else
            {
                if (traceEvent.EventType.EntityStatus.StatusWeight > previousEvent.EventType.EntityStatus.StatusWeight)
                {
                    previousEvent = traceEvent;
                }
            }
        }

        private void SetContainerStatusesFields()
        {
            EventType firstEventType = allEventTypes.Where(d => d.Code == "COOR").FirstOrDefault();
            containerPM.StatusId = firstEventType.EntityStatusId;
            if (previousEvent != null)
            {
                containerPM.StatusId = previousEvent.EventType.EntityStatusId;
            }
            container.StatusId = containerPM.StatusId;
        }

        public void Trace()
        {
            GetLoggedUser();
            TraceCreatedEvent();
            TraceUpdatedEvent();
            TraceClosedEvent();
            TraceEmptyPickUp();
            TracDepartedFromShipper();
            TracTransshipment1Arrived();
            TracTransshipment1Departed();
            TracTransshipment2Arrived();
            TracTransshipment2Departed();
            TracTransshipment3Arrived();
            TracTransshipment3Departed();
            TracDischarged();
            TracOnCarriageDischarged();
            TracEmptyReturned();
            TracGatedOut();
            TracOnCarriageArrived();
            TracOnCarriageDeparted();
            TracArrivedPOD();
            TracDepartedPOD();
            TracPreCarriageArrived();
            TracPreCarriageDeparted();
            TraceCancelledEvent();
            TraceExceptionResolved();
        }
        private void GetLoggedUser()
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "system@tenant" + tenant + ".com";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }
            Contact contact = contactRep.GetSingleContactByEmail(email, tenant);
            if (contact != null)
            {
                this.loggedContactId = contact.Id;
            }
        }
        private void TraceCreatedEvent()
        {
            if (isNewEntity)
            {
                this.TraceNewEvent();
                this.TraceOrderEvent();
            }
        }
        private void TraceNewEvent()
        {
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = containerPM.Tenant,
                EventTypeCode = "CREV",
                UserId = containerPM.CreatedByUserId,
                EntityId = containerPM.Id,
                ObjectTableName = "Container",
            });
        }
        private void TraceOrderEvent()
        {
            this.CreateTraceEventWithOutNotes("COOR", containerPM.CreateDate);
        }
        private void TraceUpdatedEvent()
        {
            if (!isNewEntity)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = containerPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = containerPM.UpdatedByUserId,
                    EntityId = containerPM.Id,
                    ObjectTableName = "Container",
                });
            }
        }
        private void TraceClosedEvent()
        {
            if (container.IsClosed && !containerPM.IsClosed)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = containerPM.Tenant,
                    EventTypeCode = "ROCO",
                    UserId = containerPM.UpdatedByUserId,
                    EntityId = containerPM.Id,
                    ObjectTableName = "Container",
                });
            }

            if (!container.IsClosed && containerPM.IsClosed)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = containerPM.Tenant,
                    EventTypeCode = "CODC",
                    UserId = containerPM.UpdatedByUserId,
                    EntityId = containerPM.Id,
                    ObjectTableName = "Container",
                });
            }
        }
        private void TraceEmptyPickUp()
        {
            if (containerPM.ActualEmptyPickupDate != null && container.ActualEmptyPickupDate == null)
            {
                this.CreateTraceEventWithOutNotes("EMPS", containerPM.ActualEmptyPickupDate);
            }

            else if (containerPM.ActualEmptyPickupDate == null && container.ActualEmptyPickupDate != null)
            {
                this.DeleteTraceEvent("EMPS");
            }
        }
        private void TracDepartedFromShipper()
        {
            if (containerPM.ShipmentPickupATD != null && container.ShipmentPickupATD == null)
            {
                this.CreateTraceEventWithOutNotes("PICS", containerPM.ShipmentPickupATD);
            }

            else if (containerPM.ShipmentPickupATD == null && container.ShipmentPickupATD != null)
            {
                this.DeleteTraceEvent("PICS");
            }
        }
        private void TracTransshipment1Arrived()
        {
            if (containerPM.ActualTransshipment1VesselArrival != null && container.ActualTransshipment1VesselArrival == null)
            {
                this.CreateTraceEventWithOutNotes("T1AV", containerPM.ActualTransshipment1VesselArrival);
            }

            else if (containerPM.ActualTransshipment1VesselArrival == null && container.ActualTransshipment1VesselArrival != null)
            {
                this.DeleteTraceEvent("T1AV");
            }
        }
        private void TracTransshipment1Departed()
        {
            if (containerPM.ActualTrans1VesselDeparture != null && container.ActualTrans1VesselDeparture == null)
            {
                this.CreateTraceEventWithOutNotes("T1DT", containerPM.ActualTrans1VesselDeparture);
            }

            else if (containerPM.ActualTrans1VesselDeparture == null && container.ActualTrans1VesselDeparture != null)
            {
                this.DeleteTraceEvent("T1DT");
            }
        }
        private void TracTransshipment2Arrived()
        {
            if (containerPM.ActualTransshipment2VesselArrival != null && container.ActualTransshipment2VesselArrival == null)
            {
                this.CreateTraceEventWithOutNotes("T2AV", containerPM.ActualTransshipment2VesselArrival);
            }

            else if (containerPM.ActualTransshipment2VesselArrival == null && container.ActualTransshipment2VesselArrival != null)
            {
                this.DeleteTraceEvent("T2AV");
            }
        }
        private void TracTransshipment2Departed()
        {
            if (containerPM.ActualTrans2VesselDeparture != null && container.ActualTrans2VesselDeparture == null)
            {
                this.CreateTraceEventWithOutNotes("T2DT", containerPM.ActualTrans2VesselDeparture);
            }

            else if (containerPM.ActualTrans2VesselDeparture == null && container.ActualTrans2VesselDeparture != null)
            {
                this.DeleteTraceEvent("T2DT");
            }
        }
        private void TracTransshipment3Arrived()
        {
            if (containerPM.ActualTransshipment3VesselArrival != null && container.ActualTransshipment3VesselArrival == null)
            {
                this.CreateTraceEventWithOutNotes("T3AV", containerPM.ActualTransshipment3VesselArrival);
            }

            else if (containerPM.ActualTransshipment3VesselArrival == null && container.ActualTransshipment3VesselArrival != null)
            {
                this.DeleteTraceEvent("T3AV");
            }
        }
        private void TracTransshipment3Departed()
        {
            if (containerPM.ActualTrans3VesselDeparture != null && container.ActualTrans3VesselDeparture == null)
            {
                this.CreateTraceEventWithOutNotes("T3DT", containerPM.ActualTrans3VesselDeparture);
            }

            else if (containerPM.ActualTrans3VesselDeparture == null && container.ActualTrans3VesselDeparture != null)
            {
                this.DeleteTraceEvent("T3DT");
            }
        }
        private void TracDischarged()
        {
            if (containerPM.ActualPODDischarge != null && container.ActualPODDischarge == null)
            {
                this.CreateTraceEventWithOutNotes("DSCH", containerPM.ActualPODDischarge);
            }

            else if (containerPM.ActualPODDischarge == null && container.ActualPODDischarge != null)
            {
                this.DeleteTraceEvent("DSCH");
            }
        }
        private void TracOnCarriageDischarged()
        {
            if (containerPM.ActualOnCarriageDeparture != null && container.ActualOnCarriageDeparture == null)
            {
                this.CreateTraceEventWithOutNotes("UNDS", containerPM.ActualOnCarriageDeparture);
            }

            else if (containerPM.ActualOnCarriageDeparture == null && container.ActualOnCarriageDeparture != null)
            {
                this.DeleteTraceEvent("UNDS");
            }
        }
        private void TracEmptyReturned()
        {
            if (containerPM.ActualEmptyReturn != null && container.ActualEmptyReturn == null)
            {
                this.CreateTraceEventWithOutNotes("EMRT", containerPM.ActualEmptyReturn);
            }
            if (containerPM.ActualEmptyReturn == null && container.ActualEmptyReturn != null)
            {
                this.DeleteTraceEvent("EMRT");
            }
        }
        private void TracGatedOut()
        {
            if ((containerPM.ActualOnCarriageDeparture != null && container.ActualOnCarriageDeparture == null) && containerPM.ActualPODDeparture == null)
            {
                this.DeleteTraceEvent("GTOT");
                this.CreateTraceEventWithOutNotes("GTOT", containerPM.ActualOnCarriageDeparture);
            }
            else if ((containerPM.ActualOnCarriageDeparture == null && container.ActualOnCarriageDeparture != null) && containerPM.ActualPODDeparture == null)
            {
                this.DeleteTraceEvent("GTOT");
            }
            else if ((containerPM.ActualPODDeparture != null && container.ActualPODDeparture == null) && containerPM.ActualOnCarriageDeparture == null)
            {
                this.DeleteTraceEvent("GTOT");
                this.CreateTraceEventWithOutNotes("GTOT", containerPM.ActualPODDeparture);
            }
            else if ((containerPM.ActualPODDeparture == null && container.ActualPODDeparture != null) && containerPM.ActualOnCarriageDeparture == null)
            {
                this.DeleteTraceEvent("GTOT");
            }
        }

        private void TracOnCarriageArrived()
        {
            if ((containerPM.ActualLIFArrival != null && container.ActualLIFArrival == null) && containerPM.ShipmentOnCarriageATA == null)
            {
                this.DeleteTraceEvent("DPWH");
                this.CreateTraceEventWithOutNotes("DPWH", containerPM.ActualLIFArrival);
            }
            else if ((containerPM.ActualLIFArrival == null && container.ActualLIFArrival != null) && containerPM.ShipmentOnCarriageATA == null)
            {
                this.DeleteTraceEvent("DPWH");
            }
            else if ((containerPM.ShipmentOnCarriageATA != null && container.ShipmentOnCarriageATA == null) && containerPM.ActualLIFArrival == null)
            {
                this.DeleteTraceEvent("DPWH");
                this.CreateTraceEventWithOutNotes("DPWH", containerPM.ShipmentOnCarriageATA);
            }
            else if ((containerPM.ShipmentOnCarriageATA == null && container.ShipmentOnCarriageATA != null) && containerPM.ActualLIFArrival == null)
            {
                this.DeleteTraceEvent("DPWH");
            }
        }

        private void TracOnCarriageDeparted()
        {
            if ((containerPM.ActualOnCarriageDeparture != null && container.ActualOnCarriageDeparture == null) && containerPM.ShipmentOnCarriageATD == null)
            {
                this.DeleteTraceEvent("ARWH");
                this.CreateTraceEventWithOutNotes("ARWH", containerPM.ActualOnCarriageDeparture);
            }
            else if ((containerPM.ActualOnCarriageDeparture == null && container.ActualOnCarriageDeparture != null) && containerPM.ShipmentOnCarriageATD == null)
            {
                this.DeleteTraceEvent("ARWH");
            }
            else if ((containerPM.ShipmentOnCarriageATD != null && container.ShipmentOnCarriageATD == null) && containerPM.ActualOnCarriageDeparture == null)
            {
                this.DeleteTraceEvent("ARWH");
                this.CreateTraceEventWithOutNotes("ARWH", containerPM.ShipmentOnCarriageATD);
            }
            else if ((containerPM.ShipmentOnCarriageATD == null && container.ShipmentOnCarriageATD != null) && containerPM.ActualOnCarriageDeparture == null)
            {
                this.DeleteTraceEvent("ARWH");
            }
        }

        private void TracArrivedPOD()
        {
            if ((containerPM.ActualPODVesselArrival != null && container.ActualPODVesselArrival == null) && !CheckShipmentLastArrivalDate())
            {
                this.DeleteTraceEvent("ARPD");
                this.CreateTraceEventWithOutNotes("ARPD", containerPM.ActualPODVesselArrival);
            }
            else if ((containerPM.ActualPODVesselArrival == null && container.ActualPODVesselArrival != null) && !CheckShipmentLastArrivalDate())
            {
                this.DeleteTraceEvent("ARPD");
            }
            else if (IsShipmentLastArrivalDateFilled() && containerPM.ActualPODVesselArrival == null)
            {
                this.DeleteTraceEvent("ARPD");
                this.CreateTraceEventWithOutNotes("ARPD", containerPM.ShipmentMainCarriageATA);
            }
            else if (IsShipmentLastArrivalDateEvenRemoved() && containerPM.ActualPODVesselArrival == null)
            {
                this.DeleteTraceEvent("ARPD");
            }
        }

        private bool CheckShipmentLastArrivalDate()
        {
            if (!string.IsNullOrEmpty(containerPM.ShipmentTransshipment3ToId))
            {
                return containerPM.ShipmentTransshipment3ATA != null ? true : false;
            }

            if (!string.IsNullOrEmpty(containerPM.ShipmentTransshipment2ToId))
            {
                return containerPM.ShipmentTransshipment2ATA != null ? true : false;
            }

            if (!string.IsNullOrEmpty(containerPM.ShipmentTransshipment1ToId))
            {
                return containerPM.ShipmentTransshipment1ATA != null ? true : false;
            }

            if (!string.IsNullOrEmpty(containerPM.ShipmentMainCarriageToId))
            {
                return containerPM.ShipmentMainCarriageToId != null ? true : false;
            }

            return false;
        }
        private bool IsShipmentLastArrivalDateFilled()
        {
            if (!string.IsNullOrEmpty(containerPM.ShipmentTransshipment3ToId))
            {
                return containerPM.ShipmentTransshipment3ATA != null && container.ShipmentTransshipment3ATA == null ? true : false;
            }

            if (!string.IsNullOrEmpty(containerPM.ShipmentTransshipment2ToId))
            {
                return containerPM.ShipmentTransshipment2ATA != null && container.ShipmentTransshipment2ATA == null ? true : false;
            }

            if (!string.IsNullOrEmpty(containerPM.ShipmentTransshipment1ToId))
            {
                return containerPM.ShipmentTransshipment1ATA != null && container.ShipmentTransshipment1ATA == null ? true : false;
            }

            if (!string.IsNullOrEmpty(containerPM.ShipmentMainCarriageToId))
            {
                return containerPM.ShipmentMainCarriageATA != null && container.ShipmentMainCarriageATA == null ? true : false;
            }

            return false;
        }
        private bool IsShipmentLastArrivalDateEvenRemoved()
        {
            if (!string.IsNullOrEmpty(containerPM.ShipmentTransshipment3ToId))
            {
                return (containerPM.ShipmentTransshipment3ATA == null && container.ShipmentTransshipment3ATA != null)
                    || (containerPM.ShipmentTransshipment3ATA == null && container.ShipmentTransshipment3ATA == null) ? true : false;
            }

            if (!string.IsNullOrEmpty(containerPM.ShipmentTransshipment2ToId))
            {
                return (containerPM.ShipmentTransshipment2ATA == null && container.ShipmentTransshipment2ATA != null)
                    || (containerPM.ShipmentTransshipment2ATA == null && container.ShipmentTransshipment2ATA == null) ? true : false;
            }

            if (!string.IsNullOrEmpty(containerPM.ShipmentTransshipment1ToId))
            {
                return (containerPM.ShipmentTransshipment1ATA == null && container.ShipmentTransshipment1ATA != null) 
                    || (containerPM.ShipmentTransshipment1ATA == null && container.ShipmentTransshipment1ATA == null) ? true : false;
            }

            if (!string.IsNullOrEmpty(containerPM.ShipmentMainCarriageToId))
            {
                return (containerPM.ShipmentMainCarriageATA == null && container.ShipmentMainCarriageATA != null)
                    || (containerPM.ShipmentMainCarriageATA == null && container.ShipmentMainCarriageATA == null) ? true : false;
            }

            return false;
        }
        private void TracDepartedPOD()
        {
            if ((containerPM.ActualPOLVesselDeparture != null && container.ActualPOLVesselDeparture == null) && containerPM.ShipmentMainCarriageATD == null)
            {
                this.DeleteTraceEvent("POLD");
                this.CreateTraceEventWithOutNotes("POLD", containerPM.ActualPOLVesselDeparture);
            }
            else if ((containerPM.ActualPOLVesselDeparture == null && container.ActualPOLVesselDeparture != null) && containerPM.ShipmentMainCarriageATD == null)
            {
                this.DeleteTraceEvent("POLD");
            }
            else if ((containerPM.ShipmentMainCarriageATD != null && container.ShipmentMainCarriageATD == null) && containerPM.ActualPOLVesselDeparture == null)
            {
                this.DeleteTraceEvent("POLD");
                this.CreateTraceEventWithOutNotes("POLD", containerPM.ShipmentMainCarriageATD);
            }
            else if ((containerPM.ShipmentMainCarriageATD == null && container.ShipmentMainCarriageATD != null) && containerPM.ActualPOLVesselDeparture == null)
            {
                this.DeleteTraceEvent("POLD");
            }
        }

        private void TracPreCarriageArrived()
        {
            if ((containerPM.ActualPOLArrival != null && container.ActualPOLArrival == null) && containerPM.ShipmentPreCarriageATA == null)
            {
                this.DeleteTraceEvent("PCAV");
                this.CreateTraceEventWithOutNotes("PCAV", containerPM.ActualPOLArrival);
            }
            else if ((containerPM.ActualPOLArrival == null && container.ActualPOLArrival != null) && containerPM.ShipmentPreCarriageATA == null)
            {
                this.DeleteTraceEvent("PCAV");
            }
            else if ((containerPM.ShipmentPreCarriageATA != null && container.ShipmentPreCarriageATA == null) && containerPM.ActualPOLArrival == null)
            {
                this.DeleteTraceEvent("PCAV");
                this.CreateTraceEventWithOutNotes("PCAV", containerPM.ShipmentPreCarriageATA);
            }
            else if ((containerPM.ShipmentPreCarriageATA == null && container.ShipmentPreCarriageATA != null) && containerPM.ActualPOLArrival == null)
            {
                this.DeleteTraceEvent("PCAV");
            }
        }

        private void TracPreCarriageDeparted()
        {
            if ((containerPM.PreCarriageATD != null && container.PreCarriageATD == null) && containerPM.ShipmentPreCarriageATD == null)
            {
                this.DeleteTraceEvent("PCDP");
                this.CreateTraceEventWithOutNotes("PCDP", containerPM.PreCarriageATD);
            }
            else if ((containerPM.PreCarriageATD == null && container.PreCarriageATD != null) && containerPM.ShipmentPreCarriageATD == null)
            {
                this.DeleteTraceEvent("PCDP");
            }
            else if ((containerPM.ShipmentPreCarriageATD != null && container.ShipmentPreCarriageATD == null) && containerPM.PreCarriageATD == null)
            {
                this.DeleteTraceEvent("PCDP");
                this.CreateTraceEventWithOutNotes("PCDP", containerPM.ShipmentPreCarriageATD);
            }
            else if ((containerPM.ShipmentPreCarriageATD == null && container.ShipmentPreCarriageATD != null) && containerPM.PreCarriageATD == null)
            {
                this.DeleteTraceEvent("PCDP");
            }
        }

        private void TraceCancelledEvent()
        {
            if (container.IsCancelled && !containerPM.IsCancelled)
            {
                this.CreateTraceEventWithOutNotes("RACO", containerPM.UpdateDate);
            }
            if (!container.IsCancelled && containerPM.IsCancelled)
            {
                this.CreateTraceEventWithOutNotes("CCCO", containerPM.CancelledDate);
            }
        }

        private void TraceExceptionResolved()
        {
            if (container.HasException && containerPM.IsExceptionResolved && !containerPM.IsUpdateEntityException)
            {
                this.CreateTraceEventWithNotes("CRES", containerPM.ExceptionDate, containerPM.ExceptionResolvedDescription);
                containerPM.ExceptionDate = null;
                containerPM.ExceptionDescription = "";
            }
        }
        public void DeleteContainerExceptionTraceEvent(string traceEventId, ContainerRepository containerRepository, int tenant)
        {
            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            TraceEvent traceEvent = traceEventRep.GetSingleTraceEvent(traceEventId);
            if (traceEvent == null)
            {
                return;
            }
            traceEvent.Deleted = true;
            traceEventRep.Update(traceEvent);
            traceEventRep.SubmitChanges();
            var exceptionEventCode = "CEXC";
            var exceptionResolvedEventCode = "CRES";
            if (traceEvent.EventType == null || (traceEvent.EventType.Code != exceptionEventCode && traceEvent.EventType.Code != exceptionResolvedEventCode))
            {
                return;
            }

            TraceEvent lastExceptiontraceEvent = traceEventRep.GetLastExceptionTraceEventByContainerId(containerPM.Id, traceEvent.EventType.Code, containerPM.Tenant);
            if (lastExceptiontraceEvent == null || lastExceptiontraceEvent.Id != traceEvent.Id)
            {
                return;
            }

            if (traceEvent.EventType.Code == exceptionEventCode)
            {
                containerPM.ExceptionDescription = null;
                containerPM.LastExceptionDescription = null;
                containerPM.ExceptionDate = null;
                containerPM.HasException = false;
            }

            if (traceEvent.EventType.Code == exceptionResolvedEventCode)
            {
                containerPM.ExceptionResolvedDescription = null;
                containerPM.IsExceptionResolved = false;
                containerPM.HasException = this.IsContainerHaveExceptionTraceEvent(traceEventRep);
            }

            container.ExceptionDescription = containerPM.ExceptionDescription;
            container.ExceptionDate = containerPM.ExceptionDate;
            container.HasException = containerPM.HasException;
            container.LastExceptionDescription = containerPM.LastExceptionDescription;
            container.ExceptionResolvedDescription = containerPM.ExceptionResolvedDescription;
            container.IsExceptionResolved = containerPM.IsExceptionResolved;


            containerRepository.Update(container);
            containerRepository.SubmitChanges();
        }

        private bool IsContainerHaveExceptionTraceEvent(TraceEventRepository traceEventRep)
        {
            bool  isTraceEventExist = traceEventRep.IsTraceEventExistByEntityIdAndEventCode(containerPM.Id, "CEXC", tenant);
            return isTraceEventExist;
        }
    }
}
