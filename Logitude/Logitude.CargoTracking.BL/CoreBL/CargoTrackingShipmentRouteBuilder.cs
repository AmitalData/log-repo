using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.ShipmentOrderModule.BL.EntityQueryServices;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.CargoTracking.BL.EntityQueryServices
{
    public class CargoTrackingShipmentRouteBuilder
    {
        private const string InlandTransportMode = "I";
        private const string WarehouseTransportMode = "W";
        private const string PortTypeCode = "PORT";
        private const string PartnerTypeCode = "PART";
        private const string ExportShipmentDirection = "E";
        private ShipmentOrderPM shipmentOrderPM;
        private ShipmentPM shipmentPM;
		ShipmentPM forwardingShipmentPM;
		private List<RoutingStep> routingSteps = new List<RoutingStep>();
        public CargoTrackingShipmentRouteBuilder(ShipmentOrderPM ShipmentOrderPM, ShipmentPM shipmentPM, ShipmentPM forwardingShipmentPM)
        {
            this.shipmentOrderPM = ShipmentOrderPM;
            this.shipmentPM = shipmentPM;
            this.forwardingShipmentPM = forwardingShipmentPM;
        }
        public List<RoutingStep> BuildRoute()
        {
            if (shipmentPM != null)
                BuildShipmentRoute(shipmentPM);

            if (shipmentOrderPM != null)
                BuildShipmentOrderRoute(shipmentOrderPM);

            return routingSteps;
        }

        private void BuildShipmentRoute(ShipmentPM shipmentPM)
        {
            BuildPickupRoutingSteps(shipmentPM);

            if (shipmentPM.DirectionId == ExportShipmentDirection)
            {
                BuildWarehouseLegStep(shipmentPM);
                BuildMainCarriageLegsSteps(shipmentPM);
            }
            else
            {
                BuildMainCarriageLegsSteps(shipmentPM);
                BuildWarehouseLegStep(shipmentPM);
            }

            BuildDeliveriesSteps(shipmentPM);
        }
        private void BuildShipmentOrderRoute(ShipmentOrderPM shipmentOrderPM)
        {
            if (shipmentOrderPM.GatewayId != null)
                BuildShipmentOrderGatewayRoute(shipmentOrderPM);
            else
                BuildShipmentOrderNonGatwayRoute(shipmentOrderPM);
        }
        private List<RoutingStep> BuildDeliveriesSteps(ShipmentPM shipmentPM)
        {
            var steps = new List<RoutingStep>();
            foreach (var delivery in shipmentPM.ShipmentDeliveries)
            {
                var step = new RoutingStep()
                {
                    TransportModeCode = InlandTransportMode,
                    FromPortLabel = GetFromPortLabelFromPickup(delivery),
                    ToPortLabel = GetToPortLabelFromPickup(delivery),
                    Description = delivery != null ? delivery.CarrierTypeName + " " + delivery.CarrierLocalName : null,
                    Type = "Delivery"
                };
                SetDeliveryDirections(delivery, step);
                steps.Add(step);
            }
            routingSteps.AddRange(steps);

            return steps;
        }

        private void SetDeliveryDirections(ShipmentDeliveryPM delivery, RoutingStep step)
        {
            var direction = BuildExportRouteDirection(delivery.ETD, "ETD");
            if (direction != null)
                step.Directions.Add(direction);

            direction = BuildImportRouteDirection(delivery.ETA, "ETA");
            if (direction != null)
                step.Directions.Add(direction);

            direction = BuildExportRouteDirection(delivery.ATD, "ATD");
            if (direction != null)
                step.Directions.Add(direction);

            direction = BuildImportRouteDirection(delivery.ATA, "ATA");
            if (direction != null)
                step.Directions.Add(direction);
        }

        private RouteDirection BuildExportRouteDirection(DateTime? fieldValue, string fieldName)
        {
            if (fieldValue != null && fieldValue.HasValue)
                return new RouteDirection(fieldValue.Value, fieldName, "out");
            return null;
        }
        private RouteDirection BuildImportRouteDirection(DateTime? fieldValue, string fieldName)
        {
            if (fieldValue != null && fieldValue.HasValue)
                return new RouteDirection(fieldValue.Value, fieldName, "in");
            return null;
        }
        private List<RoutingStep> BuildMainCarriageLegsSteps(ShipmentPM shipmentPM)
        {
            var steps = new List<RoutingStep>();
            foreach (var mainCarrigeLeg in shipmentPM.MainCarriageLegs)
            {
                var step = new RoutingStep()
                {
                    TransportModeCode = shipmentPM.TransportModeId,
                    FromPortLabel = mainCarrigeLeg.FromPortCode,
                    ToPortLabel = mainCarrigeLeg.ToPortCode,
                    Description = mainCarrigeLeg.CarrierName != null || mainCarrigeLeg.CarrierTypeName != null ? mainCarrigeLeg.CarrierTypeName +" "+ mainCarrigeLeg.CarrierName : null
                };
                SetMainCarriageLegDirections(mainCarrigeLeg, step);
                steps.Add(step);
            }
            routingSteps.AddRange(steps);

            return steps;
        }

        private void SetMainCarriageLegDirections(TransshipmentLeg mainCarrigeLeg, RoutingStep step)
        {
            var direction = BuildExportRouteDirection(mainCarrigeLeg.ETD, "ETD");
            if (direction != null)
                step.Directions.Add(direction);

            direction = BuildImportRouteDirection(mainCarrigeLeg.ETA, "ETA");
            if (direction != null)
                step.Directions.Add(direction);

            direction = BuildExportRouteDirection(mainCarrigeLeg.ATD, "ATD");
            if (direction != null)
                step.Directions.Add(direction);

            direction = BuildImportRouteDirection(mainCarrigeLeg.ATA, "ATA");
            if (direction != null)
                step.Directions.Add(direction);
        }

        private List<RoutingStep> BuildPickupRoutingSteps(ShipmentPM shipmentPM)
        {
            var steps = new List<RoutingStep>();

            if (shipmentPM == null)
                return steps;

            foreach (ShipmentPickUpPM pickup in shipmentPM.ShipmentPickUps)
            {
                var pickupRoute = new RoutingStep()
                {
                    TransportModeCode = InlandTransportMode,
                    FromPortLabel = GetFromPortLabelFromPickup(pickup),
                    ToPortLabel = GetToPortLabelFromPickup(pickup),
                    ToolTipFromPortLabel = GetToolTipFromPortLabelFromPickup(pickup),
                    ToolTipToPortLabel = GetToolTipToPortLabelFromPickup(pickup),
                    Type = "Pickup"
                };

                if (pickup.CarrierLocalName != null || pickup.CarrierTypeName != null )
                    pickupRoute.Description = pickup.CarrierTypeName +" "+ pickup.CarrierLocalName;

                SetPickupRouteDirections(pickup, pickupRoute);

                steps.Add(pickupRoute);
            }
            routingSteps.AddRange(steps);
            return steps;

        }

        private void BuildShipmentOrderNonGatwayRoute(ShipmentOrderPM shipmentOrderPM)
        {
            var shipmentOrderOriginRoute = new RoutingStep()
            {
                TransportModeCode = shipmentOrderPM.TransportModeId,
                Description = "MainCarriageLeg",
                FromPortLabel = shipmentOrderPM.OriginPortCode,
                ToPortLabel = shipmentOrderPM.DestinationPortCode
            };

            SetMainCarriageLegDirections(shipmentOrderPM, shipmentOrderOriginRoute);

            routingSteps.Add(shipmentOrderOriginRoute);
        }

        private void SetMainCarriageLegDirections(ShipmentOrderPM shipmentOrderPM, RoutingStep shipmentOrderOriginRoute)
        {
            var direction = BuildExportRouteDirection(shipmentOrderPM.ETD, "ETD");
            if (direction != null)
                shipmentOrderOriginRoute.Directions.Add(direction);

            direction = BuildImportRouteDirection(shipmentOrderPM.ETA, "ETA");
            if (direction != null)
                shipmentOrderOriginRoute.Directions.Add(direction);

            direction = BuildExportRouteDirection(shipmentOrderPM.ATD, "ATD");
            if (direction != null)
                shipmentOrderOriginRoute.Directions.Add(direction);

            direction = BuildImportRouteDirection(shipmentOrderPM.ATA, "ATA");
            if (direction != null)
                shipmentOrderOriginRoute.Directions.Add(direction);
        }

        private void BuildShipmentOrderGatewayRoute(ShipmentOrderPM shipmentOrderPM)
        {
            if(shipmentOrderPM.GatewayCode == shipmentOrderPM.OriginPortCode || shipmentOrderPM.GatewayCode == shipmentOrderPM.DestinationPortCode)
            {
                BuildShipmentOrderOriginRouteToDestinationRoute(shipmentOrderPM);
                return;
            }

            BuildShipmentOrderOriginRoute(shipmentOrderPM);
            BuildShipmentOrderDestinationRoute(shipmentOrderPM);
        }
        private void BuildShipmentOrderOriginRouteToDestinationRoute(ShipmentOrderPM shipmentOrderPM)
        {
            var step = new RoutingStep()
            {
                TransportModeCode = shipmentOrderPM.TransportModeId,
                Description = "MainCarriageLeg",
                FromPortLabel = shipmentOrderPM.OriginPortCode,
                ToPortLabel = shipmentOrderPM.DestinationPortCode
            };

            routingSteps.Add(step);
        }
        private void BuildShipmentOrderDestinationRoute(ShipmentOrderPM shipmentOrderPM)
        {
            var step = new RoutingStep()
            {
                TransportModeCode = shipmentOrderPM.TransportModeId,
                Description = "MainCarriageLeg 2",
                FromPortLabel = shipmentOrderPM.GatewayCode,
                ToPortLabel = shipmentOrderPM.DestinationPortCode
            };

            routingSteps.Add(step);
        }

        private void BuildShipmentOrderOriginRoute(ShipmentOrderPM shipmentOrderPM)
        {
            var step =  new RoutingStep()
            {
                TransportModeCode = shipmentOrderPM.TransportModeId,
                Description = "MainCarriageLeg 1",
                FromPortLabel = shipmentOrderPM.OriginPortCode,
                ToPortLabel = shipmentOrderPM.GatewayCode

            };
            routingSteps.Add(step);

        }


        private RoutingStep BuildWarehouseLegStep(ShipmentPM shipmentPM)
        {
            if (shipmentPM.WarehouseLegTerminalName == null)
                return null;

            var step = new RoutingStep()
            {
                TransportModeCode = WarehouseTransportMode,
                Description = shipmentPM.WarehouseLegRemarks ?? "WarehouseLeg",
                FromPortLabel = forwardingShipmentPM.WarehouseLegWarehouseName,
            };

            SetWarehouseLegDirections(shipmentPM, step);

            routingSteps.Add(step);

            return step;
        }

        private void SetWarehouseLegDirections(ShipmentPM shipmentPM, RoutingStep step)
        {
            if (shipmentPM.WarehouseLegExpectedEntryDate != null)
                step.Directions.Add(this.BuildExportRouteDirection(shipmentPM.WarehouseLegExpectedEntryDate, "ETD"));

            if (shipmentPM.WarehouseLegExpectedReleaseDate != null)
                step.Directions.Add(this.BuildImportRouteDirection(shipmentPM.WarehouseLegExpectedReleaseDate, "ETA"));

            if (shipmentPM.WarehouseLegActualEntryDate != null)
                step.Directions.Add(this.BuildExportRouteDirection(shipmentPM.WarehouseLegActualEntryDate, "ATA"));

            if (shipmentPM.WarehouseLegActualReleaseDate != null)
                step.Directions.Add(this.BuildImportRouteDirection(shipmentPM.WarehouseLegActualReleaseDate, "ATA"));
        }

        private void SetPickupRouteDirections(ShipmentPickUpPM pickup, RoutingStep pickupRoute)
        {
            pickupRoute.Directions = new List<RouteDirection>();

            var direction = BuildExportRouteDirection(pickup.ETD, "ETD");
            if (direction != null)
                pickupRoute.Directions.Add(direction);

            direction = BuildImportRouteDirection(pickup.ETA, "ETA");
            if (direction != null)
                pickupRoute.Directions.Add(direction);

            direction = BuildExportRouteDirection(pickup.ATD, "ATD");
            if (direction != null)
                pickupRoute.Directions.Add(direction);

            direction = BuildImportRouteDirection(pickup.ATA, "ATA");
            if (direction != null)
                pickupRoute.Directions.Add(direction);
        }
        private string GetFromPortLabelFromPickup(ShipmentPickUpPM pickup)
        {
            if (pickup.PickUpDeliveryFromTypeCode == PortTypeCode)
                return pickup.FromPortCode;
            if (pickup.PickUpDeliveryFromTypeCode == PartnerTypeCode)
                return pickup.FromLocation?.Split(' ')[0];
            return pickup.FromAddressCountryCode;
        }
        private string GetFromPortLabelFromPickup(ShipmentDeliveryPM delivery)
        {
            if (delivery.PickUpDeliveryFromTypeCode == PortTypeCode)
                return delivery.FromPortCode;
            if (delivery.PickUpDeliveryFromTypeCode == PartnerTypeCode)
                return delivery.FromLocation?.Split(' ')[0];
            return delivery.FromAddressCountryCode;
        }
        private string GetToolTipFromPortLabelFromPickup(ShipmentPickUpPM pickup)
        {
            if (pickup.PickUpDeliveryFromTypeCode == PartnerTypeCode)
                return "Partner: \n" + pickup.FromLocation?.Split('\r')[0];
            if (pickup.PickUpDeliveryFromTypeCode == "CASL")
                return "Address: \n" + (pickup.FromAddressCity_Dummy != null ? pickup.FromAddressCity_Dummy + ',' : "") + pickup.FromAddressCountryName;
            return null;
        }
        private string GetToolTipToPortLabelFromPickup(ShipmentPickUpPM pickup)
        {
            if (pickup.PickUpDeliveryToTypeCode == PartnerTypeCode)
                return "Partner: \n" + pickup.ToLocation?.Split('\r')[0];
            if (pickup.PickUpDeliveryToTypeCode == "CASL")
                return "Address: \n" + (pickup.ToAddressCity_Dummy != null ? pickup.ToAddressCity_Dummy + ',' : "") + pickup.ToAddressCountryName;
            return null;
        }
        private string GetToPortLabelFromPickup(ShipmentPickUpPM pickup)
        {
            if (pickup.PickUpDeliveryToTypeCode == PortTypeCode)
                return pickup.ToPortCode;
            if (pickup.PickUpDeliveryToTypeCode == PartnerTypeCode)
                return pickup.ToLocation?.Split(' ')[0];
            return pickup.ToAddressCountryCode;
        }
        private string GetToPortLabelFromPickup(ShipmentDeliveryPM delivery)
        {
            if (delivery.PickUpDeliveryToTypeCode == PortTypeCode)
                return delivery.ToPortCode;
            if (delivery.PickUpDeliveryToTypeCode == PartnerTypeCode)
                return delivery.ToLocation?.Split(' ')[0];
            return delivery.ToAddressCountryCode;
        }
    }

}