using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.TreeFilterQuery.Interpreter;
using Logitude.Server.Tools.TreeFilterQuery.Iterator;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Logitude.Server.Tools.TreeFilterQuery.Expression
{

    public class DateGroupFilterExpression : IQueryTreeFilterExpression
    {

        public void Interpret(QueryTreeFilterContext queryTreeFilterContext)
        {
            QueryTreeFilterIterator iterator = CreateIterator(queryTreeFilterContext);
            if (!iterator.Any()) return;
            while (iterator.HasNext())
            {
                Handel(iterator.Next());
            }
        }
        private void Handel(QueryFilterItem filterItem)
        {
            if (string.IsNullOrEmpty(filterItem.DateGroupCode))
                return;
            HandelDateGroupFilter(filterItem);
        }


        private static void HandelDateGroupFilter(QueryFilterItem queryFilterItem)
        {
            if (queryFilterItem.Operator != "NotEqual" && queryFilterItem.Operator != "Equal")
                return;
            QueryFilterItem queryFilterItemLessThan = CloneQueryFilterItem(queryFilterItem); ;
            QueryFilterItem queryFilterItemGreaterThan = CloneQueryFilterItem(queryFilterItem); ;
            if (queryFilterItem.FieldDataType == "NotEqual")
            {
                HandelNotEqualOperator(queryFilterItem, queryFilterItemLessThan, queryFilterItemGreaterThan);
            }
            else
            {
                HandelEqualOperator(queryFilterItem, queryFilterItemLessThan, queryFilterItemGreaterThan);
            }
            ChangeToGroup(queryFilterItem);
            queryFilterItem.QueryFilterItems.Add(queryFilterItemLessThan);
            queryFilterItem.QueryFilterItems.Add(queryFilterItemGreaterThan);
        }

        private static void ChangeToGroup(QueryFilterItem queryFilterItem)
        {
            queryFilterItem.FieldName = null;
            queryFilterItem.FieldValue = null;
            queryFilterItem.FieldValue2 = null;
            queryFilterItem.FieldValue3 = null;
            queryFilterItem.Operator = null;
            queryFilterItem.IsCustom = false;
            queryFilterItem.DisplayInList = false;
            queryFilterItem.IsCustomField = false;
            queryFilterItem.FieldDataType = null;
            queryFilterItem.FilterType = "And";
            queryFilterItem.DateGroupCode = null;
            queryFilterItem.IsListFilter = false;
            queryFilterItem.IsAnalyticsMetadatas = false;
            queryFilterItem.QueryFilterItems = new List<QueryFilterItem>(); ;
        }

        private static void HandelEqualOperator(QueryFilterItem queryFilterItem, QueryFilterItem queryFilterItemLessThan, QueryFilterItem queryFilterItemGreaterThan)
        {
            queryFilterItemGreaterThan.Operator = "GreaterThanOrEqual";
            queryFilterItemLessThan.Operator = "LessThan";
            var date = DateTime.Parse(queryFilterItemLessThan.FieldValue.ToString());
            date = ConvertDateByDateGroupCode(date, queryFilterItem.DateGroupCode);
            queryFilterItemLessThan.FieldValue = date;
        }

        private static void HandelNotEqualOperator(QueryFilterItem queryFilterItem, QueryFilterItem queryFilterItemLessThan, QueryFilterItem queryFilterItemGreaterThan)
        {
            queryFilterItemLessThan.Operator = "LessThan";
            queryFilterItemGreaterThan.Operator = "GreaterThanOrEqual";
            var date = DateTime.Parse(queryFilterItemGreaterThan.FieldValue.ToString());
            date = ConvertDateByDateGroupCode(date, queryFilterItem.DateGroupCode);
            queryFilterItemGreaterThan.FieldValue = date;
        }

        private static DateTime ConvertDateByDateGroupCode(DateTime date, string dateGroupCode)
        {
            switch (dateGroupCode)
            {
                case "Day":
                    return date.AddDays(1);
                case "Month":
                    return date.AddMonths(1);
                case "Year":
                    return date.AddYears(1);
                case "Quarter":
                    return date.AddMonths(3);
                default:
                    return date;
            }

        }

        private static QueryFilterItem CloneQueryFilterItem(QueryFilterItem queryFilterItem)
        {
            return new QueryFilterItem
            {
                DisplayInList = queryFilterItem.DisplayInList,
                FieldDataType = queryFilterItem.FieldDataType,
                FieldName = queryFilterItem.FieldName,
                FieldValue2 = queryFilterItem.FieldValue2,
                FilterType = queryFilterItem.FilterType,
                FieldValue = queryFilterItem.FieldValue,
                FieldValue3 = queryFilterItem.FieldValue3,
                IsAnalyticsMetadatas = queryFilterItem.IsAnalyticsMetadatas,
                IsCustom = queryFilterItem.IsCustom,
                IsCustomField = queryFilterItem.IsCustomField,
                IsListFilter = queryFilterItem.IsListFilter,
                Operator = queryFilterItem.Operator
            };
        }

        private QueryTreeFilterIterator CreateIterator(QueryTreeFilterContext queryTreeFilterContext)
        {
            var iterator = new QueryTreeFilterCollection(queryTreeFilterContext).CreateIterator();
            var collection = iterator.collection.Where(d => !string.IsNullOrEmpty(d.DateGroupCode)).ToList();
            iterator.SetCollection(collection);
            return iterator;
        }



    }


}
