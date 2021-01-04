using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ShipmentTests.Steps.Shipment
{
    public class ShipmentPackagePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }
        public double? Weight { get; set; }
        public double? Width { get; set; }
        public int? Quantity { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public int ChangeSetOp { get; set; }

    }
}
