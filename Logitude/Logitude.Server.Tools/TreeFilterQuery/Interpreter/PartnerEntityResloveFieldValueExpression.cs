using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery.Iterator;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Interpreter
{
    public class PartnerEntityResloveFieldValueExpression : IQueryTreeFilterExpression
    {


        private QueryTreeFilterContext queryTreeFilterContext;
        public void Interpret(QueryTreeFilterContext queryTreeFilterContext)
        {
            queryTreeFilterContext.IsFinish = true;
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
            if (queryTreeFilterContext.ParentEntity != null) return queryTreeFilterContext.ParentEntity;
            if (string.IsNullOrEmpty(queryTreeFilterContext.ParentObjectTableName) || string.IsNullOrEmpty(queryTreeFilterContext.ParentEntityId)) return null;
            return InjectionUtil.Instance.GetEntityByObjectTableNameAndEntityId(queryTreeFilterContext.ParentObjectTableName, queryTreeFilterContext.ParentEntityId, queryTreeFilterContext.Tenant);
        }

        private void Handel(QueryFilterItem queryFilterItem)
        {
            var fieldName = GetFieldName(queryFilterItem);
            PropertyInfo propertyInfo = queryTreeFilterContext.ParentEntity.GetType().GetProperty(fieldName);
            if (propertyInfo == null) return;
            var fieldValue = propertyInfo.GetValue(queryTreeFilterContext.ParentEntity) ?? "";
            if (fieldValue == null) return;
            if (fieldValue.GetType() == typeof(CustomFieldClass)) fieldValue = (fieldValue as CustomFieldClass).Value;
            else fieldValue = FieldValueResolver.GetFieldStringValue(new ObjectField() { DataTypeCode = queryFilterItem.FieldDataType, FieldName = queryFilterItem.FieldName }, fieldValue);
            queryFilterItem.FieldValue = fieldValue;

        }


        private string GetFieldName(QueryFilterItem queryFilterItem)
        {
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
