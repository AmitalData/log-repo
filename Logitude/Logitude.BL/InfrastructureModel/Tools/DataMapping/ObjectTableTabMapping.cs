using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class ObjectTableTabMapping
    {
        public static void MapEntity(ObjectTableTabPM objectTableTabPM, ObjectTableTab objectTableTab, bool isNewState)
        {
            objectTableTab.ControlPath = objectTableTabPM.ControlPath;
            objectTableTab.IndexOrder = objectTableTabPM.IndexOrder;
            objectTableTab.ObjectTableId = objectTableTabPM.ObjectTableId;
            objectTableTab.TabNameTextCodeId = objectTableTabPM.TabNameTextCodeId;
            objectTableTab.Tenant = objectTableTabPM.Tenant;
            objectTableTab.Code = objectTableTabPM.Code;
            objectTableTab.FeatureId = objectTableTabPM.FeatureId;
            objectTableTab.TabNameTextCodeCode = objectTableTabPM.TabNameTextCodeCode;
            objectTableTab.FeatureUniqeCode = objectTableTabPM.FeatureUniqeCode;

        }
    }
}