using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class AdvancedQueryFilterMapping
    {
        public static void MapEntity(AdvancedQueryFilterPM advancedQueryFilterPM, AdvancedQueryFilter advancedQueryFilter, bool isNewState)
        {
            advancedQueryFilter.IndexOrder = advancedQueryFilterPM.IndexOrder;
            advancedQueryFilter.IsPredefined = advancedQueryFilterPM.IsPredefined;
            advancedQueryFilter.ObjectFieldId = advancedQueryFilterPM.ObjectFieldId;
            advancedQueryFilter.Operator = advancedQueryFilterPM.Operator;
            advancedQueryFilter.PredefinedValue = advancedQueryFilterPM.PredefinedValue;
            advancedQueryFilter.PredefinedValue2 = advancedQueryFilterPM.PredefinedValue2;
            advancedQueryFilter.QueryId = advancedQueryFilterPM.QueryId;
            advancedQueryFilter.Tenant = advancedQueryFilterPM.Tenant;
            advancedQueryFilter.UserId = advancedQueryFilterPM.UserId; 
        }
    }
}