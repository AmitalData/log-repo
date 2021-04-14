using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class ExportStorge
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("DeclarationId")]
	    public string DeclarationId { get; set; }
        [Column("ExportFileNo")]
	    public string ExportFileNo { get; set; }
        [Column("OrderNo")]
	    public string OrderNo { get; set; }
        [Column("CustomFileNo")]
	    public string CustomFileNo { get; set; }
        [ForeignKey("FclLclCodeTable")]
        [Column("FclLcl")]
	    public string FclLcl { get; set; }
	      
        public virtual FclLclCode FclLclCodeTable { get; set; }
        [Column("Direction")]
	    public string Direction { get; set; }
        [ForeignKey("CustomsTransportMode")]
        [Column("TransportModeId")]
	    public string TransportModeId { get; set; }
	      
        public virtual CustomsTransportMode CustomsTransportMode { get; set; }
        [Column("StorageNo")]
	    public int StorageNo { get; set; }
        [Column("VoyageNo")]
	    public double VoyageNo { get; set; }
        [Column("StorageDate")]
	    public DateTime StorageDate { get; set; }
        [ForeignKey("CustomsStorageStatus")]
        [Column("StorageStatus")]
	    public string StorageStatus { get; set; }
	      
        public virtual StorageStatus CustomsStorageStatus { get; set; }
        [Column("IsOpenStoarge")]
	    public bool IsOpenStoarge { get; set; }
        [ForeignKey("NDMessageActionCode")]
        [Column("OperationCode")]
	    public string OperationCode { get; set; }
	      
        public virtual NDMessageActionCode NDMessageActionCode { get; set; }
        [ForeignKey("ExportSenderCode")]
        [Column("SenderCodeID")]
	    public string SenderCodeID { get; set; }
	      
        public virtual ExportDeliveryDocumentMessage ExportSenderCode { get; set; }
        [Column("MessageFromForm")]
	    public int MessageFromForm { get; set; }
        [Column("ReplyPhoneNumeric")]
	    public double ReplyPhoneNumeric { get; set; }
        [Column("OperatorID")]
	    public double OperatorID { get; set; }
        [ForeignKey("ExportInformedParty")]
        [Column("InformedParty")]
	    public string InformedParty { get; set; }
	      
        public virtual ExportDeliveryDocumentMessage ExportInformedParty { get; set; }
        [Column("DeclarationNumber")]
	    public string DeclarationNumber { get; set; }
        [Column("DeclarationsInContainer")]
	    public int DeclarationsInContainer { get; set; }
        [Column("ExportManifestNumber")]
	    public int ExportManifestNumber { get; set; }
        [ForeignKey("InternationalSite")]
        [Column("ReceivingSite")]
	    public string ReceivingSite { get; set; }
	      
        public virtual InternationalSite InternationalSite { get; set; }
        [ForeignKey("StuffingSite")]
        [Column("StuffingSiteType")]
	    public string StuffingSiteType { get; set; }
	      
        public virtual StuffingSiteType StuffingSite { get; set; }
        [ForeignKey("PortLoadingSite")]
        [Column("LoadingSite")]
	    public string LoadingSite { get; set; }
	      
        public virtual InternationalSite PortLoadingSite { get; set; }
        [Column("ForwarderReference")]
	    public string ForwarderReference { get; set; }
        [Column("TransactionQuantity")]
	    public decimal TransactionQuantity { get; set; }
        [Column("ExportDocument")]
	    public string ExportDocument { get; set; }
        [Column("MessageContent")]
	    public string MessageContent { get; set; }
        [Column("BookingNumber")]
	    public string BookingNumber { get; set; }
        [ForeignKey("DeliveryType")]
        [Column("LogisticDeliveryTypeID")]
	    public string LogisticDeliveryTypeID { get; set; }
	      
        public virtual DeliveryType DeliveryType { get; set; }
        [Column("CargoRows")]
	    public decimal CargoRows { get; set; }
        [ForeignKey("CustomerIdentificationType")]
        [Column("ExporterIdentificationType")]
	    public string ExporterIdentificationType { get; set; }
	      
        public virtual CustomerIdentificationType CustomerIdentificationType { get; set; }
        [ForeignKey("FinalDestinati")]
        [Column("FinalDestinationInternatID")]
	    public string FinalDestinationInternatID { get; set; }
	      
        public virtual InternationalSite FinalDestinati { get; set; }
        [ForeignKey("FirstDestination")]
        [Column("FirstDestinationInternatID")]
	    public string FirstDestinationInternatID { get; set; }
	      
        public virtual InternationalSite FirstDestination { get; set; }
        [ForeignKey("AbroadSite")]
        [Column("OriginAbroadSite")]
	    public string OriginAbroadSite { get; set; }
	      
        public virtual InternationalSite AbroadSite { get; set; }
        [Column("ExpectedPortArrivalDate")]
	    public string ExpectedPortArrivalDate { get; set; }
        [Column("DraggedOrSupportedNumber")]
	    public string DraggedOrSupportedNumber { get; set; }
        [ForeignKey("TruckOrTrainNo")]
        [Column("TruckOrTrainNumber")]
	    public string TruckOrTrainNumber { get; set; }
	      
        public virtual Trucker TruckOrTrainNo { get; set; }
        [ForeignKey("Trucker")]
        [Column("DriverId")]
	    public string DriverId { get; set; }
	      
        public virtual Trucker Trucker { get; set; }
        [ForeignKey("CustomsShip")]
        [Column("ShipCode")]
	    public string ShipCode { get; set; }
	      
        public virtual CustomsShip CustomsShip { get; set; }
        [Column("ShipName")]
	    public string ShipName { get; set; }
        [Column("TransportCompany")]
	    public string TransportCompany { get; set; }
        [ForeignKey("ShippingAgent")]
        [Column("ShipAgent")]
	    public string ShipAgent { get; set; }
	      
        public virtual ShippingAgent ShippingAgent { get; set; }
        [ForeignKey("ShippingLine")]
        [Column("ShippingCompanyCode")]
	    public string ShippingCompanyCode { get; set; }
	      
        public virtual ShippingLine ShippingLine { get; set; }
        [ForeignKey("SecurityClearenceTypeCode")]
        [Column("SecurityClearence")]
	    public string SecurityClearence { get; set; }
	      
        public virtual SecurityClearenceTypeCode SecurityClearenceTypeCode { get; set; }
        [ForeignKey("TransportMeansType")]
        [Column("TransferCargoMethodType")]
	    public string TransferCargoMethodType { get; set; }
	      
        public virtual TransportMeansType TransportMeansType { get; set; }
        [Column("StorageOrDockID")]
	    public string StorageOrDockID { get; set; }
        [Column("ExporterName")]
	    public string ExporterName { get; set; }
        [Column("ExporterFileNumber")]
	    public string ExporterFileNumber { get; set; }
        [ForeignKey("CustomsCountry")]
        [Column("PassportCountry")]
	    public string PassportCountry { get; set; }
	      
        public virtual CustomsCountry CustomsCountry { get; set; }
        [Column("ExporterNumber")]
	    public string ExporterNumber { get; set; }
        [Column("OpenDate")]
	    public DateTime OpenDate { get; set; }
        [ForeignKey("CargoIdentifireType")]
        [Column("CargoTypeCode")]
	    public string CargoTypeCode { get; set; }
	      
        public virtual CargoIdentifireType CargoIdentifireType { get; set; }
        [Column("Manifest")]
	    public string Manifest { get; set; }
        [Column("SecondCargoID")]
	    public string SecondCargoID { get; set; }
        [Column("ThirdCargoID")]
	    public string ThirdCargoID { get; set; }
        [Column("CargoDescription")]
	    public string CargoDescription { get; set; }
        [ForeignKey("CustomsCargoType")]
        [Column("CargoType")]
	    public string CargoType { get; set; }
	      
        public virtual CargoType CustomsCargoType { get; set; }
        [ForeignKey("CustomsHandingCode")]
        [Column("HandlingCode")]
	    public string HandlingCode { get; set; }
	      
        public virtual HandingCode CustomsHandingCode { get; set; }
        [Column("DangerousGoodsIndication")]
	    public decimal DangerousGoodsIndication { get; set; }
        [Column("CodeBreaksIndication")]
	    public decimal CodeBreaksIndication { get; set; }
        [Column("DamageCode")]
	    public decimal DamageCode { get; set; }
        [ForeignKey("ForeignCurrency")]
        [Column("ForeignCurrencyType")]
	    public string ForeignCurrencyType { get; set; }
	      
        public virtual CurrencyType ForeignCurrency { get; set; }
        [Column("ForeignCurrencyAmoun")]
	    public decimal ForeignCurrencyAmoun { get; set; }
        [Column("GoodsValueNIS")]
	    public decimal GoodsValueNIS { get; set; }
        [ForeignKey("PackingType")]
        [Column("PackageType")]
	    public string PackageType { get; set; }
	      
        public virtual PackingType PackingType { get; set; }
        [Column("Quantity")]
	    public decimal Quantity { get; set; }
        [Column("MarksNumbers")]
	    public string MarksNumbers { get; set; }
        [Column("WeightInPortMandatory")]
	    public decimal WeightInPortMandatory { get; set; }
        [Column("Weight")]
	    public decimal Weight { get; set; }
        [Column("VolumeSize")]
	    public decimal VolumeSize { get; set; }
        [Column("LicensePlateNumber")]
	    public string LicensePlateNumber { get; set; }
        [Column("CustomsItem")]
	    public string CustomsItem { get; set; }
        [ForeignKey("DangerousGoodsPackingReq")]
        [Column("RiskLevel")]
	    public string RiskLevel { get; set; }
	      
        public virtual DangerousGoodsPackingReq DangerousGoodsPackingReq { get; set; }
        [Column("DangerousSubstancename")]
	    public string DangerousSubstancename { get; set; }
        [Column("WeightVerificationNumber")]
	    public string WeightVerificationNumber { get; set; }
        [Column("ExporterReportedWeightID")]
	    public decimal ExporterReportedWeightID { get; set; }
        [Column("ExporterReportedWeightName")]
	    public string ExporterReportedWeightName { get; set; }
        [Column("ContainerNumber")]
	    public string ContainerNumber { get; set; }
        [Column("CoolingActivated")]
	    public decimal CoolingActivated { get; set; }
        [Column("RequiredTemperature")]
	    public decimal RequiredTemperature { get; set; }
        [Column("PharmaGroceryIndication")]
	    public string PharmaGroceryIndication { get; set; }
        [Column("LeftException")]
	    public decimal LeftException { get; set; }
        [Column("RightException")]
	    public decimal RightException { get; set; }
        [Column("FrontException")]
	    public decimal FrontException { get; set; }
        [Column("BackException")]
	    public decimal BackException { get; set; }
        [Column("HeightException")]
	    public decimal HeightException { get; set; }
        [Column("ContainerLineCode")]
	    public string ContainerLineCode { get; set; }
        [Column("VentValue")]
	    public decimal VentValue { get; set; }
        [Column("HumidityPercentage")]
	    public decimal HumidityPercentage { get; set; }
        [Column("Co2Percentage")]
	    public decimal Co2Percentage { get; set; }
        [Column("O2Percentage")]
	    public decimal O2Percentage { get; set; }
        [Column("SealNumber")]
	    public string SealNumber { get; set; }
        [ForeignKey("ExportSealType")]
        [Column("SealType")]
	    public string SealType { get; set; }
	      
        public virtual SealType ExportSealType { get; set; }
        [ForeignKey("ExportCoolingReportingMethod")]
        [Column("CoolingReportingMethod")]
	    public string CoolingReportingMethod { get; set; }
	      
        public virtual CoolingReportingMethod ExportCoolingReportingMethod { get; set; }
        [ForeignKey("ExportFullnessCode")]
        [Column("FullnessCode")]
	    public string FullnessCode { get; set; }
	      
        public virtual FullnessCode ExportFullnessCode { get; set; }
        [ForeignKey("SupplierPartyType")]
        [Column("OwnershipCode")]
	    public string OwnershipCode { get; set; }
	      
        public virtual SupplierPartyType SupplierPartyType { get; set; }
        [ForeignKey("ContainerType")]
        [Column("ContainerTypeWCO")]
	    public string ContainerTypeWCO { get; set; }
	      
        public virtual ContainerType ContainerType { get; set; }
        [ForeignKey("HazardousSubstance")]
        [Column("UNNumber")]
	    public string UNNumber { get; set; }
	      
        public virtual HazardousSubstance HazardousSubstance { get; set; }
        [Column("RiskGroup")]
	    public string RiskGroup { get; set; }
        [Column("ExporterRef")]
	    public string ExporterRef { get; set; }
    }
}
	 