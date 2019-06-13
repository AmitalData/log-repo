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
   
    public class PaymentCheque
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
        [Column("UpdateDate")]
	    public DateTime UpdateDate { get; set; }
        [ForeignKey("UpdatedByUser")]
        [Column("UpdatedByUserId")]
	    public string UpdatedByUserId { get; set; }
	      
        public virtual User UpdatedByUser { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [Column("InternalNumber")]
	    public string InternalNumber { get; set; }
        [Column("ChequeNumber")]
	    public string ChequeNumber { get; set; }
        [ForeignKey("PayToGLAccount")]
        [Column("PayToGLAccountId")]
	    public string PayToGLAccountId { get; set; }
	      
        public virtual GLAccount PayToGLAccount { get; set; }
        [Column("PayToName")]
	    public string PayToName { get; set; }
        [ForeignKey("BankAccount")]
        [Column("BankAccountId")]
	    public string BankAccountId { get; set; }
	      
        public virtual BankAccount BankAccount { get; set; }
        [ForeignKey("BankAccountGLAccount")]
        [Column("BankAccountGLAccountId")]
	    public string BankAccountGLAccountId { get; set; }
	      
        public virtual GLAccount BankAccountGLAccount { get; set; }
        [Column("LocalAmount")]
	    public decimal? LocalAmount { get; set; }
        [ForeignKey("Currency")]
        [Column("CurrencyId")]
	    public string CurrencyId { get; set; }
	      
        public virtual Currency Currency { get; set; }
        [Column("ForeignAmount")]
	    public decimal? ForeignAmount { get; set; }
        [Column("ExchangeRate")]
	    public decimal? ExchangeRate { get; set; }
        [Column("ValueDate")]
	    public DateTime? ValueDate { get; set; }
        [Column("PrintDate")]
	    public DateTime? PrintDate { get; set; }
        [Column("ApproveDate")]
	    public DateTime? ApproveDate { get; set; }
        [ForeignKey("ApprovedByUser")]
        [Column("ApprovedByUserId")]
	    public string ApprovedByUserId { get; set; }
	      
        public virtual User ApprovedByUser { get; set; }
        [Column("IsCancelled")]
	    public bool IsCancelled { get; set; }
        [ForeignKey("CancelledByUser")]
        [Column("CancelledByUserId")]
	    public string CancelledByUserId { get; set; }
	      
        public virtual User CancelledByUser { get; set; }
        [Column("CancelledDate")]
	    public DateTime? CancelledDate { get; set; }
        [Column("CancellationRemarks")]
	    public string CancellationRemarks { get; set; }
        [ForeignKey("PaymentChequeStatus")]
        [Column("PaymentChequeStatusCode")]
	    public string PaymentChequeStatusCode { get; set; }
	      
        public virtual PaymentChequeStatus PaymentChequeStatus { get; set; }
        [Column("EntityId")]
	    public string EntityId { get; set; }
        [ForeignKey("ObjectTable")]
        [Column("ObjectTableId")]
	    public string ObjectTableId { get; set; }
	      
        public virtual ObjectTable ObjectTable { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("UniqueField")]
	    public string UniqueField { get; set; }
        [ForeignKey("APPayment")]
        [Column("APPaymentId")]
	    public string APPaymentId { get; set; }
	      
        public virtual APPayment APPayment { get; set; }
    }
}
	 