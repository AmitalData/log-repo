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

namespace Logitude.Customs.Data.EntityPOCOs
{
   
    public class PaymentOrderConnectionTable
    {
	 string dbms;

        [Key]
        [ForeignKey("PaymentOrder")]
        [Column("PaymentOrderId" ,Order = 1)]
	    public string PaymentOrderId { get; set; }
	      
        public virtual PaymentOrder PaymentOrder { get; set; }
        [Column("ConnectedEntityCode")]
	    public string ConnectedEntityCode { get; set; }
     [Key]
        [Column("ConnectedEntityId" ,Order = 2)]
	    public string ConnectedEntityId { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
    }
}
	 