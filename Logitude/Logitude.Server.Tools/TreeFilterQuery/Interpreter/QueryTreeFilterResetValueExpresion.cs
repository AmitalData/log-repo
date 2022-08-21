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
            var objectField = GetObjectField(queryFilterItem);
            if (objectField == null) return;
            queryFilterItem.FieldValue = FieldValueResolver.GetFieldDataValue(objectField, (queryFilterItem.FieldValue != null ? queryFilterItem.FieldValue.ToString() : null));
            queryFilterItem.FieldValue2 = FieldValueResolver.GetFieldDataValue(objectField, (queryFilterItem.FieldValue2 != null ? queryFilterItem.FieldValue2.ToString() : null));
            queryFilterItem.IsCustomField = objectField.IsCustom;
            queryFilterItem.FieldDataType = objectField.DataTypeCode;
        }




        private ObjectField GetObjectField(QueryFilterItem queryFilterItem)
        {
            string tableName = queryFilterItem.FieldName.Split('.')[0] == queryTreeFilterContext.ParentObjectTableName ? queryTreeFilterContext.ParentObjectTableName : queryTreeFilterContext.ParentObjectTableName;
            if(tableName == queryTreeFilterContext.ParentObjectTableName)
            {
                return partnerObjectFields?.FirstOrDefault(f => f.FieldName == queryFilterItem.FieldName.Split('.')[0]);
            }

            return mainObjectFields?.FirstOrDefault(f => f.FieldName == queryFilterItem.FieldName.Split('.')[0]);
        }

  

    }
}
