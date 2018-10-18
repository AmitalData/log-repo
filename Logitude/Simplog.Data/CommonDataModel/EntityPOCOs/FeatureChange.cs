using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class FeatureChange
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }

        public DateTime? EventDateTime { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string PackageCode { get; set; }
        public string Notes { get; set; }
        public string SearchFields { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [ForeignKey("PackageCode")]
        public virtual Package Package { get; set; }
    }
}
