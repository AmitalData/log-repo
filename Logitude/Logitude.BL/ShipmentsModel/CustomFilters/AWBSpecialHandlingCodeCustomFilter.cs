using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.CustomFilters
{
    public class AWBSpecialHandlingCodeCustomFilter
    {
        private int tenant;
        public AWBSpecialHandlingCodeCustomFilter(int tenant)
        {
            this.tenant = tenant;
        }

        public IQueryable<AWBSpecialHandlingCode> GetFilteredQuery(QueryOperations operations, IQueryable<AWBSpecialHandlingCode> queryableData)
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
                            queryableData = queryableData.Where(d => d.AirlineId == myAirlineId);
                        }                        
                    }
                }
            }

            return queryableData;
        }
    }
}
