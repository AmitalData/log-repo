using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityAMs
{
    public class ShipmentPackageAM
    {
        public string ContainerNumber { get; set; }
        public int? Quantity { get; set; } 
        public double? Weight { get; set; }
        public string PackageTypeId { get; set; }
        public string PackageTypeCode { get; set; }
        public string ShipperSeal { get; set; }
        public double? Volume { get; set; }

    }
}
