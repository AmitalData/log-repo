using System;
using System.Collections.Generic;

namespace AmitalCloud.Shipment.Domain.EntityPMs
{
    public class ShipmentAdditionalFields
    {
        public string ShipmentId { get; set; }
        public int Tenant { get; set; }

        public string ShipperAddressId { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperCountryCode { get; set; }
        public string ConsigneeAddressId { get; set; }
        public string ConsigneeCity { get; set; }
        public string ConsigneeCountryCode { get; set; }

        public List<ShipmentPickUpDeliveryPM> ShipmentPickUpDeliveries { get; set; }

        public List<ShipmentPackagePM> ShipmentPackages { get; set; }
        public List<ShipmentOrderPackagePM> ShipmentOrderPackages { get; set; }

        public string FromPortId { get; set; }
        public string MainCarriageFromPortStateCode { get; set; }
        public string ToPortId { get; set; }
        public string MainCarriageToPortStateCode { get; set; }

        public DateTime? Transshipment1ATD { get; set; }
        public DateTime? Transshipment1ATA { get; set; }
        public DateTime? Transshipment1ETD { get; set; }
        public DateTime? Transshipment1ETA { get; set; }
        public string Transshipment1FromPortId { get; set; }
        public string Transshipment1FromPortName { get; set; }
        public string Transshipment1FromPortCode { get; set; }
        public string Transshipment1FromPortStateCode { get; set; }
        public string Transshipment1FromPortCountryCode { get; set; }
        public string Transshipment1ToPortId { get; set; }
        public string Transshipment1ToPortName { get; set; }
        public string Transshipment1ToPortCode { get; set; }
        public string Transshipment1ToPortStateCode { get; set; }
        public string Transshipment1ToPortCountryCode { get; set; }

        public DateTime? Transshipment2ATD { get; set; }
        public DateTime? Transshipment2ATA { get; set; }
        public DateTime? Transshipment2ETD { get; set; }
        public DateTime? Transshipment2ETA { get; set; }
        public string Transshipment2FromPortId { get; set; }
        public string Transshipment2FromPortName { get; set; }
        public string Transshipment2FromPortCode { get; set; }
        public string Transshipment2FromPortStateCode { get; set; }
        public string Transshipment2FromPortCountryCode { get; set; }
        public string Transshipment2ToPortId { get; set; }
        public string Transshipment2ToPortName { get; set; }
        public string Transshipment2ToPortCode { get; set; }
        public string Transshipment2ToPortStateCode { get; set; }
        public string Transshipment2ToPortCountryCode { get; set; }

        public DateTime? Transshipment3ATD { get; set; }
        public DateTime? Transshipment3ATA { get; set; }
        public DateTime? Transshipment3ETD { get; set; }
        public DateTime? Transshipment3ETA { get; set; }
        public string Transshipment3FromPortId { get; set; }
        public string Transshipment3FromPortName { get; set; }
        public string Transshipment3FromPortCode { get; set; }
        public string Transshipment3FromPortStateCode { get; set; }
        public string Transshipment3FromPortCountryCode { get; set; }
        public string Transshipment3ToPortId { get; set; }
        public string Transshipment3ToPortName { get; set; }
        public string Transshipment3ToPortCode { get; set; }
        public string Transshipment3ToPortStateCode { get; set; }
        public string Transshipment3ToPortCountryCode { get; set; }

        public bool HasPreCarriage { get; set; }
        public DateTime? PreCarriageETD { get; set; }
        public DateTime? PreCarriageATD { get; set; }
        public DateTime? PreCarriageETA { get; set; }
        public DateTime? PreCarriageATA { get; set; }
        public string PreCarriageFromPortId { get; set; }
        public string PreCarriageFromPortCode { get; set; }
        public string PreCarriageFromPortName { get; set; }
        public string PreCarriageFromPortCountryCode { get; set; }
        public string PreCarriageToPortId { get; set; }
        public string PreCarriageToPortCode { get; set; }
        public string PreCarriageToPortName { get; set; }
        public string PreCarriageToPortCountryCode { get; set; }

        public bool HasOnCarriage { get; set; }
        public DateTime? OnCarriageETD { get; set; }
        public DateTime? OnCarriageATD { get; set; }
        public DateTime? OnCarriageETA { get; set; }
        public DateTime? OnCarriageATA { get; set; }
        public string OnCarriageFromPortId { get; set; }
        public string OnCarriageFromPortCode { get; set; }
        public string OnCarriageFromPortName { get; set; }
        public string OnCarriageFromPortCountryCode { get; set; }
        public string OnCarriageToPortId { get; set; }
        public string OnCarriageToPortCode { get; set; }
        public string OnCarriageToPortName { get; set; }
        public string OnCarriageToPortCountryCode { get; set; }
        public string ContainersNumbers { get; set; }
    }
}