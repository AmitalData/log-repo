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
        public DepositCustomerActivityPM GetSingleDepositCustomerActivityPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            depositCustomerActivityQuery = new DepositCustomerActivityQueryService(customContext);
            DepositCustomerActivityPM DepositCustomerActivity = depositCustomerActivityQuery.GetSingle(code, false, false);
            return DepositCustomerActivity;
        }

        public DepositCustomerActivityList GetSingleDepositCustomerActivityList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DepositCustomerActivity", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DepositCustomerActivityListQueryService listService = new DepositCustomerActivityListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<DepositCustomerActivityList> GetDepositCustomerActivityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DepositCustomerActivity", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DepositCustomerActivityListQueryService listService = new DepositCustomerActivityListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DepositCustomerActivityList> GetDepositCustomerActivityFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DepositCustomerActivity", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DepositCustomerActivityListQueryService listService = new DepositCustomerActivityListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetDepositCustomerActivityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DepositCustomerActivity", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            DepositCustomerActivityListQueryService queryService = new DepositCustomerActivityListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}