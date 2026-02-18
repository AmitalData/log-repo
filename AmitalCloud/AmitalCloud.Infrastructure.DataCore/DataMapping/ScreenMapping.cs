using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityClasses;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class ScreenMapping
    {
        public static void MapEntity(ScreenPM screenPM, Screen screen, bool isNewState, ScreenModification screenModification)
        {

            if (isNewState)
            {
                screen.Code = screenPM.Code; //todo : = string.IsNullOrEmpty(screenPM.Code) ? screenPM.ObjectTableName + "." + screenPM.Tenant + '.' + screenPM.Id : screenPM.Code;
                screen.Type = !string.IsNullOrEmpty(screenPM.Type) ? screenPM.Type : "CLASSIC";

            }

            screen.IsReadOnly = screenPM.IsReadOnly;
            screen.Inactive = screenPM.Inactive;
            screen.ObjectTableId = screenPM.ObjectTableId;
            screen.Name = screenPM.Name;
            screen.Tenant = screenPM.Tenant;
            screen.Type = screenPM.Type;
            screen.Code = screenPM.Code;
            screen.SortedByFieldCode = screenPM.SortedByFieldCode;
            screen.SortedType = screenPM.SortedType;
            screen.RelatedScreenCode = screenPM.RelatedScreenCode;
            screen.IsHeaderScreen = screenPM.IsHeaderScreen;
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

            BuildSearchFields(screenPM, screen);
        }
        private static void BuildSearchFields(ScreenPM entityPM, Screen entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}