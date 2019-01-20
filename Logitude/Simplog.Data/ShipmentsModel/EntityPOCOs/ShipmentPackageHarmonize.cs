using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.EntityPOCOs
{
    public class ShipmentPackageHarmonize
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PackageId { get; set; }
        public string Harmonize { get; set; }

        [ForeignKey("PackageId")]
        public virtual ShipmentPackage Package { get; set; }
    }
}
