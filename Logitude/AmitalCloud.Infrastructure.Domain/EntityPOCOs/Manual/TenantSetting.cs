using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class TenantSetting
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

        //[Include]
        //[Association("TenantSettingObjectTable", "ObjectTableId", "Id", IsForeignKey = true)]
        [ForeignKey("ObjectTableId")]
        public ObjectTable ObjectTable { get; set; }

        public bool IsDocumentFilingByEmailEnabled { get; set; }
    }
}