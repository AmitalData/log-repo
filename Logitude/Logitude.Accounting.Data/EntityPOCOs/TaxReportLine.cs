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
   
    public class TaxReportLine
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("LastUpdateDateTime")]
	    public DateTime LastUpdateDateTime { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
     [Key]
        [ForeignKey("TaxReport")]
        [Column("TaxReportId" ,Order = 1)]
	    public string TaxReportId { get; set; }
	      
        public virtual TaxReport TaxReport { get; set; }
     [Key]
        [Column("Line" ,Order = 2)]
	    public int Line { get; set; }
        [Column("OutputOrInput")]
	    public string OutputOrInput { get; set; }
        [ForeignKey("TaxReportLineType")]
        [Column("LineTypeCode")]
	    public string LineTypeCode { get; set; }
	      
        public virtual TaxReportLineType TaxReportLineType { get; set; }
        [Column("VatNumber")]
	    public string VatNumber { get; set; }
        [Column("Reference")]
	    public string Reference { get; set; }
        [Column("ReferecneGroup")]
	    public string ReferecneGroup { get; set; }
        [Column("ReferenceDate")]
	    public DateTime? ReferenceDate { get; set; }
        [Column("VatAmount")]
	    public decimal? VatAmount { get; set; }
        [Column("VatableInvoiceAmount")]
	    public decimal? VatableInvoiceAmount { get; set; }
        [ForeignKey("TaxReportLineStatus")]
        [Column("StatusCode")]
	    public string StatusCode { get; set; }
	      
        public virtual TaxReportLineStatus TaxReportLineStatus { get; set; }
        [ForeignKey("TaxReportLineTransmitStatus")]
        [Column("TransmitStatusCode")]
	    public string TransmitStatusCode { get; set; }
	      
        public virtual TaxReportLineTransmitStatus TaxReportLineTransmitStatus { get; set; }
        [ForeignKey("Journal")]
        [Column("JournalId")]
	    public string JournalId { get; set; }
	      
        public virtual Journal Journal { get; set; }
        [Column("IsManuallyChanged")]
	    public bool? IsManuallyChanged { get; set; }
        [Column("IsEquipment")]
	    public bool IsEquipment { get; set; }
        [Column("TaxReportDate")]
	    public DateTime? TaxReportDate { get; set; }
        [Column("IsExternalLine")]
	    public bool IsExternalLine { get; set; }
        [Column("TotalInvoiceAmount")]
	    public decimal? TotalInvoiceAmount { get; set; }
        [Column("OriginalReference")]
	    public string OriginalReference { get; set; }
        [Column("JournalLineNumber")]
	    public int JournalLineNumber { get; set; }
        [Column("PreviousReference")]
	    public string PreviousReference { get; set; }
        [Column("VatAmountRound")]
	    public decimal? VatAmountRound { get; set; }
        [Column("LedgerTransactionId")]
	    public string LedgerTransactionId { get; set; }
        [Column("SubTotalInLocalCurrency")]
	    public double? SubTotalInLocalCurrency { get; set; }
        [Column("ConfirmationNumber")]
	    public string ConfirmationNumber { get; set; }
    }
}
	 