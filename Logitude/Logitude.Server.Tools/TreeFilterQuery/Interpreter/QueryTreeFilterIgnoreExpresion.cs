using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Interpreter
{
   public class QueryTreeFilterIgnoreExpresion : IQueryTreeFilterExpression
    {
        
       private string[] operatorsHaveFieldValue = new string[] { "LessThan", "LessThanOrEqual", "GreaterThanOrEqual", "LargerThan", "Contains", "NotContains", "Equal", "NotEqual" , "StartsWith", "InList", "InListExact", "Exclude", "InListInt" };
        public void Interpret(QueryTreeFilterContext queryTreeFilterContext)
        {

            RemoveEmptyFilters(queryTreeFilterContext.QueryFilterItem);

        }


        public  void RemoveEmptyFilters(QueryFilterItem queryFilterItem)
        {
            if (queryFilterItem.QueryFilterItems != null)
            {
                RemoveEmptyFilter(queryFilterItem);
            }
        }

        private void RemoveEmptyFilter(QueryFilterItem queryFilterItem)
        {
            queryFilterItem.QueryFilterItems.RemoveAll(d => (d.FieldValue == null || string.IsNullOrEmpty(d.FieldValue.ToString())) && operatorsHaveFieldValue.Contains(d.Operator));
            queryFilterItem.QueryFilterItems.RemoveAll(d => (string.IsNullOrEmpty(d.FilterType) == null && (string.IsNullOrEmpty(d.FieldName) || string.IsNullOrEmpty(d.Operator))));
            queryFilterItem.QueryFilterItems.RemoveAll(d => ((d.FieldValue == null || string.IsNullOrEmpty(d.FieldValue.ToString())) && (d.FieldValue2 == null || string.IsNullOrEmpty(d.FieldValue2.ToString())) && d.Operator == "Between"));

            foreach (var item in queryFilterItem.QueryFilterItems)
                RemoveEmptyFilters(item);
        }
    }
}
