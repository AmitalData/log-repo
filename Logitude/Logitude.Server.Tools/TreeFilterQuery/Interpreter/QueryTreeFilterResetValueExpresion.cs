using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.TreeFilterQuery.Iterator;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.TreeFilterQuery.Interpreter
{

    

    public class QueryTreeFilterResetValueExpresion : IQueryTreeFilterExpression
    {
        private List<ObjectField> mainObjectFields;
        private List<ObjectField> partnerObjectFields;
        private QueryTreeFilterContext queryTreeFilterContext;
        private CustomFieldClass customFilterClass = new CustomFieldClass();

        public void Interpret(QueryTreeFilterContext queryTreeFilterContext)
        {
            this.queryTreeFilterContext = queryTreeFilterContext;

            var iterator = new QueryTreeFilterCollection(queryTreeFilterContext).CreateIterator();
            mainObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName(queryTreeFilterContext.ObjectTableName, queryTreeFilterContext.Tenant);
            if (iterator.collection.Where(d => d.FieldName.Split('.')[0] == queryTreeFilterContext.ParentObjectTableName).Any())
            {
                partnerObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName(queryTreeFilterContext.ParentObjectTableName, queryTreeFilterContext.Tenant);
            }

            while (iterator.HasNext())
            {
                Handel(iterator.Next());
            }

        }

        private void Handel(QueryFilterItem queryFilterItem)
        {
            ObjectField objectField = GetObjectField(queryFilterItem);
            if (queryFilterItem.IsAnalyticsMetadatas)
            {
                objectField = new ObjectField
                {
                    DataTypeCode = queryFilterItem.FieldDataType
                };
            };
            if (objectField == null) return;

            queryFilterItem.IsCustomField = objectField.IsCustom;
            queryFilterItem.FieldDataType = objectField.DataTypeCode;
            if (queryFilterItem.IsCustomField || (!string.IsNullOrEmpty(queryFilterItem.Operator) &&  queryFilterItem.Operator.Contains("Field"))) return;
            if (queryFilterItem.FieldValue != null && string.IsNullOrEmpty(queryFilterItem.FieldValue.ToString())) queryFilterItem.FieldValue = null;
            if (queryFilterItem.FieldValue2 != null && string.IsNullOrEmpty(queryFilterItem.FieldValue2.ToString())) queryFilterItem.FieldValue2 = null;

            queryFilterItem.FieldValue = FieldValueResolver.GetFieldDataValue(objectField, GetFieldValue(objectField.DataTypeCode , queryFilterItem.FieldValue));
            queryFilterItem.FieldValue2 = FieldValueResolver.GetFieldDataValue(objectField, GetFieldValue(objectField.DataTypeCode, queryFilterItem.FieldValue2));
 
        }

        public string GetFieldValue(string dataTypeCode , object fieldValue)
        {
            if (fieldValue == null || (dataTypeCode != "DateTime" && dataTypeCode != "Date")) return fieldValue != null ? fieldValue.ToString() : null;
            try
            {
                return FieldValueResolver.ConvertToDate(fieldValue.ToString()).ToString();
            }
            catch (Exception exception)
            {
                return fieldValue != null ? fieldValue.ToString() : null;
            }
        }


        private ObjectField GetObjectField(QueryFilterItem queryFilterItem)
        {
            string tableName = queryFilterItem.FieldName.Split('.')[0] == queryTreeFilterContext.ParentObjectTableName ? queryTreeFilterContext.ParentObjectTableName : queryTreeFilterContext.ObjectTableName;
            string fieldName = GetFieldName(queryFilterItem);
            if(tableName == queryTreeFilterContext.ParentObjectTableName)
            {
                return partnerObjectFields?.FirstOrDefault(f => f.FieldName == fieldName);
            }

            return mainObjectFields?.FirstOrDefault(f => f.FieldName == fieldName);
        }

        private string GetFieldName(QueryFilterItem queryFilterItem)
        {
            if (string.IsNullOrEmpty(queryFilterItem.FieldName)) return "";
            var fieldNames = queryFilterItem.FieldName.Split('.');
            if (fieldNames.Length == 0) return null;
            return fieldNames[fieldNames.Length - 1];

        }

    }
}
