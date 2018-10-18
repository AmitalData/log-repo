using Logitude.WarehouseLib.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.WarehouseLib.Data.CustomFilters
{
    public class WarehouseReleaseCustomFilter
    {
        public static IQueryable<WarehouseRelease> GetFilteredQuery(QueryOperations operations, IQueryable<WarehouseRelease> queryableData, int tenant)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    switch (item.FieldName)
                    {
                        case "CreatedReleases":
                            {
                                queryableData = queryableData.Where(d => d.StatusCode == "CREA");
                                break;
                            }

                        case "ReleasedReleases":
                            {
                                queryableData = queryableData.Where(d => d.StatusCode == "RELE");
                                break;
                            }

                        case "CanncelledReleases":
                            {
                                queryableData = queryableData.Where(d => d.StatusCode == "CARE");
                                break;
                            }
                    }
                }
            }

            return queryableData;
        }
    }
}
