using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class TenantSettingPM
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        public string SettingCode { get; set; }

        public string SettingValue { get; set; }

        public string ObjectTableId { get; set; }

        public int Size { get; set; }

        public string Prefix { get; set; }

        public bool DontIncludeDirects { get; set; }

        public bool IsDocumentFilingByEmailEnabled { get; set; }

    }
}