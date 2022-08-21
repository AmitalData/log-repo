using Logitude.Server.Tools.TreeFilterQuery.Iterator;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Interpreter
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
            filterItem.FieldValue = GetCustomFieldStringValue(filterItem.FieldValue , filterItem.FieldDataType);
            filterItem.FieldValue2 = GetCustomFieldStringValue(filterItem.FieldValue2, filterItem.FieldDataType);

        }

        private object GetCustomFieldStringValue(object fieldValue ,string fieldDataType)
        {
            if (fieldValue == null) return null;
             string result = customFilterClass.SetFieldDataType(fieldDataType, fieldValue);
            if (result != null && result.ToString().ToLower() == "false")
                result = null;
            return result;
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
