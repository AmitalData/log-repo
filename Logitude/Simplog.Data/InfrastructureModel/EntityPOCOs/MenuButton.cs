using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class MenuButton
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
        public int Width { get; set; }
        public string ControlPath { get; set; }
        public string HtmlComponentPath { get; set; }
        public string FeatureUniqeCode { get; set; }


        //[ForeignKey("FeatureId")]
        public Feature Feature { get; set; }
        [ForeignKey("MenuButtonGroupId")]
        public virtual MenuButtonGroup MenuButtonGroup { get; set; }
        [ForeignKey("LabelTextCodeId")]
        public virtual TextCode TextCode { get; set; }
        public MenuButton ParentMenuButton { get; set; }
        public List<MenuButton> ChildrenMenuButtons { get; set; }
    }
}