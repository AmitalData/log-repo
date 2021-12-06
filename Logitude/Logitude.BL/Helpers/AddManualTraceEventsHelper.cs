using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools.Models;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Helpers
{
    public class AddManualTraceEventsHelper : IAddManualTraceEventsHelper
    {
        public int tenant;
        private IWebFreightContext webFreightContext;

        public AddManualTraceEventsHelper()
        {
        }

        public AddManualTraceEventsHelper(int tenant)
        {
            Initialize(tenant);
        }

        public void Initialize(int tenant)
        {
            this.tenant = tenant;
            webFreightContext = WebFreightContext.GetContext(tenant);
        }


        public NewTraceEventResult Trace(TraceEventsServiceArgs args, string loggedUserEmail)
        {
            NewTraceEventResult newTraceEventResult = new NewTraceEventResult();
            string loggedUserId = GetLoggedUserId(args, loggedUserEmail);

            TraceEventRepository traceEventRepository = new TraceEventRepository(webFreightContext);

            TraceEvent newTraceEvent = new TraceEvent();
            newTraceEvent.Id = Guid.NewGuid().ToString();
            newTraceEvent.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.Notes = args.Notes;
            newTraceEvent.ObjectTableId = args.ObjectTableId;
            newTraceEvent.Tenant = tenant;
            newTraceEvent.UserId = args.IsFromAPI ? args.UserId : loggedUserId;
            newTraceEvent.EventTypeId = args.EventTypeId;
            newTraceEvent.EventDateTime = args.EventDate != null ? args.EventDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
            newTraceEvent.EntityId = args.EntityId;
            newTraceEvent.Deleted = false;
            newTraceEvent.IsAddedManually = !args.IsAutomation;
            traceEventRepository.Add(newTraceEvent);
            traceEventRepository.SubmitChanges();
            newTraceEventResult.LogDateTime = newTraceEvent.LogDateTime;

            this.UpdateShipment(newTraceEvent, newTraceEventResult, args);

            return newTraceEventResult;
        }

        private string GetLoggedUserId(TraceEventsServiceArgs args, string loggedUserEmail)
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            if (args.IsAutomation)
            {
                return contactQuery.GetSingleByEmail(loggedUserEmail, tenant)?.Id;
            }
            return contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant)?.Id;
        }

        private void UpdateShipment(TraceEvent newTraceEvent, NewTraceEventResult newTraceEventResult, TraceEventsServiceArgs args)
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(webFreightContext);
            ObjectTable objectTable = objectTableRepository.GetSingleObjectTable(args.ObjectTableId, tenant, true);
            if (objectTable != null)
            {
                ShipmentPM shipmentPM = null;
                if (objectTable.Name == "Shipment" || objectTable.Name == "Master")
                {
                    shipmentPM = GetShipmentPM(args);
                    this.OnInsertTraceEventForShipment(shipmentPM, args.EventTypeId, newTraceEvent, tenant, webFreightContext, newTraceEventResult);
                }

                if (objectTable.AllowCustomFields)
                {
                    EventCustomFieldUpdateService.UpdateEventCustomFieldValue(new UpdateEventCustomFieldArgs()
                    {
                        EventTypeId = args.EventTypeId,
                        Entity = shipmentPM,
                        EventDateTime = newTraceEvent.EventDateTime,
                        EntityId = args.EntityId,
                        ObjectTableName = objectTable != null ? objectTable.Name : null,
                        Tenant = tenant
                    });
                }

                if (shipmentPM != null && !args.IsAutomation)
                {
                    ShipmentService shipmentService = new ShipmentService(ShipmentsContext.GetContext(tenant), shipmentPM, SecurityUtility.GetAuthenticatedUser(tenant));
                    shipmentService.Update();
                }
            }
        }

        private ShipmentPM GetShipmentPM(TraceEventsServiceArgs args)
        {
            if (args.EntityPM != null)
                return (ShipmentPM)args.EntityPM;

            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            return shipmentQuery.GetSinglePM(args.EntityId, tenant);
        }

        private void OnInsertTraceEventForShipment(ShipmentPM entityPM, string eventTypeId, TraceEvent newTraceEvent, int tenant, IWebFreightContext webFreightContext, NewTraceEventResult myResult)
        {
            IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(objectContext);

            if (entityPM != null)
            {
                EventTypeRepository eventTypeRep = new EventTypeRepository(webFreightContext);
                EventType eventType = eventTypeRep.GetSingleEventType(eventTypeId, tenant);
                if (eventType != null)
                {
                    if (!string.IsNullOrEmpty(eventType.EntityStatusId))
                    {
                        #region
                        if (string.IsNullOrEmpty(entityPM.StatusId))
                        {
                            EntityStatus newStatus = EntityStatusRepository.GetSingleEntityStatus(eventType.EntityStatusId, tenant, true);
                            entityPM.StatusId = eventType.EntityStatusId;
                            entityPM.StatusDate = newTraceEvent.EventDateTime;
                            entityPM.StatusLocation = null;
                            entityPM.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            entityPM.IsStatusChange = true;
                            myResult.StatusChanged = true;
                            myResult.EntityId = entityPM.Id;
                            myResult.StatusId = entityPM.StatusId;
                            myResult.StatusName = newStatus.Name;
                            myResult.StatusDate = entityPM.StatusDate;
                            myResult.StatusLocation = entityPM.StatusLocation;
                            myResult.LastStatusLogDate = entityPM.LastStatusLogDate;
                        }

                        else
                        {
                            string oldStatusId = entityPM.StatusId;
                            string newStatusId = eventType.EntityStatusId;
                            EntityStatus oldStatus = EntityStatusRepository.GetSingleEntityStatus(oldStatusId, tenant, true);
                            EntityStatus newStatus = EntityStatusRepository.GetSingleEntityStatus(newStatusId, tenant, true);
                            if (newStatus.StatusWeight >= oldStatus.StatusWeight)
                            {
                                entityPM.StatusId = newStatusId;
                                entityPM.StatusDate = newTraceEvent.EventDateTime;
                                entityPM.StatusLocation = null;
                                entityPM.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                entityPM.IsStatusChange = true;
                                myResult.StatusChanged = true;
                                myResult.EntityId = entityPM.Id;
                                myResult.StatusId = entityPM.StatusId;
                                myResult.StatusName = newStatus.Name;
                                myResult.StatusDate = entityPM.StatusDate;
                                myResult.StatusLocation = entityPM.StatusLocation;
                                myResult.LastStatusLogDate = entityPM.LastStatusLogDate;
                            }
                        }
                        #endregion
                    }

                    if (eventType.Code == "EXCE")
                    {
                        #region
                        ContactsUnseenEntitiesHelper.AddUnseenEntityRecord(newTraceEvent.Id, tenant);
                        entityPM.ExceptionDate = newTraceEvent.EventDateTime;
                        entityPM.ExceptionDescription = entityPM.LastExceptionDescription = newTraceEvent.Notes;
                        entityPM.HasException = true;
                        entityPM.ExceptionResolvedDescription = null;
                        entityPM.IsUpdateEntityException = true;
                        if (entityPM.ShipmentLevelCode == "A")
                        {
                            List<Shipment> connectedShipments = shipmentRepository.GetConnectedCustomShipments(tenant, entityPM.Id).ToList();
                            foreach (Shipment sh in connectedShipments)
                            {
                                sh.HasException = entityPM.HasException;
                                sh.ExceptionDate = entityPM.ExceptionDate;
                                sh.ExceptionDescription = entityPM.ExceptionDescription;
                                sh.ExceptionResolvedDescription = entityPM.ExceptionResolvedDescription;
                                sh.LastExceptionDescription = entityPM.LastExceptionDescription;
                                shipmentRepository.Update(sh);
                            }
                        }
                        #endregion
                    }

                    else if (eventType.Code == "EXRE")
                    {
                        #region
                        entityPM.ExceptionResolvedDescription = newTraceEvent.Notes;
                        entityPM.HasException = false;
                        entityPM.ExceptionDescription = null;
                        entityPM.ExceptionDate = null;
                        entityPM.IsUpdateEntityException = true;

                        if (entityPM.ShipmentLevelCode == "A")
                        {
                            List<Shipment> connectedShipments = shipmentRepository.GetConnectedCustomShipments(tenant, entityPM.Id).ToList();
                            foreach (Shipment sh in connectedShipments)
                            {
                                sh.ExceptionResolvedDescription = entityPM.ExceptionResolvedDescription;
                                sh.HasException = entityPM.HasException;
                                sh.ExceptionDescription = entityPM.ExceptionDescription;
                                sh.LastExceptionDescription = entityPM.LastExceptionDescription;
                                sh.ExceptionDate = entityPM.ExceptionDate;
                                shipmentRepository.Update(sh);
                            }
                        }
                        #endregion
                    }

                    if (eventType.IsCustomerView)
                    {
                        this.ComputeLastSharedEvent(entityPM, tenant);

                        myResult.LastSharedEventId = entityPM.LastSharedEventId;
                        myResult.LastSharedEventLocation = entityPM.LastSharedEventLocation;
                        myResult.LastSharedEventNotes = entityPM.LastSharedEventNotes;
                        myResult.LastSharedEventDate = entityPM.LastSharedEventDate;
                    }

                    shipmentRepository.SubmitChanges();
                }
            }
        }
        private void ComputeLastSharedEvent(ShipmentPM entityPM, int tenant)
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Shipment", 0, true);
            string objectTableId = objectTable.Id;

            TraceEventRepository traceEventRep = new TraceEventRepository(tenant);
            List<TraceEvent> myEventList = traceEventRep.GetTraceEvents(tenant, entityPM.Id, objectTableId).ToList();
            myEventList = myEventList.Where(d => d.EventType.IsCustomerView && !d.Deleted).ToList();

            if (myEventList.Count > 0)
            {
                TraceEvent myHigherEvent = myEventList.OrderByDescending(d => d.EventDateTime).FirstOrDefault();
                if (myHigherEvent != null)
                {
                    entityPM.LastSharedEventId = myHigherEvent.EventTypeId;
                    entityPM.LastSharedEventLocation = myHigherEvent.Location;
                    entityPM.LastSharedEventNotes = myHigherEvent.Notes;
                    entityPM.LastSharedEventDate = myHigherEvent.EventDateTime;
                }
            }

            else
            {
                entityPM.LastSharedEventId = null;
                entityPM.LastSharedEventLocation = null;
                entityPM.LastSharedEventNotes = null;
                entityPM.LastSharedEventDate = null;
            }
        }
    }

}
