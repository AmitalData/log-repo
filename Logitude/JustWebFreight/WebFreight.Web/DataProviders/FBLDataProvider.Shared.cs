using System;
using System.Collections.Generic;

namespace WebFreight.Web.DataProviders
{
    public class FBLDataProvider
    {
        public string ShipperAddress { get; set; }
        public string ShipperAddress_NoTel { get; set; }       
        public string ShipmentNumber { get; set; }
        public string BookingNumber { get; set; }
        public string CompanyName { get; set; }
        /// <summary>
        /// Exporter's country code + HBL ( HBL not supported yet)
        /// </summary>
        public string TenantCountryCode { get; set; }
        public byte[] TenantLogo { get; set; }

        public string House { get; set; }
        public string Master { get; set; }
        public string LongMaster { get; set; }

        /// <summary>
        /// If notify exists,show notify.Else show importer
        /// </summary>
        public string NotifyAddress { get; set; }
        public string Notify2Address { get; set; }
        public string FBLNotes { get; set; } // custom
        public string FBLNotes2 { get; set; } // custom
        public string PlaceOfReceipt { get; set; }        
        public string PlaceOfDelivery { get; set; }
        public string OnCarriageToPort { get; set; }
        public string MainCarriageVesselName { get; set; }
        public string MainCarriageVesselNameAndNumber { get; set; }
        public string LoadingPortName { get; set; }
        public string DischargePortName { get; set; }
        public List<PackageLine> PackagesLines { get; set; }
        public List<PackageLine> AttachmentList { get; set; }

        public string TotalQuantity { get; set; }
        public double? TotalQuantity_Double { get; set; }
        public string TotalWeight { get; set; }
        public string TotalVolume { get; set; }
        public string PrepaidCollect { get; set; }
        public string PlaceAndDateOfIssue { get; set; }
        public string NumberOfOriginals { get; set; }//custom
        public string Signature { get; set; }
        public string AgentInfo { get; set; }
        public string CopyOrOriginal { get; set; }//custom

        public string AgentFullDetails { get; set; }
        public string AgentContact { get; set; }
        public string AgentPhone { get; set; }
        public string AgentFax { get; set; }

        public string GeneralDescriptionOfGoods { get; set; }

        private bool hasAttachmentList = false;
        public bool HasAttachmentList
        {
            get { return hasAttachmentList; }
            set { hasAttachmentList = value; }
        }

        public bool InServerSide { get; set; }
        public string CustomPackagesNumber { get; set; }
        public string CustomPackageType { get; set; }
        public string CustomWeight { get; set; }
        public string CustomVolume { get; set; }
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

        public string MoveTypeCode { get; set; }
        public string MoveTypeName { get; set; }
        public string FromLocationCountryCode { get; set; }
        public string ToLocationCountryCode { get; set; }
        public string CopyName { get; set; }
        public string IncotermCode { get; set; }
        public string IncotermName { get; set; }
        public string ShipmentType { get; set; }
        public string ShipperCode { get; set; }
        public string AgentCode { get; set; }
        public string ForwarderAgentCode { get; set; }
        public string ForwarderAgentAddress { get; set; }
        public string PlaceOfReceiptCountryName { get; set; }
        public string LoadingPortCountryName { get; set; }
        public string PlaceOfDeliveryCountryName { get; set; }
        public string DischargePortCountryName { get; set; }
        public string FirstPickupAddress { get; set; }
        public string MainCarriageETA { get; set; }
        public string PickupDate { get; set; }

        public string ConsigneeRef1 { get; set; }
        public string ConsigneeRef2 { get; set; }
        public string ConsigneeCode { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ConsigneeAddress_NoState { set; get; }
        public string ConsigneeAlways { get; set; }
        public string PreCarriageBy { get; set; }
        public string PreCarriageFromPort { get; set; }

        public double? OpenReceivablesInLocalCurrency { get; set; }
        public double? OpenReceivablesInProfitCurrency { get; set; }
        public double? AccountedReceivablesInLocalCurrency { get; set; }
        public double? AccountedReceivablesInProfitCurrency { get; set; }

        public string ShipperRef1 { get; set; }
        public string ShipperRef2 { get; set; }

        public string MainCarriageCarrierName { get; set; }
        public bool Containerized { get; set; }

        public string TransshipmentsVesselNameAndNumber { get; set; }
               
        public DateTime? MainCarriageFirstLegETD { get; set; } //(this should print the ETD date of the first main carriage leg)
        public string Temperature { get; set; } //(shows the temperature set on the first container package added on the shipment)
        public string AgentPrimaryContactDetails { get; set; } // Should print in the following way: ContactName, mail: Contact Email, Tel: Contact Tel
        public string FinalDestination { get; set; }
        public string VoyageNumber { get; set; }
        public string VesselName { get; set; }
        public double? ValueOfGoods { get; set; }
        public string CurrencyOfValueofGoods { get; set; }
        public string CurrencyOfValueofGoodsLocal { get; set; }
        public List<ReceivablesCharges> ReceivablesCharges { get; set; }

        public string ReleasingAgentName  { get; set; }
        public string ReleasingAgentAddress { get; set; }

        public string ConsigneeNotImporterAddress { get; set; }

        public string PlaceOfReceiptPickUp { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public int? NumberOfInsidePackages { get; set; }
        
        public DateTime? FreightRelease { get; set; }
        public DateTime? TerminalAvailable { get; set; }
        public string ISFNumber { get; set; }
        public DateTime? ISFDate { get; set; }
        public string ITNumber { get; set; }
        public DateTime? ITDate { get; set; }
        public string FMCNumber { get; set; }
        public string ENSNumber { get; set; }
        public DateTime? ENSDate { get; set; }

        public string OBLType { get; set; }
        public DateTime? DocumentsClosingDate { get; set; }
        public TimeSpan? DocumentsClosingTime { get; set; }

        public string ContactDetails { get; set; }
        public string ShipperContactDetails { get; set; }
        public string ConsigneeContactDetails { get; set; }
        public string Notify1ContactDetails { get; set; }
        public string Notify2ContactDetails { get; set; }
        public string ShipperNotExporterContactDetails { get; set; }
        public string DeliveryToContactName { get; set; }
        public string DeliveryToContactEmail { get; set; }
        public string DeliveryToContactPhone { get; set; }
        public string ShipperVAT { get; set; }
        public string ConsigneeVAT { get; set; }
        public string AgentVAT { get; set; }
        public string CustomerVAT { get; set; }

        public string ShipperNotExporterAddress { get; set; }

        public List<PickUpDeliveryLine> PickUpsLines { get; set; }
        public List<PickUpDeliveryLine> DeliveriesLines { get; set; }

        public string GrossWeightUnitCode { get; set; }
        public string VolumeUnitCode { get; set; }

        public string ShipperAddress_WithName { get; set; }
        public string ShipperNotExporterAddress_WithName { get; set; }
        public string ConsigneeAddress_WithName { get; set; }
        public string NotifyAddress_WithName { get; set; }
        public string NotifyAddress2_WithName { get; set; }

        public string ShipperAddress_NoState { get; set; }
        public string ShipperNotExporterAddress_NoState { get; set; }
        public string ConsigneeNotImporterAddress_NoState { get; set; }
        public double? TotalPrepaid { get; set; }
        public double? TotalCollect { get; set; }

        public string ShipperATTN { get; set; }
        public string ConsigneeATTN { get; set; }
        public string ShipperNotExporterATTN { get; set; }
        public string ConsigneeNotImporterATTN { get; set; }
        public string Notify1ATTN { get; set; }
        public string Notify2ATTN { get; set; }
        public string AgentATTN { get; set; }

        public string ShipperAddress_NoTelFax { get; set; }
        public string ConsigneeAddress_NoTelFax { get; set; }

        public string TenantCBSA { get; set; }
        public string TenantCAAT { get; set; }
        public string CarrierCBSA { get; set; }
        public string CarrierCAAT { get; set; }
        public string ConfirmationNotes { get; set; }
        public string CreatedBy { get; set; }
        public string PickUpInstructions { get; set; }
        public string DeliveryInstructions { get; set; }
    } 
    
    public class ReceivablesCharges
    {
        public string ChargeTypeEnglish { get; set; }
        public string ChargeTypeLocal { get; set; }
        public double? PrepaidChargeAmount { get; set; }
        public double? CollectChargeAmount { get; set; }
        public string Remark { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }

    }
}
