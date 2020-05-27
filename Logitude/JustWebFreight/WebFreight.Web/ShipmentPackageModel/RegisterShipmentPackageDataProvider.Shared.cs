using System;
using System.Collections.Generic;
using System.Linq;
using WebFreight.Web.DataProviders;
//using System.Web;

namespace WebFreight.Web.ShipmentPackageModel
{
    public class RegisterShipmentPackageDataProvider : BaseDataProvider
    {

        public string TenantName { get; set; }
        public string Partner { get; set; }

        public List<RegisterShipmentPackageRecord> Records { get; set; }

        public class RegisterShipmentPackageRecord
        {
            public string ShipmentNumber { get; set; }
            public string ETD { get; set; } //main carriage ETD
            public string CustomerName { get; set; }
            public string POL { get; set; } //main carriage from
            public string POD { get; set; } //main carriage final destination to
            public string ShipperName { get; set; }
            public string ShippingLine { get; set; }
            public string ContainerNumber { get; set; } //all the containers numbers + types
            public string Type { get; set; } //FCL / LCL
            public string ETA { get; set; } //final destination ETA
            public string MasterNumber { get; set; }
            public string Vessel_Voyage { get; set; }//Vessel / main carriage carrier number
            public string Status { get; set; }
            public string AgentReference { get; set; } //agent ref1, ref2
            public string OurInvoice { get; set; } //invoices list from the shipment that has the customer 
            public string AgentName { get; set; }
            public string PartnerName { get; set; }
            public string VesselName { get; set; }
            public string CustomerReference { get; set; } //agent ref1, ref2

            public string ShipmentField1 { get; set; }
            public string ShipmentField2 { get; set; }
            public string ShipmentField3 { get; set; }
            public string ShipmentField4 { get; set; }
            public string ShipmentField5 { get; set; }
            public string ShipmentField6 { get; set; }
            public string ShipmentField7 { get; set; }
            public string ShipmentField8 { get; set; }
            public string ShipmentField9 { get; set; }
            public string ShipmentField10 { get; set; }
            public string ShipmentField11 { get; set; }
            public string ShipmentField12 { get; set; }
            public string ShipmentField13 { get; set; }
            public string ShipmentField14 { get; set; }
            public string ShipmentField15 { get; set; }
            public string ShipmentField16 { get; set; }
            public string ShipmentField17 { get; set; }
            public string ShipmentField18 { get; set; }
            public string ShipmentField19 { get; set; }
            public string ShipmentField20 { get; set; }
            public string ShipmentField21 { get; set; }
            public string ShipmentField22 { get; set; }
            public string ShipmentField23 { get; set; }
            public string ShipmentField24 { get; set; }
            public string ShipmentField25 { get; set; }
            public string ShipmentField26 { get; set; }
            public string ShipmentField27 { get; set; }
            public string ShipmentField28 { get; set; }
            public string ShipmentField29 { get; set; }
            public string ShipmentField30 { get; set; }
            public string ShipmentField31 { get; set; }
            public string ShipmentField32 { get; set; }
            public string ShipmentField33 { get; set; }
            public string ShipmentField34 { get; set; }
            public string ShipmentField35 { get; set; }
            public string ShipmentField36 { get; set; }
            public string ShipmentField37 { get; set; }
            public string ShipmentField38 { get; set; }
            public string ShipmentField39 { get; set; }
            public string ShipmentField40 { get; set; }

            public bool IsCancelled { get; set; }
            public DateTime? ETDAsDateTime { get; set; }
            public DateTime? ETAAsDateTime { get; set; }
            public string ShipmentDescriptionofGoods { get; set; }

            public string House { get; set; }
            public string ConsigneeName { get; set; }
            public string FinalDestination { get; set; } // ( last "delivery to" if exist or last "transshipment/main carriage to", same logic we have in shipping declaration data provider)
            public string ShipmentPackageReference1 { get; set; }
            public string ShipmentPackageReference2 { get; set; }
            public string ShipmentPackageReference3 { get; set; }
            public string ShipmentPackageReference4 { get; set; }
            public string ContainerTypeName { get; set; }
            public string OnCarriageTo { get; set; }
            public DateTime? ATD { get; set; }
            public DateTime? ATA { get; set; }
            public DateTime? OnCarriageATD { get; set; }
            public DateTime? OnCarriageATA { get; set; }
            public string ContainerNotes { get; set; }
            public string GrossWeight { get; set; }
            public bool? Flagged { get; set; }
            public double? GrossWeightAsDouble { get; set; }

            public string RailTo { get; set; }
            public DateTime? RailATD { get; set; }
            public DateTime? RailATA { get; set; }

            public string Ramp { get; set; } //will be always the on-carriage to
            public DateTime? ATDRamp { get; set; } //will be taken from the container level if on-carriage is split , otherwise from the on-carriage level
            public DateTime? ATARamp { get; set; } //same as the ATD
            public DateTime? ATADoor { get; set; } //if the container is connected to a delivery take the ATA from the delivery, if not try to locate the container number in the deliveries and take the date

            public DateTime? LastETA { get; set; } //: datetime : will take ETA from transshipment 3 if exist, if not from trans. 2, if not from trans. 1 and then from the main carriage if no transshipments.
            public DateTime? LastATA { get; set; } // :datetime : same logic as above, but for ATA
            public string LastVessel { get; set; }  //: string : same logic but for the vessel name

            public string BookingConfirmationNumber { get; set; }

            public string ContainerPackageItemsDescription { get; set; }
            public string ContainerPackageItemsValue { get; set; }
            public string ContainerPackageItemsQuantity { get; set; }

            public DateTime? ETARamp { get; set; }
            public DateTime? OnCarriageETA { get; set; }

            public string ShipperCityAndCountry { get; set; }
            public string ConsigneeCityAndCountry { get; set; }
            public string Incoterm { get; set; }
            public double? Volume { get; set; }
            public DateTime? PickupATD { get; set; }
            public DateTime? PickupATA { get; set; }
            public DateTime? DeliveryATD { get; set; }
            public DateTime? DeliveryATA { get; set; }
            public DateTime? DeliveryETA { get; set; }

            public double? ContainerVolume { get; set; }
            public string ShippingLineSCAC { get; set; }
            public string IncotermCode { get; set; }

            public int? TotalNumberOfContainers { get; set; }
        }
    }
}
