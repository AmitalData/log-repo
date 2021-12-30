using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.Data.EntityKeys;
using Logitude.CargoTracking.BL.CloseTables;
using Logitude.CargoTracking.Data.EntityListQueryServices;
using Logitude.CargoTracking.Data.EntityLists;
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
using Logitude.CargoTracking.BL.CoreBL;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.CargoTracking.BL.EntityQueryServices
{
    public partial class CargoTrackingShipmentQueryService
    {
        private const string ForwardingShipmentEntityType = "F";
        private const string ShipmentOrderEntityType = "O";


        public CargoTrackingShipmentPM GetSinglePM(int id, int tenant)
        {
            throw new NotImplementedException();
        }

        public CargoTrackingShipmentPM GetSinglePMById(string entityId, int tenant)
        {
            CargoTrackingShipmentMappingService cargoShipmentMapper = new CargoTrackingShipmentMappingService();

            CargoTrackingShipmentPM cargoShipmentPM = GetCargoShipmentPMByEntityId(entityId, tenant);
            var milestoneDictionary = GetMilestonesDictionaryByCode();
            cargoShipmentMapper.MapCargoTrackingShipmentFields(cargoShipmentPM, milestoneDictionary);

            return cargoShipmentPM;
        }
        public CargoTrackingShipmentPM GetSinglePMBySecurityKey(string securityKey, int tenant)
        {
            CargoTrackingShipmentPM cargoShipmentPM = GetCargoShipmentPMBySecurityKey(securityKey, tenant);

            CargoTrackingShipmentMappingService cargoShipmentMapper = new CargoTrackingShipmentMappingService();
            var milestoneDictionary = GetMilestonesDictionaryByCode();
            cargoShipmentMapper.MapCargoTrackingShipmentFields(cargoShipmentPM, milestoneDictionary);

            return cargoShipmentPM;
        }

        public CargoTrackingShipmentPM GetMainShipmentByShipmentSecurityKey(string securityKey, int tenant)
        {
            CargoTrackingShipmentPM cargoShipmentPM = GetMainShipmentWithoutMapping(securityKey, tenant);

            CargoTrackingShipmentMappingService cargoShipmentMapper = new CargoTrackingShipmentMappingService();
            var milestoneDictionary = GetMilestonesDictionaryByCode();
            cargoShipmentMapper.MapCargoTrackingShipmentFields(cargoShipmentPM, milestoneDictionary);

            return cargoShipmentPM;
        }

        public ShipmentAdditionalCloudData GetShipmentCloudData(string shipmentId, int tenant)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(tenant);
            var cloudData = shipmentQuery.GetShipmentAdditionalCloudData(shipmentId, tenant);
            return cloudData;
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
        public List<CargoTrackingShipmentList> GetShipments(string searchText, int tenant)
        {
            CargoTrackingShipmentSearchListQueryService cargoTrackingShipmentSearchListQueryService = new CargoTrackingShipmentSearchListQueryService(context);
            List<CargoTrackingShipmentList> shipments = cargoTrackingShipmentSearchListQueryService.GetShipments(searchText, tenant);
            SetFutureMilstone(shipments);
            return shipments;
        }
        public void SetFutureMilstone(List<CargoTrackingShipmentList> shipments)
        {
            var milestonesDictionaryByCode = GetMilestonesDictionaryByCode();
            foreach (CargoTrackingShipmentList shipment in shipments)
            {
                SetFutureMilstone(shipment, milestonesDictionaryByCode);
            }
        }
        public void SetFutureMilstone(CargoTrackingShipmentList shipment, Dictionary<string, CargoTrackingMilestoneList> milestonesDictionaryByCode)
        {
            CargoTrackingMilestoneBuilder cargoTrackingMilestoneBuilder = new CargoTrackingMilestoneBuilder();
            List<Milestone> shipmentMilestones = cargoTrackingMilestoneBuilder.BuildShipmentMilstones(shipment, milestonesDictionaryByCode);
            SetMilestonesStatus(shipment, shipmentMilestones);
        }
        public Dictionary<string, CargoTrackingMilestoneList> GetMilestonesDictionaryByCode()
        {
            CargoTrackingMilestoneListQueryService cargoTrackingMilestoneListQueryService = new CargoTrackingMilestoneListQueryService(context);
            var dbMilestones = cargoTrackingMilestoneListQueryService.GetList(Tenant);
            return dbMilestones.ToDictionary(e => e.Code, e => e);
        }


        public void SetMilestonesStatus(CargoTrackingShipmentList shipment, List<Milestone> shipmentMilestones)
        {

            SetCurrentMilestone(shipmentMilestones);
            SetDoneMilstones(shipmentMilestones);
            SetFutureMilstoneForShipment(shipment, shipmentMilestones);
        }
        private void SetCurrentMilestone(List<Milestone> milestones)
        {
            Milestone currentMilstone = GetMostRecentNotEstimatedMilestone(milestones);
            if (currentMilstone != null)
                currentMilstone.IsCurrent = true;
        }
        private void SetDoneMilstones(List<Milestone> shipmentMilestones)
        {
            Milestone currentMilstone = shipmentMilestones.FirstOrDefault(d => d.IsCurrent == true);
            if (currentMilstone != null)
            {
                var doneMilstones = shipmentMilestones.Where(milstone => milstone.Weight < currentMilstone.Weight).ToList();
                doneMilstones.ForEach(doneMilstone =>
                {
                    doneMilstone.Done = true;
                    doneMilstone.IsEstimation = false;
                });
            }
        }
        private void SetFutureMilstoneForShipment(CargoTrackingShipmentList Shipment, List<Milestone> milestones)
        {
            Milestone futureMilstone = GetMostRecentEstimatedMilestone(milestones);

            if (futureMilstone != null)
            {
                Shipment.FutureMilstoneCode = futureMilstone.Code;
                Shipment.FutureMilstoneName = futureMilstone.Name;
                Shipment.FutureMilstoneDate = futureMilstone.EstimationDate;
            }
        }
        private Milestone GetMostRecentNotEstimatedMilestone(List<Milestone> milestones)
        {
            return milestones.Where(s => s.IsEstimation != true && s.Date != null).OrderByDescending(s => s.Weight).FirstOrDefault();
        }
        private static Milestone GetMostRecentEstimatedMilestone(List<Milestone> milestones)
        {
            Milestone currentMilstone = milestones.Find(m => m.IsCurrent == true);
            return milestones.Where(s => s.IsEstimation == true && s.EstimationDate != null && s.Weight > currentMilstone.Weight)
                                                        .OrderBy(s => s.Weight)
                                                        .FirstOrDefault();
        }
    }
}
