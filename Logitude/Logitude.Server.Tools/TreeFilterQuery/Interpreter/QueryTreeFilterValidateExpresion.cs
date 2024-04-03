using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Interpreter
{
   public class QueryTreeFilterValidateExpresion : IQueryTreeFilterExpression
    {
        
        private string[] operatorsHaveFieldValue = new string[] { "LessThan", "LessThanOrEqual", "GreaterThanOrEqual", "LargerThan", "Contains", "NotContains", "Equal", "NotEqual" , "StartsWith", "EndsWith", "InList", "InListExact", "Exclude", "InListInt" };
        private QueryTreeFilterContext queryTreeFilterContext;
        private List<string> fieldsNames = new List<string>();
        public void Interpret(QueryTreeFilterContext queryTreeFilterContext)
        {
            this.queryTreeFilterContext = queryTreeFilterContext;
            FillFiledsNames(queryTreeFilterContext.Type);
            RemoveEmptyFilters(queryTreeFilterContext.QueryFilterItem);
        }


        public  void RemoveEmptyFilters(QueryFilterItem queryFilterItem)
        {

            if (queryFilterItem.QueryFilterItems == null || queryFilterItem.QueryFilterItems.Count() == 0) return;
             RemoveEmptyFilter(queryFilterItem);

        }

        private void RemoveEmptyFilter(QueryFilterItem queryFilterItem)
        {

            foreach (var item in queryFilterItem.QueryFilterItems)
                RemoveEmptyFilters(item);

            queryFilterItem.QueryFilterItems.RemoveAll(d => (d.FieldValue == null || string.IsNullOrEmpty(d.FieldValue.ToString())) && operatorsHaveFieldValue.Contains(d.Operator));
            queryFilterItem.QueryFilterItems.RemoveAll(d => (string.IsNullOrEmpty(d.FilterType) == null && (string.IsNullOrEmpty(d.FieldName) || string.IsNullOrEmpty(d.Operator))));
            queryFilterItem.QueryFilterItems.RemoveAll(d => ((d.FieldValue == null || string.IsNullOrEmpty(d.FieldValue.ToString())) && (d.FieldValue2 == null || string.IsNullOrEmpty(d.FieldValue2.ToString())) && d.Operator == "Between"));
            queryFilterItem.QueryFilterItems.RemoveAll(d => (!string.IsNullOrEmpty(d.FilterType) && string.IsNullOrEmpty(d.Operator) && (d.QueryFilterItems == null || d.QueryFilterItems.Count() == 0)));
            if (!queryTreeFilterContext.IsInterpreterFinished) return;
            queryFilterItem.QueryFilterItems.RemoveAll(d => !string.IsNullOrEmpty(d.Operator) && d.Operator.Contains("Field") && d.FieldValue != null && !string.IsNullOrEmpty(d.FieldValue.ToString()) && d.FieldValue.ToString().Split('.')[0] == queryTreeFilterContext.ParentObjectTableName);
            queryFilterItem.QueryFilterItems.RemoveAll(d => !string.IsNullOrEmpty(d.FieldName) && d.FieldName.Split('.')[0] == queryTreeFilterContext.ParentObjectTableName);
            queryFilterItem.QueryFilterItems.RemoveAll(d => !string.IsNullOrEmpty(d.FieldName) && !fieldsNames.Contains(d.FieldName));
            queryFilterItem.QueryFilterItems.RemoveAll(d => !string.IsNullOrEmpty(d.FieldValue?.ToString()) && d.Operator.Contains("Field") && !fieldsNames.Contains(d.FieldValue?.ToString()));

        }

        private void FillFiledsNames(Type type)
        {
            fieldsNames.Add("PartnerEntityField");
            if (type == null)
            {
                return;
            }
            List<string> fieldsPropertiesNames = type.GetRuntimeProperties()?.ToList()?.Select(field => field.Name)?.ToList();
            if (fieldsPropertiesNames != null)
            {
                fieldsNames = fieldsNames.Concat(fieldsPropertiesNames).ToList();
            }

        }
    }
}
