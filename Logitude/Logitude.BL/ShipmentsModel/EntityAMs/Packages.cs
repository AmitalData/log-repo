using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityAMs
{
    public class Packages
    { 
        public int? Quantity { get; set; }
        public double? GrossWeight { get; set; } 
        public double? Length { get; set; }
        public double? Width { get; set; } 
        public double? Height { get; set; }
        public string Type { get; set; }

    }
}
