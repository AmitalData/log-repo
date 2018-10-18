using Logitude.Infrastructure.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Infrastructure.Data.CustomFilters
{
    public class TeamCustomFilter
    {
        public static IQueryable<Team> GetFilteredQuery(QueryOperations operations, IQueryable<Team> queryableData, int tenant)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "MemberTeamId")
                    {
                        string value = Convert.ToString(item.FieldValue);
                        queryableData = queryableData.Where(d => d.Id != value );
                    }
                }
            }
            return queryableData;
        }
    }
}
