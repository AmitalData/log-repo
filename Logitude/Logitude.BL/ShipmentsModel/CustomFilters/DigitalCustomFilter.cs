using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Logitude.BL.ShipmentsModel.CustomFilters
{
    public static class DigitalCustomFilter
    {
        public static IQueryable<T> ApplyDigitalQuickSearchFilter<T>(QueryFilterItem item, IQueryable<T> data)
        {
            string[] fields = item.FieldValue?.ToString().Split(',');
            string dynamicLinqExpression = BuildDynamicQuickSearchExpression(fields, item.FieldValue2);
            return data.Where(dynamicLinqExpression);
        }

        private static string BuildDynamicQuickSearchExpression(string[] fields, object searchValue)
        {
            string dynamicLinqExpression = "t => false ||";
            if (fields != null && fields.Length > 0)
            {
                dynamicLinqExpression = AppendDynamicQuickSearchExpression(dynamicLinqExpression, fields, searchValue);
            }
            return dynamicLinqExpression.Remove(dynamicLinqExpression.LastIndexOf("||")).Trim();
        }

        private static string AppendDynamicQuickSearchExpression(string dynamicLinqExpression, string[] fields, object searchValue)
        {
            string value = BuildDynamicFilterValue(searchValue);
            foreach (var field in fields)
            {
                dynamicLinqExpression += BuildDynamicQuickSearchFieldExpression(field, value);
            }
            return dynamicLinqExpression;
        }

        private static string BuildDynamicFilterValue(object value)
        {
            return value == null ? "\"\"" : "\"" + value.ToString().ToLower() + "\"";
        }

        private static string BuildDynamicQuickSearchFieldExpression(string field, string value)
        {
            string fieldExpression = " (t." + field + " == null ? \"\" : t." + field + ".ToString().ToLower()).Contains(" + value + ")";
            return " " + fieldExpression + " " + "||";
        }
    }
}