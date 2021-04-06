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
        [Column("IsConnectedToDeclaration")]
	    public bool IsConnectedToDeclaration { get; set; }
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
    }
}
	 