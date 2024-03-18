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
   
    public class CB_ComputationMethodData
    {
	 string dbms;

        [Key]
        [Column("ID")]
	    public int ID { get; set; }
        [Column("AlternateDefinedPerUnitMeasure")]
	    public decimal? AlternateDefinedPerUnitMeasure { get; set; }
        [Column("AlternateRate")]
	    public decimal? AlternateRate { get; set; }
        [Column("CalculationReference")]
	    public string CalculationReference { get; set; }
        [ForeignKey("ComputationMethodCode")]
        [Column("ComputationMethodID")]
	    public string ComputationMethodID { get; set; }
	      
        public virtual ComputationMethod ComputationMethodCode { get; set; }
        [ForeignKey("CurrencyTypeCode")]
        [Column("CurrencyTypeID")]
	    public string CurrencyTypeID { get; set; }
	      
        public virtual CurrencyType CurrencyTypeCode { get; set; }
        [Column("DefinedPerUnitMethod")]
	    public decimal? DefinedPerUnitMethod { get; set; }
        [Column("EnglishCalculationReference")]
	    public string EnglishCalculationReference { get; set; }
        [ForeignKey("MeasurementUnitCode")]
        [Column("MeasurementUnitID")]
	    public string MeasurementUnitID { get; set; }
	      
        public virtual MeasurmentUnit MeasurementUnitCode { get; set; }
        [ForeignKey("Alternate_MeasurementUnitCode")]
        [Column("Alternate_MeasurementUnitID")]
	    public string Alternate_MeasurementUnitID { get; set; }
	      
        public virtual MeasurmentUnit Alternate_MeasurementUnitCode { get; set; }
        [Column("OptionalTaxAddition")]
	    public decimal? OptionalTaxAddition { get; set; }
        [Column("Rate")]
	    public decimal? Rate { get; set; }
        [Column("ReductionRate")]
	    public decimal? ReductionRate { get; set; }
        [ForeignKey("TariffRelatedToQuotaCode")]
        [Column("TariffRelatedToQuotaID")]
	    public string TariffRelatedToQuotaID { get; set; }
	      
        public virtual TarifRelatedToQuota TariffRelatedToQuotaCode { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("EnglishNotes")]
	    public string EnglishNotes { get; set; }
    }
}
	 