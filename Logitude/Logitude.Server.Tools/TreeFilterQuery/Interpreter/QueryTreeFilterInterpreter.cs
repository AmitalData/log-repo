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

        public QueryFilterItem Run(QueryTreeFilterContext context)
        {
         
            if (string.IsNullOrEmpty(context.AdditionalTreeFilter)) return  null;
           
            List<IQueryTreeFilterExpression> expressions = new List<IQueryTreeFilterExpression>();
            expressions.Add(new JavaScriptSerializerExpression());
            expressions.Add(new QueryTreeFilterIgnoreExpresion());
            expressions.Add(new QueryTreeFilterResetValueExpresion());
            expressions.Add(new CustomFieldExpression());

            if (!string.IsNullOrEmpty(context.ParentObjectTableName) && !string.IsNullOrEmpty(context.ParentEntityId))
            {
                expressions.Add(new PartnerEntityResloveFieldValueExpression());
                expressions.Add(new PartnerEntityQueryFilterExpression());
            }

            foreach (IQueryTreeFilterExpression expression in expressions)
            {
                expression.Interpret(context);
            }

            return  context.QueryFilterItem;
        }
    }
}
