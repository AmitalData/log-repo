using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class WarehouseCustomFilter
    {
        public WarehouseCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        private int tenant;
        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<Warehouse> GetFilteredQuery(QueryOperations operations, IQueryable<Warehouse> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "CountryId")
                    {
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(c => c.Card.CountryId == filterFieldId);
                        }
                    }
                }
            }

            return queryableData;
        }
    }
}
