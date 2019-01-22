using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityAMs
{
    public class DepositionRequestAM
    {
        public int Tenant { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public DateTime? RequestDateTime { get; set; }
        public string ForwarderShipmentNumber { get; set; }
        public int CustomerTenant { get; set; }
        
    }
}
