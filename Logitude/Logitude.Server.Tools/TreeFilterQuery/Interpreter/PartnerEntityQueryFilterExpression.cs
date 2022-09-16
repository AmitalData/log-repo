using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery.Iterator;
using Logitude.Server.Tools.TreeFilterQuery.Services;
using Newtonsoft.Json;
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

   
    public class PartnerEntityQueryFilterExpression : IQueryTreeFilterExpression
    {
        private QueryTreeFilterContext queryTreeFilterContext;
        private string partnerEntityName = string.Empty;
        private CustomFieldClass customFilterClass = new CustomFieldClass();
        public void Interpret(QueryTreeFilterContext queryTreeFilterContext)
        {
            queryTreeFilterContext.IsInterpreterFinished = true;

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
            catch (Exception exception)
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
            catch (Exception exception)
            {
                return queryTreeFilterContext.ParentEntity;
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
            switch (queryFilterItem.Operator.Replace("Field", ""))
            {
                case "LessThan": return AssertLessThan(queryFilterItem);
                case "LessThanOrEqual": return AssertLessThanOrEqual(queryFilterItem);
                case "GreaterThanOrEqual": return AssertGreaterOrEqual(queryFilterItem);
                case "LargerThan": 
                case "GreaterThan": return AssertLargerThan(queryFilterItem);
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
            var entityFieldValue = GetEntityFieldValue(queryFilterItem) ;
            return string.IsNullOrEmpty(entityFieldValue) ? true : false;
        }

        private bool IsNotEmpty(QueryFilterItem queryFilterItem)
        {

            var entityFieldValue = GetEntityFieldValue(queryFilterItem) ;
            return !string.IsNullOrEmpty(entityFieldValue) ? true : false;
        }

        private bool AssertContains(QueryFilterItem queryFilterItem)
        {
            var queryFilterValue = GetQueryFilterItemFieldValue(queryFilterItem);
            var entityFieldValue = GetEntityFieldValue(queryFilterItem);
            return ((string)entityFieldValue).Contains(queryFilterValue) ? true : false;
        }


        private bool AssertNotContains(QueryFilterItem queryFilterItem)
        {
            var queryFilterValue = GetQueryFilterItemFieldValue(queryFilterItem);
            var entityFieldValue = GetEntityFieldValue(queryFilterItem);
            return !((string)entityFieldValue).Contains(queryFilterValue) ? true : false;
        }

        private bool AssertEndsWith(QueryFilterItem queryFilterItem)
        {
            var queryFilterValue = GetQueryFilterItemFieldValue(queryFilterItem);
            var entityFieldValue = GetEntityFieldValue(queryFilterItem);
            return ((string)entityFieldValue).EndsWith(queryFilterValue) ? true : false;
        }


        private bool AssertStartsWith(QueryFilterItem queryFilterItem)
        {
            var queryFilterValue = GetQueryFilterItemFieldValue(queryFilterItem);
            var entityFieldValue = GetEntityFieldValue(queryFilterItem);

            return ((string)entityFieldValue).StartsWith(queryFilterValue) ? true : false;
        }

        private bool AssertLargerThan(QueryFilterItem queryFilterItem)
        {
            var queryFilterValue = GetQueryFilterItemFieldValue(queryFilterItem);
            var entityFieldValue = GetEntityFieldValue(queryFilterItem) ;
            return entityFieldValue.CompareTo(queryFilterValue) <= 0 ? false : true;
        }

        private bool AssertGreaterOrEqual(QueryFilterItem queryFilterItem)
        {
            var queryFilterValue = GetQueryFilterItemFieldValue(queryFilterItem);
            var entityFieldValue = GetEntityFieldValue(queryFilterItem);
            return entityFieldValue.CompareTo(queryFilterValue) == -1 ? false : true; 
        }

        private bool AssertLessThanOrEqual(QueryFilterItem queryFilterItem)
        {
            var queryFilterValue = GetQueryFilterItemFieldValue(queryFilterItem);
            var entityFieldValue = GetEntityFieldValue(queryFilterItem);
            return entityFieldValue.CompareTo(queryFilterValue) ==1 ? false : true; 
        }

        private bool AssertLessThan(QueryFilterItem queryFilterItem)
        {
            var queryFilterValue = GetQueryFilterItemFieldValue(queryFilterItem);
            var entityFieldValue = GetEntityFieldValue(queryFilterItem);
            return entityFieldValue.CompareTo(queryFilterValue) >=0 ? false : true; 
        }

        private bool AssertNotEquals(QueryFilterItem queryFilterItem)
        {
            var queryFilterValue = GetQueryFilterItemFieldValue(queryFilterItem);
            var entityFieldValue = GetEntityFieldValue(queryFilterItem);
            return entityFieldValue != queryFilterValue ? true : false;
        }

        private bool AssertEqual(QueryFilterItem queryFilterItem)
        {
            var queryFilterValue = GetQueryFilterItemFieldValue(queryFilterItem);
            var entityFieldValue = GetEntityFieldValue(queryFilterItem);
            return entityFieldValue == queryFilterValue ? true : false;

        }

        private  string GetQueryFilterItemFieldValue(QueryFilterItem queryFilterItem)
        {
            return ConvertFieldValueToString(queryFilterItem.FieldDataType , queryFilterItem.FieldValue , queryFilterItem.IsCustomField);
        }

        private string ConvertFieldValueToString(string dataTypeCode , object fieldValue , bool isCustomField)
        {
            if (fieldValue == null) return "";
            if (isCustomField) return  fieldValue != null ? fieldValue.ToString() : "";
            string result  = FieldValueResolver.GetFieldStringValue(new ObjectField() { DataTypeCode = dataTypeCode }, fieldValue);
            if ((dataTypeCode == "DateTime" || dataTypeCode == "Date") && result != null && !string.IsNullOrEmpty(result.ToString()) && result.ToString().Length >= 9)
            {
                return result.ToString().Remove(8);
            }
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
            return fieldNames[fieldNames.Length-1];


        }




        private string GetEntityFieldValue(QueryFilterItem queryFilterItem)
        {
            if (queryTreeFilterContext.ParentEntity == null) return "";
            
            object value = QueryTreeFilterFieldValueResolver.Get(queryTreeFilterContext.ParentEntity, GetFieldName(queryFilterItem.FieldName), queryFilterItem.FieldDataType);

            return ConvertFieldValueToString(queryFilterItem.FieldDataType, value, queryFilterItem.IsCustomField);
        }
    }
}
