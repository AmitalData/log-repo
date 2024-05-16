using Logitude.BL.InfrastructureModel.EntityPMs;
 
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class DefaultAndConfigurationKeyMapping
    {
        public static void MapEntity(DefaultAndConfigurationKeyPM entityPM, DefaultAndConfigurationKeys entityPOCO)
        { 
            entityPOCO.CreateDateTime = entityPM.CreateDateTime;
            entityPOCO.SetType = entityPM.SetType;
            entityPOCO.SetKey = entityPM.SetKey;
            entityPOCO.ShortDescription = entityPM.ShortDescription;
            entityPOCO.FullDesctiption = entityPM.FullDesctiption;
         
        }
    }
}