using System;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class ShipmentRoutingLeg
    {
        public string Title { get; set; }
        public string LegHeader { get; set; }
        public string FromCountryCode { get; set; }
        public string FromPortCode { get; set; }
        public string FromPortName { get; set; }
        public string ToCountryCode { get; set; }
        public string ToPortCode { get; set; }
        public string ToPortName { get; set; }
        public string TransportMode { get; set; }
        public string Carrier { get; set; }
        public string CarrierLabel { get; set; }
        public string CarrierNumber { get; set; }
        public string CarrierNumberLabel { get; set; }
        public string VesselName { get; set; }
        public DateTime? DepartureDate { get; set; }
    }

    public class MainRouteInformation : ShipmentRoutingLeg
    {
        public string LoadingPort { get; set; }
        public string LoadingPortLabel { get; set; }
        public string DischargePort { get; set; }
        public string DischargePortLabel { get; set; }
        public string Master { get; set; }
        public string MasterLabel { get; set; }
        public string TransitTime { get; set; }
    }
}
