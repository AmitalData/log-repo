using System;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.ShipmentOrderModule.Data.EntityPOCOs;

namespace Logitude.ShipmentOrderModule.BL.EntityQueryServices
{
    public partial class ShipmentOrderQueryService
    {
        public ShipmentOrderPM GetSinglePM(string id, int tenant)
        {
            return context.ShipmentOrders.Where(a => a.Tenant == tenant && a.Id == id).ToList().Select(a => MapPocoToPM(a)).FirstOrDefault();
        }

        public ShipmentOrderPM GetSinglePMByOrderNumber(string orderNumber, int tenant)
        {
            return context.ShipmentOrders.Where(a => a.Tenant == tenant && a.OrderNumber == orderNumber).ToList().Select(a => MapPocoToPM(a)).FirstOrDefault();
        }

        public string GetIdByOrderNumber(string orderNumber, int tenant)
        {
            return context.ShipmentOrders.Where(a => a.Tenant == tenant && a.OrderNumber == orderNumber).Select(a => a.Id).FirstOrDefault();
        }

        public List<ShipmentOrderPM> GetConnectedShipmentOrders(string shipmentNumber, int tenant)
        {
            return context.ShipmentOrders.Where(a => a.Tenant == tenant && a.ShipmentNumber == shipmentNumber).ToList().Select(a => MapPocoToPM(a)).ToList();
        }

        private static ShipmentOrderPM MapPocoToPM(ShipmentOrder shipmentOrder)
        {
            return new ShipmentOrderPM
            {

                Id = shipmentOrder.Id,
                Tenant = shipmentOrder.Tenant,
                CreateDate = shipmentOrder.CreateDate,
                CreatedByUserId = shipmentOrder.CreatedByUserId,
                UpdateDate = shipmentOrder.UpdateDate,
                UpdatedByUserId = shipmentOrder.UpdatedByUserId,
                SearchFields = shipmentOrder.SearchFields,
                OrderNumber = shipmentOrder.OrderNumber,
                TransportModeId = shipmentOrder.TransportModeId,
                ConsigneeId = shipmentOrder.ConsigneeId,
                ShipperId = shipmentOrder.ShipperId,
                AgentId = shipmentOrder.AgentId,
                IncotermId = shipmentOrder.IncotermId,
                AccountManagerId = shipmentOrder.AccountManagerId,
                PONumber = shipmentOrder.PONumber,
                DescriptionOfGoods = shipmentOrder.DescriptionOfGoods,
                House = shipmentOrder.House,
                VesselId = shipmentOrder.VesselId,
                CustomsAgentId = shipmentOrder.CustomsAgentId,
                SpecialServicesTypeId = shipmentOrder.SpecialServicesTypeId,
                BookingConfirmationDate = shipmentOrder.BookingConfirmationDate,
                CustomerReferences = shipmentOrder.CustomerReferences,
                IsReadyForPickup = shipmentOrder.IsReadyForPickup,
                PickupEstimatedDateTime = shipmentOrder.PickupEstimatedDateTime,
                PickupActualDateTime = shipmentOrder.PickupActualDateTime,
                ForwarderId = shipmentOrder.ForwarderId,
                Master = shipmentOrder.Master,
                ATA = shipmentOrder.ATA,
                ATD = shipmentOrder.ATD,
                ETD = shipmentOrder.ETD,
                ETA = shipmentOrder.ETA,
                ShipmentNumber = shipmentOrder.ShipmentNumber,
                SupplyDateTime = shipmentOrder.SupplyDateTime,
                OriginPortId = shipmentOrder.OriginPortId,
                DestinationPortId = shipmentOrder.DestinationPortId,
                GatewayId = shipmentOrder.GatewayId,
                CasualImporterName = shipmentOrder.CasualImporterName,
                CasualSupplierName = shipmentOrder.CasualSupplierName,
                ShipmentLevelCode = shipmentOrder.ShipmentLevelCode,
                PODate = shipmentOrder.PODate,
                BookingConfirmationNumber = shipmentOrder.BookingConfirmationNumber,
                DirectionId = shipmentOrder.DirectionId,
                CarrierNumber = shipmentOrder.CarrierNumber,
                CarrierId = shipmentOrder.CarrierId,
                IsCancelled = shipmentOrder.IsCancelled,
                SecurityKey = shipmentOrder.SecurityKey,
                Quantity = shipmentOrder.Quantity,
                Volume = shipmentOrder.Volume,
                GrossWeight = shipmentOrder.GrossWeight,
                CustomerId = shipmentOrder.CustomerId,
                LastExceptionDate = shipmentOrder.LastExceptionDate,
                LastExceptionDescription = shipmentOrder.LastExceptionDescription,
                OnHandDate = shipmentOrder.OnHandDate,
                OnHandNumber = shipmentOrder.OnHandNumber,
                IsOperationalClosed = shipmentOrder.IsOperationalClosed,
            };
        }

    }
}
