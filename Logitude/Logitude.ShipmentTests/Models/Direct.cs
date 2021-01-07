using System;
using System.Collections.Generic;

namespace Logitude.ShipmentTests.Models
{
    public class Direct
    {
        public string Id { get; set; }
        public Card Agent { get; set; }
        public Direction Direction { get; set; }
        public TransportMode TransportMode { get; set; }
        public ShipmentType ShipmentType { get; set; }
        public Card Shipper { get; set; }
        public string ShipperReference1 { get; set; }
        public string ShipperReference2 { get; set; }
        public WeightUnit GrossWeightUnit { get; set; }
        public WeightUnit ChargeableWeightUnit { get; set; }
        public VolumeUnit VolumeUnit { get; set; }
        public Incoterm Incoterm { get; set; }
        public Card MainCarriageCarrier { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public List<OceanOrInlandPackage> OceanOrInlandPackages { get; set; }
        public List<MainCarriageLeg> MainCarriageLegs { get; set; }
    }
}