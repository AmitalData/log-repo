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
   
    public class Aur_Item
    {
	 string dbms;

        [Key]
        [Column("PaymentId")]
	    public string PaymentId { get; set; }
     [Key]
        [Column("Line")]
	    public int Line { get; set; }
        [Column("SalesOrderid")]
	    public string SalesOrderid { get; set; }
        [Column("RelatedContract")]
	    public string RelatedContract { get; set; }
        [Column("ProductNumber")]
	    public string ProductNumber { get; set; }
        [Column("ProductName")]
	    public string ProductName { get; set; }
        [Column("PricePerUnit")]
	    public decimal? PricePerUnit { get; set; }
        [Column("Quantity")]
	    public decimal Quantity { get; set; }
        [Column("Discount")]
	    public decimal? Discount { get; set; }
        [Column("BaseAmount")]
	    public decimal BaseAmount { get; set; }
        [Column("Tax")]
	    public decimal Tax { get; set; }
        [Column("ExtendedAmount")]
	    public decimal ExtendedAmount { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
    }
}
	 