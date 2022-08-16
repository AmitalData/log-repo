using System;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ShipmentRoutingLeg
    {
        public string LegHeader { get; set; }
        public string FromCountryCode { get; set; }
        public string FromPortCode { get; set; }
        public string FromPortName { get; set; }
        public string ToCountryCode { get; set; }
        public string ToPortCode { get; set; }
        public string ToPortName { get; set; }
        public string TransportMode { get; set; }
    }

    public class MainRouteInformation : ShipmentRoutingLeg
    {
        public string PortOfLoading { get; set; }
        public string Gateway { get; set; }
        public string PortOfDischarge { get; set; }
        public string Destination { get; set; }
        public string OBL { get; set; }
        public string MAWB { get; set; }
        public string TransitTime { get; set; }
    }

    public class PreOnCarriageLeg : ShipmentRoutingLeg
    {
        public string Carrier { get; set; }
        public string CarrierNumber { get; set; }
        public string VesselName { get; set; }
    }

    public class MainCarriageLeg : ShipmentRoutingLeg
    {
        public string ShippingLine { get; set; }
        public string Airline { get; set; }
        public string VoyageNumber { get; set; }
        public string FlightNumber { get; set; }
        public string VesselName { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string Gateway { get; set; }
    }
}
