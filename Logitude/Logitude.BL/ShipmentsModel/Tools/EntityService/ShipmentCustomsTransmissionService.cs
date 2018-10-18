using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ShipmentCustomsTransmissionService
    {
        bool isNewEntity;
        private int tenant;
        public ShipmentCustomsTransmission Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ShipmentCustomsTransmissionPM entityPM;
        private IShipmentsContext objectContext;
        private ShipmentCustomsTransmissionRepository entityRepository;
        public ShipmentCustomsTransmissionService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ShipmentCustomsTransmissionRepository(objectContext);
        }

        public void Create(ShipmentCustomsTransmissionPM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.entityPM.Id = IdCounter.GetNumber("ShipmentCustomsTransmission", tenant).ToString();
            this.Poco = new ShipmentCustomsTransmission();
            this.Poco.Id = this.entityPM.Id;
            ShipmentMapping.MapShipmentCustomsTransmission(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ShipmentCustomsTransmissionPM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleShipmentCustomsTransmission(theEntityPm.Id, entityPM.Tenant);
            ShipmentMapping.MapShipmentCustomsTransmission(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
