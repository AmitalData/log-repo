using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class CustomAgentCustomFilter
    {
        private int tenant;
        public CustomAgentCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }

        public IQueryable<CustomAgent> GetFilteredQuery(QueryOperations operations, IQueryable<CustomAgent> queryableData)
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

                    if (item.FieldName == "PaymentTermId")
                    {
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(c => c.Card.PaymentTermId == filterFieldId);
                        }
                    }
                }
            }

            return queryableData;
        }
    }
}