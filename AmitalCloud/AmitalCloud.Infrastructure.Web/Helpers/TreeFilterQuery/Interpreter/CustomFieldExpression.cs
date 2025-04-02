using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using System;
using System.Linq;

namespace AmitalCloud.Infrastructure.Web.Helpers.TreeFilterQuery
{
    public class CustomFieldExpression : IQueryTreeFilterExpression
    {
        CustomFieldClass customFilterClass = new CustomFieldClass();
        public void Interpret(QueryTreeFilterContext queryTreeFilterContext)
        {
            QueryTreeFilterIterator iterator = CreateIterator(queryTreeFilterContext);
            if (!iterator.Any()) return;
            while (iterator.HasNext())
            {
                Handel(iterator.Next(), queryTreeFilterContext);
            }
        }

        private void Handel(QueryFilterItem filterItem, QueryTreeFilterContext queryTreeFilterContext)
        {
            if(!filterItem.IsCustomField || filterItem.Operator.Contains("Field")) return;
            filterItem.FieldValue = GetCustomFieldStringValue(GetFieldValue(filterItem.FieldDataType, filterItem.FieldValue), filterItem.FieldDataType);
            filterItem.FieldValue2 = GetCustomFieldStringValue(GetFieldValue(filterItem.FieldDataType, filterItem.FieldValue2), filterItem.FieldDataType);
        }

        private object GetCustomFieldStringValue(object fieldValue ,string fieldDataType)
        {
            if (fieldValue == null) return null;
             string result = customFilterClass.SetFieldDataType(fieldDataType, fieldValue);
            if (result != null && result.ToString().ToLower() == "false")
                result = null;
            return result;
        }

        public string GetFieldValue(string dataTypeCode, object fieldValue)
        {
            if (fieldValue == null || (dataTypeCode != "DateTime" && dataTypeCode != "Date")) return fieldValue != null ? fieldValue.ToString() : null;
            try
            {
                return FieldValueResolver.ConvertToDate(fieldValue.ToString()).ToString();
            }
            catch
            {
                return fieldValue != null ? fieldValue.ToString() : null;
            }
        }

        private QueryTreeFilterIterator CreateIterator(QueryTreeFilterContext queryTreeFilterContext)
        {
            var iterator = new QueryTreeFilterCollection(queryTreeFilterContext).CreateIterator();
            var collection = iterator.collection.Where(d => d.IsCustomField && !d.Operator.Contains("Field")).ToList();
            iterator.SetCollection(collection);
            return iterator;
        }
    }
}
