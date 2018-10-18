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
        public CollateralAnswerStatusPM GetSingleCollateralAnswerStatusPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            collateralAnswerStatusQuery = new CollateralAnswerStatusQueryService(customContext);
            CollateralAnswerStatusPM CollateralAnswerStatus = collateralAnswerStatusQuery.GetSingle(code, false, false);
            return CollateralAnswerStatus;
        }

        public CollateralAnswerStatusList GetSingleCollateralAnswerStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CollateralAnswerStatus", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CollateralAnswerStatusListQueryService listService = new CollateralAnswerStatusListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CollateralAnswerStatusList> GetCollateralAnswerStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CollateralAnswerStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CollateralAnswerStatusListQueryService listService = new CollateralAnswerStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CollateralAnswerStatusList> GetCollateralAnswerStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CollateralAnswerStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CollateralAnswerStatusListQueryService listService = new CollateralAnswerStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCollateralAnswerStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CollateralAnswerStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CollateralAnswerStatusListQueryService queryService = new CollateralAnswerStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }




    }
}