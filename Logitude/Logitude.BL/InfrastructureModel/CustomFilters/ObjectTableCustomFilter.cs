using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.BL.InfrastructureModel.CustomFilters
{
    public class ObjectTableCustomFilter
    {
        private int tenant;
        public ObjectTableCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }



        public IQueryable<ObjectTable> GetFilteredQuery(QueryOperations operations, IQueryable<ObjectTable> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "IsDeploymentPackage")
                    {
                        queryableData = FilterDataByTenant(queryableData);
                    }
                }
            }

            return queryableData;
        }

        private IQueryable<ObjectTable> FilterDataByTenant(IQueryable<ObjectTable> queryableData)
        {
            return queryableData.Where(objectTable => objectTable.AllowCustomFields == true &&
                                                      (objectTable.Tenant == Tenant || objectTable.Tenant == 0) &&
                                                      !(objectTable.IsCustom && !string.IsNullOrEmpty(objectTable.ParentObjectTableId)));
                                                      
        }

    }
}
