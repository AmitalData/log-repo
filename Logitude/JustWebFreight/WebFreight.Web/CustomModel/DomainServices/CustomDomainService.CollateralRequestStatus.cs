using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public CollateralRequestStatusPM GetSingleCollateralRequestStatusPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            collateralRequestStatusQuery = new CollateralRequestStatusQueryService(customContext);
            CollateralRequestStatusPM CollateralRequestStatus = collateralRequestStatusQuery.GetSingle(code, false, false);
            return CollateralRequestStatus;
        }

        public CollateralRequestStatusList GetSingleCollateralRequestStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CollateralRequestStatus", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CollateralRequestStatusListQueryService listService = new CollateralRequestStatusListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CollateralRequestStatusList> GetCollateralRequestStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CollateralRequestStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CollateralRequestStatusListQueryService listService = new CollateralRequestStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CollateralRequestStatusList> GetCollateralRequestStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CollateralRequestStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CollateralRequestStatusListQueryService listService = new CollateralRequestStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCollateralRequestStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CollateralRequestStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CollateralRequestStatusListQueryService queryService = new CollateralRequestStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }




    }
}