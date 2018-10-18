using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CustomerTenantAccessCardsBatchList
    {
       
        public string CustomerId { get; set; }

       
        public string CustomerTenantAccessId { get; set; }

        
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
