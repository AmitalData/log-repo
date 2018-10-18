using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class CustomerTenantAccessCardsBatch
    {
        [Key]
        [ForeignKey("Customer")]
        [Column("CustomerId", Order = 1)]
        public string CustomerId { get; set; }
        public virtual Customer Customer { get; set; }


        [Key]
        [ForeignKey("CustomerTenantAccess")]
        [Column("CustomerTenantAccessId", Order = 2)]
        public string CustomerTenantAccessId { get; set; }
        public virtual CustomerTenantAccess CustomerTenantAccess { get; set; }


        [Key]
        [Column("BatchNumber", Order = 3)]
        public string BatchNumber { get; set; }

        public int Tenant { get; set; }

        public DateTime? DoneDate { get; set; }

        public string Status { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime? FromDatetime { get; set; }

        public DateTime? ToDatetime { get; set; }

        public int TotalShipment { get; set; }

        public int Totalsucceeded { get; set; }

        public int TotalFailed { get; set; }


    }
}
