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

        private CustomFieldClass customFilterClass = new CustomFieldClass();

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
            if (queryTreeFilterContext.ParentEntity != null) return queryTreeFilterContext.ParentEntity;
            if (string.IsNullOrEmpty(queryTreeFilterContext.ParentObjectTableName) || string.IsNullOrEmpty(queryTreeFilterContext.ParentEntityId)) return null;
            try
            {
                return InjectionUtil.Instance.GetEntityByObjectTableNameAndEntityId(queryTreeFilterContext.ParentObjectTableName, queryTreeFilterContext.ParentEntityId, queryTreeFilterContext.Tenant);
            }
            catch(Exception exception)
            {
                return null;
            }

        }

        private void Handel(QueryFilterItem queryFilterItem)
        {
            var fieldName = GetFieldName(queryFilterItem);
            PropertyInfo propertyInfo = queryTreeFilterContext.ParentEntity.GetType().GetProperty(fieldName);
            if (propertyInfo == null) return;
            var fieldValue = propertyInfo.GetValue(queryTreeFilterContext.ParentEntity) ?? "";
            if (fieldValue == null) return;
            if (fieldValue.GetType() == typeof(CustomFieldClass))
            {
                fieldValue = customFilterClass.SetFieldDataType(queryFilterItem.FieldDataType, (fieldValue as CustomFieldClass).Value);
            }
            else queryFilterItem.FieldValue = fieldValue;
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
