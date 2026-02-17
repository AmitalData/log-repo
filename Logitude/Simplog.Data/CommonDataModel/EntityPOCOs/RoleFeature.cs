using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class RoleFeature
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string RoleId { get; set; }
        public string FeatureId { get; set; }
        public string FeatureAccessLevelCode  { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("FeatureId")]
        public Feature Feature { get; set; }

        [ForeignKey("FeatureAccessLevelCode")]
        public FeatureAccessLevel FeatureAccessLevel { get; set; }
    }
}