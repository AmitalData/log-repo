using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;

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
                    else if (item.FieldName == "AvailableInDocumentTypes")
                    {
                        queryableData = FilterDataByAvailableInDocumentTypes(queryableData);
                    }
                }
            }

            return queryableData;
        }

        private IQueryable<ObjectTable> FilterDataByTenant(IQueryable<ObjectTable> queryableData)
        {
            return queryableData.Where(objectTable => objectTable.AllowCustomFields == true &&
                                                      (objectTable.Tenant == Tenant || objectTable.Tenant == 0) &&
                                                      !objectTable.IsCustom);
                                                      
        }

        private IQueryable<ObjectTable> FilterDataByAvailableInDocumentTypes(IQueryable<ObjectTable> queryableData)
        {
            return queryableData.Where(objectTable => objectTable.AvailableInDocumentTypes == true && 
                                                     (objectTable.Tenant == Tenant || objectTable.Tenant == 0));
        }

    }
}
