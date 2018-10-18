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
   
    public class PaymentChequeLine
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("PaymentCheque")]
        [Column("PaymentChequeId" ,Order = 1)]
	    public string PaymentChequeId { get; set; }
	      
        public virtual PaymentCheque PaymentCheque { get; set; }
     [Key]
        [Column("Line" ,Order = 2)]
	    public int Line { get; set; }
        [Column("Notes")]
	    public string Notes { get; set; }
        [Column("Amount")]
	    public decimal? Amount { get; set; }
        [Column("SequenceNumeric")]
	    public int? SequenceNumeric { get; set; }
    }
}
	 