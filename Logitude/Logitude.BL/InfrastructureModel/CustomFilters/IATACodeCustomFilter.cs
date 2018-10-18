using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.CustomFilters
{
    public class IATACodeCustomFilter
    {
        private int tenant;
        public IATACodeCustomFilter(int tenant)
        {
            this.tenant = tenant;
        }

        public IQueryable<IATACode> GetFilteredQuery(QueryOperations operations, IQueryable<IATACode> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "AirlineId")
                    {
                        if (item.FieldValue != null)
                        {
                            string myAirlineId = item.FieldValue.ToString();
                            queryableData = queryableData.Where(d => d.AirlineId == null || d.AirlineId == myAirlineId);
                        }                        
                    }
                }
            }

            return queryableData;
        }
    }
}
