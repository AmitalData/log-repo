using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.DataContracts;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.GlobalModel.CustomFilters
{
    public class TenantManagementCustomFilter
    {
        int tenant;
        public TenantManagementCustomFilter(int tenant)
        {
            this.tenant = tenant;
        }

        public IQueryable<TenantManagement> GetFilteredQuery(QueryOperations operations, IQueryable<TenantManagement> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;
            List<TenantManagement> result = new List<TenantManagement>();

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "ActiveTrail")
                    {
                        queryableData = queryableData.Where(c => c.GlobalTenant.IsActive && c.IsTrial);
                    }

                    if (item.FieldName == "PayingCustomers")
                    {
                        queryableData = queryableData.Where(c => c.GlobalTenant.IsActive && (c.IsRecurring || c.PaidUntilDate > DateTime.Now));
                    }

                    if (item.FieldName == "NotRecuringTenants")
                    {
                        queryableData = queryableData.Where(c => !c.IsRecurring);
                    }

                    if (item.FieldName == "PackageCodeSearchField")
                    {
                        string packageCode = item.FieldValue.ToString();
                        queryableData = queryableData.Where(c => c.PackageCodeSearchField.Contains(packageCode));
                    }
                }
            }

            return queryableData;
        }
    }
}
