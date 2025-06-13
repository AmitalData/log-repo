using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityClasses;

namespace AmitalCloud.Infrastructure.Data.DataMapping
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
            queryColumn.QueryCode = queryColumnPM.QueryCode;
            queryColumn.ObjectFieldCode = queryColumnPM.ObjectFieldCode;
        }
    }
}