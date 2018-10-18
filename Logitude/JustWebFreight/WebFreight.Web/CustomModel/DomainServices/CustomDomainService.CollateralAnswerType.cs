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
        public CollateralAnswerTypePM GetSingleCollateralAnswerTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            collateralAnswerTypeQuery = new CollateralAnswerTypeQueryService(customContext);
            CollateralAnswerTypePM CollateralAnswerType = collateralAnswerTypeQuery.GetSingle(code, false, false);
            return CollateralAnswerType;
        }

        public CollateralAnswerTypeList GetSingleCollateralAnswerTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CollateralAnswerType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CollateralAnswerTypeListQueryService listService = new CollateralAnswerTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CollateralAnswerTypeList> GetCollateralAnswerTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CollateralAnswerType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CollateralAnswerTypeListQueryService listService = new CollateralAnswerTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CollateralAnswerTypeList> GetCollateralAnswerTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CollateralAnswerType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CollateralAnswerTypeListQueryService listService = new CollateralAnswerTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCollateralAnswerTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CollateralAnswerType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CollateralAnswerTypeListQueryService queryService = new CollateralAnswerTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }




    }
}