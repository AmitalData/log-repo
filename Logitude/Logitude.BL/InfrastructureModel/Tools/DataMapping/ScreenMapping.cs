using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class ScreenMapping
    {
        public static void MapEntity(ScreenPM screenPM, Screen screen, bool isNewState, ScreenModification screenModification)
        {

            if (isNewState)
            {
                screen.Code = screenPM.Code =  screenPM.ObjectTableName + "." + screenPM.Tenant + '.' + screenPM.Id;
                screen.Type = !string.IsNullOrEmpty(screenPM.Type) ? screenPM.Type : "CLASSIC";

            }

            screen.IsReadOnly = screenPM.IsReadOnly;
            screen.Inactive = screenPM.Inactive;
            screen.ObjectTableId = screenPM.ObjectTableId;
            screen.Name = screenPM.Name;
            screen.Tenant = screenPM.Tenant;
            screen.Type = screenPM.Type;
            screen.Code = screenPM.Code;

            if (screenModification != null)
            {
                screenModification.NumberOfColumns = screenPM.NumberOfColumns;
                screenModification.NumberOfRows = screenPM.NumberOfRows;
            }

            else
            {
                screen.NumberOfColumns = screenPM.NumberOfColumns;
                screen.NumberOfRows = screenPM.NumberOfRows;
            }

        }
    }
}