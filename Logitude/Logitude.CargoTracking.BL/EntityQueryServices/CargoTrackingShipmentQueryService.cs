using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.Data.EntityKeys;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.ShipmentOrderModule.BL.EntityQueryServices;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Logitude.CargoTracking.BL.EntityQueryServices
{
    public partial class CargoTrackingShipmentQueryService
    {
        private const string ForwardingShipmentEntityType = "F";
        private const string ShipmentOrderEntityType = "O";

        public CargoTrackingShipmentPM GetSingle(int id, bool getComposition, bool getFromCache)
        {
            EntityKeys = new CargoTrackingShipmentKeys() { Id = id };

            return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

        public CargoTrackingShipmentPM GetSinglePM(int id, int tenant)
        {
            throw new NotImplementedException();
        }

        public CargoTrackingShipmentPM GetSinglePMById(string entityId, int tenant)
        {
            CargoTrackingShipmentMappingService cargoShipmentMapper = new CargoTrackingShipmentMappingService();

            CargoTrackingShipmentPM cargoShipmentPM = GetCargoShipmentPMByEntityId(entityId, tenant);
            cargoShipmentMapper.MapCargoTrackingShipmentFields(cargoShipmentPM);

            return cargoShipmentPM;
        }
        public CargoTrackingShipmentPM GetSinglePMBySecurityKey(string securityKey, int tenant)
        {
            CargoTrackingShipmentPM cargoShipmentPM = GetCargoShipmentPMBySecurityKey(securityKey, tenant);

            CargoTrackingShipmentMappingService cargoShipmentMapper = new CargoTrackingShipmentMappingService();
            cargoShipmentMapper.MapCargoTrackingShipmentFields(cargoShipmentPM);

            return cargoShipmentPM;
        }

        public CargoTrackingShipmentPM GetMainShipmentByShipmentSecurityKey(string securityKey, int tenant)
        {
            CargoTrackingShipmentPM cargoShipmentPM = GetMainShipmentWithoutMapping(securityKey, tenant);

            CargoTrackingShipmentMappingService cargoShipmentMapper = new CargoTrackingShipmentMappingService();
            cargoShipmentMapper.MapCargoTrackingShipmentFields(cargoShipmentPM);

            return cargoShipmentPM;
        }

        private CargoTrackingShipmentPM GetMainShipmentWithoutMapping(string securityKey, int tenant)
        {
            CargoTrackingShipmentPM cargoShipmentPM = GetCargoShipmentPMBySecurityKey(securityKey, tenant);

            if (cargoShipmentPM.EntityType == ForwardingShipmentEntityType && cargoShipmentPM.CustomsShipmentHeaderId != null)
            {
                cargoShipmentPM = GetSinglePMById(cargoShipmentPM.CustomsShipmentHeaderId, tenant);
            }
            else if (cargoShipmentPM.EntityType == ShipmentOrderEntityType && cargoShipmentPM.ForwardingShipmentHeaderId != null)
            {
                cargoShipmentPM = GetSinglePMById(cargoShipmentPM.ForwardingShipmentHeaderId, tenant);

                if (cargoShipmentPM.EntityType == ForwardingShipmentEntityType && cargoShipmentPM.CustomsShipmentHeaderId != null)
                {
                    cargoShipmentPM = GetSinglePMById(cargoShipmentPM.CustomsShipmentHeaderId, tenant);
                }
            }

            return cargoShipmentPM;
        }

        private CargoTrackingShipmentPM GetCargoShipmentPMByEntityId(string entityId, int tenant)
        {
            CargoTrackingShipment cargoShipment = GetCargoShipmentById(entityId, tenant);
            CargoTrackingShipmentPM cargoShipmentPM = GetEntityPM(cargoShipment, false);
            return cargoShipmentPM;
        }

        private static CargoTrackingShipment GetCargoShipmentById(string entityId, int tenant)
        {
            CargoTrackingShipmentRepository cargoRepository = new CargoTrackingShipmentRepository(tenant);
            var cargoShipment = cargoRepository.GetCargoTrackingShipmentByEntityId(entityId, tenant);
            return cargoShipment;
        }

        private static CargoTrackingShipment GetCargoShipmentBySecurityKey(string securityKey, int tenant)
        {
            CargoTrackingShipmentRepository cargoRepository = new CargoTrackingShipmentRepository(tenant);
            var cargoShipment = cargoRepository.GetBySecurityKey(securityKey, tenant).FirstOrDefault();
            return cargoShipment;
        }

        private CargoTrackingShipmentPM GetCargoShipmentPMBySecurityKey(string securityKey, int tenant)
        {
            CargoTrackingShipment cargoShipment = GetCargoShipmentBySecurityKey(securityKey, tenant);
            CargoTrackingShipmentPM cargoShipmentPM = GetEntityPM(cargoShipment, false);
            return cargoShipmentPM;
        }

    }
}
