using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CustomerTenantAccessCardList
    {

        [Key]
        public string CustomerId { get; set; }
        [Key]
        public string CustomerTenantAccessId { get; set; }
        public int Tenant { get; set; }
        public DateTime? LastShipmentDateInQueue { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateByUserId { get; set; }
        public DateTime HybridStartDate { get; set; }

        public DateTime? LastMappingDateTime { get; set; }

        public DateTime UpdateDateTime { get; set; }

        public string StatusTypeCode { get; set; }
        public string StatusType { get; set; }
    }
}
