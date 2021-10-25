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
   
    public class ARPaymentBankTranfer
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
        [ForeignKey("Payment")]
        [Column("PaymentId")]
	    public string PaymentId { get; set; }
	      
        public virtual ARPayment Payment { get; set; }
        [Column("LineNumber")]
	    public int LineNumber { get; set; }
        [Column("PaymentRef")]
	    public string PaymentRef { get; set; }
        [Column("ValueDate")]
	    public DateTime ValueDate { get; set; }
        [ForeignKey("BankAccount")]
        [Column("BankAccountId")]
	    public string BankAccountId { get; set; }
	      
        public virtual BankAccount BankAccount { get; set; }
        [ForeignKey("Currency")]
        [Column("CurrencyId")]
	    public string CurrencyId { get; set; }
	      
        public virtual Currency Currency { get; set; }
        [Column("LocalAmount")]
	    public decimal LocalAmount { get; set; }
        [Column("ForeignAmount")]
	    public decimal ForeignAmount { get; set; }
        [Column("ExchageRate")]
	    public decimal ExchageRate { get; set; }
    }
}
	 