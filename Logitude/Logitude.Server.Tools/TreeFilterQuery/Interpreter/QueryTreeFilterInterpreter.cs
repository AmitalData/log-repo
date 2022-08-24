using Logitude.Server.Tools.TreeFilterQuery.Expression;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Interpreter
{
  public  class QueryTreeFilterInterpreter
    {

        public QueryFilterItem Run(QueryTreeFilterContext queryTreeFilterContext)
        {
            List<IQueryTreeFilterExpression> expressions = BuildExpressions(queryTreeFilterContext);
            foreach (IQueryTreeFilterExpression expression in expressions)
            {
                expression.Interpret(queryTreeFilterContext);
            }

            return queryTreeFilterContext.QueryFilterItem;
        }

        private List<IQueryTreeFilterExpression> BuildExpressions(QueryTreeFilterContext queryTreeFilterContext)
        {
            List<IQueryTreeFilterExpression> expressions =  new List<IQueryTreeFilterExpression>();
            expressions.Add(new JavaScriptSerializerExpression());
            expressions.Add(new QueryTreeFilterValidateExpresion());
            expressions.Add(new QueryTreeFilterResetValueExpresion());
            expressions.Add(new CustomFieldExpression());
            expressions.Add(new PartnerEntityResloveFieldValueExpression());
            expressions.Add(new PartnerEntityQueryFilterExpression());
            expressions.Add(new QueryTreeFilterValidateExpresion());
            return expressions;
        }
    }
}
