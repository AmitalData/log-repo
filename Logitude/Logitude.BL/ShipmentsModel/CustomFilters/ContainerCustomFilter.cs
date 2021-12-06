using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.CustomFilters
{
    public class ContainerCustomFilter
    {
        public int Tenant { get; set; }
        public ContainerCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<Container> GetFilteredQuery(QueryOperations operations, IQueryable<Container> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;
            bool showIsClosed = false;
            bool showIsCancelled = false;
            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "IsClosed")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            showIsClosed = true;
                        }
                    }

                    if (item.FieldName == "IsCancelled")
                    {
                        bool value = Convert.ToBoolean(item.FieldValue);
                        if (value)
                        {
                            showIsCancelled = true;
                        }
                    }
                }
            }

            if (showIsCancelled)
            {
                queryableData = queryableData.Where(d => d.IsCancelled == showIsCancelled);
            }
            else if (showIsClosed)
            {
                queryableData= queryableData.Where(d => d.IsClosed == showIsClosed && d.IsCancelled == showIsCancelled);
            }

            return queryableData;
        }
    }
}
