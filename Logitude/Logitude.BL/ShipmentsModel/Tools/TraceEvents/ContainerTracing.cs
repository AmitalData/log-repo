using System;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
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
        string myUserId = null;
        string myCustomerCareUserEmail = null;
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
            this.GetShipmentObjectTable();
            this.GetAllEvents();
        }
        private void GetShipmentObjectTable()
        {
            this.objectContext = WebFreightContext.GetContext(tenant);
            this.objectTabelRepository = new ObjectTableRepository(objectContext);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            this.objectTableId = objectTable.Id;
        }
        private void GetAllEvents()
        {
            this.traceEventRepository = new TraceEventRepository(objectContext);
            this.eventTypeRepository = new EventTypeRepository(objectContext);
            this.entityStatusRepository = new EntityStatusRepository(objectContext);
            this.allEventTypes = eventTypeRepository.GetEventTypesByTenantAndObjectTableId(tenant, objectTableId).ToList();
            this.allEntityStatuses = entityStatusRepository.GetEntityStatusByTenantAndObjectTableId(tenant, objectTableId).ToList();
        }
        private void CreateTraceEvent(string eventTypeCode)
        {
            this.CreateTraceEvent(new EventStatusTracerArgs()
            {
                Tenant = tenant,
                UserId = loggedContactId,
                EntityId = containerPM.Id,
                ObjectTableName = objectTableName,
                OldStatusId = container.StatusId,
                EventTypeCode = eventTypeCode,
            });
        }
        public void CreateTraceEvent(EventStatusTracerArgs args)
        {
            if (!string.IsNullOrEmpty(args.EventTypeCode))
            {
                if (args.ObjectTableName != this.objectTableName)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = tenant,
                        EventTypeCode = args.EventTypeCode,
                        UserId = loggedContactId,
                        EntityId = args.EntityId,
                        ObjectTableName = args.ObjectTableName,
                        Notes = args.Notes,
                        Entity = containerPM,
                    });
                }

                else
                {
                    EventType eventType = allEventTypes.Where(d => d.Code == args.EventTypeCode).FirstOrDefault();

                    if (eventType == null)
                    {
                        throw new Exception("Event Type is not recognized:" + args.EventTypeCode);
                    }

                    else
                    {
                        #region User
                        if (args.IsFromShipmentAPI)
                        {
                            myUserId = args.UserId;
                        }

                        else
                        {
                            if (myUserId == null)
                            {
                                if (!string.IsNullOrEmpty(args.UserId))
                                {
                                    myUserId = args.UserId;

                                    if (tenant != 0)
                                    {
                                        UserRepository userRepository = new UserRepository(0);
                                        User user = userRepository.GetSingleUser(myUserId, 0, true);
                                        if (user != null)
                                        {
                                            User systemUser = userRepository.GetSingleUserByEmail("system@tenant" + tenant + ".com", tenant, true);
                                            if (systemUser != null)
                                            {
                                                myUserId = systemUser.Id;
                                            }

                                            myCustomerCareUserEmail = user.Contact.Email;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Dates
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
                        #endregion

                        #region Status
                        if (args.IsAddedManually)
                        {
                            if (!string.IsNullOrEmpty(args.NewStatusId) && !string.IsNullOrEmpty(args.OldStatusId))
                            {
                                EntityStatus newStatus = EntityStatusRepository.GetSingleEntityStatus(args.NewStatusId, tenant, true);
                                EntityStatus oldStatus = EntityStatusRepository.GetSingleEntityStatus(args.OldStatusId, tenant, true);

                                Contact user = ContactRepository.GetSingleContact(myUserId, tenant, true);
                                if (newStatus != null)
                                {
                                    args.Notes = "Status was changed manually from " + oldStatus.Name + " to " + newStatus.Name + " by " + (user != null ? user.EnglishName : "");
                                }
                            }
                        }

                        else
                        {
                            ComputeEventStatus(args);
                        }
                        #endregion

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
                            UserId = myUserId,
                            CustomerCareUserEmail = myCustomerCareUserEmail,
                            Notes = args.Notes,
                            Location = args.StatusLocation,
                        };

                        this.traceEventRepository.Add(myTraceEvent);
                        this.traceEventRepository.SubmitChanges();
                        objectContext.SaveChanges();

                        if (!string.IsNullOrEmpty(eventType.CustomField))
                        {
                            EventCustomFieldUpdateService.UpdateEventCustomFieldValue(new UpdateEventCustomFieldArgs() { CustomField = eventType.CustomField, EventDateTime = myTraceEvent.EventDateTime, Entity = containerPM, EntityId = args.EntityId, ObjectTableName = args.ObjectTableName, Tenant = args.Tenant });
                        }
                    }
                }
            }
        }
        private void DeleteTraceEvent(string eventTypeCode, string pickupDeliveryIndex = null, DateTime? eventDateTime = null)
        {
            if (!string.IsNullOrEmpty(eventTypeCode))
            {
                EventType eventType = allEventTypes.Where(d => d.Code == eventTypeCode).FirstOrDefault();

                if (eventType != null)
                {
                    List<TraceEvent> AllEventTraces = this.traceEventRepository.GetAllTraceEventsByEventType(containerPM.Id, eventType.Id, tenant).ToList();
                    if (AllEventTraces.Count > 0)
                    {
                        foreach (TraceEvent iTraceEvent in AllEventTraces)
                        {
                            iTraceEvent.Deleted = true;
                            traceEventRepository.Update(iTraceEvent);
                        }

                        traceEventRepository.SubmitChanges();

                        if (!string.IsNullOrEmpty(eventType.EntityStatusId))
                        {
                            TraceEvent previousEvent = null;

                            List<TraceEvent> iTraceEventList = (from a in objectContext.TraceEvent.Include("EventType").Include("EventType.EntityStatus")
                                                                where a.Tenant == tenant
                                                                && a.EntityId == containerPM.Id
                                                                && a.ObjectTableId == objectTableId
                                                                && a.EventType.EntityStatus != null
                                                                && a.Deleted == false
                                                                select a).ToList();

                            foreach (TraceEvent e in iTraceEventList)
                            {
                                if (!e.Deleted)
                                {
                                    if (e.EventType.EntityStatus != null)
                                    {
                                        if (previousEvent == null)
                                        {
                                            previousEvent = e;
                                        }

                                        else
                                        {
                                            if (e.EventType.EntityStatus.StatusWeight > previousEvent.EventType.EntityStatus.StatusWeight)
                                            {
                                                previousEvent = e;
                                            }
                                        }
                                    }
                                }
                            }

                            if (previousEvent != null)
                            {
                                containerPM.StatusId = previousEvent.EventType.EntityStatusId;
                            }
                            else
                            {
                                EventType firstEventType = allEventTypes.Where(d => d.Code == "ORDR").FirstOrDefault();
                                containerPM.StatusId = firstEventType.EntityStatusId;
                            }
                            container.StatusId = containerPM.StatusId;
                        }
                    }
                }
            }
        }
        private void ComputeEventStatus(EventStatusTracerArgs args)
        {
            EventType newEventType = allEventTypes.Where(d => d.Code == args.EventTypeCode).FirstOrDefault();

            if (newEventType.EntityStatusId != null)
            {
                if (string.IsNullOrEmpty(args.OldStatusId))
                {
                    containerPM.StatusId = newEventType.EntityStatusId;
                    container.StatusId = containerPM.StatusId;
                }
                else
                {
                    EntityStatus newEntityStatus = allEntityStatuses.Where(d => d.Id == newEventType.EntityStatusId).FirstOrDefault();
                    EntityStatus oldEntityStatus = allEntityStatuses.Where(d => d.Id == args.OldStatusId).FirstOrDefault();

                    if (newEntityStatus.StatusWeight >= oldEntityStatus.StatusWeight)
                    {
                        containerPM.StatusId = newEventType.EntityStatusId;
                        container.StatusId = containerPM.StatusId;
                    }
                }
            }
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
        private void TraceUpdatedEvent()
        {
            if (isNewEntity)
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
        }
        private void TraceCreatedEvent()
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
                    UserId = containerPM.CreatedByUserId,
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
                    UserId = containerPM.CreatedByUserId,
                    EntityId = containerPM.Id,
                    ObjectTableName = "Container",
                });
            }
        }
        private void TraceEmptyPickUp()
        {
            if (containerPM.ActualEmptyPickupDate != null && container.ActualEmptyPickupDate == null)
            {
                this.CreateTraceEvent("EMPS");
            }

            else if (containerPM.MainCarriageETD == null && containerPM.MainCarriageETD != null)
            {
                this.DeleteTraceEvent("EMPS");
            }
        }
        private void TracDepartedFromShipper()
        {
            if (containerPM.ShipmentPickupATD != null && container.ShipmentPickupATD == null)
            {
                this.CreateTraceEvent("PICS");
            }

            else if (containerPM.ShipmentPickupATD == null && containerPM.ShipmentPickupATD != null)
            {
                this.DeleteTraceEvent("PICS");
            }
        }
        private void TracPreCarriageDeparted()
        {
            if (containerPM.ShipmentPickupATD != null && container.ShipmentPickupATD == null)
            {
                this.CreateTraceEvent("PCDP");
            }

            else if (containerPM.ShipmentPickupATD == null && containerPM.ShipmentPickupATD != null)
            {
                this.DeleteTraceEvent("PCDP");
            }
        }

        private void TracTransshipment1Arrived()
        {
            if (containerPM.ActualTransshipment1VesselArrival != null && container.ActualTransshipment1VesselArrival == null)
            {
                this.CreateTraceEvent("T1AV");
            }

            else if (containerPM.ActualTransshipment1VesselArrival == null && containerPM.ActualTransshipment1VesselArrival != null)
            {
                this.DeleteTraceEvent("T1AV");
            }
        }
        private void TracTransshipment1Departed()
        {
            if (containerPM.ActualTrans1VesselDeparture != null && container.ActualTrans1VesselDeparture == null)
            {
                this.CreateTraceEvent("T1DT");
            }

            else if (containerPM.ActualTrans1VesselDeparture == null && containerPM.ActualTrans1VesselDeparture != null)
            {
                this.DeleteTraceEvent("T1DT");
            }
        }
        private void TracTransshipment2Arrived()
        {
            if (containerPM.ActualTransshipment2VesselArrival != null && container.ActualTransshipment2VesselArrival == null)
            {
                this.CreateTraceEvent("T1AV");
            }

            else if (containerPM.ActualTransshipment2VesselArrival == null && containerPM.ActualTransshipment2VesselArrival != null)
            {
                this.DeleteTraceEvent("T1AV");
            }
        }
        private void TracTransshipment2Departed()
        {
            if (containerPM.ActualTrans2VesselDeparture != null && container.ActualTrans2VesselDeparture == null)
            {
                this.CreateTraceEvent("T2DT");
            }

            else if (containerPM.ActualTrans2VesselDeparture == null && containerPM.ActualTrans2VesselDeparture != null)
            {
                this.DeleteTraceEvent("T2DT");
            }
        }
        private void TracTransshipment3Arrived()
        {
            if (containerPM.ActualTransshipment3VesselArrival != null && container.ActualTransshipment3VesselArrival == null)
            {
                this.CreateTraceEvent("T3AV");
            }

            else if (containerPM.ActualTransshipment3VesselArrival == null && containerPM.ActualTransshipment3VesselArrival != null)
            {
                this.DeleteTraceEvent("T3AV");
            }
        }
        private void TracTransshipment3Departed()
        {
            if (containerPM.ActualTrans3VesselDeparture != null && container.ActualTrans3VesselDeparture == null)
            {
                this.CreateTraceEvent("T3DT");
            }

            else if (containerPM.ActualTrans3VesselDeparture == null && containerPM.ActualTrans3VesselDeparture != null)
            {
                this.DeleteTraceEvent("T3DT");
            }
        }
        private void TracDischarged()
        {
            if (containerPM.ActualPODDischarge != null && container.ActualPODDischarge == null)
            {
                this.CreateTraceEvent("DSCH");
            }

            else if (containerPM.ActualPODDischarge == null && containerPM.ActualPODDischarge != null)
            {
                this.DeleteTraceEvent("DSCH");
            }
        }
        private void TracOnCarriageDischarged()
        {
            if (containerPM.ActualOnCarriageDeparture != null && container.ActualOnCarriageDeparture == null)
            {
                this.CreateTraceEvent("UNDS");
            }

            else if (containerPM.ActualOnCarriageDeparture == null && containerPM.ActualOnCarriageDeparture != null)
            {
                this.DeleteTraceEvent("UNDS");
            }
        }



    }
}
