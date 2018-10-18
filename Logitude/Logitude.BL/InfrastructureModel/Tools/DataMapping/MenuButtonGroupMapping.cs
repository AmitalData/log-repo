using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
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