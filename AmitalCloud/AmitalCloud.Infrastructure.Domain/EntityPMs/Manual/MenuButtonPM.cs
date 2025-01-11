using System.ComponentModel.DataAnnotations;
using   AmitalCloud.Infrastructure.Domain.Enums;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class MenuButtonPM
    {
        public string HtmlComponentPath;

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string EventCode { get; set; }
        public string LabelTextCodeId { get; set; }
        public string MenuButtonGroupId { get; set; }
        public string ParentMenuButtonId { get; set; }
        public int Index { get; set; }
        public bool IsActive { get; set; }

        public bool IsHidden { get; set; }
        public bool IsDisabled { get; set; }

        public string LabelTextCodeCode { get; set; }

        public string FeatureId { get; set; }

        public bool ShowMenuButton { get; set; }

        public string MenuButtonType { get; set; }
        public string DropDownControl { get; set; }

        public string Style { get; set; }
        public int Width { get; set; }
        public string ControlPath { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
        public string DisplayText { get; set; }
        public string FeatureUniqeCode { get; set; }



    }
}