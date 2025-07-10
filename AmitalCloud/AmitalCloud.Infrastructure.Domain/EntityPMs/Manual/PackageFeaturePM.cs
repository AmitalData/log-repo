using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class PackageFeaturePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string PackageCode { get; set; }
        public string FeatureId { get; set; }
        public string FeatureUniqeCode { get; set; }

    }
}