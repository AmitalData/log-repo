using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class ReportExecutionLogCustomFilter
    {
        private int tenant;
        public ReportExecutionLogCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<ReportExecutionLog> GetFilteredQuery(QueryOperations operations, IQueryable<ReportExecutionLog> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "StatusCode")
                    {
                        string filterField = item.FieldValue as string;
                        if (!string.IsNullOrEmpty(filterField))
                        {
                            if (filterField == "D")
                            {
                                queryableData = queryableData.Where(d => d.StatusCode == "D");
                            }

                        }
                    }
                }
            }

            return queryableData;
        }
    }
}
