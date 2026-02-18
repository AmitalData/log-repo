using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class MenuButtonGroupPM
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Tenant { get; set; }
        public string MenuButtonGroupType { get; set; }
        public string ObjectTableId { get; set; }
        public string ObjectTableName { get; set; }

        List<MenuButtonPM> menuButtons;
        [Include]
        [Composition]
        [Association("MenuButtonMenuButtonGroup", "Id", "MenuButtonGroupId")]
        public virtual List<MenuButtonPM> MenuButtons
        {
            get
            {
                if (menuButtons == null)
                {
                    menuButtons = new List<MenuButtonPM>();
                }
                return menuButtons;
            }
            set { menuButtons = value; }
        }
    }
}