using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AmitalCloud.Shipment.Def.EntityPMs
{
    public partial class DeclarationApprovalRequestPM : EntityPM
    {
        public string ForwarderShipmentNumber { get; set; }
        public string Remarks { get; set; }
        public int Tenant { get; set; }
        public string DeclarationXmlData { get; set; }
    }
}
