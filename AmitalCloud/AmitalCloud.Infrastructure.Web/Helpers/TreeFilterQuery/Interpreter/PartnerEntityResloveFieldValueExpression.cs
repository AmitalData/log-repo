using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.Helpers;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace AmitalCloud.Infrastructure.Web.Helpers.TreeFilterQuery
{
    public class PartnerEntityResloveFieldValueExpression : IQueryTreeFilterExpression
    {
        private QueryTreeFilterContext queryTreeFilterContext;
        public void Interpret(QueryTreeFilterContext queryTreeFilterContext)
        {
            this.queryTreeFilterContext = queryTreeFilterContext;
            QueryTreeFilterIterator queryTreeFilterIterator = CreateIterator();
            if (!queryTreeFilterIterator.Any()) return;

            queryTreeFilterContext.ParentEntity = GetParentEntity();
            if (queryTreeFilterContext.ParentEntity == null) return;

            while (queryTreeFilterIterator.HasNext())
            {
                Handel(queryTreeFilterIterator.Next());
            }
        }

        private object GetParentEntity()
        {
            object parentEntity = GetContextParentEntity();
            if (parentEntity != null) return parentEntity;
            if (string.IsNullOrEmpty(queryTreeFilterContext.ParentObjectTableName) || string.IsNullOrEmpty(queryTreeFilterContext.ParentEntityId)) return null;
            try
            {
                return InjectionUtil.Instance.GetEntityByObjectTableNameAndEntityId(queryTreeFilterContext.ParentObjectTableName, queryTreeFilterContext.ParentEntityId, queryTreeFilterContext.Tenant);
            }
            catch
            {
                return null;
            }
        }

        private object GetContextParentEntity()
        {
            try
            {
                if (queryTreeFilterContext.ParentEntity != null) return JsonConvert.DeserializeObject(queryTreeFilterContext.ParentEntity.ToString());
                return null;
            }
            catch
            {
                return queryTreeFilterContext.ParentEntity;
            }
        }

        private void Handel(QueryFilterItem queryFilterItem)
        {
            var fieldName = GetFieldName(queryFilterItem);

            queryFilterItem.FieldValue = QueryTreeFilterFieldValueResolver.Get(queryTreeFilterContext.ParentEntity, fieldName, queryFilterItem.FieldDataType);
            queryFilterItem.Operator = queryFilterItem.Operator?.Replace("Field","");
        }

        private string GetFieldName(QueryFilterItem queryFilterItem)
        {
            if (queryFilterItem.FieldValue == null) return "";
            var values = queryFilterItem.FieldValue.ToString().Split('.');
            if (values.Length == 0) return null;
            return values[values.Length - 1];
        }

        private QueryTreeFilterIterator CreateIterator()
        {
            var iterator = new QueryTreeFilterCollection(queryTreeFilterContext).CreateIterator();
            var collection = iterator.collection.Where(d => d.Operator.Contains("Field") && d.FieldValue != null && d.FieldValue.ToString().Split('.')[0] == queryTreeFilterContext.ParentObjectTableName).ToList();
            iterator.SetCollection(collection);
            return iterator;
        }
    }
}
