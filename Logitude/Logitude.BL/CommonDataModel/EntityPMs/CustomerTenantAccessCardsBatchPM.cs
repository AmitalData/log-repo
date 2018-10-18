using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
   public class CustomerTenantAccessCardsBatchPM
    {
        [Key]
        public string CustomerId { get; set; }

        [Key]
        public string CustomerTenantAccessId { get; set; }

        [Key]
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
