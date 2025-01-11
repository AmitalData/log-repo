using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityLists
{
    public class MenuButtonList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string EventCode { get; set; }
        public string LabelTextCodeId { get; set; }
        public string MenuButtonGroupId { get; set; }
        public string ParentMenuButtonId { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }
        public string FeatureId { get; set; }
        public string MenuButtonType { get; set; }
        public string DropDownControl { get; set; }
        public string Style { get; set; }
        public string ControlPath { get; set; }
        public string FeatureUniqeCode { get; set; }

        public string LabelTextCodeCode { get; set; }
    }
}