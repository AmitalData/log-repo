using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentOrderTests.Models
{
    public class ShipmentOrder
    {

        public string Id { get; set; }

        public string OrderNumber { get; set; }

        public TransportMode TransportMode { get; set; }

        public Card Consignee { get; set; }

        public Card Shipper { get; set; }

        public Card Agent { get; set; }

        public Incoterm Incoterm { get; set; }

        public User AccountManager { get; set; }

        public string PONumber { get; set; }

        public string Master { get; set; }

        public string House { get; set; }

        public Vessel Vessel { get; set; }

        public Card CustomsAgent { get; set; }

        public SpecialServicesType SpecialServicesType { get; set; }

        public Card Forwarder { get; set; }

        public bool IsReadyForPickup { get; set; }

        public string DescriptionOfGoods { get; set; }

        public string TransportModeName { get; set; }

        public string CustomerReferences { get; set; }

        public DateTime? PickupEstimatedDateTime { get; set; }

        public DateTime? PickupActualDateTime { get; set; }

        public DateTime? BookingConfirmationDate { get; set; }

        public DateTime? ETD { get; set; }

        public DateTime? ETA { get; set; }

        public DateTime? ATD { get; set; }

        public DateTime? ATA { get; set; }

        public Direction Direction { get; set; }

        public string ShipmentNumber { get; set; }

        public DateTime? SupplyDateTime { get; set; }

        public Port OriginPort { get; set; }

        public Port DestinationPort { get; set; }

        public Port Gateway { get; set; }

        public string CasualImporterName { get; set; }

        public string CasualSupplierName { get; set; }

        public ShipmentLevel ShipmentLevel { get; set; }

        public DateTime? PODate { get; set; }

        public string BookingConfirmationNumber { get; set; }

        public string CarrierNumber { get; set; }

        public Card Carrier { get; set; }

        public DateTime CreateDate { get; set; }

        public string SecurityKey { get; set; }
    }
}
