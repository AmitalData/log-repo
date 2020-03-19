using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class ScreenFieldMapping
    {
        public static void MapEntity(ScreenFieldPM screenFieldPM, ScreenField screenField, bool isNewState)
        {
            screenField.Column = screenFieldPM.Column;
            screenField.ObjectFieldId = screenFieldPM.ObjectFieldId;
            screenField.Row = screenFieldPM.Row;
            screenField.ScreenId = screenFieldPM.ScreenId;
            screenField.ScreenCode = screenFieldPM.ScreenCode;

            screenField.Tenant = screenFieldPM.Tenant;
            screenField.ObjectFieldCode = screenFieldPM.ObjectFieldCode;


        }
    }
}