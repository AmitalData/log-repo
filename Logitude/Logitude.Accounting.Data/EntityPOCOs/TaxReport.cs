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

namespace Logitude.Accounting.Data.EntityPOCOs
{
   
    public class TaxReport
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("LastUpdateDate")]
	    public DateTime LastUpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("TaxReportMonth")]
	    public DateTime TaxReportMonth { get; set; }
        [Column("TaxReportNumber")]
	    public string TaxReportNumber { get; set; }
        [Column("VatNumber")]
	    public string VatNumber { get; set; }
        [Column("TaxReportTypeCode")]
	    public string TaxReportTypeCode { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [Column("TaxableOutputAmount")]
	    public decimal? TaxableOutputAmount { get; set; }
        [Column("OutputTaxAmount")]
	    public decimal? OutputTaxAmount { get; set; }
        [Column("TaxableOutputsWithDiffPercent")]
	    public decimal? TaxableOutputsWithDiffPercent { get; set; }
        [Column("OutputTaxAmountWithDiffPercent")]
	    public decimal? OutputTaxAmountWithDiffPercent { get; set; }
        [Column("ExemptTaxableOutput")]
	    public decimal? ExemptTaxableOutput { get; set; }
        [Column("OutputLinesCount")]
	    public int? OutputLinesCount { get; set; }
        [Column("OtherInputsTaxAmount")]
	    public decimal? OtherInputsTaxAmount { get; set; }
        [Column("EquipmentInputsTaxAmount")]
	    public decimal? EquipmentInputsTaxAmount { get; set; }
        [Column("InputLinesCount")]
	    public int? InputLinesCount { get; set; }
        [Column("AmountForPayRefund")]
	    public decimal? AmountForPayRefund { get; set; }
        [ForeignKey("VatReportStatus")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual VatReportStatus VatReportStatus { get; set; }
        [Column("ProcessStartDate")]
	    public DateTime? ProcessStartDate { get; set; }
        [Column("ProcessEndDate")]
	    public DateTime? ProcessEndDate { get; set; }
        [Column("ProcessProgress")]
	    public int? ProcessProgress { get; set; }
        [Column("NeedsRebulid")]
	    public bool NeedsRebulid { get; set; }
    }
}
	 