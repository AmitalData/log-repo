using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataProviders
{
    public class ShipmentDetailsDataProvider : BaseDataProvider
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<ShipmentDetals> Shipments { get; set; }
    }

    public class ShipmentDetals
    {
        public string Direction { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentLevel { get; set; }
        public string Shipper { get; set; }
        public string BUShipper { get; set; }
        public string ShipperRef1 { get; set; }
        public string ShipperRef2 { get; set; }
        public string CountOfInvoices { get; set; }
        public string ShipperInvoiceNumber { get; set; }
        public string Consignee { get; set; }
        public string BUImporte { get; set; }
        public string ConsigneeRef1 { get; set; }
        public string ConsigneeRef2 { get; set; }
        public string Agent { get; set; }
        public string AgentRef1 { get; set; }
        public string AgentRef2 { get; set; }
        public string Incoterm { get; set; }
        public string FreightPC { get; set; }
        public string OtherPC { get; set; }
        public string ModeofTransport { get; set; }
        public string Type { get; set; }
        public string FreightForwarder { get; set; }
        public string ClearingAgentimport { get; set; }
        public string MainCarriageCarrier { get; set; }
        public string Vessel { get; set; }
        public string CarrierNumber { get; set; }
        public string BookingConf { get; set; }
        public string ConfirmedBy { get; set; }
        public DateTime? Cutoffdate { get; set; }
        public string CutoffTime { get; set; }
        public string ConfirmationNotes { get; set; }
        public string MAWBMBL { get; set; }
        public string HAWBHBL { get; set; }
        public DateTime? HAWBDate { get; set; }
        public double? GrossWeightKgs { get; set; }
        public double? Volumem3 { get; set; }
        public int? NumberofPackages { get; set; }
        public double? ChargeableWeightKgs { get; set; }
        public int? TotalPackagesReceived { get; set; }
        public double? CountofTEU { get; set; }
        public bool DGR { get; set; }
        public string DescriptionofGoods { get; set; }
        public string Routing { get; set; }
        public string Numberofpickups { get; set; }
        public string PickupCity { get; set; }
        public string PickupCountry { get; set; }
        public string PortofDeparture { get; set; }
        public string CountryofDeparture { get; set; }
        public string ViaCity { get; set; }
        public string CountryofDestination { get; set; }
        public string PortofDestination { get; set; }
        public string DeliveryToName { get; set; }
        public string DeliveryTocity { get; set; }
        public DateTime? CreateDate { get; set; }
        public string NotificationDate { get; set; }
        public string GoodsReadinessDate { get; set; }
        public string DocumentsReadinessDate { get; set; }
        public DateTime? PickupFromDate { get; set; }
        public DateTime? GroupageDate { get; set; }
        public DateTime? DateonboardOrigin { get; set; }
        public DateTime? Dateofarrivaltoport { get; set; }
        public DateTime? ImportDeclarationDate { get; set; }
        public DateTime? CustomsClearanceDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public bool IncludeCustoms { get; set; }
        public string ImportDeclarationNumber { get; set; }
        public string CustomsDeclaration { get; set; }
        public string CustomsInspection { get; set; }
        public string ExportDeclarationNumber { get; set; }
        public string ExportDeclarationdate { get; set; }
        public string CountsofCITES { get; set; }
        public string CountLocalAuthorityApproval { get; set; }
        public string LocalInspection { get; set; }
        public string CountofUndertakingLetter { get; set; }
        public string CountofCertificateofOrigin { get; set; }
        public string CountofCertificateofConformity { get; set; }
        public string CountofLegalisedDocuments { get; set; }
        public double? ShipperInvoiceValue { get; set; }
        public string CurrencyofShipperInvoice { get; set; }
        public string RefundInvno { get; set; }
        public string InsuranceClaimNumber { get; set; }
        public string InsuranceClaimCurrency { get; set; }
        public string InsuranceClaimAmount { get; set; }
        public string InsuranceClaimDate { get; set; }
        public string Status { get; set; }
        public string Dept { get; set; }
        public string Branch { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Reference3 { get; set; }
        public string Reference4 { get; set; }

        public string Openedby { get; set; }
        public string OperationalClosedby { get; set; }
        public string TruckerName { get; set; }

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

        public string CustomerExternalID { get; set; }
        public string ShipperConsigneeExternalID { get; set; }

        public int NumberofDeliveries { get; set; }
        public DateTime? LastPickupArrivalDate { get; set; }
        public DateTime? LastDeliveryArrivalDate { get; set; }
        public double? ChargeableWeight { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public string FinalPortofDestination { get; set; }
        public string FinalCountryofDestination{get;set;}
        public string OnCarriageTransportMode { get; set; }

    }
}