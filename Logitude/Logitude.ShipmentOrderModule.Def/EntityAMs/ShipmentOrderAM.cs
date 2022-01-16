using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentOrderModule.Def.EntityAMs
{
    public class ShipmentOrderAM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CustomerShipmentNumber { get; set; }
        public int CustomerTenantNumber { get; set; }
    }
}
