using Simplog.Server.Infrastructure.DataContracts;
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
        public double? Volume { get; set; }
        public string TransportModeId { get; set; }
        public string OrderNumber { get; set; }
        public string AgentName { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public string ShipperName { get; set; }
        public CodeProperties Shipper { get; set; }
        public int? Quantity { get; set; }
        public double? Weight { get; set; }
        public CodeProperties Incoterm { get; set; }
    }
}
