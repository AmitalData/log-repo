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
   
    public class LedgerTransaction
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("JournalLine")]
        [Column("JournalId" ,Order = 1)]
	    public string JournalId { get; set; }
	      
        public virtual JournalLine JournalLine { get; set; }
        [ForeignKey("JournalLine")]
        [Column("JournalLineNumber" ,Order = 2)]
	    public int JournalLineNumber { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [ForeignKey("ControlAccount")]
        [Column("ControlAccountId")]
	    public string ControlAccountId { get; set; }
	      
        public virtual GLAccount ControlAccount { get; set; }
        [ForeignKey("Account")]
        [Column("AccountId")]
	    public string AccountId { get; set; }
	      
        public virtual GLAccount Account { get; set; }
        [Column("AccountingDate")]
	    public DateTime AccountingDate { get; set; }
        [Column("DocumentDate")]
	    public DateTime DocumentDate { get; set; }
        [Column("DueDate")]
	    public DateTime DueDate { get; set; }
        [Column("LocalAmountDebit")]
	    public decimal LocalAmountDebit { get; set; }
        [Column("LocalAmountCredit")]
	    public decimal LocalAmountCredit { get; set; }
        [ForeignKey("Currency")]
        [Column("CurrencyId")]
	    public string CurrencyId { get; set; }
	      
        public virtual Currency Currency { get; set; }
        [Column("ForeignAmountDebit")]
	    public decimal ForeignAmountDebit { get; set; }
        [Column("ForeignAmountCredit")]
	    public decimal ForeignAmountCredit { get; set; }
        [Column("ExchangeRate")]
	    public decimal ExchangeRate { get; set; }
        [Column("Reference1")]
	    public string Reference1 { get; set; }
        [Column("Reference2")]
	    public string Reference2 { get; set; }
        [Column("Reference3")]
	    public string Reference3 { get; set; }
        [Column("OpenAmount")]
	    public decimal OpenAmount { get; set; }
        [ForeignKey("OppositeAccount")]
        [Column("OppositeAccountId")]
	    public string OppositeAccountId { get; set; }
	      
        public virtual GLAccount OppositeAccount { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("OpenAmountCurrency")]
        [Column("OpenAmountCurrencyId")]
	    public string OpenAmountCurrencyId { get; set; }
	      
        public virtual Currency OpenAmountCurrency { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("AmountToReconcile")]
	    public decimal AmountToReconcile { get; set; }
        [Column("Mark")]
	    public bool Mark { get; set; }
        [Column("IsReconciled")]
	    public bool IsReconciled { get; set; }
        [Column("IsExternalReconcile")]
	    public bool IsExternalReconcile { get; set; }
        [Column("InReconcileProgress")]
	    public bool InReconcileProgress { get; set; }
        [Column("ReconcileRemarks")]
	    public string ReconcileRemarks { get; set; }
        [Column("InProgressExternalReconcile")]
	    public bool InProgressExternalReconcile { get; set; }
    }
}
	 