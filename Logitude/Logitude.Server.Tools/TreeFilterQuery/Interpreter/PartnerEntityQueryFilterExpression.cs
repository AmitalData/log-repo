using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery.Iterator;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Interpreter
{

   
    public class PartnerEntityQueryFilterExpression : IQueryTreeFilterExpression
    {
        private QueryTreeFilterContext queryTreeFilterContext;
        private string partnerEntityName = string.Empty;

        public void Interpret(QueryTreeFilterContext queryTreeFilterContext)
        {
            this.queryTreeFilterContext = queryTreeFilterContext;
            if (queryTreeFilterContext.ParentEntity == null) return;
            QueryTreeFilterIterator queryTreeFilterIterator = CreateIterator();
            if (!queryTreeFilterIterator.Any()) return;
            while (queryTreeFilterIterator.HasNext())
            {
                Handel(queryTreeFilterIterator.Next());
            }
        }

        private void Handel(QueryFilterItem queryFilterItem)
        {

            queryFilterItem.FieldValue = Validation(queryFilterItem);
            queryFilterItem.FieldName = "PartnerEntityField";
            queryFilterItem.Operator = "PartnerEntityExpression";

        }

        private bool Validation(QueryFilterItem queryFilterItem)
        {
            switch (queryFilterItem.FilterType.Replace("Field", ""))
            {
                case "LessThan": return AssertLessThan(queryFilterItem);
                case "LessThanOrEqual": return AssertLessThanOrEqual(queryFilterItem);
                case "GreaterThanOrEqual": return AssertGreaterOrEqual(queryFilterItem);
                case "LargerThan": return AssertLargerThan(queryFilterItem);
                case "Contains": return AssertContains(queryFilterItem);
                case "NotContains": return AssertNotContains(queryFilterItem);
                case "Equal": return AssertEqual(queryFilterItem);
                case "NotEqual": return AssertNotEquals(queryFilterItem);
                case "IsEmpty": return IsEmpty(queryFilterItem);
                case "IsNotEmpty": return IsNotEmpty(queryFilterItem);
                default: throw new InvalidOperationException("Operator not supported.");
            }

        }


        private bool IsEmpty(QueryFilterItem queryFilterItem)
        {
            var value = GetEntityFieldValue(queryFilterItem) ;
            return string.IsNullOrEmpty(value) ? true : false;
        }

        private bool IsNotEmpty(QueryFilterItem queryFilterItem)
        {

            var value = GetEntityFieldValue(queryFilterItem) ;
            return !string.IsNullOrEmpty(value) ? true : false;
        }

        private bool AssertContains(QueryFilterItem queryFilterItem)
        {
            var fieldValue = queryFilterItem.FieldValue;
            var value = GetEntityFieldValue(queryFilterItem) ;
            return ((string)fieldValue).Contains(value) ? true : false;
        }


        private bool AssertNotContains(QueryFilterItem queryFilterItem)
        {
            var fieldValue = queryFilterItem.FieldValue as string;
            var value = GetEntityFieldValue(queryFilterItem) ;
            return !((string)fieldValue).Contains(value) ? true : false;
        }

        private bool AssertEndsWith(QueryFilterItem queryFilterItem)
        {
            var fieldValue = queryFilterItem.FieldValue as string;
            var value = GetEntityFieldValue(queryFilterItem) ;
            return ((string)fieldValue).EndsWith(value) ? true : false;
        }


        private bool AssertStartsWith(QueryFilterItem queryFilterItem)
        {
            var fieldValue = queryFilterItem.FieldValue as string;
            var value = GetEntityFieldValue(queryFilterItem) ;

            return ((string)fieldValue).StartsWith(value) ? true : false;
        }

        private bool AssertLargerThan(QueryFilterItem queryFilterItem)
        {
            var fieldValue = queryFilterItem.FieldValue as string;
            var value = GetEntityFieldValue(queryFilterItem) ;
            IComparable comparable = (IComparable)fieldValue;
            return comparable.CompareTo(value) == 1 ? true : false;
        }

        private bool AssertGreaterOrEqual(QueryFilterItem queryFilterItem)
        {
            var fieldValue = queryFilterItem.FieldValue as string;
            var value = GetEntityFieldValue(queryFilterItem) ;
            IComparable comparable = (IComparable)fieldValue;
            return (comparable.CompareTo(value) == 1 || comparable.CompareTo(value) == 0) ? true : false;
        }

        private bool AssertLessThanOrEqual(QueryFilterItem queryFilterItem)
        {
            var fieldValue = queryFilterItem.FieldValue as string;
            var value = GetEntityFieldValue(queryFilterItem) ;
            IComparable comparable = (IComparable)fieldValue;
            return (comparable.CompareTo(value) == -1 || comparable.CompareTo(value) == 0) ? true : false;
        }

        private bool AssertLessThan(QueryFilterItem queryFilterItem)
        {
            var fieldValue = queryFilterItem.FieldValue as string;
            var value = GetEntityFieldValue(queryFilterItem) ;
            IComparable comparable = (IComparable)fieldValue;
            return comparable.CompareTo(value) == -1 ? true : false;
        }

        private bool AssertNotEquals(QueryFilterItem queryFilterItem)
        {
            var fieldValue = queryFilterItem.FieldValue as string;
            var value = GetEntityFieldValue(queryFilterItem) ;
            return value != fieldValue ? true : false;
        }

        private bool AssertEqual(QueryFilterItem queryFilterItem)
        {
            var fieldValue = queryFilterItem.FieldValue as string ;
            var value = GetEntityFieldValue(queryFilterItem) ;
            var result = value == fieldValue ? true : false;
            return result;
        }




        private QueryTreeFilterIterator CreateIterator()
        {
            var iterator = new QueryTreeFilterCollection(queryTreeFilterContext).CreateIterator();
            var collection = iterator.collection.Where(d => !string.IsNullOrEmpty(d.FieldName) && d.FieldName.ToString().Split('.')[0] == queryTreeFilterContext.ParentObjectTableName).ToList();
            iterator.SetCollection(collection);
            return iterator;
        }



        private string GetFieldName(string field)
        {
            if (string.IsNullOrEmpty(field)) return null;
            var fieldNames = field.ToString().Split('.');
            if (fieldNames.Length == 0) return null;
            if (fieldNames.Length > 2) return fieldNames[2];
            if (fieldNames.Length > 1) return fieldNames[1];
            return fieldNames[0];


        }



       
        private string GetEntityFieldValue(QueryFilterItem queryFilterItem)
        {
            Object value = null;
            ObjectField objectField = new ObjectField() { DataTypeCode = queryFilterItem.FieldDataType , FieldName = queryFilterItem .FieldName};
            PropertyInfo propertyInfo = GetProperty(queryTreeFilterContext.ParentEntity, objectField.FieldName);
            if (propertyInfo == null) return null;
            value = propertyInfo.GetValue(queryTreeFilterContext.ParentEntity, null);
            if(value == null ) return null;
            if (value.GetType() == typeof(CustomFieldClass)) value = (value as CustomFieldClass).Value;
            else value = FieldValueResolver.GetFieldStringValue(objectField, value);
            return value != null ? value.ToString() : null;

        }

        private PropertyInfo GetProperty(object entity, string FieldName)
        {
            Type type = entity.GetType();
            return type.GetProperty(FieldName);
        }
    }
}
