using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
  
    public class DepositionRequestPM
    {

        public int Tenant { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public DateTime? RequestDateTime { get; set; }
        public string ForwarderShipmentNumber { get; set; }

    }
}
