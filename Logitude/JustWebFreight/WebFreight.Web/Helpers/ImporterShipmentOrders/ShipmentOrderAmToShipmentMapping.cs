using Logitude.BL.CommonDataModel.CodePropertiesMapping;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.ShipmentOrderModule.Def.EntityAMs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;

namespace WebFreight.Web.Helpers.ImporterShipmentOrders
{
    public class ShipmentOrderAmToShipmentMapping
    {
        private readonly int tenant;

        public ShipmentOrderAmToShipmentMapping(int tenant)
        {
            this.tenant = tenant;
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

            GetShipperId(shipmentOrder, shipment);
            if (!string.IsNullOrEmpty(shipmentOrder.ShipperName))
            {
                shipment.ShipperName = shipmentOrder.ShipperName;
            }

            GetIncotermId(shipmentOrder, shipment);

            return shipment;
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
    }
}