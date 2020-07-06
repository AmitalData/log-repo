using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class TraceEventsDomainController : ApiController
    {
        public HttpResponseMessage GetTraceEventsForEntity(string objectTableId, string entityId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    string loggedUserEmail = authToken.Email;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    TraceEventRepository traceEventsRepository = new TraceEventRepository(tenant);
                    TraceEventQuery traceEventQuery = new TraceEventQuery(traceEventsRepository);
                    IQueryable<TraceEventPM> myResult = traceEventQuery.GetTraceEventPMsByTenantByEntityId(tenant, entityId, objectTableId).OrderByDescending(s => s.LogDateTime);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage Post(TraceEventsServiceArgs args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    string loggedUserEmail = authToken.Email;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    string loggedUserId = null;
                    ContactQuery contactQuery = new ContactQuery(tenant);
                    ContactPM contact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                    if (contact != null)
                    {
                        loggedUserId = contact.Id;
                    }

                    IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
                    TraceEventRepository traceEventRepository = new TraceEventRepository(webFreightContext);

                    TraceEvent newTraceEvent = new TraceEvent();
                    newTraceEvent.Id = Guid.NewGuid().ToString();
                    newTraceEvent.LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
                    newTraceEvent.Notes = args.Notes;
                    newTraceEvent.ObjectTableId = args.ObjectTableId;
                    newTraceEvent.Tenant = tenant;
                    newTraceEvent.UserId = loggedUserId;
                    newTraceEvent.EventTypeId = args.EventTypeId;
                    newTraceEvent.EventDateTime = args.EventDate != null ? args.EventDate.Value : TenantServerConfigration.GetCurrentDateTime(tenant);
                    newTraceEvent.EntityId = args.EntityId;
                    newTraceEvent.Deleted = false;
                    newTraceEvent.IsAddedManually = true;
                    traceEventRepository.Add(newTraceEvent);
                    traceEventRepository.SubmitChanges();

                    NewTraceEventResult myResult = new NewTraceEventResult();
                    myResult.LogDateTime = newTraceEvent.LogDateTime;

                    ObjectTableRepository objectTableRepository = new ObjectTableRepository(webFreightContext);
                    ObjectTable objectTable = objectTableRepository.GetSingleObjectTable(args.ObjectTableId, tenant, true);
                    if (objectTable != null)
                    {
                        ShipmentPM shipmentPM = null;
                        if (objectTable.Name == "Shipment" || objectTable.Name == "Master")
                        {
                            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                            shipmentPM = shipmentQuery.GetSinglePM(args.EntityId, tenant);
                            this.OnInsertTraceEventForShipment(shipmentPM, args.EventTypeId, newTraceEvent, tenant, webFreightContext, myResult);
                        }

                        if (objectTable.AllowCustomFields)
                        {
                            EventCustomFieldUpdateService.UpdateEventCustomFieldValue(new UpdateEventCustomFieldArgs()
                            {
                                EventTypeId =args.EventTypeId,
                                Entity = shipmentPM,
                                EventDateTime = newTraceEvent.EventDateTime,
                                EntityId = args.EntityId,
                                ObjectTableName = objectTable != null? objectTable.Name : null,
                                Tenant = tenant
                            });
                        }

                        if (shipmentPM != null)
                        {
                            ShipmentService shipmentService = new ShipmentService(ShipmentsContext.GetContext(tenant), shipmentPM, SecurityUtility.GetAuthenticatedUser());
                            shipmentService.Update();
                        }
                    }




                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PutDeleteTraceEvent(TraceEventsServiceArgs args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    bool isShipment = false;
                    ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
                    ObjectTable objectTable = objectTableRepository.GetSingleObjectTable(args.ObjectTableId, tenant, true);
                    if (objectTable != null)
                    {
                        if (objectTable.Name == "Shipment" || objectTable.Name == "Master")
                        {
                            isShipment = true;
                        }
                    }

                    if (isShipment)
                    {
                        ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
                        ShipmentPM entityPM = shipmentQuery.GetSinglePM(args.EntityId, tenant);
                        if (entityPM != null)
                        {
                            ShipmentTracing.DeleteShipmentTraceEvent(entityPM, args.TraceEventId, tenant, args.IsExternal);
                        }
                    }

                    else
                    {
                        TraceEventRepository traceEventRepository = new TraceEventRepository(tenant);
                        TraceEvent traceEvent = traceEventRepository.GetSingleTraceEvent(args.TraceEventId);
                        if (traceEvent != null)
                        {
                            traceEvent.Deleted = true;
                            traceEventRepository.Update(traceEvent);
                            traceEventRepository.SubmitChanges();
                        }
                    }

                    bool myResult = true;

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private void OnInsertTraceEventForShipment(ShipmentPM entityPM, string eventTypeId, TraceEvent newTraceEvent, int tenant, IWebFreightContext webFreightContext, NewTraceEventResult myResult)
        {
            IShipmentsContext objectContext = ShipmentsContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(objectContext);
            //ShipmentPM entityPOCO = shipmentRepository.GetSingleShipment(entityId, tenant);

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
                            EntityStatusRepository entityStatusRepository = new EntityStatusRepository(tenant);
                            EntityStatus newStatus = EntityStatusRepository.GetSingleEntityStatus(eventType.EntityStatusId, tenant, true);

                            entityPM.StatusId = eventType.EntityStatusId;
                            entityPM.StatusDate = newTraceEvent.EventDateTime;
                            entityPM.StatusLocation = null;
                            entityPM.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                            entityPM.IsStatusChange = true;
                            //if (entityPOCO.ShipmentLevelCode == "D" || entityPOCO.ShipmentLevelCode == "C")
                            //{
                            //    ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(objectContext);
                            //    ShipmentMasterData entityMasterData = shipmentMasterDataRepository.GetSingleMasterData

                            // if( entityPOCO.MasterShipmentDataId!=null){
                            //    entityMasterData.StatusId = entityPOCO.StatusId;
                            //    entityMasterData.StatusDate = entityPOCO.StatusDate;
                            //    entityMasterData.StatusLocation = entityPOCO.StatusLocation;
                            //    shipmentMasterDataRepository.Update(entityMasterData);
                            //}
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
                            EntityStatusRepository entityStatusRepository = new EntityStatusRepository(tenant);
                            EntityStatus oldStatus = EntityStatusRepository.GetSingleEntityStatus(oldStatusId, tenant, true);
                            EntityStatus newStatus = EntityStatusRepository.GetSingleEntityStatus(newStatusId, tenant, true);

                            if (newStatus.StatusWeight >= oldStatus.StatusWeight)
                            {
                                entityPM.StatusId = newStatusId;
                                entityPM.StatusDate = newTraceEvent.EventDateTime;
                                entityPM.StatusLocation = null;
                                entityPM.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                                entityPM.IsStatusChange = true;

                                //if (entityPOCO.ShipmentLevelCode == "D" || entityPOCO.ShipmentLevelCode == "C")
                                //{
                                //    ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(objectContext);
                                //    ShipmentMasterData entityMasterData = shipmentMasterDataRepository.GetSingleMasterData

                                // if( entityPOCO.MasterShipmentDataId!=null){
                                //    entityMasterData.StatusId = entityPOCO.StatusId;
                                //    entityMasterData.StatusDate = entityPOCO.StatusDate;
                                //    entityMasterData.StatusLocation = entityPOCO.StatusLocation;
                                //    shipmentMasterDataRepository.Update(entityMasterData);
                                //}

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

    public class NewTraceEventResult
    {
        public string EntityId { get; set; }
        public bool StatusChanged { get; set; }
        public DateTime? LogDateTime { get; set; }
        public string StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusLocation { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? LastStatusLogDate { get; set; }
        public string LastSharedEventId { get; set; }
        public string LastSharedEventLocation { get; set; }
        public string LastSharedEventNotes { get; set; }
        public DateTime? LastSharedEventDate { get; set; }
    }
}