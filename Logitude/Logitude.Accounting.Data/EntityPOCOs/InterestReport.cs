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
   
    public class InterestReport
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("CreateDateTime")]
	    public DateTime? CreateDateTime { get; set; }
        [ForeignKey("CreatedByUser")]
        [Column("CreatedByUserId")]
	    public string CreatedByUserId { get; set; }
	      
        public virtual User CreatedByUser { get; set; }
        [Column("UpdateDateTime")]
	    public DateTime? UpdateDateTime { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [ForeignKey("GLAccount")]
        [Column("GLAccountId")]
	    public string GLAccountId { get; set; }
	      
        public virtual GLAccount GLAccount { get; set; }
        [Column("ReportNumber")]
	    public string ReportNumber { get; set; }
        [Column("InterestCalculationDate")]
	    public DateTime InterestCalculationDate { get; set; }
        [Column("TotalAmount")]
	    public decimal? TotalAmount { get; set; }
        [Column("OpenBalance")]
	    public decimal? OpenBalance { get; set; }
        [Column("CloseBalance")]
	    public decimal? CloseBalance { get; set; }
        [ForeignKey("ARInvoice")]
        [Column("ARinvoiceId")]
	    public string ARinvoiceId { get; set; }
	      
        public virtual ARInvoice ARInvoice { get; set; }
        [Column("InvoiceAmount")]
	    public decimal? InvoiceAmount { get; set; }
        [Column("GLAccountInterestCreditLimit")]
	    public decimal? GLAccountInterestCreditLimit { get; set; }
        [ForeignKey("InterestReportStatuse")]
        [Column("InterestReportStatusCode")]
	    public string InterestReportStatusCode { get; set; }
	      
        public virtual InterestReportStatuse InterestReportStatuse { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("Card")]
        [Column("CustomerId")]
	    public string CustomerId { get; set; }
	      
        public virtual Card Card { get; set; }
        [Column("InvoiceFailureReason")]
	    public string InvoiceFailureReason { get; set; }
        [Column("CreditAllotmentPercentage")]
	    public decimal? CreditAllotmentPercentage { get; set; }
        [Column("CalCreditAllotmentCommission")]
	    public decimal? CalCreditAllotmentCommission { get; set; }
        [Column("CalculatedPostponedCheques")]
	    public decimal? CalculatedPostponedCheques { get; set; }
    }
}
	 