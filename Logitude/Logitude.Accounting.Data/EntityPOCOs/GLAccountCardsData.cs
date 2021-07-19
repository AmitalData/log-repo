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
   
    public class GLAccountCardsData
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("SalesmanUser")]
        [Column("SalesmanUserId")]
	    public string SalesmanUserId { get; set; }
	      
        public virtual User SalesmanUser { get; set; }
        [Column("CreditLimit")]
	    public double? CreditLimit { get; set; }
        [ForeignKey("PaymentTerm")]
        [Column("PaymentTermId")]
	    public string PaymentTermId { get; set; }
	      
        public virtual PaymentTerm PaymentTerm { get; set; }
        [ForeignKey("CollectorUser")]
        [Column("CollectorUserId")]
	    public string CollectorUserId { get; set; }
	      
        public virtual User CollectorUser { get; set; }
        [Column("Phone")]
	    public string Phone { get; set; }
        [Column("VatNumber")]
	    public string VatNumber { get; set; }
        [Column("TotalOpenShipments")]
	    public decimal? TotalOpenShipments { get; set; }
        [Column("InsuredcreditLimit")]
	    public double? InsuredcreditLimit { get; set; }
    }
}
	 