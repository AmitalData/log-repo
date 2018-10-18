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


            if (theEntityPm.EventTypeCode == "EXCE")
            {
                IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
                ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
                Shipment shipment = shipmentRepository.GetSingleShipment(entityPM.EntityId, entityPM.Tenant);
                if (shipment != null)
                {
                    shipment.LastExceptionDescription = Poco.Notes;
                    shipment.ExceptionDescription = Poco.Notes;
                    shipmentRepository.Update(shipment);
                    shipmentRepository.SubmitChanges();
                }
            }

            EventTypeRepository eventTypeRepository = new EventTypeRepository(objectContext);
            EventType myEventType = eventTypeRepository.GetSingleEventType(theEntityPm.EventTypeId, tenant);
            if(myEventType != null)
            {
                if(myEventType.IsCustomerView)
                {
                    IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
                    ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
                    Shipment shipment = shipmentRepository.GetSingleShipment(entityPM.EntityId, entityPM.Tenant);
                    if (shipment != null)
                    {
                        shipment.LastSharedEventId = myEventType.Id;
                        shipment.LastSharedEventLocation = theEntityPm.Location;
                        shipment.LastSharedEventNotes = theEntityPm.Notes;
                        shipment.LastSharedEventDate = theEntityPm.EventDateTime;
                        shipmentRepository.Update(shipment);
                        shipmentRepository.SubmitChanges();
                    }
                }
            }
        }
    }
}