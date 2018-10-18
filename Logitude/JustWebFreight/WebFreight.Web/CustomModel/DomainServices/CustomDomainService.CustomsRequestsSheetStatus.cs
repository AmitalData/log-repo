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
        public CustomsRequestsSheetStatusPM GetSingleCustomsRequestsSheetStatusPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsRequestsSheetStatusQuery = new CustomsRequestsSheetStatusQueryService(customContext);
            CustomsRequestsSheetStatusPM CustomsRequestsSheetStatus = customsRequestsSheetStatusQuery.GetSingle(code, false, false);
            return CustomsRequestsSheetStatus;
        }

        public CustomsRequestsSheetStatusList GetSingleCustomsRequestsSheetStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.CustomsRequestsSheetStatus", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsRequestsSheetStatusListQueryService listService = new CustomsRequestsSheetStatusListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CustomsRequestsSheetStatusList> GetCustomsRequestsSheetStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsRequestsSheetStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsRequestsSheetStatusListQueryService listService = new CustomsRequestsSheetStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsRequestsSheetStatusList> GetCustomsRequestsSheetStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsRequestsSheetStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsRequestsSheetStatusListQueryService listService = new CustomsRequestsSheetStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsRequestsSheetStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsRequestsSheetStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsRequestsSheetStatusListQueryService queryService = new CustomsRequestsSheetStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}