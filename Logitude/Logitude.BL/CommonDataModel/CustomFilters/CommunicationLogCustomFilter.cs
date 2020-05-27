using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class CommunicationLogCustomFilter
    {
        private int tenant;
        public CommunicationLogCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<CommunicationLog> GetFilteredQuery(QueryOperations operations, IQueryable<CommunicationLog> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "FromTo")
                    {
                        string filterField = item.FieldValue as string;
                        if (!string.IsNullOrEmpty(filterField))
                        {
                            if (filterField == "true")
                            {
                                queryableData = queryableData.Where(d => d.To == "INTTRA" || d.From == "INTTRA");
                            }

                        }
                    }
                }
            }

            return queryableData;
        }
    }
}

