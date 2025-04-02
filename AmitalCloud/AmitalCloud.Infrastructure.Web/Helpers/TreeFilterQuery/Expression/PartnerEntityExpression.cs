using AmitalCloud.Infrastructure.Domain.DataContracts;

namespace AmitalCloud.Infrastructure.Web.Helpers.TreeFilterQuery
{
    public class PartnerEntityExpression
    {
        public System.Linq.Expressions.Expression CreateExpression(System.Linq.Expressions.Expression expression, QueryFilterItem queryFilterItem)
        {
            return ((bool)queryFilterItem.FieldValue) ? System.Linq.Expressions.Expression.Equal(expression, expression) : System.Linq.Expressions.Expression.NotEqual(expression, expression);
        }
    }
}
