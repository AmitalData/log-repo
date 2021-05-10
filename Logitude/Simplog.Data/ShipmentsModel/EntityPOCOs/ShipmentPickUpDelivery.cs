using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentPickUpDelivery
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ATA { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public string CarrierId { get; set; }
        public string CarrierNumber { get; set; }
        public string Notes { get; set; }
        public string PickUpDeliveryNumber { get; set; }
        public string TruckNumber { get; set; }
        public string Driver { get; set; }
        public string TrailerNumber { get; set; }
        public string EmptyPickupContainerPartnerId { get; set; }
        public string EmptyPickupDepotReference  { get; set; }
        public string EmptyDeliveryContainerPartnerId { get; set; }
        public string EmptyDeliveryDepotReference { get; set; }
        public string PickUpDeliveryTypeCode { get; set; }
        public bool FullResponsibility { get; set; }
        public string PickUpDeliveryFromTypeCode { get; set; }
        public string FromPortId { get; set; }
        public string FromPartnerCardId { get; set; }
        public string FromAddressId { get; set; }
        public string FromAddressCity { get; set; }
        public string FromAddressZipCode { get; set; }
        public string FromAddressCountryId { get; set; }
        public string FromAddress { get; set; }             //250
        public string PickUpDeliveryToTypeCode { get; set; }
        public string ToPortId { get; set; }
        public string ToPartnerCardId { get; set; }
        public string ToAddressId { get; set; }
        public string ToAddressCity { get; set; }
        public string ToAddressZipCode { get; set; }
        public string ToAddressCountryId { get; set; }
        public string ToAddress { get; set; }               //250
        public string ParentPickUpDeliveryId { get; set; }
        public int? ChildPickUpIndex { get; set; }
        public int? ChildDeliveryIndex { get; set; }

        [ForeignKey("TransportModeCode")]
        public PickUpDeliveryTransportMode TransportMode { get; set; }
        public string TransportModeCode { get; set; }

        [ForeignKey("FromAddressCountryId")]
        public virtual Country FromAddressCountry { get; set; }

        [ForeignKey("ToAddressCountryId")]
        public virtual Country ToAddressCountry { get; set; }

        [ForeignKey("ToPartnerCardId")]
        public virtual Card ToPartnerCard { get; set; }
         
        [ForeignKey("ToPortId")]
        public virtual Port ToPort { get; set; }    
        
        [ForeignKey("FromPortId")]
        public virtual Port FromPort { get; set; }

        [ForeignKey("FromPartnerCardId")]
        public virtual Card FromPartnerCard { get; set; }

        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }

        [ForeignKey("CarrierId")]
        public Card CarrierCard { get; set; }
       
        [ForeignKey("PickUpDeliveryFromTypeCode")]
        public PickUpDeliveryFromToType PickUpDeliveryFromType { get; set; }
       
        [ForeignKey("PickUpDeliveryToTypeCode")]
        public PickUpDeliveryFromToType PickUpDeliveryToType { get; set; }
       
        [ForeignKey("PickUpDeliveryTypeCode")]
        public PickUpDeliveryType PickUpDeliveryType { get; set; }
      
        [ForeignKey("EmptyPickupContainerPartnerId")]
        public virtual Card EmptyPickupContainerPartner { get; set; }

        [ForeignKey("EmptyDeliveryContainerPartnerId")]
        public virtual Card EmptyDeliveryContainerPartner { get; set; }

        [ForeignKey("FromAddressId")]
        public Address FromAddressObj { get; set; }
      
        [ForeignKey("ToAddressId")]
        public Address ToAddressObj { get; set; }

        [ForeignKey("ParentPickUpDeliveryId")]
        public ShipmentPickUpDelivery ParentPickUpDelivery { get; set; }
        public string StandaloneShipmentId { get; set; }

        [ForeignKey("StandaloneShipmentId")]
        public virtual Shipment StandaloneShipment { get; set; }
        public string StandaloneShipmentNumber { get; set; }
    }
}
