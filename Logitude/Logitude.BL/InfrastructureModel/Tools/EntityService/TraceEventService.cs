using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.Validating;
using Logitude.BL.InfrastructureModel.Tools.TraceEvents;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.Security;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class TraceEventService
    {
        bool isNewEntity;
        private int tenant;
        public TraceEvent Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private TraceEventPM entityPM;
        private IWebFreightContext objectContext;
        private TraceEventRepository entityRepository;
        public TraceEventService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new TraceEventRepository(objectContext);
        }

        public void Create(TraceEventPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("TraceEvent", tenant).ToString();
            this.Poco = new TraceEvent();
            this.Poco.Id = this.entityPM.Id;

            TraceEventValidating.Validate(theEntityPm);
            TraceEventTracing.Trace(theEntityPm, Poco, isNewEntity);
            TraceEventMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(TraceEventPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleTraceEvent(theEntityPm.Id);

            //TraceEventValidating.Validate(theEntityPm);
            //TraceEventTracing.Trace(theEntityPm, Poco, isNewEntity);
            this.Poco.Deleted = true;
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();

            this.Poco.Id = Guid.NewGuid().ToString();
            this.Poco.LogDateTime = TenantServerConfigration.GetCurrentDateTime(theEntityPm.Tenant);
            TraceEventMapping.MapEntity(theEntityPm, this.Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();


            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            ShipmentPM shipmentPM = null;

            if (theEntityPm.EventTypeCode == "EXCE")
            {
                shipmentPM = shipmentQuery.GetSinglePM(entityPM.EntityId, tenant);
                if (shipmentPM != null)
                {
                    shipmentPM.LastExceptionDescription = Poco.Notes;
                    shipmentPM.ExceptionDescription = Poco.Notes;
                    shipmentPM.IsUpdateEntityException = true;

                }
            }

            this.UpdateContinerExceptionFields(theEntityPm);


            EventTypeRepository eventTypeRepository = new EventTypeRepository(objectContext);
            EventType myEventType = eventTypeRepository.GetSingleEventType(theEntityPm.EventTypeId, tenant);
            if (myEventType != null)
            {
                if (myEventType.IsCustomerView)
                {
                    if (shipmentPM == null) shipmentPM = shipmentQuery.GetSinglePM(entityPM.EntityId, tenant);
                    if (shipmentPM != null)
                    {
                        shipmentPM.LastSharedEventId = myEventType.Id;
                        shipmentPM.LastSharedEventLocation = theEntityPm.Location;
                        shipmentPM.LastSharedEventNotes = theEntityPm.Notes;
                        shipmentPM.LastSharedEventDate = theEntityPm.EventDateTime;

                    }
                }
            }

            UpdateEventCustomFieldValue(theEntityPm, shipmentPM);

            if (shipmentPM != null)
            {
                ShipmentService shipmentService = new ShipmentService(ShipmentsContext.GetContext(tenant), shipmentPM, SecurityUtility.GetAuthenticatedUser());
                shipmentService.Update();
            }


        }

        private void UpdateContinerExceptionFields(TraceEventPM entityPm)
        {
            if (entityPm.EventTypeCode != "CEXC")
                return;

            ContainerPM containerPM = GetContainerPM(entityPm);
            if (containerPM == null)
                return;

            this.UpdateContainer(containerPM);
        }

        private ContainerPM GetContainerPM(TraceEventPM entityPm)
        {
            if (entityPm == null)
                return null;

            if (string.IsNullOrEmpty(entityPm.EntityId))
                return null;

            ContainerQuery containerQuery = new ContainerQuery(tenant);

            return containerQuery.GetSinglePM(entityPm.EntityId, tenant);
        }

        private void UpdateContainer(ContainerPM containerPM)
        {
            ContainerService containerService = new ContainerService(ShipmentsContext.GetContext(tenant), tenant);
            containerPM.LastExceptionDescription = Poco.Notes;
            containerPM.ExceptionDescription = Poco.Notes;
            containerPM.IsUpdateEntityException = true;

            containerService.Update(containerPM);
        }

        private void UpdateEventCustomFieldValue(TraceEventPM theEntityPm, ShipmentPM shipmentPM)
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTableRepository.GetSingleObjectTable(theEntityPm.ObjectTableId, tenant, true);
            if (objectTable.AllowCustomFields)
            {
                EventCustomFieldUpdateService.UpdateEventCustomFieldValue(new UpdateEventCustomFieldArgs() { EventTypeId = theEntityPm.EventTypeId, Entity = shipmentPM, EventDateTime = theEntityPm.EventDateTime, EntityId = theEntityPm.EntityId, ObjectTableName = objectTable != null ? objectTable.Name : null, Tenant = tenant });
            }
        }
    }
}