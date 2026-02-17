using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityClasses;

namespace AmitalCloud.Infrastructure.Data.DataMapping
{
    public class ObjectTableTabMapping
    {
        public static void MapEntity(ObjectTableTabPM objectTableTabPM, ObjectTableTab objectTableTab, bool isNewState)
        {

            if (isNewState)
            {
                objectTableTab.Id = objectTableTabPM.Id;
                objectTableTab.Tenant = objectTableTabPM.Tenant;
                objectTableTab.Code = objectTableTabPM.Code;

            }

            objectTableTab.ControlPath = objectTableTabPM.ControlPath;
            objectTableTab.ObjectTableId = objectTableTabPM.ObjectTableId;
            objectTableTab.TabNameTextCodeId = objectTableTabPM.TabNameTextCodeId;
            objectTableTab.FeatureId = objectTableTabPM.FeatureId;
            objectTableTab.TabNameTextCodeCode = objectTableTabPM.TabNameTextCodeCode;
            objectTableTab.FeatureUniqeCode = objectTableTabPM.FeatureUniqeCode;
            objectTableTab.HtmlComponentName = objectTableTabPM.HtmlComponentName;
            objectTableTab.HtmlComponentUrl = objectTableTabPM.HtmlComponentUrl;
            objectTableTab.ScreenCode = objectTableTabPM.ScreenCode;
            objectTableTab.Type = objectTableTabPM.Type;
            objectTableTab.OriginalTabCode = objectTableTabPM.OriginalTabCode;
            objectTableTab.HideTabNameInScreen = objectTableTabPM.HideTabNameInScreen;
            //if (objectTableTabPM.HasTabModification) return;

            objectTableTab.IndexOrder = objectTableTabPM.IndexOrder;

        }


    }


}