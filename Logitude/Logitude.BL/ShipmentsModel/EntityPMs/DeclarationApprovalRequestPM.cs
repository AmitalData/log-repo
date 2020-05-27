using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityPMs
{
    public class DeclarationApprovalRequestPM
    {
        public string ForwarderShipmentNumber { get; set; }
        public string Remarks { get; set; }
        public int Tenant { get; set; }
        public string DeclarationXmlData { get; set; }
    }
}
