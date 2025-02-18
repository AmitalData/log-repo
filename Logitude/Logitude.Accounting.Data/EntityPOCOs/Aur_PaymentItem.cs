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
   
    public class Aur_PaymentItem
    {
	 string dbms;

        [Key]
        [ForeignKey("Aur_Payment")]
        [Column("PaymentId")]
	    public string PaymentId { get; set; }
	      
        public virtual Aur_Payment Aur_Payment { get; set; }
     [Key]
        [Column("Line")]
	    public int Line { get; set; }
        [Column("PaymentSequence")]
	    public int? PaymentSequence { get; set; }
        [Column("QuoteId")]
	    public string QuoteId { get; set; }
        [Column("Project")]
	    public string Project { get; set; }
        [Column("ProjectNumber")]
	    public string ProjectNumber { get; set; }
        [Column("SectionType")]
	    public string SectionType { get; set; }
        [Column("BaseAmount")]
	    public decimal BaseAmount { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
    }
}
	 