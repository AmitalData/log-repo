using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class DefaultAndConfigurationKeyMapping
    {
        public static void MapEntity(DefaultAndConfigurationKeyPM entityPM, DefaultAndConfigurationKey entityPOCO)
        {
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.SetType1 = entityPM.SetType1;
            entityPOCO.SetKey = entityPM.SetKey;
            entityPOCO.ShortDescription = entityPM.ShortDescription;
            entityPOCO.FullDesctiption = entityPM.FullDesctiption;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.SetType2 = entityPM.SetType2;
        }
    }
}