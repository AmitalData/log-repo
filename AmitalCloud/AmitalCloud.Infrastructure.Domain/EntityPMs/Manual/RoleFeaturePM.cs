using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class RoleFeaturePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string RoleId { get; set; }
        public string FeatureId { get; set; }
        public string FeatureAccessLevelCode { get; set; }
        public string FeatureUniqeCode { get; set; }

    }
}