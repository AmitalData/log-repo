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
   
    public class ExportStorgeCargo
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("StorageID")]
	    public string StorageID { get; set; }
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
    }
}
	 