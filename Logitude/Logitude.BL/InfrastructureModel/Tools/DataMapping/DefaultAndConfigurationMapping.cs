using Logitude.BL.InfrastructureModel.EntityPMs;
 
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class DefaultAndConfigurationMapping
    {
        public static void MapEntity(DefaultAndConfigurationPM entityPM, DefaultAndConfiguration entityPOCO)
        { 
            entityPOCO.Id = entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.SearchFields = entityPM.SearchFields;
            entityPOCO.Is_Active = entityPM.Is_Active;
            entityPOCO.StoreInCache = entityPM.StoreInCache;
            entityPOCO.SetKey = entityPM.SetKey;
            entityPOCO.AdditionalKey = entityPM.AdditionalKey;
            entityPOCO.SortOrder = entityPM.SortOrder;
            entityPOCO.SetValueType1 = entityPM.SetValueType1;
            entityPOCO.Value1 = entityPM.Value1;
            entityPOCO.SetValueType2 = entityPM.SetValueType2;
            entityPOCO.Value2 = entityPM.Value2;
            entityPOCO.AllowInheritance = entityPM.AllowInheritance;
         
        }
    }
}