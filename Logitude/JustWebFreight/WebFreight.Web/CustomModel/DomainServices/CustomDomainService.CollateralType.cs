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
        public CollateralTypePM GetSingleCollateralTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            collateralTypeQuery = new CollateralTypeQueryService(customContext);
            CollateralTypePM CollateralType = collateralTypeQuery.GetSingle(code, false, false);
            return CollateralType;
        }

        public CollateralTypeList GetSingleCollateralTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.CollateralType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CollateralTypeListQueryService listService = new CollateralTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CollateralTypeList> GetCollateralTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.CollateralType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CollateralTypeListQueryService listService = new CollateralTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CollateralTypeList> GetCollateralTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.CollateralType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CollateralTypeListQueryService listService = new CollateralTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCollateralTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.CollateralType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CollateralTypeListQueryService queryService = new CollateralTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }




    }
}