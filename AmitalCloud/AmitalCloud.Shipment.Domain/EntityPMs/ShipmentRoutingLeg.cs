using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class ShipmentRoutingLeg : EntityPM
    {
        public string Title { get; set; }
        public string LegHeader { get; set; }
        public string FromPort { get; set; }
        public string ToPort { get; set; }
        public string TransportMode { get; set; }
        public string Carrier { get; set; }
        public string CarrierLabel { get; set; }
        public string CarrierNumber { get; set; }
        public string CarrierNumberLabel { get; set; }
        public string VesselName { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string DepartureDateType { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ArrivalDateType { get; set; }
    }

    public partial class MainRouteInformation : ShipmentRoutingLeg
    {
        public string LoadingPort { get; set; }
        public string LoadingPortLabel { get; set; }
        public string DischargePort { get; set; }
        public string DischargePortLabel { get; set; }
        public string Master { get; set; }
        public string MasterLabel { get; set; }
        public TransitTime TransitTime { get; set; }
    }

    public partial class TransitTime
    {
        public DateTime? LoadingDate { get; set; }
        public DateTime? DichargeDate { get; set; }
    }
}
