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
   
    public class BankDepositLine
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("BankDeposit")]
        [Column("DepositId" ,Order = 1)]
	    public string DepositId { get; set; }
	      
        public virtual BankDeposit BankDeposit { get; set; }
     [Key]
        [Column("Line" ,Order = 2)]
	    public int Line { get; set; }
        [ForeignKey("ARPaymentCheque")]
        [Column("ARPaymentChequeId")]
	    public string ARPaymentChequeId { get; set; }
	      
        public virtual ARPaymentCheque ARPaymentCheque { get; set; }
        [Column("IsOutOfDeposit")]
	    public bool? IsOutOfDeposit { get; set; }
        [Column("OutOfDepositeDate")]
	    public DateTime? OutOfDepositeDate { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("SearchFields")]
	    public string SearchFields { get; set; }
    }
}
	 