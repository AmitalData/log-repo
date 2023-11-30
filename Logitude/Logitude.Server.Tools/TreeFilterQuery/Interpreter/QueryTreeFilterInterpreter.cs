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

        TreeFilterQueryArgs treeFilterQueryArgs;
        public QueryTreeFilterInterpreter(TreeFilterQueryArgs treeFilterQueryArgs)
        {
            this.treeFilterQueryArgs = treeFilterQueryArgs;
        }


        public QueryFilterItem Run()
        {
            QueryTreeFilterContext queryTreeFilterContext = this.BuildContext(treeFilterQueryArgs); 
            List<IQueryTreeFilterExpression> expressions = BuildExpressions();
            foreach (IQueryTreeFilterExpression expression in expressions)
            {
                expression.Interpret(queryTreeFilterContext);
            }

            return queryTreeFilterContext.QueryFilterItem;
        }

        private List<IQueryTreeFilterExpression> BuildExpressions()
        {
            List<IQueryTreeFilterExpression> expressions =  new List<IQueryTreeFilterExpression>();
            expressions.Add(new JavaScriptSerializerExpression());
            expressions.Add(new QueryTreeFilterValidateExpresion());
            expressions.Add(new QueryTreeFilterResetValueExpresion());
            expressions.Add(new CustomFieldExpression());
            expressions.Add(new DateGroupFilterExpression());
            expressions.Add(new PartnerEntityResloveFieldValueExpression());
            expressions.Add(new PartnerEntityQueryFilterExpression());
            expressions.Add(new QueryTreeFilterValidateExpresion());
            return expressions;
        }

        private QueryTreeFilterContext BuildContext(TreeFilterQueryArgs treeFilterQueryArgs)
        {
            return new QueryTreeFilterContext()
            {

                AdditionalTreeFilter = treeFilterQueryArgs.AdditionalTreeFilter,
                ObjectTableName = treeFilterQueryArgs.ObjectTableName,
                ParentEntityId = treeFilterQueryArgs.ParentEntityId,
                ParentObjectTableName = treeFilterQueryArgs.ParentObjectTableName,
                Tenant = treeFilterQueryArgs.Tenant,
                Type = treeFilterQueryArgs.Type,
                ParentEntity = treeFilterQueryArgs.ParentEntity,

            };

        }



    }
}
