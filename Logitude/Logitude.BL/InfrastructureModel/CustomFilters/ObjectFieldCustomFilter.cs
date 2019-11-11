
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.CustomFilters
{
    public class ObjectFieldCustomFilter
    {
        private int tenant;
        public ObjectFieldCustomFilter(int tenant)
        {
            this.tenant = tenant;
        }

        public IQueryable<ObjectField> GetFilteredQuery(QueryOperations operations, IQueryable<ObjectField> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "AllowedinAutomationConditions")
                    {
                        if (item.FieldValue != null)
                        {
                            queryableData = queryableData.Where(d => d.AllowedinAutomationConditions == true || d.IsCustom == true);
                        }
                    }
                    if (item.FieldName == "AllowedinAutomationConditionsEntitiesFields")
                    {
                        queryableData = GetAllowedinAutomationConditionsEntitiesFields(queryableData, item);
                    }

                }
            }

            return queryableData;
        }

        private static IQueryable<ObjectField> GetAllowedinAutomationConditionsEntitiesFields(IQueryable<ObjectField> queryableData, QueryFilterItem item)
        {
            IQueryable<ObjectField> objectFieldLists = queryableData;
            if (item.FieldValue != null)
            {
                string fieldValue = item.FieldValue.ToString();
                var EntityTableIdLists = fieldValue.Split(',');
                objectFieldLists = queryableData.Where(d => (d.AllowedinAutomationConditions == true || d.IsCustom == true) && EntityTableIdLists.Contains(d.ObjectTableId));
            }

            return objectFieldLists;
        }
    }
}
