using System;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentOrderModule.BL.EntityQueryServices
{
    public partial class ShipmentOrderQueryService
    {
        public ShipmentOrderPM GetSinglePM(string id, int tenant)
        {
            return context.ShipmentOrders.Where(a => a.Tenant == tenant && a.Id == id).Select(a => new ShipmentOrderPM
            {

                Id = a.Id,
                Tenant = a.Tenant,
                CreateDate = a.CreateDate,
                CreatedByUserId = a.CreatedByUserId,
                UpdateDate = a.UpdateDate,
                UpdatedByUserId = a.UpdatedByUserId,
                SearchFields = a.SearchFields,
                OrderNumber = a.OrderNumber,
                TransportModeId = a.TransportModeId,
                ConsigneeId = a.ConsigneeId,
                ShipperId = a.ShipperId,
                AgentId = a.AgentId,
                IncotermId = a.IncotermId,
                AccountManagerId = a.AccountManagerId,
                PONumber = a.PONumber,
                DescriptionOfGoods = a.DescriptionOfGoods,
                ShipmentTypeId = a.ShipmentTypeId,
                House = a.House,
                VesselId = a.VesselId,
                CustomsAgentId = a.CustomsAgentId,
                SpecialServicesTypeId = a.SpecialServicesTypeId,
                BookingConfirmationDate = a.BookingConfirmationDate,
                CustomerReferences = a.CustomerReferences,
                IsReadyForPickup = a.IsReadyForPickup,
                PickupEstimatedDateTime = a.PickupEstimatedDateTime,
                PickupActualDateTime = a.PickupActualDateTime,
                ForwarderId = a.ForwarderId,
                Master = a.Master,
                ATA = a.ATA,
                ATD = a.ATD,
                ETD = a.ETD,
                ETA = a.ETA,

            }).FirstOrDefault();
        }

        public ShipmentOrderPM GetSinglePMByOrderNumber(string orderNumber, int tenant)
        {
            return context.ShipmentOrders.Where(a => a.Tenant == tenant && a.OrderNumber == orderNumber).Select(a => new ShipmentOrderPM
            {

                Id = a.Id,
                Tenant = a.Tenant,
                CreateDate = a.CreateDate,
                CreatedByUserId = a.CreatedByUserId,
                UpdateDate = a.UpdateDate,
                UpdatedByUserId = a.UpdatedByUserId,
                SearchFields = a.SearchFields,
                OrderNumber = a.OrderNumber,
                TransportModeId = a.TransportModeId,
                ConsigneeId = a.ConsigneeId,
                ShipperId = a.ShipperId,
                AgentId = a.AgentId,
                IncotermId = a.IncotermId,
                AccountManagerId = a.AccountManagerId,
                PONumber = a.PONumber,
                DescriptionOfGoods = a.DescriptionOfGoods,
                ShipmentTypeId = a.ShipmentTypeId,
                House = a.House,
                VesselId = a.VesselId,
                CustomsAgentId = a.CustomsAgentId,
                SpecialServicesTypeId = a.SpecialServicesTypeId,
                BookingConfirmationDate = a.BookingConfirmationDate,
                CustomerReferences = a.CustomerReferences,
                IsReadyForPickup = a.IsReadyForPickup,
                PickupEstimatedDateTime = a.PickupEstimatedDateTime,
                PickupActualDateTime = a.PickupActualDateTime,
                ForwarderId = a.ForwarderId,
                Master = a.Master,
                ATA = a.ATA,
                ATD = a.ATD,
                ETD = a.ETD,
                ETA = a.ETA,

            }).FirstOrDefault();
        }



        public string GetIdByOrderNumber(string orderNumber)
        {
            return context.ShipmentOrders.FirstOrDefault(a => a.OrderNumber == orderNumber)?.Id;
        }

    }
}
