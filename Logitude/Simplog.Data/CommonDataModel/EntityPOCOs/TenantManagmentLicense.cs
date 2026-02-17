using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TenantManagmentLicense
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PackageCode { get; set; }
        public int? NumberOfUsers { get; set; }

        [ForeignKey("PackageCode")]
        public Package Package { get; set; }
    }
}
