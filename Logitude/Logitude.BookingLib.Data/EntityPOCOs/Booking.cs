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

namespace Logitude.BookingLib.Data.EntityPOCOs
{
   
    public class Booking
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("BookingNumber")]
	    public string BookingNumber { get; set; }
        [ForeignKey("Direction")]
        [Column("DirectionCode")]
	    public string DirectionCode { get; set; }
	      
        public virtual Direction Direction { get; set; }
        [ForeignKey("TransportMode")]
        [Column("TransportModeCode")]
	    public string TransportModeCode { get; set; }
	      
        public virtual TransportMode TransportMode { get; set; }
        [ForeignKey("Shipment")]
        [Column("ShipmentId")]
	    public string ShipmentId { get; set; }
	      
        public virtual Shipment Shipment { get; set; }
        [Column("CreateDate")]
	    public DateTime? CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [ForeignKey("BookingStatus")]
        [Column("BookingStatusCode")]
	    public string BookingStatusCode { get; set; }
	      
        public virtual BookingStatus BookingStatus { get; set; }
        [Column("Master")]
	    public string Master { get; set; }
        [ForeignKey("BookingSpaceAllocation")]
        [Column("SpaceAllocationCode")]
	    public string SpaceAllocationCode { get; set; }
	      
        public virtual BookingSpaceAllocation BookingSpaceAllocation { get; set; }
        [ForeignKey("MainCarriageCarrier")]
        [Column("MainCarriageCarrierId")]
	    public string MainCarriageCarrierId { get; set; }
	      
        public virtual Card MainCarriageCarrier { get; set; }
        [Column("MainCarriageIsFromStack")]
	    public bool MainCarriageIsFromStack { get; set; }
        [ForeignKey("MainCarriageSpaceAllocation")]
     
	    public string MainCarriageSpaceAllocationCode { get; set; }
	      
        public virtual BookingSpaceAllocation MainCarriageSpaceAllocation { get; set; }
     
	    public string MainCarriageAllotmentIdentification { get; set; }
        [ForeignKey("MainCarriageFromPort")]
        [Column("MainCarriageFromPortId")]
	    public string MainCarriageFromPortId { get; set; }
	      
        public virtual Port MainCarriageFromPort { get; set; }
        [ForeignKey("MainCarriageToPort")]
        [Column("MainCarriageToPortId")]
	    public string MainCarriageToPortId { get; set; }
	      
        public virtual Port MainCarriageToPort { get; set; }
        [Column("MainCarriageCarrierPrefix")]
	    public string MainCarriageCarrierPrefix { get; set; }
        [Column("MainCarriageCarrierNumber")]
	    public string MainCarriageCarrierNumber { get; set; }
        [Column("MainCarriageETD")]
	    public DateTime? MainCarriageETD { get; set; }
        [ForeignKey("Transshipment1FromPort")]
        [Column("Transshipment1FromPortId")]
	    public string Transshipment1FromPortId { get; set; }
	      
        public virtual Port Transshipment1FromPort { get; set; }
        [ForeignKey("Transshipment1ToPort")]
        [Column("Transshipment1ToPortId")]
	    public string Transshipment1ToPortId { get; set; }
	      
        public virtual Port Transshipment1ToPort { get; set; }
        [ForeignKey("Transshipment1Carrier")]
        [Column("Transshipment1CarrierId")]
	    public string Transshipment1CarrierId { get; set; }
	      
        public virtual Card Transshipment1Carrier { get; set; }
        [Column("Transshipment1ETD")]
	    public DateTime? Transshipment1ETD { get; set; }
     
	    public string Transshipment1AllotmentIdentification { get; set; }
        [Column("Transshipment1CarrierPrefix")]
	    public string Transshipment1CarrierPrefix { get; set; }
        [Column("Transshipment1CarrierNumber")]
	    public string Transshipment1CarrierNumber { get; set; }
        [ForeignKey("Transshipment1SpaceAllocation")]
     
	    public string Transshipment1SpaceAllocationCode { get; set; }
	      
        public virtual BookingSpaceAllocation Transshipment1SpaceAllocation { get; set; }
        [ForeignKey("Transshipment2FromPort")]
        [Column("Transshipment2FromPortId")]
	    public string Transshipment2FromPortId { get; set; }
	      
        public virtual Port Transshipment2FromPort { get; set; }
        [ForeignKey("Transshipment2ToPort")]
        [Column("Transshipment2ToPortId")]
	    public string Transshipment2ToPortId { get; set; }
	      
        public virtual Port Transshipment2ToPort { get; set; }
        [ForeignKey("Transshipment2Carrier")]
        [Column("Transshipment2CarrierId")]
	    public string Transshipment2CarrierId { get; set; }
	      
        public virtual Card Transshipment2Carrier { get; set; }
        [Column("Transshipment2ETD")]
	    public DateTime? Transshipment2ETD { get; set; }
     
	    public string Transshipment2AllotmentIdentification { get; set; }
        [Column("Transshipment2CarrierPrefix")]
	    public string Transshipment2CarrierPrefix { get; set; }
        [Column("Transshipment2CarrierNumber")]
	    public string Transshipment2CarrierNumber { get; set; }
        [ForeignKey("Transshipment2SpaceAllocation")]
     
	    public string Transshipment2SpaceAllocationCode { get; set; }
	      
        public virtual BookingSpaceAllocation Transshipment2SpaceAllocation { get; set; }
        [ForeignKey("FFRStatus")]
        [Column("FFRStatusCode")]
	    public string FFRStatusCode { get; set; }
	      
        public virtual FFRStatus FFRStatus { get; set; }
        [Column("FFRStatusDate")]
	    public DateTime? FFRStatusDate { get; set; }
        [Column("FNAReason")]
	    public string FNAReason { get; set; }
        [ForeignKey("Shipper")]
        [Column("ShipperId")]
	    public string ShipperId { get; set; }
	      
        public virtual Card Shipper { get; set; }
        [ForeignKey("ShipperAddress")]
        [Column("ShipperAddressId")]
	    public string ShipperAddressId { get; set; }
	      
        public virtual Address ShipperAddress { get; set; }
        [Column("ShipperReference")]
	    public string ShipperReference { get; set; }
        [ForeignKey("Consignee")]
        [Column("ConsigneeId")]
	    public string ConsigneeId { get; set; }
	      
        public virtual Card Consignee { get; set; }
        [ForeignKey("ConsigneeAddress")]
        [Column("ConsigneeAddressId")]
	    public string ConsigneeAddressId { get; set; }
	      
        public virtual Address ConsigneeAddress { get; set; }
        [Column("ConsigneeReference")]
	    public string ConsigneeReference { get; set; }
        [ForeignKey("IssuingCarrierAgent")]
        [Column("IssuingCarrierAgentId")]
	    public string IssuingCarrierAgentId { get; set; }
	      
        public virtual Card IssuingCarrierAgent { get; set; }
        [ForeignKey("IssuingCarrierAddress")]
        [Column("IssuingCarrierAddressId")]
	    public string IssuingCarrierAddressId { get; set; }
	      
        public virtual Address IssuingCarrierAddress { get; set; }
        [ForeignKey("IATACode")]
        [Column("IATACodeId")]
	    public string IATACodeId { get; set; }
	      
        public virtual IATACode IATACode { get; set; }
        [Column("CASSCode")]
	    public string CASSCode { get; set; }
        [ForeignKey("AWBSpecialHandlingCode1")]
        [Column("AWBSpecialHandlingCodeId1")]
	    public string AWBSpecialHandlingCodeId1 { get; set; }
	      
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode1 { get; set; }
        [ForeignKey("AWBSpecialHandlingCode2")]
        [Column("AWBSpecialHandlingCodeId2")]
	    public string AWBSpecialHandlingCodeId2 { get; set; }
	      
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode2 { get; set; }
        [ForeignKey("AWBSpecialHandlingCode3")]
        [Column("AWBSpecialHandlingCodeId3")]
	    public string AWBSpecialHandlingCodeId3 { get; set; }
	      
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode3 { get; set; }
        [ForeignKey("AWBSpecialHandlingCode4")]
        [Column("AWBSpecialHandlingCodeId4")]
	    public string AWBSpecialHandlingCodeId4 { get; set; }
	      
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode4 { get; set; }
        [ForeignKey("AWBSpecialHandlingCode5")]
        [Column("AWBSpecialHandlingCodeId5")]
	    public string AWBSpecialHandlingCodeId5 { get; set; }
	      
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode5 { get; set; }
        [ForeignKey("AWBSpecialHandlingCode6")]
        [Column("AWBSpecialHandlingCodeId6")]
	    public string AWBSpecialHandlingCodeId6 { get; set; }
	      
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode6 { get; set; }
        [ForeignKey("AWBSpecialHandlingCode7")]
        [Column("AWBSpecialHandlingCodeId7")]
	    public string AWBSpecialHandlingCodeId7 { get; set; }
	      
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode7 { get; set; }
        [ForeignKey("AWBSpecialHandlingCode8")]
        [Column("AWBSpecialHandlingCodeId8")]
	    public string AWBSpecialHandlingCodeId8 { get; set; }
	      
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode8 { get; set; }
        [ForeignKey("AWBSpecialHandlingCode9")]
        [Column("AWBSpecialHandlingCodeId9")]
	    public string AWBSpecialHandlingCodeId9 { get; set; }
	      
        public virtual AWBSpecialHandlingCode AWBSpecialHandlingCode9 { get; set; }
        [Column("AWBCarrierTarrifReference")]
	    public string AWBCarrierTarrifReference { get; set; }
        [Column("DescriptionOfGoods")]
	    public string DescriptionOfGoods { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("SpecialServicesRequest")]
	    public string SpecialServicesRequest { get; set; }
        [Column("OtherServicesInformation")]
	    public string OtherServicesInformation { get; set; }
        [Column("Routing")]
	    public string Routing { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("IssuingCarrierIATACode")]
	    public string IssuingCarrierIATACode { get; set; }
        [Column("GrossWeightEdited")]
	    public bool GrossWeightEdited { get; set; }
        [Column("ChargeableWeightEdited")]
	    public bool ChargeableWeightEdited { get; set; }
        [Column("NumberOfPackages")]
	    public int? NumberOfPackages { get; set; }
        [Column("GrossWeight")]
	    public decimal? GrossWeight { get; set; }
        [Column("Volume")]
	    public decimal? Volume { get; set; }
        [Column("VolumetricWeight")]
	    public decimal? VolumetricWeight { get; set; }
        [Column("ChargeableWeight")]
	    public decimal? ChargeableWeight { get; set; }
        [Column("AWBCommodityItemNumber")]
	    public string AWBCommodityItemNumber { get; set; }
        [ForeignKey("GrossWeightUnit")]
        [Column("GrossWeightUnitCode")]
	    public string GrossWeightUnitCode { get; set; }
	      
        public virtual WeightUnit GrossWeightUnit { get; set; }
        [ForeignKey("ChargeableWeightUnit")]
        [Column("ChargeableWeightUnitCode")]
	    public string ChargeableWeightUnitCode { get; set; }
	      
        public virtual WeightUnit ChargeableWeightUnit { get; set; }
        [ForeignKey("DimensionsUnit")]
        [Column("DimensionsUnitCode")]
	    public string DimensionsUnitCode { get; set; }
	      
        public virtual DimensionsUnit DimensionsUnit { get; set; }
        [ForeignKey("VolumeUnit")]
        [Column("VolumeUnitCode")]
	    public string VolumeUnitCode { get; set; }
	      
        public virtual VolumeUnit VolumeUnit { get; set; }
        [Column("GrossWeightInKG")]
	    public decimal? GrossWeightInKG { get; set; }
        [Column("ChargeableWeightInKG")]
	    public decimal? ChargeableWeightInKG { get; set; }
        [Column("Ratio")]
	    public decimal? Ratio { get; set; }
        [Column("DimFactor")]
	    public decimal? DimFactor { get; set; }
        [ForeignKey("MainCarriageFinalDestinationPort")]
     
	    public string MainCarriageFinalDestinationPortId { get; set; }
	      
        public virtual Port MainCarriageFinalDestinationPort { get; set; }
        [Column("IsDangerous")]
	    public bool IsDangerous { get; set; }
        [Column("DangerousClassNumber")]
	    public string DangerousClassNumber { get; set; }
        [Column("DangerousUnNumber")]
	    public string DangerousUnNumber { get; set; }
        [Column("DangerousPackagingGroup")]
	    public string DangerousPackagingGroup { get; set; }
        [Column("DangerousIMDGCode")]
	    public string DangerousIMDGCode { get; set; }
        [Column("DangerousFlashPoint")]
	    public string DangerousFlashPoint { get; set; }
        [Column("DangerousMaterialDescription")]
	    public string DangerousMaterialDescription { get; set; }
        [Column("MainHarmonize")]
	    public string MainHarmonize { get; set; }
        [Column("AWBHandlingInformation")]
	    public string AWBHandlingInformation { get; set; }
        [Column("AnswerOtherServicesInformation")]
	    public string AnswerOtherServicesInformation { get; set; }
        [Column("HasResponse")]
	    public bool HasResponse { get; set; }
        [Column("FMAAcknowledgementReason")]
	    public string FMAAcknowledgementReason { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [Column("HasErrors")]
	    public bool HasErrors { get; set; }
        [Column("WaitingForResponse")]
	    public bool WaitingForResponse { get; set; }
        [ForeignKey("Interline")]
        [Column("InterlineId")]
	    public string InterlineId { get; set; }
	      
        public virtual Card Interline { get; set; }
        [ForeignKey("BookingLevel")]
        [Column("BookingLevelCode")]
	    public string BookingLevelCode { get; set; }
	      
        public virtual BookingLevel BookingLevel { get; set; }
        [Column("AirlinePrefix")]
	    public string AirlinePrefix { get; set; }
        [ForeignKey("BookingProduct")]
        [Column("BookingProductId")]
	    public string BookingProductId { get; set; }
	      
        public virtual BookingProduct BookingProduct { get; set; }
        [ForeignKey("LastSentByUser")]
        [Column("LastSentByUserId")]
	    public string LastSentByUserId { get; set; }
	      
        public virtual User LastSentByUser { get; set; }
        [ForeignKey("AWBDescriptionOfGoods")]
        [Column("DescriptionOfGoodsId")]
	    public string DescriptionOfGoodsId { get; set; }
	      
        public virtual AWBDescriptionOfGoods AWBDescriptionOfGoods { get; set; }
        [Column("ConcurrencyGUID")]
	    public string ConcurrencyGUID { get; set; }
        [Column("AccountNumber")]
	    public string AccountNumber { get; set; }
        [Column("IsTemperatureSensitive")]
	    public bool IsTemperatureSensitive { get; set; }
        [Column("LastFSRStatusRequestDate")]
	    public DateTime? LastFSRStatusRequestDate { get; set; }
        [Column("UpdatedByPartner")]
	    public string UpdatedByPartner { get; set; }
    }
}
	 