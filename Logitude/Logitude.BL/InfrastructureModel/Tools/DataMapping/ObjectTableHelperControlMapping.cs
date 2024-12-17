using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class ObjectTableHelperControlMapping
    {
        public static void MapEntity(ObjectTableHelperControlPM objectTableHelperControlPM, ObjectTableHelperControl objectTableHelperControl, bool isNewState)
        {
            objectTableHelperControl.ControlPath = objectTableHelperControlPM.ControlPath;
            objectTableHelperControl.ObjectTableId = objectTableHelperControlPM.ObjectTableId;
            objectTableHelperControl.Tenant = objectTableHelperControlPM.Tenant;
            objectTableHelperControl.Code = objectTableHelperControlPM.Code;
            objectTableHelperControl.FeatureId = objectTableHelperControlPM.FeatureId;
            objectTableHelperControl.FeatureUniqeCode = objectTableHelperControlPM.FeatureUniqeCode;

        }
    }
}