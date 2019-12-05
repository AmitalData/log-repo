using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.Tools.DataMapping
{
    public class QueryColumnMapping
    {
        public static void MapEntity(QueryColumnPM queryColumnPM, QueryColumn queryColumn, bool isNewState)
        {
            queryColumn.ColumnWidth = queryColumnPM.ColumnWidth;
            queryColumn.IndexOrder = queryColumnPM.IndexOrder;
            queryColumn.ObjectFieldId = queryColumnPM.ObjectFieldId;
            queryColumn.QueryId = queryColumnPM.QueryId;
            queryColumn.Tenant = queryColumnPM.Tenant;
            queryColumn.UserId = queryColumnPM.UserId;
            queryColumn.ObjectFieldCode = queryColumnPM.ObjectFieldCode;
        }
    }
}