using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
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
    }
}