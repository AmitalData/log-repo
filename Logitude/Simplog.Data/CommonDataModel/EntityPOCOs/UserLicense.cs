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
    public class UserLicense
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        public string PackageCode { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("PackageCode")]
        public Package Package { get; set; }
    }
}
