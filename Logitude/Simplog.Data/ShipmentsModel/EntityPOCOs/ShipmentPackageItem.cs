using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentPackageItem
    {
        [Key]
        public string PackageId { get; set; }
        [Key]
        public int LineNumber { get; set; }

        public int Tenant { get; set; }
        public string Description { get; set; }
        public int? Quantity { get; set; }
        public decimal? GoodsValue { get; set; }

        [ForeignKey("PackageId")]
        public virtual ShipmentPackage ShipmentPackage { get; set; }
    }
}
