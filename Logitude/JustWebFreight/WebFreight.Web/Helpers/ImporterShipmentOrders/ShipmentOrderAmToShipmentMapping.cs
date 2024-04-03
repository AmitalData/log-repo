using Logitude.BL.CommonDataModel.CodePropertiesMapping;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.ShipmentOrderModule.Def.EntityAMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;

namespace WebFreight.Web.Helpers.ImporterShipmentOrders
{
    public class ShipmentOrderAmToShipmentMapping
    {
        private readonly int tenant;
        private readonly PackageTypeRepository packageTypeRepository;

        public ShipmentOrderAmToShipmentMapping(int tenant)
        {
            this.tenant = tenant;
            ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
            packageTypeRepository = new PackageTypeRepository(commoncontext);
        }

        public ShipmentPM Map(ShipmentOrderAM shipmentOrder, ShipmentPM shipment)
        {
            shipment.Volume = shipmentOrder.Volume;
            shipment.TransportModeId = shipmentOrder.TransportModeId;
            shipment.ForwarderShipmentNumber = shipmentOrder.OrderNumber;
            shipment.CustomerReference3 = shipmentOrder.CustomerReferences;
            shipment.PrivateLabelAgentName = shipmentOrder.AgentName;
            shipment.MainCarriageATA = shipmentOrder.MainCarriageATA;
            shipment.MainCarriageETA = shipmentOrder.MainCarriageETA;
            shipment.MainCarriageATD = shipmentOrder.MainCarriageATD;
            shipment.MainCarriageETD = shipmentOrder.MainCarriageETD;
            shipment.NumberOfContainers = shipmentOrder.Quantity;
            shipment.NumberOfPackages = shipmentOrder.Quantity;
            shipment.PackagesQuantity = shipmentOrder.Quantity;
            shipment.GrossWeight = shipmentOrder.Weight;

            shipment.StatusId = GetCreatedEntityStatusId();
            shipment.StatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            shipment.LastStatusLogDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            shipment.IsStatusChange = true;
            shipment.IsShipmentOrder = true;
            shipment.IsCancelled = shipmentOrder.IsCancelled;

            GetShipperId(shipmentOrder, shipment);
            if (!string.IsNullOrEmpty(shipmentOrder.ShipperName))
            {
                shipment.ShipperName = shipmentOrder.ShipperName;
            }

            GetIncotermId(shipmentOrder, shipment);
            shipment.ShipmentPackages = GetShipmentPackages(shipmentOrder, shipment);
            shipment.NumberOfContainers = shipmentOrder.Quantity;
            shipment.NumberOfPackages = shipmentOrder.Quantity;
            shipment.PackagesQuantity = shipmentOrder.Quantity;
            shipment.GrossWeight = shipmentOrder.Weight;
            MapShipmentAdditionalCloudData(shipment, shipmentOrder);
            return shipment;
        }

        private List<ShipmentPackagePM> GetShipmentPackages(ShipmentOrderAM shipmentOrder, ShipmentPM shipment)
        {
            ShipmentPackagePM package = BuildShipmentPackage(shipmentOrder);
            if (package == null) return shipment.ShipmentPackages;

            var shipmentPackages = shipment.ShipmentPackages ?? new List<ShipmentPackagePM>();
            shipmentPackages.Add(package);
            return shipmentPackages;
        }

        private ShipmentPackagePM BuildShipmentPackage(ShipmentOrderAM shipmentOrder)
        {
            var packageType = GetPackageTypeByCode(shipmentOrder.PackageTypeCode);
            if (packageType == null) return null;
            return new ShipmentPackagePM
            {
                PackageTypeId = packageType.Id,
                Quantity = shipmentOrder.Quantity,
                IsContainer = packageType.IsContainer,
                Volume = shipmentOrder.Volume,
                Weight = shipmentOrder.Weight,
                ChangeSetOp = ChangeSetOperation.Insert
            };
        }

        private PackageType GetPackageTypeByCode(string packageTypeCode)
        {
            if (packageTypeCode == null) return null;
            var package = packageTypeRepository.GetSinglePackageTypeByCode(packageTypeCode, tenant, true);
            if (package == null) throw new Exception("package type doesn't exist in the database, insert this entity before using it.");
            return package;
        }

        private string GetCreatedEntityStatusId()
        {
            var statusId = EntityStatusRepository.GetSingleEntityStatusByCode("OPOP", tenant, true)?.Id;
            if (string.IsNullOrEmpty(statusId))
            {
                throw new Exception("created status field doesn't exist in the database, insert this entity before using it.");
            }
            return statusId;
        }

        private void GetIncotermId(ShipmentOrderAM shipmentOrder, ShipmentPM shipment)
        {
            if (shipmentOrder.Incoterm == null) return;
            var incotermId = IncotermCodePropertiesMapping.GetIncotermIdFromIncotermProperties(shipmentOrder.CustomerTenantNumber, shipmentOrder.Incoterm);
            if (string.IsNullOrEmpty(incotermId))
            {
                throw new Exception("IncotermId field doesn't exist in the database, insert this entity before using it.");
            }
            shipment.IncotermId = incotermId;
        }

        private void GetShipperId(ShipmentOrderAM shipmentOrder, ShipmentPM shipment)
        {
            if (shipmentOrder.Shipper == null) return;
            if (string.IsNullOrEmpty(shipmentOrder.Shipper.Code) && string.IsNullOrEmpty(shipmentOrder.Shipper.Id) && string.IsNullOrEmpty(shipmentOrder.Shipper.ExternalCode))
            {
                return;
            }
            var shipperId = CardCodePropertiesMapping.GetCardIdFromCardProperties(shipmentOrder.CustomerTenantNumber, shipmentOrder.Shipper);
            if (!string.IsNullOrEmpty(shipperId))
            {
                shipment.ShipperId = shipperId;
            }
        }

        private void MapShipmentAdditionalCloudData(ShipmentPM shipment, ShipmentOrderAM shipmentOrder)
        {
            if (shipment.ShipmentAdditionalData == null) shipment.ShipmentAdditionalData = new ShipmentAdditionalData();
            shipment.ShipmentAdditionalData.ShipmentOrderNumber = shipmentOrder.OrderNumber;
        }
    }
}