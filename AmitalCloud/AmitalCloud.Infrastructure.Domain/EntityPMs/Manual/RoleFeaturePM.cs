using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class RoleFeaturePM
    {
        public RoleFeaturePM() { }
        public RoleFeaturePM(RoleFeature a)
        {
            FeatureId = a.FeatureId;
            Id = a.Id;
            Tenant = a.Tenant;
            RoleId = a.RoleId;
            FeatureAccessLevelCode = a.FeatureAccessLevelCode;
            FeatureUniqeCode = a.FeatureUniqeCode;
        }

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string RoleId { get; set; }
        public string FeatureId { get; set; }
        public string FeatureAccessLevelCode { get; set; }
        public string FeatureUniqeCode { get; set; }

    }
}