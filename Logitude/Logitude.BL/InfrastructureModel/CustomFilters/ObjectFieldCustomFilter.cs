
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
                            string myAirlineId = item.FieldValue.ToString();
                            queryableData = queryableData.Where(d => d.AllowedinAutomationConditions == true || d.IsCustom == true);
                        }
                    }

                }
            }

            return queryableData;
        }
    }
}
