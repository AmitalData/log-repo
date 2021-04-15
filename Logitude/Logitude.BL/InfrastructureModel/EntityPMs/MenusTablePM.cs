using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class MenusTablePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string MenuTypeCode { get; set; }
        public string CategoryTypeCode { get; set; }
        public int IndexOfOrder { get; set; }
        public string Icon { get; set; }
        public string TextCode { get; set; }
        public string UserControlName { get; set; }
        public string ObjectTableId { get; set; }
        public string ObjectTableName { get; set; }
        public string Code { get; set; }
        public string FeatureId { get; set; }
        public string FeatureCode { get; set; }
        public bool ShowMenuTable { get; set; }
        public string HtmlView { get; set; }
        public string FeatureUniqeCode { get; set; }
        public string QuerySection { get; set; }

    }
}