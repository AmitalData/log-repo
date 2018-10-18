using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ShipmentCarrierStatusService
    {
        bool isNewEntity;
        private int tenant;
        public ShipmentCarrierStatus Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ShipmentCarrierStatusPM entityPM;
        private IShipmentsContext objectContext;
        private ShipmentCarrierStatusRepository entityRepository;
        public ShipmentCarrierStatusService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ShipmentCarrierStatusRepository(objectContext);
        }

        public void Create(ShipmentCarrierStatusPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ShipmentCarrierStatus", tenant).ToString();
            this.Poco = new ShipmentCarrierStatus();
            this.Poco.Id = this.entityPM.Id;

            //ShipmentCarrierStatusValidator.Validate(theEntityPm);
            //CounterTracing.Trace(theEntityPm, Poco, isNewEntity);
            //CounterMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();

        }

        public void Update(ShipmentCarrierStatusPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleShipmentCarrierStatus(theEntityPm.Id, theEntityPm.Tenant);

            //CounterValidating.Validate(theEntityPm);
            //CounterTracing.Trace(theEntityPm, Poco, isNewEntity);
            //CounterMapping.MapEntity(theEntityPm, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}