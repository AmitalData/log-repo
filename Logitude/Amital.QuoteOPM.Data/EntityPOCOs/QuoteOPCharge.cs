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

namespace Amital.QuoteOPM.Data.EntityPOCOs
{
   
    public class QuoteOPCharge
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("QuoteOP")]
        [Column("QuoteId")]
	    public string QuoteId { get; set; }
	      
        public virtual QuoteOP QuoteOP { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [Column("ValueDate")]
	    public DateTime ValueDate { get; set; }
        [Column("ContainerType1MarkUpValue")]
	    public double? ContainerType1MarkUpValue { get; set; }
        [Column("ContainerType2MarkUpValue")]
	    public double? ContainerType2MarkUpValue { get; set; }
        [Column("ContainerType3MarkUpValue")]
	    public double? ContainerType3MarkUpValue { get; set; }
        [Column("ContainerType4MarkUpValue")]
	    public double? ContainerType4MarkUpValue { get; set; }
        [Column("ContainerType5MarkUpValue")]
	    public double? ContainerType5MarkUpValue { get; set; }
        [ForeignKey("ContainerType1MarkUpType")]
        [Column("ContainerType1MarkUpTypeCode")]
	    public string ContainerType1MarkUpTypeCode { get; set; }
	      
        public virtual MarkUpOPType ContainerType1MarkUpType { get; set; }
        [ForeignKey("ContainerType2MarkUpType")]
        [Column("ContainerType2MarkUpTypeCode")]
	    public string ContainerType2MarkUpTypeCode { get; set; }
	      
        public virtual MarkUpOPType ContainerType2MarkUpType { get; set; }
        [ForeignKey("ContainerType3MarkUpType")]
        [Column("ContainerType3MarkUpTypeCode")]
	    public string ContainerType3MarkUpTypeCode { get; set; }
	      
        public virtual MarkUpOPType ContainerType3MarkUpType { get; set; }
        [ForeignKey("ContainerType4MarkUpType")]
        [Column("ContainerType4MarkUpTypeCode")]
	    public string ContainerType4MarkUpTypeCode { get; set; }
	      
        public virtual MarkUpOPType ContainerType4MarkUpType { get; set; }
        [ForeignKey("ContainerType5MarkUpType")]
        [Column("ContainerType5MarkUpTypeCode")]
	    public string ContainerType5MarkUpTypeCode { get; set; }
	      
        public virtual MarkUpOPType ContainerType5MarkUpType { get; set; }
        [Column("CostExchangeRate")]
	    public double? CostExchangeRate { get; set; }
        [ForeignKey("CostCurrency")]
        [Column("CostCurrencyId")]
	    public string CostCurrencyId { get; set; }
	      
        public virtual Currency CostCurrency { get; set; }
        [Column("CostIsFixedRate")]
	    public bool CostIsFixedRate { get; set; }
        [ForeignKey("ChargesType")]
        [Column("ChargesTypeId")]
	    public string ChargesTypeId { get; set; }
	      
        public virtual ChargesType ChargesType { get; set; }
        [ForeignKey("VendorCard")]
        [Column("VendorId")]
	    public string VendorId { get; set; }
	      
        public virtual Card VendorCard { get; set; }
        [ForeignKey("Currency")]
        [Column("SaleCurrencyId")]
	    public string SaleCurrencyId { get; set; }
	      
        public virtual Currency Currency { get; set; }
        [Column("SaleExchangeRate")]
	    public double? SaleExchangeRate { get; set; }
        [ForeignKey("MarkUpType")]
        [Column("MarkUpTypeCode")]
	    public string MarkUpTypeCode { get; set; }
	      
        public virtual MarkUpOPType MarkUpType { get; set; }
        [Column("MarkUpValue")]
	    public double? MarkUpValue { get; set; }
        [Column("IsAllIN")]
	    public bool IsAllIN { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [ForeignKey("CostMeasurement")]
        [Column("CostMeasurementId")]
	    public string CostMeasurementId { get; set; }
	      
        public virtual Measurement CostMeasurement { get; set; }
        [Column("CostQuantity")]
	    public double? CostQuantity { get; set; }
        [Column("CostUnitPrice")]
	    public double? CostUnitPrice { get; set; }
        [Column("CostTotalAmount")]
	    public double? CostTotalAmount { get; set; }
        [Column("CostTotalAmountLocal")]
	    public double? CostTotalAmountLocal { get; set; }
        [Column("CostContainerType1UnitPrice")]
	    public double? CostContainerType1UnitPrice { get; set; }
        [Column("CostContainerType2UnitPrice")]
	    public double? CostContainerType2UnitPrice { get; set; }
        [Column("CostContainerType3UnitPrice")]
	    public double? CostContainerType3UnitPrice { get; set; }
        [Column("CostContainerType4UnitPrice")]
	    public double? CostContainerType4UnitPrice { get; set; }
        [Column("CostContainerType5UnitPrice")]
	    public double? CostContainerType5UnitPrice { get; set; }
        [ForeignKey("SaleMeasurement")]
        [Column("SaleMeasurementId")]
	    public string SaleMeasurementId { get; set; }
	      
        public virtual Measurement SaleMeasurement { get; set; }
        [Column("SaleQuantity")]
	    public double? SaleQuantity { get; set; }
        [Column("SaleUnitPrice")]
	    public double? SaleUnitPrice { get; set; }
        [Column("SaleTotalAmount")]
	    public double? SaleTotalAmount { get; set; }
        [Column("SaleTotalAmountLocal")]
	    public double? SaleTotalAmountLocal { get; set; }
        [Column("SaleContainerType1UnitPrice")]
	    public double? SaleContainerType1UnitPrice { get; set; }
        [Column("SaleContainerType2UnitPrice")]
	    public double? SaleContainerType2UnitPrice { get; set; }
        [Column("SaleContainerType3UnitPrice")]
	    public double? SaleContainerType3UnitPrice { get; set; }
        [Column("SaleContainerType4UnitPrice")]
	    public double? SaleContainerType4UnitPrice { get; set; }
        [Column("SaleContainerType5UnitPrice")]
	    public double? SaleContainerType5UnitPrice { get; set; }
        [Column("CostMaxAmount")]
	    public double? CostMaxAmount { get; set; }
        [Column("CostMinAmount")]
	    public double? CostMinAmount { get; set; }
        [Column("SaleMaxAmount")]
	    public double? SaleMaxAmount { get; set; }
        [Column("SaleMinAmount")]
	    public double? SaleMinAmount { get; set; }
        [Column("IsChargeBySteps")]
	    public bool IsChargeBySteps { get; set; }
        [Column("SaleIsFixedRate")]
	    public bool SaleIsFixedRate { get; set; }
        [Column("CostAmountInSaleCurrency")]
	    public double? CostAmountInSaleCurrency { get; set; }
        [ForeignKey("VatType")]
        [Column("VatTypeId")]
	    public string VatTypeId { get; set; }
	      
        public virtual VatType VatType { get; set; }
        [Column("VatPercentage")]
	    public double? VatPercentage { get; set; }
        [Column("SaleUnitPriceInSaleCurrency")]
	    public double? SaleUnitPriceInSaleCurrency { get; set; }
        [Column("SaleUnitPrice1InSaleCurrency")]
	    public double? SaleUnitPrice1InSaleCurrency { get; set; }
        [Column("SaleUnitPrice2InSaleCurrency")]
	    public double? SaleUnitPrice2InSaleCurrency { get; set; }
        [Column("SaleUnitPrice3InSaleCurrency")]
	    public double? SaleUnitPrice3InSaleCurrency { get; set; }
        [Column("SaleUnitPrice4InSaleCurrency")]
	    public double? SaleUnitPrice4InSaleCurrency { get; set; }
        [Column("SaleUnitPrice5InSaleCurrency")]
	    public double? SaleUnitPrice5InSaleCurrency { get; set; }
        [Column("SaleAmountInSaleCurrency")]
	    public double? SaleAmountInSaleCurrency { get; set; }
        [Column("IsCostAllIn")]
	    public bool IsCostAllIn { get; set; }
        [Column("TariffId")]
	    public string TariffId { get; set; }
        [Column("TariffNumber")]
	    public string TariffNumber { get; set; }
        [Column("TariffVersion")]
	    public int TariffVersion { get; set; }
        [Column("IsRegionalTax")]
	    public bool IsRegionalTax { get; set; }
        [Column("TariffLineId")]
	    public string TariffLineId { get; set; }
    }
}
	 