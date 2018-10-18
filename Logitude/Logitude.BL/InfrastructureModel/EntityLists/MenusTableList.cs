using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class MenusTableList
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
        public string FeatureId { get; set; }
    }
}