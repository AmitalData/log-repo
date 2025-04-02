using AmitalCloud.Infrastructure.Domain.DataContracts;
using System.Web.Script.Serialization;

namespace AmitalCloud.Infrastructure.Web.Helpers.TreeFilterQuery
{
    public class JavaScriptSerializerExpression: IQueryTreeFilterExpression
    {
        public void Interpret(QueryTreeFilterContext queryTreeFilterContext)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            queryTreeFilterContext.QueryFilterItem = javaScriptSerializer.Deserialize<QueryFilterItem>(queryTreeFilterContext.AdditionalTreeFilter);
        }
    }
}
