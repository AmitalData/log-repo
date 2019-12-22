using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class TipMapping
    {
        public static void MapEntity(TipPM tipPM, Tip tip, bool isNewState)
        {
            tip.Code = tipPM.Code;
            tip.ShortTextCode = tipPM.ShortTextCodeId;
            tip.Tenant = tipPM.Tenant;
            tip.VisibilityDefaultValue = tipPM.VisibilityDefaultValue;
            tip.ObjectTableId = tipPM.ObjectTableId;
            tip.ShortTextCodeCode = tipPM.ShortTextCodeCode;
        }
    }
}