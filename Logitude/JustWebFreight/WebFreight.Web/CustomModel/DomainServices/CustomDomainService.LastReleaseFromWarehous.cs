using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public LastReleaseFromWarehousePM GetSingleLastReleaseFromWarehousePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            lastReleaseFromWarehouseQuery = new LastReleaseFromWarehouseQueryService(customContext);
            LastReleaseFromWarehousePM LastReleaseFromWarehouse = lastReleaseFromWarehouseQuery.GetSingle(code, false, false);
            return LastReleaseFromWarehouse;
        }

        public LastReleaseFromWarehouseList GetSingleLastReleaseFromWarehouseList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.LastReleaseFromWarehouse", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            LastReleaseFromWarehouseListQueryService listService = new LastReleaseFromWarehouseListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<LastReleaseFromWarehouseList> GetLastReleaseFromWarehouseLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.LastReleaseFromWarehouse", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            LastReleaseFromWarehouseListQueryService listService = new LastReleaseFromWarehouseListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<LastReleaseFromWarehouseList> GetLastReleaseFromWarehouseFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.LastReleaseFromWarehouse", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            LastReleaseFromWarehouseListQueryService listService = new LastReleaseFromWarehouseListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetLastReleaseFromWarehouseFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.LastReleaseFromWarehouse", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            LastReleaseFromWarehouseListQueryService queryService = new LastReleaseFromWarehouseListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}