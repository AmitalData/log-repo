using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityLists
{
    public class ContainerFollowUpList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactName { get; set; }
        public string CarrierId { get; set; }
        public string CarrierName { get; set; }
        public string VesselName { get; set; }
        public string ShipmentNotes { get; set; }
        public string ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string ConsigneeId { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeReference { get; set; }
        public string LongMaster { get; set; }
        public string House { get; set; }
        public string DirectionId { get; set; }
        public string DirectionName { get; set; }
        public string TransportModeId { get; set; }
        public string TransportModeName { get; set; }
        public string ShipmentType { get; set; }
        public string ShipmentTypeId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string ShipmentLevelName { get; set; }
        public string StatusId { get; set; }
        public string SearchFields { get; set; }
        public bool IsCancelled { get; set; }
        public string ContainerTypeName { get; set; }
        public string ContainerNumber { get; set; }
        public string ShipperSeal { get; set; }
        public double? Volume { get; set; }
        public bool IsDangerous { get; set; }
        public string Description { get; set; }
        public string MarksAndNumbers { get; set; }

        public bool IsDeliveryFU { get; set; }
        public string DeliveryId { get; set; }
        public DateTime? DeliveryETD { get; set; }
        public DateTime? DeliveryATD { get; set; }
        public DateTime? DeliveryETA { get; set; }
        public DateTime? DeliveryATA { get; set; }
        public DateTime? DeliveryDeparture { get; set; }
        public DateTime? DeliveryArrival { get; set; }
        public string DeliveryFrom { get; set; }
        public string DeliveryTo { get; set; }
        public string DeliveryTransportModeCode { get; set; }
        public string DeliveryTransportModeName { get; set; }

        public bool IsEmptyContainerReturnFU { get; set; }
        public string EmptyContainerReturnId { get; set; }
        public DateTime? EmptyContainerReturnETD { get; set; }
        public DateTime? EmptyContainerReturnATD { get; set; }
        public DateTime? EmptyContainerReturnETA { get; set; }
        public DateTime? EmptyContainerReturnATA { get; set; }
        public DateTime? ReturnDeparture { get; set; }
        public DateTime? ReturnArrival { get; set; }
        public string EmptyContainerReturnFrom { get; set; }
        public string EmptyContainerReturnTo { get; set; }
        public string ECRTransportModeCode { get; set; }
        public string ECRTransportModeName { get; set; }

    }
}
