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
   
    public class VendorCommission
    {
	 string dbms;

        [Key]
        [ForeignKey("Vendor")]
        [Column("VendorId" ,Order = 1)]
	    public string VendorId { get; set; }
	      
        public virtual CustomsVendor Vendor { get; set; }
     [Key]
        [ForeignKey("Customer")]
        [Column("CustomerId" ,Order = 2)]
	    public string CustomerId { get; set; }
	      
        public virtual Customer Customer { get; set; }
        [Column("CommisionPercentage")]
	    public decimal? CommisionPercentage { get; set; }
        [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("ModificationAndDiscountType")]
        [Column("ModificationsTypeCode" ,Order = 3)]
	    public string ModificationsTypeCode { get; set; }
	      
        public virtual ModificationAndDiscountType ModificationAndDiscountType { get; set; }
    }
}
	 