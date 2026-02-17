using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class TipsVisibilityMapping
    {
        public static void MapEntity(TipsVisibilityPM tipPM, TipsVisibility tip, bool isNewState)
        {
            tip.IsVisible = tipPM.IsVisible;
            tip.UserId = tipPM.UserId;
            tip.Tenant = tipPM.Tenant;
            tip.TipCode = tipPM.TipCode;
        }
    }
}