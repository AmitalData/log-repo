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
   
    public class ARPaymentsJournal
    {
	 string dbms;

        [Key]
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("Payment")]
        [Column("PaymentId")]
	    public string PaymentId { get; set; }
	      
        public virtual ARPayment Payment { get; set; }
     [Key]
        [Column("IsVoided")]
	    public bool IsVoided { get; set; }
    }
}
	 