using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class CustomerTenantAccessCard
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

        public int Tenant { get; set; }
         
        public DateTime? LastShipmentDateInQueue { get; set; }
        public DateTime CreateDate { get; set; }


        public string CreateByUserId { get; set; }
        [ForeignKey("CreateByUserId")]
        public virtual User CreateByUser { get; set; }

        //public DateTime HybridStartDate { get; set; }

        public DateTime? LastMappingDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }


        [ForeignKey("StatusType")] 
        public string StatusTypeCode { get; set; }

        public virtual CustomerTenantAccessStatusType StatusType { get; set; }
        public bool IsExportActivated { get; set; }
        public bool IsImportActivated { get; set; }
    }
}
