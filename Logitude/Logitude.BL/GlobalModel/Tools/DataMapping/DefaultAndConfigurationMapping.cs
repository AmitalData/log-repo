using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.BL.GlobalModel.Tools.DataMapping
{
    public class DefaultAndConfigurationMapping
    {
        public static void MapEntity(DefaultAndConfigurationPM entityPM, DefaultAndConfiguration entityPOCO, bool isNewEntity)
        {
            entityPOCO.Id = isNewEntity ? IdCounter.GetNumber("DefaultAndConfiguration", entityPM.Tenant) : entityPM.Id;
            entityPOCO.Tenant = entityPM.Tenant;
            entityPOCO.CreateDate = entityPM.CreateDate;
            entityPOCO.SearchFields = entityPM.Tenant + "," + entityPM.Value1 + "," + entityPM.Value2 + "," + entityPM.SetKey + "," + entityPM.AdditionalKey;
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