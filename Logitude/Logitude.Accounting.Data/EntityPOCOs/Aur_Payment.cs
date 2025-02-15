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
   
    public class Aur_Payment
    {
	 string dbms;

        [Key]
        [Column("Id")]
	    public string Id { get; set; }
        [Column("CreateDate")]
	    public DateTime CreateDate { get; set; }
        [Column("DraftNumber")]
	    public string DraftNumber { get; set; }
        [Column("ForMonth")]
	    public string ForMonth { get; set; }
        [Column("SaleOrder")]
	    public string SaleOrder { get; set; }
        [Column("Customer")]
	    public string Customer { get; set; }
        [Column("CustomerReference1")]
	    public string CustomerReference1 { get; set; }
        [Column("InvoiceType")]
	    public string InvoiceType { get; set; }
        [Column("PaymentRequestStatus")]
	    public string PaymentRequestStatus { get; set; }
        [Column("ErrorMessage")]
	    public string ErrorMessage { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
    }
}
	 