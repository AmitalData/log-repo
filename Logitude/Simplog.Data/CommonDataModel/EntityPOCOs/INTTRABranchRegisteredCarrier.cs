using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class INTTRABranchRegisteredCarrier
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime? UpdateDate { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }
        public string UpdatedByUserId { get; set; }

        [ForeignKey("ShippingLineId")]
        public virtual ShippingLine ShippingLine { get; set; }
        public string ShippingLineId { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
        public string BranchId { get; set; }
    }
}
