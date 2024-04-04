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
   
    public class CB_QuotaDetailsHistory
    {
	 string dbms;

           [Column("ID")]
	    public int ID { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("UpdateDate")]
	    public DateTime? UpdateDate { get; set; }
        [Column("StartDate")]
	    public DateTime? StartDate { get; set; }
        [Column("EndDate")]
	    public DateTime? EndDate { get; set; }
        [ForeignKey("EntityStatusCode")]
        [Column("EntityStatusID")]
	    public string EntityStatusID { get; set; }
	      
        public virtual CustomsEntityStatus EntityStatusCode { get; set; }
        [Column("IsImportLicenseRequired")]
	    public bool IsImportLicenseRequired { get; set; }
        [ForeignKey("MeasurementUnitCode")]
        [Column("MeasurementUnitID")]
	    public string MeasurementUnitID { get; set; }
	      
        public virtual MeasurmentUnit MeasurementUnitCode { get; set; }
        [Column("Quantity")]
	    public int? Quantity { get; set; }
        [ForeignKey("QuotaComputationBasisCode")]
        [Column("QuotaComputationBasisID")]
	    public string QuotaComputationBasisID { get; set; }
	      
        public virtual QuotaComputationBasis QuotaComputationBasisCode { get; set; }
        [ForeignKey("QuotaIncrementCode")]
        [Column("QuotaIncrementID")]
	    public string QuotaIncrementID { get; set; }
	      
        public virtual QuotaIncrement QuotaIncrementCode { get; set; }
        [ForeignKey("RenewalMethodCode")]
        [Column("RenewalMethodID")]
	    public string RenewalMethodID { get; set; }
	      
        public virtual RenewalMethod RenewalMethodCode { get; set; }
        [Column("RenewalUntilDate")]
	    public DateTime? RenewalUntilDate { get; set; }
        [Column("QuotaID")]
	    public int QuotaID { get; set; }
        [Column("ChangeRequestTypePriority")]
	    public int ChangeRequestTypePriority { get; set; }
        [Column("QuotaValueIncrement")]
	    public decimal? QuotaValueIncrement { get; set; }
        [ForeignKey("PerYearFrequencyCode")]
        [Column("PerYearFrequency")]
	    public string PerYearFrequency { get; set; }
	      
        public virtual PerYearFrequency PerYearFrequencyCode { get; set; }
        [ForeignKey("CurrencyTypeCode")]
        [Column("CurrencyTypeID")]
	    public string CurrencyTypeID { get; set; }
	      
        public virtual CurrencyType CurrencyTypeCode { get; set; }
     [Key]
        [Column("CB_ID")]
	    public string CB_ID { get; set; }
    }
}
	 