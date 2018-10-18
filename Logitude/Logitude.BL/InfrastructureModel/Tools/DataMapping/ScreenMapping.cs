using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class ScreenMapping
    {
        public static void MapEntity(ScreenPM screenPM, Screen screen, bool isNewState, ScreenModification screenModification)
        {
            screen.Code = screenPM.Code;
            screen.IsReadOnly = screenPM.IsReadOnly;
            screen.ObjectTableId = screenPM.ObjectTableId;
            screen.Name = screenPM.Name;
            screen.Tenant = screenPM.Tenant;

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