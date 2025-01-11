using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class MenuButtonGroupMapping
    {
        public static void MapEntity(MenuButtonGroupPM menuButtonGroupPM, MenuButtonGroup menuButtonGroup, bool isNewState)
        {
            menuButtonGroup.MenuButtonGroupType = menuButtonGroupPM.MenuButtonGroupType;
            menuButtonGroup.Name = menuButtonGroupPM.Name;
            menuButtonGroup.ObjectTableId = menuButtonGroupPM.ObjectTableId;
            menuButtonGroup.Tenant = menuButtonGroupPM.Tenant;
        }
    }
}