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
   
    public class JournalLine
    {
	 string dbms;

        [Key]
        [ForeignKey("Journal")]
        [Column("JournalId" ,Order = 1)]
	    public string JournalId { get; set; }
	      
        public virtual Journal Journal { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [Column("Line" ,Order = 2)]
	    public int Line { get; set; }
        [Column("ActionCode")]
	    public string ActionCode { get; set; }
        [ForeignKey("DebitControlAccount")]
        [Column("DebitControlAccountId")]
	    public string DebitControlAccountId { get; set; }
	      
        public virtual GLAccount DebitControlAccount { get; set; }
        [ForeignKey("DebitAccount")]
        [Column("DebitAccountId")]
	    public string DebitAccountId { get; set; }
	      
        public virtual GLAccount DebitAccount { get; set; }
        [ForeignKey("CreditControlAccount")]
        [Column("CreditControlAccountId")]
	    public string CreditControlAccountId { get; set; }
	      
        public virtual GLAccount CreditControlAccount { get; set; }
        [ForeignKey("CreditAccount")]
        [Column("CreditAccountId")]
	    public string CreditAccountId { get; set; }
	      
        public virtual GLAccount CreditAccount { get; set; }
        [Column("DocumentDate")]
	    public DateTime DocumentDate { get; set; }
        [Column("AccountingDate")]
	    public DateTime AccountingDate { get; set; }
        [Column("DueDate")]
	    public DateTime DueDate { get; set; }
        [Column("LocalAmount")]
	    public decimal LocalAmount { get; set; }
        [ForeignKey("Currency")]
        [Column("CurrencyId")]
	    public string CurrencyId { get; set; }
	      
        public virtual Currency Currency { get; set; }
        [Column("ForeignAmount")]
	    public decimal ForeignAmount { get; set; }
        [Column("ExchangeRate")]
	    public decimal? ExchangeRate { get; set; }
        [Column("Reference1")]
	    public string Reference1 { get; set; }
        [Column("Reference2")]
	    public string Reference2 { get; set; }
        [Column("Reference3")]
	    public string Reference3 { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("ExternalOpenAmount")]
	    public decimal? ExternalOpenAmount { get; set; }
        [Column("ExternalReconcileNumber")]
	    public string ExternalReconcileNumber { get; set; }
        [Column("IsExternalReconcile")]
	    public bool IsExternalReconcile { get; set; }
        [ForeignKey("JournalActionType")]
        [Column("ActionId")]
	    public string ActionId { get; set; }
	      
        public virtual JournalActionType JournalActionType { get; set; }
    }
}
	 