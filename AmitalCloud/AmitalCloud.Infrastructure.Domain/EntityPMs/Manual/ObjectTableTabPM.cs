using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ObjectTableTabPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string ControlPath { get; set; }
        public string TabNameTextCodeId { get; set; }
        public int IndexOrder { get; set; }
        public string TabNameTextCodeDefaultText { get; set; }
        public string ObjectTableName { get; set; }
        public string TabNameTextCodeCode { get; set; }
        public string Code { get; set; }
        public string FeatureId { get; set; }

        public bool IsHidden { get; set; }
        public bool Disabled { get; set; }

        public string HtmlComponentName { get; set; }
        public string HtmlComponentUrl { get; set; }
        public string FeatureUniqeCode { get; set; }

        public string Changeset { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string ScreenCode { get; set; }
        public string ScreenName { get; set; }
        public string OriginalTabCode { get; set; }

        public bool HasTabModification { get; set; }
        public bool HideTabNameInScreen { get; set; }
        public bool CreateDefaultTextCode { get; set; }
        public string TabNameTextCodeType { get; set; }



    }
}