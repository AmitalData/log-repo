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
   
    public class DeclarationPending
    {
	 string dbms;

           [Column("Tenant")]
	    public int Tenant { get; set; }
     [Key]
        [ForeignKey("Declaration")]
        [Column("DeclarationID" ,Order = 1)]
	    public string DeclarationID { get; set; }
	      
        public virtual Declaration Declaration { get; set; }
     [Key]
        [ForeignKey("CourierPendingReason")]
        [Column("CourierPendingReasonCode")]
	    public string CourierPendingReasonCode { get; set; }
	      
        public virtual CourierPendingReason CourierPendingReason { get; set; }
        [Column("PendingRemarks")]
	    public string PendingRemarks { get; set; }
        [Column("Status")]
	    public string Status { get; set; }
    }
}
	 