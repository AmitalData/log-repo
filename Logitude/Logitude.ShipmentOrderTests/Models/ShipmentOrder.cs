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

        public int Tenant { get; set; }

        public string OrderNumber { get; set; }

        public ShipmentOrderCustomProperty TransportMode { get; set; }

        public ShipmentOrderCustomProperty Consignee { get; set; }

        public ShipmentOrderCustomProperty Shipper { get; set; }

        public ShipmentOrderCustomProperty Agent { get; set; }

        public ShipmentOrderCustomProperty Incoterm { get; set; }

        public ShipmentOrderCustomProperty AccountManager { get; set; }

        public string PONumber { get; set; }

        public string Master { get; set; }

        public string House { get; set; }

        public ShipmentOrderCustomProperty Vessel { get; set; }

        public ShipmentOrderCustomProperty CustomsAgent { get; set; }

        public ShipmentOrderCustomProperty SpecialServicesType { get; set; }

        public ShipmentOrderCustomProperty Forwarder { get; set; }

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

        public ShipmentOrderCustomProperty Direction { get; set; }

        public string ShipmentNumber { get; set; }

        public DateTime? SupplyDateTime { get; set; }

        public ShipmentOrderCustomProperty OriginPort { get; set; }

        public ShipmentOrderCustomProperty DestinationPort { get; set; }

        public ShipmentOrderCustomProperty Gateway { get; set; }

        public string CasualImporterName { get; set; }

        public string CasualSupplierName { get; set; }

        public ShipmentOrderCustomProperty ShipmentLevel { get; set; }

        public DateTime? PODate { get; set; }

        public string BookingConfirmationNumber { get; set; }

        public string CarrierNumber { get; set; }
    }
}
