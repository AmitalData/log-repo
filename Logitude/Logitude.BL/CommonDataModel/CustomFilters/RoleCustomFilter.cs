using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class RoleCustomFilter
    {
        private int tenant;
        public RoleCustomFilter(int tenant)
        {
            this.tenant = tenant;
        }

        public IQueryable<Role> GetFilteredQuery(QueryOperations operations, IQueryable<Role> iQueryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "RoleCodeTenantFilter")
                    {
                        if (item.FieldValue != null)
                        {
                            bool value = Convert.ToBoolean(item.FieldValue);

                            if (value)
                            {                                
                                bool isAddingTenantZeroRoles = false;

                                if (tenant == 0)
                                {
                                    isAddingTenantZeroRoles = true;
                                }

                                if (!isAddingTenantZeroRoles)
                                {
                                    iQueryableData = iQueryableData.Where(d => d.Code != "DIST" && d.Code != "CUCA");
                                }
                            }
                        }
                    }
                }
            }

            return iQueryableData;
        }
    }
}
