using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.EntityPOCOs
{
    public class PackageConnectedPackage
    {
        [Key]
        public string Id { get; set; }
        public string PackageCode { get; set; }
        public string ConnectedPackageCode { get; set; }

        [ForeignKey("PackageCode")]
        public Package Package { get; set; }

        [ForeignKey("ConnectedPackageCode")]
        public Package ConnectedPackage { get; set; }
    }
}
