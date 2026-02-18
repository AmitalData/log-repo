using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class MenuButtonMapping
    {
        public static void MapEntity(MenuButtonPM menuButtonPM, MenuButton menuButton, bool isNewState)
        {
            menuButton.EventCode = menuButtonPM.EventCode;
            menuButton.Index = menuButtonPM.Index;
            menuButton.IsActive = menuButtonPM.IsActive;
            menuButton.LabelTextCodeId = menuButtonPM.LabelTextCodeId;
            menuButton.MenuButtonGroupId = menuButtonPM.MenuButtonGroupId;
            menuButton.ParentMenuButtonId = menuButtonPM.ParentMenuButtonId;
            menuButton.Tenant = menuButtonPM.Tenant;
            menuButton.MenuButtonType = menuButtonPM.MenuButtonType;
            menuButton.DropDownControl = menuButtonPM.DropDownControl;
            menuButton.Style = menuButtonPM.Style;
            menuButton.FeatureUniqeCode = menuButtonPM.FeatureUniqeCode;

            menuButton.LabelTextCodeCode = menuButtonPM.LabelTextCodeCode;
        }
    }
}