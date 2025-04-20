using Logitude.BL.Helpers;
using NetCommonHelper.Logger;
using Simplog.Server.Infrastructure.DataContracts;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Logitude.Customs.BL.AzureSearch
{
    public static class FilterHelper
    {
        private static readonly DevLog logger = DevLog.Instance;
        private static CustomFieldClass customFilterClass = new CustomFieldClass();
        private static DefaultAndConfiguration_Ext ConnectionDetails => DefaultService.Instance.Get(0, "AzureSearchAI", "Customs");
        public static string serviceName => ConnectionDetails.Value1;
        public static string apiKey => ConnectionDetails.Value2;

        public static string ConvertQueryFilter(List<QueryFilterItem> additionalFilters)
        {
            List<string> filterParts = new List<string>();

            if (additionalFilters == null || additionalFilters.Count == 0)
                return string.Empty;

            additionalFilters.ForEach(filter =>
            {
                if (string.IsNullOrEmpty(filter.FieldName))
                {
                    LogFilterError(filter);
                    return;
                }

                string _operator = ConvertLogitudeOperatorToAzureSearchOperator(filter.Operator);
                string value1 = GetValue(filter.FieldDataType, filter.FieldValue);
                string value2 = GetValue(filter.FieldDataType, filter.FieldValue2);
                string value3 = GetValue(filter.FieldDataType, filter.FieldValue3);
                filter.FieldName = char.ToLower(filter.FieldName[0]) + filter.FieldName.Substring(1);

                if (_operator == QueryFilterOperatorTypes.IsNull || _operator == QueryFilterOperatorTypes.IsNotNull)
                {
                    filterParts.Add($"{filter.FieldName} {_operator}");
                    return;
                }

                if (string.IsNullOrEmpty(value1))
                {
                    LogFilterError(filter);
                    return;
                }

                switch (_operator)
                {
                    case QueryFilterOperatorTypes.StartsWith:
                        filterParts.Add($"search.ismatch('{value1}*', '{filter.FieldName}')");
                        break;
                    case QueryFilterOperatorTypes.InListExact:
                    case QueryFilterOperatorTypes.InListInt:
                        filterParts.Add($"search.in({filter.FieldName}, '{value1}', ',')");
                        break;
                    case QueryFilterOperatorTypes.Exclude:
                        filterParts.Add($"not search.in({filter.FieldName}, '{value1}', ',')");
                        break;
                    case QueryFilterOperatorTypes.InList:
                        {
                            value1 = "(" +
                                string.Join(" or ",
                                    filter.FieldValue.ToString().Split(',').Select(v => $"search.ismatch('*{v}*', '{filter.FieldName}')")) +
                                ")";
                            filterParts.Add(value1);
                            break;
                        }
                    case QueryFilterOperatorTypes.Between:
                        {
                            if (string.IsNullOrEmpty(value2))
                            {
                                LogFilterError(filter);
                                break;
                            }
                            filterParts.Add($"({filter.FieldName} ge {value1} and {filter.FieldName} le {value2})");
                            break;
                        }

                    default:
                        if ((_operator == "eq" || _operator == "ne") &&
                        (filter.FieldDataType == "Text" || filter.FieldDataType == "nText" || filter.FieldDataType == "LookUp" || filter.FieldDataType == "PickList" || filter.FieldDataType?.ToLower() == "string"))
                            value1 = "'" + value1 + "'";

                        filterParts.Add($"{filter.FieldName} {_operator} {value1}");
                        break;
                }
            });

            string allFilters = "(" + string.Join(") and (", filterParts) + ")";
            return allFilters;
        }

        public static string ConvertQueryable<T>(IQueryable<T> queryable)
        {
            if (queryable == null)
                return string.Empty;

            List<QueryFilterItem> queryFilterItems = PrintQueryClauses(queryable);
            string filters = ConvertQueryFilter(queryFilterItems);
            return filters;
        }

        private static string GetValue(string fieldDataType, object val)
        {
            if (val is bool)
                return (bool)val == true ? "true" : "false";

            return customFilterClass.SetFieldDataType(fieldDataType, val);
        }

        private static void LogFilterError(QueryFilterItem filter)
        {
            logger.WriteInfo($"Invalid value for filter {filter?.FieldName} in DeclarationAzureSearchService.Search, value1: {filter?.FieldValue}, value2: {filter?.FieldValue2}, value3: {filter?.FieldValue3}, type: {filter?.FieldDataType}, type: {filter?.FieldDataType}");
        }

        public static string ConvertLogitudeOperatorToAzureSearchOperator(string logitudeOperator)
        {
            switch (logitudeOperator)
            {
                case QueryFilterOperatorTypes.LargerThan:
                    return "gt";
                case QueryFilterOperatorTypes.GreaterThanOrEqual:
                    return "ge";
                case QueryFilterOperatorTypes.LessThan:
                    return "lt";
                case QueryFilterOperatorTypes.LessThanOrEqual:
                    return "le";
                case QueryFilterOperatorTypes.StartsWith:
                case QueryFilterOperatorTypes.Contains:
                case QueryFilterOperatorTypes.InList:
                case QueryFilterOperatorTypes.InListExact:
                case QueryFilterOperatorTypes.InListInt:
                case QueryFilterOperatorTypes.Between:
                    return logitudeOperator;
                case QueryFilterOperatorTypes.NotEqual:
                    return "ne";
                case QueryFilterOperatorTypes.Exclude:
                    return "not in";
                case QueryFilterOperatorTypes.IsNotNull:
                    return "ne null";
                case QueryFilterOperatorTypes.IsNull:
                    return "eq null";
                default:
                    return "eq";
            }
        }

        public static List<QueryFilterItem> PrintQueryClauses<T>(IQueryable<T> query)
        {
            ClauseVisitor visitor = new ClauseVisitor();
            visitor.Visit(query.Expression);
            return visitor.Clauses;
        }

        private class ClauseVisitor : ExpressionVisitor
        {
            public List<QueryFilterItem> Clauses { get; } = new List<QueryFilterItem>();

            protected override Expression VisitBinary(BinaryExpression node)
            {
                if (node.Left is MemberExpression member && node.Right is ConstantExpression constant)
                {
                    string type = constant.Value is string ? "Text" : null;
                    string _operator = node.NodeType.ToString() == "GreaterThan" ? QueryFilterOperatorTypes.LargerThan : node.NodeType.ToString();

                    Clauses.Add(new QueryFilterItem() { FieldName = member.Member.Name, Operator = _operator, FieldDataType = type, FieldValue = constant.Value });
                }
                return base.VisitBinary(node);
            }
        }

        public static class QueryFilterOperatorTypes
        {
            public const string LargerThan = "LargerThan";
            public const string GreaterThanOrEqual = "GreaterThanOrEqual";
            public const string LessThan = "LessThan";
            public const string LessThanOrEqual = "LessThanOrEqual";
            public const string StartsWith = "StartsWith";
            public const string Contains = "Contains";
            public const string InList = "InList";
            public const string InListExact = "InListExact";
            public const string InListInt = "InListInt";
            public const string Between = "Between";
            public const string NotEqual = "NotEqual";
            public const string Exclude = "Exclude";
            public const string IsNotNull = "IsNotNull";
            public const string IsNull = "IsNull";
        }
    }
}
