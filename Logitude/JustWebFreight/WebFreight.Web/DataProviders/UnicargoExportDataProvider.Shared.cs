using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class UnicargoExportDataProvider : BaseDataProvider
    {
        public List<UnicargoExport> Shipments { get; set; }
    }

    public class UnicargoExport
    {
        public string ShipmentNumber { get; set; }
        public string House { get; set; }
        public string Incoterms { get; set; }
        public string MainHarmonize { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string TransportMode { get; set; }
        public string Direction { get; set; }
        public string FileNumber { get; set; }
        public string ShipmentLevel { get; set; }
        public string AdditionalAirwayBill { get; set; }
        public string OpStatus { get; set; }
        public DateTime? CargoReadyDate { get; set; }
        public string LFD { get; set; }
        public DateTime? AvailableDate { get; set; }
        public string Openedby { get; set; }
        public string Salesman { get; set; }
        public string AccountManager { get; set; }
        public string Department { get; set; }
        public string Branch { get; set; }
        public string SpecialServiceType { get; set; }
        public double? ValueofGoods { get; set; }
        public string Comments { get; set; }
        public string PaymentStatus { get; set; }
        public string LeadType { get; set; }
        public string Handler { get; set; }
        public DateTime? CreateDate { get; set; }
        public string PickupRef { get; set; }
        public string Shipper { get; set; }
        public string Consignee { get; set; }
        public string Notify1 { get; set; }
        public string Notify2 { get; set; }
        public string CoLoader { get; set; }
        public string FreightForwarder { get; set; }
        public string Consolidator { get; set; }
        public string Customer { get; set; }
        public string ShippernotExporter { get; set; }
        public string Agent { get; set; }
        public string ReleasingAgent { get; set; }
        public string PackageType { get; set; }
        public string TotalPieces { get; set; }
        public double? Volume { get; set; }
        public double? GrossWeight { get; set; }
        public double? VolumetricWeight { get; set; }
        public double? Ratio { get; set; }
        public string ShipperSeal { get; set; }
        public string DescriptionofGoods { get; set; }
        public string ContinerNumber { get; set; }
        public string PickupFromPartner { get; set; }
        public string PickupFromPartnerAddress { get; set; }
        public string PickupToPartner { get; set; }
        public string PickupToPartnerAddress { get; set; }
        public DateTime? PickupExpectedDeparture { get; set; }
        public DateTime? PickupExpectedArrival { get; set; }
        public DateTime? PickupActualDeparture { get; set; }
        public string PickupToPort { get; set; }
        public DateTime? PickupActualArrival { get; set; }
        public string MainCarriageLeg1LoadingPort { get; set; }
        public string MainCarriageLeg1ViaPort1  { get; set; }
        public string MainCarriageLeg1ViaPort2 { get; set; }
        public string MainMainCarriageLeg1ViaPort3 { get; set; }
        public string MainCarriageLeg1DischargePort { get; set; }
        public string MainCarriageLeg1ShippingLine { get; set; }
        public string MainCarriageLeg1VoyageNo { get; set; }
        public string MainCarriageLeg1OBL { get; set; }
        public DateTime? MainCarriageLeg1OBLDate { get; set; }
        public DateTime? MainCarriageLeg1CutoffDate { get; set; }
        public string MainCarriageLeg1Vessel { get; set; }
        public DateTime? MainCarriageLeg1ETD { get; set; }
        public DateTime? MainCarriageLeg1ETA { get; set; }
        public DateTime? MainCarriageLeg1ATD { get; set; }
        public DateTime? MainCarriageLeg1ATA { get; set; }
        public string Transshipment1ShippingLine { get; set; }
        public string Transshipment1VoyageNo { get; set; }
        public string Transshipment1OBL { get; set; }
        public string Transshipment1Vessel { get; set; }
        public DateTime? Transshipment1ETD { get; set; }
        public DateTime? Transshipment1ETA { get; set; }
        public DateTime? Transshipment1ATD { get; set; }
        public DateTime? Transshipment1ATA { get; set; }
        public string DeliveryFromPort { get; set; }
        public string DeliveryToPartner { get; set; }
        public string DeliveryToPatnerAddress { get; set; }
        public string DeliveryFromPartner { get; set; }
        public string DeliveryTransportMode { get; set; }
        public DateTime? DeliveryExpectedDeparture { get; set; }
        public DateTime? DeliveryExpectedArrival { get; set; }
        public DateTime? DeliveryActualDeparture { get; set; }
        public DateTime? DeliveryActualArrival { get; set; }




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

    }
}