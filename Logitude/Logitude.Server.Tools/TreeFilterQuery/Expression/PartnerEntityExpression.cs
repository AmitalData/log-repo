using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Expression
{
    public class PartnerEntityExpression
    {
        public  System.Linq.Expressions.Expression CreateExpression(System.Linq.Expressions.Expression expression , QueryFilterItem queryFilterItem )
        {
            return ((bool)queryFilterItem.FieldValue) ? System.Linq.Expressions.Expression.Equal(expression, expression) : System.Linq.Expressions.Expression.NotEqual(expression, expression);
        }
    }
}
