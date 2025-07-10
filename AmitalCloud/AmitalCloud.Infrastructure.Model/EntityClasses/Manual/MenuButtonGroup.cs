using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class MenuButtonGroup
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Tenant { get; set; }
        public string MenuButtonGroupType { get; set; }
        public string ObjectTableId { get; set; }
        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }


        List<MenuButton> menuButtons;
        //[Include]
        [Composition]
        //[Association("MenuButtonMenuButtonGroup","Id","MenuButtonGroupId")]
        public virtual List<MenuButton> MenuButtons
        {
            get
            {
                if (menuButtons == null)
                {
                    menuButtons = new List<MenuButton>();
                }
                return menuButtons;
            }
            set { menuButtons = value; }
        }
    }
}