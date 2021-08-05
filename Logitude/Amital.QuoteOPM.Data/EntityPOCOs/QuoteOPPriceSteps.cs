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

namespace Amital.QuoteOPM.Data.EntityPOCOs
{
   
    public class QuoteOPPriceSteps
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
        [ForeignKey("QuoteOP")]
        [Column("QuoteOPId")]
	    public string QuoteOPId { get; set; }
	      
        public virtual QuoteOP QuoteOP { get; set; }
        [ForeignKey("QuoteOPCharge")]
        [Column("QuoteOPChargeId")]
	    public string QuoteOPChargeId { get; set; }
	      
        public virtual QuoteOPCharge QuoteOPCharge { get; set; }
        [Column("Step")]
	    public double? Step { get; set; }
        [Column("CostUnitPrice")]
	    public double? CostUnitPrice { get; set; }
        [Column("SaleUnitPrice")]
	    public double? SaleUnitPrice { get; set; }
        [Column("MarkupValue")]
	    public double? MarkupValue { get; set; }
    }
}
	 