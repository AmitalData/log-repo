using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class RoleFeature
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string RoleId { get; set; }
        public string FeatureId { get; set; }
        public string FeatureAccessLevelCode { get; set; }
        public bool IsDeleted { get; set; }
        public string FeatureUniqeCode { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        //[ForeignKey("FeatureId")]
        public Feature Feature { get; set; }

        [ForeignKey("FeatureAccessLevelCode")]
        public FeatureAccessLevel FeatureAccessLevel { get; set; }
    }
}
