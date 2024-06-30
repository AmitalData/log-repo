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
    public class ShipmentReferanceService
    {
        bool isNewEntity;
        private int tenant;
        public ShipmentReferance Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ShipmentReferancePM entityPM;
        private IShipmentsContext objectContext;
        private ShipmentReferanceRepository entityRepository;
        public ShipmentReferanceService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ShipmentReferanceRepository(objectContext);
        }

        public void Create(ShipmentReferancePM theEntityPm)
        {
            this.isNewEntity = true;
            this.entityPM = theEntityPm;
            this.Poco = new ShipmentReferance();
            this.Poco.ShipmentId = this.entityPM.ShipmentId;
            ShipmentMapping.MapShipmentReferance(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ShipmentReferancePM theEntityPm)
        {
            this.isNewEntity = false;
            this.entityPM = theEntityPm;
            this.Poco = entityRepository.GetSingleShipmentReferance(theEntityPm.ShipmentId, entityPM.Tenant);
            ShipmentMapping.MapShipmentReferance(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
    }
}
